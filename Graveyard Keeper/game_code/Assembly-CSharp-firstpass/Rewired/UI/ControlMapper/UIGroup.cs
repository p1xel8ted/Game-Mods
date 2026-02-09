// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.UIGroup
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Rewired.UI.ControlMapper;

[AddComponentMenu("")]
public class UIGroup : MonoBehaviour
{
  [SerializeField]
  public Text _label;
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
