// Decompiled with JetBrains decompiler
// Type: SimpleDeleteOnCondiiton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SimpleDeleteOnCondiiton : BaseMonoBehaviour
{
  public List<Interaction_SimpleConversation.VariableAndCondition> DeleteConditions = new List<Interaction_SimpleConversation.VariableAndCondition>();
  public List<GameObject> ObjectsToDelete = new List<GameObject>();
  public List<Interaction_SimpleConversation.VariableAndCondition> SetConditions = new List<Interaction_SimpleConversation.VariableAndCondition>();
  public bool DisableNotDestroy;
  public bool DoInAwake;

  public void Awake()
  {
    if (!this.DoInAwake)
      return;
    this.Start();
  }

  public void Start()
  {
    if (this.DeleteConditions.Count <= 0)
      return;
    bool flag = true;
    foreach (Interaction_SimpleConversation.VariableAndCondition deleteCondition in this.DeleteConditions)
    {
      if (DataManager.Instance.GetVariable(deleteCondition.Variable) != deleteCondition.Condition)
      {
        flag = false;
        break;
      }
    }
    if (!flag)
      return;
    foreach (GameObject gameObject in this.ObjectsToDelete)
    {
      if (this.DisableNotDestroy)
        gameObject.SetActive(false);
      else
        Object.Destroy((Object) gameObject);
    }
  }

  public void Play()
  {
    foreach (Interaction_SimpleConversation.VariableAndCondition setCondition in this.SetConditions)
      DataManager.Instance.SetVariable(setCondition.Variable, setCondition.Condition);
  }
}
