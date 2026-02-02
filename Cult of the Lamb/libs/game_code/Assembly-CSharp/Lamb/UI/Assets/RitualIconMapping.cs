// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Assets.RitualIconMapping
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.Assets;

[CreateAssetMenu(fileName = "Ritual Icon Mapping", menuName = "Massive Monster/Ritual Icon Mapping", order = 1)]
public class RitualIconMapping : ScriptableObject
{
  [SerializeField]
  public List<RitualIconMapping.RitualSprite> _items;
  public Dictionary<UpgradeSystem.Type, Sprite> _itemMap;

  public void Initialise()
  {
    this._itemMap = new Dictionary<UpgradeSystem.Type, Sprite>();
    foreach (RitualIconMapping.RitualSprite ritualSprite in this._items)
      this._itemMap.Add(ritualSprite.RitualType, ritualSprite.Sprite);
  }

  public Sprite GetImage(UpgradeSystem.Type type)
  {
    if (type == UpgradeSystem.Type.Count)
      return (Sprite) null;
    if (this._itemMap == null || this._items.Count != this._itemMap.Keys.Count)
      this.Initialise();
    Sprite sprite;
    return this._itemMap.TryGetValue(type, out sprite) ? sprite : (Sprite) null;
  }

  public void GetImage(UpgradeSystem.Type type, Image image)
  {
    Sprite image1 = this.GetImage(type);
    image.sprite = image1;
    if (!((UnityEngine.Object) image1 == (UnityEngine.Object) null))
      return;
    image.material = (Material) null;
    image.color = Color.magenta;
  }

  [Serializable]
  public class RitualSprite
  {
    public UpgradeSystem.Type RitualType;
    public Sprite Sprite;
  }
}
