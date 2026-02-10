// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMHorizontalSelector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI;

public class MMHorizontalSelector : MonoBehaviour
{
  public const string kNextAnimationState = "Forward";
  public const string kPreviousAnimationState = "Previous";
  public Action<int> OnSelectionChanged;
  public bool _interactable = true;
  [Header("Text")]
  [SerializeField]
  public TextMeshProUGUI _text;
  [SerializeField]
  public TextMeshProUGUI _textHelper;
  [Header("Content")]
  [SerializeField]
  public bool _invertAnimations;
  [SerializeField]
  public bool _loopContent;
  [SerializeField]
  public bool _localizeContent;
  [SerializeField]
  public bool _prefillContent;
  [SerializeField]
  [TermsPopup("")]
  public string[] _prefilledContent;
  [Header("Buttons")]
  [SerializeField]
  public MMButton _leftButton;
  [SerializeField]
  public MMButton _rightButton;
  [Header("Misc")]
  [SerializeField]
  public CanvasGroup _canvasGroup;
  [SerializeField]
  public Animator _animator;
  public string[] _content;
  public int _contentIndex;

  public MMButton LeftButton => this._leftButton;

  public MMButton RightButton => this._rightButton;

  public bool LocalizeContent
  {
    get => this._localizeContent;
    set => this._localizeContent = value;
  }

  public bool Interactable
  {
    get => this._interactable;
    set
    {
      this._interactable = value;
      this._leftButton.interactable = this._interactable;
      this._rightButton.interactable = this._interactable;
      this._canvasGroup.alpha = this._interactable ? 1f : 0.5f;
    }
  }

  public Color Color
  {
    get => this._text.color;
    set => this._text.color = this._textHelper.color = value;
  }

  public int CurrentSelection => this._contentIndex;

  public int ContentIndex
  {
    get => this._contentIndex;
    set
    {
      if (this._contentIndex == value || this._content == null || value < 0 || value > this._content.Length - 1)
        return;
      this._contentIndex = value;
      this._text.text = this._content[this._contentIndex];
      this._textHelper.text = this._content[this._contentIndex];
      this._leftButton.gameObject.SetActive(this._contentIndex > 0 && !this._loopContent);
      this._rightButton.gameObject.SetActive(this._contentIndex < this._content.Length - 1 && !this._loopContent);
    }
  }

  public string[] Content => this._content;

  public void Awake()
  {
    if (this._prefillContent)
      this.UpdateContent(this._prefilledContent);
    this._leftButton.onClick.AddListener(new UnityAction(this.OnLeftButtonClicked));
    this._rightButton.onClick.AddListener(new UnityAction(this.OnRightButtonClicked));
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.ReLocalize);
  }

  public void OnDestroy()
  {
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.ReLocalize);
  }

  public void OnLeftButtonClicked()
  {
    int contentIndex = this._contentIndex;
    if (this._contentIndex == 0 && this._loopContent)
      this._contentIndex = this._content.Length - 1;
    else if (this._contentIndex > 0)
    {
      --this._contentIndex;
    }
    else
    {
      this._text.gameObject.transform.DOKill();
      this._text.gameObject.transform.localPosition = Vector3.zero;
      this._text.gameObject.transform.DOPunchPosition(new Vector3(-5f, 0.0f, 0.0f), 0.5f).SetUpdate<Tweener>(true);
      return;
    }
    this._leftButton.gameObject.SetActive(this._contentIndex > 0 && !this._loopContent);
    this._rightButton.gameObject.SetActive(true);
    Action<int> selectionChanged = this.OnSelectionChanged;
    if (selectionChanged != null)
      selectionChanged(this._contentIndex);
    this._text.text = this._content[this._contentIndex];
    this._textHelper.text = this._content[contentIndex];
    this._animator.Play(this._invertAnimations ? "Forward" : "Previous", -1, 0.0f);
  }

  public void OnRightButtonClicked()
  {
    int contentIndex = this._contentIndex;
    if (this._contentIndex == this._content.Length - 1 && this._loopContent)
      this._contentIndex = 0;
    else if (this._contentIndex < this._content.Length - 1)
    {
      ++this._contentIndex;
    }
    else
    {
      this._text.gameObject.transform.DOKill();
      this._text.gameObject.transform.localPosition = Vector3.zero;
      this._text.gameObject.transform.DOPunchPosition(new Vector3(5f, 0.0f, 0.0f), 0.5f).SetUpdate<Tweener>(true);
      return;
    }
    this._rightButton.gameObject.SetActive(this._contentIndex < this._content.Length - 1 && !this._loopContent);
    this._leftButton.gameObject.SetActive(true);
    Action<int> selectionChanged = this.OnSelectionChanged;
    if (selectionChanged != null)
      selectionChanged(this._contentIndex);
    this._text.text = this._content[this._contentIndex];
    this._textHelper.text = this._content[contentIndex];
    this._animator.Play(this._invertAnimations ? "Previous" : "Forward", -1, 0.0f);
  }

  public void PrefillContent(List<string> content) => this.PrefillContent(content.ToArray());

  public void PrefillContent(params string[] content)
  {
    this._prefillContent = true;
    this._prefilledContent = content;
    this.UpdateContent(this._prefilledContent);
  }

  public void ForceLTR() => this._text.isRightToLeftText = false;

  public void UpdateContent(string[] newContent)
  {
    this._content = new string[newContent.Length];
    for (int index = 0; index < newContent.Length; ++index)
      this._content[index] = this._localizeContent ? LocalizationManager.GetTranslation(newContent[index]) : newContent[index];
    this._text.text = this._content[this._contentIndex];
    this._textHelper.text = this._content[this._contentIndex];
    this._leftButton.gameObject.SetActive(this._contentIndex > 0 && !this._loopContent);
    this._rightButton.gameObject.SetActive(this._contentIndex < this._content.Length - 1 && !this._loopContent);
  }

  public void ReLocalize()
  {
    if (!this._prefillContent || !this._localizeContent)
      return;
    Debug.Log((object) (this.gameObject.transform.parent.name + " Re-Localize").Colour(Color.red));
    this.UpdateContent(this._prefilledContent);
  }
}
