using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace NetCoreUnmanagedInterop
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = sizeof(byte))]
    public struct InteropBool : IEquatable<InteropBool>
    {
        public static readonly InteropBool TRUE = new InteropBool((byte)1);
        public static readonly InteropBool FALSE = new InteropBool((byte)0);

        private readonly byte value;

        private InteropBool(byte value)
        {
            this.value = value;
        }

        public bool Equals(InteropBool other)
        {
            return value == other.value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is InteropBool && Equals((InteropBool)obj);
        }

        public override int GetHashCode()
        {
            return this ? TRUE.value : FALSE.value;
        }

        public override string ToString()
        {
            return this ? "true" : "false";
        }

        public static bool operator ==(InteropBool lhs, InteropBool rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(InteropBool lhs, InteropBool rhs)
        {
            return !lhs.Equals(rhs);
        }

        public static implicit operator InteropBool(bool operand)
        {
            return operand ? TRUE : FALSE;
        }

        public static implicit operator bool(InteropBool operand)
        {
            return operand != FALSE;
        }
    }
}
