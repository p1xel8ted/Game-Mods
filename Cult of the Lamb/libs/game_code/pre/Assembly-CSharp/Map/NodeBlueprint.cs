// Decompiled with JetBrains decompiler
// Type: Map.NodeBlueprint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  [Space]
  public bool ForceDisplayModifier;
  public Sprite ForceDisplayModifierIcon;
  [Space]
  public bool UseCustomLayerIcons;
  public LayerSprite[] LayerSprites;
  public bool Testing;

  public Sprite GetSprite(FollowerLocation dungeonLocation, bool decoration = true)
  {
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
