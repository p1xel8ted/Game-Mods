// Decompiled with JetBrains decompiler
// Type: Interaction_Teleporter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
public class Interaction_Teleporter : Interaction
{
  public ParticleSystem particleSystem;
  public static Interaction_Teleporter Instance;
  public bool IsHome;
  public string sReturnToBase;

  public void Start()
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

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (this.IsHome || !(this.Label != "") || !collision.gameObject.CompareTag("Player"))
      return;
    this.particleSystem.Play();
  }

  public void OnTriggerExit2D(Collider2D collision)
  {
    if (!(this.Label != "") || !collision.gameObject.CompareTag("Player"))
      return;
    this.particleSystem.Stop();
  }
}
