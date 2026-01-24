namespace Rebirth;

public static class Helper
{
    public static bool IsOld(Follower follower)
    {
        if (Plugin.RebirthOldFollowers.Value)
        {
            return false;
        }
        return follower.Outfit.CurrentOutfit == FollowerOutfitType.Old && (follower.Brain.Info.OldAge || follower.Brain.HasThought(Thought.OldAge));
    }

    public static bool DoHalfStats()
    {
        return Random.Range(0f, 1f) <= Plugin.XpPenaltyChance.Value / 100f;
    }
    
}