// Decompiled with JetBrains decompiler
// Type: src.UINavigator.UINavigatorDebugger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace src.UINavigator;

[RequireComponent(typeof (UINavigatorNew))]
public class UINavigatorDebugger : MonoBehaviour
{
  public UINavigatorNew _uiNavigatorNew;

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
