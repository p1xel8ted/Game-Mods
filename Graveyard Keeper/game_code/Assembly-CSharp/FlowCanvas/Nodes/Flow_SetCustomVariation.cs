// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_SetCustomVariation
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Set Custom Variation", 0)]
public class Flow_SetCustomVariation : MyFlowNode
{
  public Flow_SetCustomVariation.VariationsSet[] tavern_sets = new Flow_SetCustomVariation.VariationsSet[6]
  {
    new Flow_SetCustomVariation.VariationsSet()
    {
      nearest_gd_point_tag = "tavern_custom_variation_1",
      variation = 1
    },
    new Flow_SetCustomVariation.VariationsSet()
    {
      nearest_gd_point_tag = "tavern_custom_variation_2",
      variation = 2
    },
    new Flow_SetCustomVariation.VariationsSet()
    {
      nearest_gd_point_tag = "tavern_custom_variation_3",
      variation = 4
    },
    new Flow_SetCustomVariation.VariationsSet()
    {
      nearest_gd_point_tag = "tavern_custom_variation_4",
      variation = 8
    },
    new Flow_SetCustomVariation.VariationsSet()
    {
      nearest_gd_point_tag = "tavern_custom_variation_5",
      variation = 1
    },
    new Flow_SetCustomVariation.VariationsSet()
    {
      nearest_gd_point_tag = "tavern_custom_variation_6",
      variation = 2
    }
  };
  public int _variation = -1;

  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<Flow_SetCustomVariation.CustomVariationSetType> in_type = this.AddValueInput<Flow_SetCustomVariation.CustomVariationSetType>("Type");
    this.AddValueOutput<int>("Variation", (ValueHandler<int>) (() => this._variation));
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      this._variation = -1;
      WorldGameObject worldGameObject = this.WGOParamOrSelf(in_wgo);
      if ((UnityEngine.Object) worldGameObject == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "Flow_SetCustomVariation error: WGO is null!");
        flow_out.Call(f);
      }
      else
      {
        switch (in_type.value)
        {
          case Flow_SetCustomVariation.CustomVariationSetType.None:
            Debug.LogError((object) "Flow_SetCustomVariation error: type is None!");
            flow_out.Call(f);
            break;
          case Flow_SetCustomVariation.CustomVariationSetType.PlayersTavern:
            Flow_SetCustomVariation.VariationsSet[] tavernSets = this.tavern_sets;
            Vector2 position = (Vector2) worldGameObject.transform.position;
            Flow_SetCustomVariation.VariationsSet variationsSet = tavernSets[0];
            GDPoint gdPointByGdTag1 = WorldMap.GetGDPointByGDTag(variationsSet.nearest_gd_point_tag);
            if ((UnityEngine.Object) gdPointByGdTag1 == (UnityEngine.Object) null)
            {
              Debug.LogError((object) $"FATAL ERROR! Flow_SetCustomVariation error: not found GDPoint with tag \"{variationsSet.nearest_gd_point_tag}\"!");
              flow_out.Call(f);
              break;
            }
            float num = ((Vector2) gdPointByGdTag1.pos - position).sqrMagnitude;
            if (tavernSets.Length > 1)
            {
              for (int index = 1; index < tavernSets.Length; ++index)
              {
                GDPoint gdPointByGdTag2 = WorldMap.GetGDPointByGDTag(tavernSets[index].nearest_gd_point_tag, false);
                if (!((UnityEngine.Object) gdPointByGdTag2 == (UnityEngine.Object) null))
                {
                  float sqrMagnitude = ((Vector2) gdPointByGdTag2.pos - position).sqrMagnitude;
                  if ((double) sqrMagnitude < (double) num)
                  {
                    variationsSet = tavernSets[index];
                    num = sqrMagnitude;
                  }
                }
              }
            }
            if (variationsSet != null)
            {
              Debug.Log((object) $"Set custom variation to WGO={worldGameObject.obj_id}, variation={variationsSet.variation}");
              worldGameObject.variation = variationsSet.variation;
              this._variation = variationsSet.variation;
              worldGameObject.Redraw();
            }
            flow_out.Call(f);
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }));
  }

  public class VariationsSet
  {
    public string nearest_gd_point_tag = string.Empty;
    public int variation = 1;
  }

  public enum CustomVariationSetType
  {
    None,
    PlayersTavern,
  }
}
