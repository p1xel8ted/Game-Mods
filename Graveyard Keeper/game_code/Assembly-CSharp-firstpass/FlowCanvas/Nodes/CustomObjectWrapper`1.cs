// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.CustomObjectWrapper`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[ParadoxNotion.Design.Icon("", false, "GetRuntimeIconType")]
public abstract class CustomObjectWrapper<T> : CustomObjectWrapper where T : UnityEngine.Object
{
  [SerializeField]
  public T _target;

  public T target
  {
    get => this._target;
    set
    {
      if (!((UnityEngine.Object) this._target != (UnityEngine.Object) value))
        return;
      this._target = value;
      this.GatherPorts();
    }
  }

  public override string name
  {
    get => !((UnityEngine.Object) this.target != (UnityEngine.Object) null) ? base.name : this.target.name;
  }

  public override void SetTarget(UnityEngine.Object target)
  {
    if (!(target is T obj))
      return;
    this.target = obj;
  }

  public System.Type GetRuntimeIconType()
  {
    return !((UnityEngine.Object) this.target != (UnityEngine.Object) null) ? (System.Type) null : this.target.GetType();
  }
}
