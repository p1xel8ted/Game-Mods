// Decompiled with JetBrains decompiler
// Type: FollowerOutfitCustomTarget
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
public class FollowerOutfitCustomTarget : MonoBehaviour
{
  public const string Path = "Prefabs/Resources/Follower Outfit Custom Target";
  public SpriteRenderer SpriteRenderer;
  public PlayerFarming pFarming;
  public CameraFollowTarget c;
  public TarotCards.TarotCard DrawnCard;
  public System.Action Callback;
  public FollowerClothingType PickedOutfit;
  public bool Activating;

  public static void Create(
    Vector3 StartPosition,
    Vector3 EndPosition,
    float Duration,
    FollowerClothingType Skin,
    System.Action Callback)
  {
    Transform parent = !((UnityEngine.Object) RoomManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) RoomManager.Instance.CurrentRoomPrefab.transform != (UnityEngine.Object) null) ? GameObject.FindGameObjectWithTag("Unit Layer").transform : RoomManager.Instance.CurrentRoomPrefab.transform;
    FollowerOutfitCustomTarget.Create(StartPosition, EndPosition, parent, Duration, Skin, Callback);
  }

  public static FollowerOutfitCustomTarget Create(
    Vector3 startPosition,
    Vector3 endPosition,
    Transform parent,
    float duration,
    FollowerClothingType skin,
    System.Action callback)
  {
    FollowerOutfitCustomTarget component = (UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Resources/Follower Outfit Custom Target"), startPosition + Vector3.back * 0.5f, Quaternion.identity, parent) as GameObject).GetComponent<FollowerOutfitCustomTarget>();
    component.Play(endPosition, duration, skin, callback);
    return component;
  }

  public StateMachine state => PlayerFarming.Instance.state;

  public void Play(
    Vector3 EndPosition,
    float Duration,
    FollowerClothingType Skin,
    System.Action Callback)
  {
    this.PickedOutfit = Skin;
    this.Callback = Callback;
    this.transform.localScale = Vector3.zero;
    this.transform.DOScale(Vector3.one, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    DG.Tweening.Sequence sequence = DOTween.Sequence();
    sequence.AppendInterval(0.5f);
    sequence.Append((Tween) this.transform.DOMove(EndPosition + Vector3.back * 0.5f, Duration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      this.SpriteRenderer.enabled = false;
      if (DataManager.Instance.UnlockedClothing.Contains(this.PickedOutfit))
        return;
      this.OpenMenu();
    })));
    sequence.Play<DG.Tweening.Sequence>();
  }

  public void OpenMenu()
  {
    DataManager.Instance.AddNewClothes(this.PickedOutfit);
    NotificationCentre.Instance.PlayGenericNotification("Notifications/OutfitUnlocked/Notification/On", NotificationBase.Flair.Positive);
    Debug.Log((object) ("Show Menu: Skin = " + this.PickedOutfit.ToString()));
    UINewItemOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowNewItemOverlay();
    overlayController.Show(UINewItemOverlayController.TypeOfCard.Outfit, PlayerFarming.Instance.gameObject.transform.position, this.PickedOutfit);
    overlayController.OnHidden = overlayController.OnHidden + new System.Action(this.BackToIdle);
  }

  public void BackToIdle() => this.StartCoroutine((IEnumerator) this.BackToIdleRoutine());

  public IEnumerator BackToIdleRoutine()
  {
    FollowerOutfitCustomTarget outfitCustomTarget = this;
    LetterBox.Hide();
    HUD_Manager.Instance.Show(0);
    GameManager.GetInstance().CameraResetTargetZoom();
    outfitCustomTarget.state.CURRENT_STATE = StateMachine.State.Idle;
    yield return (object) null;
    outfitCustomTarget.StopAllCoroutines();
    GameManager.GetInstance().StartCoroutine((IEnumerator) outfitCustomTarget.DelayEffectsRoutine());
    System.Action callback = outfitCustomTarget.Callback;
    if (callback != null)
      callback();
  }

  public IEnumerator DelayEffectsRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    FollowerOutfitCustomTarget outfitCustomTarget = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      UnityEngine.Object.Destroy((UnityEngine.Object) outfitCustomTarget.gameObject);
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
  public void \u003CPlay\u003Eb__11_0()
  {
    this.SpriteRenderer.enabled = false;
    if (DataManager.Instance.UnlockedClothing.Contains(this.PickedOutfit))
      return;
    this.OpenMenu();
  }
}
