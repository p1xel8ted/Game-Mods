// Decompiled with JetBrains decompiler
// Type: FishingDistanceChoosingBarGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FishingDistanceChoosingBarGUI : MonoBehaviour
{
  public GameObject backs_gfx;
  public GameObject marker;
  public FishingDistanceChoosingBarSelectionGUI selection_left;
  public FishingDistanceChoosingBarSelectionGUI selection_center;
  public FishingDistanceChoosingBarSelectionGUI selection_right;
  public FishingDistanceChoosingBarItemGUI fish_item_prefab;
  public bool _is_to_right;
  public float _bar_length;
  public bool _marker_is_active;
  public int _prev_zone = -1;

  public void Init()
  {
    this._is_to_right = MainGame.me.gui_elements.fishing.is_to_right;
    this._bar_length = (float) (this.GetComponent<UIWidget>().width - 30);
    string id = MainGame.me.gui_elements.fishing.reservoir_data.id;
    for (int index = 0; index < this.backs_gfx.transform.childCount; ++index)
    {
      Transform child = this.backs_gfx.transform.GetChild(index);
      child.gameObject.SetActive(child.gameObject.name == id);
    }
    this.fish_item_prefab.gameObject.SetActive(true);
    this.selection_left.Init(this._is_to_right ? 0 : 2);
    this.selection_center.Init(1);
    this.selection_right.Init(this._is_to_right ? 2 : 0);
    this.fish_item_prefab.gameObject.SetActive(false);
    this._marker_is_active = true;
    this._prev_zone = -1;
    this.SetMarkerActive(false);
  }

  public void SetMarkerPos(float pos)
  {
    if (this._is_to_right)
      pos -= 0.5f;
    else
      pos = 0.5f - pos;
    int num = Mathf.CeilToInt((float) (((double) pos + 0.5) * 3.0));
    if (num < 1)
      num = 1;
    else if (num > 3)
      num = 3;
    pos *= this._bar_length;
    Transform transform = this.marker.transform;
    transform.localPosition = new Vector3(pos, transform.localPosition.y, 0.0f);
    this.selection_left.SetSelected(num == 1);
    this.selection_center.SetSelected(num == 2);
    this.selection_right.SetSelected(num == 3);
    if (num == this._prev_zone)
      return;
    this._prev_zone = num;
    Sounds.OnGUITabClick();
  }

  public void UpdateBar()
  {
    this.selection_left.UpdateFishItems();
    this.selection_center.UpdateFishItems();
    this.selection_right.UpdateFishItems();
  }

  public void SetMarkerActive(bool set_active)
  {
    if (this._marker_is_active == set_active)
      return;
    this._marker_is_active = set_active;
    this.marker.SetActive(this._marker_is_active);
  }
}
