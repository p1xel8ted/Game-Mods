// Decompiled with JetBrains decompiler
// Type: DecorationCustomTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class DecorationCustomTarget : MonoBehaviour
{
  public const string Path = "Prefabs/Resources/Decoration Custom Target";
  public SpriteRenderer SpriteRenderer;
  public PlayerFarming pFarming;
  public CameraFollowTarget c;
  public StructureBrain.TYPES DecorationType;
  public System.Action Callback;
  public bool Activating;

  public static GameObject Create(
    Vector3 StartPosition,
    Vector3 EndPosition,
    float Duration,
    StructureBrain.TYPES Decoration,
    System.Action Callback)
  {
    Transform parent = !((UnityEngine.Object) RoomManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) RoomManager.Instance.CurrentRoomPrefab.transform != (UnityEngine.Object) null) ? GameObject.FindGameObjectWithTag("Unit Layer").transform : RoomManager.Instance.CurrentRoomPrefab.transform;
    return DecorationCustomTarget.Create(StartPosition, EndPosition, Duration, Decoration, parent, Callback);
  }

  public static GameObject Create(
    Vector3 StartPosition,
    Vector3 EndPosition,
    float Duration,
    StructureBrain.TYPES Decoration,
    Transform parent,
    System.Action Callback)
  {
    DecorationCustomTarget component = (UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Resources/Decoration Custom Target"), StartPosition + Vector3.back * 0.5f, Quaternion.identity, parent) as GameObject).GetComponent<DecorationCustomTarget>();
    component.Play(EndPosition, Duration, Decoration, Callback);
    return component.gameObject;
  }

  public StateMachine state => PlayerFarming.Instance.state;

  public void Play(
    Vector3 EndPosition,
    float Duration,
    StructureBrain.TYPES Decoration,
    System.Action Callback)
  {
    this.DecorationType = Decoration;
    this.Callback = Callback;
    this.transform.localScale = Vector3.zero;
    this.transform.DOScale(Vector3.one, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    DG.Tweening.Sequence sequence = DOTween.Sequence();
    sequence.AppendInterval(0.5f);
    sequence.Append((Tween) this.transform.DOMove(EndPosition + Vector3.back * 0.5f, Duration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      this.SpriteRenderer.enabled = false;
      this.OpenMenu();
    })));
    sequence.Play<DG.Tweening.Sequence>();
  }

  public void OpenMenu()
  {
    AudioManager.Instance.PlayOneShot("event:/player/new_item_pickup", this.gameObject);
    BiomeConstants.Instance.EmitPickUpVFX(this.transform.position);
    CameraManager.instance.ShakeCameraForDuration(0.7f, 0.9f, 0.3f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
    if (this.DecorationType != StructureBrain.TYPES.NONE)
    {
      StructuresData.CompleteResearch(this.DecorationType);
      StructuresData.SetRevealed(this.DecorationType);
    }
    UINewItemOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowNewItemOverlay();
    overlayController.pickedBuilding = this.DecorationType;
    overlayController.Show(UINewItemOverlayController.TypeOfCard.Decoration, this.transform.position, false);
    overlayController.OnHidden = overlayController.OnHidden + new System.Action(this.BackToIdle);
  }

  public void BackToIdle() => this.StartCoroutine((IEnumerator) this.BackToIdleRoutine());

  public IEnumerator BackToIdleRoutine()
  {
    DecorationCustomTarget decorationCustomTarget = this;
    LetterBox.Hide();
    HUD_Manager.Instance.Show(0);
    GameManager.GetInstance().CameraResetTargetZoom();
    decorationCustomTarget.state.CURRENT_STATE = StateMachine.State.Idle;
    yield return (object) null;
    decorationCustomTarget.StopAllCoroutines();
    GameManager.GetInstance().StartCoroutine((IEnumerator) decorationCustomTarget.DelayEffectsRoutine());
    System.Action callback = decorationCustomTarget.Callback;
    if (callback != null)
      callback();
  }

  public IEnumerator DelayEffectsRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    DecorationCustomTarget decorationCustomTarget = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      UnityEngine.Object.Destroy((UnityEngine.Object) decorationCustomTarget.gameObject);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(1f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  [CompilerGenerated]
  public void \u003CPlay\u003Eb__10_0()
  {
    this.SpriteRenderer.enabled = false;
    this.OpenMenu();
  }
}
