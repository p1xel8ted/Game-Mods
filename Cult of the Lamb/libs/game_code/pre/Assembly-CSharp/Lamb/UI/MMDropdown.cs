// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMDropdown
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using src.Extensions;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI;

public class MMDropdown : MonoBehaviour
{
  public Action<int> OnValueChanged;
  [Header("Components")]
  [SerializeField]
  private MMButton _button;
  [SerializeField]
  private TMP_Text _text;
  [Header("Dropdown Properties")]
  [SerializeField]
  private bool _localizeContent;
  [SerializeField]
  private bool _prefillContent;
  [SerializeField]
  [TermsPopup("")]
  private string[] _prefilledContent;
  private int _contentIndex;
  private string[] _content;
  private bool _interactable = true;

  public MMButton Button => this._button;

  public string[] Content => this._content;

  public int ContentIndex
  {
    get => this._contentIndex;
    set
    {
      if (this._contentIndex == value || this._content == null || value < 0 || value > this._content.Length - 1)
        return;
      this._contentIndex = value;
      this._text.text = this._content[this._contentIndex];
    }
  }

  public bool Interactable
  {
    get => this._interactable;
    set
    {
      this._interactable = value;
      this._button.Interactable = this._interactable;
    }
  }

  private void Awake()
  {
    this._button.onClick.AddListener(new UnityAction(this.Open));
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.ReLocalize);
  }

  private void OnDestroy()
  {
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.ReLocalize);
  }

  public void PrefillContent(List<string> content) => this.PrefillContent(content.ToArray());

  public void PrefillContent(params string[] content)
  {
    this._prefillContent = true;
    this._prefilledContent = content;
    this.UpdateContent(this._prefilledContent);
  }

  private void UpdateContent(string[] newContent)
  {
    this._content = new string[newContent.Length];
    for (int index = 0; index < newContent.Length; ++index)
      this._content[index] = this._localizeContent ? LocalizationManager.GetTranslation(newContent[index]) : newContent[index];
    this._text.text = this._content[this._contentIndex];
  }

  private void ReLocalize()
  {
    if (!this._prefillContent || !this._localizeContent)
      return;
    Debug.Log((object) (this.gameObject.transform.parent.name + " Re-Localize").Colour(Color.red));
    this.UpdateContent(this._prefilledContent);
  }

  public void Open()
  {
    UIMenuBase uiMenuBase = UIMenuBase.ActiveMenus.LastElement<UIMenuBase>();
    UIDropdownOverlayController overlayController = MonoSingleton<UIManager>.Instance.DropdownOverlayControllerTemplate.Instantiate<UIDropdownOverlayController>();
    overlayController.Show(this);
    overlayController.OnItemChosen += (Action<int>) (index =>
    {
      Action<int> onValueChanged = this.OnValueChanged;
      if (onValueChanged != null)
        onValueChanged(index);
      this.ContentIndex = index;
    });
    UIDropdownOverlayController menu = overlayController;
    uiMenuBase.PushInstance<UIDropdownOverlayController>(menu);
  }
}
