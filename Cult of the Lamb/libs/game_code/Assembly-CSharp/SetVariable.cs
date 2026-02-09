// Decompiled with JetBrains decompiler
// Type: SetVariable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
