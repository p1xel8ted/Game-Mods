// Decompiled with JetBrains decompiler
// Type: BubbleWidgetAlchemyItem
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BubbleWidgetAlchemyItem : BubbleWidget<BubbleWidgetAlchemyItemData>
{
  public UILabel lbl_header;
  public UILabel lbl_info;
  public UILabel lbl_info_l;
  public UILabel lbl_info_r;
  public Color clr_normal;
  public Color clr_not_complete;

  public override void Init() => base.Init();

  public override void Draw(BubbleWidgetAlchemyItemData data)
  {
    if (!this.initialized)
      this.Init();
    this.data = data;
    if (string.IsNullOrEmpty(data.item_id))
    {
      this.Deactivate<BubbleWidgetAlchemyItem>();
    }
    else
    {
      bool flag = false;
      if ((UnityEngine.Object) MainGame.me != (UnityEngine.Object) null && MainGame.me.save != null)
        flag = MainGame.me.save.IsSurveyComplete(CraftDefinition.CraftSubType.Alchemy, data.item_id);
      if (!Application.isPlaying && UnityEngine.Random.Range(0, 100) > 50)
        flag = true;
      this.lbl_header.color = this.lbl_info.color = flag ? this.clr_normal : this.clr_not_complete;
      if (!flag)
      {
        this.lbl_header.text = $"{GJL.L("alch_survey_hdr")}{GJL.L(":")}\n{GJL.L("alch_not_complete")}";
      }
      else
      {
        this.lbl_info.text = "";
        ItemDefinition data1 = GameBalance.me.GetData<ItemDefinition>(data.item_id);
        if (data1 == null)
          return;
        ItemDefinition.ItemDetails itemDetails = data1.GetItemDetails();
        if (itemDetails.alchemy == null)
          return;
        switch (itemDetails.alchemy.details_type)
        {
          case ItemDefinition.ItemDetailsAlchemy.DetailsType.Decompose:
            this.DrawDecomposeInfo(itemDetails);
            break;
          case ItemDefinition.ItemDetailsAlchemy.DetailsType.Slots:
            this.DrawSlotsInfo(itemDetails);
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
        this.ui_widget.Update();
      }
    }
  }

  public void DrawDecomposeInfo(ItemDefinition.ItemDetails details)
  {
    this.lbl_header.text = $"{GJL.L("alch_decompose")}{GJL.L(":")} ";
    string str = "";
    foreach (int decompose in details.alchemy.decomposes)
      str = $"{str}(alc{decompose.ToString()})";
    this.lbl_header.text += string.IsNullOrEmpty(str) ? "-" : str;
  }

  public void DrawSlotsInfo(ItemDefinition.ItemDetails details)
  {
    this.lbl_header.text = GJL.L("alch_slots") + GJL.L(":");
    string str1 = "";
    string str2 = "";
    int n = 0;
    foreach (List<bool> slot in details.alchemy.slots)
    {
      ++n;
      if (n > 1)
      {
        str1 += "\n";
        str2 += "\n";
        this.lbl_info.text += "\n";
      }
      str2 = $"{str2}{GJL.L("alc_d_tier")} {GJCommons.GetRomeNumber(n)}";
      foreach (bool flag in slot)
        str1 += flag ? "(alc+)" : "(alc-)";
    }
    this.lbl_info_l.text = str2;
    this.lbl_info_r.text = str1;
    this.lbl_info_l.color = this.lbl_info_r.color = this.clr_normal;
  }
}
