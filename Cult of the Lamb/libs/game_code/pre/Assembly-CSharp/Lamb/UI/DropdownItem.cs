// Decompiled with JetBrains decompiler
// Type: Lamb.UI.DropdownItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private MMButton _button;
  [SerializeField]
  private TMP_Text _text;
  [SerializeField]
  private GameObject _currentSelection;
  private int _index;

  public MMButton Button => this._button;

  public int Index => this._index;

  public void Configure(int index, string content, bool currentSelection)
  {
    this._index = index;
    this._text.text = content;
    this._currentSelection.SetActive(currentSelection);
    this._button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
  }

  private void OnButtonClicked()
  {
    Action<DropdownItem> onItemSelected = this.OnItemSelected;
    if (onItemSelected == null)
      return;
    onItemSelected(this);
  }
}
