// Decompiled with JetBrains decompiler
// Type: src.UI.Overlays.EventOverlay.UIEventOverlay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using System;
using System.Runtime.CompilerServices;
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
  public TextMeshProUGUI _title;
  [SerializeField]
  public TextMeshProUGUI _description;
  [Header("Entries")]
  [SerializeField]
  public UIEventOverlay.Entry _entry1;
  [SerializeField]
  public UIEventOverlay.Entry _entry2;
  [SerializeField]
  public UIEventOverlay.Entry _entry3;
  [Header("Buttons")]
  [SerializeField]
  public MMButton _acceptButton;

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

  public override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  [CompilerGenerated]
  public void \u003CShow\u003Eb__7_0() => this.Hide();

  [Serializable]
  public struct Entry
  {
    [SerializeField]
    public TextMeshProUGUI _text;
    [SerializeField]
    public Image _image;

    public void Setup(SeasonalEventData.OnboardingEntry onboardingEntry)
    {
      this._text.text = onboardingEntry.Text;
      this._image.sprite = onboardingEntry.Image;
    }
  }
}
