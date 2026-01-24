// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeGamePieceHolderExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections.Generic;

#nullable disable
namespace Flockade;

public static class FlockadeGamePieceHolderExtensions
{
  public static Sequence SetActive<T>(this IEnumerable<T> self, bool active) where T : FlockadeGamePieceHolder
  {
    Sequence s = DOTween.Sequence().AppendInterval(0.0166666675f);
    foreach (T obj in self)
      s.Join((Tween) obj.SetActive(active));
    return s;
  }
}
