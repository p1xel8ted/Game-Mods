// Decompiled with JetBrains decompiler
// Type: NotificationDynamicGeneric
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class NotificationDynamicGeneric : NotificationDynamicBase
{
  [SerializeField]
  private TextMeshProUGUI _textIcon;
  [SerializeField]
  private TextMeshProUGUI _text;

  public override Color FullColour => StaticColors.RedColor;

  public override Color EmptyColour => StaticColors.RedColor;

  protected override void UpdateIcon()
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
      default:
        str = "";
        break;
    }
    this._textIcon.text = str;
    this._text.text = this.Data.TotalCount.ToString();
  }
}
