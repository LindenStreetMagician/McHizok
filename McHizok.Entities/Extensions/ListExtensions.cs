namespace McHizok.Entities.Extensions;

public static class ListExtensions
{
    static readonly Random random = new();

    public static T GetRandomElement<T>(this List<T> list)
    {
        return list[random.Next(list.Count)];
    }
}
