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

    /// <summary>
    /// Checks if a follower is "unique" (has special skin or unique traits that should be preserved on rebirth).
    /// Unique followers include: Webber, Sozo, Ratau, Baal, Aym, and other special characters.
    /// </summary>
    public static bool IsUniqueFollower(FollowerBrainInfo brainInfo)
    {
        var info = brainInfo._info;

        // Check for unique traits (Immortal, DontStarve, BornToTheRot, etc.)
        if (info.Traits.Any(t => FollowerTrait.UniqueTraits.Contains(t)))
        {
            return true;
        }

        // Check for special skin names (not generic animal skins)
        if (!string.IsNullOrEmpty(info.SkinName))
        {
            // Known unique follower skin names
            var uniqueSkins = new[]
            {
                "Webber", "Sozo", "Ratau", "Baal", "Aym", "Haro", "Klunko", "Bop",
                "Jalala", "Rinor", "Flinky", "Helob", "Leshy", "Heket", "Kallamar", "Shamura",
                "Narinder", "Plimbo", "Chemach", "Fox", "Cthulhu"
            };
            if (uniqueSkins.Any(s => info.SkinName.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0))
            {
                return true;
            }
        }

        return false;
    }
}