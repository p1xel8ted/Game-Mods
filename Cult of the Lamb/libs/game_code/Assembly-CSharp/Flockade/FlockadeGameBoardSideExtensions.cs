// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeGameBoardSideExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
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
