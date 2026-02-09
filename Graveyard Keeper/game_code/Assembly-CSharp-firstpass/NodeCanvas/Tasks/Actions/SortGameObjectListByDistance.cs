// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.SortGameObjectListByDistance
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Blackboard/Lists")]
[Description("Will sort the gameobjects in the target list by their distance to the agent (closer first) and save that list to the blackboard")]
public class SortGameObjectListByDistance : ActionTask<Transform>
{
  [RequiredField]
  [BlackboardOnly]
  public BBParameter<List<GameObject>> targetList;
  [BlackboardOnly]
  public BBParameter<List<GameObject>> saveAs;
  public bool reverse;

  public override string info
  {
    get => $"Sort {this.targetList?.ToString()} by distance as {this.saveAs?.ToString()}";
  }

  public override void OnExecute()
  {
    this.saveAs.value = this.targetList.value.OrderBy<GameObject, float>((Func<GameObject, float>) (go => Vector3.Distance(go.transform.position, this.agent.position))).ToList<GameObject>();
    if (this.reverse)
      this.saveAs.value.Reverse();
    this.EndAction();
  }

  [CompilerGenerated]
  public float \u003COnExecute\u003Eb__5_0(GameObject go)
  {
    return Vector3.Distance(go.transform.position, this.agent.position);
  }
}
