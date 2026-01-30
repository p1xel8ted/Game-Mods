// Decompiled with JetBrains decompiler
// Type: Lamb.UI.WeatherInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
