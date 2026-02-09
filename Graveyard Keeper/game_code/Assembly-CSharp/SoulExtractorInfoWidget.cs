// Decompiled with JetBrains decompiler
// Type: SoulExtractorInfoWidget
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SoulExtractorInfoWidget : MonoBehaviour
{
  [SerializeField]
  public float min_corruption_chance;
  [SerializeField]
  public float max_corruption_chance;
  [SerializeField]
  public UILabel label;

  public void SetData(float min_corruption_chance = 0.0f, float max_corruption_chance = 0.0f)
  {
    this.min_corruption_chance = min_corruption_chance;
    this.max_corruption_chance = max_corruption_chance;
  }

  public void Redraw()
  {
    string empty = string.Empty;
    string str = !this.min_corruption_chance.EqualsTo(0.0f) || !this.max_corruption_chance.EqualsTo(0.0f) ? $"{(int) ((double) this.min_corruption_chance * 100.0)}-{(int) ((double) this.max_corruption_chance * 100.0)}" : "0";
    this.label.text = $"{GJL.L("possible_damage")}: {str}%";
  }
}
