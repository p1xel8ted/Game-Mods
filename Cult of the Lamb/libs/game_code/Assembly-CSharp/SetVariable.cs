// Decompiled with JetBrains decompiler
// Type: SetVariable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SetVariable : MonoBehaviour
{
  public List<SetVariable.VariableAndCondition> SetConditions = new List<SetVariable.VariableAndCondition>();
  [SerializeField]
  public bool onStart;

  public void Start()
  {
    if (!this.onStart)
      return;
    this.Set();
  }

  public void Set()
  {
    foreach (SetVariable.VariableAndCondition setCondition in this.SetConditions)
    {
      if (DataManager.Instance.GetVariable(setCondition.Variable) != setCondition.Condition)
      {
        DataManager.Instance.SetVariable(setCondition.Variable, setCondition.Condition);
        break;
      }
    }
  }

  [Serializable]
  public class VariableAndCondition
  {
    public DataManager.Variables Variable;
    public bool Condition = true;
  }
}
