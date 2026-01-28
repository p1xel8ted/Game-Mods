// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Assets.FleeceIconMapping
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.Assets;

[CreateAssetMenu(fileName = "Fleece Icon Mapping", menuName = "Massive Monster/Fleece Icon Mapping", order = 1)]
public class FleeceIconMapping : ScriptableObject
{
  [SerializeField]
  public List<FleeceIconMapping.ItemSpritePair> _itemImages;
  public Dictionary<int, Sprite> _itemMap;

  public void Initialise()
  {
    this._itemMap = new Dictionary<int, Sprite>();
    foreach (FleeceIconMapping.ItemSpritePair itemImage in this._itemImages)
      this._itemMap.Add(itemImage.FleeceIndex, itemImage.Sprite);
  }

  public Sprite GetImage(int index)
  {
    if (this._itemMap == null || this._itemImages.Count != this._itemMap.Keys.Count)
      this.Initialise();
    Sprite sprite;
    return this._itemMap.TryGetValue(index, out sprite) ? sprite : (Sprite) null;
  }

  public void GetImage(int index, Image image)
  {
    Sprite image1 = this.GetImage(index);
    image.sprite = image1;
    if (!((UnityEngine.Object) image1 == (UnityEngine.Object) null))
      return;
    image.material = (Material) null;
    image.color = Color.magenta;
  }

  [Serializable]
  public class ItemSpritePair
  {
    public int FleeceIndex;
    public Sprite Sprite;
  }
}
