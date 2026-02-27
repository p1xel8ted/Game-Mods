// Decompiled with JetBrains decompiler
// Type: Lamb.UI.XboxControllers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using src.Extensions;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class XboxControllers : InputDisplay
{
  [SerializeField]
  private RectTransform _xboxSeriesContainer;
  private GameObject _xboxSeries;

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
