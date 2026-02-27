// Decompiled with JetBrains decompiler
// Type: Lamb.UI.HistoricalNotificationBase`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public abstract class HistoricalNotificationBase<T> : UIHistoricalNotification where T : FinalizedNotification
{
  [SerializeField]
  protected TextMeshProUGUI _description;

  public void Configure(T finalizedNotification)
  {
    this._description.text = this.GetLocalizedDescription(finalizedNotification);
    this.ConfigureImpl(finalizedNotification);
  }

  protected abstract void ConfigureImpl(T finalizedNotification);

  protected virtual string GetLocalizedDescription(T finalizedNotification)
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
