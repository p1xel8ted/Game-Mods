// Decompiled with JetBrains decompiler
// Type: RanchSelectItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.RanchSelect;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public abstract class RanchSelectItem : BaseMonoBehaviour
{
  public Action<RanchSelectEntry> OnFollowerSelected;
  public Action<RanchSelectEntry> OnFollowerHighlighted;
  [SerializeField]
  public MMButton _button;
  public StructuresData.Ranchable_Animal _animalInfo;
  [CompilerGenerated]
  public RanchSelectEntry \u003CRanchSelectEntry\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CShowNecklaceReward\u003Ek__BackingField;

  public MMButton Button => this._button;

  public StructuresData.Ranchable_Animal AnimalInfo => this._animalInfo;

  public RanchSelectEntry RanchSelectEntry
  {
    set => this.\u003CRanchSelectEntry\u003Ek__BackingField = value;
    get => this.\u003CRanchSelectEntry\u003Ek__BackingField;
  }

  public bool ShowNecklaceReward
  {
    get => this.\u003CShowNecklaceReward\u003Ek__BackingField;
    set => this.\u003CShowNecklaceReward\u003Ek__BackingField = value;
  }

  public void Configure(RanchSelectEntry ranchSelectEntry, bool selectable)
  {
    this._animalInfo = ranchSelectEntry.AnimalInfo;
    this.RanchSelectEntry = ranchSelectEntry;
    this.ShowNecklaceReward = ranchSelectEntry.ShowNecklaceReward;
    this._button.Confirmable = selectable;
    this.ConfigureImpl();
  }

  public abstract void ConfigureImpl();
}
