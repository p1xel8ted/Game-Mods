// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadePieceTypeExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
namespace Flockade;

public static class FlockadePieceTypeExtensions
{
  public static FlockadePieceType GetKindType(this FlockadePieceType self)
  {
    return self < FlockadePieceType.Scribe ? (self >= FlockadePieceType.Shield ? FlockadePieceType.Shield : FlockadePieceType.Sword) : (self >= FlockadePieceType.Shepherd ? FlockadePieceType.Shepherd : FlockadePieceType.Scribe);
  }
}
