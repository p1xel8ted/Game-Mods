// Decompiled with JetBrains decompiler
// Type: Interaction_Elevator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Interaction_Elevator : Interaction
{
  public Interaction_Elevator.Floor MyFloor;
  public Interaction_Elevator TargetElevator;
  public LineRenderer lineRenderer;
  private string sGrapple;

  public override void GetLabel() => this.Label = this.sGrapple;

  private void Start()
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

  private new void OnDrawGizmos()
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
