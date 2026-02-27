// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIDropdownOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private RectTransform _dropdownRectTransform;
  [SerializeField]
  private MMButton _backgroundButton;
  [SerializeField]
  private MMScrollRect _scrollRect;
  [SerializeField]
  private RectTransform _contentContainer;
  [SerializeField]
  private DropdownItem _itemTemplate;

  public override void Awake()
  {
    base.Awake();
    this._canvasGroup.alpha = 0.0f;
  }

  public void Show(MMDropdown dropdown, bool instant = false)
  {
    this._dropdownRectTransform.position = dropdown.transform.position;
    this._dropdownRectTransform.anchoredPosition -= new Vector2(0.0f, dropdown.GetComponent<RectTransform>().rect.height * 0.5f);
    this._backgroundButton.onClick.AddListener(new UnityAction(((UIMenuBase) this).OnCancelButtonInput));
    for (int index = 0; index < dropdown.Content.Length; ++index)
    {
      DropdownItem dropdownItem = this._itemTemplate.Instantiate<DropdownItem>();
      dropdownItem.transform.SetParent((Transform) this._contentContainer);
      dropdownItem.transform.localScale = Vector3.one;
      dropdownItem.OnItemSelected += new Action<DropdownItem>(this.OnItemClicked);
      dropdownItem.Configure(index, dropdown.Content[index], index == dropdown.ContentIndex);
      if (index == dropdown.ContentIndex)
        this.OverrideDefault((Selectable) dropdownItem.Button);
    }
    this.Show(instant);
  }

  private void OnItemClicked(DropdownItem item)
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

  protected override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
}
