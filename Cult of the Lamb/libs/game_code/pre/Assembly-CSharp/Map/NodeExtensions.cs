// Decompiled with JetBrains decompiler
// Type: Map.NodeExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
