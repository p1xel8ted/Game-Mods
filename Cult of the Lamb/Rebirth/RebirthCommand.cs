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


        Helper.TooOld = Helper.IsOld(follower);
        if (Helper.TooOld)
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

        Helper.TooOld = Helper.IsOld(follower);
        if (Helper.TooOld)
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
        Object.FindObjectOfType<FollowerRecruit>().ManualTriggerAnimateIn();
        BiomeBaseManager.Instance.SpawnExistingRecruits = true;
        yield return new WaitForSeconds(2f);
    }


    internal static void SpawnRecruit(Follower follower)
    {
        BiomeBaseManager.Instance.SpawnExistingRecruits = true;
        NotificationCentre.NotificationsEnabled = false;
        var name = follower.name;
        var oldId = follower.Brain.Info.ID;
        var oldXp = follower.Brain.Info.XPLevel;
        var newXp = Mathf.CeilToInt(oldXp / 2f);
        var halfXp = Helper.DoHalfStats();

        var fi = FollowerInfo.NewCharacter(FollowerLocation.Base);

        if (fi != null)
        {
            GameManager.GetInstance().StartCoroutine(GiveFollowerIE(fi, follower));
            Plugin.Log.LogWarning($"New follower: {fi.Name}");
            SaveData.AddBornAgainFollower(fi);
            fi.XPLevel = oldXp;

            if (halfXp)
            {
                if (fi.XPLevel >= 2)
                {
                    fi.XPLevel -= 1;
                }

                fi.XPLevel = newXp;
            }
        }
        else
        {
            Plugin.Log.LogWarning("New follower is null!");
        }

        NotificationCentre.NotificationsEnabled = true;

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

        follower.Die(NotificationCentre.NotificationType.None, force: true);
    }


    // ReSharper disable once OptionalParameterHierarchyMismatch
    public override void Execute(interaction_FollowerInteraction interaction, FollowerCommands finalCommand)
    {
        if (finalCommand == FollowerCommands.AreYouSureYes)
        {
            SpawnRecruit(interaction.follower);
        }
    }
}