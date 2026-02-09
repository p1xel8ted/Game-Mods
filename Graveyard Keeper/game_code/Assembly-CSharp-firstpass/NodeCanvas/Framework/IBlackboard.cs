// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.IBlackboard
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Framework;

[SpoofAOT]
public interface IBlackboard
{
  event Action<Variable> onVariableAdded;

  event Action<Variable> onVariableRemoved;

  string name { get; set; }

  Dictionary<string, Variable> variables { get; set; }

  GameObject propertiesBindTarget { get; }

  Variable AddVariable(string varName, System.Type type);

  Variable AddVariable(string varName, object value);

  Variable RemoveVariable(string varName);

  Variable GetVariable(string varName, System.Type ofType = null);

  Variable GetVariableByID(string ID);

  Variable<T> GetVariable<T>(string varName);

  T GetValue<T>(string varName);

  Variable SetValue(string varName, object value);

  string[] GetVariableNames();

  string[] GetVariableNames(System.Type ofType);
}
