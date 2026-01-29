// Decompiled with JetBrains decompiler
// Type: Lamb.UI.IndoctrinationColourItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class IndoctrinationColourItem : IndoctrinationCharacterItem<IndoctrinationColourItem>
{
  [SerializeField]
  public SkeletonGraphic clothingSpine;
  [CompilerGenerated]
  public FollowerClothingType \u003CClothingType\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CClothingColour\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CClothingVariant\u003Ek__BackingField;

  public FollowerClothingType ClothingType
  {
    get => this.\u003CClothingType\u003Ek__BackingField;
    set => this.\u003CClothingType\u003Ek__BackingField = value;
  }

  public int ClothingColour
  {
    get => this.\u003CClothingColour\u003Ek__BackingField;
    set => this.\u003CClothingColour\u003Ek__BackingField = value;
  }

  public string ClothingVariant
  {
    get => this.\u003CClothingVariant\u003Ek__BackingField;
    set => this.\u003CClothingVariant\u003Ek__BackingField = value;
  }

  public void Configure(int skin, int colour, WorshipperData.SkinAndData skinAndData)
  {
    this._skinAndData = skinAndData;
    this.Configure(skin, colour);
  }

  public void Configure(ClothingData clothingData, int color, string variant)
  {
    this.clothingSpine.ConfigureFollowerOutfit(clothingData, color, variant);
    this.ClothingColour = color;
    this.ClothingVariant = variant;
    this.ClothingType = clothingData.ClothingType;
    this._spine.gameObject.SetActive(false);
  }

  public void Configure(int skin, int colour)
  {
    this._spine.ConfigureFollowerSkin(this._skinAndData, skin, colour);
    this.clothingSpine.gameObject.SetActive(false);
  }

  public override void OnButtonClickedImpl()
  {
    Action<IndoctrinationColourItem> onItemSelected = this.OnItemSelected;
    if (onItemSelected == null)
      return;
    onItemSelected(this);
  }

  public override void OnButtonHighlightedImpl()
  {
    Action<IndoctrinationColourItem> onItemHighlighted = this.OnItemHighlighted;
    if (onItemHighlighted == null)
      return;
    onItemHighlighted(this);
  }
}
