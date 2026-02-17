// Decompiled with JetBrains decompiler
// Type: Map.NodeExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
namespace Map;

public static class NodeExtensions
{
  public static bool ShouldIncrementRandomRoomsEncountered(this NodeType nodeType)
  {
    switch (nodeType)
    {
      case NodeType.Sherpa:
      case NodeType.Knucklebones:
      case NodeType.Special_Teleporter:
      case NodeType.special_Challenge:
      case NodeType.Special_RewardChoice:
      case NodeType.Special_HappyFollower:
      case NodeType.Special_DissentingFollower:
      case NodeType.Rare_Gold:
      case NodeType.Lore_Haro:
      case NodeType.Special_HealthChoice:
        return true;
      default:
        return false;
    }
  }
}
