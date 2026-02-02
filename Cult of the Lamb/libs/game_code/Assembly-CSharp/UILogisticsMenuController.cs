// Decompiled with JetBrains decompiler
// Type: UILogisticsMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using src.UINavigator;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
public class UILogisticsMenuController : UIMenuBase
{
  [SerializeField]
  public LogisticsMenuItem logisticsMenuItem;
  [SerializeField]
  public GameObject clearSlotPrompt;
  [SerializeField]
  public RectTransform connectingParent;
  [SerializeField]
  public RectTransform connectionsParent;
  [SerializeField]
  public RectTransform connectionsContentParent;
  [SerializeField]
  public RectTransform rootContentParent;
  [SerializeField]
  public RectTransform targetContentParent;
  [SerializeField]
  public RectTransform connectionsContent;
  [SerializeField]
  public RectTransform rootContent;
  [SerializeField]
  public RectTransform targetContent;
  [SerializeField]
  public LogisticsConnectionItem connectionItem;
  [SerializeField]
  public LogisticsMenuItem rootItem;
  [SerializeField]
  public TMP_Text rootHeader;
  [SerializeField]
  public LogisticsMenuItem targetItem;
  [SerializeField]
  public TMP_Text targetHeader;
  [SerializeField]
  public TMP_Text functionalityDescription;
  [SerializeField]
  public MMButton button;
  [SerializeField]
  public CanvasGroup infoCardCanvasGroup;
  [SerializeField]
  public List<UILogisticsMenuController.Category> categories;
  public List<LogisticsMenuItem> rootStructures = new List<LogisticsMenuItem>();
  public List<LogisticsMenuItem> targetStructures = new List<LogisticsMenuItem>();
  public LogisticsMenuItem selectedRoot;
  public LogisticsMenuItem selectedTarget;
  public Structures_Logistics structureBrain;
  public UILogisticsMenuController.State currentState;
  public int slot;
  public LogisticsConnectionItem highlightedMenuItem;
  public List<LogisticsConnectionItem> logisticsConnectionItems = new List<LogisticsConnectionItem>();

  public List<UILogisticsMenuController.Category> Categories => this.categories;

  public void Show(Structures_Logistics structureBrain)
  {
    this.structureBrain = structureBrain;
    this.Show();
  }

  public override void OnShowStarted()
  {
    base.OnShowStarted();
    for (int index = 0; index < this.categories.Count; ++index)
      this.CreateLogisticsMenuItem(this.categories[index].RootStructure, this.rootContent, this.rootStructures);
    if (this.rootStructures.Count > 0)
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this.rootStructures[0].MMButton);
    this.infoCardCanvasGroup.DOComplete();
    this.infoCardCanvasGroup.DOFade(0.0f, 0.0f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    this.ShowConnections();
  }

  public void ShowConnections()
  {
    this.currentState = UILogisticsMenuController.State.Connections;
    this.connectionsParent.gameObject.SetActive(true);
    CanvasGroup component = this.connectionsParent.gameObject.GetComponent<CanvasGroup>();
    component.DOComplete();
    component.alpha = 0.0f;
    component.DOFade(1f, 0.33f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    this.connectionsContentParent.gameObject.SetActive(true);
    this.connectingParent.gameObject.SetActive(false);
    this.rootContentParent.gameObject.SetActive(false);
    this.targetContentParent.gameObject.SetActive(false);
    this.button.transform.parent.gameObject.SetActive(false);
    this.functionalityDescription.text = "";
    this.rootHeader.text = "";
    this.targetHeader.text = "";
    this.rootItem.Image.color = new Color(1f, 1f, 1f, 0.0f);
    this.targetItem.Image.color = new Color(1f, 1f, 1f, 0.0f);
    for (int index = this.logisticsConnectionItems.Count - 1; index > 0; --index)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.logisticsConnectionItems[index].gameObject);
      this.logisticsConnectionItems.RemoveAt(index);
    }
    this.connectionItem.Configure(true);
    this.ConfigureConnectionItem(this.connectionItem);
    for (int index = 0; index < this.structureBrain.Data.AvailableSlots; ++index)
    {
      this.ConfigureConnectionItem(this.logisticsConnectionItems[index]);
      if (index < this.structureBrain.Data.LogisticSlots.Count)
        this.logisticsConnectionItems[index].Configure(this.structureBrain.Data.LogisticSlots[index].RootStructureType, this.structureBrain.Data.LogisticSlots[index].TargetStructureType);
      LogisticsConnectionItem logisticsConnectionItem = UnityEngine.Object.Instantiate<LogisticsConnectionItem>(this.connectionItem, (Transform) this.connectionsContent);
      logisticsConnectionItem.Configure(index + 1 < this.structureBrain.Data.AvailableSlots);
      this.ConfigureConnectionItem(logisticsConnectionItem);
    }
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this.logisticsConnectionItems[0].MMButton);
  }

  public void ConfigureConnectionItem(LogisticsConnectionItem item)
  {
    item.OnHighlighted -= new Action<LogisticsConnectionItem>(this.OnConnectionItemHighlighted);
    item.OnSelected -= new Action<LogisticsConnectionItem>(this.OnConnectionItemSelected);
    item.OnHighlighted += new Action<LogisticsConnectionItem>(this.OnConnectionItemHighlighted);
    item.OnSelected += new Action<LogisticsConnectionItem>(this.OnConnectionItemSelected);
    if (this.logisticsConnectionItems.Contains(item))
      return;
    this.logisticsConnectionItems.Add(item);
  }

  public void ShowRoots()
  {
    this.currentState = UILogisticsMenuController.State.Connecting;
    this.connectingParent.gameObject.SetActive(true);
    CanvasGroup component1 = this.connectingParent.gameObject.GetComponent<CanvasGroup>();
    component1.DOComplete();
    component1.alpha = 0.0f;
    component1.DOFade(1f, 0.33f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    this.rootContentParent.gameObject.SetActive(true);
    CanvasGroup component2 = this.rootContentParent.gameObject.GetComponent<CanvasGroup>();
    component2.DOComplete();
    component2.alpha = 0.0f;
    component2.DOFade(1f, 0.33f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    this.connectionsParent.gameObject.SetActive(false);
    this.targetContentParent.gameObject.SetActive(false);
    this.connectionsContentParent.gameObject.SetActive(false);
    if (this.rootStructures.Count > 0)
    {
      if ((UnityEngine.Object) this.selectedRoot != (UnityEngine.Object) null)
      {
        MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this.selectedRoot.MMButton);
        this.selectedRoot.OnItemHighlighted();
      }
      else
        this.SelectNextAvailableButton(this.rootStructures);
    }
    this.selectedRoot = (LogisticsMenuItem) null;
    this.selectedTarget = (LogisticsMenuItem) null;
  }

  public void ShowTargets()
  {
    this.currentState = UILogisticsMenuController.State.Targeting;
    for (int index = this.targetStructures.Count - 1; index >= 0; --index)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.targetStructures[index].gameObject);
    this.targetStructures.Clear();
    List<StructureBrain.TYPES> dependences = this.GetDependences(this.selectedRoot.StructureType);
    for (int index = 0; index < dependences.Count; ++index)
    {
      bool disabled = false;
      foreach (StructuresData.LogisticsSlot logisticSlot in this.structureBrain.Data.LogisticSlots)
      {
        if (logisticSlot.RootStructureType == this.selectedRoot.StructureType && logisticSlot.TargetStructureType == dependences[index])
          disabled = true;
      }
      this.CreateLogisticsMenuItem(dependences[index], this.targetContent, this.targetStructures, disabled);
    }
    this.rootContentParent.gameObject.SetActive(false);
    this.targetContentParent.gameObject.SetActive(true);
    CanvasGroup component = this.targetContentParent.gameObject.GetComponent<CanvasGroup>();
    component.DOComplete();
    component.alpha = 0.0f;
    component.DOFade(1f, 0.33f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    this.SelectNextAvailableButton(this.targetStructures);
  }

  public void SelectNextAvailableButton(List<LogisticsMenuItem> items)
  {
    for (int index = 0; index < items.Count; ++index)
    {
      if (items[index].MMButton.Interactable)
      {
        MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) items[index].MMButton);
        items[index].OnItemHighlighted();
        return;
      }
    }
    MonoSingleton<UINavigatorNew>.Instance.Clear();
  }

  public void CreateLogisticsMenuItem(
    StructureBrain.TYPES structureType,
    RectTransform content,
    List<LogisticsMenuItem> list,
    bool disabled = false)
  {
    bool flag = true;
    foreach (LogisticsMenuItem logisticsMenuItem in list)
    {
      if (logisticsMenuItem.StructureType == structureType)
        flag = false;
    }
    if (!flag || Interaction_LogisticsBuilding.GetStructures(structureType).Count <= 0)
      return;
    LogisticsMenuItem logisticsMenuItem1 = UnityEngine.Object.Instantiate<LogisticsMenuItem>(this.logisticsMenuItem, (Transform) content);
    logisticsMenuItem1.Configure(structureType, disabled);
    logisticsMenuItem1.OnHighlighted += new Action<LogisticsMenuItem>(this.OnMenuItemHightlighted);
    logisticsMenuItem1.OnSelected += new Action<LogisticsMenuItem>(this.OnMenuItemSelected);
    list.Add(logisticsMenuItem1);
  }

  public void OnMenuItemSelected(LogisticsMenuItem menuItem)
  {
    this.button.transform.parent.gameObject.SetActive(false);
    if ((UnityEngine.Object) this.selectedRoot == (UnityEngine.Object) menuItem)
    {
      this.selectedRoot = (LogisticsMenuItem) null;
      this.OnMenuItemHightlighted(menuItem);
      this.rootHeader.text = "";
      this.targetHeader.text = "";
      this.functionalityDescription.text = "";
      this.targetItem.Image.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
      this.rootContentParent.gameObject.SetActive(true);
      this.targetContentParent.gameObject.SetActive(false);
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) menuItem.MMButton);
    }
    else if ((UnityEngine.Object) this.selectedTarget == (UnityEngine.Object) menuItem)
    {
      this.currentState = UILogisticsMenuController.State.Targeting;
      this.selectedTarget = (LogisticsMenuItem) null;
      this.OnMenuItemHightlighted(menuItem);
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this.targetStructures[0].MMButton);
    }
    else if ((UnityEngine.Object) this.selectedRoot == (UnityEngine.Object) null)
    {
      this.selectedRoot = menuItem;
      this.rootItem.Image.color = Color.white;
      this.ShowTargets();
    }
    else
    {
      this.selectedTarget = menuItem;
      this.targetItem.Image.color = Color.white;
      this.button.transform.parent.gameObject.SetActive(true);
      this.currentState = UILogisticsMenuController.State.Finalising;
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this.button);
    }
  }

  public void OnConnectionItemSelected(LogisticsConnectionItem menuItem)
  {
    int slot = this.logisticsConnectionItems.IndexOf(menuItem);
    this.slot = slot;
    if (menuItem.IsLocked)
    {
      if (this.AddNewConnection(slot))
        menuItem.Configure(true, true);
      else
        menuItem.ShakeLocked();
    }
    else if (menuItem.IsEmpty)
    {
      this.AddNewConnection(slot);
    }
    else
    {
      this.clearSlotPrompt.gameObject.SetActive(false);
      this.ShowRoots();
    }
  }

  public void Update()
  {
    if (!((UnityEngine.Object) this.highlightedMenuItem != (UnityEngine.Object) null) || !InputManager.UI.GetCookButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) || this.currentState != UILogisticsMenuController.State.Connections)
      return;
    for (int index = 0; index < this.structureBrain.Data.LogisticSlots.Count; ++index)
    {
      if (this.structureBrain.Data.LogisticSlots[index].RootStructureType == this.highlightedMenuItem.RootStructureType && this.structureBrain.Data.LogisticSlots[index].TargetStructureType == this.highlightedMenuItem.TargetStructureType)
      {
        this.structureBrain.Data.LogisticSlots.RemoveAt(index);
        this.highlightedMenuItem.Configure(true);
        this.highlightedMenuItem = (LogisticsConnectionItem) null;
        this.clearSlotPrompt.gameObject.SetActive(false);
        AudioManager.Instance.PlayOneShot("event:/dlc/building/commandtent/connection_clear");
        break;
      }
    }
  }

  public void OnConnectionItemHighlighted(LogisticsConnectionItem menuItem)
  {
    this.highlightedMenuItem = (LogisticsConnectionItem) null;
    this.clearSlotPrompt.gameObject.SetActive(false);
    if (menuItem.IsLocked)
    {
      this.infoCardCanvasGroup.DOComplete();
      this.infoCardCanvasGroup.DOFade(0.0f, 0.33f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    }
    else
    {
      if (menuItem.IsEmpty)
        return;
      this.infoCardCanvasGroup.DOComplete();
      this.infoCardCanvasGroup.DOFade(1f, 0.33f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
      this.rootHeader.text = StructuresData.LocalizedName(menuItem.RootStructureType);
      this.targetHeader.text = StructuresData.LocalizedName(menuItem.TargetStructureType);
      this.rootItem.Configure(menuItem.RootStructureType);
      this.targetItem.Configure(menuItem.TargetStructureType);
      this.rootItem.Image.color = new Color(1f, 1f, 1f, 1f);
      this.targetItem.Image.color = new Color(1f, 1f, 1f, 1f);
      this.functionalityDescription.text = LocalizationManager.GetTranslation($"Structures/Logistics/{menuItem.RootStructureType}/{menuItem.TargetStructureType}");
      this.clearSlotPrompt.gameObject.SetActive(true);
      this.highlightedMenuItem = menuItem;
    }
  }

  public void OnMenuItemHightlighted(LogisticsMenuItem menuItem)
  {
    if ((UnityEngine.Object) this.selectedRoot == (UnityEngine.Object) menuItem || (UnityEngine.Object) this.selectedTarget == (UnityEngine.Object) menuItem)
      return;
    if ((UnityEngine.Object) this.selectedRoot == (UnityEngine.Object) null)
    {
      this.infoCardCanvasGroup.DOComplete();
      this.infoCardCanvasGroup.DOFade(0.0f, 0.33f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
      this.rootItem.Configure(menuItem.StructureType);
      this.rootItem.Image.color = new Color(1f, 1f, 1f, 0.5f);
      this.targetItem.Image.color = new Color(1f, 1f, 1f, 0.0f);
      this.rootHeader.text = StructuresData.LocalizedName(menuItem.StructureType);
      this.functionalityDescription.text = "";
    }
    else
    {
      this.infoCardCanvasGroup.DOComplete();
      this.infoCardCanvasGroup.DOFade(1f, 0.33f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
      this.functionalityDescription.text = LocalizationManager.GetTranslation($"Structures/Logistics/{this.selectedRoot.StructureType}/{menuItem.StructureType}");
      this.targetItem.Configure(menuItem.StructureType);
      this.targetItem.Image.color = new Color(1f, 1f, 1f, 0.5f);
      this.targetHeader.text = StructuresData.LocalizedName(menuItem.StructureType);
    }
  }

  public List<StructureBrain.TYPES> GetDependences(StructureBrain.TYPES structureType)
  {
    List<StructureBrain.TYPES> dependences = new List<StructureBrain.TYPES>();
    foreach (UILogisticsMenuController.Category category in this.categories)
    {
      if (category.RootStructure == structureType && !dependences.Contains(category.TargetStructure))
        dependences.Add(category.TargetStructure);
    }
    return dependences;
  }

  public void ConfirmLogistics()
  {
    if (this.slot >= this.structureBrain.Data.LogisticSlots.Count)
    {
      this.structureBrain.Data.LogisticSlots.Add(new StructuresData.LogisticsSlot());
      this.slot = this.structureBrain.Data.LogisticSlots.Count - 1;
    }
    AudioManager.Instance.PlayOneShot("event:/dlc/building/commandtent/connection_confirm");
    this.structureBrain.Data.LogisticSlots[this.slot].RootStructureType = this.selectedRoot.StructureType;
    this.structureBrain.Data.LogisticSlots[this.slot].TargetStructureType = this.selectedTarget.StructureType;
    this.logisticsConnectionItems[this.slot].Configure(true);
    this.logisticsConnectionItems[this.slot].Configure(this.selectedRoot.StructureType, this.selectedTarget.StructureType);
    this.ShowConnections();
  }

  public override void OnCancelButtonInput()
  {
    if (this.currentState == UILogisticsMenuController.State.Connections)
    {
      this.Hide();
      UIManager.PlayAudio("event:/ui/close_menu");
    }
    else if (this.currentState == UILogisticsMenuController.State.Connecting)
      this.ShowConnections();
    else if (this.currentState == UILogisticsMenuController.State.Targeting)
    {
      this.ShowRoots();
    }
    else
    {
      if (this.currentState != UILogisticsMenuController.State.Finalising)
        return;
      if ((UnityEngine.Object) this.selectedTarget != (UnityEngine.Object) null)
        this.OnMenuItemSelected(this.selectedTarget);
      else
        this.OnMenuItemSelected(this.selectedRoot);
    }
  }

  public bool AddNewConnection(int slot)
  {
    if (slot >= this.structureBrain.Data.AvailableSlots)
    {
      if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.YEW_HOLY) < 5)
        return false;
      Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.YEW_HOLY, -5);
      ++this.structureBrain.Data.AvailableSlots;
      LogisticsConnectionItem logisticsConnectionItem = UnityEngine.Object.Instantiate<LogisticsConnectionItem>(this.connectionItem, (Transform) this.connectionsContent);
      logisticsConnectionItem.Configure(false);
      this.ConfigureConnectionItem(logisticsConnectionItem);
    }
    else
      this.ShowRoots();
    return true;
  }

  public override void OnHideCompleted()
  {
    base.OnHideCompleted();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public enum State
  {
    Connections,
    Connecting,
    Targeting,
    Finalising,
  }

  [Serializable]
  public struct Category
  {
    public StructureBrain.TYPES RootStructure;
    public StructureBrain.TYPES TargetStructure;
  }
}
