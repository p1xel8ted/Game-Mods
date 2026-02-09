// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.GraphOwnerControl
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System;
using System.Collections;
using System.Runtime.CompilerServices;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Utility")]
[Name("Control Graph Owner", 0)]
[Description("Start, Resume, Pause, Stop a GraphOwner's behaviour")]
public class GraphOwnerControl : ActionTask<GraphOwner>
{
  public GraphOwnerControl.Control control;
  public bool waitActionFinish = true;

  public override string info => $"{this.agentInfo}.{this.control.ToString()}";

  public override void OnExecute()
  {
    if (this.control == GraphOwnerControl.Control.StartBehaviour)
    {
      if (this.waitActionFinish)
      {
        this.agent.StartBehaviour((Action<bool>) (s => this.EndAction(s)));
      }
      else
      {
        this.agent.StartBehaviour();
        this.EndAction();
      }
    }
    else if ((UnityEngine.Object) this.agent == (UnityEngine.Object) this.ownerAgent)
      this.StartCoroutine(this.YieldDo());
    else
      this.Do();
  }

  public IEnumerator YieldDo()
  {
    yield return (object) null;
    this.Do();
  }

  public void Do()
  {
    if (this.control == GraphOwnerControl.Control.StopBehaviour)
    {
      this.EndAction();
      this.agent.StopBehaviour();
    }
    if (this.control != GraphOwnerControl.Control.PauseBehaviour)
      return;
    this.EndAction();
    this.agent.PauseBehaviour();
  }

  public override void OnStop()
  {
    if (!this.waitActionFinish || this.control != GraphOwnerControl.Control.StartBehaviour)
      return;
    this.agent.StopBehaviour();
  }

  [CompilerGenerated]
  public void \u003COnExecute\u003Eb__5_0(bool s) => this.EndAction(s);

  public enum Control
  {
    StartBehaviour,
    StopBehaviour,
    PauseBehaviour,
  }
}
