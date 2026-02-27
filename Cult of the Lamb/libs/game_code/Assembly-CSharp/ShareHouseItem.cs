// Decompiled with JetBrains decompiler
// Type: ShareHouseItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class ShareHouseItem : FollowerInformationBox
{
  public Action<ShareHouseItem> OnShareHouseItemSelected;
  [Header("Share House Item")]
  [SerializeField]
  public GameObject _vacancyContainer;
  [SerializeField]
  public GameObject _contentContainer;

  public void Awake() => this.Button.OnSelected += new System.Action(this.OnSelfSelected);

  public void ConfigureVacant() => this.ConfigureImpl();

  public override void ConfigureImpl()
  {
    if (this._followerInfo == null)
    {
      this._vacancyContainer.SetActive(true);
      this._contentContainer.SetActive(false);
    }
    else
    {
      this._vacancyContainer.SetActive(false);
      this._contentContainer.SetActive(true);
      base.ConfigureImpl();
    }
  }

  public void OnSelfSelected()
  {
    Action<ShareHouseItem> houseItemSelected = this.OnShareHouseItemSelected;
    if (houseItemSelected == null)
      return;
    houseItemSelected(this);
  }
}
