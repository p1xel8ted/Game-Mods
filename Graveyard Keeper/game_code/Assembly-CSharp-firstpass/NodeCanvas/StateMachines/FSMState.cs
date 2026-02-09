// Decompiled with JetBrains decompiler
// Type: NodeCanvas.StateMachines.FSMState
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using NodeCanvas.Framework;
using ParadoxNotion;
using UnityEngine;

#nullable disable
namespace NodeCanvas.StateMachines;

public abstract class FSMState : Node, IState
{
  [SerializeField]
  public FSMState.TransitionEvaluationMode _transitionEvaluation;
  public float _elapsedTime;
  public bool hasInit;

  public override int maxInConnections => -1;

  public override int maxOutConnections => -1;

  public sealed override System.Type outConnectionType => typeof (FSMConnection);

  public override bool allowAsPrime => true;

  public sealed override Alignment2x2 commentsAlignment => Alignment2x2.Bottom;

  public sealed override Alignment2x2 iconAlignment => Alignment2x2.Default;

  public FSMState.TransitionEvaluationMode transitionEvaluation
  {
    get => this._transitionEvaluation;
    set => this._transitionEvaluation = value;
  }

  public float elapsedTime
  {
    get => this._elapsedTime;
    set => this._elapsedTime = value;
  }

  public FSM FSM => (FSM) this.graph;

  public FSMConnection[] GetTransitions()
  {
    return this.outConnections.Cast<FSMConnection>().ToArray<FSMConnection>();
  }

  public void Finish() => this.Finish(true);

  public void Finish(bool inSuccess) => this.status = inSuccess ? NodeCanvas.Status.Success : NodeCanvas.Status.Failure;

  public sealed override void OnGraphStarted()
  {
  }

  public sealed override void OnGraphStoped() => this.status = NodeCanvas.Status.Resting;

  public sealed override void OnGraphPaused()
  {
    if (this.status != NodeCanvas.Status.Running)
      return;
    this.OnPause();
  }

  public sealed override NodeCanvas.Status OnExecute(Component agent, IBlackboard bb)
  {
    if (!this.hasInit)
    {
      this.hasInit = true;
      this.OnInit();
    }
    if (this.status == NodeCanvas.Status.Resting || this.status == NodeCanvas.Status.Running)
    {
      this.status = NodeCanvas.Status.Running;
      for (int index = 0; index < this.outConnections.Count; ++index)
      {
        if (((FSMConnection) this.outConnections[index]).condition != null)
          ((FSMConnection) this.outConnections[index]).condition.Enable(agent, bb);
      }
      this.OnEnter();
    }
    return this.status;
  }

  public void Update()
  {
    this.elapsedTime += Time.deltaTime;
    if (this.transitionEvaluation == FSMState.TransitionEvaluationMode.CheckContinuously)
      this.CheckTransitions();
    else if (this.transitionEvaluation == FSMState.TransitionEvaluationMode.CheckAfterStateFinished && this.status != NodeCanvas.Status.Running)
      this.CheckTransitions();
    if (this.status != NodeCanvas.Status.Running)
      return;
    this.OnUpdate();
  }

  public bool CheckTransitions()
  {
    for (int index = 0; index < this.outConnections.Count; ++index)
    {
      FSMConnection outConnection = (FSMConnection) this.outConnections[index];
      ConditionTask condition = outConnection.condition;
      if (outConnection.isActive)
      {
        if (condition != null && condition.CheckCondition(this.graphAgent, this.graphBlackboard) || condition == null && this.status != NodeCanvas.Status.Running)
        {
          this.FSM.EnterState((FSMState) outConnection.targetNode);
          outConnection.status = NodeCanvas.Status.Success;
          return true;
        }
        outConnection.status = NodeCanvas.Status.Failure;
      }
    }
    return false;
  }

  public sealed override void OnReset()
  {
    this.status = NodeCanvas.Status.Resting;
    this.elapsedTime = 0.0f;
    for (int index = 0; index < this.outConnections.Count; ++index)
    {
      if (((FSMConnection) this.outConnections[index]).condition != null)
        ((FSMConnection) this.outConnections[index]).condition.Disable();
    }
    this.OnExit();
  }

  public virtual void OnInit()
  {
  }

  public virtual void OnEnter()
  {
  }

  public virtual void OnUpdate()
  {
  }

  public virtual void OnExit()
  {
  }

  public virtual void OnPause()
  {
  }

  public enum TransitionEvaluationMode
  {
    CheckContinuously,
    CheckAfterStateFinished,
    CheckManually,
  }
}
