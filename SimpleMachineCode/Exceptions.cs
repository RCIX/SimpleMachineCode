using System;

namespace SimpleMachineCode.Exceptions
{
    public sealed class ProgramTooLargeException : Exception
    {
        public ProgramTooLargeException() : base() { }
        public ProgramTooLargeException(string message) : base(message) { }
        public ProgramTooLargeException(string message, Exception innerException) : base(message, innerException) { }
    }
    public sealed class ProgramEmptyException : Exception
    {
        public ProgramEmptyException() : base() { }
        public ProgramEmptyException(string message) : base(message) { }
        public ProgramEmptyException(string message, Exception innerException) : base(message, innerException) { }
    }
    public sealed class InvalidInstructionCounterException : Exception
    {
        public InvalidInstructionCounterException() : base() { }
        public InvalidInstructionCounterException(string message) : base(message) { }
        public InvalidInstructionCounterException(string message, Exception innerException) : base(message, innerException) { }
    }
    public sealed class InvalidChannelException : Exception
    {
        public InvalidChannelException() : base() { }
        public InvalidChannelException(string message) : base(message) { }
        public InvalidChannelException(string message, Exception innerException) : base(message, innerException) { }
    }
    public sealed class InvalidInstructionException : Exception
    {
        public InvalidInstructionException() : base() { }
        public InvalidInstructionException(string message) : base(message) { }
        public InvalidInstructionException(string message, Exception innerException) : base(message, innerException) { }
    }
    public sealed class InvalidCommandException : Exception
    {
        public InvalidCommandException() : base() { }
        public InvalidCommandException(string message) : base(message) { }
        public InvalidCommandException(string message, Exception innerException) : base(message, innerException) { }
    }
    public sealed class MalformedLineException : Exception
    {
        public MalformedLineException() : base() { }
        public MalformedLineException(string message) : base(message) { }
        public MalformedLineException(string message, Exception innerException) : base(message, innerException) { }
    }
    public sealed class InvalidJumpLabelException : Exception
    {
        public InvalidJumpLabelException() : base() { }
        public InvalidJumpLabelException(string message) : base(message) { }
        public InvalidJumpLabelException(string message, Exception innerException) : base(message, innerException) { }
    }
}
