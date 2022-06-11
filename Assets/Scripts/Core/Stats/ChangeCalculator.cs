namespace Core.Stats
{
    public static class ChangeCalculator
    {
        public static int Calculate(int baseChange, bool percentage, int baseValue, int efficiency = 0)
        {
            return percentage ? (baseChange / 100) * baseValue : baseChange - efficiency;
        }
    }
}
