// Decompiled with JetBrains decompiler
// Type: Lamb.UI.FollowerInteractionWheel.UIFollowerInteractionWheelOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using I2.Loc;
using Spine.Unity.Examples;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI.FollowerInteractionWheel;

public class UIFollowerInteractionWheelOverlayController : 
  UIRadialMenuBase<UIFollowerWheelInteractionItem, FollowerCommands[]>
{
  public const string kNextCategory_In_Animation = "NextCategory_In";
  public const string kNextCategory_Out_Animation = "NextCategory_Out";
  public const string kPrevCategory_In_Animation = "PrevCategory_In";
  public const string kPrevCategory_Out_Animation = "PrevCategory_Out";
  public Follower _follower;
  public StructuresData.Ranchable_Animal _animal;
  public bool _cancellable;
  public List<CommandItem> _rootCommandItems;
  public Stack<List<CommandItem>> _commandStack = new Stack<List<CommandItem>>();
  public Stack<FollowerCommands> _commandHistory = new Stack<FollowerCommands>();
  public EventInstance _loopInstance;
  public FollowerSpineEventListener listener;
  public TextMeshProUGUI followerName;

  public override bool SelectOnHighlight => false;

  public void Show(
    Follower follower,
    List<CommandItem> commandItems,
    bool instant = false,
    bool cancellable = true,
    PlayerFarming playerFarming = null)
  {
    this.Show(instant);
    this._cancellable = cancellable;
    this._follower = follower;
    this._rootCommandItems = commandItems;
    string str1 = !string.IsNullOrEmpty(follower.Brain.Info.ViewerID) ? $" <sprite name=\"icon_TwitchIcon\"> <color=#{ColorUtility.ToHtmlStringRGB(StaticColors.TwitchPurple)}>" : " <color=yellow>";
    string str2 = follower.Brain.Info.XPLevel > 1 ? $" {ScriptLocalization.Interactions.Level} {follower.Brain.Info.XPLevel.ToNumeral()}" : "";
    if (follower.Brain.Info.IsDisciple)
      str2 = " <sprite name=\"icon_Disciple\">";
    this.followerName.text = $"{str1}{follower.Brain.Info.Name}</color>{str2}{(follower.Brain.Info.MarriedToLeader ? " <sprite name=\"icon_Married\">" : "")}";
    if (!this._cancellable)
      this._controlPrompts.HideCancelButton();
    if ((UnityEngine.Object) this.listener != (UnityEngine.Object) null)
      this.listener.PlayFollowerVOLoop("event:/dialogue/followers/general_acknowledge");
    else
      Debug.Log((object) "Listener not found");
  }

  public void Show(
    StructuresData.Ranchable_Animal animal,
    List<CommandItem> commandItems,
    bool instant,
    bool cancellable,
    PlayerFarming playerFarming = null)
  {
    this.Show(instant);
    this._animal = animal;
    this._cancellable = cancellable;
    this._rootCommandItems = commandItems;
    this.followerName.gameObject.SetActive(false);
    if (this._cancellable)
      return;
    this._controlPrompts.HideCancelButton();
  }

  public override void OnHideStarted()
  {
    base.OnHideStarted();
    if (!((UnityEngine.Object) this.listener != (UnityEngine.Object) null))
      return;
    this.listener.StopFollowerVOLoop();
  }

  public new void OnDisable()
  {
    if (!((UnityEngine.Object) this.listener != (UnityEngine.Object) null))
      return;
    this.listener.StopFollowerVOLoop();
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (!((UnityEngine.Object) this.listener != (UnityEngine.Object) null))
      return;
    this.listener.StopFollowerVOLoop();
  }

  public override void OnShowStarted() => this.ConfigureItems(this._rootCommandItems);

  public void ConfigureItems(List<CommandItem> allCommands)
  {
    if (allCommands.Count <= 8)
    {
      allCommands.Sort((Comparison<CommandItem>) ((p1, p2) => p2.IsAvailable(this._follower).CompareTo(p1.IsAvailable(this._follower))));
      this.AssignToWheel(allCommands);
    }
    else
    {
      if ((UnityEngine.Object) this._follower != (UnityEngine.Object) null)
        allCommands.Sort((Comparison<CommandItem>) ((p1, p2) => p2.IsAvailable(this._follower).CompareTo(p1.IsAvailable(this._follower))));
      else if (this._animal != null)
        allCommands.Sort((Comparison<CommandItem>) ((p1, p2) => ((AnimalInteractionModel.AnimalCommandItem) p2).IsAvailable(this._animal).CompareTo(((AnimalInteractionModel.AnimalCommandItem) p1).IsAvailable(this._animal))));
      List<List<CommandItem>> commandItemListList1 = new List<List<CommandItem>>();
      int count;
      for (int index = 0; index < allCommands.Count; index += count)
      {
        count = Mathf.Min(7, allCommands.Count - index);
        commandItemListList1.Add(allCommands.GetRange(index, count));
      }
      List<List<CommandItem>> commandItemListList2 = new List<List<CommandItem>>();
      for (int index1 = 0; index1 < commandItemListList1.Count; ++index1)
      {
        List<CommandItem> commandItemList = new List<CommandItem>((IEnumerable<CommandItem>) commandItemListList1[index1]);
        bool flag1 = index1 == 0;
        int num1 = index1 == commandItemListList1.Count - 1 ? 1 : 0;
        bool flag2 = !flag1;
        bool flag3 = num1 == 0;
        int num2 = 0;
        if (flag2)
          ++num2;
        if (flag3)
          ++num2;
        int num3 = 8 - num2;
        while (commandItemList.Count > num3)
          commandItemList.RemoveAt(commandItemList.Count - 1);
        if (flag2 & flag3)
        {
          FollowerCommandItems.PrevPageCommandItem prevPageCommandItem = FollowerCommandItems.PrevPage();
          prevPageCommandItem.PageNumber = index1 + 1;
          prevPageCommandItem.TotalPageNumbers = commandItemListList1.Count;
          int index2 = Mathf.Min(3, commandItemList.Count);
          commandItemList.Insert(index2, (CommandItem) prevPageCommandItem);
          FollowerCommandItems.NextPageCommandItem nextPageCommandItem = FollowerCommandItems.NextPage();
          nextPageCommandItem.PageNumber = index1 + 2;
          nextPageCommandItem.TotalPageNumbers = commandItemListList1.Count;
          int index3 = Mathf.Min(4, commandItemList.Count);
          commandItemList.Insert(index3, (CommandItem) nextPageCommandItem);
        }
        else if (flag2)
        {
          FollowerCommandItems.PrevPageCommandItem prevPageCommandItem = FollowerCommandItems.PrevPage();
          prevPageCommandItem.PageNumber = index1 + 1;
          prevPageCommandItem.TotalPageNumbers = commandItemListList1.Count;
          int index4 = Mathf.Min(3, commandItemList.Count);
          commandItemList.Insert(index4, (CommandItem) prevPageCommandItem);
        }
        else if (flag3)
        {
          FollowerCommandItems.NextPageCommandItem nextPageCommandItem = FollowerCommandItems.NextPage();
          nextPageCommandItem.PageNumber = index1 + 2;
          nextPageCommandItem.TotalPageNumbers = commandItemListList1.Count;
          int index5 = Mathf.Min(3, commandItemList.Count);
          commandItemList.Insert(index5, (CommandItem) nextPageCommandItem);
        }
        commandItemListList2.Add(commandItemList);
      }
      for (int index = 0; index < commandItemListList2.Count; ++index)
      {
        List<CommandItem> commandItemList = commandItemListList2[index];
        if (index < commandItemListList2.Count - 1)
        {
          CommandItem commandItem = commandItemList.Find((Predicate<CommandItem>) (c => c.Command == FollowerCommands.NextPage));
          if (commandItem != null)
            commandItem.SubCommands = commandItemListList2[index + 1];
        }
        if (index > 0)
        {
          CommandItem commandItem = commandItemList.Find((Predicate<CommandItem>) (c => c.Command == FollowerCommands.PrevPage));
          if (commandItem != null)
            commandItem.SubCommands = commandItemListList2[index - 1];
        }
      }
      this.AssignToWheel(commandItemListList2[0]);
    }
  }

  public void AssignToWheel(List<CommandItem> items)
  {
    Debug.Log((object) $"AssignToWheel with {items.Count} items:");
    for (int index = 0; index < items.Count; ++index)
      Debug.Log((object) $"  - Index {index}, Command = {items[index].Command}");
    for (int index = 0; index < this._wheelItems.Count; ++index)
    {
      if (index >= items.Count)
        this._wheelItems[index].Configure((StructuresData.Ranchable_Animal) null, (AnimalInteractionModel.AnimalCommandItem) null);
      else if (this._animal != null)
      {
        if (items[index].Command == FollowerCommands.NextPage || items[index].Command == FollowerCommands.PrevPage)
          this._wheelItems[index].Configure((Follower) null, items[index]);
        else
          this._wheelItems[index].Configure(this._animal, items[index] as AnimalInteractionModel.AnimalCommandItem);
      }
      else
        this._wheelItems[index].Configure(this._follower, items[index]);
    }
  }

  public IEnumerator NextCategory(List<CommandItem> nextItems)
  {
    UIFollowerInteractionWheelOverlayController overlayController = this;
    yield return (object) overlayController._animator.YieldForAnimation("NextCategory_Out");
    overlayController.ConfigureItems(nextItems);
    overlayController._controlPrompts.ShowCancelButton();
    yield return (object) overlayController._animator.YieldForAnimation("NextCategory_In");
    overlayController.StartCoroutine((IEnumerator) overlayController.DoWheelLoop());
  }

  public IEnumerator PrevCategory(List<CommandItem> prevItems)
  {
    UIFollowerInteractionWheelOverlayController overlayController = this;
    yield return (object) overlayController._animator.YieldForAnimation("PrevCategory_Out");
    overlayController.ConfigureItems(prevItems);
    yield return (object) overlayController._animator.YieldForAnimation("PrevCategory_In");
    overlayController.StartCoroutine((IEnumerator) overlayController.DoWheelLoop());
  }

  public override void OnChoiceFinalized()
  {
  }

  public override void MakeChoice(UIFollowerWheelInteractionItem item)
  {
    if (item.CommandItem.SubCommands != null && item.CommandItem.SubCommands.Count > 0 && item.CommandItem.IsAvailable((Follower) null))
    {
      this._commandStack.Push(this._rootCommandItems);
      this._commandHistory.Push(item.CommandItem.Command);
      this._rootCommandItems = item.CommandItem.SubCommands;
      this.StartCoroutine((IEnumerator) this.NextCategory(this._rootCommandItems));
    }
    else if (item.FollowerCommand == FollowerCommands.PrevPage)
    {
      this._rootCommandItems = this._commandStack.Pop();
      int num = (int) this._commandHistory.Pop();
      this.StartCoroutine((IEnumerator) this.PrevCategory(this._rootCommandItems));
    }
    else if (item.FollowerCommand == FollowerCommands.AreYouSureNo)
    {
      this.OnCancelButtonInput();
    }
    else
    {
      this._finalizedSelection = true;
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
    if (!this._cancellable && this._commandStack.Count <= 0 || !this._canvasGroup.interactable)
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

  [CompilerGenerated]
  public int \u003CConfigureItems\u003Eb__21_0(CommandItem p1, CommandItem p2)
  {
    return p2.IsAvailable(this._follower).CompareTo(p1.IsAvailable(this._follower));
  }

  [CompilerGenerated]
  public int \u003CConfigureItems\u003Eb__21_1(CommandItem p1, CommandItem p2)
  {
    return p2.IsAvailable(this._follower).CompareTo(p1.IsAvailable(this._follower));
  }

  [CompilerGenerated]
  public int \u003CConfigureItems\u003Eb__21_2(CommandItem p1, CommandItem p2)
  {
    return ((AnimalInteractionModel.AnimalCommandItem) p2).IsAvailable(this._animal).CompareTo(((AnimalInteractionModel.AnimalCommandItem) p1).IsAvailable(this._animal));
  }
}
