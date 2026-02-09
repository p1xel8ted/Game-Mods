// Decompiled with JetBrains decompiler
// Type: WorldZoneDefinition
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class WorldZoneDefinition : BalanceBaseObject
{
  public WorldZoneDefinition.QualityCalcMethod calc_method;
  public string quality_icon;
  public string hud_descr_str;
  public string gui_descr_str;
  public string string_format;
  public string zone_group;
  public GameRes zone_params = new GameRes();

  public enum QualityCalcMethod
  {
    None,
    Sum,
    Average,
  }
}
