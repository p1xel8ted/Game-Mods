// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.GetCloserGameObjectInList
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Blackboard/Lists")]
[Description("Get the closer game object to the agent from within a list of game objects and save it in the blackboard.")]
public class GetCloserGameObjectInList : ActionTask<Transform>
{
  [RequiredField]
  public BBParameter<List<GameObject>> list;
  [BlackboardOnly]
  public BBParameter<GameObject> saveAs;

  public override string info
  {
    get => $"Get Closer from '{this.list?.ToString()}' as {this.saveAs?.ToString()}";
  }

  public override void OnExecute()
  {
    if (this.list.value.Count == 0)
    {
      this.EndAction(false);
    }
    else
    {
      float num1 = float.PositiveInfinity;
      GameObject gameObject1 = (GameObject) null;
      foreach (GameObject gameObject2 in this.list.value)
      {
        float num2 = Vector3.Distance(this.agent.position, gameObject2.transform.position);
        if ((double) num2 < (double) num1)
        {
          num1 = num2;
          gameObject1 = gameObject2;
        }
      }
      this.saveAs.value = gameObject1;
      this.EndAction(true);
    }
  }
}
