// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.UIGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
namespace Rewired.UI.ControlMapper;

[AddComponentMenu("")]
public class UIGroup : MonoBehaviour
{
  [SerializeField]
  public TMP_Text _label;
  [SerializeField]
  public Transform _content;

  public string labelText
  {
    get => !((Object) this._label != (Object) null) ? string.Empty : this._label.text;
    set
    {
      if ((Object) this._label == (Object) null)
        return;
      this._label.text = value;
    }
  }

  public Transform content => this._content;

  public void SetLabelActive(bool state)
  {
    if ((Object) this._label == (Object) null)
      return;
    this._label.gameObject.SetActive(state);
  }
}
