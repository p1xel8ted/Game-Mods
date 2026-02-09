// Decompiled with JetBrains decompiler
// Type: PrayCraftGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (ResourceBasedCraftGUI))]
public class PrayCraftGUI : MonoBehaviour
{
  public ResourceBasedCraftGUI _res_craft;
  public UIProgressBar progress_bar;
  public UIWidget angels_widget;
  public UIWidget icon_widget;
  public UILabel l_value_in;
  public UILabel l_value_out;
  public UILabel l_value_max;
  public UILabel l_pick_res;
  public UILabel l_button;
  public UILabel l_total_values;
  public GameObject full_indicator;
  public int angels_y_min;
  public int angels_y_max;
  public int angels_y_sky;
  public bool _animating;
  public float _result_f;
  public float _t;
  public float _cur_quality;
  public bool _block_close;
  public Item _selected_item;
  public CraftDefinition pray_craft;
  public System.Action _on_finished_animation;
  public System.Action _on_middle_animation;

  public void Init()
  {
    this._res_craft = this.GetComponent<ResourceBasedCraftGUI>();
    this._res_craft.Init();
    this.gameObject.SetActive(false);
  }

  public void Open(WorldGameObject craftery_wgo)
  {
    this._res_craft.Open(craftery_wgo, CraftDefinition.CraftType.PrayCraft);
    this.angels_widget.alpha = 0.0f;
    this.l_value_max.text = this.l_value_in.text = this.l_value_out.text = "";
    this.pray_craft = (CraftDefinition) null;
    this._cur_quality = craftery_wgo.GetMyWorldZone().GetTotalQuality();
    this.l_pick_res.text = GJL.L("preach_pick_res_hint", $"{this._cur_quality:0.0}");
    this.full_indicator.SetActive(false);
    this.progress_bar.value = 0.0f;
    this.progress_bar.thumb.gameObject.SetActive(false);
    this.icon_widget.alpha = 1f;
    this.RedrawTextValues(0.0f, 0.0f);
  }

  public void OnPrayButtonPressed()
  {
    if (this._block_close)
      return;
    Debug.Log((object) nameof (OnPrayButtonPressed));
    MainGame.me.player.SetParam("prayed_this_week", 1f);
    this._animating = false;
    this._res_craft.Hide(true);
    GS.RunFlowScript("pray");
  }

  public void OnResourcePickerClosed(Item item)
  {
    if (item == null)
      return;
    this._selected_item = item;
    float chance = this._cur_quality / item.definition.linked_craft.needs_quality;
    bool flag = false;
    if ((double) chance >= 1.0)
    {
      chance = 1f;
      flag = true;
    }
    this.pray_craft = this._selected_item == null || this._selected_item.IsEmpty() ? (CraftDefinition) null : this._selected_item.definition.linked_craft;
    this.RedrawTextValues(item.definition.linked_craft.needs_quality, chance);
    this.l_button.text = GJL.L(flag ? "btn_pray" : "btn_try_pray");
  }

  public void RedrawTextValues(float needs_q, float chance)
  {
    this.l_total_values.text = GJL.L("pray_gui_church_q", $"(cross){this._cur_quality:0.#}");
    if (this.pray_craft == null)
      return;
    UILabel lTotalValues = this.l_total_values;
    lTotalValues.text = $"{lTotalValues.text}\n{GJL.L("pray_gui_sermon_needs", $"(cross){needs_q:0}")}\n{GJL.L("sermon_success_chance", Mathf.RoundToInt(chance * 100f).ToString() + "%")}";
  }

  public bool CanClose() => !this._block_close;

  public void DoPrayForBuff(bool success, System.Action on_finished_animation, System.Action on_middle)
  {
    Debug.Log((object) ("DoPrayForBuff, success = " + success.ToString()));
    this._on_finished_animation = on_finished_animation;
    this._on_middle_animation = on_middle;
    if (this.pray_craft == null)
    {
      Debug.LogError((object) "pray_craft is null");
      this.OnMiddlePrayBuffAnimation();
      this.OnFinishedPrayBuffAnimation();
    }
    else
      MainGame.me.player_component.StartPrayAnimation(this.pray_craft, success);
  }

  public void OnFinishedPrayBuffAnimation()
  {
    Debug.Log((object) nameof (OnFinishedPrayBuffAnimation));
    Stats.DesignEvent("Pray:" + this.pray_craft.id);
    if (this._on_finished_animation == null)
      return;
    this._on_finished_animation();
  }

  public void OnMiddlePrayBuffAnimation()
  {
    Debug.Log((object) nameof (OnMiddlePrayBuffAnimation));
    if (this._on_middle_animation == null)
      return;
    this._on_middle_animation();
  }

  public float GetCurrentZoneQuality() => this._cur_quality;
}
