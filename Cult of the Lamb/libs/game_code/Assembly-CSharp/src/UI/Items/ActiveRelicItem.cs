// Decompiled with JetBrains decompiler
// Type: src.UI.Items.ActiveRelicItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace src.UI.Items;

public class ActiveRelicItem : MonoBehaviour
{
  [SerializeField]
  public Image _relicImage;
  [SerializeField]
  public MMSelectable _selectable;
  [CompilerGenerated]
  public RelicData \u003CRelicData\u003Ek__BackingField;

  public RelicData RelicData
  {
    set => this.\u003CRelicData\u003Ek__BackingField = value;
    get => this.\u003CRelicData\u003Ek__BackingField;
  }

  public MMSelectable Selectable => this._selectable;

  public void Configure(RelicData relicData)
  {
    this.RelicData = relicData;
    this._relicImage.sprite = relicData.Sprite;
  }
}
