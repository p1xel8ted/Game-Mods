// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_add_player_param_after_hp_0_k
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Affect add_player_param_after_hp_0_k", 0)]
public class Flow_add_player_param_after_hp_0_k : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> wgo = this.AddValueInput<WorldGameObject>("WGO");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject wgo1 = this.WGOParamOrSelf(wgo);
      GameRes game_res = new GameRes(wgo1.obj_def.add_player_param_after_hp_0);
      if (wgo1.obj_def.add_player_param_after_hp_0_k.has_expression)
      {
        float num = wgo1.obj_def.add_player_param_after_hp_0_k.EvaluateFloat(wgo1);
        foreach (GameResAtom atom in game_res.ToAtomList())
          game_res.Set(atom.type, atom.value * num);
      }
      MainGame.me.player.AddToParams(game_res);
      if ((double) game_res.Get("hp") < 0.0)
        EffectBubblesManager.ShowStackedHP(MainGame.me.player, game_res.Get("hp"));
      flow_out.Call(f);
    }));
  }
}
