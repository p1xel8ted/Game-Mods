// Decompiled with JetBrains decompiler
// Type: src.UI.Overlays.EventOverlay.UIEventOverlay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace src.UI.Overlays.EventOverlay;

public class UIEventOverlay : UIMenuBase
{
  [Header("Copy")]
  [SerializeField]
  private TextMeshProUGUI _title;
  [SerializeField]
  private TextMeshProUGUI _description;
  [Header("Entries")]
  [SerializeField]
  private UIEventOverlay.Entry _entry1;
  [SerializeField]
  private UIEventOverlay.Entry _entry2;
  [SerializeField]
  private UIEventOverlay.Entry _entry3;
  [Header("Buttons")]
  [SerializeField]
  private MMButton _acceptButton;

  public void Show(SeasonalEventData seasonalEventData, bool instant = false)
  {
    this.Show(instant);
    this._title.text = seasonalEventData.OnboardingTitle;
    this._description.text = seasonalEventData.Description;
    this._entry1.Setup(seasonalEventData.Entry1);
    this._entry2.Setup(seasonalEventData.Entry2);
    this._entry3.Setup(seasonalEventData.Entry3);
    this._acceptButton.onClick.AddListener((UnityAction) (() => this.Hide()));
  }

  protected override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  [Serializable]
  private struct Entry
  {
    [SerializeField]
    private TextMeshProUGUI _text;
    [SerializeField]
    private Image _image;

    public void Setup(SeasonalEventData.OnboardingEntry onboardingEntry)
    {
      this._text.text = onboardingEntry.Text;
      this._image.sprite = onboardingEntry.Image;
    }
  }
}
