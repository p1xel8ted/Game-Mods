// Decompiled with JetBrains decompiler
// Type: NotificationGeneric
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using TMPro;
using UnityEngine;

#nullable disable
public class NotificationGeneric : NotificationBase
{
  [SerializeField]
  private TextMeshProUGUI _icon;
  private string _locKey;
  private string[] _nonLocalizedParameters;
  private string[] _localizedParameters;

  protected override float _onScreenDuration => 3f;

  protected override float _showHideDuration => 0.4f;

  public void ConfigureLocalizedParams(string locKey, params string[] localizedParameters)
  {
    this._localizedParameters = localizedParameters;
    this.Configure(locKey);
  }

  public void ConfigureNonLocalizedParams(string locKey, params string[] nonLocalizedParameters)
  {
    this._nonLocalizedParameters = nonLocalizedParameters;
    this.Configure(locKey);
  }

  public void Configure(NotificationCentre.NotificationType type, NotificationBase.Flair flair = NotificationBase.Flair.None)
  {
    this._icon.text = this.GetNotificationIcon(type);
    this.Configure(NotificationCentre.GetLocKey(type), flair);
  }

  public void Configure(string locKey, NotificationBase.Flair flair = NotificationBase.Flair.None)
  {
    this._locKey = locKey;
    this.Configure(flair);
  }

  protected override void Localize()
  {
    if (this._localizedParameters != null && this._localizedParameters.Length != 0)
    {
      string[] strArray = new string[this._localizedParameters.Length];
      for (int index = 0; index < this._localizedParameters.Length; ++index)
        strArray[index] = LocalizationManager.GetTranslation(this._localizedParameters[index]);
      this._description.text = string.Format(LocalizationManager.GetTranslation(this._locKey), (object[]) strArray);
    }
    else if (this._nonLocalizedParameters != null && this._nonLocalizedParameters.Length != 0)
      this._description.text = string.Format(LocalizationManager.GetTranslation(this._locKey), (object[]) this._nonLocalizedParameters);
    else
      this._description.text = LocalizationManager.GetTranslation(this._locKey);
  }

  private string GetNotificationIcon(NotificationCentre.NotificationType Notification)
  {
    switch (Notification)
    {
      case NotificationCentre.NotificationType.BuildComplete:
        return "\uF6E3";
      case NotificationCentre.NotificationType.FaithUp:
        return "UP";
      case NotificationCentre.NotificationType.FaithUpDoubleArrow:
        return "UP DOUBLE";
      case NotificationCentre.NotificationType.FaithDown:
        return "DOWN";
      case NotificationCentre.NotificationType.FaithDownDoubleArrow:
        return "DOWN DOUBLE";
      default:
        return "";
    }
  }
}
