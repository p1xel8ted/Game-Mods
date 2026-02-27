// Decompiled with JetBrains decompiler
// Type: Interaction_TimeToken
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Interaction_TimeToken : Interaction
{
  public string LabelName = "Time Token";
  public int timeToAdd = 20;

  private void Start() => this.UpdateLocalisation();

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
