// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ReflectedConstructorNodeWrapper
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using ParadoxNotion.Serialization;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

public class ReflectedConstructorNodeWrapper : ReflectedMethodBaseNodeWrapper
{
  [SerializeField]
  public SerializedConstructorInfo _constructor;
  [CompilerGenerated]
  public BaseReflectedConstructorNode \u003CreflectedConstructorNode\u003Ek__BackingField;

  public override SerializedMethodBaseInfo serializedMethodBase
  {
    get => (SerializedMethodBaseInfo) this._constructor;
  }

  public BaseReflectedConstructorNode reflectedConstructorNode
  {
    get => this.\u003CreflectedConstructorNode\u003Ek__BackingField;
    set => this.\u003CreflectedConstructorNode\u003Ek__BackingField = value;
  }

  public ConstructorInfo constructor
  {
    get => this._constructor == null ? (ConstructorInfo) null : this._constructor.Get();
  }

  public override string name
  {
    get
    {
      if (ConstructorInfo.op_Inequality(this.constructor, (ConstructorInfo) null))
        return $"New {this.constructor.DeclaringType.FriendlyName()} ()";
      return this._constructor != null ? $"<color=#ff6457>* Missing Function *\n{this._constructor.GetMethodString()}</color>" : "NOT SET";
    }
  }

  public override void SetMethodBase(MethodBase newMethod, object instance = null)
  {
    if (!(newMethod is ConstructorInfo))
      return;
    this.SetConstructor((ConstructorInfo) newMethod);
  }

  public void SetConstructor(ConstructorInfo newConstructor)
  {
    this._constructor = new SerializedConstructorInfo(newConstructor);
    this.GatherPorts();
    this.SetDefaultParameterValues((MethodBase) newConstructor);
  }

  public override void RegisterPorts()
  {
    if (ConstructorInfo.op_Equality(this.constructor, (ConstructorInfo) null))
      return;
    ReflectedMethodRegistrationOptions options = new ReflectedMethodRegistrationOptions();
    options.callable = this.callable;
    options.exposeParams = this.exposeParams;
    options.exposedParamsCount = this.exposedParamsCount;
    this.reflectedConstructorNode = BaseReflectedConstructorNode.GetConstructorNode(this.constructor, options);
    if (this.reflectedConstructorNode == null)
      return;
    this.reflectedConstructorNode.RegisterPorts((FlowNode) this, options);
  }
}
