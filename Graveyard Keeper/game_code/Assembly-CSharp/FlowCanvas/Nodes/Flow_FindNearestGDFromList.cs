// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_FindNearestGDFromList
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Functions")]
[Name("Find Nearest GD point from List<gd_tag>", 0)]
[FlowNode.ContextDefinedOutputs(new System.Type[] {typeof (GameObject)})]
public class Flow_FindNearestGDFromList : MyFlowNode
{
  public List<string> gd_point_tags = new List<string>();

  public override void RegisterPorts()
  {
    WorldGameObject _wgo = (WorldGameObject) null;
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO");
    this.AddValueOutput<GameObject>("GD point", (ValueHandler<GameObject>) (() =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(par_wgo);
      _wgo = worldGameObject;
      if ((UnityEngine.Object) worldGameObject == (UnityEngine.Object) null)
        Debug.LogError((object) "Error: WGO is null");
      return WorldMap.FIndNearestGDPointFromList(this.gd_point_tags, _wgo).gameObject;
    }));
  }
}
