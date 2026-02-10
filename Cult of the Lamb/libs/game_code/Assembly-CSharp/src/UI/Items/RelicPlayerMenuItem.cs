// Decompiled with JetBrains decompiler
// Type: src.UI.Items.RelicPlayerMenuItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace src.UI.Items;

public class RelicPlayerMenuItem : PlayerMenuItem<RelicData>
{
  [SerializeField]
  public Image _icon;
  [SerializeField]
  public GameObject _lockedContainer;
  [CompilerGenerated]
  public RelicData \u003CData\u003Ek__BackingField;

  public RelicData Data
  {
    set => this.\u003CData\u003Ek__BackingField = value;
    get => this.\u003CData\u003Ek__BackingField;
  }

  public override void Configure(RelicData data)
  {
    this.Data = data;
    this._icon.gameObject.SetActive((Object) this.Data != (Object) null);
    this._lockedContainer.SetActive((Object) this.Data == (Object) null);
    if (!((Object) this.Data != (Object) null))
      return;
    this._icon.sprite = this.Data.Sprite;
  }
}
