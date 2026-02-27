// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.UIGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
namespace Rewired.UI.ControlMapper;

[AddComponentMenu("")]
public class UIGroup : MonoBehaviour
{
  [SerializeField]
  private TMP_Text _label;
  [SerializeField]
  private Transform _content;

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
