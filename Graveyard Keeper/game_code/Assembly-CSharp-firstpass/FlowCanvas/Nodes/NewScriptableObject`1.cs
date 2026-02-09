// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.NewScriptableObject`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Creates a new ScriptableObject instance")]
[Category("Unity")]
public class NewScriptableObject<T> : CallableFunctionNode<T> where T : ScriptableObject
{
  public override T Invoke() => ScriptableObject.CreateInstance<T>();
}
