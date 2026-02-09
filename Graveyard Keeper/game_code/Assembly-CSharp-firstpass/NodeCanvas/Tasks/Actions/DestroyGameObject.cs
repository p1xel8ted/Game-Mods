// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.DestroyGameObject
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("GameObject")]
public class DestroyGameObject : ActionTask<Transform>
{
  public override string info => $"Destroy {this.agentInfo}";

  public override void OnUpdate()
  {
    Object.Destroy((Object) this.agent.gameObject);
    this.EndAction();
  }
}
