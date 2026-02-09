// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.BBParameter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using ParadoxNotion.Design;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Framework;

[SpoofAOT]
[Serializable]
public abstract class BBParameter
{
  [SerializeField]
  public string _name;
  [SerializeField]
  public string _targetVariableID;
  [NonSerialized]
  public IBlackboard _bb;
  [NonSerialized]
  public Variable _varRef;

  public static BBParameter CreateInstance(System.Type t, IBlackboard bb)
  {
    if (System.Type.op_Equality(t, (System.Type) null))
      return (BBParameter) null;
    BBParameter instance = (BBParameter) Activator.CreateInstance(typeof (BBParameter<>).RTMakeGenericType(t));
    instance.bb = bb;
    return instance;
  }

  public static void SetBBFields(object o, IBlackboard bb)
  {
    List<BBParameter> objectBbParameters = BBParameter.GetObjectBBParameters(o);
    for (int index = 0; index < objectBbParameters.Count; ++index)
      objectBbParameters[index].bb = bb;
  }

  public static List<BBParameter> GetObjectBBParameters(object o)
  {
    List<BBParameter> objectBbParameters = new List<BBParameter>();
    if (o == null)
      return objectBbParameters;
    foreach (FieldInfo field in o.GetType().RTGetFields())
    {
      if (typeof (BBParameter).RTIsAssignableFrom(field.FieldType))
      {
        object instance = field.GetValue(o);
        if (instance == null && !field.FieldType.RTIsAbstract())
        {
          instance = Activator.CreateInstance(field.FieldType);
          field.SetValue(o, instance);
        }
        if (instance != null)
          objectBbParameters.Add((BBParameter) instance);
      }
      else if (typeof (IList).RTIsAssignableFrom(field.FieldType) && !field.FieldType.IsArray)
      {
        System.Type[] genericArguments = field.FieldType.RTGetGenericArguments();
        if (genericArguments.Length != 0)
        {
          System.Type type = genericArguments[0];
          if (!System.Type.op_Equality(type, (System.Type) null) && typeof (BBParameter).RTIsAssignableFrom(type) && field.GetValue(o) is IList list)
          {
            for (int index = 0; index < list.Count; ++index)
            {
              object instance = list[index];
              if (instance == null && !field.FieldType.RTIsAbstract())
              {
                instance = Activator.CreateInstance(type);
                list[index] = instance;
              }
              if (instance != null)
                objectBbParameters.Add((BBParameter) instance);
            }
          }
        }
      }
    }
    if (o is ISubParametersContainer)
    {
      BBParameter[] subParameters = (o as ISubParametersContainer).GetSubParameters();
      if (subParameters != null && subParameters.Length != 0)
        objectBbParameters.AddRange((IEnumerable<BBParameter>) subParameters);
    }
    return objectBbParameters;
  }

  public string targetVariableID
  {
    get => this._targetVariableID;
    set => this._targetVariableID = value;
  }

  public Variable varRef
  {
    get => this._varRef;
    set
    {
      if (this._varRef == value)
        return;
      this._varRef = value;
      this.Bind(value);
    }
  }

  public IBlackboard bb
  {
    get => this._bb;
    set
    {
      if (this._bb == value)
        return;
      this._bb = value;
      this.varRef = value != null ? this.ResolveReference(this._bb, true) : (Variable) null;
    }
  }

  public string name
  {
    get => this._name;
    set
    {
      if (!(this._name != value))
        return;
      this._name = value;
      this.varRef = value != null ? this.ResolveReference(this.bb, false) : (Variable) null;
    }
  }

  public bool useBlackboard
  {
    get => this.name != null;
    set
    {
      if (!value)
        this.name = (string) null;
      if (!value || this.name != null)
        return;
      this.name = string.Empty;
    }
  }

  public bool isNone => this.name == string.Empty;

  public bool isNull => object.Equals(this.objectValue, (object) null);

  public System.Type refType => this.varRef == null ? (System.Type) null : this.varRef.varType;

  public object value
  {
    get => this.objectValue;
    set => this.objectValue = value;
  }

  public abstract object objectValue { get; set; }

  public abstract System.Type varType { get; }

  public abstract void Bind(Variable data);

  public Variable ResolveReference(IBlackboard targetBlackboard, bool useID)
  {
    string name = this.name;
    if (name != null && name.Contains("/"))
    {
      string[] strArray = name.Split('/');
      targetBlackboard = (IBlackboard) GlobalBlackboard.Find(strArray[0]);
      name = strArray[1];
    }
    Variable variable = (Variable) null;
    if (targetBlackboard == null)
      return (Variable) null;
    if (useID && this.targetVariableID != null)
      variable = targetBlackboard.GetVariableByID(this.targetVariableID);
    if (variable == null && !string.IsNullOrEmpty(name))
      variable = targetBlackboard.GetVariable(name, this.varType);
    return variable;
  }

  public Variable PromoteToVariable(IBlackboard targetBB)
  {
    if (string.IsNullOrEmpty(this.name))
    {
      this.varRef = (Variable) null;
      return (Variable) null;
    }
    string name1 = this.name;
    string name2 = targetBB != null ? targetBB.name : string.Empty;
    if (this.name.Contains("/"))
    {
      string[] strArray = this.name.Split('/');
      name2 = strArray[0];
      name1 = strArray[1];
      targetBB = (IBlackboard) GlobalBlackboard.Find(name2);
    }
    if (targetBB == null)
    {
      this.varRef = (Variable) null;
      ParadoxNotion.Services.Logger.LogError((object) $"Parameter '{name1}' failed to promote to a variable, because Blackboard named '{name2}' could not be found.", "Variable", (object) this);
      return (Variable) null;
    }
    this.varRef = targetBB.AddVariable(name1, this.varType);
    if (this.varRef == null)
      ParadoxNotion.Services.Logger.LogError((object) $"Parameter {name1} (of type '{this.varType.FriendlyName()}') failed to promote to a Variable in Blackboard '{name2}'.", "Variable", (object) this);
    return this.varRef;
  }

  public override string ToString()
  {
    if (this.isNone)
      return "<b>NONE</b>";
    if (this.useBlackboard)
      return $"<b>${this.name}</b>";
    if (this.isNull)
      return "<b>NULL</b>";
    if (this.objectValue is IList)
      return $"<b>{this.varType.FriendlyName()}</b>";
    return this.objectValue is IDictionary ? $"<b>{this.varType.FriendlyName()}</b>" : $"<b>{this.objectValue.ToStringAdvanced()}</b>";
  }
}
