// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.GetGameObjectPosition
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("GameObject")]
[Obsolete("Use Get Property instead")]
public class GetGameObjectPosition : ActionTask<Transform>
{
  [BlackboardOnly]
  public BBParameter<Vector3> saveAs;

  public override string info => $"Get {this.agentInfo} position as {this.saveAs?.ToString()}";

  public override void OnExecute()
  {
    this.saveAs.value = this.agent.position;
    this.EndAction();
  }
}
