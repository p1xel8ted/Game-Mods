// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.Blackboard
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework.Internal;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Framework;

[SpoofAOT]
public class Blackboard : MonoBehaviour, ISerializationCallbackReceiver, IBlackboard
{
  [SerializeField]
  public string _serializedBlackboard;
  [SerializeField]
  public List<UnityEngine.Object> _objectReferences;
  [NonSerialized]
  public BlackboardSource _blackboard = new BlackboardSource();
  [NonSerialized]
  public bool hasDeserialized;

  public event Action<Variable> onVariableAdded;

  public event Action<Variable> onVariableRemoved;

  void ISerializationCallbackReceiver.OnBeforeSerialize()
  {
  }

  void ISerializationCallbackReceiver.OnAfterDeserialize()
  {
    if (this.hasDeserialized && JSONSerializer.applicationPlaying)
      return;
    this.hasDeserialized = true;
    this._blackboard = JSONSerializer.Deserialize<BlackboardSource>(this._serializedBlackboard, this._objectReferences);
    if (this._blackboard != null)
      return;
    this._blackboard = new BlackboardSource();
  }

  public virtual void Awake()
  {
    this._blackboard.InitializePropertiesBinding(this.propertiesBindTarget, false);
  }

  public new string name
  {
    get
    {
      return !string.IsNullOrEmpty(this._blackboard.name) ? this._blackboard.name : this.gameObject.name + "_BB";
    }
    set
    {
      if (string.IsNullOrEmpty(value))
        value = this.gameObject.name + "_BB";
      this._blackboard.name = value;
    }
  }

  public object this[string varName]
  {
    get => this._blackboard[varName];
    set => this.SetValue(varName, value);
  }

  public Dictionary<string, Variable> variables
  {
    get => this._blackboard.variables;
    set => this._blackboard.variables = value;
  }

  public GameObject propertiesBindTarget => this.gameObject;

  public Variable AddVariable(string name, System.Type type)
  {
    Variable variable = this._blackboard.AddVariable(name, type);
    if (this.onVariableAdded != null)
      this.onVariableAdded(variable);
    return variable;
  }

  public Variable AddVariable(string name, object value)
  {
    Variable variable = this._blackboard.AddVariable(name, value);
    if (this.onVariableAdded != null)
      this.onVariableAdded(variable);
    return variable;
  }

  public Variable RemoveVariable(string name)
  {
    Variable variable = this._blackboard.RemoveVariable(name);
    if (this.onVariableRemoved != null)
      this.onVariableRemoved(variable);
    return variable;
  }

  public Variable GetVariable(string name, System.Type ofType = null)
  {
    return this._blackboard.GetVariable(name, ofType);
  }

  public Variable GetVariableByID(string ID) => this._blackboard.GetVariableByID(ID);

  public Variable<T> GetVariable<T>(string name) => this._blackboard.GetVariable<T>(name);

  public T GetValue<T>(string name) => this._blackboard.GetValue<T>(name);

  public Variable SetValue(string name, object value) => this._blackboard.SetValue(name, value);

  public string[] GetVariableNames() => this._blackboard.GetVariableNames();

  public string[] GetVariableNames(System.Type ofType) => this._blackboard.GetVariableNames(ofType);

  public string Save() => this.Save(this.name);

  public string Save(string saveKey)
  {
    string str = this.Serialize();
    PlayerPrefs.SetString(saveKey, str);
    return str;
  }

  public bool Load() => this.Load(this.name);

  public bool Load(string saveKey)
  {
    string json = PlayerPrefs.GetString(saveKey);
    if (!string.IsNullOrEmpty(json))
      return this.Deserialize(json);
    Debug.Log((object) ("No data to load blackboard variables from key " + saveKey));
    return false;
  }

  public string Serialize() => this.Serialize(this._objectReferences);

  public string Serialize(List<UnityEngine.Object> storedObjectReferences)
  {
    return JSONSerializer.Serialize(typeof (BlackboardSource), (object) this._blackboard, objectReferences: storedObjectReferences);
  }

  public bool Deserialize(string json) => this.Deserialize(json, this._objectReferences);

  public bool Deserialize(string json, List<UnityEngine.Object> storedObjectReferences, bool removeMissing = true)
  {
    BlackboardSource blackboardSource = JSONSerializer.Deserialize<BlackboardSource>(json, storedObjectReferences);
    if (blackboardSource == null)
      return false;
    foreach (KeyValuePair<string, Variable> variable in blackboardSource.variables)
    {
      if (this._blackboard.variables.ContainsKey(variable.Key))
        this._blackboard.SetValue(variable.Key, variable.Value.value);
      else
        this._blackboard.variables[variable.Key] = variable.Value;
    }
    if (removeMissing)
    {
      foreach (string key in new List<string>((IEnumerable<string>) this._blackboard.variables.Keys))
      {
        if (!blackboardSource.variables.ContainsKey(key))
          this._blackboard.variables.Remove(key);
      }
    }
    this._blackboard.InitializePropertiesBinding(this.propertiesBindTarget, true);
    return true;
  }
}
