namespace Core.Stats
{
    public static class ChangeCalculator
    {
        public static int Calculate(int delta, bool percentage, int baseValue)
        {
            return percentage ? (delta / 100) * baseValue : delta;
        }
    }
}
