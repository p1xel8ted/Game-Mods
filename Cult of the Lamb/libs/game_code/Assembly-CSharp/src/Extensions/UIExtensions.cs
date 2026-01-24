// Decompiled with JetBrains decompiler
// Type: src.Extensions.UIExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using System;
using System.Collections;

#nullable disable
namespace src.Extensions;

public static class UIExtensions
{
  public static IEnumerator YieldUntilHide(this UIMenuBase menu)
  {
    bool hidden = false;
    menu.OnHide += (Action) (() => hidden = true);
    while (!hidden)
      yield return (object) null;
  }

  public static IEnumerator YieldUntilHidden(this UIMenuBase menu)
  {
    bool hidden = false;
    menu.OnHidden += (Action) (() => hidden = true);
    while (!hidden)
      yield return (object) null;
  }

  public static IEnumerator YieldUntilShow(this UIMenuBase menu)
  {
    bool hidden = false;
    menu.OnShow += (Action) (() => hidden = true);
    while (!hidden)
      yield return (object) null;
  }

  public static IEnumerator YieldUntilShown(this UIMenuBase menu)
  {
    bool hidden = false;
    menu.OnShown += (Action) (() => hidden = true);
    while (!hidden)
      yield return (object) null;
  }
}
