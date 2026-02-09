// Decompiled with JetBrains decompiler
// Type: FishingDistanceChoosingBarSelectionGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Fishing;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FishingDistanceChoosingBarSelectionGUI : MonoBehaviour
{
  public GameObject selection_frame;
  public UIGrid grid;
  public FishingDistanceChoosingBarGUI father;
  public bool _is_selected;
  public int _distance;
  public List<FishingDistanceChoosingBarItemGUI> _fish_items = new List<FishingDistanceChoosingBarItemGUI>();

  public void Init(int dist)
  {
    this._distance = dist;
    this._is_selected = true;
    this.SetSelected(false);
    this.father = MainGame.me.gui_elements.fishing.distance_choosing_bar;
    this.UpdateFishItems(true);
  }

  public void SetSelected(bool selected = true)
  {
    if (this._is_selected == selected)
      return;
    this._is_selected = selected;
    this.selection_frame.SetActive(this._is_selected);
  }

  public void CreateNewFishItem()
  {
    if (this._fish_items == null)
      this._fish_items = new List<FishingDistanceChoosingBarItemGUI>();
    FishingDistanceChoosingBarItemGUI choosingBarItemGui = Object.Instantiate<FishingDistanceChoosingBarItemGUI>(this.father.fish_item_prefab, this.grid.transform);
    choosingBarItemGui.gameObject.SetActive(true);
    this._fish_items.Add(choosingBarItemGui);
  }

  public void RemoveFishItem()
  {
    if (this._fish_items.Count == 0)
      return;
    Object.Destroy((Object) this._fish_items[this._fish_items.Count - 1].gameObject);
    this._fish_items.RemoveAt(this._fish_items.Count - 1);
  }

  public void UpdateFishItems(bool need_reposition = false)
  {
    List<FishWithWeight> fishesWithWeight = MainGame.me.gui_elements.fishing.fishes_with_weights[this._distance];
    float num1 = 0.0f;
    foreach (FishWithWeight fishWithWeight in fishesWithWeight)
      num1 += fishWithWeight.weight;
    List<string> stringList1 = new List<string>();
    List<string> stringList2 = new List<string>();
    List<float> floatList = new List<float>();
    foreach (FishWithWeight fishWithWeight in fishesWithWeight)
    {
      string withoutQualitySuffix = ItemDefinition.StaticGetNameWithoutQualitySuffix(fishWithWeight.fish.item_id);
      int index = stringList1.IndexOf(withoutQualitySuffix);
      if (index >= 0)
      {
        floatList[index] += fishWithWeight.weight;
      }
      else
      {
        string str1 = "i_unknown_fish";
        string str2 = $"{GUIElements.me.fishing.reservoir_data.id}:{(this._distance + 1).ToString()}:{ItemDefinition.StaticGetNameWithoutQualitySuffix(fishWithWeight.fish.item_id)}";
        if (MainGame.me.save.known_fishes.Contains(str2))
        {
          foreach (ItemDefinition itemDefinition in GameBalance.me.items_data)
          {
            if (itemDefinition.id == fishWithWeight.fish.item_id)
            {
              str1 = itemDefinition.GetIcon();
              break;
            }
          }
        }
        stringList1.Add(withoutQualitySuffix);
        stringList2.Add(str1);
        floatList.Add(fishWithWeight.weight);
      }
    }
    if (stringList1.Count > 3)
    {
      string str3 = string.Empty;
      foreach (string str4 in stringList1)
        str3 = str3 + (string.IsNullOrEmpty(str3) ? "" : ", ") + str4;
      string[] strArray = new string[8];
      strArray[0] = "Wrong fishes count at ";
      strArray[1] = GUIElements.me.fishing.reservoir_data.id;
      strArray[2] = ":";
      int num2 = this._distance + 1;
      strArray[3] = num2.ToString();
      strArray[4] = " == ";
      num2 = stringList1.Count;
      strArray[5] = num2.ToString();
      strArray[6] = ":\n";
      strArray[7] = str3;
      Debug.LogError((object) string.Concat(strArray));
    }
    while (stringList1.Count > this._fish_items.Count && this._fish_items.Count != 3)
    {
      this.CreateNewFishItem();
      need_reposition = true;
    }
    while (stringList1.Count < this._fish_items.Count)
    {
      this.RemoveFishItem();
      need_reposition = true;
    }
    for (int index = 0; index < this._fish_items.Count && index != stringList1.Count; ++index)
      this._fish_items[index].SetFishItem(stringList2[index], (double) num1 > 0.0 ? Mathf.RoundToInt(floatList[index] * 100f / num1) : 0);
    if (!need_reposition)
      return;
    this.grid.Reposition();
    this.grid.repositionNow = true;
  }
}
