// Decompiled with JetBrains decompiler
// Type: DoctrineController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using Lamb.UI;
using Lamb.UI.Rituals;
using Lamb.UI.SermonWheelOverlay;
using src.Extensions;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class DoctrineController : MonoBehaviour
{
  private Interaction_TempleAltar TempleAltar;
  [SerializeField]
  private UISermonWheelController _sermonWheel;
  private SermonCategory SermonCategory;
  private EventInstance loop;
  public static System.Action OnUnlockedFirstRitual;

  private void OnEnable()
  {
    this.TempleAltar = this.GetComponent<Interaction_TempleAltar>();
    UIPlayerUpgradesMenuController.OnDoctrineUnlockSelected += new System.Action(this.Play);
  }

  private void OnDisable()
  {
    UIPlayerUpgradesMenuController.OnDoctrineUnlockSelected -= new System.Action(this.Play);
  }

  private void Play() => this.StartCoroutine((IEnumerator) this.PlayIE());

  private IEnumerator PlayIE()
  {
    DoctrineController doctrineController = this;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "sermons/doctrine-start", false);
    PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "sermons/doctrine-loop", true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/sermon/start_sermon", PlayerFarming.Instance.gameObject);
    AudioManager.Instance.PlayOneShot("event:/building/building_bell_ring", PlayerFarming.Instance.gameObject);
    doctrineController.loop = AudioManager.Instance.CreateLoop("event:/sermon/preach_loop", PlayerFarming.Instance.gameObject, true, false);
    doctrineController.StartCoroutine((IEnumerator) doctrineController.TempleAltar.CentrePlayer());
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 12f);
    float t = Time.time;
    yield return (object) doctrineController.StartCoroutine((IEnumerator) Interaction_TempleAltar.Instance.FollowersEnterForSermonRoutine());
    if ((double) Time.time - (double) t < 0.5)
      yield return (object) new WaitForSeconds(1f);
    if (!DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.Special_Bonfire))
    {
      doctrineController.TempleAltar.SermonCategory = SermonCategory.Special;
      doctrineController.StartCoroutine((IEnumerator) doctrineController.DeclareDoctrine());
      AudioManager.Instance.PlayOneShot("event:/sermon/select_sermon", PlayerFarming.Instance.gameObject);
    }
    else
    {
      GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, 0.0f));
      GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 11f);
      AudioManager.Instance.PlayOneShot("event:/sermon/sermon_menu_appear", PlayerFarming.Instance.gameObject);
      doctrineController.SermonCategory = SermonCategory.None;
      UISermonWheelController sermonWheelController = doctrineController._sermonWheel.Instantiate<UISermonWheelController>();
      // ISSUE: reference to a compiler-generated method
      sermonWheelController.OnItemChosen = sermonWheelController.OnItemChosen + new Action<SermonCategory>(doctrineController.\u003CPlayIE\u003Eb__7_0);
      // ISSUE: reference to a compiler-generated method
      sermonWheelController.OnHide = sermonWheelController.OnHide + new System.Action(doctrineController.\u003CPlayIE\u003Eb__7_1);
      sermonWheelController.Show();
    }
  }

  private IEnumerator DeclareDoctrine()
  {
    DoctrineController doctrineController = this;
    GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, 1f));
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 7f);
    PlayerFarming.Instance.Spine.skeleton.FindBone("ritualring").Rotation += 60f;
    PlayerFarming.Instance.Spine.skeleton.UpdateWorldTransform();
    PlayerFarming.Instance.Spine.skeleton.Update(Time.deltaTime);
    yield return (object) new WaitForSeconds(0.6f);
    doctrineController.TempleAltar.PulseDisplacementObject(doctrineController.TempleAltar.state.transform.position);
    yield return (object) new WaitForSeconds(0.4f);
    ChurchFollowerManager.Instance.StartSermonEffectClean();
    AudioManager.Instance.PlayOneShot("event:/sermon/upgrade_menu_appear");
    yield return (object) new WaitForSeconds(0.5f);
    yield return (object) doctrineController.StartCoroutine((IEnumerator) doctrineController.TempleAltar.AskQuestionRoutine());
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "sermons/declare-doctrine", false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    PlayerDoctrineStone.Instance.CanvasGroup.alpha = 1f;
    PlayerDoctrineStone.Instance.Spine.AnimationState.SetAnimation(0, "declare-doctrine", false);
    PlayerDoctrineStone.Instance.Spine.enabled = true;
    AudioManager.Instance.PlayOneShot("event:/temple_key/fragment_move", PlayerFarming.Instance.gameObject);
    yield return (object) new WaitForSeconds(2.9f);
    if (DoctrineUpgradeSystem.UnlockedUpgrades.Count > 0)
    {
      DoctrineUpgradeSystem.DoctrineType unlockedUpgrade = DoctrineUpgradeSystem.UnlockedUpgrades[DoctrineUpgradeSystem.UnlockedUpgrades.Count - 1];
      if (DoctrineUpgradeSystem.ShowDoctrineTutorialForType(unlockedUpgrade) == DoctrineUpgradeSystem.DoctrineCategory.Ritual)
      {
        int num = (int) DoctrineUpgradeSystem.RitualForDoctrineUpgrade(unlockedUpgrade);
        yield return (object) new WaitForSecondsRealtime(0.5f);
        GameManager.GetInstance().CameraSetOffset(Vector3.left * 2.25f);
        UIRitualsMenuController ritualsMenu = MonoSingleton<UIManager>.Instance.RitualsMenuTemplate.Instantiate<UIRitualsMenuController>();
        ritualsMenu.Show(UpgradeSystem.UnlockedUpgrades[UpgradeSystem.UnlockedUpgrades.Count - 1]);
        UIRitualsMenuController ritualsMenuController = ritualsMenu;
        ritualsMenuController.OnHidden = ritualsMenuController.OnHidden + (System.Action) (() => ritualsMenu = (UIRitualsMenuController) null);
        while ((UnityEngine.Object) ritualsMenu != (UnityEngine.Object) null)
          yield return (object) null;
        yield return (object) new WaitForSecondsRealtime(0.25f);
        GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, 1f));
      }
    }
    ChurchFollowerManager.Instance.EndSermonEffectClean();
    AudioManager.Instance.PlayOneShot("event:/sermon/end_sermon", PlayerFarming.Instance.gameObject);
    int num1 = (int) doctrineController.loop.stop(STOP_MODE.ALLOWFADEOUT);
    AudioManager.Instance.StopLoop(doctrineController.loop);
    yield return (object) new WaitForSeconds(0.333333343f);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTaskType == FollowerTaskType.AttendTeaching)
      {
        if (allBrain.HasTrait(FollowerTrait.TraitType.SermonEnthusiast))
          allBrain.AddThought(Thought.WatchedSermonDevotee);
        else
          allBrain.AddThought(Thought.WatchedSermon);
        Follower f = FollowerManager.FindFollowerByID(allBrain.Info.ID);
        allBrain.GetWillLevelUp(FollowerBrain.AdorationActions.Sermon);
        allBrain.AddAdoration(FollowerBrain.AdorationActions.Sermon, (System.Action) (() =>
        {
          if (f.Brain.CurrentTaskType != FollowerTaskType.AttendTeaching)
            return;
          f.Brain.CurrentTask.StartAgain(f);
        }));
        doctrineController.StartCoroutine((IEnumerator) doctrineController.TempleAltar.DelayFollowerReaction(allBrain, UnityEngine.Random.Range(0.1f, 0.5f)));
      }
    }
    PlayerDoctrineStone.Instance.CanvasGroup.alpha = 0.0f;
    PlayerDoctrineStone.Instance.Spine.enabled = false;
    doctrineController.TempleAltar.ResetSprite();
    AudioManager.Instance.PlayOneShot("event:/sermon/book_put_down", PlayerFarming.Instance.gameObject);
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = false;
    PlayerFarming.Instance.Spine.UseDeltaTime = true;
    UpgradeSystem.AddCooldown(UpgradeSystem.PrimaryRitual1, 1200f);
    --PlayerDoctrineStone.Instance.CompletedDoctrineStones;
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.DeclareDoctrine);
    DoctrineUpgradeSystem.DoctrineCategory doctrineCategory = DoctrineUpgradeSystem.ShowDoctrineTutorialForType(Interaction_TempleAltar.DoctrineUnlockType);
    Debug.Log((object) ("category: " + (object) doctrineCategory));
    switch (doctrineCategory)
    {
      case DoctrineUpgradeSystem.DoctrineCategory.Trait:
        if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Traits))
        {
          MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Traits);
          break;
        }
        break;
      case DoctrineUpgradeSystem.DoctrineCategory.FollowerAction:
        if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.FollowerAction))
        {
          MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.FollowerAction);
          break;
        }
        break;
      case DoctrineUpgradeSystem.DoctrineCategory.Ritual:
        if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Rituals))
        {
          System.Action unlockedFirstRitual = DoctrineController.OnUnlockedFirstRitual;
          if (unlockedFirstRitual != null)
          {
            unlockedFirstRitual();
            break;
          }
          break;
        }
        break;
    }
    yield return (object) new WaitForSeconds(1f);
    Interaction_TempleAltar.Instance.OnInteract(PlayerFarming.Instance.state);
  }

  private IEnumerator CancelDoctrineDeclaration()
  {
    ChurchFollowerManager.Instance.EndSermonEffectClean();
    AudioManager.Instance.PlayOneShot("event:/sermon/end_sermon", PlayerFarming.Instance.gameObject);
    int num = (int) this.loop.stop(STOP_MODE.ALLOWFADEOUT);
    AudioManager.Instance.StopLoop(this.loop);
    yield return (object) new WaitForSeconds(0.333333343f);
    this.TempleAltar.ResetSprite();
    AudioManager.Instance.PlayOneShot("event:/sermon/book_put_down", PlayerFarming.Instance.gameObject);
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = false;
    PlayerFarming.Instance.Spine.UseDeltaTime = true;
    yield return (object) new WaitForSeconds(1f);
    Interaction_TempleAltar.Instance.OnInteract(PlayerFarming.Instance.state);
  }
}
