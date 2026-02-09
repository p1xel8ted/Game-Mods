// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ReflectedMethodNodeWrapper
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using ParadoxNotion;
using ParadoxNotion.Serialization;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

public class ReflectedMethodNodeWrapper : ReflectedMethodBaseNodeWrapper
{
  [SerializeField]
  public SerializedMethodInfo _method;
  [CompilerGenerated]
  public BaseReflectedMethodNode \u003CreflectedMethodNode\u003Ek__BackingField;

  public override SerializedMethodBaseInfo serializedMethodBase
  {
    get => (SerializedMethodBaseInfo) this._method;
  }

  public BaseReflectedMethodNode reflectedMethodNode
  {
    get => this.\u003CreflectedMethodNode\u003Ek__BackingField;
    set => this.\u003CreflectedMethodNode\u003Ek__BackingField = value;
  }

  public MethodInfo method => this._method == null ? (MethodInfo) null : this._method.Get();

  public override string name
  {
    get
    {
      if (MethodInfo.op_Inequality(this.method, (MethodInfo) null))
      {
        ReflectionTools.MethodType specialNameType = ReflectionTools.MethodType.Normal;
        string s = this.method.FriendlyName(out specialNameType);
        if (specialNameType == ReflectionTools.MethodType.Operator)
        {
          ReflectionTools.op_FriendlyNamesShort.TryGetValue(this.method.Name, out s);
          return s;
        }
        string str = s.SplitCamelCase();
        if (this.method.IsGenericMethod)
          str += $" ({((IEnumerable<System.Type>) this.method.GetGenericArguments()).First<System.Type>().FriendlyName()})";
        return !this.method.IsStatic || this.method.IsExtensionMethod() ? str : $"{this.method.DeclaringType.FriendlyName()}.{str}";
      }
      return this._method != null ? $"<color=#ff6457>* Missing Function *\n{this._method.GetMethodString()}</color>" : "NOT SET";
    }
  }

  public override void SetMethodBase(MethodBase newMethod, object instance = null)
  {
    if (!(newMethod is MethodInfo))
      return;
    this.SetMethod((MethodInfo) newMethod, instance);
  }

  public void SetMethod(MethodInfo newMethod, object instance = null)
  {
    if (newMethod.IsGenericMethodDefinition)
    {
      System.Type parameterConstraintType = newMethod.GetFirstGenericParameterConstraintType();
      newMethod = newMethod.MakeGenericMethod(parameterConstraintType);
    }
    newMethod = newMethod.GetBaseDefinition();
    this._method = new SerializedMethodInfo(newMethod);
    this._callable = System.Type.op_Equality(newMethod.ReturnType, typeof (void));
    this.GatherPorts();
    this.SetDropInstanceReference((MethodBase) newMethod, instance);
    this.SetDefaultParameterValues((MethodBase) newMethod);
  }

  public override void OnPortConnected(Port port, Port otherPort)
  {
    if (!this.method.IsGenericMethod)
      return;
    MethodInfo genericMethodForWild = FlowNode.TryGetNewGenericMethodForWild(this.method.GetFirstGenericParameterConstraintType(), port, otherPort, this.method);
    if (!MethodInfo.op_Inequality(genericMethodForWild, (MethodInfo) null))
      return;
    this._method = new SerializedMethodInfo(genericMethodForWild);
    this.GatherPorts();
  }

  public override System.Type GetNodeWildDefinitionType()
  {
    return this.method.GetFirstGenericParameterConstraintType();
  }

  public override void RegisterPorts()
  {
    if (MethodInfo.op_Equality(this.method, (MethodInfo) null))
      return;
    ReflectedMethodRegistrationOptions options = new ReflectedMethodRegistrationOptions();
    options.callable = this.callable;
    options.exposeParams = this.exposeParams;
    options.exposedParamsCount = this.exposedParamsCount;
    this.reflectedMethodNode = BaseReflectedMethodNode.GetMethodNode(this.method, options);
    if (this.reflectedMethodNode == null)
      return;
    this.reflectedMethodNode.RegisterPorts((FlowNode) this, options);
  }
}
