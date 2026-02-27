// Decompiled with JetBrains decompiler
// Type: Interaction_FlockadeMultiplePieces
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using Flockade;
using I2.Loc;
using Lamb.UI;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_FlockadeMultiplePieces : Interaction
{
  [SerializeField]
  public List<FlockadePieceType> piecesToUnlock;
  [SerializeField]
  public SpriteRenderer itemSpriteRenderer;
  [SerializeField]
  public GameObject shadow;
  public bool Activated;

  public string pickUpLabel => ScriptLocalization.Interactions.PickUp;

  public override void OnEnable()
  {
    base.OnEnable();
    this.HandleUnlockedDestruction();
  }

  public void Start()
  {
    this.UpdateLocalisation();
    this.HandleUnlockedDestruction();
  }

  public override void UpdateLocalisation() => base.UpdateLocalisation();

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

  public void DoPickUpRoutine() => this.StartCoroutine(this.PickUpRoutine());

  public IEnumerator PickUpRoutine()
  {
    Interaction_FlockadeMultiplePieces flockadeMultiplePieces = this;
    flockadeMultiplePieces.shadow.SetActive(false);
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", flockadeMultiplePieces.transform.position);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(flockadeMultiplePieces.playerFarming.gameObject, 5f);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    foreach (FlockadePieceType piece in flockadeMultiplePieces.piecesToUnlock)
      FlockadePieceManager.UnlockPiece(piece);
    PlayerSimpleInventory component = flockadeMultiplePieces.state.gameObject.GetComponent<PlayerSimpleInventory>();
    Vector3 pieceTargetPosition = new Vector3(component.ItemImage.transform.position.x, component.ItemImage.transform.position.y + 1.5f, -1f);
    flockadeMultiplePieces.itemSpriteRenderer.transform.DOShakeScale(2.5f, 0.1f);
    flockadeMultiplePieces.state.CURRENT_STATE = StateMachine.State.FoundItem;
    float Timer = 0.0f;
    while ((double) (Timer += Time.unscaledDeltaTime) < 2.0)
    {
      flockadeMultiplePieces.transform.position = Vector3.Lerp(flockadeMultiplePieces.transform.position, pieceTargetPosition, 5f * Time.deltaTime);
      yield return (object) null;
    }
    flockadeMultiplePieces.transform.position = pieceTargetPosition;
    yield return (object) new WaitForSeconds(0.5f);
    flockadeMultiplePieces.itemSpriteRenderer.enabled = false;
    System.Threading.Tasks.Task task = MonoSingleton<UIManager>.Instance.LoadFlockadePiecesAssets();
    yield return (object) new WaitUntil((Func<bool>) (() => task.IsCompleted));
    UIFlockadePiecesMenuController piecesMenuController = MonoSingleton<UIManager>.Instance.FlockadePiecesMenuTemplate.Instantiate<UIFlockadePiecesMenuController>();
    piecesMenuController.Show(flockadeMultiplePieces.piecesToUnlock, flockadeMultiplePieces.playerFarming);
    piecesMenuController.OnHide = piecesMenuController.OnHide + new System.Action(flockadeMultiplePieces.BackToIdle);
  }

  public void BackToIdle() => this.StartCoroutine(this.BackToIdleRoutine(this.playerFarming));

  public IEnumerator BackToIdleRoutine(PlayerFarming playerFarming)
  {
    Interaction_FlockadeMultiplePieces flockadeMultiplePieces = this;
    Time.timeScale = 1f;
    LetterBox.Hide();
    HUD_Manager.Instance.Show(0);
    AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_close", flockadeMultiplePieces.gameObject);
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
    UnityEngine.Object.Destroy((UnityEngine.Object) flockadeMultiplePieces.gameObject);
  }

  public void HandleUnlockedDestruction()
  {
    bool flag = true;
    foreach (FlockadePieceType piece in this.piecesToUnlock)
    {
      if (!FlockadePieceManager.IsPieceUnlocked(piece))
        flag = false;
    }
    if (!flag)
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }
}
