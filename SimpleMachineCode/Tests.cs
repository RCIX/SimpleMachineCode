using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SimpleMachineCode.Processor;
using SimpleMachineCode.Commands;
using SimpleMachineCode.Exceptions;
using SimpleMachineCode.Enums;

namespace SimpleMachineCode
{
    [TestFixture]
    class VirtualProcessorTests
    {
        private VirtualProcessor vp;

        [SetUp]
        public void SetUp()
        {
            vp = new VirtualProcessor();
        }

        [Test]
        public void TestHaltCommand()
        {
            Dictionary<short, Command> commands = new Dictionary<short, Command>();
            commands.Add(0, CommandFactory.CreateHaltCommand());
            vp.CurrentProgram = commands;
            while (!vp.Halted)
                vp.ExecuteInstruction();
            Assert.True(vp.Halted);
        }

        [Test]
        public void TestLoadCommand()
        {
            Dictionary<short, Command> commands = new Dictionary<short, Command>();
            commands.Add(0, CommandFactory.CreateLoadCommand(0, 2));
            commands.Add(1, CommandFactory.CreateHaltCommand());
            vp.CurrentProgram = commands;
            while (!vp.Halted)
                vp.ExecuteInstruction();
            Assert.AreEqual(vp.Registers[0], 2);
        }

        [Test]
        public void TestInputCommand()
        {
            Dictionary<short, Command> commands = new Dictionary<short, Command>();
            commands.Add(0, CommandFactory.CreateInputCommand(0, 0));
            commands.Add(1, CommandFactory.CreateHaltCommand());
            vp.InputChannels.Add(0, () => { return 5; });
            vp.CurrentProgram = commands;
            while (!vp.Halted)
                vp.ExecuteInstruction();
            Assert.AreEqual(vp.Registers[0], 5);
        }

        [Test]
        public void TestOutputCommand()
        {
            short number = 0;
            Dictionary<short, Command> commands = new Dictionary<short, Command>();
            commands.Add(0, CommandFactory.CreateLoadCommand(0, 10));
            commands.Add(1, CommandFactory.CreateOutputCommand(0, 0));
            commands.Add(2, CommandFactory.CreateHaltCommand());
            vp.OutputChannels.Add(0, (val) => { number = val; });
            vp.CurrentProgram = commands;
            while (!vp.Halted)
                vp.ExecuteInstruction();
            Assert.AreEqual(number, 10);

        }

        [Test]
        public void TestAddCommand()
        {
            DoStandardCommandTesting(CommandOpcodes.Add, 2, 2, 4);
            DoStandardCommandTesting(CommandOpcodes.Add, 5, 1, 6);
            DoStandardCommandTesting(CommandOpcodes.Add, 45, -2, 43);
            DoStandardCommandTesting(CommandOpcodes.Add, 2, -2, 0);
        }

        [Test]
        public void TestSubtractCommand()
        {
            DoStandardCommandTesting(CommandOpcodes.Subtract, 4, 2, 2);
            DoStandardCommandTesting(CommandOpcodes.Subtract, 765, 23, 742);
            DoStandardCommandTesting(CommandOpcodes.Subtract, -38, 63, -101);
            DoStandardCommandTesting(CommandOpcodes.Subtract, 2, 2, 0);
            
        }

        [Test]
        public void TestMultiplyCommand()
        {
            DoStandardCommandTesting(CommandOpcodes.Multiply, 4, 4, 16);
            DoStandardCommandTesting(CommandOpcodes.Multiply, 2, 5, 10);
            DoStandardCommandTesting(CommandOpcodes.Multiply, 249, 60, 14940);
            DoStandardCommandTesting(CommandOpcodes.Multiply, -999, 20, -19980);
        }

        [Test]
        public void TestDivideCommand()
        {
            DoStandardCommandTesting(CommandOpcodes.Divide, 16, 8, 2);
            DoStandardCommandTesting(CommandOpcodes.Divide, 920, 23, 40);
            DoStandardCommandTesting(CommandOpcodes.Divide, -40, 4, -10);
        }

        [Test]
        public void TestModulusCommand()
        {
            DoStandardCommandTesting(CommandOpcodes.Modulus, 20, 5, 0);
            DoStandardCommandTesting(CommandOpcodes.Modulus, 569, 54, 29);
            DoStandardCommandTesting(CommandOpcodes.Modulus, -296, 17, -7);
        }

        [Test]
        public void TestLeftShiftCommand()
        {
            DoStandardCommandTesting(CommandOpcodes.LeftShift, 5, 2, 20);
            DoStandardCommandTesting(CommandOpcodes.LeftShift, 9348, 4, 18496);
            DoStandardCommandTesting(CommandOpcodes.LeftShift, -5699, 3, 19944);
        }

        [Test]
        public void TestRightShiftCommand()
        {
            DoStandardCommandTesting(CommandOpcodes.RightShift, 20, 2, 5);
            DoStandardCommandTesting(CommandOpcodes.RightShift, 18496, 4, 1156);
            DoStandardCommandTesting(CommandOpcodes.RightShift, 19944, 3, 2493);
        }

        [Test]
        public void TestLogicalAndCommand()
        {
            DoStandardCommandTesting(CommandOpcodes.LogicalAnd, 5, 3, 1);
            DoStandardCommandTesting(CommandOpcodes.LogicalAnd, 928, 295, 288);
            DoStandardCommandTesting(CommandOpcodes.LogicalAnd, 124, 489, 104);
        }

        [Test]
        public void TestLogicalOrCommand()
        {
            DoStandardCommandTesting(CommandOpcodes.LogicalOr, 5, 3, 7);
            DoStandardCommandTesting(CommandOpcodes.LogicalOr, 23, 97, 119);
            DoStandardCommandTesting(CommandOpcodes.LogicalOr, 234, 756, 766);
            DoStandardCommandTesting(CommandOpcodes.LogicalOr, 374, 843, 895);
        }

        [Test]
        public void TestLogicalXorCommand()
        {
            DoStandardCommandTesting(CommandOpcodes.LogicalXor, 5, 3, 6);
            DoStandardCommandTesting(CommandOpcodes.LogicalXor, 29, 45, 48);
            DoStandardCommandTesting(CommandOpcodes.LogicalXor, -21, 35, -56);
            DoStandardCommandTesting(CommandOpcodes.LogicalXor, -32, 13, -19);
        }

        [Test]
        public void TestLogicalNotCommand()
        {
            DoStandardCommandTesting(CommandOpcodes.LogicalNot, 5, 0, -6);
            DoStandardCommandTesting(CommandOpcodes.LogicalNot, 340, 0, -341);
            DoStandardCommandTesting(CommandOpcodes.LogicalNot, 4958, 0, -4959);
            DoStandardCommandTesting(CommandOpcodes.LogicalNot, 340, 0, -341);
            DoStandardCommandTesting(CommandOpcodes.LogicalNot, -9128, 0, 9127);
        }

        [Test]
        public void TestCompareCommand()
        {
            DoCompareCommandTesting(20, 40, -20);
            DoCompareCommandTesting(58, 20, 38);
            DoCompareCommandTesting(40, 40, 0);
            DoCompareCommandTesting(-20, -5, -15);
        }

        [Test]
        public void TestReturnsOnHalted()
        {
            Dictionary<short, Command> commands = new Dictionary<short, Command>();
            commands.Add(0, CommandFactory.CreateHaltCommand());
            vp.CurrentProgram = commands;
            vp.ExecuteInstruction();
            Assert.AreEqual(vp.InstructionCounter, (short)1);
            vp.ExecuteInstruction();
            Assert.AreEqual(vp.InstructionCounter, (short)1);
        }
        
        [Test]
        public void TestOnHaltedEvent()
        {
            bool succeeded = false;
            Dictionary<short, Command> commands = new Dictionary<short, Command>();
            commands.Add(0, CommandFactory.CreateHaltCommand());
            vp.CurrentProgram = commands;
            vp.OnHalted += () => 
            {
                succeeded = true;
            };
            vp.ExecuteInstruction();
            Assert.True(succeeded);
        }

        [Test]
        public void TestJumpOpcodes()
        {
            DoJumpCommandTesting(JumpOpcodes.Above, 2, 0);
            DoJumpCommandTesting(JumpOpcodes.AboveOrEqual, 0, -4);
            DoJumpCommandTesting(JumpOpcodes.Below, -5, 2);
            DoJumpCommandTesting(JumpOpcodes.BelowOrEqual, -5, 2);
            DoJumpCommandTesting(JumpOpcodes.Equal, 0, 1);
            DoJumpCommandTesting(JumpOpcodes.NotEqual, 1, 0);
        }

        [Test]
        public void TestUnconditionalJumpOpcode()
        {
            double numberOfRuns = 0;
            vp.Reset();
            vp.OutputChannels[0] = (val) =>
            {
                numberOfRuns += 1;
            };
            Dictionary<short, Command> commands = new Dictionary<short, Command>();
            commands.Add(0, CommandFactory.CreateLoadCommand(0, 10));
            commands.Add(1, CommandFactory.CreateOutputCommand(0, 0));
            commands.Add(2, CommandFactory.CreateJumpCommand(JumpOpcodes.Unconditional, 4));
            commands.Add(3, CommandFactory.CreateHaltCommand());
            commands.Add(4, CommandFactory.CreateOutputCommand(0, 0));
            commands.Add(5, CommandFactory.CreateHaltCommand());
            vp.CurrentProgram = commands;
            while (!vp.Halted)
                vp.ExecuteInstruction();
            Assert.AreEqual(numberOfRuns, 2);
        }

        [Test]
        [ExpectedException(typeof(ProgramEmptyException))]
        public void TestThrowsEmptyProgramException()
        {
            vp.ExecuteInstruction();
        }

        [Test]
        [ExpectedException(typeof(InvalidChannelException))]
        public void TestThrowsInvalidInputChannelException()
        {
            Dictionary<short, Command> commands = new Dictionary<short, Command>();
            commands.Add(0, CommandFactory.CreateInputCommand(0, 2));
            commands.Add(1, CommandFactory.CreateHaltCommand());
            vp.CurrentProgram = commands;
            while (!vp.Halted)
                vp.ExecuteInstruction();
        }

        [Test]
        [ExpectedException(typeof(InvalidChannelException))]
        public void TestThrowsInvalidOutputChannelException()
        {

            Dictionary<short, Command> commands = new Dictionary<short, Command>();
            commands.Add(0, CommandFactory.CreateOutputCommand(0, 2));
            commands.Add(1, CommandFactory.CreateHaltCommand());
            vp.CurrentProgram = commands;
            while (!vp.Halted)
                vp.ExecuteInstruction();
        }

        [Test]
        [ExpectedException(typeof(InvalidInstructionException))]
        public void TestThrowsInvalidInstructionException()
        {

            Dictionary<short, Command> commands = new Dictionary<short, Command>();
            Command command = new Command();
            command.Opcode = 35;
            commands.Add(0, command);
            commands.Add(1, CommandFactory.CreateHaltCommand());
            vp.CurrentProgram = commands;
            while (!vp.Halted)
                vp.ExecuteInstruction();
        }

        [Test]
        [ExpectedException(typeof(InvalidInstructionCounterException))]
        public void TestThrowsInvalidInstructionCounterException()
        {
            Dictionary<short, Command> commands = new Dictionary<short, Command>();
            commands.Add(0, CommandFactory.CreateHaltCommand());
            vp.CurrentProgram = commands;
            vp.InstructionCounter = 2;
            vp.ExecuteInstruction();
        }

        private void DoStandardCommandTesting(CommandOpcodes opcode, short value1, short value2, short expected)
        {
            vp.Reset();
            Dictionary<short, Command> commands = new Dictionary<short, Command>();
            commands.Add(0, CommandFactory.CreateLoadCommand(0, value1));
            commands.Add(1, CommandFactory.CreateLoadCommand(1, value2));
            Command customCommand = new Command
            {
                Opcode = (byte)opcode,
                Data1 = 2,
                Data2 = 0,
                Data3 = 1
            };
            commands.Add(2, customCommand);
            commands.Add(3, CommandFactory.CreateHaltCommand());
            vp.CurrentProgram = commands;
            while (!vp.Halted)
                vp.ExecuteInstruction();
            Assert.AreEqual(vp.Registers[2], expected);
        }

        private void DoCompareCommandTesting(short value1, short value2, short expected)
        {
            vp.Reset();
            Dictionary<short, Command> commands = new Dictionary<short, Command>();
            commands.Add(0, CommandFactory.CreateLoadCommand(0, value1));
            commands.Add(1, CommandFactory.CreateLoadCommand(1, value2));
            commands.Add(2, CommandFactory.CreateComparisonCommand(0, 1));
            commands.Add(3, CommandFactory.CreateHaltCommand());
            vp.CurrentProgram = commands;
            while (!vp.Halted)
                vp.ExecuteInstruction();
            Assert.AreEqual(vp.CompareRegister, expected);
        }

        private void DoJumpCommandTesting(JumpOpcodes opcode, short comparison1, short comparison2)
        {
            double numberOfRuns = 0;
            vp.Reset();
            vp.OutputChannels[0] = (val) =>
            {
                numberOfRuns += 1;
            };
            Dictionary<short, Command> commands = new Dictionary<short, Command>();
            commands.Add(0, CommandFactory.CreateLoadCommand(0, 10));
            commands.Add(1, CommandFactory.CreateOutputCommand(0, 0));
            commands.Add(2, CommandFactory.CreateJumpCommand(opcode, 4));
            commands.Add(3, CommandFactory.CreateHaltCommand());
            commands.Add(4, CommandFactory.CreateOutputCommand(0, 0));
            commands.Add(5, CommandFactory.CreateHaltCommand());
            vp.CurrentProgram = commands;
            vp.CompareRegister = comparison1;
            while (!vp.Halted)
                vp.ExecuteInstruction();
            Assert.AreEqual(numberOfRuns, 2);
            numberOfRuns = 0;
            vp.InstructionCounter = 0;
            vp.Halted = false;
            vp.CompareRegister = comparison2;
            while (!vp.Halted)
                vp.ExecuteInstruction();
            Assert.AreEqual(numberOfRuns, 1);

        }
    }

    [TestFixture]
    class CommandTests
    {
        private Random rand = new Random();

        private Command GetRandomCommand()
        {
            byte[] arr = new byte[4];
            rand.NextBytes(arr);
            return (Command)arr;
        }

        [Test]
        public void TestImplicitCasting()
        {
            Command command1 = new Command
            {
                Opcode = 0,
                Data1 = 0,
                Data2 = 0,
                Data3 = 0
            };
            byte[] arr = command1;
            Assert.That(arr, Is.EqualTo(new byte[] { 0, 0, 0, 0 }));
            Command command2 = arr;
            Assert.That(command2, Is.EqualTo(command1));
        }

        [Test]
        public void TestHashCodeFunction()
        {
            Command command1 = GetRandomCommand(); 
            Command command2 = command1.Copy();
            Assert.That(command1.GetHashCode(), Is.EqualTo(command2.GetHashCode()));
            Command command3 = GetRandomCommand();
            Assert.That(command1.GetHashCode(), Is.Not.EqualTo(command3.GetHashCode()));
        }

        [Test]
        public void TestEqualityFunction()
        {
            Command command1 = GetRandomCommand();
            Command command2 = command1.Copy();
            Command command3 = GetRandomCommand();
            Assert.That(command1.Equals(command2));
            Assert.That(command1 == command2);
            Assert.That(!command1.Equals(command3));
            Assert.That(command1 != command3);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestThrowsArgumentExceptionOnConversion()
        {
            byte[] arr = new byte[] { 0, 0, 0, 0, 0 };
            Command command = arr;
        }

    }
}
