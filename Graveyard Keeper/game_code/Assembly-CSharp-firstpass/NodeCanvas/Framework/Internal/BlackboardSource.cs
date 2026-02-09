// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.Internal.BlackboardSource
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using ParadoxNotion;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Framework.Internal;

[Serializable]
public sealed class BlackboardSource : IBlackboard
{
  [SerializeField]
  public string _name;
  [SerializeField]
  public Dictionary<string, Variable> _variables = new Dictionary<string, Variable>((IEqualityComparer<string>) StringComparer.Ordinal);

  public event Action<Variable> onVariableAdded;

  public event Action<Variable> onVariableRemoved;

  public string name
  {
    get => this._name;
    set => this._name = value;
  }

  public Dictionary<string, Variable> variables
  {
    get => this._variables;
    set => this._variables = value;
  }

  public GameObject propertiesBindTarget => (GameObject) null;

  public object this[string varName]
  {
    get
    {
      try
      {
        return this.variables[varName].value;
      }
      catch
      {
        return (object) null;
      }
    }
    set => this.SetValue(varName, value);
  }

  public void InitializePropertiesBinding(GameObject targetGO, bool callSetter)
  {
    foreach (Variable variable in this.variables.Values)
      variable.InitializePropertyBinding(targetGO, callSetter);
  }

  public Variable AddVariable(string varName, object value)
  {
    if (value == null)
    {
      Debug.LogError((object) "<b>Blackboard:</b> You can't use AddVariable with a null value. Use AddVariable(string, Type) to add the new data first");
      return (Variable) null;
    }
    Variable variable = this.AddVariable(varName, value.GetType());
    if (variable != null)
      variable.value = value;
    return variable;
  }

  public Variable AddVariable(string varName, System.Type type)
  {
    if (this.variables.ContainsKey(varName))
    {
      Variable variable = this.GetVariable(varName, type);
      if (variable == null)
      {
        Debug.LogError((object) $"<b>Blackboard:</b> Variable with name '{varName}' already exists in blackboard '{this.name}', but is of different type! Returning null instead of new.");
        return variable;
      }
      Debug.LogWarning((object) $"<b>Blackboard:</b> Variable with name '{varName}' already exists in blackboard '{this.name}'. Returning existing instead of new.");
      return variable;
    }
    Variable instance = (Variable) Activator.CreateInstance(typeof (Variable<>).RTMakeGenericType(type));
    instance.name = varName;
    this.variables[varName] = instance;
    if (this.onVariableAdded != null)
      this.onVariableAdded(instance);
    return instance;
  }

  public Variable RemoveVariable(string varName)
  {
    Variable variable = (Variable) null;
    if (this.variables.TryGetValue(varName, out variable))
    {
      this.variables.Remove(varName);
      if (this.onVariableRemoved != null)
        this.onVariableRemoved(variable);
    }
    return variable;
  }

  public T GetValue<T>(string varName)
  {
    try
    {
      return (this.variables[varName] as Variable<T>).value;
    }
    catch
    {
      try
      {
        return (T) this.variables[varName].value;
      }
      catch
      {
        if (!this.variables.ContainsKey(varName))
        {
          Debug.LogError((object) $"<b>Blackboard:</b> No Variable of name '{varName}' and type '{typeof (T).FriendlyName()}' exists on Blackboard '{this.name}'. Returning default T...");
          return default (T);
        }
      }
    }
    Debug.LogError((object) $"<b>Blackboard:</b> Can't cast value of variable with name '{varName}' to type '{typeof (T).FriendlyName()}'");
    return default (T);
  }

  public Variable SetValue(string varName, object value)
  {
    try
    {
      Variable variable = this.variables[varName];
      variable.value = value;
      return variable;
    }
    catch
    {
      if (!this.variables.ContainsKey(varName))
      {
        Debug.Log((object) $"<b>Blackboard:</b> No Variable of name '{varName}' and type '{(value != null ? (object) value.GetType().FriendlyName() : (object) "null")}' exists on Blackboard '{this.name}'. Adding new instead...");
        Variable variable = this.AddVariable(varName, value);
        variable.isProtected = true;
        return variable;
      }
    }
    Debug.LogError((object) $"<b>Blackboard:</b> Can't cast value '{(value != null ? (object) value.ToString() : (object) "null")}' to blackboard variable of name '{varName}' and type '{this.variables[varName].varType.Name}'");
    return (Variable) null;
  }

  public Variable GetVariable(string varName, System.Type ofType = null)
  {
    Variable variable;
    return this.variables != null && varName != null && this.variables.TryGetValue(varName, out variable) && (System.Type.op_Equality(ofType, (System.Type) null) || variable.CanConvertTo(ofType)) ? variable : (Variable) null;
  }

  public Variable GetVariableByID(string ID)
  {
    if (this.variables != null && ID != null)
    {
      foreach (KeyValuePair<string, Variable> variable in this.variables)
      {
        if (variable.Value.ID == ID)
          return variable.Value;
      }
    }
    return (Variable) null;
  }

  public Variable<T> GetVariable<T>(string varName)
  {
    return (Variable<T>) this.GetVariable(varName, typeof (T));
  }

  public string[] GetVariableNames() => this.variables.Keys.ToArray<string>();

  public string[] GetVariableNames(System.Type ofType)
  {
    return this.variables.Values.Where<Variable>((Func<Variable, bool>) (v => v.CanConvertTo(ofType))).Select<Variable, string>((Func<Variable, string>) (v => v.name)).ToArray<string>();
  }

  public Variable<T> AddVariable<T>(string varName, T value)
  {
    Variable<T> variable = this.AddVariable<T>(varName);
    variable.value = value;
    return variable;
  }

  public Variable<T> AddVariable<T>(string varName)
  {
    return (Variable<T>) this.AddVariable(varName, typeof (T));
  }
}
