// Decompiled with JetBrains decompiler
// Type: Pet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Pet : UnitObject
{
  public GameObject Owner;
  private Vector3 FollowPosition;
  private float Delay = 2f;
  public float FollowDistance = 2f;
  private float LookAround;
  private float FacingAngle;

  private void Start() => this.Delay = 2f + Random.Range(0.0f, 1f);

  public override void Update()
  {
    base.Update();
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
        this.state.facingAngle = this.FacingAngle + 45f * Mathf.Cos(this.LookAround += 0.03f) * GameManager.DeltaTime;
        if (!((Object) this.Owner != (Object) null) || (double) Vector3.Distance(this.Owner.transform.position, this.transform.position) <= 3.0 && (double) (this.Delay -= Time.deltaTime) >= 0.0)
          break;
        this.NewPosition();
        this.givePath(this.FollowPosition);
        break;
      case StateMachine.State.Moving:
        this.Delay = (float) Random.Range(3, 10);
        this.FacingAngle = this.state.facingAngle;
        this.LookAround = 90f;
        break;
    }
  }

  private void NewPosition()
  {
    if ((Object) this.Owner == (Object) null)
      return;
    Vector2 vector2 = Random.insideUnitCircle * 2f;
    this.FollowPosition = this.Owner.transform.position + new Vector3(vector2.x, vector2.y, 0.0f);
    this.FollowPosition = (Vector3) AstarPath.active.GetNearest(this.FollowPosition, UnitObject.constraint).node.position;
  }
}
