// Decompiled with JetBrains decompiler
// Type: Lamb.UI.FollowerInteractionWheel.UIFollowerWheelInteractionItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.FollowerInteractionWheel;

public class UIFollowerWheelInteractionItem : UIRadialWheelItem
{
  [SerializeField]
  private Image _notification;
  [SerializeField]
  private TextMeshProUGUI _text;
  private string _iconText;
  private string _title;
  private string _description;
  private FollowerCommands _command;
  private CommandItem _commandItem;

  public FollowerCommands FollowerCommand => this._command;

  public CommandItem CommandItem => this._commandItem;

  public void Configure(Follower follower, CommandItem commandItem)
  {
    FollowerInteractionAlerts followerInteractions1 = DataManager.Instance.Alerts.FollowerInteractions;
    followerInteractions1.OnAlertRemoved = followerInteractions1.OnAlertRemoved - new Action<FollowerCommands>(this.ClearAlert);
    if (commandItem != null)
    {
      this._iconText = FontImageNames.IconForCommand(commandItem.Command);
      this._title = commandItem.GetTitle(follower);
      this._description = commandItem.GetDescription(follower);
      this._command = commandItem.Command;
      this._text.text = this._iconText;
      this._commandItem = commandItem;
      if (DataManager.Instance.Alerts.FollowerInteractions.HasAlert(commandItem.Command))
      {
        this._notification.gameObject.SetActive(true);
        FollowerInteractionAlerts followerInteractions2 = DataManager.Instance.Alerts.FollowerInteractions;
        followerInteractions2.OnAlertRemoved = followerInteractions2.OnAlertRemoved + new Action<FollowerCommands>(this.ClearAlert);
      }
      else
        this._notification.gameObject.SetActive(false);
    }
    this.gameObject.SetActive(commandItem != null);
    this._canvasGroup.alpha = commandItem == null ? 0.0f : 1f;
    this._button.interactable = commandItem != null;
    if (commandItem != null && !commandItem.IsAvailable(follower))
    {
      this._description = commandItem.GetLockedDescription(follower);
      this.DoInactive();
    }
    else
      this.DoActive();
  }

  private void ClearAlert(FollowerCommands command)
  {
    if (command != this._command)
      return;
    this._notification.gameObject.SetActive(false);
  }

  public override string GetTitle() => this._title;

  public override bool IsValidOption() => true;

  public override bool Visible() => this._command != 0;

  public override void DoSelected()
  {
    base.DoSelected();
    DataManager.Instance.Alerts.FollowerInteractions.Remove(this._command);
  }

  private void OnDestroy()
  {
    if (DataManager.Instance == null)
      return;
    FollowerInteractionAlerts followerInteractions = DataManager.Instance.Alerts.FollowerInteractions;
    followerInteractions.OnAlertRemoved = followerInteractions.OnAlertRemoved - new Action<FollowerCommands>(this.ClearAlert);
  }

  public override string GetDescription() => this._description;
}
