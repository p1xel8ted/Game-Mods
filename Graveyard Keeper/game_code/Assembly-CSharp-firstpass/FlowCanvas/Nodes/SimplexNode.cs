// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.SimplexNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using ParadoxNotion.Design;
using System;
using System.Reflection;

#nullable disable
namespace FlowCanvas.Nodes;

[SpoofAOT]
public abstract class SimplexNode
{
  [NonSerialized]
  public string _name;
  [NonSerialized]
  public string _description;
  public ParameterInfo[] _parameters;

  public virtual string name
  {
    get
    {
      if (string.IsNullOrEmpty(this._name))
      {
        NameAttribute attribute = ReflectionTools.RTGetAttribute<NameAttribute>(this.GetType(), false);
        this._name = attribute != null ? attribute.name : this.GetType().FriendlyName().SplitCamelCase();
      }
      return this._name;
    }
  }

  public virtual string description
  {
    get
    {
      if (string.IsNullOrEmpty(this._description))
      {
        DescriptionAttribute attribute = ReflectionTools.RTGetAttribute<DescriptionAttribute>(this.GetType(), false);
        this._description = attribute != null ? attribute.description : "No Description";
      }
      return this._description;
    }
  }

  public ParameterInfo[] parameters
  {
    get
    {
      if (this._parameters != null)
        return this._parameters;
      MethodInfo method = this.GetType().GetMethod("Invoke", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
      return this._parameters = MethodInfo.op_Inequality(method, (MethodInfo) null) ? method.GetParameters() : new ParameterInfo[0];
    }
  }

  public void RegisterPorts(FlowNode node)
  {
    this.OnRegisterPorts(node);
    foreach (PropertyInfo property in this.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public))
    {
      if (property.CanRead && !property.GetGetMethod().IsVirtual)
        node.TryAddPropertyValueOutput(property, (object) this);
    }
    this.OnRegisterExtraPorts(node);
  }

  public void SetDefaultParameters(FlowNode node)
  {
    if (this.parameters == null)
      return;
    for (int index = 0; index < this.parameters.Length; ++index)
    {
      ParameterInfo parameter = this.parameters[index];
      if (parameter.IsOptional && parameter.DefaultValue != null && node.GetInputPort(parameter.Name) is ValueInput inputPort)
        inputPort.serializedValue = parameter.DefaultValue;
    }
  }

  public abstract void OnRegisterPorts(FlowNode node);

  public virtual void OnRegisterExtraPorts(FlowNode node)
  {
  }

  public virtual void OnGraphStarted()
  {
  }

  public virtual void OnGraphPaused()
  {
  }

  public virtual void OnGraphUnpaused()
  {
  }

  public virtual void OnGraphStoped()
  {
  }

  public string GetParameterName(int n)
  {
    return this.parameters[n].Name.Replace("i", "i").Replace("ı", "i");
  }
}
