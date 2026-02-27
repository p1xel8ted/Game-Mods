// Decompiled with JetBrains decompiler
// Type: DoctrineController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public Interaction_TempleAltar TempleAltar;
  public InventoryItem.ITEM_TYPE _currency;
  public static bool SinOnboarding;
  public EventInstance loop;
  public static System.Action OnUnlockedFirstRitual;

  public void OnEnable()
  {
    this.TempleAltar = this.GetComponent<Interaction_TempleAltar>();
    UIPlayerUpgradesMenuController.OnDoctrineUnlockSelected += new System.Action(this.Play);
    UIPlayerUpgradesMenuController.OnCrystalDoctrineUnlockSelected += new System.Action(this.PlayCrystalDoctrine);
  }

  public void OnDisable()
  {
    AudioManager.Instance.StopLoop(this.loop);
    UIPlayerUpgradesMenuController.OnDoctrineUnlockSelected -= new System.Action(this.Play);
    UIPlayerUpgradesMenuController.OnCrystalDoctrineUnlockSelected -= new System.Action(this.PlayCrystalDoctrine);
  }

  public void Play()
  {
    this._currency = InventoryItem.ITEM_TYPE.DOCTRINE_STONE;
    this.StartCoroutine(this.PlayIE());
  }

  public void PlayCrystalDoctrine()
  {
    this._currency = InventoryItem.ITEM_TYPE.CRYSTAL_DOCTRINE_STONE;
    this.StartCoroutine(this.PlayIE());
  }

  public IEnumerator PlayIE()
  {
    DoctrineController doctrineController = this;
    SimulationManager.UnPause();
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "sermons/doctrine-start", false);
    PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "sermons/doctrine-loop", true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/sermon/start_sermon", PlayerFarming.Instance.gameObject);
    AudioManager.Instance.PlayOneShot("event:/building/building_bell_ring", PlayerFarming.Instance.gameObject);
    doctrineController.loop = AudioManager.Instance.CreateLoop("event:/sermon/preach_loop", PlayerFarming.Instance.gameObject, true, false);
    doctrineController.StartCoroutine(doctrineController.TempleAltar.CentrePlayer());
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 12f);
    float t = Time.time;
    yield return (object) doctrineController.StartCoroutine(Interaction_TempleAltar.Instance.FollowersEnterForSermonRoutine());
    if ((double) Time.time - (double) t < 0.5)
      yield return (object) new WaitForSeconds(1f);
    if (!DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.Special_Bonfire))
    {
      doctrineController.TempleAltar.SermonCategory = SermonCategory.Special;
      doctrineController.StartCoroutine(doctrineController.DeclareDoctrine());
      AudioManager.Instance.PlayOneShot("event:/sermon/select_sermon", PlayerFarming.Instance.gameObject);
    }
    else
    {
      GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, 0.0f));
      GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 11f);
      AudioManager.Instance.PlayOneShot("event:/sermon/sermon_menu_appear", PlayerFarming.Instance.gameObject);
      SermonCategory sermonCategory = SermonCategory.None;
      UISermonWheelController menu = MonoSingleton<UIManager>.Instance.SermonWheelTemplate.Instantiate<UISermonWheelController>();
      menu.Show(doctrineController._currency);
      UISermonWheelController sermonWheelController = menu;
      sermonWheelController.OnItemChosen = sermonWheelController.OnItemChosen + (Action<SermonCategory>) (chosenCategory =>
      {
        Debug.Log((object) $"Chose category {chosenCategory}".Colour(Color.yellow));
        sermonCategory = chosenCategory;
      });
      yield return (object) menu.YieldUntilHide();
      if (sermonCategory != SermonCategory.None)
      {
        doctrineController.TempleAltar.SermonCategory = sermonCategory;
        doctrineController.StartCoroutine(doctrineController.DeclareDoctrine());
        AudioManager.Instance.PlayOneShot("event:/sermon/select_sermon", PlayerFarming.Instance.gameObject);
      }
      else
        yield return (object) doctrineController.CancelDoctrineDeclaration();
    }
  }

  public IEnumerator DeclareDoctrine()
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
    yield return (object) doctrineController.StartCoroutine(doctrineController.TempleAltar.AskQuestionRoutine(doctrineController._currency));
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "sermons/declare-doctrine", false);
    PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    if (!DoctrineController.SinOnboarding)
    {
      if (doctrineController._currency == InventoryItem.ITEM_TYPE.CRYSTAL_DOCTRINE_STONE)
        PlayerFarming.Instance.PlayerDoctrineStone.Spine.Skeleton.SetSkin("crystal");
      PlayerFarming.Instance.PlayerDoctrineStone.CanvasGroup.alpha = 1f;
      PlayerFarming.Instance.PlayerDoctrineStone.Spine.AnimationState.SetAnimation(0, "declare-doctrine", false);
      PlayerFarming.Instance.PlayerDoctrineStone.Spine.enabled = true;
    }
    AudioManager.Instance.PlayOneShot("event:/temple_key/fragment_move", PlayerFarming.Instance.gameObject);
    yield return (object) new WaitForSeconds(1.9f);
    if (DoctrineController.SinOnboarding)
      AudioManager.Instance.PlayOneShot("event:/temple_key/fragment_pickup", PlayerFarming.Instance.gameObject);
    yield return (object) new WaitForSeconds(1f);
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
        FollowerManager.FindFollowerByID(allBrain.Info.ID);
        allBrain.AddAdoration(FollowerBrain.AdorationActions.Sermon, (System.Action) null);
        doctrineController.StartCoroutine(doctrineController.TempleAltar.DelayFollowerReaction(allBrain, UnityEngine.Random.Range(0.1f, 0.5f)));
      }
    }
    PlayerFarming.Instance.PlayerDoctrineStone.Spine.Skeleton.SetSkin("normal");
    PlayerFarming.Instance.PlayerDoctrineStone.CanvasGroup.alpha = 0.0f;
    PlayerFarming.Instance.PlayerDoctrineStone.Spine.enabled = false;
    doctrineController.TempleAltar.ResetSprite();
    AudioManager.Instance.PlayOneShot("event:/sermon/book_put_down", PlayerFarming.Instance.gameObject);
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = false;
    PlayerFarming.Instance.Spine.UseDeltaTime = true;
    if (doctrineController._currency == InventoryItem.ITEM_TYPE.CRYSTAL_DOCTRINE_STONE)
    {
      UpgradeSystem.AddCooldown(UpgradeSystem.Type.Ritual_CrystalDoctrine, 1200f);
      Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.CRYSTAL_DOCTRINE_STONE, -1);
    }
    else
    {
      UpgradeSystem.AddCooldown(UpgradeSystem.PrimaryRitual1, 1200f);
      if (!DoctrineController.SinOnboarding)
        --PlayerFarming.Instance.PlayerDoctrineStone.CompletedDoctrineStones;
    }
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.DeclareDoctrine);
    if (doctrineController.TempleAltar.SermonCategory == SermonCategory.Winter)
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.DeclareDoctrine_WINTER);
    DoctrineUpgradeSystem.DoctrineCategory doctrineCategory = DoctrineUpgradeSystem.ShowDoctrineTutorialForType(Interaction_TempleAltar.DoctrineUnlockType);
    Debug.Log((object) ("category: " + doctrineCategory.ToString()));
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
    yield return (object) new WaitForSeconds(1.5f);
    yield return (object) null;
    if (!DoctrineController.SinOnboarding)
      Interaction_TempleAltar.Instance.OnInteract(PlayerFarming.Instance.state);
    DoctrineController.SinOnboarding = false;
  }

  public IEnumerator CancelDoctrineDeclaration()
  {
    ChurchFollowerManager.Instance.EndSermonEffectClean();
    AudioManager.Instance.PlayOneShot("event:/sermon/end_sermon", PlayerFarming.Instance.gameObject);
    int num = (int) this.loop.stop(STOP_MODE.ALLOWFADEOUT);
    AudioManager.Instance.StopLoop(this.loop);
    yield return (object) new WaitForSeconds(0.333333343f);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("sermons/sermon-stop-nobook", 0, false);
    yield return (object) new WaitForSeconds(0.7f);
    this.TempleAltar.ResetSprite();
    AudioManager.Instance.PlayOneShot("event:/sermon/book_put_down", PlayerFarming.Instance.gameObject);
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = false;
    PlayerFarming.Instance.Spine.UseDeltaTime = true;
    if (!DoctrineController.SinOnboarding)
      Interaction_TempleAltar.Instance.OnInteract(PlayerFarming.Instance.state);
    DoctrineController.SinOnboarding = false;
  }
}
