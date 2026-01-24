// Decompiled with JetBrains decompiler
// Type: Interaction_KeyPiece
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using Lamb.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_KeyPiece : Interaction
{
  public static Interaction_KeyPiece Instance;
  public float Delay = 0.5f;
  public string LabelName;
  public ParticleSystem Particles;
  public SpriteRenderer Image;
  public SpriteRenderer Shadow;
  public List<Sprite> KeyImages = new List<Sprite>();
  public System.Action Callback;
  public Vector3 BookTargetPosition;
  public float Timer;

  public void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.LabelName = ScriptLocalization.Interactions.KeyPiece;
  }

  public override void GetLabel()
  {
    if ((double) this.Delay < 0.0)
      this.Label = this.LabelName;
    else
      this.Label = "";
  }

  public override void Update()
  {
    base.Update();
    this.Delay -= Time.deltaTime;
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.Image.sprite = this.KeyImages[DataManager.Instance.CurrentKeyPieces % this.KeyImages.Count];
    this.Particles.Stop();
    Interaction_KeyPiece.Instance = this;
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    if (!((UnityEngine.Object) Interaction_KeyPiece.Instance == (UnityEngine.Object) this))
      return;
    Interaction_KeyPiece.Instance = (Interaction_KeyPiece) null;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.gameObject.GetComponent<PickUp>().enabled = false;
    this.StartCoroutine((IEnumerator) this.PlayerPickUpBook());
  }

  public IEnumerator PlayerPickUpBook()
  {
    Interaction_KeyPiece interactionKeyPiece = this;
    interactionKeyPiece.Timer = 0.0f;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionKeyPiece.playerFarming.gameObject, 5f);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    PlayerSimpleInventory component = interactionKeyPiece.state.gameObject.GetComponent<PlayerSimpleInventory>();
    interactionKeyPiece.BookTargetPosition = new Vector3(component.ItemImage.transform.position.x, component.ItemImage.transform.position.y, -1f);
    interactionKeyPiece.BookTargetPosition = interactionKeyPiece.state.transform.position + new Vector3(0.0f, 0.2f, -1.2f);
    interactionKeyPiece.Shadow.enabled = false;
    interactionKeyPiece.state.CURRENT_STATE = StateMachine.State.FoundItem;
    interactionKeyPiece.Particles.Play();
    AudioManager.Instance.PlayOneShot("event:/temple_key/fragment_pickup", interactionKeyPiece.gameObject);
    interactionKeyPiece.transform.DOMove(interactionKeyPiece.BookTargetPosition, 0.2f);
    yield return (object) new WaitForSeconds(2.5f);
    ++Inventory.KeyPieces;
    UIKeyScreenOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowKeyScreen();
    overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() =>
    {
      if (DataManager.Instance.HadFirstTempleKey || Inventory.TempleKeys <= 0 || !DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Fleeces))
        return;
      MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Fleeces).OnHidden += (System.Action) (() =>
      {
        ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/UnlockFleece", Objectives.CustomQuestTypes.UnlockFleece));
        DataManager.Instance.HadFirstTempleKey = true;
      });
    });
    interactionKeyPiece.Particles.Stop();
    yield return (object) new WaitForSeconds(0.5f);
    interactionKeyPiece.Image.enabled = false;
    interactionKeyPiece.state.CURRENT_STATE = StateMachine.State.Idle;
    GameManager.GetInstance().OnConversationEnd();
    System.Action callback = interactionKeyPiece.Callback;
    if (callback != null)
      callback();
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionKeyPiece.gameObject);
  }
}
