// Decompiled with JetBrains decompiler
// Type: NotificationDynamicGeneric
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class NotificationDynamicGeneric : NotificationDynamicBase
{
  [SerializeField]
  public TextMeshProUGUI _textIcon;
  [SerializeField]
  public TextMeshProUGUI _text;

  public override Color FullColour => StaticColors.RedColor;

  public override Color EmptyColour => StaticColors.RedColor;

  public override void UpdateIcon()
  {
    string str;
    switch (this.Data.Type)
    {
      case NotificationCentre.NotificationType.Exhausted:
        str = "<sprite name=\"icon_Sleep\">";
        break;
      case NotificationCentre.NotificationType.Dynamic_Starving:
        str = "\uF623";
        break;
      case NotificationCentre.NotificationType.Dynamic_Homeless:
        str = "\uF236";
        break;
      case NotificationCentre.NotificationType.Dynamic_Sick:
        str = "<sprite name=\"icon_Sickness\">";
        break;
      case NotificationCentre.NotificationType.Dynamic_Dissenter:
        str = "<sprite name=\"icon_Faith\">";
        break;
      case NotificationCentre.NotificationType.Dynamic_Injured:
        str = "<sprite name=\"icon_Injured\">";
        break;
      case NotificationCentre.NotificationType.Dynamic_Drunk:
        str = "<sprite name=\"icon_Drunk\">";
        break;
      case NotificationCentre.NotificationType.Dynamic_Freezing:
        str = "\uF2DC";
        break;
      default:
        str = "";
        break;
    }
    this._textIcon.text = str;
    this._text.text = this.Data.TotalCount.ToString();
  }
}
