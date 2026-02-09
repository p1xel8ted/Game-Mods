// Decompiled with JetBrains decompiler
// Type: PerkDefinition
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class PerkDefinition : BalanceBaseObject
{
  public GameRes output_res = new GameRes();
  public string icon = "";
  public float stars;
  public bool show = true;

  public string GetIcon() => !string.IsNullOrEmpty(this.icon) ? this.icon : "i_" + this.id;

  public string GetDescriptionIfExists()
  {
    string lng_id = this.id + "_d";
    string str = GJL.L(lng_id);
    return !(lng_id == str) ? str : string.Empty;
  }
}
