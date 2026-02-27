// Decompiled with JetBrains decompiler
// Type: TarotCustomTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using src.Extensions;
using System.Collections;
using UnityEngine;

#nullable disable
public class TarotCustomTarget : MonoBehaviour
{
  private const string Path = "Prefabs/Resources/Tarot Custom Target";
  public SpriteRenderer SpriteRenderer;
  public TarotCards.Card CardOverride = TarotCards.Card.Count;
  private PlayerFarming pFarming;
  private CameraFollowTarget c;
  public GameObject Menu;
  public TarotCards.Card CardType = TarotCards.Card.Count;
  private TarotCards.TarotCard DrawnCard;
  private System.Action Callback;
  private bool Activating;

  public static TarotCustomTarget Create(
    Vector3 StartPosition,
    Vector3 EndPosition,
    float Duration,
    TarotCards.Card CardType,
    System.Action Callback)
  {
    Transform parent = !((UnityEngine.Object) RoomManager.Instance != (UnityEngine.Object) null) ? BiomeConstants.Instance.gameObject.transform : (!((UnityEngine.Object) RoomManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) RoomManager.Instance.CurrentRoomPrefab.transform != (UnityEngine.Object) null) ? GameObject.FindGameObjectWithTag("Unit Layer").transform : RoomManager.Instance.CurrentRoomPrefab.transform);
    TarotCustomTarget component = (UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Resources/Tarot Custom Target"), StartPosition + Vector3.back * 0.5f, Quaternion.identity, parent) as GameObject).GetComponent<TarotCustomTarget>();
    component.Play(EndPosition, Duration, CardType, Callback);
    return component;
  }

  private StateMachine state => PlayerFarming.Instance.state;

  public void Play(Vector3 EndPosition, float Duration, TarotCards.Card CardType, System.Action Callback)
  {
    this.CardType = CardType;
    this.Callback = Callback;
    this.transform.localScale = Vector3.zero;
    this.transform.DOScale(Vector3.one, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    DG.Tweening.Sequence sequence = DOTween.Sequence();
    sequence.AppendInterval(0.5f);
    sequence.Append((Tween) this.transform.DOMove(EndPosition + Vector3.back * 0.5f, Duration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      this.SpriteRenderer.enabled = false;
      this.StartCoroutine((IEnumerator) this.DoRoutineRoutine());
    })));
    sequence.Play<DG.Tweening.Sequence>();
  }

  private IEnumerator DoRoutineRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    TarotCustomTarget tarotCustomTarget = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      GameManager.GetInstance().CameraSetTargetZoom(6f);
      tarotCustomTarget.OpenMenu();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    GameManager.GetInstance().CameraSetTargetZoom(4f);
    tarotCustomTarget.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    tarotCustomTarget.state.facingAngle = -90f;
    tarotCustomTarget.c = CameraFollowTarget.Instance;
    tarotCustomTarget.c.DisablePlayerLook = true;
    tarotCustomTarget.pFarming = PlayerFarming.Instance;
    AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_pull", tarotCustomTarget.gameObject);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.simpleSpineAnimator.Animate("cards/cards-start", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("cards/cards-loop", 0, true, 0.0f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(1f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private void OpenMenu()
  {
    HUD_Manager.Instance.Hide(false, 0);
    GameManager.GetInstance().CameraSetOffset(new Vector3(-3f, 0.0f, 0.0f));
    UITarotCardsMenuController cardsMenuController = MonoSingleton<UIManager>.Instance.TarotCardsMenuTemplate.Instantiate<UITarotCardsMenuController>();
    cardsMenuController.Show(this.CardType);
    cardsMenuController.OnHide = cardsMenuController.OnHide + (System.Action) (() => HUD_Manager.Instance.Show(0));
    cardsMenuController.OnHidden = cardsMenuController.OnHidden + (System.Action) (() =>
    {
      GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, 0.0f));
      this.BackToIdle();
    });
    this.DrawnCard = new TarotCards.TarotCard(this.CardType, 0);
  }

  private void BackToIdle() => this.StartCoroutine((IEnumerator) this.BackToIdleRoutine());

  private IEnumerator BackToIdleRoutine()
  {
    TarotCustomTarget tarotCustomTarget = this;
    LetterBox.Hide();
    HUD_Manager.Instance.Show(0);
    AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_close", tarotCustomTarget.gameObject);
    GameManager.GetInstance().CameraResetTargetZoom();
    tarotCustomTarget.c.DisablePlayerLook = false;
    GameManager.GetInstance().RemoveFromCamera(tarotCustomTarget.gameObject);
    GameManager.GetInstance().AddPlayerToCamera();
    yield return (object) null;
    tarotCustomTarget.pFarming.simpleSpineAnimator.Animate("cards/cards-stop-seperate", 0, false);
    tarotCustomTarget.pFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(1.83333337f);
    tarotCustomTarget.StopAllCoroutines();
    tarotCustomTarget.state.CURRENT_STATE = StateMachine.State.Idle;
    GameManager.GetInstance().StartCoroutine((IEnumerator) tarotCustomTarget.DelayEffectsRoutine());
    System.Action callback = tarotCustomTarget.Callback;
    if (callback != null)
      callback();
  }

  private IEnumerator DelayEffectsRoutine()
  {
    TarotCustomTarget tarotCustomTarget = this;
    yield return (object) new WaitForSeconds(0.2f);
    if (LocationManager.LocationIsDungeon(PlayerFarming.Location))
      TrinketManager.AddTrinket(tarotCustomTarget.DrawnCard);
    NotificationCentre.Instance.PlayGenericNotification("UI/TarotMenu/NewTarotCardUnlocked");
    UnityEngine.Object.Destroy((UnityEngine.Object) tarotCustomTarget.gameObject);
  }
}
