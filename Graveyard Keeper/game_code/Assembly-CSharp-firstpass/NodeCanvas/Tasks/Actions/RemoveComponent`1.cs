// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.RemoveComponent`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("GameObject")]
public class RemoveComponent<T> : ActionTask<Transform> where T : Component
{
  public override string info => $"Remove '{typeof (T).Name}'";

  public override void OnExecute()
  {
    T component = this.agent.GetComponent<T>();
    if ((Object) component != (Object) null)
    {
      Object.Destroy((Object) component);
      this.EndAction(true);
    }
    else
      this.EndAction(false);
  }
}
