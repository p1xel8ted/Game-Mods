// Decompiled with JetBrains decompiler
// Type: TooltipBubbleGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TooltipBubbleGUI : WidgetsBubbleGUI
{
  public static List<TooltipBubbleGUI> _all = new List<TooltipBubbleGUI>();
  public static bool _available = true;

  public override void DestroyBubble()
  {
    TooltipBubbleGUI._all.Remove(this);
    this.widget.alpha = 0.0f;
    base.DestroyBubble();
  }

  public static TooltipBubbleGUI Show(
    BubbleWidgetDataContainer data,
    bool for_gamepad,
    Collider2D tooltip_collider)
  {
    if (data == null || !data.has_data)
    {
      Debug.LogError((object) "no tooltip data");
      return (TooltipBubbleGUI) null;
    }
    TooltipBubbleGUI tooltipBubbleGui = GUIElements.me.tooltip_bubble.Copy<TooltipBubbleGUI>();
    if ((Object) tooltipBubbleGui.simple_table != (Object) null)
    {
      tooltipBubbleGui.simple_table.use_hash = false;
      tooltipBubbleGui.simple_table.ClearHashes();
    }
    tooltipBubbleGui.LinkColliderForGamepad(for_gamepad, tooltip_collider);
    tooltipBubbleGui.Show(data, true);
    TooltipBubbleGUI._all.Add(tooltipBubbleGui);
    GJL.EnsureChildLabelsHasCorrectFont(tooltipBubbleGui.gameObject, false);
    GJL.ApplyCustomFontSettings(tooltipBubbleGui.gameObject);
    return tooltipBubbleGui;
  }

  public static void ChangeAvaibility(bool available)
  {
    if (!available)
    {
      while (TooltipBubbleGUI._all.Count > 0)
        TooltipBubbleGUI._all[0].DestroyBubble();
    }
    TooltipBubbleGUI._available = available;
  }
}
