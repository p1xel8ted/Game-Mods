// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Assets.FleeceIconMapping
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private List<FleeceIconMapping.ItemSpritePair> _itemImages;
  private Dictionary<int, Sprite> _itemMap;

  private void Initialise()
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
