// Decompiled with JetBrains decompiler
// Type: SoulExtractorPanelBarGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SoulExtractorPanelBarGUI : MonoBehaviour
{
  public const string MIN_CORRUPTION_ID = "min_corruption_chance";
  public const string MAX_CORRUPTION_ID = "max_corruption_chance";
  public const string CORRUPTION_CHANCE = "corruption_chance";
  public const int HORIZONTAL_BAR_SIZE = 108;
  [Range(0.0f, 1f)]
  public float durability;
  [SerializeField]
  public float min_corruption_chance;
  [SerializeField]
  public float max_corruption_chance;
  [SerializeField]
  public float corruption_chance;
  [SerializeField]
  public UIProgressBar progressBar;
  [SerializeField]
  public UIProgressBar min_corruption_widget;
  [SerializeField]
  public UIProgressBar range_corruption_widget;
  [SerializeField]
  public GameObject range_corruption_widget_thumb;
  [SerializeField]
  public UILabel corruption_chance_label;
  [SerializeField]
  public GameObject hints_container;

  public void SetData(
    float durability,
    float min_corruption_chance = 0.0f,
    float max_corruption_chance = 0.0f,
    float corruption_chance = 0.0f)
  {
    this.durability = durability;
    this.min_corruption_chance = min_corruption_chance;
    this.max_corruption_chance = max_corruption_chance;
    this.corruption_chance = corruption_chance;
  }

  public void Redraw()
  {
    Mathf.CeilToInt(this.durability * 100f);
    this.range_corruption_widget_thumb.SetActive(true);
    if (this.min_corruption_chance.EqualsTo(0.0f))
    {
      this.min_corruption_widget.SetActive(false);
    }
    else
    {
      if ((double) this.durability <= (double) this.min_corruption_chance)
        this.min_corruption_chance = this.durability;
      this.min_corruption_widget.value = this.min_corruption_chance;
      this.min_corruption_widget.ForceUpdate();
      this.min_corruption_widget.SetActive(true);
    }
    if (this.max_corruption_chance.EqualsTo(0.0f))
    {
      this.range_corruption_widget.gameObject.SetActive(false);
    }
    else
    {
      if ((double) this.durability <= (double) this.max_corruption_chance)
      {
        this.max_corruption_chance = this.durability;
        this.range_corruption_widget_thumb.SetActive(false);
      }
      this.range_corruption_widget.value = this.max_corruption_chance;
      this.range_corruption_widget.ForceUpdate();
      this.corruption_chance_label.text = $"{Mathf.CeilToInt(this.corruption_chance * 100f)}%";
      this.range_corruption_widget.gameObject.SetActive(true);
    }
    this.hints_container.SetActive(false);
    this.progressBar.value = this.durability;
    this.progressBar.ForceUpdate();
  }
}
