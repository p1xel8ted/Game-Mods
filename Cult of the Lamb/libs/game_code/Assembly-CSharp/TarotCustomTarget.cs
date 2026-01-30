// Decompiled with JetBrains decompiler
// Type: TarotCustomTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using src.Extensions;
using src.UINavigator;
using System.Collections;
using UnityEngine;

#nullable disable
public class TarotCustomTarget : MonoBehaviour
{
  public const string Path = "Prefabs/Resources/Tarot Custom Target";
  public SpriteRenderer SpriteRenderer;
  public TarotCards.Card CardOverride = TarotCards.Card.Count;
  [SerializeField]
  public Sprite normalTarotCard;
  [SerializeField]
  public Sprite dlcTarotCard;
  public CameraFollowTarget c;
  public GameObject Menu;
  public TarotCards.Card CardType = TarotCards.Card.Count;
  public TarotCards.TarotCard DrawnCard;
  public System.Action Callback;
  public bool Activating;

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

  public StateMachine state => PlayerFarming.Instance.state;

  public void Play(Vector3 EndPosition, float Duration, TarotCards.Card CardType, System.Action Callback)
  {
    this.CardType = CardType;
    this.Callback = Callback;
    this.SpriteRenderer.sprite = !TarotCards.DLCWithBlueBack.Contains<TarotCards.Card>(CardType) ? this.normalTarotCard : this.dlcTarotCard;
    PlayerFarming playerFarming = this.state.GetComponent<PlayerFarming>();
    this.transform.localScale = Vector3.zero;
    this.transform.DOScale(Vector3.one, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    DG.Tweening.Sequence sequence = DOTween.Sequence();
    sequence.AppendInterval(0.5f);
    sequence.Append((Tween) this.transform.DOMove(EndPosition + Vector3.back * 0.5f, Duration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      this.SpriteRenderer.enabled = false;
      this.StartCoroutine((IEnumerator) this.DoRoutineRoutine(playerFarming));
    })));
    sequence.Play<DG.Tweening.Sequence>();
  }

  public IEnumerator DoRoutineRoutine(PlayerFarming playerFarming)
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
      tarotCustomTarget.OpenMenu(playerFarming);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    GameManager.GetInstance().CameraSetTargetZoom(4f);
    tarotCustomTarget.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    tarotCustomTarget.state.facingAngle = -90f;
    tarotCustomTarget.c = CameraFollowTarget.Instance;
    tarotCustomTarget.c.DisablePlayerLook = true;
    AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_pull", tarotCustomTarget.gameObject);
    playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    playerFarming.simpleSpineAnimator.Animate("cards/cards-start", 0, false);
    playerFarming.simpleSpineAnimator.AddAnimate("cards/cards-loop", 0, true, 0.0f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(1f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void OpenMenu(PlayerFarming playerFarming)
  {
    HUD_Manager.Instance.Hide(false, 0);
    GameManager.GetInstance().CameraSetOffset(new Vector3(-3f, 0.0f, 0.0f));
    MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = playerFarming;
    UITarotCardsMenuController cardsMenuController = MonoSingleton<UIManager>.Instance.TarotCardsMenuTemplate.Instantiate<UITarotCardsMenuController>();
    cardsMenuController.Show(this.CardType);
    cardsMenuController.OnHide = cardsMenuController.OnHide + (System.Action) (() => HUD_Manager.Instance.Show(0));
    cardsMenuController.OnHidden = cardsMenuController.OnHidden + (System.Action) (() =>
    {
      GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, 0.0f));
      this.BackToIdle(playerFarming);
    });
    this.DrawnCard = new TarotCards.TarotCard(this.CardType, 0);
  }

  public void BackToIdle(PlayerFarming playerFarming)
  {
    this.StartCoroutine((IEnumerator) this.BackToIdleRoutine(playerFarming));
  }

  public IEnumerator BackToIdleRoutine(PlayerFarming playerFarming)
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
    playerFarming.simpleSpineAnimator.Animate("cards/cards-stop-seperate", 0, false);
    playerFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(1.83333337f);
    tarotCustomTarget.StopAllCoroutines();
    PlayerFarming.SetStateForAllPlayers();
    GameManager.GetInstance().StartCoroutine((IEnumerator) tarotCustomTarget.DelayEffectsRoutine(playerFarming));
    System.Action callback = tarotCustomTarget.Callback;
    if (callback != null)
      callback();
  }

  public IEnumerator DelayEffectsRoutine(PlayerFarming playerFarming)
  {
    TarotCustomTarget tarotCustomTarget = this;
    yield return (object) new WaitForSeconds(0.2f);
    if (LocationManager.LocationIsDungeon(PlayerFarming.Location) && !PlayerFleeceManager.FleecePreventTarotCards())
      TrinketManager.AddTrinket(tarotCustomTarget.DrawnCard, playerFarming);
    NotificationCentre.Instance.PlayGenericNotification("UI/TarotMenu/NewTarotCardUnlocked");
    UnityEngine.Object.Destroy((UnityEngine.Object) tarotCustomTarget.gameObject);
  }
}
