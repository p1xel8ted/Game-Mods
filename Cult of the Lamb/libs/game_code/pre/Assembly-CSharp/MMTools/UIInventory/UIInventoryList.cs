// Decompiled with JetBrains decompiler
// Type: MMTools.UIInventory.UIInventoryList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace MMTools.UIInventory;

public class UIInventoryList : BaseMonoBehaviour
{
  public List<UIInventoryItem> Items = new List<UIInventoryItem>();
  public Vector2Int GridSize = new Vector2Int(2, 2);
  public Vector2 Padding = new Vector2(100f, 50f);
  public UIInventoryItem ListObject;
  public Transform Container;

  public void PopulateList(List<InventoryItem> ItemsToPopulate)
  {
    int index = -1;
    while (++index < this.Items.Count)
    {
      if (index < ItemsToPopulate.Count)
        this.Items[index].Init(ItemsToPopulate[index]);
      else
        this.Items[index].InitEmpty();
    }
  }

  private void GenerateList()
  {
    this.ClearList();
    for (int index = 0; index < this.GridSize.y; ++index)
    {
      int num = 0;
      while (num < this.GridSize.x)
        ++num;
    }
  }

  private void ClearList()
  {
    foreach (UIInventoryItem uiInventoryItem in this.Items)
    {
      if ((Object) uiInventoryItem != (Object) null)
        Object.DestroyImmediate((Object) uiInventoryItem.gameObject);
    }
    this.Items = new List<UIInventoryItem>();
  }
}
