// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeGameBoardSideExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
namespace Flockade;

public static class FlockadeGameBoardSideExtensions
{
  public static FlockadeGameBoardSide GetOpposing(this FlockadeGameBoardSide self)
  {
    return self != FlockadeGameBoardSide.Left ? FlockadeGameBoardSide.Left : FlockadeGameBoardSide.Right;
  }
}
