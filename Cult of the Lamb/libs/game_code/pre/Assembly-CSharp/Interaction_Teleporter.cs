// Decompiled with JetBrains decompiler
// Type: Interaction_Teleporter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
public class Interaction_Teleporter : Interaction
{
  public ParticleSystem particleSystem;
  public static Interaction_Teleporter Instance;
  public bool IsHome;
  private string sReturnToBase;

  private void Start()
  {
    this.ActivateDistance = 1f;
    this.particleSystem.Stop();
    this.HoldToInteract = true;
    this.UpdateLocalisation();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sReturnToBase = ScriptLocalization.Interactions.ReturnToBase;
  }

  public override void GetLabel() => this.Label = this.IsHome ? "" : this.sReturnToBase;

  public override void OnEnableInteraction() => Interaction_Teleporter.Instance = this;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    GameManager.ToShip();
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (this.IsHome || !(this.Label != "") || !(collision.gameObject.tag == "Player"))
      return;
    this.particleSystem.Play();
  }

  private void OnTriggerExit2D(Collider2D collision)
  {
    if (!(this.Label != "") || !(collision.gameObject.tag == "Player"))
      return;
    this.particleSystem.Stop();
  }
}
