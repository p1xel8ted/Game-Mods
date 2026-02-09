// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.GetAllChildGameObjects
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("GameObject")]
public class GetAllChildGameObjects : ActionTask<Transform>
{
  [BlackboardOnly]
  public BBParameter<List<GameObject>> saveAs;
  public bool recursive;

  public override string info
  {
    get
    {
      return $"{this.saveAs} = {(this.recursive ? (object) "All" : (object) "First")} Children Of {this.agentInfo}";
    }
  }

  public override void OnExecute()
  {
    List<Transform> source = new List<Transform>();
    foreach (Transform parent in this.agent.transform)
    {
      source.Add(parent);
      if (this.recursive)
        source.AddRange((IEnumerable<Transform>) this.Get(parent));
    }
    this.saveAs.value = source.Select<Transform, GameObject>((Func<Transform, GameObject>) (t => t.gameObject)).ToList<GameObject>();
    this.EndAction();
  }

  public List<Transform> Get(Transform parent)
  {
    List<Transform> transformList = new List<Transform>();
    foreach (Transform parent1 in parent)
    {
      transformList.Add(parent1);
      transformList.AddRange((IEnumerable<Transform>) this.Get(parent1));
    }
    return transformList;
  }
}
