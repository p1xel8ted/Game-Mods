// Decompiled with JetBrains decompiler
// Type: Interaction_DungeonPortal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
public class Interaction_DungeonPortal : Interaction
{
  private string sReturnToBase;

  private void Start() => this.UpdateLocalisation();

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
