// Decompiled with JetBrains decompiler
// Type: DeleteOnVariable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class DeleteOnVariable : BaseMonoBehaviour
{
  public List<DeleteOnVariable.VariableAndCondition> DeleteConditions = new List<DeleteOnVariable.VariableAndCondition>();
  public bool justDeactive;

  private void OnEnable()
  {
    SaveAndLoad.OnLoadComplete += new System.Action(this.OnLoadComplete);
    this.Play();
  }

  private void OnDisable() => SaveAndLoad.OnLoadComplete -= new System.Action(this.OnLoadComplete);

  private void OnLoadComplete()
  {
    SaveAndLoad.OnLoadComplete -= new System.Action(this.OnLoadComplete);
    this.Play();
  }

  private void Play()
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
