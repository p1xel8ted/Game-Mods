// Decompiled with JetBrains decompiler
// Type: Interaction_DungeonKey
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using MMBiomeGeneration;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_DungeonKey : Interaction
{
  private string sLabelName;
  private bool Activated;
  public SpriteRenderer Shadow;
  public SpriteRenderer Image;
  public ParticleSystem GodRays;
  public GameObject PlayerPosition;
  private Vector3 BookTargetPosition;
  private float Timer;

  private void Start() => this.UpdateLocalisation();

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

  private IEnumerator PlayerPickUpBook()
  {
    Interaction_DungeonKey interactionDungeonKey = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionDungeonKey.state.gameObject, 6f);
    if ((Object) BiomeGenerator.Instance != (Object) null)
      BiomeGenerator.Instance.HasKey = true;
    PlayerFarming.Instance.GoToAndStop(interactionDungeonKey.PlayerPosition, interactionDungeonKey.gameObject);
    while (PlayerFarming.Instance.GoToAndStopping)
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
