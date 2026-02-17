// Decompiled with JetBrains decompiler
// Type: Lamb.UI.DropdownItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI;

public class DropdownItem : MonoBehaviour
{
  public Action<DropdownItem> OnItemSelected;
  [SerializeField]
  public MMButton _button;
  [SerializeField]
  public TMP_Text _text;
  [SerializeField]
  public GameObject _currentSelection;
  public int _index;

  public MMButton Button => this._button;

  public int Index => this._index;

  public TMP_Text Text => this._text;

  public void Configure(int index, string content, bool currentSelection)
  {
    this._index = index;
    this._text.text = content;
    this._currentSelection.SetActive(currentSelection);
    this._button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
  }

  public void OnButtonClicked()
  {
    Action<DropdownItem> onItemSelected = this.OnItemSelected;
    if (onItemSelected == null)
      return;
    onItemSelected(this);
  }
}
