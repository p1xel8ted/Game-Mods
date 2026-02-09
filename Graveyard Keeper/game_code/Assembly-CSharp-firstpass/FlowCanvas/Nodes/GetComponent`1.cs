// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.GetComponent`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Unity")]
[Description("Get a component attached on an object")]
public class GetComponent<T> : PureFunctionNode<T, GameObject> where T : Component
{
  public T _component;

  public override T Invoke(GameObject gameObject)
  {
    if ((Object) gameObject == (Object) null)
      return default (T);
    if ((Object) this._component == (Object) null || (Object) this._component.gameObject != (Object) gameObject)
      this._component = gameObject.GetComponent<T>();
    return this._component;
  }
}
