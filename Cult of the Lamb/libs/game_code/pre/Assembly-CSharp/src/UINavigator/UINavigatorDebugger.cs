// Decompiled with JetBrains decompiler
// Type: src.UINavigator.UINavigatorDebugger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace src.UINavigator;

[RequireComponent(typeof (UINavigatorNew))]
public class UINavigatorDebugger : MonoBehaviour
{
  private UINavigatorNew _uiNavigatorNew;

  public Selectable _currentSelectable
  {
    get
    {
      return this._uiNavigatorNew.CurrentSelectable == null ? (Selectable) null : this._uiNavigatorNew.CurrentSelectable.Selectable;
    }
  }

  public void Awake() => this._uiNavigatorNew = this.GetComponent<UINavigatorNew>();

  public void Update()
  {
  }
}
