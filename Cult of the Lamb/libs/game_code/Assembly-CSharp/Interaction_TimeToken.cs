// Decompiled with JetBrains decompiler
// Type: Interaction_TimeToken
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Interaction_TimeToken : Interaction
{
  public string LabelName = "Time Token";
  public int timeToAdd = 20;

  public void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation() => base.UpdateLocalisation();

  public override void GetLabel() => this.Label = this.LabelName;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.PickUp();
  }

  public void PickUp()
  {
    CameraManager.shakeCamera(0.3f, (float) Random.Range(0, 360));
    HUD_Timer.Timer += (float) this.timeToAdd;
    Object.Destroy((Object) this.gameObject);
  }
}
