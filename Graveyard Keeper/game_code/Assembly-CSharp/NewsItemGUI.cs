// Decompiled with JetBrains decompiler
// Type: NewsItemGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class NewsItemGUI : MonoBehaviour
{
  [SerializeField]
  [HideInInspector]
  public SimpleUITable _table;
  [HideInInspector]
  [SerializeField]
  public UIWidget[] _widgets;
  public UILabel version;
  public UILabel version_upcoming;
  public UILabel eta;
  public UILabel items;
  public UILabel release_date;
  public UI2DSprite upcoming_progress;
  public GameObject upcoming_go;
  public GameObject version_go;

  public void Init()
  {
    this._table = this.GetComponent<SimpleUITable>();
    this._widgets = this.GetComponentsInChildren<UIWidget>(true);
  }

  public void Draw(NewsItemData data)
  {
    this.upcoming_go.SetActive(data.is_upcoming);
    this.version_go.SetActive(!data.is_upcoming);
    if (data.is_upcoming)
    {
      this.version_upcoming.text = data.version;
      this.upcoming_progress.fillAmount = (float) data.progress / 100f;
      int num = (data.date - DateTime.UtcNow).Days;
      if (num < 1)
        num = 1;
      this.eta.text = $"ETA: {num.ToString()} d";
    }
    else
    {
      this.version.text = data.version;
      this.release_date.text = data.date.ToString("dd MMM yyyy");
    }
    this.items.text = data.items;
    this._table.Reposition();
    foreach (UIWidget widget in this._widgets)
    {
      widget.Update();
      widget.UpdateAnchors();
    }
  }
}
