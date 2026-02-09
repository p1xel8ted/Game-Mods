// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Refugees.Flow_GetRefugee
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes.Refugees;

[Category("Game Actions/Refugees")]
[Name("Get Refugee By Index", 0)]
public class Flow_GetRefugee : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<int> wgo_index = this.AddValueInput<int>("Refugee Index", "refugee_index");
    this.AddValueOutput<WorldGameObject>("WGO", (ValueHandler<WorldGameObject>) (() => wgo_index.value >= MainGame.me.save.refugees_camp_data.active_refugee_list.Count ? (WorldGameObject) null : MainGame.me.save.refugees_camp_data.active_refugee_list[wgo_index.value].world_game_object));
  }
}
