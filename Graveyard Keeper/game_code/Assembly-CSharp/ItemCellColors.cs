// Decompiled with JetBrains decompiler
// Type: ItemCellColors
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class ItemCellColors
{
  public const float BROKEN_ALPHA = 0.75f;
  public Color normal;
  public Color mouse_overed;
  public Color gamepad_overed;
  public Color wrong;
  public Color inactive;
  public Color enough_res;
  public Color not_enough_res;
  public Color money;

  public Color broken => new Color(this.wrong.r, this.wrong.g, this.wrong.b, 0.75f);
}
