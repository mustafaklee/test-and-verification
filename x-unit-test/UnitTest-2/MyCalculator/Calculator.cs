namespace UnitTest
{
    public class Calculator
    {
        public int Add(int a, int b)
        {
            return a + b;
        }
        public int Subtract(int a, int b)
        {
            return a - b;
        }

        public int Multiply(int a, int b)
        {
            return a * b;
        }

        public int Divide(int a, int b)
        {
            if (b == 0)
                throw new DivideByZeroException("Bir sayıyı sıfıra bölemezsin");
            return a / b;
        }
        public int abs(int a)
        {
            if (a < 0)
                return -1 * a;
            else
                return a;
        }
    }
}
