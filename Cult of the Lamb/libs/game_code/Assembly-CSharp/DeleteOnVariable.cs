// Decompiled with JetBrains decompiler
// Type: DeleteOnVariable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class DeleteOnVariable : BaseMonoBehaviour
{
  public List<DeleteOnVariable.VariableAndCondition> DeleteConditions = new List<DeleteOnVariable.VariableAndCondition>();
  public bool justDeactive;

  public void OnEnable()
  {
    SaveAndLoad.OnLoadComplete += new System.Action(this.OnLoadComplete);
    this.Play();
  }

  public void OnDisable() => SaveAndLoad.OnLoadComplete -= new System.Action(this.OnLoadComplete);

  public void OnLoadComplete()
  {
    SaveAndLoad.OnLoadComplete -= new System.Action(this.OnLoadComplete);
    this.Play();
  }

  public void Play()
  {
    bool flag = true;
    foreach (DeleteOnVariable.VariableAndCondition deleteCondition in this.DeleteConditions)
    {
      if (DataManager.Instance.GetVariable(deleteCondition.Variable) != deleteCondition.Condition)
      {
        flag = false;
        break;
      }
    }
    if (!flag)
      return;
    if (!this.justDeactive)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    else
      this.gameObject.SetActive(false);
  }

  [Serializable]
  public class VariableAndCondition
  {
    public DataManager.Variables Variable;
    public bool Condition = true;
  }
}
