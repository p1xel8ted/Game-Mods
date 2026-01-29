// Decompiled with JetBrains decompiler
// Type: GoToBarricadeWhenIdle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (Health))]
public class GoToBarricadeWhenIdle : BaseMonoBehaviour
{
  public StateMachine state;
  public FollowPath followPath;
  public Barricade myBarricade;
  public Health health;

  public void Start()
  {
    this.state = this.GetComponent<StateMachine>();
    this.followPath = this.GetComponent<FollowPath>();
    this.health = this.GetComponent<Health>();
  }

  public void Update()
  {
    if (this.state.CURRENT_STATE != StateMachine.State.Idle)
      return;
    if ((Object) this.myBarricade == (Object) null)
    {
      foreach (Barricade barricade in Barricade.barricades)
      {
        if (!barricade.occupied && (double) Vector2.Distance((Vector2) barricade.transform.position, (Vector2) this.transform.position) < 0.699999988079071)
        {
          this.myBarricade = barricade;
          this.followPath.givePath(barricade.transform.position);
          this.followPath.NewPath += new FollowPath.Action(this.leaveBarricade);
          barricade.occupied = true;
          break;
        }
      }
    }
    else
    {
      if (this.state.CURRENT_STATE == StateMachine.State.Moving || this.state.isDefending)
        return;
      this.state.isDefending = true;
    }
  }

  public void leaveBarricade()
  {
    if (!((Object) this.myBarricade != (Object) null))
      return;
    this.state.isDefending = false;
    this.myBarricade.occupied = false;
    this.myBarricade = (Barricade) null;
    this.followPath.NewPath -= new FollowPath.Action(this.leaveBarricade);
  }

  public void OnDestroy() => this.followPath.NewPath -= new FollowPath.Action(this.leaveBarricade);
}
