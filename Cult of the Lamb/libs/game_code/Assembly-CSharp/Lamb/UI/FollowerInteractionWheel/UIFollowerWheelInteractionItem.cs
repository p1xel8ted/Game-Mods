// Decompiled with JetBrains decompiler
// Type: Lamb.UI.FollowerInteractionWheel.UIFollowerWheelInteractionItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.FollowerInteractionWheel;

public class UIFollowerWheelInteractionItem : UIRadialWheelItem
{
  [SerializeField]
  public Image _notification;
  [SerializeField]
  public TextMeshProUGUI _text;
  [SerializeField]
  public TextMeshProUGUI _subText;
  public string _iconText;
  public string _subIconText;
  public string _title;
  public string _description;
  public FollowerCommands _command;
  public CommandItem _commandItem;

  public FollowerCommands FollowerCommand => this._command;

  public CommandItem CommandItem => this._commandItem;

  public void Configure(Follower follower, CommandItem commandItem)
  {
    FollowerInteractionAlerts followerInteractions1 = DataManager.Instance.Alerts.FollowerInteractions;
    followerInteractions1.OnAlertRemoved = followerInteractions1.OnAlertRemoved - new Action<FollowerCommands>(this.ClearAlert);
    if (commandItem != null)
    {
      this._iconText = commandItem.GetIcon();
      this._subIconText = commandItem.GetSubIcon();
      this._title = commandItem.GetTitle(follower);
      this._description = commandItem.GetDescription(follower);
      this._command = commandItem.Command;
      this._text.text = this._iconText;
      this._subText.text = this._subIconText;
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

  public void Configure(
    StructuresData.Ranchable_Animal animal,
    AnimalInteractionModel.AnimalCommandItem commandItem)
  {
    if (commandItem != null)
    {
      this._iconText = commandItem.GetIcon();
      this._subIconText = commandItem.GetSubIcon();
      this._title = commandItem.GetTitle(animal);
      this._description = commandItem.GetDescription(animal);
      this._command = commandItem.Command;
      this._text.text = this._iconText;
      this._subText.text = this._subIconText;
      this._commandItem = (CommandItem) commandItem;
    }
    this.gameObject.SetActive(commandItem != null);
    this._canvasGroup.alpha = commandItem == null ? 0.0f : 1f;
    this._button.interactable = commandItem != null;
    if (commandItem != null && !commandItem.IsAvailable(animal))
    {
      this._description = commandItem.GetLockedDescription(animal);
      this.DoInactive();
    }
    else
      this.DoActive();
    this._notification.gameObject.SetActive(this.IsObjectiveItem(animal));
  }

  public void ClearAlert(FollowerCommands command)
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

  public void OnDestroy()
  {
    if (DataManager.Instance == null)
      return;
    FollowerInteractionAlerts followerInteractions = DataManager.Instance.Alerts.FollowerInteractions;
    followerInteractions.OnAlertRemoved = followerInteractions.OnAlertRemoved - new Action<FollowerCommands>(this.ClearAlert);
  }

  public override string GetDescription() => this._description;

  public bool IsObjectiveItem(StructuresData.Ranchable_Animal animal)
  {
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective.Type == Objectives.TYPES.FEED_ANIMAL && this._commandItem is AnimalInteractionModel.FeedItem commandItem && ((Objectives_FeedAnimal) objective).Food == commandItem.FoodType && ((Objectives_FeedAnimal) objective).TargetAnimal == animal.ID)
        return true;
    }
    return false;
  }
}
