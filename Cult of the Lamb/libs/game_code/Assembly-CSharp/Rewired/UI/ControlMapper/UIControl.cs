// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.UIControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;

#nullable disable
namespace Rewired.UI.ControlMapper;

[AddComponentMenu("")]
public class UIControl : MonoBehaviour
{
  public TMP_Text title;
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
