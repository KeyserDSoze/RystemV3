namespace System.Collections
{
    public static class EnumerableExtensions
    {
        public static object? ElementAt(this IEnumerable entities, int index)
        {
            if (entities is IList list)
                return list[index];
            else
            {
                int counter = 0;
                foreach (var entity in entities)
                {
                    if (counter == index)
                        return entity;
                    counter++;
                }
                return null;
            }
        }
        public static bool SetElementAt(this IEnumerable entities, int index, object? value)
        {
            if (entities is IList list)
                list[index] = value;
            else
                return false;
            return true;
        }
    }
}
