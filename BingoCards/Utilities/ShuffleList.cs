namespace BingoCards.Utilities
{
    //
    // https://stackoverflow.com/questions/273313/Randomize-a-listt
    //
    public static class ThreadSafeRandom
    {
        [ThreadStatic] private static Random? Local;
        public static Random ThisThreadsRandom
        {
            get { return Local ??= new Random(unchecked(Environment.TickCount * 31 + Environment.CurrentManagedThreadId)); }
        }
    }
    static class ShuffleList
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                (list[n], list[k]) = (list[k], list[n]);
            }
        }
    }
}
