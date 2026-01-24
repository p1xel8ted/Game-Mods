// Decompiled with JetBrains decompiler
// Type: Task_ChopWood
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Task_ChopWood : Task
{
  public Tree TargetTree;

  public Task_ChopWood() => this.Type = Task_Type.CHOP_WOOD;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.FindClosestTree();
  }

  public override void ClearTask()
  {
    base.ClearTask();
    if ((Object) this.TargetTree != (Object) null)
      this.TargetTree.TaskDoer = (GameObject) null;
    this.TargetTree = (Tree) null;
  }

  public override void TaskUpdate()
  {
    if ((Object) this.TargetObject != (Object) null)
    {
      switch (this.state.CURRENT_STATE)
      {
        case StateMachine.State.Idle:
          if (this.TargetTree.Dead)
          {
            this.ClearTarget();
            break;
          }
          if ((double) Vector3.Distance(this.t.transform.position, this.TargetObject.transform.position) > 1.0)
          {
            if (this.state.CURRENT_STATE == StateMachine.State.Moving)
              break;
            this.PathToTargetObject(this.t, 0.0f);
            break;
          }
          this.state.facingAngle = Utils.GetAngle(this.t.transform.position, this.TargetObject.transform.position);
          this.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
          this.Timer = 0.0f;
          break;
        case StateMachine.State.Moving:
          if ((Object) this.TargetObject == (Object) null)
          {
            this.ClearTarget();
            break;
          }
          if (this.TargetTree.Dead)
          {
            this.ClearTarget();
            break;
          }
          if ((double) (this.Timer += Time.deltaTime) <= 1.0)
            break;
          this.Timer = 0.0f;
          this.PathToTargetObject(this.t, 0.0f);
          break;
        case StateMachine.State.SignPostAttack:
          if ((double) (this.Timer += Time.deltaTime) <= 0.5)
            break;
          this.Timer = 0.0f;
          this.DoAttack(1f);
          break;
        case StateMachine.State.RecoverFromAttack:
          if ((double) (this.Timer += Time.deltaTime) <= 0.5)
            break;
          this.Timer = 0.0f;
          if (this.TargetTree.Dead)
            this.ClearTarget();
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          break;
      }
    }
    else
    {
      if (!((Object) this.TargetObject == (Object) null))
        return;
      this.ClearTask();
    }
  }

  public void FindClosestTree()
  {
    GameObject gameObject = (GameObject) null;
    Tree tree1 = (Tree) null;
    float num1 = float.MaxValue;
    foreach (Tree tree2 in Tree.Trees)
    {
      float num2 = Vector3.Distance(this.t.transform.position, tree2.transform.position);
      if ((double) num2 < (double) num1 && !tree2.Dead && (Object) tree2.TaskDoer == (Object) null)
      {
        gameObject = tree2.gameObject;
        tree1 = tree2;
        num1 = num2;
      }
    }
    this.TargetObject = gameObject;
    this.TargetTree = tree1;
    if (!((Object) this.TargetTree != (Object) null))
      return;
    this.TargetTree.TaskDoer = this.t.gameObject;
  }

  public void ClearTarget()
  {
    this.Timer = 0.0f;
    this.TargetObject = (GameObject) null;
    if ((Object) this.TargetTree != (Object) null)
      this.TargetTree.TaskDoer = (GameObject) null;
    this.TargetTree = (Tree) null;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
  }
}
