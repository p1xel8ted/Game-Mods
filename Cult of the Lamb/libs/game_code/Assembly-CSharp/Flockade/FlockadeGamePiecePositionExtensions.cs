// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeGamePiecePositionExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
namespace Flockade;

public static class FlockadeGamePiecePositionExtensions
{
  public static FlockadeFight.GamePiecePosition GetOpposing(
    this FlockadeFight.GamePiecePosition self)
  {
    FlockadeFight.GamePiecePosition opposing;
    switch (self)
    {
      case FlockadeFight.GamePiecePosition.Left:
        opposing = FlockadeFight.GamePiecePosition.Right;
        break;
      case FlockadeFight.GamePiecePosition.Right:
        opposing = FlockadeFight.GamePiecePosition.Left;
        break;
      default:
        opposing = self;
        break;
    }
    return opposing;
  }
}
