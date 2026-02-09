// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.UIControl
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Rewired.UI.ControlMapper;

[AddComponentMenu("")]
public class UIControl : MonoBehaviour
{
  public Text title;
  public int _id;
  public bool _showTitle;
  public static int _uidCounter;

  public int id => this._id;

  public void Awake() => this._id = UIControl.GetNextUid();

  public bool showTitle
  {
    get => this._showTitle;
    set
    {
      if ((UnityEngine.Object) this.title == (UnityEngine.Object) null)
        return;
      this.title.gameObject.SetActive(value);
      this._showTitle = value;
    }
  }

  public virtual void SetCancelCallback(Action cancelCallback)
  {
  }

  public static int GetNextUid()
  {
    if (UIControl._uidCounter == int.MaxValue)
      UIControl._uidCounter = 0;
    int uidCounter = UIControl._uidCounter;
    ++UIControl._uidCounter;
    return uidCounter;
  }
}
