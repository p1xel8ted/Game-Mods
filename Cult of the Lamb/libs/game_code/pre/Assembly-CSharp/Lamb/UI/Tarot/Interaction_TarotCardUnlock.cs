// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Tarot.Interaction_TarotCardUnlock
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using src.Extensions;
using System.Collections;
using UnityEngine;

#nullable disable
namespace Lamb.UI.Tarot;

public class Interaction_TarotCardUnlock : Interaction
{
  private string sSummonTrinket;
  private string sNoAvailableTrinkets;
  public bool Activated;
  public SpriteRenderer sprite;
  private PlayerFarming pFarming;
  private CameraFollowTarget c;
  private int devotionCost;
  public GameObject Menu;
  public TarotCards.Card CardOverride = TarotCards.Card.Count;
  public Transform SpawnPosition;
  private TarotCards.TarotCard DrawnCard;
  private bool Activating;

  private void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sSummonTrinket = ScriptLocalization.Interactions.PickUp;
  }

  public override void GetLabel()
  {
    this.Interactable = true;
    this.Label = this.Activated ? "" : this.sSummonTrinket;
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activated)
      return;
    base.OnInteract(state);
    this.Activated = true;
    state.CURRENT_STATE = StateMachine.State.InActive;
    this.DoRoutine();
  }

  private IEnumerator CentrePlayer()
  {
    Interaction_TarotCardUnlock interactionTarotCardUnlock = this;
    float Progress = 0.0f;
    Vector3 StartPosition = interactionTarotCardUnlock.state.transform.position;
    while ((double) (Progress += Time.deltaTime * 2f) < 0.5)
    {
      interactionTarotCardUnlock.state.transform.position = Vector3.Lerp(StartPosition, interactionTarotCardUnlock.transform.position + Vector3.down, Mathf.SmoothStep(0.0f, 1f, Progress));
      yield return (object) null;
    }
  }

  private void DoRoutine() => this.StartCoroutine((IEnumerator) this.DoRoutineRoutine());

  private IEnumerator DoRoutineRoutine()
  {
    Interaction_TarotCardUnlock interactionTarotCardUnlock = this;
    // ISSUE: reference to a compiler-generated method
    interactionTarotCardUnlock.transform.DOMove(PlayerFarming.Instance.CameraBone.transform.position + new Vector3(0.0f, 0.0f, -1f), 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(interactionTarotCardUnlock.\u003CDoRoutineRoutine\u003Eb__17_0));
    HUD_Manager.Instance.Hide(false, 0);
    interactionTarotCardUnlock.transform.DOScale(0.0f, 1f);
    GameManager.GetInstance().CameraSetTargetZoom(4f);
    LetterBox.Show(false);
    interactionTarotCardUnlock.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    interactionTarotCardUnlock.state.facingAngle = -90f;
    interactionTarotCardUnlock.c = CameraFollowTarget.Instance;
    interactionTarotCardUnlock.c.DisablePlayerLook = true;
    interactionTarotCardUnlock.pFarming = interactionTarotCardUnlock.state.GetComponent<PlayerFarming>();
    interactionTarotCardUnlock.StartCoroutine((IEnumerator) interactionTarotCardUnlock.CentrePlayer());
    AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_pull", interactionTarotCardUnlock.gameObject);
    interactionTarotCardUnlock.pFarming.simpleSpineAnimator.Animate("cards/cards-start", 0, false);
    interactionTarotCardUnlock.pFarming.simpleSpineAnimator.AddAnimate("cards/cards-loop", 0, true, 0.0f);
    NotificationCentre.Instance.PlayGenericNotification("UI/TarotMenu/NewTarotCardUnlocked");
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().CameraSetTargetZoom(6f);
    for (int index = 0; index < interactionTarotCardUnlock.transform.childCount; ++index)
      interactionTarotCardUnlock.transform.GetChild(index).gameObject.SetActive(false);
    interactionTarotCardUnlock.OpenMenu();
  }

  private void OpenMenu()
  {
    HUD_Manager.Instance.Hide(false, 0);
    GameManager.GetInstance().CameraSetOffset(new Vector3(-3f, 0.0f, 0.0f));
    UITarotCardsMenuController cardsMenuController = MonoSingleton<UIManager>.Instance.TarotCardsMenuTemplate.Instantiate<UITarotCardsMenuController>();
    cardsMenuController.Show(this.CardOverride);
    cardsMenuController.OnHide = cardsMenuController.OnHide + (System.Action) (() => HUD_Manager.Instance.Show(0));
    cardsMenuController.OnHidden = cardsMenuController.OnHidden + (System.Action) (() =>
    {
      GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, 0.0f));
      this.BackToIdle();
    });
    this.DrawnCard = new TarotCards.TarotCard(this.CardOverride, 0);
  }

  private void CallBack()
  {
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
    this.Activating = false;
  }

  private void BackToIdle() => this.StartCoroutine((IEnumerator) this.BackToIdleRoutine());

  private IEnumerator BackToIdleRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Interaction_TarotCardUnlock interactionTarotCardUnlock = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      interactionTarotCardUnlock.pFarming.simpleSpineAnimator.Animate("cards/cards-stop-seperate", 0, false);
      interactionTarotCardUnlock.pFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
      interactionTarotCardUnlock.StopAllCoroutines();
      GameManager.GetInstance().StartCoroutine((IEnumerator) interactionTarotCardUnlock.DelayEffectsRoutine());
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    LetterBox.Hide();
    HUD_Manager.Instance.Show(0);
    AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_close", interactionTarotCardUnlock.gameObject);
    GameManager.GetInstance().CameraResetTargetZoom();
    interactionTarotCardUnlock.c.DisablePlayerLook = false;
    interactionTarotCardUnlock.state.CURRENT_STATE = StateMachine.State.Idle;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private IEnumerator DelayEffectsRoutine()
  {
    Interaction_TarotCardUnlock interactionTarotCardUnlock = this;
    yield return (object) new WaitForSeconds(0.2f);
    if (LocationManager.LocationIsDungeon(PlayerFarming.Location))
      TrinketManager.AddTrinket(interactionTarotCardUnlock.DrawnCard);
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionTarotCardUnlock.gameObject);
  }
}
