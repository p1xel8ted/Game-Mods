// Decompiled with JetBrains decompiler
// Type: CraftItemPerkGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CraftItemPerkGUI : MonoBehaviour
{
  public UI2DSprite icon;
  public Tooltip tooltip;

  public void DrawPerk(string perk_id)
  {
    bool flag = !string.IsNullOrEmpty(perk_id);
    this.gameObject.SetActive(flag);
    if (!flag)
      return;
    PerkDefinition data = GameBalance.me.GetData<PerkDefinition>(perk_id);
    this.icon.sprite2D = EasySpritesCollection.GetSprite(data.GetIcon());
    this.icon.alpha = MainGame.me.save.unlocked_perks.Contains(perk_id) ? 1f : 0.25f;
    this.icon.MakePixelPerfect();
    this.tooltip.SetData((BubbleWidgetData) new BubbleWidgetTextData(GJL.L(perk_id), UITextStyles.TextStyle.HintTitle, NGUIText.Alignment.Left));
    string descriptionIfExists = data.GetDescriptionIfExists();
    if (string.IsNullOrEmpty(descriptionIfExists))
      return;
    this.tooltip.AddData((BubbleWidgetData) new BubbleWidgetTextData(descriptionIfExists, UITextStyles.TextStyle.TinyDescription, NGUIText.Alignment.Left));
  }

  public void DrawBuff(string buff_id)
  {
    BuffDefinition data = GameBalance.me.GetData<BuffDefinition>(buff_id);
    if (data == null)
    {
      Debug.LogError((object) ("Couldn't draw a null buff: " + buff_id));
    }
    else
    {
      this.gameObject.SetActive(true);
      this.icon.sprite2D = EasySpritesCollection.GetSprite(data.GetIconName());
      this.icon.alpha = 1f;
      this.icon.MakePixelPerfect();
      this.tooltip.SetData((BubbleWidgetData) new BubbleWidgetTextData(data.GetLocalizedName(), UITextStyles.TextStyle.HintTitle, NGUIText.Alignment.Left));
      string descriptionIfExists = data.GetDescriptionIfExists();
      if (string.IsNullOrEmpty(descriptionIfExists))
        return;
      this.tooltip.AddData((BubbleWidgetData) new BubbleWidgetTextData(descriptionIfExists, UITextStyles.TextStyle.TinyDescription, NGUIText.Alignment.Left));
    }
  }
}
