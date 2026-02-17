// Decompiled with JetBrains decompiler
// Type: Lamb.UI.HistoricalNotificationBase`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public abstract class HistoricalNotificationBase<T> : UIHistoricalNotification where T : FinalizedNotification
{
  [SerializeField]
  public TextMeshProUGUI _description;

  public void Configure(T finalizedNotification)
  {
    this._description.text = this.GetLocalizedDescription(finalizedNotification);
    this.ConfigureImpl(finalizedNotification);
  }

  public abstract void ConfigureImpl(T finalizedNotification);

  public virtual string GetLocalizedDescription(T finalizedNotification)
  {
    if (finalizedNotification.LocalisedParameters.Length != 0)
    {
      string[] strArray = new string[finalizedNotification.LocalisedParameters.Length];
      for (int index = 0; index < finalizedNotification.LocalisedParameters.Length; ++index)
        strArray[index] = LocalizationManager.GetTranslation(finalizedNotification.LocalisedParameters[index]);
      return string.Format(LocalizationManager.GetTranslation(finalizedNotification.LocKey), (object[]) strArray);
    }
    return finalizedNotification.NonLocalisedParameters.Length != 0 ? string.Format(LocalizationManager.GetTranslation(finalizedNotification.LocKey), (object[]) finalizedNotification.NonLocalisedParameters) : LocalizationManager.GetTranslation(finalizedNotification.LocKey);
  }
}
