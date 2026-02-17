// Decompiled with JetBrains decompiler
// Type: Lamb.UI.IndoctrinationFormItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.Alerts;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
namespace Lamb.UI;

public class IndoctrinationFormItem : 
  IndoctrinationCharacterItem<IndoctrinationFormItem>,
  ISelectHandler,
  IEventSystemHandler
{
  [SerializeField]
  public CharacterSkinAlert _alert;
  public string followerSkin;

  public override void Configure(WorshipperData.SkinAndData skinAndData)
  {
    base.Configure(skinAndData);
    this._alert.Configure(skinAndData.Skin[0].Skin);
    this.followerSkin = skinAndData.Skin[0].Skin;
  }

  public override void OnButtonClickedImpl()
  {
    Action<IndoctrinationFormItem> onItemSelected = this.OnItemSelected;
    if (onItemSelected == null)
      return;
    onItemSelected(this);
  }

  public void OnSelect(BaseEventData eventData) => this._alert.TryRemoveAlert();

  public override void OnButtonHighlightedImpl()
  {
    Action<IndoctrinationFormItem> onItemHighlighted = this.OnItemHighlighted;
    if (onItemHighlighted == null)
      return;
    onItemHighlighted(this);
  }
}
