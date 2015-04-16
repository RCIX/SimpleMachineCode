using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleMachineCode.Enums;
using System.IO;
using SimpleMachineCode.Exceptions;

namespace SimpleMachineCode.Commands
{
    public static class CommandFactory
    {
        /// <summary>
        /// Creates a command that loads the specified int16 into the specified register.
        /// </summary>
        /// <param name="register">the register to load the value into.</param>
        /// <param name="value">the value to assign to that register.</param>
        /// <returns>a Command with the specified parameters.</returns>
        public static Command CreateLoadCommand(byte register, short value)
        {
            Command command = new Command();
            command.Opcode = (byte)CommandOpcodes.Load;
            command.Data1 = register;
            Utils.FromShort(value, out command.Data2, out command.Data3);
            return command;
        }

        /// <summary>
        /// Creates a command that loads the specified int16 into the specified register.
        /// </summary>
        /// <param name="strRegister">A string representation of a byte, used as the register to load the value into.</param>
        /// <param name="strValue">A string representation of a short, used as the value to assign to that register.</param>
        /// <returns>a Command with the specified parameters.</returns>
        public static Command CreateLoadCommand(string strValue, string strRegister)
        {
            short value;
            if (!Int16.TryParse(strValue, out value))
                throw new ArgumentException("string '" + strValue + "', passed as strValue, was invalid.");
            byte register;
            if (!Byte.TryParse(strRegister, out register))
                throw new ArgumentException("string '" + strRegister + "', passed as strRegister, was invalid.");
            return CreateLoadCommand(register, value);
        }

        /// <summary>
        /// Creates a command that queries the specified input channel and places the result in a register.
        /// </summary>
        /// <param name="register">the register from which to load.</param>
        /// <param name="channel">the channel from which to load.</param>
        /// <returns>a Command with the specified parameters.</returns>
        public static Command CreateInputCommand(byte register, byte channel)
        {
            Command command = new Command();
            command.Opcode = (byte)CommandOpcodes.Input;
            command.Data1 = register;
            command.Data2 = channel;
            return command;
        }

        /// <summary>
        /// Creates a command that queries the specified input channel and places the result in a register.
        /// </summary>
        /// <param name="strRegister">A string representation of a byte, used as the register from which to load.</param>
        /// <param name="strChannel">A string representation of a byte, used as the channel from which to load.</param>
        /// <returns>a Command with the specified parameters.</returns>
        public static Command CreateInputCommand(string strRegister, string strChannel)
        {

            byte channel;
            byte register;
            if (!Byte.TryParse(strChannel, out channel))
                throw new ArgumentException("string '" + strRegister + "', passed as strRegister, was invalid.");
            if (!Byte.TryParse(strRegister, out register))
                throw new ArgumentException("string '" + strRegister + "', passed as strRegister, was invalid.");
            return CreateInputCommand(channel, register);
        }

        /// <summary>
        /// Creates a command that outputs the specified register's contents to the specified output channel.
        /// </summary>
        /// <param name="register">the register from which to output.</param>
        /// <param name="channel">the channel to output to.</param>
        /// <returns>a Command with the specified parameters.</returns>
        public static Command CreateOutputCommand(byte register, byte channel)
        {
            Command command = new Command();
            command.Opcode = (byte)CommandOpcodes.Output;
            command.Data1 = register;
            command.Data2 = channel;
            return command;
        }

        /// <summary>
        /// Creates a command that outputs the specified register's contents to the specified output channel.
        /// </summary>
        /// <param name="strRegister">A string representation of a byte, used as the register from which to load.</param>
        /// <param name="strChannel">A string representation of a byte, used as the channel to output to.</param>
        /// <returns></returns>
        public static Command CreateOutputCommand(string strRegister, string strChannel)
        {
            byte channel;
            byte register;
            if (!Byte.TryParse(strChannel, out channel))
                throw new ArgumentException("string '" + strRegister + "', passed as strRegister, was invalid.");
            if (!Byte.TryParse(strRegister, out register))
                throw new ArgumentException("string '" + strRegister + "', passed as strRegister, was invalid.");
            return CreateOutputCommand(channel, register);
        }

        /// <summary>
        /// Creates a command that adds the contents of the two specified registers and places the result in a third register.
        /// </summary>
        /// <param name="register1">the first register to add.</param>
        /// <param name="register2">the second register to add.</param>
        /// <param name="outRegister">the register in which to place the output.</param>
        /// <returns>a Command with the specified parameters.</returns>
        public static Command CreateAddCommand(byte register1, byte register2, byte outRegister)
        {
            Command command = new Command();
            command.Opcode = (byte)CommandOpcodes.Add;
            command.Data1 = outRegister;
            command.Data2 = register1;
            command.Data3 = register2;
            return command;
        }

        /// <summary>
        /// Creates a command that adds the contents of the two specified registers and places the result in a third register.
        /// </summary>
        /// <param name="strRegister1">A string representation of a byte, used as the first register to add.</param>
        /// <param name="strRegister2">A string representation of a byte, used as the second register to add.</param>
        /// <param name="strOutRegister">A string representation of a byte, used as the register in which to place the output.</param>
        /// <returns>a Command with the specified parameters.</returns>
        public static Command CreateAddCommand(string strRegister1, string strRegister2, string strOutRegister)
        {
            byte register1;
            byte register2;
            byte outRegister;
            if (!Byte.TryParse(strRegister1, out register1))
                throw new ArgumentException("string '" + strRegister1 + "', passed as strRegister, was invalid.");
            if (!Byte.TryParse(strRegister2, out register2))
                throw new ArgumentException("string '" + strRegister2 + "', passed as strRegister, was invalid.");
            if (!Byte.TryParse(strOutRegister, out outRegister))
                throw new ArgumentException("string '" + strOutRegister + "', passed as strRegister, was invalid.");
            return CreateAddCommand(register1, register2, outRegister);
        }

        /// <summary>
        /// Creates a command that subtracts the contents of the two specified registers and places the result in a third register.
        /// </summary>
        /// <param name="register1">the first register to subtract.</param>
        /// <param name="register2">the second register to subtract.</param>
        /// <param name="outRegister">the register in which to place the output.</param>
        /// <returns>a Command with the specified parameters.</returns>
        public static Command CreateSubtractCommand(byte register1, byte register2, byte outRegister)
        {
            Command command = new Command();
            command.Opcode = (byte)CommandOpcodes.Subtract;
            command.Data1 = outRegister;
            command.Data2 = register1;
            command.Data3 = register2;
            return command;
        }

        /// <summary>
        /// Creates a command that subtracts the contents of the two specified registers and places the result in a third register.
        /// </summary>
        /// <param name="strRegister1">A string representation of a byte, used as the first register to subtract.</param>
        /// <param name="strRegister2">A string representation of a byte, used as the second register to subtract.</param>
        /// <param name="strOutRegister">A string representation of a byte, used as the register in which to place the output.</param>
        /// <returns>a Command with the specified parameters.</returns>
        public static Command CreateSubtractCommand(string strRegister1, string strRegister2, string strOutRegister)
        {
            byte register1;
            byte register2;
            byte outRegister;
            if (!Byte.TryParse(strRegister1, out register1))
                throw new ArgumentException("string '" + strRegister1 + "', passed as strRegister, was invalid.");
            if (!Byte.TryParse(strRegister2, out register2))
                throw new ArgumentException("string '" + strRegister2 + "', passed as strRegister, was invalid.");
            if (!Byte.TryParse(strOutRegister, out outRegister))
                throw new ArgumentException("string '" + strOutRegister + "', passed as strRegister, was invalid.");
            return CreateSubtractCommand(register1, register2, outRegister);
        }

        /// <summary>
        /// Creates a command that multiplies the contents of the two specified registers and places the result in a third register.
        /// </summary>
        /// <param name="register1">the first register to multiply.</param>
        /// <param name="register2">the second register to multiply.</param>
        /// <param name="outRegister">the register in which to place the output.</param>
        /// <returns>a Command with the specified parameters.</returns>
        public static Command CreateMultiplyCommand(byte register1, byte register2, byte outRegister)
        {
            Command command = new Command();
            command.Opcode = (byte)CommandOpcodes.Multiply;
            command.Data1 = outRegister;
            command.Data2 = register1;
            command.Data3 = register2;
            return command;
        }

        /// <summary>
        /// Creates a command that multiplies the contents of the two specified registers and places the result in a third register.
        /// </summary>
        /// <param name="strRegister1">A string representation of a byte, used as the first register to multiply.</param>
        /// <param name="strRegister2">A string representation of a byte, used as the second register to multiply.</param>
        /// <param name="strOutRegister">A string representation of a byte, used as the register in which to place the output.</param>
        /// <returns>a Command with the specified parameters.</returns>
        public static Command CreateMultiplyCommand(string strRegister1, string strRegister2, string strOutRegister)
        {
            byte register1;
            byte register2;
            byte outRegister;
            if (!Byte.TryParse(strRegister1, out register1))
                throw new ArgumentException("string '" + strRegister1 + "', passed as strRegister, was invalid.");
            if (!Byte.TryParse(strRegister2, out register2))
                throw new ArgumentException("string '" + strRegister2 + "', passed as strRegister, was invalid.");
            if (!Byte.TryParse(strOutRegister, out outRegister))
                throw new ArgumentException("string '" + strOutRegister + "', passed as strRegister, was invalid.");
            return CreateMultiplyCommand(register1, register2, outRegister);
        }

        /// <summary>
        /// Creates a command that divides the contents of the two specified registers and places the result in a third register.
        /// </summary>
        /// <param name="register1">the first register to divide.</param>
        /// <param name="register2">the second register to divide.</param>
        /// <param name="outRegister">the register in which to place the output.</param>
        /// <returns>a Command with the specified parameters.</returns>
        public static Command CreateDivideCommand(byte register1, byte register2, byte outRegister)
        {
            Command command = new Command();
            command.Opcode = (byte)CommandOpcodes.Divide;
            command.Data1 = outRegister;
            command.Data2 = register1;
            command.Data3 = register2;
            return command;
        }

        /// <summary>
        /// Creates a command that divides the contents of the two specified registers and places the result in a third register.
        /// </summary>
        /// <param name="strRegister1">A string representation of a byte, used as the first register to divide.</param>
        /// <param name="strRegister2">A string representation of a byte, used as the second register to divide.</param>
        /// <param name="strOutRegister">A string representation of a byte, used as the register in which to place the output.</param>
        /// <returns>a Command with the specified parameters.</returns>
        public static Command CreateDivideCommand(string strRegister1, string strRegister2, string strOutRegister)
        {
            byte register1;
            byte register2;
            byte outRegister;
            if (!Byte.TryParse(strRegister1, out register1))
                throw new ArgumentException("string '" + strRegister1 + "', passed as strRegister, was invalid.");
            if (!Byte.TryParse(strRegister2, out register2))
                throw new ArgumentException("string '" + strRegister2 + "', passed as strRegister, was invalid.");
            if (!Byte.TryParse(strOutRegister, out outRegister))
                throw new ArgumentException("string '" + strOutRegister + "', passed as strRegister, was invalid.");
            return CreateDivideCommand(register1, register2, outRegister);
        }

        /// <summary>
        /// Creates a command that puts the modulus of the two specified registers in a third register.
        /// </summary>
        /// <param name="register1">the first register to use.</param>
        /// <param name="register2">the second register to use.</param>
        /// <param name="outRegister">the register in which to place the output.</param>
        /// <returns>a Command with the specified parameters.</returns>
        public static Command CreateModulusCommand(byte register1, byte register2, byte outRegister)
        {
            Command command = new Command();
            command.Opcode = (byte)CommandOpcodes.Modulus;
            command.Data1 = outRegister;
            command.Data2 = register1;
            command.Data3 = register2;
            return command;
        }

        /// <summary>
        /// Creates a command that puts the modulus of the two specified registers in a third register.
        /// </summary>
        /// <param name="strRegister1">A string representation of a byte, used as the first register to use.</param>
        /// <param name="strRegister2">A string representation of a byte, used as the second register to use.</param>
        /// <param name="strOutRegister">A string representation of a byte, used as the register in which to place the output.</param>
        /// <returns>a Command with the specified parameters.</returns>
        public static Command CreateModulusCommand(string strRegister1, string strRegister2, string strOutRegister)
        {
            byte register1;
            byte register2;
            byte outRegister;
            if (!Byte.TryParse(strRegister1, out register1))
                throw new ArgumentException("string '" + strRegister1 + "', passed as strRegister, was invalid.");
            if (!Byte.TryParse(strRegister2, out register2))
                throw new ArgumentException("string '" + strRegister2 + "', passed as strRegister, was invalid.");
            if (!Byte.TryParse(strOutRegister, out outRegister))
                throw new ArgumentException("string '" + strOutRegister + "', passed as strRegister, was invalid.");
            return CreateModulusCommand(register1, register2, outRegister);
        }

        /// <summary>
        /// Creates a command that represents a left shift operation.
        /// </summary>
        /// <param name="register">the register to shift.</param>
        /// <param name="count">the register that contains the number of places to shift.</param>
        /// <param name="outRegister">the register in which to place the output.</param>
        /// <returns>a Command with the specified parameters.</returns>
        public static Command CreateLeftShiftCommand(byte register, byte countRegister, byte outRegister)
        {
            Command command = new Command();
            command.Opcode = (byte)CommandOpcodes.LeftShift;
            command.Data1 = outRegister;
            command.Data2 = register;
            command.Data3 = countRegister;
            return command;
        }

        /// <summary>
        /// Creates a command that shifts the specified register left a number of times dictated by the contents of 
        /// the other specified register, then places the result in the specified out register.
        /// </summary>
        /// <param name="strRegister">A string representation of a byte, used as the register to shift.</param>
        /// <param name="strCountRegister">A string representation of a byte, used as the register that contains the number of places to shift.</param>
        /// <param name="strOutRegister">A string representation of a byte, used as the register in which to place the output.</param>
        /// <returns></returns>
        public static Command CreateLeftShiftCommand(string strRegister, string strCountRegister, string strOutRegister)
        {
            byte register = Convert.ToByte(strRegister);
            byte countRegister = Convert.ToByte(strCountRegister);
            byte outRegister = Convert.ToByte(strOutRegister);
            return CreateLeftShiftCommand(register, countRegister, outRegister);
        }

        /// <summary>
        /// Creates a command that shifts the specified register right a number of times dictated by the contents of 
        /// the other specified register, then places the result in the specified out register.
        /// </summary>
        /// <param name="register">the register to shift.</param>
        /// <param name="count">the register that contains the number of places to shift.</param>
        /// <param name="outRegister">the register in which to place the output.</param>
        /// <returns>a Command with the specified parameters.</returns>
        public static Command CreateRightShiftCommand(byte register, byte countRegister, byte outRegister)
        {
            Command command = new Command();
            command.Opcode = (byte)CommandOpcodes.RightShift;
            command.Data1 = outRegister;
            command.Data2 = register;
            command.Data3 = countRegister;
            return command;
        }

        /// <summary>
        /// Creates a command that shifts the specified register right a number of times dictated by the contents of 
        /// the other specified register, then places the result in the specified out register.
        /// </summary>
        /// <param name="strRegister">A string representation of a byte, used as the register to shift.</param>
        /// <param name="strCountRegister">A string representation of a byte, used as the register that contains the number of places to shift.</param>
        /// <param name="strOutRegister">A string representation of a byte, used as the register in which to place the output.</param>
        /// <returns>a Command with the specified parameters.</returns>
        public static Command CreateRightShiftCommand(string strRegister, string strCountRegister, string strOutRegister)
        {
            byte register = Convert.ToByte(strRegister);
            byte countRegister = Convert.ToByte(strCountRegister);
            byte outRegister = Convert.ToByte(strOutRegister);
            return CreateRightShiftCommand(register, countRegister, outRegister);
        }

        /// <summary>
        /// Creates a command that compares the first register to the second and stores the difference in a special register.
        /// </summary>
        /// <param name="register1">the first register to compare.</param>
        /// <param name="register2">the second register to compare.</param>
        /// <returns>a Command with the specified parameters.</returns>
        public static Command CreateComparisonCommand(byte register1, byte register2)
        {
            Command command = new Command();
            command.Opcode = (byte)CommandOpcodes.Compare;
            command.Data1 = register1;
            command.Data2 = register2;
            return command;
        }

        /// <summary>
        /// Creates a command that compares the first register to the second and stores the difference in a special register.
        /// </summary>
        /// <param name="strRegister1">A string representation of a byte, used as the first register to compare.</param>
        /// <param name="strRegister2">A string representation of a byte, used as the second register to compare.</param>
        /// <returns>a Command with the specified parameters.</returns>
        public static Command CreateComparisonCommand(string strRegister1, string strRegister2)
        {
            byte register1;
            byte register2;
            if (!Byte.TryParse(strRegister1, out register1))
                throw new ArgumentException("string '" + strRegister1 + "', passed as strRegister, was invalid.");
            if (!Byte.TryParse(strRegister2, out register2))
                throw new ArgumentException("string '" + strRegister2 + "', passed as strRegister, was invalid.");
            return CreateComparisonCommand(register1, register2);
        }

        /// <summary>
        /// Creates a command that checks if the compare register meets the specified condition relative to zero and jumps to the specified address if it is met.
        /// </summary>
        /// <param name="opcode">the type of jump to execute.</param>
        /// <param name="address">the address to jump to, as an index in the command list.</param>
        /// <returns>a Command with the specified parameters.</returns>
        public static Command CreateJumpCommand(JumpOpcodes opcode, short address)
        {
            Command command = new Command();
            command.Opcode = (byte)CommandOpcodes.Jump;
            command.Data1 = (byte)opcode;
            Utils.FromShort(address, out command.Data2, out command.Data3);
            return command;
        }

        /// <summary>
        /// Creates a command that checks if the compare register meets the specified condition relative to zero and jumps to the specified address if it is met.
        /// </summary>
        /// <param name="strOpcode">A string representation of a byte, used as the type of jump to execute.</param>
        /// <param name="strAddress">A string representation of a byte, used as the address to jump to, as an index in the command list.</param>
        /// <returns>a Command with the specified parameters.</returns>
        public static Command CreateJumpCommand(string strOpcode, string strAddress)
        {
            JumpOpcodes opcode;
            switch (strOpcode)
            {
                case "above":
                    opcode = JumpOpcodes.Above;
                    break;
                case "aboveorequal":
                    opcode = JumpOpcodes.AboveOrEqual;
                    break;
                case "below":
                    opcode = JumpOpcodes.Below;
                    break;
                case "beloworequal":
                    opcode = JumpOpcodes.BelowOrEqual;
                    break;
                case "equal":
                    opcode = JumpOpcodes.Equal;
                    break;
                case "notequal":
                    opcode = JumpOpcodes.NotEqual;
                    break;
                case "unconditional":
                    opcode = JumpOpcodes.Unconditional;
                    break;
                default:
                    throw new ArgumentException("jump opcode string was unrecognized: '" + strOpcode + "'.");
            }
            short address;
            if (!Int16.TryParse(strAddress, out address))
                throw new ArgumentException("string '" + strAddress + "', passed as strValue, was invalid.");
            return CreateJumpCommand(opcode, address);
        }

        /// <summary>
        /// Creates a command that puts the logical AND of the two specified registers in a third register.
        /// </summary>
        /// <param name="register1">the first register to AND.</param>
        /// <param name="register2">the second register to AND.</param>
        /// <param name="outRegister">the register in which to place the output.</param>
        /// <returns>a Command with the specified parameters.</returns>
        public static Command CreateLogicalAndCommand(byte register1, byte register2, byte outRegister)
        {
            Command command = new Command();
            command.Opcode = (byte)CommandOpcodes.LogicalAnd;
            command.Data1 = outRegister;
            command.Data2 = register1;
            command.Data3 = register2;
            return command;
        }

        /// <summary>
        /// Creates a command that puts the logical AND of the two specified registers in a third register.
        /// </summary>
        /// <param name="strRegister1">A string representation of a byte, used as the first register to AND.</param>
        /// <param name="strRegister2">A string representation of a byte, used as the second register to AND.</param>
        /// <param name="strOutRegister">A string representation of a byte, used as the register in which to place the output.</param>
        /// <returns>a Command with the specified parameters.</returns>
        public static Command CreateLogicalAndCommand(string strRegister1, string strRegister2, string strOutRegister)
        {
            byte register1;
            byte register2;
            byte outRegister;
            if (!Byte.TryParse(strRegister1, out register1))
                throw new ArgumentException("string '" + strRegister1 + "', passed as strRegister, was invalid.");
            if (!Byte.TryParse(strRegister2, out register2))
                throw new ArgumentException("string '" + strRegister2 + "', passed as strRegister, was invalid.");
            if (!Byte.TryParse(strOutRegister, out outRegister))
                throw new ArgumentException("string '" + strOutRegister + "', passed as strRegister, was invalid.");
            return CreateLogicalAndCommand(register1, register2, outRegister);
        }

        /// <summary>
        /// Creates a command that represents a logical OR operation.
        /// </summary>
        /// <param name="register1">the first register to OR.</param>
        /// <param name="register2">the second register to OR.</param>
        /// <param name="outRegister">the register in which to place the output.</param>
        /// <returns>a Command with the specified parameters.</returns>
        public static Command CreateLogicalOrCommand(byte register1, byte register2, byte outRegister)
        {
            Command command = new Command();
            command.Opcode = (byte)CommandOpcodes.LogicalOr;
            command.Data1 = outRegister;
            command.Data2 = register1;
            command.Data3 = register2;
            return command;
        }

        /// <summary>
        /// Creates a command that puts the logical OR of the two specified registers in a third register.
        /// </summary>
        /// <param name="strRegister1">A string representation of a byte, used as the first register to OR.</param>
        /// <param name="strRegister2">A string representation of a byte, used as the second register to OR.</param>
        /// <param name="strOutRegister">A string representation of a byte, used as the register in which to place the output.</param>
        /// <returns>a Command with the specified parameters.</returns>
        public static Command CreateLogicalOrCommand(string strRegister1, string strRegister2, string strOutRegister)
        {
            byte register1;
            byte register2;
            byte outRegister;
            if (!Byte.TryParse(strRegister1, out register1))
                throw new ArgumentException("string '" + strRegister1 + "', passed as strRegister, was invalid.");
            if (!Byte.TryParse(strRegister2, out register2))
                throw new ArgumentException("string '" + strRegister2 + "', passed as strRegister, was invalid.");
            if (!Byte.TryParse(strOutRegister, out outRegister))
                throw new ArgumentException("string '" + strOutRegister + "', passed as strRegister, was invalid.");
            return CreateLogicalOrCommand(register1, register2, outRegister);
        }

        /// <summary>
        /// Creates a command that puts the logical NOT of the two specified registers in a third register.
        /// </summary>
        /// <param name="register">the register to NOT.</param>
        /// <param name="outRegister">the register in which to place the output.</param>
        /// <returns>a Command with the specified parameters.</returns>
        public static Command CreateLogicalNotCommand(byte register, byte outRegister)
        {
            Command command = new Command();
            command.Opcode = (byte)CommandOpcodes.LogicalNot;
            command.Data1 = outRegister;
            command.Data2 = register;
            return command;
        }

        /// <summary>
        /// Creates a command that puts the logical NOT of the two specified registers in a third register.
        /// </summary>
        /// <param name="strRegister">A string representation of a byte, used as the register to NOT.</param>
        /// <param name="strOutRegister">A string representation of a byte, used as the register in which to place the output.</param>
        /// <returns></returns>
        public static Command CreateLogicalNotCommand(string strRegister, string strOutRegister)
        {
            byte register;
            byte outRegister;
            if (!Byte.TryParse(strRegister, out register))
                throw new ArgumentException("string '" + strRegister + "', passed as strRegister, was invalid.");
            if (!Byte.TryParse(strOutRegister, out outRegister))
                throw new ArgumentException("string '" + strOutRegister + "', passed as strRegister, was invalid.");
            return CreateLogicalNotCommand(register, outRegister);
        }

        /// <summary>
        /// Creates a command that represents a logical XOR operation.
        /// </summary>
        /// <param name="register1">the first register to XOR.</param>
        /// <param name="register2">the second register to XOR.</param>
        /// <param name="outRegister">the register in which to place the output.</param>
        /// <returns>a Command with the specified parameters.</returns>
        public static Command CreateLogicalXorCommand(byte register1, byte register2, byte outRegister)
        {
            Command command = new Command();
            command.Opcode = (byte)CommandOpcodes.LogicalXor;
            command.Data1 = outRegister;
            command.Data2 = register1;
            command.Data3 = register2;
            return command;
        }

        /// <summary>
        /// Creates a command that puts the logical XOR of the two specified registers in a third register.
        /// </summary>
        /// <param name="strRegister1">A string representation of a byte, used as the first register to XOR.</param>
        /// <param name="strRegister2">A string representation of a byte, used as the second register to XOR.</param>
        /// <param name="strOutRegister">A string representation of a byte, used as the register in which to place the output.</param>
        /// <returns>a Command with the specified parameters.</returns>
        public static Command CreateLogicalXorCommand(string strRegister1, string strRegister2, string strOutRegister)
        {
            byte register1;
            byte register2;
            byte outRegister;
            if (!Byte.TryParse(strRegister1, out register1))
                throw new ArgumentException("string '" + strRegister1 + "', passed as strRegister, was invalid.");
            if (!Byte.TryParse(strRegister2, out register2))
                throw new ArgumentException("string '" + strRegister2 + "', passed as strRegister, was invalid.");
            if (!Byte.TryParse(strOutRegister, out outRegister))
                throw new ArgumentException("string '" + strOutRegister + "', passed as strRegister, was invalid.");
            return CreateLogicalXorCommand(register1, register2, outRegister);
        }

        /// <summary>
        /// Creates a command that represents a halt command.
        /// </summary>
        /// <returns>a Command with the specified parameters.</returns>
        public static Command CreateHaltCommand()
        {
            Command command = new Command();
            command.Opcode = (byte)CommandOpcodes.Halt;
            return command;
        }
    }
}
