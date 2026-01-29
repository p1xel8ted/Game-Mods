// Decompiled with JetBrains decompiler
// Type: Pet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Pet : UnitObject
{
  public GameObject Owner;
  public Vector3 FollowPosition;
  public float Delay = 2f;
  public float FollowDistance = 2f;
  public float LookAround;
  public float FacingAngle;

  public void Start() => this.Delay = 2f + Random.Range(0.0f, 1f);

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

  public void NewPosition()
  {
    if ((Object) this.Owner == (Object) null)
      return;
    Vector2 vector2 = Random.insideUnitCircle * 2f;
    this.FollowPosition = this.Owner.transform.position + new Vector3(vector2.x, vector2.y, 0.0f);
    this.FollowPosition = (Vector3) AstarPath.active.GetNearest(this.FollowPosition, UnitObject.constraint).node.position;
  }
}
