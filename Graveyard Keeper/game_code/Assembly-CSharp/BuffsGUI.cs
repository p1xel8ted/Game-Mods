// Decompiled with JetBrains decompiler
// Type: BuffsGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BuffsGUI : BaseGUI
{
  public GameObject buffs_hud;
  public UIPanel buffs_hud_panel;
  public BuffIcon buff_icon_prefab;
  public UIGrid grid;
  [NonSerialized]
  public List<BuffIcon> _buffs = new List<BuffIcon>();

  public override void Init()
  {
    base.Init();
    this.buff_icon_prefab.gameObject.SetActive(false);
    this.gameObject.SetActive(true);
  }

  public void Redraw()
  {
    this.Clear();
    if (!MainGame.game_started)
      return;
    foreach (PlayerBuff buff in MainGame.me.save.buffs)
    {
      if (!buff.definition.is_hidden)
      {
        BuffIcon buffIcon = this.buff_icon_prefab.Copy<BuffIcon>();
        this._buffs.Add(buffIcon);
        buffIcon.Draw(buff);
      }
    }
    this.grid.repositionNow = true;
    this.grid.Reposition();
    if (!GUIElements.me.inventory.is_shown)
      return;
    GUIElements.me.inventory.RedrawBuffsAndPerks();
  }

  public void Clear()
  {
    foreach (Component buff in this._buffs)
      NGUITools.Destroy((UnityEngine.Object) buff.gameObject);
    this._buffs.Clear();
  }

  public override void Update()
  {
    base.Update();
    foreach (BuffIcon buff in this._buffs)
      buff.Redraw();
  }
}
