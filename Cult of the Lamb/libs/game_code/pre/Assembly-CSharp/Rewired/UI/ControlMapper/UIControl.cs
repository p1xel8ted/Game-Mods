// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.UIControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;

#nullable disable
namespace Rewired.UI.ControlMapper;

[AddComponentMenu("")]
public class UIControl : MonoBehaviour
{
  public TMP_Text title;
  private int _id;
  private bool _showTitle;
  private static int _uidCounter;

  public int id => this._id;

  private void Awake() => this._id = UIControl.GetNextUid();

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

  private static int GetNextUid()
  {
    if (UIControl._uidCounter == int.MaxValue)
      UIControl._uidCounter = 0;
    int uidCounter = UIControl._uidCounter;
    ++UIControl._uidCounter;
    return uidCounter;
  }
}
