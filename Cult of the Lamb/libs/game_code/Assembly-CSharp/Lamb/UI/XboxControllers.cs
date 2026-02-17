// Decompiled with JetBrains decompiler
// Type: Lamb.UI.XboxControllers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src.Extensions;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class XboxControllers : InputDisplay
{
  [SerializeField]
  public RectTransform _xboxSeriesContainer;
  public GameObject _xboxSeries;

  public override void Configure(InputType inputType)
  {
    if ((Object) this._xboxSeries == (Object) null)
    {
      this._xboxSeries = MonoSingleton<UIManager>.Instance.XboxControllerTemplate.Instantiate<InputController>((Transform) this._xboxSeriesContainer).gameObject;
      this._xboxSeries.transform.localPosition = Vector3.zero;
      this._xboxSeries.transform.localScale = Vector3.one;
    }
    this._xboxSeries.SetActive(inputType == InputType.XboxSeries);
  }
}
