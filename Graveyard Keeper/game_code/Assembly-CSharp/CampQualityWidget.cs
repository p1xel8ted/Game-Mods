// Decompiled with JetBrains decompiler
// Type: CampQualityWidget
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DLCRefugees;
using UnityEngine;

#nullable disable
public class CampQualityWidget : CustomInventoryWidget
{
  public UILabel header_bottom_label;
  public const string YELLOW_COLOR = "eacf16";
  public const string RED_COLOR = "76b900";
  public const string GREEN_COLOR = "76b900";
  public const string ARROW_UP = "(up)";
  public const string ARROW_DOWN = "(down)";
  public UILabel full_slots_count;
  public UILabel empty_slots_count;
  public CampHappinessProgressWidget progress_widget;
  public CampHappinessProgressWidget progress_widget_right;
  public UIGrid progress_widgets_grid;

  public override void Init() => base.Init();

  public override void Redraw()
  {
    base.Redraw();
    float happinessProgress = RefugeesCampEngine.instance.GetCampHappinessProgress();
    int totalHappiness = RefugeesCampEngine.instance.GetTotalHappiness();
    int num1 = Mathf.FloorToInt(RefugeesCampEngine.instance.camp_zone.GetTotalQuality()) - totalHappiness;
    float prediction = RefugeesCampEngine.instance.PredictRefugeeHappinessChange(1f);
    if (totalHappiness <= 0 && (double) happinessProgress + (double) prediction < 0.0)
      prediction = -happinessProgress;
    int num2;
    if ((Object) this.header_label != (Object) null)
    {
      UILabel headerLabel = this.header_label;
      string[] strArray = new string[5]
      {
        this.inventory.name,
        " ",
        totalHappiness.ToString(),
        "/",
        null
      };
      num2 = totalHappiness + num1;
      strArray[4] = num2.ToString();
      string str = string.Concat(strArray);
      headerLabel.text = str;
    }
    if ((Object) this.header_bottom_label != (Object) null)
    {
      num2 = Mathf.RoundToInt(happinessProgress * 100f);
      string str1 = $"[c][eacf16]{num2.ToString()}%[-][/c] ";
      bool flag = false;
      if ((double) prediction > 0.0)
        str1 += "[c][76b900](up) ";
      else if ((double) prediction < 0.0)
        str1 += "[c][76b900](down) ";
      else
        flag = true;
      if (!flag)
      {
        string str2 = str1;
        num2 = Mathf.RoundToInt(prediction * 100f);
        string str3 = num2.ToString();
        str1 = $"{str2}{str3}%[-][/c]";
      }
      this.header_bottom_label.text = str1;
    }
    if ((Object) this.full_slots_count != (Object) null)
      this.full_slots_count.text = totalHappiness.ToString();
    if ((Object) this.empty_slots_count != (Object) null)
      this.empty_slots_count.text = num1.ToString();
    this.UpdateProgressWidget(happinessProgress, prediction);
  }

  public void UpdateProgressWidget(float cur_pos, float prediction)
  {
    float num1 = cur_pos + prediction;
    bool flag = (double) num1 > 1.0;
    int num2 = (double) num1 < 0.0 ? 1 : 0;
    Debug.Log((object) $"#CAMP# Camp quality: cur_pos={cur_pos}, prediction={prediction}, predicted_pos={num1}");
    if (num2 != 0)
    {
      this.progress_widget.SetActive(true);
      float num3 = Mathf.Abs(num1);
      this.progress_widget.green_filler.fillAmount = 0.0f;
      this.progress_widget.yellow_filler.invert = true;
      this.progress_widget.yellow_filler.fillAmount = 1f - num3;
      this.progress_widget.red_filler.invert = false;
      this.progress_widget.red_filler.fillAmount = num3;
      this.progress_widget_right.SetActive(true);
      this.progress_widget_right.yellow_filler.fillAmount = 0.0f;
      this.progress_widget_right.green_filler.fillAmount = 0.0f;
      this.progress_widget_right.red_filler.invert = true;
      this.progress_widget_right.red_filler.fillAmount = cur_pos;
    }
    else
    {
      if ((double) prediction > 0.0)
      {
        this.progress_widget.red_filler.fillAmount = 0.0f;
        this.progress_widget.green_filler.invert = true;
        this.progress_widget.green_filler.fillAmount = Mathf.Min(num1, 1f);
        this.progress_widget.yellow_filler.invert = true;
        this.progress_widget.yellow_filler.fillAmount = cur_pos;
      }
      else
      {
        this.progress_widget.green_filler.fillAmount = 0.0f;
        this.progress_widget.red_filler.invert = true;
        this.progress_widget.red_filler.fillAmount = cur_pos;
        this.progress_widget.yellow_filler.invert = true;
        this.progress_widget.yellow_filler.fillAmount = num1;
      }
      if (!flag)
      {
        this.progress_widget_right.SetActive(false);
      }
      else
      {
        this.progress_widget.SetActive(true);
        this.progress_widget_right.red_filler.fillAmount = 0.0f;
        this.progress_widget_right.yellow_filler.fillAmount = 0.0f;
        this.progress_widget_right.green_filler.invert = true;
        this.progress_widget_right.green_filler.fillAmount = Mathf.Min(num1 - 1f, 1f);
      }
    }
    this.progress_widgets_grid.repositionNow = true;
    this.progress_widgets_grid.Reposition();
  }

  public override GamepadNavigationItem GetFirstNavigationItem(Direction dir)
  {
    return this.GetComponent<GamepadNavigationItem>();
  }
}
