// Decompiled with JetBrains decompiler
// Type: SoulContainerGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SoulContainerGUI : BaseGUI
{
  [SerializeField]
  public UniversalObjectInfoGUI _universal_object_info;
  [SerializeField]
  public UIGrid _grid;
  [SerializeField]
  public SoulContainerWidget _soul_container_widget;
  public List<SoulContainerWidget> _cached_widgets;

  public override void Init()
  {
    this._cached_widgets = new List<SoulContainerWidget>();
    this._soul_container_widget.SetActive(false);
    base.Init();
  }

  public void Open(WorldGameObject wgo)
  {
    this.Open();
    int inventorySize = wgo.obj_def.inventory_size;
    int count = this._cached_widgets.Count;
    for (int index = 0; index < inventorySize; ++index)
    {
      if (count > 0)
      {
        this._cached_widgets[index].Draw(index, wgo);
        this._cached_widgets[index].SetActive(true);
        --count;
      }
      else
      {
        SoulContainerWidget soulContainerWidget = this._soul_container_widget.Copy<SoulContainerWidget>();
        this._cached_widgets.Add(soulContainerWidget);
        soulContainerWidget.Draw(index, wgo);
      }
    }
    this._universal_object_info.Draw(wgo.GetUniversalObjectInfo());
    this._grid.Reposition();
  }

  public void Redraw(WorldGameObject wgo)
  {
    int ordinal_number = 0;
    foreach (SoulContainerWidget cachedWidget in this._cached_widgets)
    {
      if (cachedWidget.gameObject.activeSelf)
      {
        cachedWidget.Draw(ordinal_number, wgo);
        ++ordinal_number;
      }
    }
  }

  public override void Hide(bool play_hide_sound = true)
  {
    for (int index = 0; index < this._cached_widgets.Count; ++index)
      this._cached_widgets[index].SetActive(false);
    base.Hide(play_hide_sound);
  }

  public override bool OnPressedBack()
  {
    this.OnClosePressed();
    return true;
  }
}
