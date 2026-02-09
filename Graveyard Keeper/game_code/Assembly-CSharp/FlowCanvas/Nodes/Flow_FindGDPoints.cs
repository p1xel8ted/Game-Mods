// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_FindGDPoints
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Color("eed9a7")]
[Name("Find GD points", 0)]
[FlowNode.ContextDefinedOutputs(new System.Type[] {typeof (GameObject)})]
[Category("Game Functions")]
public class Flow_FindGDPoints : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> par_name = this.AddValueInput<string>("Name");
    ValueInput<string> par_ctag = this.AddValueInput<string>("GD tag");
    this.AddValueOutput<List<GameObject>>("List", "GD point", (ValueHandler<List<GameObject>>) (() =>
    {
      if (!par_name.isDefaultValue)
        return WorldMap.GetGDPointsByName(par_name.value);
      if (!par_ctag.isDefaultValue)
        return WorldMap.GetGDPointsByGDTag(par_ctag.value);
      Debug.LogError((object) "Flow_FindGDPoint: no input parameters set");
      return (List<GameObject>) null;
    }));
  }

  public override string name
  {
    get
    {
      string name = base.name;
      if (!this.GetInputValuePort<string>("Name").isDefaultValue)
        return name + " by name";
      return !this.GetInputValuePort<string>("GD tag").isDefaultValue ? name + " by GD tag" : name + "\nby ??????";
    }
    set => base.name = value;
  }

  public override void OnNodeInspectorGUI()
  {
    base.OnNodeInspectorGUI();
    this.MakeStringNullIfEmpty("Name");
    this.MakeStringNullIfEmpty("GD tag");
  }
}
