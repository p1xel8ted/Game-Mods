// Decompiled with JetBrains decompiler
// Type: FollowerSelectItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.FollowerSelect;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public abstract class FollowerSelectItem : BaseMonoBehaviour
{
  public Action<FollowerInfo> OnFollowerSelected;
  public Action<FollowerInfo> OnFollowerHighlighted;
  [SerializeField]
  public MMButton _button;
  public FollowerInfo _followerInfo;
  [CompilerGenerated]
  public FollowerSelectEntry \u003CFollowerSelectEntry\u003Ek__BackingField;

  public MMButton Button => this._button;

  public FollowerInfo FollowerInfo => this._followerInfo;

  public FollowerSelectEntry FollowerSelectEntry
  {
    set => this.\u003CFollowerSelectEntry\u003Ek__BackingField = value;
    get => this.\u003CFollowerSelectEntry\u003Ek__BackingField;
  }

  public void Configure(FollowerInfo followerInfo)
  {
    this._followerInfo = followerInfo;
    this.ConfigureImpl();
  }

  public void Configure(FollowerSelectEntry followerSelectEntry)
  {
    this._followerInfo = followerSelectEntry.FollowerInfo;
    this.FollowerSelectEntry = followerSelectEntry;
    this._button.Confirmable = this.FollowerSelectEntry.AvailabilityStatus == FollowerSelectEntry.Status.Available;
    this.ConfigureImpl();
  }

  public abstract void ConfigureImpl();
}
