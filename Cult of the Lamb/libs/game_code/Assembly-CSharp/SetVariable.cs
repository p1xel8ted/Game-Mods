// Decompiled with JetBrains decompiler
// Type: SetVariable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
