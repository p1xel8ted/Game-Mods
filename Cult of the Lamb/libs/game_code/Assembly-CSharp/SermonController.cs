// Decompiled with JetBrains decompiler
// Type: SermonController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using Lamb.UI;
using src.Extensions;
using src.Rituals;
using src.UI.Overlays.SermonXPOverlay;
using src.UI.Overlays.TutorialOverlay;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unify;
using UnityEngine;

#nullable disable
public class SermonController : MonoBehaviour
{
  public static Action<bool> OnEnd;
  public static Action<bool> OnBegin;
  public StateMachine state;
  public EventInstance sermonLoop;
  public Interaction_TempleAltar TempleAltar;
  public UIDoctrineBar UIDoctrineBar;
  public float barLocalXP;
  public bool skipDevotionGathering;
  public SermonCategory SermonCategory = SermonCategory.PlayerUpgrade;
  public UIHeartsOfTheFaithfulChoiceMenuController _heartsOfTheFaithfulMenuTemplate;

  public void Play(StateMachine state)
  {
    this.state = state;
    this.TempleAltar = this.GetComponent<Interaction_TempleAltar>();
    this.StartCoroutine((IEnumerator) this.TeachSermonRoutine());
  }

  public IEnumerator TeachSermonRoutine()
  {
    SermonController sermonController = this;
    Action<bool> onBegin = SermonController.OnBegin;
    if (onBegin != null)
      onBegin(false);
    sermonController.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    PlayerFarming.Instance.simpleSpineAnimator.Animate("build", 0, true);
    AudioManager.Instance.PlayOneShot("event:/sermon/start_sermon", PlayerFarming.Instance.gameObject);
    AudioManager.Instance.PlayOneShot("event:/building/building_bell_ring", PlayerFarming.Instance.gameObject);
    sermonController.StartCoroutine((IEnumerator) sermonController.TempleAltar.CentrePlayer());
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 12f);
    yield return (object) sermonController.StartCoroutine((IEnumerator) sermonController.TempleAltar.FollowersEnterForSermonRoutine());
    SimulationManager.Pause();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 7f);
    GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, -0.5f));
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, 0.0f));
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 8f);
    PlayerFarming.Instance.Spine.skeleton.FindBone("ritualring").Rotation += 60f;
    PlayerFarming.Instance.Spine.skeleton.UpdateWorldTransform();
    PlayerFarming.Instance.Spine.skeleton.Update(Time.deltaTime);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("sermons/sermon-start", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("sermons/sermon-loop", 0, true, 0.0f);
    sermonController.sermonLoop = AudioManager.Instance.CreateLoop("event:/sermon/preach_loop", PlayerFarming.Instance.gameObject, true, false);
    yield return (object) new WaitForSeconds(0.6f);
    sermonController.TempleAltar.PulseDisplacementObject(sermonController.state.transform.position);
    yield return (object) new WaitForSeconds(0.4f);
    ChurchFollowerManager.Instance.StartSermonEffect();
    bool flag = GameManager.GetInstance().UpgradePlayerConfiguration.HasUnlockAvailable(true);
    if (!flag)
      flag = DataManager.Instance.MAJOR_DLC && DataManager.Instance.InteractedDLCShrine && !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Major_DLC_Sermon_Packs);
    if (flag)
    {
      GameManager.GetInstance().CamFollowTarget.SetOffset(new Vector3(0.0f, -4f, 0.0f));
      yield return (object) new WaitForSeconds(0.4f);
      UISermonXPOverlayController sermonXpOverlayController = MonoSingleton<UIManager>.Instance.SermonXPOverlayTemplate.Instantiate<UISermonXPOverlayController>();
      sermonXpOverlayController.Show(Ritual.FollowerToAttendSermon, true);
      foreach (SermonXPItem allItem in sermonXpOverlayController.GetAllItems())
      {
        allItem.Show();
        yield return (object) new WaitForSeconds(0.025f);
      }
      yield return (object) new WaitForSeconds(0.6f);
      GameManager.GetInstance().CameraSetZoom(10f);
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(sermonController.TempleAltar.DoctrineXPPrefab, GameObject.FindWithTag("Canvas").transform);
      sermonController.UIDoctrineBar = gameObject.GetComponent<UIDoctrineBar>();
      float xp = DoctrineUpgradeSystem.GetXPBySermon(sermonController.SermonCategory);
      float target = DoctrineUpgradeSystem.GetXPTargetBySermon(sermonController.SermonCategory);
      yield return (object) sermonController.StartCoroutine((IEnumerator) sermonController.UIDoctrineBar.Show(xp, sermonController.SermonCategory));
      GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 11f);
      UITutorialOverlayController TutorialOverlay;
      if (Mathf.RoundToInt(xp * 10f) >= Mathf.RoundToInt(target * 10f))
      {
        yield return (object) sermonController.StartCoroutine((IEnumerator) sermonController.UIDoctrineBar.UpdateSecondBar(xp, 0.5f));
        sermonController.StartCoroutine((IEnumerator) sermonController.UIDoctrineBar.FlashBarRoutine(0.3f, 1f));
        yield return (object) new WaitForSeconds(0.5f);
        yield return (object) sermonController.StartCoroutine((IEnumerator) sermonController.UIDoctrineBar.Hide());
        TutorialOverlay = (UITutorialOverlayController) null;
        if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.PlayerLevelUp))
          TutorialOverlay = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.PlayerLevelUp);
        while ((UnityEngine.Object) TutorialOverlay != (UnityEngine.Object) null)
          yield return (object) null;
        yield return (object) sermonController.StartCoroutine((IEnumerator) sermonController.PlayerUpgrade());
        xp = 0.0f;
        target = DoctrineUpgradeSystem.GetXPTargetBySermon(sermonController.SermonCategory);
        TutorialOverlay = (UITutorialOverlayController) null;
      }
      int followersCount = Ritual.FollowerToAttendSermon.Count;
      float delay = 1.5f / (float) followersCount;
      sermonController.barLocalXP = xp;
      int xpGained = 0;
      int count = 0;
      LetterBox.Instance.ShowSkipPrompt();
      Coroutine skipRoutine = sermonController.StartCoroutine((IEnumerator) sermonController.WaitForSkip());
      while (GameManager.GetInstance().UpgradePlayerConfiguration.HasUnlockAvailable(true) || DataManager.Instance.MAJOR_DLC && DataManager.Instance.InteractedDLCShrine && !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Major_DLC_Sermon_Packs))
      {
        int level = Mathf.Clamp(Ritual.FollowerToAttendSermon[count].Info.XPLevel, 1, 5);
        xp += 0.1f * (float) level;
        int i = -1;
        while (++i < level)
        {
          if (!sermonController.skipDevotionGathering)
          {
            SoulCustomTarget.Create(PlayerFarming.Instance.CrownBone.gameObject, Ritual.FollowerToAttendSermon[count].LastPosition, Color.white, (System.Action) (() =>
            {
              this.IncrementXPBar();
              ++xpGained;
              this.UIDoctrineBar.SetIncreaseCounter(xpGained);
            }), 0.2f, 500f);
          }
          else
          {
            sermonController.IncrementXPBar();
            ++xpGained;
          }
          sermonXpOverlayController.UpdateXPItem(Ritual.FollowerToAttendSermon[count], level - i - 1);
          if (!sermonController.skipDevotionGathering)
            yield return (object) new WaitForSeconds(0.15f);
        }
        if (sermonController.skipDevotionGathering)
        {
          SoulCustomTarget.Create(PlayerFarming.Instance.CrownBone.gameObject, Ritual.FollowerToAttendSermon[count].LastPosition, Color.white, (System.Action) null, 0.2f, 500f);
          sermonController.UIDoctrineBar.SetIncreaseCounter(xpGained);
        }
        yield return (object) new WaitForSeconds(delay);
        if (Mathf.RoundToInt(xp * 10f) >= Mathf.RoundToInt(target * 10f))
        {
          yield return (object) sermonController.StartCoroutine((IEnumerator) sermonController.UIDoctrineBar.UpdateSecondBar(xp, 0.5f));
          sermonController.StartCoroutine((IEnumerator) sermonController.UIDoctrineBar.FlashBarRoutine(0.3f, 1f));
          yield return (object) new WaitForSeconds(0.5f);
          yield return (object) sermonController.StartCoroutine((IEnumerator) sermonController.UIDoctrineBar.Hide());
          TutorialOverlay = (UITutorialOverlayController) null;
          if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.PlayerLevelUp))
            TutorialOverlay = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.PlayerLevelUp);
          while ((UnityEngine.Object) TutorialOverlay != (UnityEngine.Object) null)
            yield return (object) null;
          yield return (object) sermonController.StartCoroutine((IEnumerator) sermonController.PlayerUpgrade());
          xp = 0.0f;
          if (count < followersCount - 1 && GameManager.GetInstance().UpgradePlayerConfiguration.HasUnlockAvailable(true))
          {
            yield return (object) sermonController.StartCoroutine((IEnumerator) sermonController.UIDoctrineBar.Show(0.0f, sermonController.SermonCategory));
            sermonController.barLocalXP = 0.0f;
          }
          else
            sermonController.StartCoroutine((IEnumerator) sermonController.UIDoctrineBar.Hide());
          target = DoctrineUpgradeSystem.GetXPTargetBySermon(sermonController.SermonCategory);
          TutorialOverlay = (UITutorialOverlayController) null;
        }
        ++count;
        if (count >= followersCount)
          break;
      }
      sermonController.StopCoroutine(skipRoutine);
      LetterBox.Instance.HideSkipPrompt();
      ChurchFollowerManager.Instance.EndSermonEffect();
      yield return (object) new WaitForSeconds(0.5f);
      yield return (object) sermonController.StartCoroutine((IEnumerator) sermonController.UIDoctrineBar.UpdateSecondBar(xp, 0.5f));
      yield return (object) new WaitForSeconds(0.5f);
      yield return (object) sermonController.StartCoroutine((IEnumerator) sermonController.UIDoctrineBar.Hide());
      DoctrineUpgradeSystem.SetXPBySermon(sermonController.SermonCategory, xp);
      sermonXpOverlayController.Hide();
      UnityEngine.Object.Destroy((UnityEngine.Object) sermonController.UIDoctrineBar.gameObject);
      sermonXpOverlayController = (UISermonXPOverlayController) null;
      skipRoutine = (Coroutine) null;
    }
    else
    {
      yield return (object) new WaitForSeconds(2f);
      for (int index = 0; index < PlayerFarming.players.Count; ++index)
      {
        Interaction_TempleAltar.Instance.PulseDisplacementObject(PlayerFarming.players[index].transform.position);
        BiomeConstants.Instance.EmitHeartPickUpVFX(PlayerFarming.players[index].CameraBone.transform.position, 0.0f, "blue", "burst_big");
        PlayerFarming.players[index].health.BlueHearts += 2f;
        if (PlayerFarming.players[index].isLamb)
          DataManager.Instance.PLAYER_BLUE_HEARTS += 2f;
        else
          DataManager.Instance.COOP_PLAYER_BLUE_HEARTS += 2f;
      }
      yield return (object) new WaitForSeconds(1f);
      ChurchFollowerManager.Instance.EndSermonEffect();
    }
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().CameraSetZoom(8f);
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    AchievementsWrapper.UnlockAchievement(Achievements.Instance.Lookup("DELIVER_FIRST_SERMON"));
    PlayerFarming.Instance.simpleSpineAnimator.Animate("sermons/sermon-stop", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/sermon/end_sermon", PlayerFarming.Instance.gameObject);
    int num = (int) sermonController.sermonLoop.stop(STOP_MODE.ALLOWFADEOUT);
    AudioManager.Instance.StopLoop(sermonController.sermonLoop);
    yield return (object) new WaitForSeconds(0.333333343f);
    sermonController.TempleAltar.ResetSprite();
    AudioManager.Instance.PlayOneShot("event:/sermon/book_put_down", PlayerFarming.Instance.gameObject);
    DataManager.Instance.PreviousSermonDayIndex = TimeManager.CurrentDay;
    PlayerFarming.Instance.Spine.UseDeltaTime = true;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTaskType == FollowerTaskType.AttendTeaching)
      {
        if (allBrain.HasTrait(FollowerTrait.TraitType.SermonEnthusiast))
          allBrain.AddThought(Thought.WatchedSermonDevotee);
        else
          allBrain.AddThought(Thought.WatchedSermon);
        Follower f = FollowerManager.FindFollowerByID(allBrain.Info.ID);
        if ((UnityEngine.Object) f != (UnityEngine.Object) null)
          allBrain.AddAdoration(FollowerBrain.AdorationActions.Sermon, (System.Action) (() =>
          {
            if (f.Brain.CurrentTask == null || !(f.Brain.CurrentTask is FollowerTask_AttendTeaching))
              return;
            f.Brain.CurrentTask.StartAgain(f);
          }));
        sermonController.StartCoroutine((IEnumerator) sermonController.TempleAltar.DelayFollowerReaction(allBrain, UnityEngine.Random.Range(0.1f, 0.5f)));
      }
    }
    yield return (object) sermonController.StartCoroutine((IEnumerator) sermonController.AwaitDoctrine());
    DataManager.Instance.PreviousSermonCategory = sermonController.SermonCategory;
    sermonController.TempleAltar.ResetSprite();
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.GiveSermon);
    sermonController.TempleAltar.Activated = false;
    if (DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.SermonEnthusiast))
    {
      CultFaithManager.AddThought(Thought.Cult_SermonEnthusiast_Trait);
      FaithBarFake.Play(FollowerThoughts.GetData(Thought.Cult_SermonEnthusiast_Trait).Modifier);
    }
    else
    {
      CultFaithManager.AddThought(Thought.Cult_Sermon);
      FaithBarFake.Play(FollowerThoughts.GetData(Thought.Cult_Sermon).Modifier);
    }
    if (DataManager.Instance.WokeUpEveryoneDay == TimeManager.CurrentDay && TimeManager.CurrentPhase == DayPhase.Night && FollowerBrainStats.ShouldSleep)
      CultFaithManager.AddThought(Thought.Cult_WokeUpEveryone);
    yield return (object) new WaitForSeconds(1f);
    Action<bool> onEnd = SermonController.OnEnd;
    if (onEnd != null)
      onEnd(false);
    Interaction_TempleAltar.Instance.OnInteract(PlayerFarming.Instance.state);
  }

  public IEnumerator AwaitDoctrine()
  {
    while (this.IsPickUpDoctrineActive())
      yield return (object) null;
  }

  public bool IsPickUpDoctrineActive()
  {
    foreach (PickUp pickUp in PickUp.PickUps)
    {
      if (pickUp.isActiveAndEnabled && pickUp.type == InventoryItem.ITEM_TYPE.DOCTRINE_STONE)
        return true;
    }
    return false;
  }

  public void IncrementXPBar()
  {
    this.barLocalXP += 0.1f;
    this.StartCoroutine((IEnumerator) this.UIDoctrineBar.UpdateFirstBar(this.barLocalXP, 0.1f));
  }

  public void IncrementCustomXPBar(float value)
  {
    this.barLocalXP += value;
    this.StartCoroutine((IEnumerator) this.UIDoctrineBar.UpdateFirstBar(this.barLocalXP, 0.1f));
  }

  public IEnumerator SacrificeLevelUp(int amount, System.Action Callback)
  {
    SermonController sermonController = this;
    float xp = DoctrineUpgradeSystem.GetXPBySermon(sermonController.SermonCategory);
    double xpTargetBySermon = (double) DoctrineUpgradeSystem.GetXPTargetBySermon(sermonController.SermonCategory);
    sermonController.barLocalXP = xp;
    sermonController.TempleAltar = Interaction_TempleAltar.Instance;
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(sermonController.TempleAltar.DoctrineXPPrefab, GameObject.FindWithTag("Canvas").transform);
    sermonController.UIDoctrineBar = gameObject.GetComponent<UIDoctrineBar>();
    yield return (object) sermonController.StartCoroutine((IEnumerator) sermonController.UIDoctrineBar.Show(xp, sermonController.SermonCategory));
    float num1 = DoctrineUpgradeSystem.GetXPBySermon(sermonController.SermonCategory) * 10f;
    float increment = (float) (2.0 / ((double) (DoctrineUpgradeSystem.GetXPTargetBySermon(sermonController.SermonCategory) * 10f) - (double) num1));
    while (amount > 0)
    {
      SoulCustomTarget.Create(PlayerFarming.Instance.CrownBone.gameObject, Interaction_TempleAltar.Instance.PortalEffect.transform.position, Color.white, new System.Action(sermonController.\u003CSacrificeLevelUp\u003Eb__16_0), 0.2f, 500f);
      xp += 0.1f;
      --amount;
      if (Mathf.RoundToInt(xp * 10f) < Mathf.RoundToInt(DoctrineUpgradeSystem.GetXPTargetBySermon(sermonController.SermonCategory) * 10f))
        yield return (object) new WaitForSeconds(increment);
      else
        break;
    }
    if (Mathf.RoundToInt(xp * 10f) >= Mathf.RoundToInt(DoctrineUpgradeSystem.GetXPTargetBySermon(sermonController.SermonCategory) * 10f))
    {
      yield return (object) sermonController.StartCoroutine((IEnumerator) sermonController.UIDoctrineBar.UpdateSecondBar(xp, 0.5f));
      yield return (object) new WaitForSeconds(0.5f);
      sermonController.StartCoroutine((IEnumerator) sermonController.UIDoctrineBar.FlashBarRoutine(0.3f, 1f));
      yield return (object) new WaitForSeconds(0.5f);
      xp = 0.0f;
      DoctrineUpgradeSystem.SetXPBySermon(sermonController.SermonCategory, xp);
      yield return (object) sermonController.StartCoroutine((IEnumerator) sermonController.UIDoctrineBar.Hide());
      yield return (object) sermonController.StartCoroutine((IEnumerator) sermonController.PlayerUpgrade());
    }
    else
      DoctrineUpgradeSystem.SetXPBySermon(sermonController.SermonCategory, xp);
    if (amount <= 0 || !GameManager.GetInstance().UpgradePlayerConfiguration.HasUnlockAvailable(true))
    {
      yield return (object) sermonController.StartCoroutine((IEnumerator) sermonController.UIDoctrineBar.UpdateSecondBar(xp, 0.5f));
      yield return (object) new WaitForSeconds(0.5f);
      sermonController.StartCoroutine((IEnumerator) sermonController.UIDoctrineBar.Hide());
      Debug.Log((object) "End now don't give any more souls!");
      System.Action action = Callback;
      if (action != null)
        action();
    }
    else
    {
      yield return (object) sermonController.StartCoroutine((IEnumerator) sermonController.UIDoctrineBar.Show(0.0f, sermonController.SermonCategory));
      sermonController.barLocalXP = 0.0f;
      if (GameManager.GetInstance().UpgradePlayerConfiguration.HasUnlockAvailable(true))
      {
        float num2 = DoctrineUpgradeSystem.GetXPBySermon(sermonController.SermonCategory) * 10f;
        increment = (float) (2.0 / ((double) (DoctrineUpgradeSystem.GetXPTargetBySermon(sermonController.SermonCategory) * 10f) - (double) num2));
        while (amount > 0)
        {
          SoulCustomTarget.Create(PlayerFarming.Instance.CrownBone.gameObject, Interaction_TempleAltar.Instance.PortalEffect.transform.position, Color.white, new System.Action(sermonController.\u003CSacrificeLevelUp\u003Eb__16_1), 0.2f, 500f);
          xp += 0.1f;
          --amount;
          if (Mathf.RoundToInt(xp * 10f) < Mathf.RoundToInt(DoctrineUpgradeSystem.GetXPTargetBySermon(sermonController.SermonCategory) * 10f))
            yield return (object) new WaitForSeconds(increment);
          else
            break;
        }
        yield return (object) sermonController.StartCoroutine((IEnumerator) sermonController.UIDoctrineBar.UpdateSecondBar(xp, 0.5f));
        yield return (object) new WaitForSeconds(0.5f);
        DoctrineUpgradeSystem.SetXPBySermon(sermonController.SermonCategory, xp);
        yield return (object) sermonController.StartCoroutine((IEnumerator) sermonController.UIDoctrineBar.Hide());
      }
      System.Action action = Callback;
      if (action != null)
        action();
    }
  }

  public IEnumerator PlayerUpgrade()
  {
    SermonController sermonController = this;
    UpgradeSystem.DisciplePoints = 1;
    UIHeartsOfTheFaithfulChoiceMenuController.Types upgradeType = UIHeartsOfTheFaithfulChoiceMenuController.Types.Hearts;
    UpgradeSystem.Type upgrade = UpgradeSystem.Type.Count;
    UIUpgradePlayerTreeMenuController menu1 = MonoSingleton<UIManager>.Instance.ShowPlayerUpgradeTree();
    UIUpgradePlayerTreeMenuController treeMenuController = menu1;
    treeMenuController.OnUpgradeUnlocked = treeMenuController.OnUpgradeUnlocked + (Action<UpgradeSystem.Type>) (type =>
    {
      upgrade = type;
      upgradeType = RitualFlockOfTheFaithful.GetUpgradeType(type);
    });
    yield return (object) menu1.YieldUntilHidden();
    yield return (object) new WaitForSecondsRealtime(0.25f);
    if (sermonController.WasRelicUpgrade(upgrade))
    {
      Time.timeScale = 0.0f;
      UIRelicMenuController menu2 = MonoSingleton<UIManager>.Instance.RelicMenuTemplate.Instantiate<UIRelicMenuController>();
      menu2.Show(EquipmentManager.GetRelicsForUpgradeType(upgrade));
      yield return (object) menu2.YieldUntilHidden();
      Time.timeScale = 1f;
    }
    UpgradeSystem.DisciplePoints = 0;
    ++DataManager.Instance.Doctrine_PlayerUpgrade_Level;
    yield return (object) sermonController.StartCoroutine((IEnumerator) sermonController.EmitParticles(upgradeType));
  }

  public IEnumerator WaitForSkip()
  {
    while (true)
    {
      if (InputManager.Gameplay.GetAttackButtonDown())
      {
        this.skipDevotionGathering = true;
        LetterBox.Instance.HideSkipPrompt();
      }
      yield return (object) null;
    }
  }

  public bool WasRelicUpgrade(UpgradeSystem.Type upgrade)
  {
    switch (upgrade)
    {
      case UpgradeSystem.Type.Relics_Blessed_1:
      case UpgradeSystem.Type.Relic_Pack1:
      case UpgradeSystem.Type.Relics_Dammed_1:
      case UpgradeSystem.Type.Relic_Pack2:
      case UpgradeSystem.Type.Relic_Pack_Default:
      case UpgradeSystem.Type.Relics_Fire:
      case UpgradeSystem.Type.Relics_Ice:
        return true;
      default:
        return false;
    }
  }

  public IEnumerator EmitParticles(
    UIHeartsOfTheFaithfulChoiceMenuController.Types chosenUpgrade)
  {
    SermonController sermonController = this;
    yield return (object) new WaitForSeconds(0.2f);
    int Loops = 1;
    float Delay = 0.0f;
    while (--Loops >= 0)
    {
      Interaction_TempleAltar.Instance.PulseDisplacementObject(PlayerFarming.Instance.transform.position);
      CameraManager.instance.ShakeCameraForDuration(0.8f, 1f, 0.2f);
      GameManager.GetInstance().CamFollowTarget.targetDistance += 0.2f;
      switch (chosenUpgrade)
      {
        case UIHeartsOfTheFaithfulChoiceMenuController.Types.Hearts:
          AudioManager.Instance.PlayOneShot("event:/hearts_of_the_faithful/hearts_appear", sermonController.gameObject);
          AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", sermonController.gameObject.transform.position);
          BiomeConstants.Instance.EmitHeartPickUpVFX(PlayerFarming.Instance.CameraBone.transform.position, 0.0f, "red", "burst_big");
          break;
        case UIHeartsOfTheFaithfulChoiceMenuController.Types.Strength:
          AudioManager.Instance.PlayOneShot("event:/hearts_of_the_faithful/swords_appear", sermonController.gameObject);
          BiomeConstants.Instance.EmitHeartPickUpVFX(PlayerFarming.Instance.CameraBone.transform.position, 0.0f, "strength", "strength");
          break;
      }
      Delay += 0.1f;
      yield return (object) new WaitForSeconds(0.8f - Delay);
    }
  }

  public void OnDestroy() => AudioManager.Instance.StopLoop(this.sermonLoop);

  [CompilerGenerated]
  public void \u003CSacrificeLevelUp\u003Eb__16_0() => this.IncrementXPBar();

  [CompilerGenerated]
  public void \u003CSacrificeLevelUp\u003Eb__16_1() => this.IncrementXPBar();
}
