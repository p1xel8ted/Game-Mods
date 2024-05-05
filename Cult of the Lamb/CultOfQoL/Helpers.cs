namespace CultOfQoL;

public static class Helpers
{
    internal static List<Follower> AllFollowers => FollowerManager.Followers.SelectMany(followerList => followerList.Value).ToList();

    public static IEnumerator FilterEnumerator(IEnumerator original, Type[] typesToRemove)
    {
        while (original.MoveNext())
        {
            var current = original.Current;
            if (current != null && !typesToRemove.Contains(current.GetType()))
            {
                yield return current;
            }
        }
    }

    public static string GetGameObjectPath(GameObject obj)
    {
        var path = obj.name;
        var currentParent = obj.transform.parent;

        while (currentParent != null)
        {
            path = currentParent.name + "/" + path;
            currentParent = currentParent.parent;
        }

        return path;
    }
}