// Decompiled with JetBrains decompiler
// Type: WOPPrefabName
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class WOPPrefabName
{
  [SerializeField]
  public string part_1 = "";
  [SerializeField]
  public int stage;
  [SerializeField]
  public string part_2 = "";
  [SerializeField]
  public bool need_herb;

  public string GetName()
  {
    if (string.IsNullOrEmpty(this.part_1))
      return "";
    if (this.stage <= 0)
      return this.part_1;
    return string.IsNullOrEmpty(this.part_2) ? this.part_1 + this.stage.ToString() : this.part_1 + this.stage.ToString() + this.part_2;
  }

  public bool EqualsTo(WOPPrefabName other_obj)
  {
    return other_obj != null && this.part_1 == other_obj.part_1 && this.stage == other_obj.stage && this.part_2 == other_obj.part_2 && this.need_herb == other_obj.need_herb;
  }
}
