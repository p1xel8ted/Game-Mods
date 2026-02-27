// Decompiled with JetBrains decompiler
// Type: SermonWheelCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class SermonWheelCategory : UIRadialWheelItem
{
  [SerializeField]
  private SermonCategoryTextIcon _textIcon;
  [SerializeField]
  private GameObject _progressBar;
  [SerializeField]
  private Image _progressBarFill;
  [SerializeField]
  private Image _topCircle;
  [SerializeField]
  private TextMeshProUGUI _progress;
  private bool _isLocked;
  private bool _isHidden;
  private string _title;
  private string _description;

  public SermonCategory SermonCategory => this._textIcon.SermonCategory;

  private void Start()
  {
    this._description = DoctrineUpgradeSystem.GetSermonCategoryLocalizedDescription(this.SermonCategory);
    if (!this.IsValidOption())
    {
      this._progressBar.SetActive(false);
      this._title = $"{DoctrineUpgradeSystem.GetSermonCategoryLocalizedName(this.SermonCategory)} - {ScriptLocalization.UI_Generic.Max.Colour(StaticColors.RedColor)}";
      this._canvasGroup.alpha = 0.75f;
    }
    else
      this._title = $"{DoctrineUpgradeSystem.GetSermonCategoryLocalizedName(this.SermonCategory)} - {(DoctrineUpgradeSystem.GetLevelBySermon(this.SermonCategory) + 1).ToNumeral()}";
    this._progress.text = $"{(object) DoctrineUpgradeSystem.GetLevelBySermon(this.SermonCategory)}/{(object) 4}";
  }

  private void SetAsLocked()
  {
    this._textIcon.SetLock();
    this._progressBar.SetActive(false);
    this._isLocked = true;
  }

  private void SetAsHidden()
  {
    this._isHidden = true;
    this._textIcon.SetHidden();
    this._progressBar.SetActive(false);
    this._button.interactable = false;
    this._button.enabled = false;
    this._topCircle.enabled = false;
  }

  public override string GetTitle() => this._title;

  public override bool IsValidOption()
  {
    return DoctrineUpgradeSystem.GetLevelBySermon(this.SermonCategory) < 4 && !this._isLocked;
  }

  public override bool Visible() => !this._isHidden;

  public override string GetDescription() => this._description;
}
