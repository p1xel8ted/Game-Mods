// Decompiled with JetBrains decompiler
// Type: src.UI.Menus.PrisonInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using Spine.Unity;
using TMPro;
using UnityEngine;

#nullable disable
namespace src.UI.Menus;

public class PrisonInfoCard : UIInfoCardBase<FollowerInfo>
{
  [Header("Prison Info Card")]
  [SerializeField]
  public TextMeshProUGUI _followerNameText;
  [SerializeField]
  public SkeletonGraphic _followerSpine;
  [SerializeField]
  public RectTransform _redOutline;
  public FollowerInfo _followerInfo;

  public SkeletonGraphic FollowerSpine => this._followerSpine;

  public RectTransform RedOutline => this._redOutline;

  public override void Configure(FollowerInfo config)
  {
    if (this._followerInfo == config)
      return;
    this._followerInfo = config;
    this._followerNameText.text = config.Name;
    this._followerSpine.ConfigureFollower(config);
  }
}
