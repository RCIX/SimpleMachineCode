using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using SimpleMachineCode.Exceptions;
using SimpleMachineCode.Commands;
using SimpleMachineCode.Enums;
using NUnit.Framework;

namespace SimpleMachineCode.Processor
{
    public delegate void EmptyEventHandler();
    public sealed class VirtualProcessor
    {
        private Dictionary<short, Command> _currentProgram;
        private Dictionary<byte, short> _registers;
        public Timer Timer { get; private set; }
        public event EmptyEventHandler OnHalted = null;


        /// <summary>
        /// The current program. If you change the dictionary itself, then it automatically 
        /// shuts off the current program. 
        /// </summary>
        public Dictionary<short, Command> CurrentProgram
        {
            get { return _currentProgram; }
            set { _currentProgram = value; }
        }

        /// <summary>
        /// the registers that this processor has.
        /// </summary>
        public Dictionary<byte, short> Registers
        {
            get { return _registers; }
        }

        /// <summary>
        /// The "input channels" that this processor has. An input channel is a means to 
        /// retrieve a short from an arbitrary location with any sort of calculation 
        /// neccessary.
        /// </summary>
        public Dictionary<byte, Func<short>> InputChannels { get; set; }

        /// <summary>
        /// The "output channels" that this processor has. An output channel is a means to
        /// output a short to an arbitrary location with any sort of calculation neccesary.
        /// </summary>
        public Dictionary<byte, Action<short>> OutputChannels { get; set; }

        /// <summary>
        /// the register that stores the results from any comparison operations.
        /// </summary>
        public short CompareRegister { get; set; }

        /// <summary>
        /// the instruction that will be executed next out of the list of instructions.
        /// </summary>
        public short InstructionCounter { get; set; }

        /// <summary>
        /// represents whether this machine is currently halted and needs changing of the 
        /// program counter or resetting to continue.
        /// </summary>
        public bool Halted { get; set; }


        /// <summary>
        /// Initializes the VirtualProcessor. The program will not be defined, nor will any 
        /// input/output channels, and they must be set if they are to be used.
        /// </summary>
        public VirtualProcessor()
        {
            Reset();
        }

        /// <summary>
        /// Resets the processor to an initial state. All registers, input/output channels, and 
        /// the current program will be destroyed.
        /// </summary>
        public void Reset()
        {
            InstructionCounter = 0;
            CompareRegister = 0;
            _registers = new Dictionary<byte, short>(256);
            InputChannels = new Dictionary<byte, Func<short>>(256);
            OutputChannels = new Dictionary<byte, Action<short>>(256);
            OnHalted = null;
            Halted = false;
        }

        /// <summary>
        /// Runs the program until a halt instruction is countered or the program enters an 
        /// invalid state.
        /// </summary>
        public void ExecuteTillHalt()
        {
            while (!this.Halted)
                ExecuteInstruction();
        }

        /// <summary>
        /// Executes a single instruction from the current program.
        /// </summary>
        public void ExecuteInstruction()
        {
            if (Halted == true)
                return;
            if ( CurrentProgram == null || CurrentProgram.Count == 0)
                throw new ProgramEmptyException();
            Command command;
            if (!CurrentProgram.TryGetValue(InstructionCounter, out command))
                throw new InvalidInstructionCounterException();
            bool increment = true;
            switch ((CommandOpcodes)command.Opcode)
            {
                case CommandOpcodes.Load:
                    Registers[command.Data1] = Utils.ToShort(command.Data2, command.Data3);
                    break;
                case CommandOpcodes.Input:
                    Func<short> inChannel;
                    
                    if (InputChannels.TryGetValue(command.Data2, out inChannel))
                        Registers[command.Data1] = inChannel();
                    else
                        throw new InvalidChannelException("Invalid input channel " + command.Data2);
                    break;
                case CommandOpcodes.Output:
                    Action<short> outChannel;
                    OutputChannels.TryGetValue(command.Data2, out outChannel);
                    if (outChannel != null)
                        outChannel(Registers[command.Data1]);
                    else
                        throw new InvalidChannelException("Invalid output channel " + command.Data2);
                    break;
                case CommandOpcodes.Add:
                    Registers[command.Data1] = (short)(Registers[command.Data2] + Registers[command.Data3]);
                    break;
                case CommandOpcodes.Subtract:
                    Registers[command.Data1] = (short)(Registers[command.Data2] - Registers[command.Data3]);
                    break;
                case CommandOpcodes.Multiply:
                    Registers[command.Data1] = (short)(Registers[command.Data2] * Registers[command.Data3]);
                    break;
                case CommandOpcodes.Divide:
                    Registers[command.Data1] = (short)(Registers[command.Data2] / Registers[command.Data3]);
                    break;
                case CommandOpcodes.Modulus:
                    Registers[command.Data1] = (short)(Registers[command.Data2] % Registers[command.Data3]);
                    break;
                case CommandOpcodes.LeftShift:
                    Registers[command.Data1] = (short)(Registers[command.Data2] << Registers[command.Data3]);
                    break;
                case CommandOpcodes.RightShift:
                    Registers[command.Data1] = (short)(Registers[command.Data2] >> Registers[command.Data3]);
                    break;
                case CommandOpcodes.Compare:
                    CompareRegister = (short)(Registers[command.Data1] - Registers[command.Data2]);
                    break;
                case CommandOpcodes.Jump:
                    switch ((JumpOpcodes)command.Data1)
                    {
                        case JumpOpcodes.Above:
                            if (CompareRegister > 0)
                            {
                                InstructionCounter = Utils.ToShort(command.Data2, command.Data3);
                                increment = false;
                            }
                            break;
                        case JumpOpcodes.AboveOrEqual:
                            if (CompareRegister >= 0)
                            {
                                InstructionCounter = Utils.ToShort(command.Data2, command.Data3);
                                increment = false;
                            }
                            break;
                        case JumpOpcodes.Below:
                            if (CompareRegister < 0)
                            {
                                InstructionCounter = Utils.ToShort(command.Data2, command.Data3);
                                increment = false;
                            }
                            break;
                        case JumpOpcodes.BelowOrEqual:
                            if (CompareRegister <= 0)
                            {
                                InstructionCounter = Utils.ToShort(command.Data2, command.Data3);
                                increment = false;
                            }
                            break;
                        case JumpOpcodes.Equal:
                            if (CompareRegister == 0)
                            {
                                InstructionCounter = Utils.ToShort(command.Data2, command.Data3);
                                increment = false;
                            }
                            break;
                        case JumpOpcodes.NotEqual:
                            if (CompareRegister != 0)
                            {
                                InstructionCounter = Utils.ToShort(command.Data2, command.Data3);
                                increment = false;
                            }
                            break;
                        case JumpOpcodes.Unconditional:
                            InstructionCounter = Utils.ToShort(command.Data2, command.Data3);
                            increment = false;
                            break;
                    }
                    break;
                case CommandOpcodes.LogicalAnd:
                    Registers[command.Data1] = (short)(Registers[command.Data2] & Registers[command.Data3]);
                    break;
                case CommandOpcodes.LogicalOr:
                    Registers[command.Data1] = (short)(Registers[command.Data2] | Registers[command.Data3]);
                    break;
                case CommandOpcodes.LogicalNot:
                    Registers[command.Data1] = (short)(~Registers[command.Data2]);
                    break;
                case CommandOpcodes.LogicalXor:
                    Registers[command.Data1] = (short)(Registers[command.Data2] ^ Registers[command.Data3]);
                    break;
                case CommandOpcodes.Halt:
                    Halted = true;
                    if (OnHalted != null) OnHalted();
                    break;
                default:
                    throw new InvalidInstructionException();
            }
            if (increment)
                InstructionCounter++;
        }
    }
}
