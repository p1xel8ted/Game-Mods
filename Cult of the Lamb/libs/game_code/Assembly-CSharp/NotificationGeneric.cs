// Decompiled with JetBrains decompiler
// Type: NotificationGeneric
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using TMPro;
using UnityEngine;

#nullable disable
public class NotificationGeneric : NotificationBase, IPoolListener
{
  [SerializeField]
  public TextMeshProUGUI _icon;
  public string _locKey;
  public string[] _nonLocalizedParameters;
  public string[] _localizedParameters;

  public override float _onScreenDuration => 3f;

  public override float _showHideDuration => 0.4f;

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

  public override void Localize()
  {
    if (this._localizedParameters != null && this._localizedParameters.Length != 0)
    {
      string[] strArray = new string[this._localizedParameters.Length];
      for (int index = 0; index < this._localizedParameters.Length; ++index)
        strArray[index] = LocalizationManager.GetTranslation(this._localizedParameters[index]);
      this._description.text = string.Format(LocalizationManager.GetTranslation(this._locKey), (object[]) strArray);
    }
    else if (this._nonLocalizedParameters != null && this._nonLocalizedParameters.Length != 0)
    {
      string[] strArray = new string[this._nonLocalizedParameters.Length];
      for (int index = 0; index < this._nonLocalizedParameters.Length; ++index)
        strArray[index] = LocalizeIntegration.FixEnglishWord(this._nonLocalizedParameters[index]);
      this._description.text = string.Format(LocalizationManager.GetTranslation(this._locKey), (object[]) strArray);
    }
    else
      this._description.text = LocalizationManager.GetTranslation(this._locKey);
  }

  public string GetNotificationIcon(NotificationCentre.NotificationType Notification)
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

  public void OnRecycled()
  {
    this._nonLocalizedParameters = (string[]) null;
    this._localizedParameters = (string[]) null;
  }
}
