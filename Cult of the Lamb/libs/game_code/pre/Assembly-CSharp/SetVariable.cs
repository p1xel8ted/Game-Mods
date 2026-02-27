// Decompiled with JetBrains decompiler
// Type: SetVariable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SetVariable : MonoBehaviour
{
  public List<SetVariable.VariableAndCondition> SetConditions = new List<SetVariable.VariableAndCondition>();

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
