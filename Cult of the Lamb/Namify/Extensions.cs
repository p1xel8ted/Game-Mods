namespace Namify;

public static class Extensions
{
    public static void AddRange<T>(this SortedSet<T> set, IEnumerable<T> elements)
    {
        foreach (var element in elements)
        {
            set.Add(element);
        }
    }
}