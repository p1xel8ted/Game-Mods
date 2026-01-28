// Decompiled with JetBrains decompiler
// Type: Interaction_DungeonKey
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MMBiomeGeneration;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_DungeonKey : Interaction
{
  public string sLabelName;
  public bool Activated;
  public SpriteRenderer Shadow;
  public SpriteRenderer Image;
  public ParticleSystem GodRays;
  public GameObject PlayerPosition;
  public Vector3 BookTargetPosition;
  public float Timer;

  public void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sLabelName = ScriptLocalization.Interactions.KeyPiece;
  }

  public override void GetLabel() => this.Label = this.Activated ? "" : this.sLabelName;

  public override void OnInteract(StateMachine state)
  {
    if (this.Activated)
      return;
    this.Activated = true;
    base.OnInteract(state);
    this.StartCoroutine((IEnumerator) this.PlayerPickUpBook());
  }

  public IEnumerator PlayerPickUpBook()
  {
    Interaction_DungeonKey interactionDungeonKey = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionDungeonKey.state.gameObject, 6f);
    if ((Object) BiomeGenerator.Instance != (Object) null)
      BiomeGenerator.Instance.HasKey = true;
    interactionDungeonKey.playerFarming.GoToAndStop(interactionDungeonKey.PlayerPosition, interactionDungeonKey.gameObject);
    while (interactionDungeonKey.playerFarming.GoToAndStopping)
      yield return (object) null;
    yield return (object) new WaitForSeconds(0.2f);
    PlayerSimpleInventory component = interactionDungeonKey.state.gameObject.GetComponent<PlayerSimpleInventory>();
    interactionDungeonKey.BookTargetPosition = component.ItemImage.transform.position;
    interactionDungeonKey.Shadow.enabled = false;
    interactionDungeonKey.state.CURRENT_STATE = StateMachine.State.FoundItem;
    while ((double) (interactionDungeonKey.Timer += Time.deltaTime) < 1.0)
    {
      interactionDungeonKey.transform.position = Vector3.Lerp(interactionDungeonKey.transform.position, interactionDungeonKey.BookTargetPosition, 5f * Time.deltaTime);
      yield return (object) null;
    }
    interactionDungeonKey.Image.enabled = false;
    interactionDungeonKey.state.CURRENT_STATE = StateMachine.State.Idle;
    interactionDungeonKey.GodRays.Stop();
    GameManager.GetInstance().OnConversationEnd();
    Object.Destroy((Object) interactionDungeonKey.gameObject);
  }
}
