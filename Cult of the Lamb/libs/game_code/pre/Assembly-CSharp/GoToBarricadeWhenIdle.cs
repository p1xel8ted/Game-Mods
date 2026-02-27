// Decompiled with JetBrains decompiler
// Type: GoToBarricadeWhenIdle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (Health))]
public class GoToBarricadeWhenIdle : BaseMonoBehaviour
{
  private StateMachine state;
  private FollowPath followPath;
  private Barricade myBarricade;
  private Health health;

  private void Start()
  {
    this.state = this.GetComponent<StateMachine>();
    this.followPath = this.GetComponent<FollowPath>();
    this.health = this.GetComponent<Health>();
  }

  private void Update()
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

  private void leaveBarricade()
  {
    if (!((Object) this.myBarricade != (Object) null))
      return;
    this.state.isDefending = false;
    this.myBarricade.occupied = false;
    this.myBarricade = (Barricade) null;
    this.followPath.NewPath -= new FollowPath.Action(this.leaveBarricade);
  }

  private void OnDestroy() => this.followPath.NewPath -= new FollowPath.Action(this.leaveBarricade);
}
