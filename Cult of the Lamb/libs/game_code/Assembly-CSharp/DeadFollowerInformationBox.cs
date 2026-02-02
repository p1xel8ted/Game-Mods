// Decompiled with JetBrains decompiler
// Type: DeadFollowerInformationBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Spine.Unity;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class DeadFollowerInformationBox : FollowerSelectItem, IPoolListener
{
  [SerializeField]
  public RectTransform _followerContainer;
  [SerializeField]
  public SkeletonGraphic _followerSpine;
  [SerializeField]
  public TMP_Text _followerName;
  [SerializeField]
  public TMP_Text _ageHeaderText;
  [SerializeField]
  public TMP_Text _causeOfDeathText;
  [SerializeField]
  public TMP_Text _timeOfDeathText;
  [SerializeField]
  public TMP_Text _ageText;

  public override void ConfigureImpl()
  {
    this._followerName.text = this._followerInfo.GetNameFormatted();
    this._ageHeaderText.text = ScriptLocalization.UI_FollowerInfo.Age;
    this._ageHeaderText.text = this._ageHeaderText.text.Replace(":", string.Empty);
    this._ageHeaderText.text = string.Format(this._ageHeaderText.text, (object) string.Empty);
    this._causeOfDeathText.text = this.FollowerInfo.GetDeathText();
    if (LocalizeIntegration.IsArabic())
    {
      this._timeOfDeathText.isRightToLeftText = true;
      this._timeOfDeathText.text = string.Format(ScriptLocalization.UI.DayNumber, (object) LocalizeIntegration.ReverseText(this._followerInfo.TimeOfDeath.ToString()));
    }
    else
      this._timeOfDeathText.text = string.Format(ScriptLocalization.UI.DayNumber, (object) this._followerInfo.TimeOfDeath);
    this._ageText.text = this._followerInfo.Age.ToString();
    this._followerSpine.ConfigureFollower(this._followerInfo);
    this._followerSpine.SetFaceAnimation("dead", true);
    this._followerSpine.SetAnimation("dead");
    this._button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
  }

  public void OnButtonClicked()
  {
    Action<FollowerInfo> followerSelected = this.OnFollowerSelected;
    if (followerSelected == null)
      return;
    followerSelected(this.FollowerInfo);
  }

  public void OnRecycled()
  {
    this._button.onClick.RemoveListener(new UnityAction(this.OnButtonClicked));
  }
}
