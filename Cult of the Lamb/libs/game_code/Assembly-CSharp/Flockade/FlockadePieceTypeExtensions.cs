// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadePieceTypeExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
