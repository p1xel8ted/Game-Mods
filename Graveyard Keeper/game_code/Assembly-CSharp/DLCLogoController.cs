// Decompiled with JetBrains decompiler
// Type: DLCLogoController
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DLCLogoController : MonoBehaviour
{
  [SerializeField]
  public UIWidget _ui_widget;
  [SerializeField]
  public UITable _ui_table;
  [SerializeField]
  public List<DLCLogoElement> dlc_logos = new List<DLCLogoElement>();

  public void Show()
  {
    int num1 = DLCEngine.DLCAvailableCount();
    int num2 = 0;
    DLCLogoElement dlcLogoElement = (DLCLogoElement) null;
    for (int index = 0; index < this.dlc_logos.Count; ++index)
    {
      if (DLCEngine.IsDLCAvailable(this.dlc_logos[index].dlc_version))
      {
        ++num2;
        this.dlc_logos[index].Show(num2 != num1);
        if (num2 == num1)
          dlcLogoElement = this.dlc_logos[index];
      }
      else
        this.dlc_logos[index].Hide();
    }
    this._ui_table.repositionNow = true;
    this._ui_table.Reposition();
    if ((Object) dlcLogoElement != (Object) null)
    {
      this._ui_widget.height = Mathf.RoundToInt(Mathf.Abs(dlcLogoElement.gameObject.transform.localPosition.y));
      this.gameObject.SetActive(true);
    }
    else
      this.gameObject.SetActive(false);
  }

  public void Hide() => this.gameObject.SetActive(false);
}
