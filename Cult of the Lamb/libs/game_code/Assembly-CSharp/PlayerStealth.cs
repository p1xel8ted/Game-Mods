// Decompiled with JetBrains decompiler
// Type: PlayerStealth
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PlayerStealth : BaseMonoBehaviour
{
  public PlayerFarming playerFarming;
  public PlayerController playerController;
  public UnitObject unitObject;
  public StateMachine state;
  public EnemyStealth ClosestStealthCover;
  public Health health;
  public SkeletonAnimation Spine;
  public float CheckDist;
  public float Distance;
  public float StealthSpeed = 3.5f;

  public void Start()
  {
    this.playerFarming = this.GetComponent<PlayerFarming>();
    this.playerController = this.GetComponent<PlayerController>();
    this.unitObject = this.GetComponent<UnitObject>();
    this.state = this.GetComponent<StateMachine>();
    this.health = this.GetComponent<Health>();
  }

  public void Update()
  {
    if ((double) Time.timeScale <= 0.0 || this.playerFarming.GoToAndStopping)
      return;
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
      case StateMachine.State.Moving:
        using (List<EnemyStealth>.Enumerator enumerator = EnemyStealth.EnemyStealths.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            EnemyStealth current = enumerator.Current;
            if ((double) Vector3.Distance(current.transform.position, this.transform.position) < (double) current.EnterRadius && current.health.Unaware)
              this.state.CURRENT_STATE = StateMachine.State.Stealth;
          }
          break;
        }
      case StateMachine.State.Stealth:
        if ((double) Mathf.Abs(this.playerController.xDir) > 0.30000001192092896 || (double) Mathf.Abs(this.playerController.yDir) > 0.30000001192092896)
        {
          if (this.Spine.AnimationName != "run-crouched")
            this.Spine.AnimationState.SetAnimation(0, "run-crouched", true);
          this.playerController.forceDir = Utils.GetAngle(Vector3.zero, new Vector3(this.playerController.xDir, this.playerController.yDir));
          this.state.facingAngle = Utils.GetAngle(this.transform.position, this.transform.position + new Vector3(this.unitObject.vx, this.unitObject.vy));
          this.playerController.speed += (float) (((double) this.StealthSpeed - (double) this.playerController.speed) / 3.0) * GameManager.DeltaTime;
        }
        else
        {
          if (this.Spine.AnimationName != "idle-crouched")
            this.Spine.AnimationState.SetAnimation(0, "idle-crouched", true);
          this.playerController.speed += (float) ((0.0 - (double) this.playerController.speed) / 3.0) * GameManager.DeltaTime;
        }
        this.ClosestStealthCover = (EnemyStealth) null;
        this.Distance = (float) int.MaxValue;
        foreach (EnemyStealth enemyStealth in EnemyStealth.EnemyStealths)
        {
          this.CheckDist = Vector3.Distance(enemyStealth.transform.position, this.transform.position);
          if (!enemyStealth.health.Unaware)
          {
            this.ClosestStealthCover = (EnemyStealth) null;
            break;
          }
          if ((double) this.CheckDist < (double) this.Distance)
          {
            this.ClosestStealthCover = enemyStealth;
            this.Distance = this.CheckDist;
          }
        }
        if (this.health.InStealthCover || !((Object) this.ClosestStealthCover == (Object) null) && (double) this.Distance <= (double) this.ClosestStealthCover.ExitRadius)
          break;
        this.ClosestStealthCover = (EnemyStealth) null;
        this.state.CURRENT_STATE = StateMachine.State.Idle;
        break;
    }
  }
}
