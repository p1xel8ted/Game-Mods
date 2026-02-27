// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Assets.InventoryIconMapping
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.Assets;

[CreateAssetMenu(fileName = "Inventory Icon Mapping", menuName = "Massive Monster/Inventory Icon Mapping", order = 1)]
public class InventoryIconMapping : ScriptableObject
{
  [SerializeField]
  private List<InventoryIconMapping.ItemSpritePair> _itemImages;
  private Dictionary<InventoryItem.ITEM_TYPE, Sprite> _itemMap;

  private void Initialise()
  {
    this._itemMap = new Dictionary<InventoryItem.ITEM_TYPE, Sprite>();
    foreach (InventoryIconMapping.ItemSpritePair itemImage in this._itemImages)
      this._itemMap.Add(itemImage.ItemType, itemImage.Sprite);
  }

  public Sprite GetImage(InventoryItem.ITEM_TYPE type)
  {
    if (type == InventoryItem.ITEM_TYPE.NONE)
      return (Sprite) null;
    if (this._itemMap == null || this._itemImages.Count != this._itemMap.Keys.Count)
      this.Initialise();
    Sprite sprite;
    return this._itemMap.TryGetValue(type, out sprite) ? sprite : (Sprite) null;
  }

  public void GetImage(InventoryItem.ITEM_TYPE type, Image image)
  {
    Sprite image1 = this.GetImage(type);
    image.sprite = image1;
    if (!((UnityEngine.Object) image1 == (UnityEngine.Object) null))
      return;
    image.material = (Material) null;
    image.color = Color.magenta;
  }

  [Serializable]
  public class ItemSpritePair
  {
    public InventoryItem.ITEM_TYPE ItemType;
    public Sprite Sprite;
  }
}
