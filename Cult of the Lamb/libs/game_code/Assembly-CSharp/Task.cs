// Decompiled with JetBrains decompiler
// Type: Task
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class Task
{
  public Task_Type Type;
  public GameObject TargetObject;
  public StateMachine state;
  public TaskDoer t;
  public float Timer;
  public Vector3 TargetV3;
  public bool ClearOnComplete = true;
  public Task CurrentTask;
  public Task ParentTask;
  public string SpineSkin = "normal";
  public string SpineHatSlot = "";

  public virtual void StartTask(TaskDoer t, GameObject TargetObject)
  {
    this.t = t;
    this.state = t.gameObject.GetComponent<StateMachine>();
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.TargetObject = TargetObject;
  }

  public virtual void ClearTask()
  {
    this.TargetObject = (GameObject) null;
    if (!this.ClearOnComplete)
      return;
    this.t.ClearTask();
  }

  public static Task GetTaskByType(Task_Type TaskType)
  {
    switch (TaskType)
    {
      case Task_Type.FOLLOW:
        return (Task) new Task_Follow();
      case Task_Type.CHOP_WOOD:
        return (Task) new Task_ChopWood();
      case Task_Type.COMBAT:
        return (Task) new Task_Combat();
      case Task_Type.ARCHER:
        return (Task) new Task_Archer();
      case Task_Type.SHIELD:
        return (Task) new Task_Combat_Shield();
      case Task_Type.FARMER:
        return (Task) new Task_Farmer();
      case Task_Type.COOK:
        return (Task) new Task_Cook();
      case Task_Type.DANCER:
        return (Task) new Task_Dancer();
      case Task_Type.DEFENCE_TOWER:
        return (Task) new Task_DefenceTower();
      case Task_Type.COLLECT_DUNGEON_RESOURCES:
        return (Task) new Task_CollectDungeonResources();
      case Task_Type.ASTROLOGIST:
        return (Task) new Task_Astrologist();
      case Task_Type.BARRACKS:
        return (Task) new Task_Barracks();
      case Task_Type.IMPRISONED:
        return (Task) new Task_Imprisoned();
      default:
        return (Task) null;
    }
  }

  public virtual void TaskUpdate()
  {
  }

  public void PathToTargetObject(TaskDoer t, float FollowDistance)
  {
    float angle = Utils.GetAngle(t.transform.position, this.TargetObject.transform.position);
    Vector3 position1 = this.TargetObject.transform.position + new Vector3(-FollowDistance * Mathf.Cos(angle * ((float) Math.PI / 180f)), -FollowDistance * Mathf.Sin(angle * ((float) Math.PI / 180f)), 0.0f);
    Vector3 position2 = (Vector3) AstarPath.active.GetNearest(position1, UnitObject.constraint).node.position;
    t.givePath(position2);
  }

  public void PathToPosition(Vector3 Position)
  {
    Position = (Vector3) AstarPath.active.GetNearest(Position, UnitObject.constraint).node.position;
    this.t.givePath(Position);
  }

  public void PathToTargetObjectFormation(TaskDoer t, float FollowDistance)
  {
    float num = (float) (90 * t.Position);
    this.TargetV3 = this.TargetObject.transform.position + new Vector3(1f * Mathf.Cos(num * ((float) Math.PI / 180f)), 1f * Mathf.Sin(num * ((float) Math.PI / 180f)), 0.0f);
    this.TargetV3 = (Vector3) AstarPath.active.GetNearest(this.TargetV3, UnitObject.constraint).node.position;
    t.givePath(this.TargetV3);
  }

  public virtual void DoAttack(float AttackRange, StateMachine.State NextState = StateMachine.State.RecoverFromAttack)
  {
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/humanoid/attack", this.t.transform.position);
    this.Timer = 0.0f;
    this.state.CURRENT_STATE = NextState;
    Addressables_wrapper.InstantiateAsync((object) "Assets/Prefabs/Enemies/Weapons/Swipe.prefab", (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      Swipe component = obj.Result.GetComponent<Swipe>();
      float facingAngle = this.state.facingAngle;
      Vector3 Position = this.t.transform.position + new Vector3(AttackRange * Mathf.Cos(facingAngle * ((float) Math.PI / 180f)), AttackRange * Mathf.Sin(facingAngle * ((float) Math.PI / 180f)), -0.5f);
      double Angle = (double) facingAngle;
      int team = (int) this.t.health.team;
      Health health = this.t.health;
      double Radius = (double) AttackRange;
      component.Init(Position, (float) Angle, (Health.Team) team, health, (Action<Health, Health.AttackTypes, Health.AttackFlags, float>) null, (float) Radius);
    }));
  }

  public void ClearCurrentTask()
  {
    if (this.CurrentTask != null)
      this.CurrentTask.ClearTask();
    this.CurrentTask = (Task) null;
  }
}
