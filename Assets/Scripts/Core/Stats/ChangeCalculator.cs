namespace Core.Stats
{
    public static class ChangeCalculator
    {
        public static int Calculate(int delta, bool percentage, int baseValue, int efficiency = 0)
        {
            return percentage ? (delta / 100) * baseValue : delta - efficiency;
        }
    }
}
