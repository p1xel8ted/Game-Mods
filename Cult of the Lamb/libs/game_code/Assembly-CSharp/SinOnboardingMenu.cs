// Decompiled with JetBrains decompiler
// Type: SinOnboardingMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using MMTools;
using Spine.Unity;
using src.UI.Overlays.TutorialOverlay;
using src.UINavigator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SinOnboardingMenu : UIMenuBase
{
  [SerializeField]
  public SkeletonGraphic crown;
  [SerializeField]
  public SkeletonGraphic backgroundSpine;
  [SerializeField]
  public GameObject convoTarget;
  public int _cachedMMConversationSortingOrder;
  public Canvas _mmConversationCanvas;

  public SkeletonGraphic Crown => this.crown;

  public SkeletonGraphic SinBackgroundSpine => this.backgroundSpine;

  public override bool _addToActiveMenus => false;

  public override void OnHideCompleted()
  {
    if ((UnityEngine.Object) MMConversation.mmConversation != (UnityEngine.Object) null)
      MMConversation.mmConversation.SpeechBubble.gameObject.SetActive(true);
    if ((UnityEngine.Object) this._mmConversationCanvas != (UnityEngine.Object) null)
      this._mmConversationCanvas.sortingOrder = this._cachedMMConversationSortingOrder;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public IEnumerator CrownSequence()
  {
    SinOnboardingMenu coroutineSupport = this;
    bool depthOfField = SettingsManager.Settings.Graphics.DepthOfField;
    SettingsManager.Settings.Graphics.DepthOfField = false;
    coroutineSupport.crown.AnimationState.SetAnimation(0, "snake-start", false);
    coroutineSupport.crown.AnimationState.AddAnimation(0, "snake-loop", true, 0.0f);
    MMVibrate.StopRumble();
    float time = 0.0f;
    float progress = 0.0f;
    while ((double) time < 3.5)
    {
      time += Time.unscaledDeltaTime;
      progress += Time.unscaledDeltaTime;
      if ((double) progress > 0.25)
        MMVibrate.Rumble(UnityEngine.Random.value / 2f, UnityEngine.Random.value, UnityEngine.Random.value / 2f, (MonoBehaviour) coroutineSupport);
      yield return (object) null;
    }
    yield return (object) new WaitForSecondsRealtime(0.5f);
    bool waiting = true;
    coroutineSupport.ShowConvo(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(Interaction_TempleAltar.Instance.gameObject, "Conversation_NPC/SinOnboarding/0/0"),
      new ConversationEntry(Interaction_TempleAltar.Instance.gameObject, "Conversation_NPC/SinOnboarding/0/1"),
      new ConversationEntry(Interaction_TempleAltar.Instance.gameObject, "Conversation_NPC/SinOnboarding/0/2"),
      new ConversationEntry(Interaction_TempleAltar.Instance.gameObject, "Conversation_NPC/SinOnboarding/1/0"),
      new ConversationEntry(Interaction_TempleAltar.Instance.gameObject, "Conversation_NPC/SinOnboarding/2/0")
    }, (List<MMTools.Response>) null, (System.Action) (() => waiting = false)));
    while (waiting)
      yield return (object) null;
    SimulationManager.Pause();
    ChurchFollowerManager.Instance.SetOverlayCanvasOrder(0);
    waiting = true;
    UIUpgradeTreeMenuController treeMenuController = MonoSingleton<UIManager>.Instance.ShowUpgradeTree((System.Action) (() => waiting = false), UpgradeSystem.Type.PleasureSystem);
    treeMenuController.OnHide = treeMenuController.OnHide + (System.Action) (() =>
    {
      LetterBox.Show(true);
      HUD_Manager.Instance.Hide(true);
      GameManager.GetInstance().OnConversationNew();
    });
    while (waiting)
      yield return (object) null;
    if (!DataManager.Instance.OnboardedMysticShop)
      DataManager.Instance.MysticKeeperOnboardedSin = true;
    if (!DataManager.Instance.OnboardedRelics)
      DataManager.Instance.ChemachOnboardedSin = true;
    DataManager.Instance.HasBuiltPleasureShrine = true;
    DataManager.Instance.PleasureRevealed = true;
    waiting = true;
    coroutineSupport.ShowConvo(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(Interaction_TempleAltar.Instance.gameObject, "Conversation_NPC/SinOnboarding/2/1"),
      new ConversationEntry(Interaction_TempleAltar.Instance.gameObject, "Conversation_NPC/SinOnboarding/3/0")
    }, (List<MMTools.Response>) null, (System.Action) (() => waiting = false)));
    while (waiting)
      yield return (object) null;
    SimulationManager.Pause();
    LetterBox.Show(true);
    HUD_Manager.Instance.Hide(true);
    GameManager.GetInstance().OnConversationNew();
    DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Sin);
    UITutorialOverlayController tutorialOverlay = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Sin);
    UITutorialOverlayController overlayController1 = tutorialOverlay;
    overlayController1.OnHidden = overlayController1.OnHidden + (System.Action) (() =>
    {
      waiting = false;
      HUD_Manager.Instance.Hide(true);
    });
    UITutorialOverlayController overlayController2 = tutorialOverlay;
    overlayController2.OnShow = overlayController2.OnShow + (System.Action) (() =>
    {
      tutorialOverlay.HideBlurBackground();
      tutorialOverlay.GetComponent<Canvas>().sortingOrder = 1001;
    });
    waiting = true;
    while (waiting)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew(false);
    PlayerFarming.Instance.state.LockStateChanges = true;
    yield return (object) new WaitForSecondsRealtime(0.5f);
    ChurchFollowerManager.Instance.RedLightingVolume.gameObject.SetActive(false);
    ChurchFollowerManager.Instance.NormalLightingVolume.TransitionIn();
    GameManager.GetInstance().WaitForSeconds(0.5f, (System.Action) (() =>
    {
      ChurchFollowerManager.Instance.PlayOverlay(ChurchFollowerManager.OverlayType.Ritual, "sin-stop", "off", true);
      AudioManager.Instance.StopLoop(Interaction_TempleAltar.Instance.SinLoop);
    }));
    AudioManager.Instance.PlayOneShot("event:/dialogue/sin_snake/sin_snake");
    coroutineSupport.crown.AnimationState.SetAnimation(0, "snake-stop", false);
    yield return (object) new WaitForSeconds(0.5f);
    AudioManager.Instance.PlayOneShot("event:/rituals/end_sins_onboarding");
    coroutineSupport.backgroundSpine.AnimationState.SetAnimation(0, "off", false);
    AudioManager.Instance.PlayOneShot("event:/sermon/upgrade_menu_appear", PlayerFarming.Instance.gameObject);
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().CamFollowTarget.enabled = true;
    GameManager.GetInstance().OnConversationNext(coroutineSupport.crown.gameObject, 6f);
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "rituals/sin-onboarding-loop", true);
    float q = 0.0f;
    DOTween.To((DOGetter<float>) (() => q), (DOSetter<float>) (x => q = x), 1f, 1f).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => GameManager.GetInstance().CamFollowTarget.SetOffset(Vector3.Lerp(Vector3.zero, Vector3.forward * 2.25f, q))));
    SettingsManager.Settings.Graphics.DepthOfField = depthOfField;
    yield return (object) coroutineSupport.StartCoroutine((IEnumerator) coroutineSupport.SinOutroSequenceIE(new Vector3(0.15f, PlayerFarming.Instance.CrownBone.position.y, PlayerFarming.Instance.CrownBone.position.z - 0.05f)));
    ChurchFollowerManager.Instance.DisableAllOverlays();
    coroutineSupport.Crown.gameObject.SetActive(false);
    PlayerFarming.Instance.state.LockStateChanges = false;
    ChurchFollowerManager.Instance.SetOverlayCanvasOrder(0);
    System.Action doctrineUnlockSelected = UIPlayerUpgradesMenuController.OnDoctrineUnlockSelected;
    if (doctrineUnlockSelected != null)
      doctrineUnlockSelected();
    DoctrineController.SinOnboarding = true;
    waiting = true;
    while (waiting && !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ritual_Purge) && !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ritual_Nudism))
      yield return (object) null;
    SimulationManager.Pause();
    yield return (object) new WaitForSecondsRealtime(4f);
    MonoSingleton<UINavigatorNew>.Instance.LockInput = MonoSingleton<UINavigatorNew>.Instance.LockNavigation = false;
    GameManager.GetInstance().WaitForSecondsRealtime(1f, (System.Action) (() => ChurchFollowerManager.Instance.DisableAllOverlays()));
  }

  public void ShowConvo(ConversationObject obj)
  {
    foreach (ConversationEntry entry in obj.Entries)
    {
      entry.Offset = new Vector3(0.0f, 70f, 0.0f);
      entry.soundPath = "event:/dialogue/sin_snake/sin_snake";
    }
    MMConversation.Play(obj);
    if (!((UnityEngine.Object) MMConversation.mmConversation != (UnityEngine.Object) null) || !MMConversation.mmConversation.TryGetComponent<Canvas>(out this._mmConversationCanvas))
      return;
    this._cachedMMConversationSortingOrder = this._mmConversationCanvas.sortingOrder;
    if (!((UnityEngine.Object) this._canvas != (UnityEngine.Object) null))
      return;
    this._mmConversationCanvas.sortingOrder = this._canvas.sortingOrder + 1;
  }

  public IEnumerator SinIntroSequenceIE(Vector3 spawnPos, Vector3 targetPos)
  {
    this.crown.gameObject.SetActive(true);
    this.crown.transform.position = spawnPos;
    this.crown.transform.localScale = Vector3.one * 0.055f;
    this.crown.transform.DOMove(spawnPos - Vector3.forward, 6f);
    GameManager.GetInstance().OnConversationNext(this.crown.gameObject, 6f);
    yield return (object) new WaitForSeconds(1f);
    this.crown.transform.DOMove(targetPos, 5f);
    this.crown.transform.DOScale(Vector3.one * 0.1f, 6f);
    BiomeConstants.Instance.DepthOfFieldTween(0.15f, 8.7f, 26f, 0.0f, 0.0f);
  }

  public IEnumerator SinOutroSequenceIE(Vector3 targetPos)
  {
    SinOnboardingMenu coroutineSupport = this;
    coroutineSupport.crown.transform.localScale = Vector3.one * 0.1f;
    coroutineSupport.crown.transform.DOMove(targetPos, 3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine);
    coroutineSupport.crown.transform.DOScale(0.05f, 5f);
    MMVibrate.Rumble(1.1f, 1.3f, 3f, (MonoBehaviour) coroutineSupport);
    Quaternion fromRot = GameManager.GetInstance().CamFollowTarget.transform.rotation;
    BiomeConstants.Instance.DepthOfFieldTween(3f, 8.7f, 26f, 1f, 200f);
    float q = 0.0f;
    while ((double) q < 3.0)
    {
      q += Time.deltaTime;
      if ((double) q > 1.5)
        GameManager.GetInstance().CamFollowTarget.transform.rotation = Quaternion.Lerp(fromRot, Quaternion.Euler(-45f, 0.0f, 0.0f), (float) (((double) q - 1.5) / 1.5));
      yield return (object) null;
    }
    MMVibrate.StopRumble();
    yield return (object) new WaitForEndOfFrame();
    MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
  }
}
