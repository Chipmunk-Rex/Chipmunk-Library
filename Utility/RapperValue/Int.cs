namespace Chipmunk.Library.Utility.RapperValue
{
    public class Int
    {
        private int value;
        public Int(int value)
        {
            this.value = value;
        }
        public static implicit operator Int(int i)
        {
            return new Int(i);
        }
        public static implicit operator int(Int i)
        {
            return i.value;
        }
        public static Int operator +(Int a, Int b)
        {
            return new Int(a.value + b.value);
        }
        public static Int operator -(Int a, Int b)
        {
            return new Int(a.value - b.value);
        }
        public static Int operator *(Int a, Int b)
        {
            return new Int(a.value * b.value);
        }
        public static Int operator /(Int a, Int b)
        {
            return new Int(a.value / b.value);
        }
        public static Int operator %(Int a, Int b)
        {
            return new Int(a.value % b.value);
        }
        public static Int operator ++(Int a)
        {
            return new Int(a.value + 1);
        }
        public static Int operator --(Int a)
        {
            return new Int(a.value - 1);
        }
        public static bool operator ==(Int a, Int b)
        {
            return a.value == b.value;
        }
        public static bool operator !=(Int a, Int b)
        {
            return a.value != b.value;
        }
        public static bool operator >(Int a, Int b)
        {
            return a.value > b.value;
        }
        public static bool operator <(Int a, Int b)
        {
            return a.value < b.value;
        }
        public static bool operator >=(Int a, Int b)
        {
            return a.value >= b.value;
        }
        public static bool operator <=(Int a, Int b)
        {
            return a.value <= b.value;
        }
        public override bool Equals(object obj)
        {
            if (obj is Int i)
            {
                return value == i.value;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
        public override string ToString()
        {
            return value.ToString();
        }
        public static Int Parse(string s)
        {
            return new Int(int.Parse(s));
        }
        public static Int TryParse(string s)
        {
            if (int.TryParse(s, out int result))
            {
                return new Int(result);
            }
            return null;
        }
        public static Int operator +(Int a, int b)
        {
            return new Int(a.value + b);
        }
        public static Int operator -(Int a, int b)
        {
            return new Int(a.value - b);
        }
        public static Int operator *(Int a, int b)
        {
            return new Int(a.value * b);
        }
        public static Int operator /(Int a, int b)
        {
            return new Int(a.value / b);
        }
        public static Int operator %(Int a, int b)
        {
            return new Int(a.value % b);
        }
        public static Int operator +(int a, Int b)
        {
            return new Int(a + b.value);
        }
        public static Int operator -(int a, Int b)
        {
            return new Int(a - b.value);
        }
        public static Int operator *(int a, Int b)
        {
            return new Int(a * b.value);
        }
        public static Int operator /(int a, Int b)
        {
            return new Int(a / b.value);
        }
        public static Int operator %(int a, Int b)
        {
            return new Int(a % b.value);
        }
    }
}