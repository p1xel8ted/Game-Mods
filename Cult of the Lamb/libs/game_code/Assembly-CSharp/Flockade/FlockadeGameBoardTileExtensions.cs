// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeGameBoardTileExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Flockade;

public static class FlockadeGameBoardTileExtensions
{
  public static void SetInspectable<T>(this IEnumerable<T> self, bool inspectable, Color highlight = default (Color)) where T : FlockadeGameBoardTile
  {
    foreach (T obj in self)
      obj.SetInspectable(inspectable, highlight);
  }
}
