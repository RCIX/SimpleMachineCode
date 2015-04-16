using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SimpleMachineCode.Exceptions;
using SimpleMachineCode.Commands;
using SimpleMachineCode.Enums;

namespace SimpleMachineCode.Assembler
{
    public static class Assembler
    {
        private const string CommentIdentifier = "//";
        private const string LabelIdentifier = ":";

        /// <summary>
        /// Takes in a string that conforms to valid SMC assembly and converts it to a stream of bytes.
        /// </summary>
        /// <param name="program">The string that contains the program.</param>
        /// <returns>a byte array that represents the program.</returns>
        public static byte[] Compile(string program)
        {
            string[] splitProgram = program.Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.None);
            short i = 0;
            Dictionary<short, string> programDictionary = splitProgram.ToDictionary((value) => i++);
            return ConvertToBytes(Preprocess(programDictionary));
        }

        /// <summary>
        /// Converts a a byte array into a data format usable in a virtual processor.
        /// </summary>
        /// <param name="program">the program as represented by an array of bytes.</param>
        /// <returns>a command set that a virtual processor can use.</returns>
        public static Dictionary<short, Command> BytesToProgram(byte[] program)
        {
            if (program.Length % 4 != 0)
                throw new ArgumentException("The byte array's length in bytes must be a multiple of 4.");
            Dictionary<short, Command> commands = new Dictionary<short, Command>((int)program.Length / 4);
            for (int i = 0; i < program.Length; i += 4)
            {
                Command command = new Command();
                command.Opcode = program[i];
                command.Data1 = program[i + 1];
                command.Data2 = program[i + 2];
                command.Data3 = program[i + 3];
                commands.Add((short)(i / 4), command);
            }
            return commands;
        }

        private static Dictionary<short, string> Preprocess(Dictionary<short, string> commandSet)
        {
            Dictionary<short, string> newCommandSet = new Dictionary<short, string>(commandSet.Count);
            short index = 0;
            foreach (KeyValuePair<short, string> kvp in commandSet)
            {
                string command = kvp.Value;
                if (kvp.Value.Contains(CommentIdentifier))
                {
                    command = command.Remove(command.IndexOf(CommentIdentifier));
                }
                if (!string.IsNullOrEmpty(command.Trim()))
                {
                    newCommandSet.Add(index, command);
                    index++;
                }
            }
            return newCommandSet;
        }

        private static byte[] ConvertToBytes(Dictionary<short, string> commandSet)
        {
            //label snagging pass
            Dictionary<string, short> jumpMap;
            Dictionary<short, string> newCommandSet = CreateJumpMap(commandSet, out jumpMap);

            short commandCounter = 0;
            byte[] programAsBytes = new byte[newCommandSet.Count * 4];
            foreach (KeyValuePair<short, string> kvp in newCommandSet)
            {
                string line = kvp.Value;
                //this extracts the command out of the line by splitting on spaces and 
                //throwing away everything but the first value
                string instruction = line.Split(new char[] { Convert.ToChar(" ") })[0];
                //this extracts the parameters out of the line by splitting on commas 
                //(with or without spaces)
                string[] parameters = line.Split(new string[] { ", ", "," }, StringSplitOptions.RemoveEmptyEntries);
                //if the first item in the parameter array has a sufficiently long 
                //length and the array has items then remove the instruction and space after it
                if (parameters.Length > 0 && parameters[0].Length >= instruction.Length + 1)
                    parameters[0] = parameters[0].Substring(instruction.Length + 1);
                Command lineCommand = new Command();
                switch (instruction)
                {
                    case "load":
                        lineCommand = CommandFactory.CreateLoadCommand(parameters[0], parameters[1]);
                        break;
                    case "input":
                        lineCommand = CommandFactory.CreateInputCommand(parameters[1], parameters[0]);
                        break;
                    case "output":
                        lineCommand = CommandFactory.CreateOutputCommand(parameters[1], parameters[0]);
                        break;
                    case "add":
                        lineCommand = CommandFactory.CreateAddCommand(parameters[0], parameters[1], parameters[2]);
                        break;
                    case "subtract":
                        lineCommand = CommandFactory.CreateSubtractCommand(parameters[0], parameters[1], parameters[2]);
                        break;
                    case "multiply":
                        lineCommand = CommandFactory.CreateMultiplyCommand(parameters[0], parameters[1], parameters[2]);
                        break;
                    case "divide":
                        lineCommand = CommandFactory.CreateDivideCommand(parameters[0], parameters[1], parameters[2]);
                        break;
                    case "modulus":
                        lineCommand = CommandFactory.CreateModulusCommand(parameters[0], parameters[1], parameters[2]);
                        break;
                    case "leftshift":
                        lineCommand = CommandFactory.CreateLeftShiftCommand(parameters[0], parameters[1], parameters[2]);
                        break;
                    case "rightshift":
                        lineCommand = CommandFactory.CreateRightShiftCommand(parameters[0], parameters[1], parameters[2]);
                        break;
                    case "compare":
                        lineCommand = CommandFactory.CreateComparisonCommand(parameters[0], parameters[1]);
                        break;
                    case "jump":
                        if (!jumpMap.ContainsKey(parameters[1]))
                            throw new InvalidJumpLabelException("jump label '" + parameters[1] + "' is invalid.");
                        lineCommand = CommandFactory.CreateJumpCommand(parameters[0], jumpMap[parameters[1]].ToString());
                        break;
                    case "logicaland":
                        lineCommand = CommandFactory.CreateLogicalAndCommand(parameters[0], parameters[1], parameters[2]);
                        break;
                    case "logicalor":
                        lineCommand = CommandFactory.CreateLogicalOrCommand(parameters[0], parameters[1], parameters[2]);
                        break;
                    case "logicalnot":
                        lineCommand = CommandFactory.CreateLogicalNotCommand(parameters[0], parameters[1]);
                        break;
                    case "logicalxor":
                        lineCommand = CommandFactory.CreateLogicalXorCommand(parameters[0], parameters[1], parameters[2]);
                        break;
                    case "halt":
                        lineCommand = CommandFactory.CreateHaltCommand();
                        break;
                    default:
                        throw new MalformedLineException("invalid instruction '" + line + "'.");
                }
                programAsBytes[commandCounter] = lineCommand.Opcode;
                programAsBytes[commandCounter + 1] = lineCommand.Data1;
                programAsBytes[commandCounter + 2] = lineCommand.Data2;
                programAsBytes[commandCounter + 3] = lineCommand.Data3;
                commandCounter += 4;
            }
            return programAsBytes;
        }
        
        /// <summary>
        /// Takes a command set and converts it to one with labels removed, but outputs a 
        /// "jump map" to translate jump labels specified in jump instructions into line 
        /// numbers
        /// </summary>
        /// <param name="commandSet"></param>
        /// <param name="jumpMap"></param>
        /// <returns></returns>
        private static Dictionary<short,string> CreateJumpMap(Dictionary<short, string> commandSet, out Dictionary<string, short> jumpMap)
        {
            jumpMap = new Dictionary<string, short>();
            Dictionary<short, string> newCommandSet = new Dictionary<short, string>();
            short decrementCount = 0;
            foreach (KeyValuePair<short, string> kvp in commandSet)
            {
                string line = kvp.Value;
                if (line.StartsWith(LabelIdentifier))
                {
                    jumpMap.Add(line.Replace(LabelIdentifier, ""), (short)(kvp.Key - decrementCount));
                    decrementCount++;
                }
                else
                {
                    newCommandSet.Add((short)(kvp.Key - decrementCount), kvp.Value);
                }
            }
            return newCommandSet;
        }
    }
}
