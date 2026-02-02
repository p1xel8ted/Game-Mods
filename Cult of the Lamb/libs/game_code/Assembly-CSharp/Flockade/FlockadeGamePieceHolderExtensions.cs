// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeGamePieceHolderExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
