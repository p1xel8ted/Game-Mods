// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMDropdown
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Rewired;
using src.Extensions;
using src.UINavigator;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI;

public class MMDropdown : MonoBehaviour
{
  public Action<int> OnValueChanged;
  public System.Action OnOpenDropdownOverlay;
  public System.Action OnCloseDropdownOverlay;
  [Header("Components")]
  [SerializeField]
  public MMButton _button;
  [SerializeField]
  public TMP_Text _text;
  [Header("Dropdown Properties")]
  [SerializeField]
  public bool _localizeContent;
  [SerializeField]
  public bool _prefillContent;
  [SerializeField]
  [TermsPopup("")]
  public string[] _prefilledContent;
  [SerializeField]
  public MMDropdown.Display _displayType;
  [SerializeField]
  public GameObject _dropdownGameobject;
  [SerializeField]
  public GameObject _buttonPrompt;
  [SerializeField]
  public bool _showPromptIfController;
  public UIDropdownOverlayController _dropdownOverlayController;
  public int _contentIndex;
  public string[] _content;
  public bool _interactable = true;
  public bool _forceLTR;

  public MMButton Button => this._button;

  public string[] Content => this._content;

  public MMDropdown.Display DisplayType => this._displayType;

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

  public void Awake()
  {
    this._button.onClick.AddListener(new UnityAction(this.Open));
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.ReLocalize);
    if (this._showPromptIfController)
    {
      InputManager.General.OnActiveControllerChanged += new Action<Controller>(this.OnActiveControllerChanged);
      this._buttonPrompt.SetActive(InputManager.General.InputIsController());
      this._dropdownGameobject.SetActive(!this._buttonPrompt.activeSelf);
    }
    else
      this._buttonPrompt.SetActive(false);
  }

  public void OnActiveControllerChanged(Controller controller)
  {
    this._buttonPrompt.SetActive(controller.type == ControllerType.Joystick);
    this._dropdownGameobject.SetActive(!this._buttonPrompt.activeSelf);
  }

  public void OnDestroy()
  {
    if ((UnityEngine.Object) this._dropdownOverlayController != (UnityEngine.Object) null)
      this._dropdownOverlayController.Hide(true);
    InputManager.General.OnActiveControllerChanged -= new Action<Controller>(this.OnActiveControllerChanged);
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.ReLocalize);
  }

  public void PrefillContent(List<string> content) => this.PrefillContent(content.ToArray());

  public void PrefillContent(params string[] content)
  {
    this._prefillContent = true;
    this._prefilledContent = content;
    this.UpdateContent(this._prefilledContent);
  }

  public void ForceLTR()
  {
    this._forceLTR = true;
    this._text.isRightToLeftText = false;
  }

  public void UpdateContent(string[] newContent)
  {
    this._content = new string[newContent.Length];
    for (int index = 0; index < newContent.Length; ++index)
      this._content[index] = this._localizeContent ? LocalizationManager.GetTranslation(newContent[index]) : newContent[index];
    this._text.text = this._content[this._contentIndex];
  }

  public void ReLocalize()
  {
    if (!this._prefillContent || !this._localizeContent)
      return;
    Debug.Log((object) (this.gameObject.transform.parent.name + " Re-Localize").Colour(Color.red));
    this.UpdateContent(this._prefilledContent);
  }

  public void Open()
  {
    if ((UnityEngine.Object) this._dropdownOverlayController != (UnityEngine.Object) null)
      return;
    UIMenuBase uiMenuBase = UIMenuBase.ActiveMenus.LastElement<UIMenuBase>();
    if ((UnityEngine.Object) uiMenuBase == (UnityEngine.Object) null)
      return;
    this._dropdownOverlayController = MonoSingleton<UIManager>.Instance.DropdownOverlayControllerTemplate.Instantiate<UIDropdownOverlayController>();
    this._dropdownOverlayController.Show(this, forceLTR: this._forceLTR);
    this._dropdownOverlayController.OnItemChosen += (Action<int>) (index =>
    {
      Action<int> onValueChanged = this.OnValueChanged;
      if (onValueChanged != null)
        onValueChanged(index);
      this.ContentIndex = index;
    });
    UIDropdownOverlayController overlayController = this._dropdownOverlayController;
    overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() =>
    {
      System.Action closeDropdownOverlay = this.OnCloseDropdownOverlay;
      if (closeDropdownOverlay == null)
        return;
      closeDropdownOverlay();
    });
    uiMenuBase.PushInstance<UIDropdownOverlayController>(this._dropdownOverlayController);
    System.Action openDropdownOverlay = this.OnOpenDropdownOverlay;
    if (openDropdownOverlay == null)
      return;
    openDropdownOverlay();
  }

  public void Update()
  {
    if (!this.gameObject.activeInHierarchy || !this._showPromptIfController || !this._interactable || !InputManager.UI.GetCookButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
      return;
    this.Open();
  }

  [CompilerGenerated]
  public void \u003COpen\u003Eb__38_0(int index)
  {
    Action<int> onValueChanged = this.OnValueChanged;
    if (onValueChanged != null)
      onValueChanged(index);
    this.ContentIndex = index;
  }

  [CompilerGenerated]
  public void \u003COpen\u003Eb__38_1()
  {
    System.Action closeDropdownOverlay = this.OnCloseDropdownOverlay;
    if (closeDropdownOverlay == null)
      return;
    closeDropdownOverlay();
  }

  [Serializable]
  public enum Display
  {
    Bottom,
    Top,
  }
}
