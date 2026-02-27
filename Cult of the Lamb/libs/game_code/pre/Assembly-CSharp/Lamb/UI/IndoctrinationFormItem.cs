// Decompiled with JetBrains decompiler
// Type: Lamb.UI.IndoctrinationFormItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private CharacterSkinAlert _alert;

  public override void Configure(WorshipperData.SkinAndData skinAndData)
  {
    base.Configure(skinAndData);
    this._alert.Configure(skinAndData.Skin[0].Skin);
  }

  protected override void OnButtonClickedImpl()
  {
    Action<IndoctrinationFormItem> onItemSelected = this.OnItemSelected;
    if (onItemSelected == null)
      return;
    onItemSelected(this);
  }

  public void OnSelect(BaseEventData eventData) => this._alert.TryRemoveAlert();
}
