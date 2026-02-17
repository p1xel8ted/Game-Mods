// Decompiled with JetBrains decompiler
// Type: Interaction_DungeonPortal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
public class Interaction_DungeonPortal : Interaction
{
  public string sReturnToBase;

  public void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sReturnToBase = ScriptLocalization.Interactions.ReturnToBase;
  }

  public override void GetLabel()
  {
    this.Interactable = true;
    this.Label = this.sReturnToBase;
  }

  public override void OnInteract(StateMachine state)
  {
    foreach (AudioSource audioSource in Object.FindObjectsOfType<AudioSource>())
      audioSource.Stop();
    base.OnInteract(state);
    GameManager.ToShip();
  }
}
