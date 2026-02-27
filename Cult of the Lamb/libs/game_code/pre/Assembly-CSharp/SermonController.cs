// Decompiled with JetBrains decompiler
// Type: SermonController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using Lamb.UI;
using src.Rituals;
using src.UI.Overlays.TutorialOverlay;
using System;
using System.Collections;
using Unify;
using UnityEngine;

#nullable disable
public class SermonController : MonoBehaviour
{
  private StateMachine state;
  private EventInstance sermonLoop;
  private Interaction_TempleAltar TempleAltar;
  private UIDoctrineBar UIDoctrineBar;
  private float barLocalXP;
  private SermonCategory SermonCategory = SermonCategory.PlayerUpgrade;
  private UIHeartsOfTheFaithfulChoiceMenuController _heartsOfTheFaithfulMenuTemplate;

  public void Play(StateMachine state)
  {
    this.state = state;
    this.TempleAltar = this.GetComponent<Interaction_TempleAltar>();
    this.StartCoroutine((IEnumerator) this.TeachSermonRoutine());
  }

  private IEnumerator TeachSermonRoutine()
  {
    SermonController sermonController = this;
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
    if (GameManager.GetInstance().UpgradePlayerConfiguration.HasUnlockAvailable())
    {
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(sermonController.TempleAltar.DoctrineXPPrefab, GameObject.FindWithTag("Canvas").transform);
      sermonController.UIDoctrineBar = gameObject.GetComponent<UIDoctrineBar>();
      float xp = DoctrineUpgradeSystem.GetXPBySermon(sermonController.SermonCategory);
      float target = DoctrineUpgradeSystem.GetXPTargetBySermon(sermonController.SermonCategory);
      float num = 1.5f - (target - xp);
      yield return (object) sermonController.StartCoroutine((IEnumerator) sermonController.UIDoctrineBar.Show(xp, sermonController.SermonCategory));
      GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 11f);
      int followersCount = Ritual.FollowerToAttendSermon.Count;
      float delay = 1.5f / (float) followersCount;
      sermonController.barLocalXP = xp;
      int count = 0;
      while (GameManager.GetInstance().UpgradePlayerConfiguration.HasUnlockAvailable())
      {
        xp += 0.1f * (float) Ritual.FollowerToAttendSermon[count].Info.XPLevel;
        target = DoctrineUpgradeSystem.GetXPTargetBySermon(sermonController.SermonCategory);
        int i = -1;
        while (++i < Ritual.FollowerToAttendSermon[count].Info.XPLevel)
        {
          // ISSUE: reference to a compiler-generated method
          SoulCustomTarget.Create(PlayerFarming.Instance.CrownBone.gameObject, Ritual.FollowerToAttendSermon[count].LastPosition, Color.white, new System.Action(sermonController.\u003CTeachSermonRoutine\u003Eb__7_0), 0.2f, 500f);
          yield return (object) new WaitForSeconds(0.1f);
        }
        yield return (object) new WaitForSeconds(delay);
        if (Mathf.RoundToInt(xp * 10f) >= Mathf.RoundToInt(target * 10f))
        {
          yield return (object) sermonController.StartCoroutine((IEnumerator) sermonController.UIDoctrineBar.UpdateSecondBar(xp, 0.5f));
          sermonController.StartCoroutine((IEnumerator) sermonController.UIDoctrineBar.FlashBarRoutine(0.3f, 1f));
          yield return (object) new WaitForSeconds(0.5f);
          yield return (object) sermonController.StartCoroutine((IEnumerator) sermonController.UIDoctrineBar.Hide());
          UITutorialOverlayController TutorialOverlay = (UITutorialOverlayController) null;
          if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.PlayerLevelUp))
            TutorialOverlay = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.PlayerLevelUp);
          while ((UnityEngine.Object) TutorialOverlay != (UnityEngine.Object) null)
            yield return (object) null;
          yield return (object) sermonController.StartCoroutine((IEnumerator) sermonController.PlayerUpgrade());
          xp = 0.0f;
          if (count < followersCount - 1 && GameManager.GetInstance().UpgradePlayerConfiguration.HasUnlockAvailable())
          {
            yield return (object) sermonController.StartCoroutine((IEnumerator) sermonController.UIDoctrineBar.Show(0.0f, sermonController.SermonCategory));
            sermonController.barLocalXP = 0.0f;
          }
          else
            sermonController.StartCoroutine((IEnumerator) sermonController.UIDoctrineBar.Hide());
          TutorialOverlay = (UITutorialOverlayController) null;
        }
        ++count;
        if (count >= followersCount)
          break;
      }
      ChurchFollowerManager.Instance.EndSermonEffect();
      yield return (object) new WaitForSeconds(0.5f);
      yield return (object) sermonController.StartCoroutine((IEnumerator) sermonController.UIDoctrineBar.UpdateSecondBar(xp, 0.5f));
      yield return (object) new WaitForSeconds(0.5f);
      yield return (object) sermonController.StartCoroutine((IEnumerator) sermonController.UIDoctrineBar.Hide());
      DoctrineUpgradeSystem.SetXPBySermon(sermonController.SermonCategory, xp);
      UnityEngine.Object.Destroy((UnityEngine.Object) sermonController.UIDoctrineBar.gameObject);
    }
    else
    {
      yield return (object) new WaitForSeconds(2f);
      Interaction_TempleAltar.Instance.PulseDisplacementObject(PlayerFarming.Instance.transform.position);
      BiomeConstants.Instance.EmitHeartPickUpVFX(PlayerFarming.Instance.CameraBone.transform.position, 0.0f, "blue", "burst_big");
      PlayerFarming.Instance.health.BlueHearts += 2f;
      yield return (object) new WaitForSeconds(1f);
      ChurchFollowerManager.Instance.EndSermonEffect();
    }
    yield return (object) new WaitForSeconds(0.5f);
    AchievementsWrapper.UnlockAchievement(Achievements.Instance.Lookup("DELIVER_FIRST_SERMON"));
    PlayerFarming.Instance.simpleSpineAnimator.Animate("sermons/sermon-stop", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/sermon/end_sermon", PlayerFarming.Instance.gameObject);
    int num1 = (int) sermonController.sermonLoop.stop(STOP_MODE.ALLOWFADEOUT);
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
        {
          allBrain.GetWillLevelUp(FollowerBrain.AdorationActions.Sermon);
          allBrain.AddAdoration(FollowerBrain.AdorationActions.Sermon, (System.Action) (() =>
          {
            if (f.Brain.CurrentTask == null || !(f.Brain.CurrentTask is FollowerTask_AttendTeaching))
              return;
            f.Brain.CurrentTask.StartAgain(f);
          }));
        }
        sermonController.StartCoroutine((IEnumerator) sermonController.TempleAltar.DelayFollowerReaction(allBrain, UnityEngine.Random.Range(0.1f, 0.5f)));
      }
    }
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
    if (DataManager.Instance.WokeUpEveryoneDay == TimeManager.CurrentDay && TimeManager.CurrentPhase == DayPhase.Night && !FollowerBrainStats.IsWorkThroughTheNight)
      CultFaithManager.AddThought(Thought.Cult_WokeUpEveryone);
    yield return (object) new WaitForSeconds(1f);
    Interaction_TempleAltar.Instance.OnInteract(PlayerFarming.Instance.state);
  }

  private void IncrementXPBar()
  {
    this.barLocalXP += 0.1f;
    this.StartCoroutine((IEnumerator) this.UIDoctrineBar.UpdateFirstBar(this.barLocalXP, 0.1f));
  }

  private void IncrementCustomXPBar(float value)
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
    while (amount > 0)
    {
      // ISSUE: reference to a compiler-generated method
      SoulCustomTarget.Create(PlayerFarming.Instance.CrownBone.gameObject, Interaction_TempleAltar.Instance.PortalEffect.transform.position, Color.white, new System.Action(sermonController.\u003CSacrificeLevelUp\u003Eb__11_0), 0.2f, 500f);
      xp += 0.1f;
      --amount;
      if (Mathf.RoundToInt(xp * 10f) < Mathf.RoundToInt(DoctrineUpgradeSystem.GetXPTargetBySermon(sermonController.SermonCategory) * 10f))
        yield return (object) new WaitForSeconds(0.2f);
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
    if (amount <= 0 || !GameManager.GetInstance().UpgradePlayerConfiguration.HasUnlockAvailable())
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
      if (GameManager.GetInstance().UpgradePlayerConfiguration.HasUnlockAvailable())
      {
        while (amount > 0)
        {
          // ISSUE: reference to a compiler-generated method
          SoulCustomTarget.Create(PlayerFarming.Instance.CrownBone.gameObject, Interaction_TempleAltar.Instance.PortalEffect.transform.position, Color.white, new System.Action(sermonController.\u003CSacrificeLevelUp\u003Eb__11_1), 0.2f, 500f);
          xp += 0.1f;
          --amount;
          if (Mathf.RoundToInt(xp * 10f) < Mathf.RoundToInt(DoctrineUpgradeSystem.GetXPTargetBySermon(sermonController.SermonCategory) * 10f))
            yield return (object) new WaitForSeconds(0.2f);
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
    UIUpgradePlayerTreeMenuController playerUpgradeMenuInstance = MonoSingleton<UIManager>.Instance.ShowPlayerUpgradeTree();
    UIUpgradePlayerTreeMenuController treeMenuController1 = playerUpgradeMenuInstance;
    treeMenuController1.OnHide = treeMenuController1.OnHide + (System.Action) (() => { });
    UIUpgradePlayerTreeMenuController treeMenuController2 = playerUpgradeMenuInstance;
    treeMenuController2.OnUpgradeUnlocked = treeMenuController2.OnUpgradeUnlocked + (Action<UpgradeSystem.Type>) (type => upgradeType = RitualFlockOfTheFaithful.GetUpgradeType(type));
    UIUpgradePlayerTreeMenuController treeMenuController3 = playerUpgradeMenuInstance;
    treeMenuController3.OnHidden = treeMenuController3.OnHidden + (System.Action) (() => playerUpgradeMenuInstance = (UIUpgradePlayerTreeMenuController) null);
    while ((UnityEngine.Object) playerUpgradeMenuInstance != (UnityEngine.Object) null)
      yield return (object) null;
    UpgradeSystem.DisciplePoints = 0;
    ++DataManager.Instance.Doctrine_PlayerUpgrade_Level;
    yield return (object) sermonController.StartCoroutine((IEnumerator) sermonController.EmitParticles(upgradeType));
  }

  private IEnumerator EmitParticles(
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
}
