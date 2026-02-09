// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_DonateToBox
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Donate to box", 0)]
[Category("Game Actions")]
public class Flow_DonateToBox : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("Box WGO");
    ValueInput<float> par_money = this.AddValueInput<float>("Money");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(par_wgo);
      EffectBubblesManager.ShowImmediately(worldGameObject.bubble_pos, Trading.FormatMoney(par_money.value));
      worldGameObject.AddToParams("_money", par_money.value);
      Sounds.PlaySound("donations_coin");
      flow_out.Call(f);
    }));
  }
}
