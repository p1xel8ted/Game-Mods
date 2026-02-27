// Decompiled with JetBrains decompiler
// Type: Map.NodeBlueprint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Map;

[CreateAssetMenu]
public class NodeBlueprint : ScriptableObject
{
  public Sprite sprite;
  public Sprite spriteNoDecoration;
  public Sprite flair;
  public NodeType nodeType;
  public string[] RoomPrefabs;
  [Space]
  [Range(0.0f, 1f)]
  public float Probability = 1f;
  [Space]
  public bool CanHaveModifier;
  public bool CanBeHidden;
  public bool OnlyOne;
  public bool RequireCondition;
  public List<BiomeGenerator.VariableAndCondition> ConditionalVariables = new List<BiomeGenerator.VariableAndCondition>();
  public bool HasEnsuredConditions;
  public List<BiomeGenerator.VariableAndCondition> EnsuredConditionalVariables = new List<BiomeGenerator.VariableAndCondition>();
  public bool IgnoreRequiredConditions;
  [Space]
  public bool ForceDisplayModifier;
  public Sprite ForceDisplayModifierIcon;
  [Space]
  public bool UseCustomLayerIcons;
  public LayerSprite[] LayerSprites;
  [Space]
  public bool HasCost;
  public InventoryItem.ITEM_TYPE CostType;
  public int CostAmount;
  public BiomeGenerator.VariableAndCondition UnlockedVariable;
  public NodeBlueprint UnlockedBlueprint;
  [Space]
  public FollowerLocation ForcedDungeon = FollowerLocation.None;
  public bool Testing;

  public Sprite GetSprite(FollowerLocation dungeonLocation, bool decoration = true)
  {
    if (this.ForcedDungeon != FollowerLocation.None)
      dungeonLocation = this.ForcedDungeon;
    if (this.UseCustomLayerIcons && dungeonLocation != FollowerLocation.None)
    {
      foreach (LayerSprite layerSprite in this.LayerSprites)
      {
        if (layerSprite.DungeonLocation == dungeonLocation)
          return decoration ? layerSprite.Sprite : layerSprite.SpriteNoDecoration;
      }
    }
    return decoration ? this.sprite : this.spriteNoDecoration;
  }
}
