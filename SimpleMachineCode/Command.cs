using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleMachineCode.Commands
{
    public struct Command
    {
        public byte Opcode;
        public byte Data1;
        public byte Data2;
        public byte Data3;

        #region Equality and Hashing
        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 23 + Opcode.GetHashCode();
            hash *= 23 + Data1.GetHashCode();
            hash *= 23 + Data2.GetHashCode();
            hash *= 23 + Data3.GetHashCode();
            return hash;
        }

        public override bool Equals(object obj)
        {
            return obj is Command && this.Equals((Command)obj);
        }

        public bool Equals(Command other)
        {
            return this.Opcode.Equals(other.Opcode) && 
                this.Data1.Equals(other.Data1) && 
                this.Data2.Equals(other.Data2) && 
                this.Data3.Equals(other.Data3);
        }

        public Command Copy()
        {
            return new Command
            {
                Opcode = this.Opcode,
                Data1 = this.Data1,
                Data2 = this.Data2,
                Data3 = this.Data3
            };
        }

        public static bool operator ==(Command a, Command b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Command a, Command b)
        {
            return !(a == b);
        }
        #endregion

        public static implicit operator byte[](Command c)
        {
            byte[] commandAsBytes = new byte[4];
            commandAsBytes[0] = c.Opcode;
            commandAsBytes[1] = c.Data1;
            commandAsBytes[2] = c.Data2;
            commandAsBytes[3] = c.Data3;
            return commandAsBytes;
        }
        public static implicit operator Command(byte[] b)
        {
            if (b.Length != 4)
                throw new ArgumentException("the byte conversion must take place on a 4 byte array.");
            Command c = new Command { Opcode = b[0], Data1 = b[1], Data2 = b[2], Data3 = b[3] };
            return c;
        }
    }
}
