// Decompiled with JetBrains decompiler
// Type: TechTreeGUIUnlockItem
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class TechTreeGUIUnlockItem : MonoBehaviour
{
  public UI2DSprite spr;
  public UI2DSprite item_quality;
  public UILabel label_name;
  public UILabel label_description;
  public UIWidget background;

  public void Draw(TechUnlock unlock, bool init_tooltip)
  {
    this.gameObject.SetActive(unlock != null);
    if (unlock == null)
      return;
    this.Draw(unlock.GetData(), init_tooltip ? unlock : (TechUnlock) null);
  }

  public void Draw(TechUnlock.TechUnlockData unlock_data, TechUnlock tooltip_data = null)
  {
    this.gameObject.SetActive(unlock_data != null);
    if (unlock_data == null)
      return;
    this.spr.sprite2D = unlock_data.sprite;
    if ((UnityEngine.Object) this.label_name != (UnityEngine.Object) null)
      this.label_name.text = unlock_data.name;
    if ((UnityEngine.Object) this.label_description != (UnityEngine.Object) null)
    {
      this.label_description.text = unlock_data.description;
      this.label_description.gameObject.SetActive(!string.IsNullOrEmpty(this.label_description.text));
      this.label_description.ProcessText();
      if ((UnityEngine.Object) this.background != (UnityEngine.Object) null)
        this.background.height = Math.Max(46, 28 + (int) this.label_description.localSize.y);
    }
    if ((UnityEngine.Object) this.item_quality != (UnityEngine.Object) null)
    {
      this.item_quality.sprite2D = unlock_data.quality_icon;
      this.item_quality.SetActive((UnityEngine.Object) this.item_quality.sprite2D != (UnityEngine.Object) null);
    }
    if (tooltip_data != null)
    {
      Tooltip component = this.GetComponent<Tooltip>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        component.ClearData();
        tooltip_data.GetTooltip(component);
      }
    }
    else
    {
      BoxCollider2D component = this.GetComponent<BoxCollider2D>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.enabled = false;
    }
    SimpleUITable componentInChildren = this.GetComponentInChildren<SimpleUITable>();
    if (!((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null))
      return;
    componentInChildren.Reposition();
  }
}
