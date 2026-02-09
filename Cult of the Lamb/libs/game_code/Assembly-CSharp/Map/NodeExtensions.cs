// Decompiled with JetBrains decompiler
// Type: Map.NodeExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
