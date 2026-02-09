// Decompiled with JetBrains decompiler
// Type: BuffDefinition
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class BuffDefinition : BalanceBaseObject
{
  public bool is_hidden;
  public GameRes res = new GameRes();
  public SmartExpression length;
  public bool do_not_show_timer;
  public float tick_period;
  public SmartExpression se_start;
  public SmartExpression se_finish;
  public SmartExpression se_tick;
  public string custom_icon;
  public GameRes condition_player_res = new GameRes();
  public float craft_q;
  public BuffDefinition.BuffOverlayType overlay_type;

  public string GetIconName()
  {
    return !string.IsNullOrEmpty(this.custom_icon) ? this.custom_icon : "b_" + this.id;
  }

  public string GetLocalizedName() => GJL.L(this.id);

  public string GetDescriptionIfExists()
  {
    string lng_id = this.id + "_d";
    string str = GJL.L(lng_id);
    return !(lng_id == str) ? str : string.Empty;
  }

  public enum BuffOverlayType
  {
    Set,
    Add,
  }
}
