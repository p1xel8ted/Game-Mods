// Decompiled with JetBrains decompiler
// Type: Map.MapLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Map;

[Serializable]
public class MapLayer
{
  public NodeBlueprint BluePrint;
  public NodeBlueprint[] OtherBluePrints;
  [Tooltip("Default node for this map layer. If Randomize Nodes is 0, you will get this node 100% of the time")]
  public NodeType nodeType;
  public FloatMinMax distanceFromPreviousLayer;
  [Tooltip("Distance between the nodes on this layer")]
  public float nodesApartDistance;
  [Tooltip("If this is set to 0, nodes on this layer will appear in a straight line. Closer to 1f = more position randomization")]
  [Range(0.0f, 1f)]
  public float randomizePosition;
  [Tooltip("Chance to get a random node that is different from the default node on this layer")]
  [Range(0.0f, 1f)]
  public float randomizeNodes;
  public bool CanBeReplacedWithCombatNode = true;
}
