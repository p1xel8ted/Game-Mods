// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ReflectedMethodBaseNodeWrapper
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using System.Reflection;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[DoNotList]
[ParadoxNotion.Design.Icon("", false, "GetRuntimeIconType")]
public abstract class ReflectedMethodBaseNodeWrapper : FlowNode
{
  [SerializeField]
  public bool _callable;
  [SerializeField]
  public bool _exposeParams;
  [SerializeField]
  public int _exposedParamsCount;

  public System.Type GetRuntimeIconType()
  {
    return !MethodBase.op_Inequality(this.method, (MethodBase) null) ? (System.Type) null : this.method.DeclaringType;
  }

  public abstract SerializedMethodBaseInfo serializedMethodBase { get; }

  public MethodBase method
  {
    get
    {
      return this.serializedMethodBase == null ? (MethodBase) null : this.serializedMethodBase.GetBase();
    }
  }

  public bool callable
  {
    get => this._callable;
    set
    {
      if (this._callable == value)
        return;
      this._callable = value;
      this.GatherPorts();
    }
  }

  public bool exposeParams
  {
    get => this._exposeParams;
    set
    {
      if (this._exposeParams == value)
        return;
      this._exposeParams = value;
      this._exposedParamsCount = Mathf.Max(this._exposedParamsCount, 1);
      this.GatherPorts();
    }
  }

  public int exposedParamsCount
  {
    get => this._exposedParamsCount;
    set
    {
      if (this._exposedParamsCount == value)
        return;
      this._exposedParamsCount = value;
      if (this._exposedParamsCount <= 0)
        this._exposeParams = false;
      this.GatherPorts();
    }
  }

  public abstract void SetMethodBase(MethodBase newMethod, object instance = null);

  public void SetDefaultParameterValues(MethodBase newMethod)
  {
    ParameterInfo[] parameters = newMethod.GetParameters();
    for (int index = 0; index < parameters.Length; ++index)
    {
      ParameterInfo parameterInfo = parameters[index];
      if (parameterInfo.IsOptional && parameterInfo.DefaultValue != null && this.GetInputPort(parameters[index].Name) is ValueInput inputPort)
        inputPort.serializedValue = parameterInfo.DefaultValue;
    }
  }

  public void SetDropInstanceReference(MethodBase newMethod, object instance = null)
  {
    if (instance == null || newMethod.IsStatic)
      return;
    ValueInput firstInputOfType = (ValueInput) this.GetFirstInputOfType(instance.GetType());
    if (firstInputOfType == null)
      return;
    firstInputOfType.serializedValue = instance;
  }
}
