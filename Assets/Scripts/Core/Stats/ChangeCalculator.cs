namespace Core.Stats
{
    public static class ChangeCalculator
    {
        public static int Calculate(int delta, bool percentage, int baseValue, int penalty = 0)
        {
            return percentage ? (delta / 100) * baseValue : delta + penalty;
        }
    }
}
