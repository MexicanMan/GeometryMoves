namespace Test.Utils
{
    public static class MathUtils
    {
        public static int Repeat(int value, int max)
        {
            return (value % max + max) % max;
        }
    }
}