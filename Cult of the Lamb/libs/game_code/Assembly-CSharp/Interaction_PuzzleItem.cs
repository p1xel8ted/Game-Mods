// Decompiled with JetBrains decompiler
// Type: Interaction_PuzzleItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_PuzzleItem : Interaction
{
  public static List<Interaction_PuzzleItem> PuzzleItems = new List<Interaction_PuzzleItem>();
  [SerializeField]
  public Interaction_PuzzleItem.PuzzleItemType type;
  [SerializeField]
  public GameObject container;
  [SerializeField]
  public string skinName;
  [Header("Switch")]
  [SerializeField]
  public bool startActive;
  [SerializeField]
  public GameObject activated;
  [SerializeField]
  public GameObject deactivated;
  [SerializeField]
  public UnityEvent onActivated;
  [SerializeField]
  public UnityEvent onDeactivated;
  [Space]
  [SerializeField]
  public bool ignoreFromCompletionCheck;
  public PuzzleSlot selectedSlot;
  public bool carrying;
  public bool active;
  public Vector3 startPos;
  public EventInstance loopedSound;
  public PuzzleSlot closestPuzzleSlot;
  public float ClosestPosition = 100f;
  public bool FoundOne;
  public bool addedOutline;

  public Interaction_PuzzleItem.PuzzleItemType Type => this.type;

  public void Start()
  {
    this.active = this.startActive;
    if (this.type == Interaction_PuzzleItem.PuzzleItemType.Switch)
      this.UpdateSprite();
    Interaction_PuzzleItem.PuzzleItems.Add(this);
    this.UpdateLocalisation();
    this.startPos = this.transform.position;
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if ((bool) (UnityEngine.Object) this.playerFarming && (UnityEngine.Object) this.playerFarming.PuzzlePieceCarried == (UnityEngine.Object) this)
      this.playerFarming.PuzzlePieceCarried = (Interaction_PuzzleItem) null;
    Interaction_PuzzleItem.PuzzleItems.Remove(this);
    AudioManager.Instance.StopLoop(this.loopedSound);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.carrying = false;
  }

  public override void OnDisable() => base.OnDisable();

  public new void OnPlayerLeft()
  {
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.type == Interaction_PuzzleItem.PuzzleItemType.Pickupable)
    {
      if (this.carrying || state.CURRENT_STATE == StateMachine.State.ChargingHeavyAttack)
        return;
      base.OnInteract(state);
      this.StartCoroutine((IEnumerator) this.PickUpBody());
    }
    else
    {
      if (this.type != Interaction_PuzzleItem.PuzzleItemType.Switch)
        return;
      this.active = !this.active;
      this.UpdateSprite();
      if (this.active)
      {
        this.loopedSound = AudioManager.Instance.CreateLoop("event:/dlc/env/puzzle_room/switch_active_loop", this.gameObject, true);
        this.onActivated?.Invoke();
        AudioManager.Instance.PlayOneShot("event:/dlc/env/puzzle_room/switch_activate", this.transform.position);
      }
      else
      {
        this.onDeactivated?.Invoke();
        AudioManager.Instance.PlayOneShot("event:/dlc/env/puzzle_room/switch_deactivate", this.transform.position);
        AudioManager.Instance.StopLoop(this.loopedSound);
      }
      if (!this.active || this.ignoreFromCompletionCheck)
        return;
      bool flag = true;
      foreach (Interaction_PuzzleItem puzzleItem in Interaction_PuzzleItem.PuzzleItems)
      {
        if (!puzzleItem.active && !puzzleItem.ignoreFromCompletionCheck)
          flag = false;
      }
      if (!flag)
        return;
      PuzzleController.Instance.Complete();
    }
  }

  public void UpdateSprite()
  {
    this.activated.gameObject.SetActive(this.active);
    this.deactivated.gameObject.SetActive(!this.active);
  }

  public override void Update()
  {
    base.Update();
    if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null)
      return;
    if (this.carrying)
    {
      this.FoundOne = false;
      this.ClosestPosition = 100f;
      foreach (PuzzleSlot puzzleSlot in PuzzleSlot.PuzzleSlots)
      {
        float num = Vector3.Distance(puzzleSlot.gameObject.transform.position, this.playerFarming.gameObject.transform.position);
        if ((double) num < 1.0 && !puzzleSlot.Correct && !puzzleSlot.IsSpikes)
        {
          if ((double) num < (double) this.ClosestPosition)
          {
            this.ClosestPosition = num;
            this.closestPuzzleSlot = puzzleSlot;
            if (!this.addedOutline)
            {
              this.Outliner.OutlineLayers[0].Clear();
              this.Outliner.OutlineLayers[0].Add(this.closestPuzzleSlot.gameObject);
              this.addedOutline = true;
            }
          }
          this.FoundOne = true;
          this.closestPuzzleSlot = puzzleSlot;
        }
      }
      if (!this.FoundOne)
      {
        if ((UnityEngine.Object) this.closestPuzzleSlot != (UnityEngine.Object) null && this.addedOutline)
        {
          this.Outliner.OutlineLayers[0].Clear();
          this.addedOutline = false;
        }
        this.closestPuzzleSlot = (PuzzleSlot) null;
        this.ClosestPosition = 100f;
        if (!((UnityEngine.Object) this.playerFarming.PuzzlePieceCarried == (UnityEngine.Object) this))
          return;
        this.Label = "Drop";
        this.HasChanged = true;
        this.playerFarming.interactor.indicator.text.text = ScriptLocalization.Interactions.Drop;
      }
      else
      {
        if (!((UnityEngine.Object) this.playerFarming.PuzzlePieceCarried == (UnityEngine.Object) this))
          return;
        this.Label = "Place";
        this.HasChanged = true;
        this.playerFarming.interactor.indicator.text.text = ScriptLocalization.Interactions.PlaceBuilding;
      }
    }
    else
    {
      if (!((UnityEngine.Object) this.playerFarming.PuzzlePieceCarried == (UnityEngine.Object) null))
        return;
      this.Label = ScriptLocalization.Interactions.PickUp;
      if (this.type == Interaction_PuzzleItem.PuzzleItemType.Switch)
        this.Label = this.Interactable ? ScriptLocalization.FollowerInteractions.MakeDemand : "";
      else if (this.type == Interaction_PuzzleItem.PuzzleItemType.TrapCharger)
        this.Label = "";
      this.HasChanged = true;
      if (!(bool) (UnityEngine.Object) this.playerFarming || !((UnityEngine.Object) this.playerFarming.currentInteraction == (UnityEngine.Object) this))
        return;
      this.playerFarming.interactor.indicator.text.text = this.Label;
    }
  }

  public static List<StructureBrain.TYPES> GetDecorationsToUnlock()
  {
    return new List<StructureBrain.TYPES>()
    {
      StructureBrain.TYPES.DECORATION_DST_ALCHEMY,
      StructureBrain.TYPES.DECORATION_DST_DEERCLOPS,
      StructureBrain.TYPES.DECORATION_DST_MARBLETREE,
      StructureBrain.TYPES.DECORATION_DST_PIGSTICK,
      StructureBrain.TYPES.DECORATION_DST_SCIENCEMACHINE,
      StructureBrain.TYPES.DECORATION_DST_TREE,
      StructureBrain.TYPES.DECORATION_DST_WALL
    };
  }

  public IEnumerator PickUpBody()
  {
    Interaction_PuzzleItem puzzleItem = this;
    if ((UnityEngine.Object) puzzleItem.selectedSlot != (UnityEngine.Object) null)
    {
      puzzleItem.selectedSlot.Added((Interaction_PuzzleItem) null);
      puzzleItem.selectedSlot = (PuzzleSlot) null;
    }
    puzzleItem.carrying = true;
    puzzleItem.playerFarming.PuzzlePieceCarried = puzzleItem;
    AudioManager.Instance.PlayOneShot("event:/player/body_pickup", puzzleItem.gameObject);
    puzzleItem.container.gameObject.SetActive(false);
    puzzleItem.playerFarming.playerController.SetSpecialMovingAnimations("egg/idle", "egg/run-up", "egg/run-down", "egg/run", "egg/run-up-diagonal", "egg/run-horizontal");
    puzzleItem.playerFarming.Spine.Skeleton.Skin.AddSkin(puzzleItem.playerFarming.Spine.Skeleton.Data.FindSkin(puzzleItem.skinName));
    puzzleItem.playerFarming.state.CURRENT_STATE = StateMachine.State.Idle_CarryingBody;
    while (!InputManager.Gameplay.GetInteractButtonUp(puzzleItem.playerFarming))
      yield return (object) null;
    bool incorrect = false;
    while (!((UnityEngine.Object) puzzleItem.playerFarming == (UnityEngine.Object) null) && puzzleItem.playerFarming.gameObject.activeSelf)
    {
      if (!LetterBox.IsPlaying && puzzleItem.playerFarming.state.CURRENT_STATE != StateMachine.State.Idle_CarryingBody && puzzleItem.playerFarming.state.CURRENT_STATE != StateMachine.State.Moving_CarryingBody)
        puzzleItem.playerFarming.state.CURRENT_STATE = StateMachine.State.Idle_CarryingBody;
      if (InputManager.Gameplay.GetInteractButtonHeld(puzzleItem.playerFarming) && !MonoSingleton<UIManager>.Instance.MenusBlocked && (double) Time.timeScale >= 1.0)
      {
        if ((UnityEngine.Object) puzzleItem.closestPuzzleSlot != (UnityEngine.Object) null && !puzzleItem.closestPuzzleSlot.IsCorrect(puzzleItem))
        {
          puzzleItem.Interactable = false;
          puzzleItem.playerFarming.indicator.PlayShake();
          puzzleItem.DropBody();
          puzzleItem.Outliner.OutlineLayers[0].Clear();
          puzzleItem.transform.position = puzzleItem.playerFarming.transform.position;
          AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive", puzzleItem.gameObject);
          yield return (object) puzzleItem.MoveToStatue(onShakeStartSFX: "event:/dlc/env/puzzle_room/hat_place_fail");
          incorrect = true;
          puzzleItem.FoundOne = false;
          puzzleItem.Interactable = true;
          break;
        }
        break;
      }
      yield return (object) null;
    }
    if (incorrect)
    {
      Debug.Log((object) "Negative feedback received, resetting");
      AudioManager.Instance.PlayOneShot("event:/Stings/generic_negative", puzzleItem.gameObject);
      puzzleItem.startPos = new Vector3(puzzleItem.closestPuzzleSlot.transform.position.x * 0.8f, puzzleItem.closestPuzzleSlot.transform.position.y * 0.8f, 0.0f);
      CameraManager.instance.ShakeCameraForDuration(1f, 1f, 0.33f);
      yield return (object) puzzleItem.transform.DOMove(puzzleItem.startPos, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).WaitForCompletion();
    }
    else if (puzzleItem.FoundOne)
    {
      puzzleItem.DropBody();
      puzzleItem.Outliner.OutlineLayers[0].Clear();
      puzzleItem.transform.position = puzzleItem.playerFarming.transform.position;
      AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive", puzzleItem.gameObject);
      AudioManager.Instance.PlayOneShot("event:/dlc/env/puzzle_room/hat_place_success", puzzleItem.gameObject);
      yield return (object) puzzleItem.MoveToStatue(0.5f);
      Vector3 endValue1 = new Vector3(puzzleItem.closestPuzzleSlot.transform.position.x, puzzleItem.closestPuzzleSlot.transform.position.y + 1.54f, -2.625f);
      Vector3 endValue2 = new Vector3(puzzleItem.closestPuzzleSlot.transform.position.x, puzzleItem.closestPuzzleSlot.transform.position.y + 0.88f, -1.5f);
      DG.Tweening.Sequence sequence = DOTween.Sequence();
      sequence.Append((Tween) puzzleItem.transform.DOMove(endValue1, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad));
      sequence.Append((Tween) puzzleItem.transform.DOMove(endValue2, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InQuad));
      sequence.Play<DG.Tweening.Sequence>();
      yield return (object) sequence.WaitForCompletion();
      CameraManager.instance.ShakeCameraForDuration(3f, 3f, 0.25f);
      puzzleItem.closestPuzzleSlot.Added(puzzleItem);
      puzzleItem.selectedSlot = puzzleItem.closestPuzzleSlot;
      BiomeConstants.Instance.EmitSmokeExplosionVFX(puzzleItem.closestPuzzleSlot.transform.position);
      AudioManager.Instance.PlayOneShot("event:/player/body_drop_grave", puzzleItem.gameObject);
      AudioManager.Instance.PlayOneShot("event:/dlc/env/puzzle_room/hat_place_success_impact", puzzleItem.gameObject);
      puzzleItem.transform.position = puzzleItem.closestPuzzleSlot.transform.position;
      puzzleItem.Outliner.OutlineLayers[0].Clear();
      puzzleItem.gameObject.SetActive(false);
    }
    else
    {
      puzzleItem.transform.position = puzzleItem.playerFarming.transform.position;
      puzzleItem.DropBody();
    }
  }

  public IEnumerator MoveToStatue(float shakeMult = 1f, string onShakeStartSFX = "")
  {
    Interaction_PuzzleItem interactionPuzzleItem = this;
    Vector3 endValue = new Vector3(interactionPuzzleItem.closestPuzzleSlot.transform.position.x, interactionPuzzleItem.closestPuzzleSlot.transform.position.y + 1.1f, -1.875f);
    Vector3 position = interactionPuzzleItem.closestPuzzleSlot.transform.position;
    float num = 0.12f * shakeMult;
    DG.Tweening.Sequence sequence1 = DOTween.Sequence();
    sequence1.AppendInterval(0.5f * shakeMult);
    sequence1.Append((Tween) interactionPuzzleItem.closestPuzzleSlot.transform.DOMove(position + Vector3.left * num, 0.09f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad));
    sequence1.Append((Tween) interactionPuzzleItem.closestPuzzleSlot.transform.DOMove(position + Vector3.right * num, 0.18f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutQuad));
    sequence1.Append((Tween) interactionPuzzleItem.closestPuzzleSlot.transform.DOMove(position, 0.09f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InQuad));
    DG.Tweening.Sequence sequence2 = DOTween.Sequence();
    sequence2.Append((Tween) interactionPuzzleItem.transform.DOMove(endValue, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad));
    sequence2.AppendInterval(0.5f * shakeMult);
    sequence2.Append((Tween) interactionPuzzleItem.transform.DOMove(endValue, 0.1f));
    sequence1.Play<DG.Tweening.Sequence>();
    sequence2.Play<DG.Tweening.Sequence>();
    if (!string.IsNullOrEmpty(onShakeStartSFX))
      AudioManager.Instance.PlayOneShot(onShakeStartSFX, interactionPuzzleItem.gameObject);
    yield return (object) sequence2.WaitForCompletion();
  }

  public void DropBody()
  {
    this.playerFarming.playerController.ResetSpecialMovingAnimations();
    this.playerFarming.state.CURRENT_STATE = StateMachine.State.Idle;
    this.playerFarming.PuzzlePieceCarried = (Interaction_PuzzleItem) null;
    if (!this.carrying)
      return;
    this.carrying = false;
    AudioManager.Instance.PlayOneShot("event:/player/body_drop", this.gameObject);
    this.container.gameObject.SetActive(true);
  }

  [Serializable]
  public enum PuzzleItemType
  {
    Pickupable,
    Switch,
    TrapCharger,
  }
}
