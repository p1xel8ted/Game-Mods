// Decompiled with JetBrains decompiler
// Type: Interaction_EnterEndlessMode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using MMTools;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
[Preserve]
public class Interaction_EnterEndlessMode : Interaction
{
  public string sLabel;
  [SerializeField]
  public List<GameObject> _objectsToEnable = new List<GameObject>();
  [SerializeField]
  public List<GameObject> _objectsToDisable = new List<GameObject>();
  [SerializeField]
  public GameObject _portalVFX;
  [SerializeField]
  public GameObject _portalVFX_Recharge;
  [SerializeField]
  public GameObject _portalVFXLighting;
  [SerializeField]
  public GameObject crownStatue;
  [SerializeField]
  public GameObject cameraInclude;
  public Tween currentTween;
  public bool wasRecharging;
  public const int SIN_COST = 1;
  public GameObject distortionObject;

  public bool _Interactable
  {
    get => this.Interactable;
    set
    {
      Debug.Log((object) "AAAA".Colour(Color.yellow));
      this.Interactable = value;
    }
  }

  public void Start() => this.UpdateLocalisation();

  public override void OnEnable()
  {
    this._portalVFX_Recharge.SetActive(false);
    this._portalVFX.SetActive(false);
    base.OnEnable();
    this.distortionObject.gameObject.SetActive(false);
    this.CheckState();
    TimeManager.OnNewDayStarted += new System.Action(this.CheckStateDelayed);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    TimeManager.OnNewDayStarted -= new System.Action(this.CheckStateDelayed);
  }

  public void CheckStateDelayed()
  {
    this.StartCoroutine((IEnumerator) this.CheckStateDelayedRoutine());
  }

  public IEnumerator CheckStateDelayedRoutine()
  {
    yield return (object) new WaitForSeconds(0.2f);
    this.CheckState(true);
  }

  public override void OnBecomeCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeCurrent(playerFarming);
    if (DataManager.Instance.EndlessModeOnCooldown && DataManager.Instance.PleasureEnabled && !DataManager.Instance.EndlessModeSinOncooldown)
      playerFarming.indicator.ShowTopInfo(ScriptLocalization.Interactions.Recharging);
    if (DataManager.Instance.EndlessModeOnCooldown && DataManager.Instance.PleasureEnabled && DataManager.Instance.EndlessModeSinOncooldown)
      this._Interactable = false;
    else
      this._Interactable = true;
  }

  public override void OnBecomeNotCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeNotCurrent(playerFarming);
    playerFarming.indicator.HideTopInfo();
  }

  public void CheckState(bool fromDayChange = false)
  {
    if (!DataManager.Instance.OnboardedEndlessMode)
      return;
    foreach (GameObject gameObject in this._objectsToEnable)
      gameObject.SetActive(true);
    foreach (GameObject gameObject in this._objectsToDisable)
      gameObject.SetActive(false);
    if (DataManager.Instance.EndlessModeOnCooldown)
    {
      this.wasRecharging = true;
      this._portalVFX_Recharge.SetActive(true);
      this._portalVFX.SetActive(false);
    }
    else
    {
      if (this.wasRecharging & fromDayChange)
        AudioManager.Instance.PlayOneShot("event:/Stings/Choir_Short", this.gameObject);
      this.wasRecharging = false;
      this._portalVFX_Recharge.SetActive(false);
      this._portalVFX.SetActive(true);
      this.HasChanged = true;
    }
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sLabel = ScriptLocalization.Interactions.EnterPurgatory;
  }

  public override void GetLabel()
  {
    if (string.IsNullOrEmpty(this.sLabel))
      this.UpdateLocalisation();
    if (DataManager.Instance.EndlessModeOnCooldown && DataManager.Instance.PleasureEnabled && !DataManager.Instance.EndlessModeSinOncooldown)
      this.Label = ScriptLocalization.Interactions.Pray + CostFormatter.FormatCost(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 1);
    else if (DataManager.Instance.EndlessModeOnCooldown)
      this.Label = ScriptLocalization.Interactions.Recharging;
    else
      this.Label = this.sLabel;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (DataManager.Instance.EndlessModeOnCooldown && DataManager.Instance.PleasureEnabled && !DataManager.Instance.EndlessModeSinOncooldown)
    {
      if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.PLEASURE_POINT) < 1)
      {
        this.playerFarming.indicator.PlayShake();
      }
      else
      {
        this.StartCoroutine((IEnumerator) this.ReopenPurgatoryRoutine());
        this.playerFarming.indicator.HideTopInfo();
      }
    }
    else
    {
      if (DataManager.Instance.EndlessModeOnCooldown)
        return;
      this._Interactable = false;
      this.StartCoroutine((IEnumerator) this.EnterEndlessModeIE());
    }
  }

  public IEnumerator ReopenPurgatoryRoutine()
  {
    Interaction_EnterEndlessMode enterEndlessMode = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(enterEndlessMode.gameObject, 11f);
    Vector3 TargetPosition = enterEndlessMode.transform.position - new Vector3(0.0f, 4f, 0.0f);
    enterEndlessMode.playerFarming.GoToAndStop(TargetPosition, maxDuration: 2f, forcePositionOnTimeout: true, groupAction: true);
    while (enterEndlessMode.playerFarming.GoToAndStopping)
      yield return (object) null;
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationNext(enterEndlessMode.gameObject, 7f);
    GameManager.GetInstance().AddPlayerToCamera();
    enterEndlessMode.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    enterEndlessMode.playerFarming.simpleSpineAnimator.Animate("pray", 0, false);
    UIManager.PlayAudio("event:/Stings/church_bell");
    yield return (object) new WaitForSeconds(0.5f);
    CameraManager.instance.ShakeCameraForDuration(0.2f, 0.35f, 1.5f);
    yield return (object) new WaitForSeconds(1.5f);
    CameraManager.instance.ShakeCameraForDuration(0.5f, 0.7f, 0.3f);
    UIManager.PlayAudio("event:/tarot/tarot_card_reveal");
    Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.PLEASURE_POINT, -1);
    DataManager.Instance.EndlessModeOnCooldown = false;
    DataManager.Instance.EndlessModeSinOncooldown = true;
    enterEndlessMode.CheckState(true);
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationEnd();
  }

  public void PulseDisplacementObject()
  {
    if (this.distortionObject.gameObject.activeSelf)
    {
      this.distortionObject.transform.localScale = Vector3.zero;
      this.distortionObject.transform.DORestart();
      this.distortionObject.transform.DOPlay();
    }
    else
    {
      this.distortionObject.SetActive(true);
      this.distortionObject.transform.localScale = Vector3.zero;
      this.distortionObject.transform.DOScale(9f, 0.9f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.distortionObject.SetActive(false)));
    }
  }

  public IEnumerator EnterEndlessModeIE()
  {
    Interaction_EnterEndlessMode enterEndlessMode = this;
    enterEndlessMode._portalVFXLighting.gameObject.SetActive(true);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(enterEndlessMode.playerFarming.gameObject);
    enterEndlessMode.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    enterEndlessMode.playerFarming.Spine.AnimationState.SetAnimation(0, "warp-out-down", false);
    AudioManager.Instance.PlayOneShot("event:/pentagram/pentagram_teleport_segment", enterEndlessMode.gameObject);
    enterEndlessMode.playerFarming.circleCollider2D.enabled = false;
    enterEndlessMode.playerFarming.transform.DOMove(enterEndlessMode.transform.position - Vector3.up, 1f);
    enterEndlessMode.currentTween.Kill();
    BiomeConstants.Instance.GoopFadeIn(1f, 1.4f);
    BiomeConstants.Instance.ChromaticAbberationTween(2f, BiomeConstants.Instance.ChromaticAberrationDefaultValue, 1f);
    yield return (object) new WaitForSeconds(1.15f);
    enterEndlessMode.cameraInclude.gameObject.SetActive(true);
    enterEndlessMode.PulseDisplacementObject();
    yield return (object) new WaitForSeconds(1f);
    BiomeConstants.Instance.ChromaticAbberationTween(0.1f, 0.6f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
    SimulationManager.Pause();
    bool enteredEndlessMode = false;
    enterEndlessMode.cameraInclude.gameObject.SetActive(false);
    enterEndlessMode._portalVFXLighting.gameObject.SetActive(false);
    System.Threading.Tasks.Task loadTask = MonoSingleton<UIManager>.Instance.LoadSandboxMenuAssets();
    yield return (object) new WaitUntil((Func<bool>) (() => loadTask.IsCompleted));
    UISandboxMenuController sandboxMenuController = MonoSingleton<UIManager>.Instance.SandboxMenuTemplate.Instantiate<UISandboxMenuController>();
    sandboxMenuController.Show();
    sandboxMenuController.OnScenarioChosen += (System.Action<ScenarioData>) (scenario =>
    {
      DungeonSandboxManager.CurrentScenario = scenario;
      DungeonSandboxManager.CurrentFleece = scenario.FleeceType;
      GameManager.NewRun("", false);
      GameManager.DungeonUseAllLayers = true;
      DataManager.Instance.EndlessModeOnCooldown = true;
      enteredEndlessMode = true;
      UIManager.PlayAudio("event:/ui/heretics_defeated");
      MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "Dungeon Sandbox", 1f, "", (System.Action) (() => SaveAndLoad.Save()));
    });
    sandboxMenuController.OnHide = sandboxMenuController.OnHide + (System.Action) (() =>
    {
      this._Interactable = true;
      if (enteredEndlessMode)
        return;
      this.StartCoroutine((IEnumerator) this.ExitEndlessModeIE());
    });
  }

  public IEnumerator ExitEndlessModeIE()
  {
    Interaction_EnterEndlessMode enterEndlessMode = this;
    enterEndlessMode._Interactable = false;
    enterEndlessMode._portalVFXLighting.gameObject.SetActive(true);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(enterEndlessMode.playerFarming.gameObject);
    enterEndlessMode.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    enterEndlessMode.playerFarming.state.facingAngle = enterEndlessMode.playerFarming.state.LookAngle = 0.0f;
    enterEndlessMode.playerFarming.Spine.AnimationState.SetAnimation(0, "warp-in-up", false);
    AudioManager.Instance.PlayOneShot("event:/pentagram/pentagram_teleport_segment", enterEndlessMode.gameObject);
    enterEndlessMode.playerFarming.circleCollider2D.enabled = false;
    enterEndlessMode.playerFarming.transform.DOMove(enterEndlessMode.transform.position - Vector3.up * 3f, 1f);
    BiomeConstants.Instance.GoopFadeOut(1f);
    enterEndlessMode.currentTween.Kill();
    yield return (object) new WaitForSeconds(0.2f);
    enterEndlessMode.PulseDisplacementObject();
    yield return (object) new WaitForSeconds(2.6f);
    enterEndlessMode._portalVFXLighting.gameObject.SetActive(false);
    SimulationManager.UnPause();
    enterEndlessMode.playerFarming.circleCollider2D.enabled = true;
    GameManager.GetInstance().OnConversationEnd();
  }

  public override void IndicateHighlighted(PlayerFarming playerFarming)
  {
    base.IndicateHighlighted(playerFarming);
  }

  public override void EndIndicateHighlighted(PlayerFarming playerFarming)
  {
    base.EndIndicateHighlighted(playerFarming);
  }

  [CompilerGenerated]
  public void \u003CPulseDisplacementObject\u003Eb__27_0()
  {
    this.distortionObject.SetActive(false);
  }
}
