// Decompiled with JetBrains decompiler
// Type: Interaction_FlockadePiece
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using Flockade;
using I2.Loc;
using Lamb.UI;
using src.Extensions;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_FlockadePiece : Interaction
{
  [SerializeField]
  public FlockadePieceType pieceType;
  [SerializeField]
  public SpriteRenderer pieceVisual;
  [SerializeField]
  public GameObject shadow;
  public string pickUpLabel;
  public bool Activated;
  public bool unlockRoomOnCollect;
  public DataManager.Variables requiresOpponentBeaten = DataManager.Variables.FlockadeFlockadeWon;

  public override void OnEnable()
  {
    base.OnEnable();
    if (!FlockadePieceManager.IsPieceUnlocked(this.pieceType) && !this.requiresOpponentBeaten.Equals((object) false))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void setPieceType(FlockadePieceType type)
  {
    this.pieceType = type;
    Sprite sprite = this.GetSprite();
    if (!((UnityEngine.Object) sprite != (UnityEngine.Object) null))
      return;
    this.pieceVisual.sprite = sprite;
  }

  public void Start()
  {
    this.pickUpLabel = ScriptLocalization.Interactions.PickUp;
    this.UpdateLocalisation();
    if (FlockadePieceManager.IsPieceUnlocked(this.pieceType))
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    this.setPieceType(this.pieceType);
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.pickUpLabel = ScriptLocalization.Interactions.PickUp;
  }

  public override void GetLabel()
  {
    this.Interactable = true;
    this.Label = this.pickUpLabel;
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activated)
      return;
    base.OnInteract(state);
    this.Activated = true;
    state.CURRENT_STATE = StateMachine.State.InActive;
    this.DoPickUpRoutine();
  }

  public Sprite GetSprite()
  {
    FlockadeGamePieceConfiguration piece = FlockadePieceManager.GetPiecesPool().GetPiece(this.pieceType);
    return (UnityEngine.Object) piece != (UnityEngine.Object) null ? piece.Image : (Sprite) null;
  }

  public void DoPickUpRoutine() => this.StartCoroutine((IEnumerator) this.PickUpRoutine());

  public IEnumerator PickUpRoutine()
  {
    Interaction_FlockadePiece interactionFlockadePiece = this;
    interactionFlockadePiece.shadow.SetActive(false);
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", interactionFlockadePiece.transform.position);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionFlockadePiece.playerFarming.gameObject, 5f);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    FlockadePieceManager.UnlockPiece(interactionFlockadePiece.pieceType);
    PlayerSimpleInventory component = interactionFlockadePiece.state.gameObject.GetComponent<PlayerSimpleInventory>();
    Vector3 pieceTargetPosition = new Vector3(component.ItemImage.transform.position.x, component.ItemImage.transform.position.y + 1.5f, -1f);
    interactionFlockadePiece.pieceVisual.transform.DOShakeScale(2.5f, 0.1f);
    interactionFlockadePiece.state.CURRENT_STATE = StateMachine.State.FoundItem;
    float Timer = 0.0f;
    while ((double) (Timer += Time.unscaledDeltaTime) < 2.0)
    {
      interactionFlockadePiece.transform.position = Vector3.Lerp(interactionFlockadePiece.transform.position, pieceTargetPosition, 5f * Time.deltaTime);
      yield return (object) null;
    }
    interactionFlockadePiece.transform.position = pieceTargetPosition;
    yield return (object) new WaitForSeconds(0.5f);
    interactionFlockadePiece.pieceVisual.enabled = false;
    System.Threading.Tasks.Task task = MonoSingleton<UIManager>.Instance.LoadFlockadePiecesAssets();
    yield return (object) new WaitUntil((Func<bool>) (() => task.IsCompleted));
    UIFlockadePiecesMenuController piecesMenuController = MonoSingleton<UIManager>.Instance.FlockadePiecesMenuTemplate.Instantiate<UIFlockadePiecesMenuController>();
    piecesMenuController.Show(interactionFlockadePiece.pieceType, interactionFlockadePiece.playerFarming);
    piecesMenuController.OnHide = piecesMenuController.OnHide + new System.Action(interactionFlockadePiece.BackToIdle);
    if (interactionFlockadePiece.unlockRoomOnCollect)
      RoomLockController.RoomCompleted(true);
  }

  public void BackToIdle()
  {
    this.StartCoroutine((IEnumerator) this.BackToIdleRoutine(this.playerFarming));
  }

  public IEnumerator BackToIdleRoutine(PlayerFarming playerFarming)
  {
    Interaction_FlockadePiece interactionFlockadePiece = this;
    Time.timeScale = 1f;
    LetterBox.Hide();
    HUD_Manager.Instance.Show(0);
    AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_close", interactionFlockadePiece.gameObject);
    GameManager.GetInstance().CameraResetTargetZoom();
    CameraFollowTarget.Instance.DisablePlayerLook = false;
    yield return (object) new WaitForSeconds(0.5f);
    playerFarming.Spine.AnimationState.SetAnimation(0, "idle", true);
    foreach (Health health in Health.team2)
    {
      if ((UnityEngine.Object) health != (UnityEngine.Object) null)
        health.ClearFreezeTime();
    }
    Health.isGlobalTimeFreeze = false;
    PlayerFarming.SetStateForAllPlayers();
    playerFarming.Spine.UseDeltaTime = true;
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionFlockadePiece.gameObject);
  }
}
