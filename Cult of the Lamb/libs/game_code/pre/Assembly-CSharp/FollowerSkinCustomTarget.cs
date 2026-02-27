// Decompiled with JetBrains decompiler
// Type: FollowerSkinCustomTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerSkinCustomTarget : MonoBehaviour
{
  private const string Path = "Prefabs/Resources/FollowerSkin Custom Target";
  public SpriteRenderer SpriteRenderer;
  private PlayerFarming pFarming;
  private CameraFollowTarget c;
  public GameObject Menu;
  private TarotCards.TarotCard DrawnCard;
  private System.Action Callback;
  public bool FollowerSkinForceSelection;
  public SkeletonDataAsset Spine;
  private List<string> FollowerSkinsAvailable = new List<string>();
  public string PickedSkin;
  private bool Activating;

  public static void Create(
    Vector3 StartPosition,
    Vector3 EndPosition,
    float Duration,
    string Skin,
    System.Action Callback)
  {
    Transform parent = !((UnityEngine.Object) RoomManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) RoomManager.Instance.CurrentRoomPrefab.transform != (UnityEngine.Object) null) ? GameObject.FindGameObjectWithTag("Unit Layer").transform : RoomManager.Instance.CurrentRoomPrefab.transform;
    (UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Resources/FollowerSkin Custom Target"), StartPosition + Vector3.back * 0.5f, Quaternion.identity, parent) as GameObject).GetComponent<FollowerSkinCustomTarget>().Play(EndPosition, Duration, Skin, Callback);
  }

  private StateMachine state => PlayerFarming.Instance.state;

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

  private void OpenMenu()
  {
    Debug.Log((object) ("Show Menu: Skin = " + this.PickedSkin));
    UINewItemOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowNewItemOverlay();
    overlayController.Show(UINewItemOverlayController.TypeOfCard.FollowerSkin, PlayerFarming.Instance.gameObject.transform.position, this.PickedSkin);
    overlayController.OnHidden = overlayController.OnHidden + new System.Action(this.BackToIdle);
  }

  private void BackToIdle() => this.StartCoroutine((IEnumerator) this.BackToIdleRoutine());

  private IEnumerator BackToIdleRoutine()
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

  private IEnumerator DelayEffectsRoutine()
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
}
