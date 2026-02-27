// Decompiled with JetBrains decompiler
// Type: BellTower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class BellTower : Interaction
{
  public UnityEvent OnHitCallback;
  private Health health;
  public GameObject Bell;
  private float BellSpeed;
  private float zRotation;
  [Range(0.1f, 0.9f)]
  public float Elastic = 0.1f;
  [Range(0.1f, 0.9f)]
  public float Friction = 0.9f;
  public float ImpactForce = 20f;

  public override void GetLabel() => this.Label = ScriptLocalization.Interactions.DinnerBell;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    Vector3 position1 = this.transform.position;
    AudioManager.Instance.PlayOneShot("event:/building/building_bell_ring", position1);
    Vector3 position2 = PlayerFarming.Instance.gameObject.transform.position;
    CameraManager.shakeCamera(0.3f, Utils.GetAngle(position1, position2));
    this.BellSpeed = this.ImpactForce * ((double) position2.x < (double) position1.x ? 1f : -1f);
    if (this.OnHitCallback != null)
      this.OnHitCallback.Invoke();
    foreach (Follower follower in FollowerManager.FollowersAtLocation(FollowerLocation.Base))
      follower.Brain.SetPersonalOverrideTask(FollowerTaskType.EatMeal);
  }

  private new void Update()
  {
    this.BellSpeed += (0.0f - this.zRotation) * this.Elastic;
    this.zRotation += (this.BellSpeed *= this.Friction);
    this.Bell.transform.eulerAngles = new Vector3(-60f, 0.0f, this.zRotation);
  }
}
