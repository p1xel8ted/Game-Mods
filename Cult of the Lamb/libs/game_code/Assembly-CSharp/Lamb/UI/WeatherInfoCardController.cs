// Decompiled with JetBrains decompiler
// Type: Lamb.UI.WeatherInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.Rituals;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class WeatherInfoCardController : 
  UIInfoCardController<WeatherInfoCard, SeasonsManager.WeatherEvent>
{
  public override bool IsSelectionValid(
    Selectable selectable,
    out SeasonsManager.WeatherEvent showParam)
  {
    showParam = SeasonsManager.WeatherEvent.None;
    NotificationDynamicWeather component;
    if (!selectable.TryGetComponent<NotificationDynamicWeather>(out component))
      return false;
    showParam = component.WeatherType;
    return true;
  }

  public override SeasonsManager.WeatherEvent DefaultShowParam()
  {
    return SeasonsManager.WeatherEvent.None;
  }
}
