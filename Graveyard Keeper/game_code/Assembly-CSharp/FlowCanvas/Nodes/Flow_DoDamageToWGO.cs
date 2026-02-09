// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_DoDamageToWGO
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Description("Do Damage To WGO")]
[Name("Do Damage To WGO", 0)]
public class Flow_DoDamageToWGO : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<float> in_damage = this.AddValueInput<float>("damage");
    ValueInput<Direction> in_direction = this.AddValueInput<Direction>("damaged from");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if ((double) in_damage.value < 1.0)
      {
        Debug.LogError((object) "Wrong damage!");
      }
      else
      {
        WorldGameObject worldGameObject = (UnityEngine.Object) in_wgo.value != (UnityEngine.Object) null ? in_wgo.value : MainGame.me.player;
        if (worldGameObject.is_dead || !worldGameObject.components.hp.enabled)
          return;
        worldGameObject.components.hp.DecHP(in_damage.value);
        float damage_direction;
        switch (in_direction.value)
        {
          case Direction.None:
          case Direction.IgnoreDirection:
          case Direction.ToPlayer:
            damage_direction = -90f;
            break;
          case Direction.Right:
            damage_direction = 0.0f;
            break;
          case Direction.Up:
            damage_direction = 90f;
            break;
          case Direction.Left:
            damage_direction = 180f;
            break;
          case Direction.Down:
            damage_direction = -90f;
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
        if (worldGameObject.components.character.enabled)
          worldGameObject.components.character.OnWasDamaged(damage_direction);
        flow_out.Call(f);
      }
    }));
  }
}
