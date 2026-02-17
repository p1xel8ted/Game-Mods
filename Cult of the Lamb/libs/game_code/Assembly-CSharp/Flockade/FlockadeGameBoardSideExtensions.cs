// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeGameBoardSideExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
