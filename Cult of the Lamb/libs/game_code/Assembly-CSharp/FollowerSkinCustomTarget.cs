// Decompiled with JetBrains decompiler
// Type: FollowerSkinCustomTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerSkinCustomTarget : MonoBehaviour
{
  public const string Path = "Prefabs/Resources/FollowerSkin Custom Target";
  public SpriteRenderer SpriteRenderer;
  public PlayerFarming pFarming;
  public CameraFollowTarget c;
  public GameObject Menu;
  public TarotCards.TarotCard DrawnCard;
  public System.Action Callback;
  public bool FollowerSkinForceSelection;
  public SkeletonDataAsset Spine;
  public List<string> FollowerSkinsAvailable = new List<string>();
  public string PickedSkin;
  public bool Activating;

  public static void Create(
    Vector3 StartPosition,
    Vector3 EndPosition,
    float Duration,
    string Skin,
    System.Action Callback)
  {
    Transform parent = !((UnityEngine.Object) RoomManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) RoomManager.Instance.CurrentRoomPrefab.transform != (UnityEngine.Object) null) ? GameObject.FindGameObjectWithTag("Unit Layer").transform : RoomManager.Instance.CurrentRoomPrefab.transform;
    FollowerSkinCustomTarget.Create(StartPosition, EndPosition, parent, Duration, Skin, Callback);
  }

  public static FollowerSkinCustomTarget Create(
    Vector3 startPosition,
    Vector3 endPosition,
    Transform parent,
    float duration,
    string skin,
    System.Action callback)
  {
    FollowerSkinCustomTarget component = (UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Resources/FollowerSkin Custom Target"), startPosition + Vector3.back * 0.5f, Quaternion.identity, parent) as GameObject).GetComponent<FollowerSkinCustomTarget>();
    component.Play(endPosition, duration, skin, callback);
    return component;
  }

  public StateMachine state => PlayerFarming.Instance.state;

  public void Play(Vector3 EndPosition, float Duration, string Skin, System.Action Callback)
  {
    this.PickedSkin = Skin;
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
    Debug.Log((object) ("Show Menu: Skin = " + this.PickedSkin));
    UINewItemOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowNewItemOverlay();
    overlayController.Show(UINewItemOverlayController.TypeOfCard.FollowerSkin, PlayerFarming.Instance.gameObject.transform.position, this.PickedSkin);
    overlayController.OnHidden = overlayController.OnHidden + new System.Action(this.BackToIdle);
  }

  public void BackToIdle() => this.StartCoroutine((IEnumerator) this.BackToIdleRoutine());

  public IEnumerator BackToIdleRoutine()
  {
    FollowerSkinCustomTarget skinCustomTarget = this;
    LetterBox.Hide();
    HUD_Manager.Instance.Show(0);
    GameManager.GetInstance().CameraResetTargetZoom();
    skinCustomTarget.state.CURRENT_STATE = StateMachine.State.Idle;
    yield return (object) null;
    skinCustomTarget.StopAllCoroutines();
    GameManager.GetInstance().StartCoroutine((IEnumerator) skinCustomTarget.DelayEffectsRoutine());
    System.Action callback = skinCustomTarget.Callback;
    if (callback != null)
      callback();
  }

  public IEnumerator DelayEffectsRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    FollowerSkinCustomTarget skinCustomTarget = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      UnityEngine.Object.Destroy((UnityEngine.Object) skinCustomTarget.gameObject);
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
  public void \u003CPlay\u003Eb__15_0()
  {
    this.SpriteRenderer.enabled = false;
    this.OpenMenu();
  }
}
