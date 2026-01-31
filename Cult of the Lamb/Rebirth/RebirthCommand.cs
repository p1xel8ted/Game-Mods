using Object = UnityEngine.Object;

namespace Rebirth;

internal class RebirthFollowerCommand : CustomFollowerCommand
{
    public override string InternalName => "REBIRTH_COMMAND";

    public override Sprite CommandIcon => TextureHelper.CreateSpriteFromPath(Path.Combine(Plugin.PluginPath, "assets", "rebirth_command.png"));


    public override string GetTitle(Follower follower)
    {
        return "Rebirth";
    }

    public RebirthFollowerCommand()
    {
        SubCommands = FollowerCommandGroups.AreYouSureCommands();
    }

    public override string GetDescription(Follower follower)
    {
        return "This follower getting you down? Order a rebirth!";
    }

    public override string GetLockedDescription(Follower follower)
    {
        if (DataManager.Instance.Followers_Recruit.Count > 0)
        {
            return "You already have a follower awaiting indoctrination!";
        }


        if (Helper.IsOld(follower))
        {
            return "Not enough life essence left to satisfy those below.";
        }

        return "Yeah, you shouldn't be seeing this...";
    }


    public override bool IsAvailable(Follower follower)
    {
        if (DataManager.Instance.Followers_Recruit.Count > 0)
        {
            return false;
        }

        if (Helper.IsOld(follower))
        {
            return false;
        }

        BornAgainFollower = SaveData.FollowerBornAgain(follower.Brain._directInfoAccess);
        return !BornAgainFollower;
    }

    public override bool ShouldAppearFor(Follower follower)
    {
        var bornAgainFollower = SaveData.FollowerBornAgain(follower.Brain._directInfoAccess);
        return !bornAgainFollower;
    }

    private static bool BornAgainFollower { get; set; }


    private static IEnumerator GiveFollowerIE(FollowerInfo f, Follower old)
    {
        yield return DieRoutine(old);

        yield return new WaitForSeconds(3f);
        BiomeBaseManager.Instance.SpawnExistingRecruits = false;
        yield return new WaitForEndOfFrame();

        yield return new WaitForSeconds(3f);
        DataManager.Instance.Followers_Recruit.Add(f);
        FollowerManager.SpawnExistingRecruits(BiomeBaseManager.Instance.RecruitSpawnLocation.transform.position);
        Object.FindObjectOfType<FollowerRecruit>()?.ManualTriggerAnimateIn();
        BiomeBaseManager.Instance.SpawnExistingRecruits = true;
        NotificationCentre.NotificationsEnabled = true;
        yield return new WaitForSeconds(2f);
    }


    internal static void SpawnRecruit(Follower follower)
    {
        var originalBrainInfo = follower.Brain.Info;
        var originalFollowerInfo = originalBrainInfo._info;
        var isUnique = Plugin.PreserveUniqueFollowers.Value && Helper.IsUniqueFollower(originalBrainInfo);

        BiomeBaseManager.Instance.SpawnExistingRecruits = true;
        NotificationCentre.NotificationsEnabled = false;
        var name = originalBrainInfo.Name;
        var oldId = originalBrainInfo.ID;
        var oldXp = originalBrainInfo.XPLevel;
        var newXp = Mathf.CeilToInt(oldXp * Plugin.XpPenaltyMultiplier.Value / 100f);
        var halfXp = Helper.DoHalfStats();

        // Create new follower - force skin if unique follower
        var fi = isUnique
            ? FollowerInfo.NewCharacter(FollowerLocation.Base, originalFollowerInfo.SkinName)
            : FollowerInfo.NewCharacter(FollowerLocation.Base);

        if (fi != null)
        {
            if (isUnique)
            {
                // Preserve unique follower skin and traits (name is always randomized)
                fi.SkinColour = originalFollowerInfo.SkinColour;
                fi.Traits = new List<FollowerTrait.TraitType>(originalFollowerInfo.Traits);
                Plugin.Log.LogInfo($"Unique follower rebirth: {name} -> {fi.Name} (skin: {originalFollowerInfo.SkinName}, traits: {string.Join(", ", originalFollowerInfo.Traits)})");
            }

            GameManager.GetInstance().StartCoroutine(GiveFollowerIE(fi, follower));
            Plugin.Log.LogInfo($"New follower: {fi.Name} (unique: {isUnique})");
            SaveData.AddBornAgainFollower(fi);
            fi.XPLevel = halfXp ? newXp : oldXp;
        }
        else
        {
            Plugin.Log.LogWarning("New follower is null!");
            NotificationCentre.NotificationsEnabled = true;
        }

        GameManager.GetInstance().StartCoroutine(ShowMessages(name, halfXp));
        RemoveFromDeadLists(oldId);
    }

    private static IEnumerator ShowMessages(string name, bool halfXp)
    {
        yield return new WaitForSeconds(3f);
        NotificationCentreScreen.Play($"{name} died to be reborn! All hail {name}!");
        yield return new WaitForSeconds(5f);
        if (!halfXp) yield break;
        NotificationCentre.Instance.PlayGenericNotification($"Oh no! {name} lost half of their XP during Rebirth!");
        yield return new WaitForSeconds(3f);
    }

    //this is stop being able to resurrect the old dead body of a born-again follower
    private static void RemoveFromDeadLists(int id)
    {
        Follower.Followers.RemoveAll(a => a.Brain._directInfoAccess.ID == id);
        DataManager.Instance.Followers_Dead.RemoveAll(a => a.ID == id);
        DataManager.Instance.Followers_Dead_IDs.RemoveAll(a => a == id);
    }

    private static IEnumerator DieRoutine(Follower follower)
    {
        follower.HideAllFollowerIcons();
        yield return new WaitForSeconds(0.5f);

        follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;

        yield return new WaitForSeconds(1f);
        follower.SetBodyAnimation("wave", true);
        yield return new WaitForSeconds(0.75f);

        // Set death cause to "old age" so it doesn't show as "murdered" in graveyard
        // This doesn't affect murder stats - those are only incremented by the actual Murder command
        follower.Brain._directInfoAccess.DiedOfOldAge = true;

        follower.Die(NotificationCentre.NotificationType.None, force: true);
    }


    // ReSharper disable once OptionalParameterHierarchyMismatch
    public override void Execute(interaction_FollowerInteraction interaction, FollowerCommands finalCommand)
    {
        if (finalCommand != FollowerCommands.AreYouSureYes) return;
        interaction.Close(true, reshowMenu: false);
        SpawnRecruit(interaction.follower);
    }
}