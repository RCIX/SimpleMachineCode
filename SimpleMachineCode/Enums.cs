namespace SimpleMachineCode.Enums
{
    /// <summary>
    /// The various jump opcodes that can be used.
    /// </summary>
    public enum JumpOpcodes : byte
    {
        /// <summary>
        /// An unconditional jump.
        /// </summary>
        Unconditional = 0,
        /// <summary>
        /// A jump only if the results of the comparison is 0.
        /// </summary>
        Equal = 1,
        /// <summary>
        /// A jump only if the results of the comparison is not 0.
        /// </summary>
        NotEqual = 2,
        /// <summary>
        /// A jump only if the results of the comparison is positive.
        /// </summary>
        Above = 3,
        /// <summary>
        /// A jump only if the results of the comparison is positive or 0.
        /// </summary>
        AboveOrEqual = 4,
        /// <summary>
        /// A jump only if the results of the comparison are below 0.
        /// </summary>
        Below = 5,
        /// <summary>
        /// A jump only if the results of the comparison are below or equal to 0.
        /// </summary>
        BelowOrEqual = 6
    }
    /// <summary>
    /// The various opcodes that can be used.
    /// </summary>
    public enum CommandOpcodes : byte
    {
        Load = 0,
        Input = 1,
        Output = 2,
        Add = 3,
        Subtract = 4,
        Multiply = 5,
        Divide = 6,
        Modulus = 7,
        LeftShift = 8,
        RightShift = 9,
        Compare = 10,
        Jump = 11,
        LogicalAnd = 12,
        LogicalOr = 13,
        LogicalNot = 14,
        LogicalXor = 15,
        Halt = 255
    }
}
