// Decompiled with JetBrains decompiler
// Type: Interaction_Elevator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Interaction_Elevator : Interaction
{
  public Interaction_Elevator.Floor MyFloor;
  public Interaction_Elevator TargetElevator;
  public LineRenderer lineRenderer;
  public string sGrapple;

  public override void GetLabel() => this.Label = this.sGrapple;

  public void Start()
  {
    this.UpdateLocalisation();
    this.lineRenderer.SetPosition(0, this.transform.position);
    this.lineRenderer.SetPosition(1, this.TargetElevator.transform.position);
    if ((double) this.TargetElevator.transform.position.x >= (double) this.transform.position.x)
      return;
    this.lineRenderer.gameObject.SetActive(false);
  }

  public override void UpdateLocalisation() => base.UpdateLocalisation();

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    state.GetComponent<PlayerController>();
  }

  public new void OnDrawGizmos()
  {
    if (!((Object) this.TargetElevator != (Object) null))
      return;
    Utils.DrawLine(this.transform.position, this.TargetElevator.transform.position, Color.yellow);
  }

  public enum Floor
  {
    FirstFloor,
    SecondFloor,
    ThirdFloor,
  }
}
