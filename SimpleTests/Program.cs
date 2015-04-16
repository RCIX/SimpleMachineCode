using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SimpleMachineCode.Processor;
using SimpleMachineCode.Commands;
using SimpleMachineCode.Assembler;

namespace SimpleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            //processor construction and setup
            VirtualProcessor processor = new VirtualProcessor();
            processor.OnHalted += () =>
            {
                Console.WriteLine(processor.Registers[2]);
            };
            processor.InputChannels.Add(0, () => { return 2; });
            //end processor construction and setup
            //build program
            Dictionary<short, Command> commands = new Dictionary<short, Command>();
            commands.Add(0, CommandFactory.CreateInputCommand(0, 0));
            commands.Add(1, CommandFactory.CreateLoadCommand(1, 200));
            commands.Add(2, CommandFactory.CreateAddCommand(0, 1, 2));
            commands.Add(3, CommandFactory.CreateHaltCommand());
            //end build program
            //execution
            processor.CurrentProgram = commands;
            while (!processor.Halted)
                processor.ExecuteInstruction();
            //end execution
            Console.ReadLine();
            processor.Reset();
            using (StreamReader sr = new StreamReader(Path.Combine(Environment.CurrentDirectory, "TestProgram1.txt")))
            {
                string program = sr.ReadToEnd();
                byte[] compiledProgram = Assembler.Compile(program);
                processor.CurrentProgram = Assembler.BytesToProgram(compiledProgram);
                processor.OutputChannels[0] = (val) => Console.WriteLine(val);
                while (!processor.Halted)
                    processor.ExecuteInstruction();
            }
            Console.ReadLine();
        }
    }
}
