using CultOfQoL.Core;

namespace CultOfQoL.Patches.Followers;

/// <summary>
/// Direct-effect implementations for mass actions.
///
/// These methods bypass the game's coroutine-based routines to avoid callback timing issues.
/// Each method applies the follower's reaction animation + effects directly.
/// </summary>
public static class MassActionEffects
{
    /// <summary>
    /// Applies bribe effects directly.
    /// Animation: Reactions/react-love2
    /// </summary>
    public static void ApplyBribe(Follower follower, bool deductGold = true, bool spawnCoins = true)
    {
        if (follower?.Brain == null) return;

        follower.FacePosition(PlayerFarming.Instance.transform.position);
        follower.SetBodyAnimation("Reactions/react-love2", false);
        follower.AddBodyAnimation("idle", true, 0f);

        follower.Brain.AddThought(Thought.Bribed);
        follower.Brain.Stats.Bribed = true;

        if (deductGold)
        {
            Inventory.ChangeItemQuantity(20, -3);
        }

        if (spawnCoins && PlayerFarming.Instance?.CameraBone != null)
        {
            for (var i = 0; i < 3; i++)
            {
                AudioManager.Instance?.PlayOneShot("event:/followers/pop_in", follower.transform.position);
                ResourceCustomTarget.Create(
                    follower.gameObject,
                    PlayerFarming.Instance.CameraBone.transform.position,
                    InventoryItem.ITEM_TYPE.BLACK_GOLD,
                    null
                );
            }
        }

        follower.Brain.AddAdoration(FollowerBrain.AdorationActions.Bribe, null);
        AudioManager.Instance?.PlayOneShot("event:/followers/gain_loyalty", follower.gameObject.transform.position);

        Plugin.WriteLog($"[MassEffect] Bribe applied to {follower.Brain.Info.Name}");
    }

    /// <summary>
    /// Applies intimidate effects directly.
    /// Animation: Reactions/react-intimidate
    /// </summary>
    public static void ApplyIntimidate(Follower follower, bool allowScaredTrait)
    {
        if (follower?.Brain == null) return;

        follower.FacePosition(PlayerFarming.Instance.transform.position);
        follower.SetBodyAnimation("Reactions/react-intimidate", false);
        follower.AddBodyAnimation("idle", true, 0f);

        // 5% chance to add Scared trait (vanilla only applies on the host follower)
        if (allowScaredTrait && Random.value < 0.05f &&
            !follower.Brain.HasTrait(FollowerTrait.TraitType.Scared) &&
            !follower.Brain.HasTrait(FollowerTrait.TraitType.CriminalScarred))
        {
            follower.Brain.AddTrait(FollowerTrait.TraitType.Scared, true);
        }

        follower.Brain.Stats.Intimidated = true;
        follower.Brain.AddThought(Thought.Intimidated);
        follower.Brain.AddAdoration(FollowerBrain.AdorationActions.Intimidate, null);
        AudioManager.Instance?.PlayOneShot("event:/followers/gain_loyalty", follower.gameObject.transform.position);

        Plugin.WriteLog($"[MassEffect] Intimidate applied to {follower.Brain.Info.Name}");
    }

    /// <summary>
    /// Applies bless effects directly.
    /// Animation: devotion/devotion-start -> idle
    /// </summary>
    public static void ApplyBless(Follower follower)
    {
        if (follower?.Brain == null) return;

        follower.FacePosition(PlayerFarming.Instance.transform.position);
        follower.SetBodyAnimation("devotion/devotion-start", false);
        follower.AddBodyAnimation("idle", true, 0f);

        follower.Brain.Stats.ReceivedBlessing = true;
        follower.Brain.Stats.LastBlessing = DataManager.Instance.CurrentDayIndex;
        follower.Brain.AddAdoration(FollowerBrain.AdorationActions.Bless, null);

        var faithMultiplier = MassActionCosts.GetFaithMultiplier();
        if (faithMultiplier > 0f)
        {
            CultFaithManager.AddThought(Thought.Cult_Bless, follower.Brain.Info.ID, faithMultiplier);
        }
        AudioManager.Instance?.PlayOneShot("event:/followers/gain_loyalty", follower.gameObject.transform.position);

        Plugin.WriteLog($"[MassEffect] Bless applied to {follower.Brain.Info.Name} (faith multiplier: {faithMultiplier:F2})");
    }

    /// <summary>
    /// Applies romance/kiss effects directly.
    /// Animation: kiss
    /// </summary>
    public static bool ApplyRomance(Follower follower, bool allowZombieReaction)
    {
        if (follower?.Brain == null) return false;

        follower.FacePosition(PlayerFarming.Instance.transform.position);
        follower.SetBodyAnimation("kiss", true);
        follower.AddBodyAnimation("idle", true, 0f);

        follower.Brain.Stats.KissedAction = true;
        follower.Brain.AddThought(Thought.SpouseKiss);

        var triggeredZombieReaction = false;
        if (allowZombieReaction && follower.Brain.Info.HasTrait(FollowerTrait.TraitType.Zombie))
        {
            triggeredZombieReaction = true;
            GameManager.GetInstance()?.StartCoroutine(PlayZombieRomanceReaction(follower));
        }

        // Handle smooch count and traits
        ++follower.Brain._directInfoAccess.SmoochCount;
        if (follower.Brain._directInfoAccess.SmoochCount >= 3)
        {
            if (follower.Brain.HasTrait(FollowerTrait.TraitType.MarriedUnhappily))
            {
                follower.Brain.RemoveTrait(FollowerTrait.TraitType.MarriedUnhappily, true);
                follower.Brain._directInfoAccess.SmoochCount = 0;
            }
            else if (!follower.Brain.HasTrait(FollowerTrait.TraitType.MarriedHappily))
            {
                follower.Brain.AddTrait(FollowerTrait.TraitType.MarriedHappily, true);
                follower.Brain._directInfoAccess.SmoochCount = 0;
            }
        }

        follower.Brain.AddAdoration(FollowerBrain.AdorationActions.SmoochSpouse, null);
        AudioManager.Instance?.PlayOneShot("event:/followers/gain_loyalty", follower.gameObject.transform.position);
        BiomeConstants.Instance?.EmitHeartPickUpVFX(follower.transform.position, 0f, "red", "burst_big", false);

        Plugin.WriteLog($"[MassEffect] Romance applied to {follower.Brain.Info.Name}");
        return triggeredZombieReaction;
    }

    /// <summary>
    /// Applies pet effects directly.
    /// Animation: pet-dog
    /// </summary>
    public static void ApplyPet(Follower follower)
    {
        if (follower?.Brain == null) return;

        follower.FacePosition(PlayerFarming.Instance.transform.position);
        follower.SetBodyAnimation("pet-dog", true);
        follower.AddBodyAnimation("idle", true, 0f);

        follower.Brain.Stats.PetDog = true;
        follower.Brain.AddAdoration(FollowerBrain.AdorationActions.PetDog, null);
        AudioManager.Instance?.PlayOneShot("event:/followers/love_hearts", follower.gameObject.transform.position);
        BiomeConstants.Instance?.EmitHeartPickUpVFX(follower.transform.position, 0f, "red", "burst_big", false);

        Plugin.WriteLog($"[MassEffect] Pet applied to {follower.Brain.Info.Name}");
    }

    /// <summary>
    /// Applies reassure effects directly.
    /// Animation: Scared/scared-reassured
    /// </summary>
    public static void ApplyReassure(Follower follower)
    {
        if (follower?.Brain == null) return;

        follower.FacePosition(PlayerFarming.Instance.transform.position);
        follower.SetBodyAnimation("Scared/scared-reassured", true);
        follower.AddBodyAnimation("idle", true, 0f);

        var info = follower.Brain._directInfoAccess;
        info.ReassuranceCount = Mathf.Clamp(++info.ReassuranceCount, 1, 3);

        if (info.ReassuranceCount >= 3)
        {
            info.ReassuranceCount = 0;
            RemoveTraitIfPresent(follower.Brain, FollowerTrait.TraitType.Terrified);
            RemoveTraitIfPresent(follower.Brain, FollowerTrait.TraitType.Scared);
            RemoveTraitIfPresent(follower.Brain, FollowerTrait.TraitType.CriminalScarred);
            RemoveTraitIfPresent(follower.Brain, FollowerTrait.TraitType.MissionaryTerrified);
            RemoveTraitIfPresent(follower.Brain, FollowerTrait.TraitType.Bastard);
            RemoveTraitIfPresent(follower.Brain, FollowerTrait.TraitType.CriminalHardened);
        }

        follower.Brain.Stats.ScaredTraitInteracted = true;
        follower.Brain.AddAdoration(FollowerBrain.AdorationActions.Reassure, null);
        AudioManager.Instance?.PlayOneShot("event:/followers/love_hearts", follower.gameObject.transform.position);
        BiomeConstants.Instance?.EmitHeartPickUpVFX(follower.transform.position, 0f, "red", "burst_big", false);

        Plugin.WriteLog($"[MassEffect] Reassure applied to {follower.Brain.Info.Name}");
    }

    /// <summary>
    /// Applies bully effects directly.
    /// Animation: Scared/scared-bullied
    /// </summary>
    public static void ApplyBully(Follower follower)
    {
        if (follower?.Brain == null) return;

        follower.FacePosition(PlayerFarming.Instance.transform.position);
        follower.SetBodyAnimation("Scared/scared-bullied", false);
        follower.AddBodyAnimation("idle", true, 0f);

        var info = follower.Brain._directInfoAccess;
        info.ReassuranceCount = Mathf.Clamp(--info.ReassuranceCount, -3, -1);

        if (!follower.Brain.HasTrait(FollowerTrait.TraitType.Terrified) && info.ReassuranceCount <= -3)
        {
            follower.Brain.AddTrait(FollowerTrait.TraitType.Terrified, true);
            info.ReassuranceCount = 0;
        }

        follower.Brain.Stats.ScaredTraitInteracted = true;
        follower.Brain.AddThought(Thought.Intimidated);
        follower.Brain.AddAdoration(FollowerBrain.AdorationActions.Bully, null);
        AudioManager.Instance?.PlayOneShot("event:/followers/gain_loyalty", follower.gameObject.transform.position);

        Plugin.WriteLog($"[MassEffect] Bully applied to {follower.Brain.Info.Name}");
    }

    /// <summary>
    /// Applies inspire/dance effects directly.
    /// Animation: dance
    /// </summary>
    public static void ApplyInspire(Follower follower)
    {
        if (follower?.Brain == null) return;

        follower.FacePosition(PlayerFarming.Instance.transform.position);
        follower.SetBodyAnimation("dance", true);
        follower.AddBodyAnimation("idle", true, 0f);

        follower.Brain.Stats.Inspired = true;
        follower.Brain.AddThought(Thought.DancedWithLeader);
        follower.Brain.AddAdoration(FollowerBrain.AdorationActions.Inspire, null);

        var faithMultiplier = MassActionCosts.GetFaithMultiplier();
        if (faithMultiplier > 0f)
        {
            CultFaithManager.AddThought(Thought.Cult_Inspire, follower.Brain.Info.ID, faithMultiplier);
        }
        AudioManager.Instance?.PlayOneShot("event:/followers/love_hearts", follower.gameObject.transform.position);

        Plugin.WriteLog($"[MassEffect] Inspire applied to {follower.Brain.Info.Name} (faith multiplier: {faithMultiplier:F2})");
    }

    /// <summary>
    /// Applies reeducate effects directly.
    /// Animation: Reactions/react-enlightened1
    /// </summary>
    public static void ApplyReeducate(Follower follower)
    {
        if (follower?.Brain == null) return;

        follower.FacePosition(PlayerFarming.Instance.transform.position);
        follower.SetBodyAnimation("Reactions/react-enlightened1", false);
        follower.AddBodyAnimation("idle", true, 0f);

        follower.Brain.Stats.Reeducation -= 7.5f;

        if (follower.Brain.Stats.Reeducation > 0f && follower.Brain.Stats.Reeducation < 4f)
        {
            follower.Brain.Stats.Reeducation = 0f;
        }

        if (follower.Brain.Info.CursedState == Thought.None)
        {
            follower.Brain.AddPleasure(FollowerBrain.PleasureActions.RemovedDissent);
        }

        follower.Brain.Stats.ReeducatedAction = true;
        AudioManager.Instance?.PlayOneShot("event:/followers/love_hearts", follower.gameObject.transform.position);
        BiomeConstants.Instance?.EmitHeartPickUpVFX(follower.transform.position, 0f, "black", "burst_big", false);

        Plugin.WriteLog($"[MassEffect] Reeducate applied to {follower.Brain.Info.Name}");
    }

    /// <summary>
    /// Applies extort effects directly.
    /// No specific animation - follower faces player, gold flies out.
    /// </summary>
    public static void ApplyExtort(Follower follower)
    {
        if (follower?.Brain == null) return;

        follower.FacePosition(PlayerFarming.Instance.transform.position);

        var goldAmount = Random.Range(3, 7);
        follower.Brain.Stats.PaidTithes = true;

        // Spawn gold flying to player
        for (var i = 0; i < goldAmount; i++)
        {
            ResourceCustomTarget.Create(
                PlayerFarming.Instance.gameObject,
                follower.transform.position,
                InventoryItem.ITEM_TYPE.BLACK_GOLD,
                () => Inventory.AddItem(20, 1)
            );
        }

        AudioManager.Instance?.PlayOneShot("event:/followers/pop_in", follower.transform.position);

        Plugin.WriteLog($"[MassEffect] Extort applied to {follower.Brain.Info.Name} (+{goldAmount} gold)");
    }

    private static void RemoveTraitIfPresent(FollowerBrain brain, FollowerTrait.TraitType trait)
    {
        if (brain.HasTrait(trait))
        {
            brain.RemoveTrait(trait, true);
        }
    }

    private static IEnumerator PlayZombieRomanceReaction(Follower follower)
    {
        if (follower == null) yield break;

        var player = PlayerFarming.Instance;
        if (player?.Spine?.AnimationState != null)
        {
            player.Spine.AnimationState.SetAnimation(0, "eat-react-bad", false);
            player.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
        }

        yield return new WaitForSeconds(0.23333334f);
        AudioManager.Instance?.PlayOneShot("event:/dialogue/followers/hold_back_vom", follower.gameObject);
        yield return new WaitForSeconds(0.73333335f);
        AudioManager.Instance?.PlayOneShot("event:/dialogue/followers/vom", follower.gameObject);
        yield return new WaitForSeconds(2f);
    }

    /// <summary>
    /// Checks if a follower is imprisoned using proper enum values.
    /// </summary>
    public static bool IsImprisoned(FollowerBrain brain)
    {
        return brain.Info.CursedState is
            Thought.InnocentImprisoned or
            Thought.ImprisonedLibertarian or
            Thought.InnocentImprisonedDisciplinarian or
            Thought.DissenterImprisoned or
            Thought.InnocentImprisonedSleeping or
            Thought.DissenterImprisonedSleeping or
            Thought.PrisonReEducation;
    }

    /// <summary>
    /// Checks if a follower is dissenting.
    /// </summary>
    public static bool IsDissenting(FollowerBrain brain)
    {
        return brain.Info.CursedState == Thought.Dissenter;
    }

    /// <summary>
    /// Checks if a follower is available (not sleeping/mating).
    /// </summary>
    public static bool IsAvailable(FollowerBrain brain)
    {
        return brain.CurrentTaskType is not (FollowerTaskType.Sleep or FollowerTaskType.SleepBedRest or FollowerTaskType.Mating);
    }

    /// <summary>
    /// Applies level-up effects directly.
    /// Animation: devotion/devotion-start -> Reactions/react-bow -> idle
    /// Spawns souls or gold based on game progression.
    /// </summary>
    public static void ApplyLevelUp(Follower follower, FollowerTaskType previousTask, interaction_FollowerInteraction interaction = null)
    {
        if (follower?.Brain == null) return;

        var player = PlayerFarming.Instance;
        if (player == null) return;

        // Verify follower can actually level up
        if (!follower.Brain.CanLevelUp())
        {
            Plugin.WriteLog($"[MassEffect] {follower.Brain.Info.Name} cannot level up (Adoration: {follower.Brain.Stats.Adoration}/{follower.Brain.Stats.MAX_ADORATION})");
            return;
        }

        follower.FacePosition(player.transform.position);
        follower.SetBodyAnimation("devotion/devotion-start", false);
        follower.AddBodyAnimation("Reactions/react-bow", false, 0f);
        follower.AddBodyAnimation("idle", true, 0f);

        // Store previous level for logging
        var previousLevel = follower.Brain.Info.XPLevel;

        // Reset adoration and increment level (same as vanilla LevelUpRoutine lines 951-952)
        follower.Brain.Stats.Adoration = 0f;
        ++follower.Brain.Info.XPLevel;

        // Sync the UI bar to reflect the adoration reset (vanilla does this via BarController.ShrinkBarToEmpty)
        follower.AdorationUI?.BarController?.SetBarSize(0f, false, true);
        follower.AdorationUI?.SetObjects();

        // Update level text to show the new level (SetObjects doesn't touch levelText)
        if (follower.AdorationUI?.levelText != null)
        {
            follower.AdorationUI.levelText.text = follower.Brain.Info.XPLevel.ToNumeral();
        }

        // Match vanilla: trigger rewards callback if present
        interaction?.OnGivenRewards?.Invoke();

        AudioManager.Instance?.PlayOneShot("event:/player/float_follower", follower.gameObject);

        // Handle BornToTheRot trait - gives magma stones instead of souls
        if (follower.Brain.HasTrait(FollowerTrait.TraitType.BornToTheRot))
        {
            var rotAmount = Random.Range(5, 10);
            for (var i = 0; i < rotAmount; i++)
            {
                ResourceCustomTarget.Create(player.gameObject, follower.transform.position, InventoryItem.ITEM_TYPE.MAGMA_STONE, null);
                Inventory.ChangeItemQuantity(172, 1); // MAGMA_STONE
            }
        }
        else
        {
            // Spawn souls or gold based on progression
            var spawnSouls = GameManager.HasUnlockAvailable() || DataManager.Instance.DeathCatBeaten;
            for (var i = 0; i < 20; i++)
            {
                if (spawnSouls)
                {
                    SoulCustomTarget.Create(player.CameraBone, follower.CameraBone.transform.position, Color.white, () => player.GetSoul(1));
                }
                else
                {
                    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, follower.transform.position + Vector3.back, 0f)
                        .SetInitialSpeedAndDiraction(8f + Random.Range(-0.5f, 1f), 270 + Random.Range(-90, 90));
                }
            }
        }

        // Complete current task
        follower.Brain.CompleteCurrentTask();

        // Reset wake-up day if was sleeping
        if (previousTask == FollowerTaskType.Sleep)
        {
            follower.Brain._directInfoAccess.WakeUpDay = -1;
        }

        // Doctrine stone drop (vanilla LevelUpRoutine behavior)
        if (follower.Brain.Info.CursedState != Thought.Child &&
            DoctrineUpgradeSystem.TryGetStillDoctrineStone() &&
            DataManager.Instance.GetVariable(DataManager.Variables.FirstDoctrineStone))
        {
            switch (++DataManager.Instance.SpaceOutDoctrineStones % 3)
            {
                case 0:
                    var pickUp = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.DOCTRINE_STONE, 1, follower.transform.position);
                    if (pickUp != null)
                    {
                        var component = pickUp.GetComponent<Interaction_DoctrineStone>();
                        if (component != null)
                        {
                            component.AutomaticallyInteract = true;
                            component.MagnetToPlayer(player.gameObject);
                        }
                    }
                    break;
            }
        }

        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.LoyaltyCollectReward);

        Plugin.WriteLog($"[MassEffect] LevelUp applied to {follower.Brain.Info.Name} (level {previousLevel} -> {follower.Brain.Info.XPLevel}, adoration now {follower.Brain.Stats.Adoration})");
    }
}
