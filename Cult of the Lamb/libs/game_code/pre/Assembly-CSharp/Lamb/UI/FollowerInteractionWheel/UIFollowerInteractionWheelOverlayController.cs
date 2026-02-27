// Decompiled with JetBrains decompiler
// Type: Lamb.UI.FollowerInteractionWheel.UIFollowerInteractionWheelOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using I2.Loc;
using Spine.Unity.Examples;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI.FollowerInteractionWheel;

public class UIFollowerInteractionWheelOverlayController : 
  UIRadialMenuBase<UIFollowerWheelInteractionItem, FollowerCommands[]>
{
  private const string kNextCategory_In_Animation = "NextCategory_In";
  private const string kNextCategory_Out_Animation = "NextCategory_Out";
  private const string kPrevCategory_In_Animation = "PrevCategory_In";
  private const string kPrevCategory_Out_Animation = "PrevCategory_Out";
  private Follower _follower;
  private bool _cancellable;
  private List<CommandItem> _rootCommandItems;
  private Stack<List<CommandItem>> _commandStack = new Stack<List<CommandItem>>();
  private Stack<FollowerCommands> _commandHistory = new Stack<FollowerCommands>();
  public EventInstance _loopInstance;
  private FollowerSpineEventListener listener;
  public TextMeshProUGUI followerName;

  protected override bool SelectOnHighlight => false;

  public void Show(
    Follower follower,
    List<CommandItem> commandItems,
    bool instant = false,
    bool cancellable = true)
  {
    this.Show(instant);
    this._cancellable = cancellable;
    this._follower = follower;
    this._rootCommandItems = commandItems;
    this.followerName.text = $"{(!string.IsNullOrEmpty(follower.Brain.Info.ViewerID) ? $" <sprite name=\"icon_TwitchIcon\"> <color=#{ColorUtility.ToHtmlStringRGB(StaticColors.TwitchPurple)}>" : " <color=yellow>")}{follower.Brain.Info.Name}</color>{(follower.Brain.Info.XPLevel > 1 ? $" {ScriptLocalization.Interactions.Level} {follower.Brain.Info.XPLevel.ToNumeral()}" : "")}{(follower.Brain.Info.MarriedToLeader ? " <sprite name=\"icon_Married\">" : "")}";
    if (!this._cancellable)
      this._controlPrompts.HideCancelButton();
    if ((UnityEngine.Object) this.listener != (UnityEngine.Object) null)
      this.listener.PlayFollowerVOLoop("event:/dialogue/followers/general_acknowledge");
    else
      Debug.Log((object) "Listener not found");
  }

  protected override void OnHideStarted()
  {
    base.OnHideStarted();
    if (!((UnityEngine.Object) this.listener != (UnityEngine.Object) null))
      return;
    this.listener.StopFollowerVOLoop();
  }

  private void OnDisable()
  {
    if (!((UnityEngine.Object) this.listener != (UnityEngine.Object) null))
      return;
    this.listener.StopFollowerVOLoop();
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    if (!((UnityEngine.Object) this.listener != (UnityEngine.Object) null))
      return;
    this.listener.StopFollowerVOLoop();
  }

  protected override void OnShowStarted() => this.ConfigureItems(this._rootCommandItems);

  private void ConfigureItems(List<CommandItem> commandItems)
  {
    if (commandItems.Count > 8)
    {
      commandItems.Sort((Comparison<CommandItem>) ((p1, p2) => p2.IsAvailable(this._follower).CompareTo(p1.IsAvailable(this._follower))));
      List<FollowerCommandItems.NextPageCommandItem> nextPageCommandItemList = new List<FollowerCommandItems.NextPageCommandItem>();
      FollowerCommandItems.NextPageCommandItem nextPageCommandItem1 = FollowerCommandItems.NextPage();
      nextPageCommandItem1.PageNumber = 1;
      nextPageCommandItemList.Add(nextPageCommandItem1);
      commandItems.Insert(4, (CommandItem) nextPageCommandItem1);
      int num = 0;
      for (int index = commandItems.Count - 1; index >= 8; --index)
      {
        CommandItem commandItem = commandItems[index];
        commandItems.RemoveAt(index);
        nextPageCommandItem1.SubCommands.Insert(0, commandItem);
        ++num;
        if (num >= 7 && index > 8)
        {
          FollowerCommandItems.NextPageCommandItem nextPageCommandItem2 = FollowerCommandItems.NextPage();
          nextPageCommandItemList.Add(nextPageCommandItem2);
          nextPageCommandItem1.SubCommands.Insert(4, (CommandItem) nextPageCommandItem2);
          nextPageCommandItem1 = nextPageCommandItem2;
          nextPageCommandItem1.PageNumber = nextPageCommandItemList.Count;
          num = 0;
        }
      }
      foreach (FollowerCommandItems.NextPageCommandItem nextPageCommandItem3 in nextPageCommandItemList)
        nextPageCommandItem3.TotalPageNumbers = nextPageCommandItemList.Count + 1;
    }
    for (int index = 0; index < this._wheelItems.Count; ++index)
    {
      if (index > commandItems.Count - 1)
        this._wheelItems[index].Configure((Follower) null, (CommandItem) null);
      else
        this._wheelItems[index].Configure(this._follower, commandItems[index]);
    }
  }

  private IEnumerator NextCategory(List<CommandItem> nextItems)
  {
    UIFollowerInteractionWheelOverlayController overlayController = this;
    yield return (object) overlayController._animator.YieldForAnimation("NextCategory_Out");
    overlayController.ConfigureItems(nextItems);
    overlayController._controlPrompts.ShowCancelButton();
    yield return (object) overlayController._animator.YieldForAnimation("NextCategory_In");
    overlayController.StartCoroutine((IEnumerator) overlayController.DoWheelLoop());
  }

  private IEnumerator PrevCategory(List<CommandItem> prevItems)
  {
    UIFollowerInteractionWheelOverlayController overlayController = this;
    yield return (object) overlayController._animator.YieldForAnimation("PrevCategory_Out");
    overlayController.ConfigureItems(prevItems);
    yield return (object) overlayController._animator.YieldForAnimation("PrevCategory_In");
    overlayController.StartCoroutine((IEnumerator) overlayController.DoWheelLoop());
  }

  protected override void OnChoiceFinalized()
  {
  }

  protected override void MakeChoice(UIFollowerWheelInteractionItem item)
  {
    if (item.CommandItem.SubCommands != null && item.CommandItem.SubCommands.Count > 0 && item.CommandItem.IsAvailable((Follower) null))
    {
      this._commandStack.Push(this._rootCommandItems);
      this._commandHistory.Push(item.CommandItem.Command);
      this._rootCommandItems = item.CommandItem.SubCommands;
      this.StartCoroutine((IEnumerator) this.NextCategory(this._rootCommandItems));
    }
    else if (item.FollowerCommand == FollowerCommands.AreYouSureNo)
    {
      this.OnCancelButtonInput();
    }
    else
    {
      this.Hide();
      this._commandHistory.Push(item.CommandItem.Command);
      Action<FollowerCommands[]> onItemChosen = this.OnItemChosen;
      if (onItemChosen == null)
        return;
      onItemChosen(this._commandHistory.ToArray());
    }
  }

  public override void OnCancelButtonInput()
  {
    if (!this._cancellable || !this._canvasGroup.interactable)
      return;
    UIManager.PlayAudio("event:/ui/go_back");
    if (this._commandStack.Count > 0)
    {
      this._rootCommandItems = this._commandStack.Pop();
      int num = (int) this._commandHistory.Pop();
      this.StopAllCoroutines();
      this.CleanupWheelLoop();
      this.StartCoroutine((IEnumerator) this.PrevCategory(this._rootCommandItems));
      if (this._commandStack.Count != 0 || this._cancellable)
        return;
      this._controlPrompts.HideCancelButton();
    }
    else
      base.OnCancelButtonInput();
  }
}
