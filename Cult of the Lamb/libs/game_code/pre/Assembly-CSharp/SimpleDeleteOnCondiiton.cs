// Decompiled with JetBrains decompiler
// Type: SimpleDeleteOnCondiiton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SimpleDeleteOnCondiiton : BaseMonoBehaviour
{
  public List<Interaction_SimpleConversation.VariableAndCondition> DeleteConditions = new List<Interaction_SimpleConversation.VariableAndCondition>();
  public List<GameObject> ObjectsToDelete = new List<GameObject>();
  public List<Interaction_SimpleConversation.VariableAndCondition> SetConditions = new List<Interaction_SimpleConversation.VariableAndCondition>();

  private void Start()
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
    foreach (Object @object in this.ObjectsToDelete)
      Object.Destroy(@object);
  }

  public void Play()
  {
    foreach (Interaction_SimpleConversation.VariableAndCondition setCondition in this.SetConditions)
      DataManager.Instance.SetVariable(setCondition.Variable, setCondition.Condition);
  }
}
