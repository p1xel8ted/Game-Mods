// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeGameBoardSideExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
