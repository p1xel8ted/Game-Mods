// Decompiled with JetBrains decompiler
// Type: Interaction_KeyPieceDungeon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using MMBiomeGeneration;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_KeyPieceDungeon : Interaction
{
  public GameObject PlayerPosition;
  public ParticleSystem Particles;
  public SpriteRenderer Image;
  public SpriteRenderer Shadow;
  public List<Sprite> KeyImages = new List<Sprite>();
  public ParticleSystem GodRays;
  public bool Activated;
  public string sLabelName;

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.ActivateDistance = 2f;
    this.Image.sprite = this.KeyImages[DataManager.Instance.CurrentKeyPieces % this.KeyImages.Count];
    this.Particles.Stop();
    this.UpdateLocalisation();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sLabelName = ScriptLocalization.Interactions.KeyPiece;
  }

  public override void GetLabel() => this.Label = this.Activated ? "" : this.sLabelName;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.Activated = true;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(state.gameObject, 5f);
    this.playerFarming.GoToAndStop(this.PlayerPosition, this.gameObject, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.PlayerPickUpBook())));
  }

  public IEnumerator PlayerPickUpBook()
  {
    Interaction_KeyPieceDungeon interactionKeyPieceDungeon = this;
    yield return (object) new WaitForSeconds(0.2f);
    PlayerSimpleInventory component = interactionKeyPieceDungeon.state.gameObject.GetComponent<PlayerSimpleInventory>();
    Vector3 BookTargetPosition = new Vector3(component.ItemImage.transform.position.x, component.ItemImage.transform.position.y, -1f);
    interactionKeyPieceDungeon.Shadow.enabled = false;
    interactionKeyPieceDungeon.state.CURRENT_STATE = StateMachine.State.FoundItem;
    interactionKeyPieceDungeon.Particles.Play();
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir", interactionKeyPieceDungeon.gameObject);
    float Timer = 0.0f;
    while ((double) (Timer += Time.deltaTime) < 1.5)
    {
      interactionKeyPieceDungeon.transform.position = Vector3.Lerp(interactionKeyPieceDungeon.transform.position, BookTargetPosition, 5f * Time.deltaTime);
      yield return (object) null;
    }
    interactionKeyPieceDungeon.transform.position = BookTargetPosition;
    ++Inventory.KeyPieces;
    UIKeyScreenOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowKeyScreen();
    overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() =>
    {
      if (DataManager.Instance.HadFirstTempleKey || Inventory.TempleKeys <= 0 || !DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Fleeces))
        return;
      MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Fleeces).OnHidden += (System.Action) (() => DataManager.Instance.HadFirstTempleKey = true);
    });
    DataManager.SaveKeyPieceFromLocation(BiomeGenerator.Instance.DungeonLocation, GameManager.CurrentDungeonLayer);
    interactionKeyPieceDungeon.Particles.Stop();
    yield return (object) new WaitForSeconds(0.5f);
    interactionKeyPieceDungeon.Image.enabled = false;
    interactionKeyPieceDungeon.state.CURRENT_STATE = StateMachine.State.Idle;
    GameManager.GetInstance().OnConversationEnd();
    interactionKeyPieceDungeon.GodRays.Stop();
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionKeyPieceDungeon.gameObject);
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__11_0()
  {
    this.StartCoroutine((IEnumerator) this.PlayerPickUpBook());
  }
}
