// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIDropdownOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src.Extensions;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UIDropdownOverlayController : UIMenuBase
{
  public Action<int> OnItemChosen;
  [SerializeField]
  public RectTransform _dropdownRectTransform;
  [SerializeField]
  public MMButton _backgroundButton;
  [SerializeField]
  public MMScrollRect _scrollRect;
  [SerializeField]
  public RectTransform _contentContainer;
  [SerializeField]
  public DropdownItem _itemTemplate;

  public override void Awake()
  {
    base.Awake();
    this._canvasGroup.alpha = 0.0f;
  }

  public void Show(MMDropdown dropdown, bool instant = false, bool forceLTR = false)
  {
    Vector2 position = (Vector2) dropdown.transform.position;
    if (dropdown.DisplayType == MMDropdown.Display.Top)
      position.y += this._dropdownRectTransform.sizeDelta.y;
    this._dropdownRectTransform.position = (Vector3) position;
    this._dropdownRectTransform.anchoredPosition -= new Vector2(0.0f, dropdown.GetComponent<RectTransform>().rect.height * 0.5f);
    this._backgroundButton.onClick.AddListener(new UnityAction(((UIMenuBase) this).OnCancelButtonInput));
    for (int index = 0; index < dropdown.Content.Length; ++index)
    {
      DropdownItem dropdownItem = this._itemTemplate.Instantiate<DropdownItem>();
      if (forceLTR)
        dropdownItem.Text.isRightToLeftText = false;
      dropdownItem.transform.SetParent((Transform) this._contentContainer);
      dropdownItem.transform.localScale = Vector3.one;
      dropdownItem.OnItemSelected += new Action<DropdownItem>(this.OnItemClicked);
      dropdownItem.Configure(index, dropdown.Content[index], index == dropdown.ContentIndex);
      if (index == dropdown.ContentIndex)
        this.OverrideDefault((Selectable) dropdownItem.Button);
    }
    this.Show(instant);
  }

  public void OnItemClicked(DropdownItem item)
  {
    Action<int> onItemChosen = this.OnItemChosen;
    if (onItemChosen != null)
      onItemChosen(item.Index);
    this.Hide();
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  public override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
}
