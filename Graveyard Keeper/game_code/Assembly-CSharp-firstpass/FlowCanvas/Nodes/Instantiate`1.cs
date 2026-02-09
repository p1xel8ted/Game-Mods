// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Instantiate`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Instantiate an object")]
[ExposeAsDefinition]
[Category("Unity")]
public class Instantiate<T> : CallableFunctionNode<T, T, Vector3, Quaternion, Transform> where T : Object
{
  public override T Invoke(T original, Vector3 position, Quaternion rotation, Transform parent)
  {
    return (Object) original == (Object) null ? default (T) : Object.Instantiate<T>(original, position, rotation, parent);
  }
}
