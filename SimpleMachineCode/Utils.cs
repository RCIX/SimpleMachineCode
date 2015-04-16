using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleMachineCode
{
    public static class Utils
    {
        /// <summary>
        /// converts a short into two component bytes.
        /// </summary>
        /// <param name="input">the short to break down.</param>
        /// <param name="high">the high byte output of the function.</param>
        /// <param name="low">the low byte output of the function.</param>
        public static void FromShort(short input, out byte high, out byte low)
        {
            high = (byte)(input >> 8);
            low = (byte)(input - ((input >> 8) << 8));
        }
        /// <summary>
        /// converts 2 component bytes into a short.
        /// </summary>
        /// <param name="high">the high byte of a short.</param>
        /// <param name="low">the low byte of a short.</param>
        /// <returns>the short that the two bytes represented.</returns>
        public static short ToShort(byte high, byte low)
        {
            return (short)((short)low + ((short)high << 8));
        }

    }
}
