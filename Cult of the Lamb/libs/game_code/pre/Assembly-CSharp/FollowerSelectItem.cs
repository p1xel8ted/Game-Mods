// Decompiled with JetBrains decompiler
// Type: FollowerSelectItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public abstract class FollowerSelectItem : BaseMonoBehaviour
{
  public Action<FollowerInfo> OnFollowerSelected;
  [SerializeField]
  protected MMButton _button;
  protected FollowerInfo _followerInfo;

  public MMButton Button => this._button;

  public FollowerInfo FollowerInfo => this._followerInfo;

  public void Configure(FollowerInfo followerInfo)
  {
    this._followerInfo = followerInfo;
    this.ConfigureImpl();
  }

  protected abstract void ConfigureImpl();
}
