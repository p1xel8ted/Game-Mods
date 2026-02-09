// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_FindGDPoint
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Find GD point", 0)]
[Category("Game Functions")]
[Color("eed9a7")]
[FlowNode.ContextDefinedOutputs(new System.Type[] {typeof (GameObject)})]
public class Flow_FindGDPoint : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> par_name = this.AddValueInput<string>("Name");
    ValueInput<string> par_ctag = this.AddValueInput<string>("GD tag");
    ValueInput<bool> par_ignore_errors = this.AddValueInput<bool>("Not found is OK", "Ignore not\nfound error");
    this.AddValueOutput<GameObject>("GD point", (ValueHandler<GameObject>) (() =>
    {
      if (par_name.HasValue<string>())
      {
        GDPoint gdPointByName = WorldMap.GetGDPointByName(par_name.value, !par_ignore_errors.value);
        return !((UnityEngine.Object) gdPointByName == (UnityEngine.Object) null) ? gdPointByName.gameObject : (GameObject) null;
      }
      if (par_ctag.HasValue<string>())
      {
        GDPoint gdPointByGdTag = WorldMap.GetGDPointByGDTag(par_ctag.value, !par_ignore_errors.value);
        return !((UnityEngine.Object) gdPointByGdTag == (UnityEngine.Object) null) ? gdPointByGdTag.gameObject : (GameObject) null;
      }
      Debug.LogError((object) "Flow_FindGDPoint: no input parameters set");
      return (GameObject) null;
    }));
  }

  public override string name
  {
    get
    {
      string name = base.name;
      if (!this.GetInputValuePort<string>("Name").isDefaultValue || this.GetInputValuePort<string>("Name").isConnected)
        return name + " by name";
      return !this.GetInputValuePort<string>("GD tag").isDefaultValue || this.GetInputValuePort<string>("GD tag").isConnected ? name + " by GD tag" : name + "\nby ??????";
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
