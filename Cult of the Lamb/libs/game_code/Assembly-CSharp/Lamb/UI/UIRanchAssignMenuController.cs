// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIRanchAssignMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src.UINavigator;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class UIRanchAssignMenuController : UIMenuBase
{
  public Action<StructuresData.Ranchable_Animal> OnAssigned;
  [SerializeField]
  public RanchAssignMenuItem ranchAssignMenuItem;
  [SerializeField]
  public RanchAssignMenuInfoCardController infoCardController;
  [SerializeField]
  public RectTransform content;
  [SerializeField]
  public RectTransform contentNonWool;
  public List<RanchAssignMenuItem> assignItems = new List<RanchAssignMenuItem>();
  public Structures_Ranch structureBrain;

  public void Show(Structures_Ranch structureBrain)
  {
    this.structureBrain = structureBrain;
    this.infoCardController.gameObject.SetActive(true);
    this.Show();
  }

  public override void OnShowStarted()
  {
    base.OnShowStarted();
    for (int index = 0; index < InventoryItem.AllAnimals.Count; ++index)
    {
      if (Interaction_Ranchable.IsWoolWorkLoot(InventoryItem.AllAnimals[index]))
        this.CreateRanchAssignMenuItem(InventoryItem.AllAnimals[index], this.content, this.assignItems, !AnimalData.HasDiscoveredAnimal(InventoryItem.AllAnimals[index]));
      else
        this.CreateRanchAssignMenuItem(InventoryItem.AllAnimals[index], this.contentNonWool, this.assignItems, !AnimalData.HasDiscoveredAnimal(InventoryItem.AllAnimals[index]));
    }
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this.assignItems[0].Button);
  }

  public void CreateRanchAssignMenuItem(
    InventoryItem.ITEM_TYPE animalType,
    RectTransform content,
    List<RanchAssignMenuItem> list,
    bool disabled = false)
  {
    bool flag = true;
    foreach (RanchAssignMenuItem ranchAssignMenuItem in list)
    {
      if (ranchAssignMenuItem.AnimalType == animalType)
        flag = false;
    }
    if (!flag)
      return;
    RanchAssignMenuItem ranchAssignMenuItem1 = UnityEngine.Object.Instantiate<RanchAssignMenuItem>(this.ranchAssignMenuItem, (Transform) content);
    ranchAssignMenuItem1.Configure(animalType, disabled);
    ranchAssignMenuItem1.OnHighlighted += new Action<RanchAssignMenuItem>(this.OnMenuItemHightlighted);
    ranchAssignMenuItem1.OnSelected += new Action<RanchAssignMenuItem>(this.OnMenuItemSelected);
    list.Add(ranchAssignMenuItem1);
  }

  public void OnMenuItemSelected(RanchAssignMenuItem menuItem)
  {
    this.Hide(true);
    Action<StructuresData.Ranchable_Animal> onAssigned = this.OnAssigned;
    if (onAssigned == null)
      return;
    onAssigned(menuItem.DummyAnimal);
  }

  public void OnMenuItemHightlighted(RanchAssignMenuItem menuItem)
  {
  }

  public override void OnCancelButtonInput()
  {
    if (!this.CanvasGroup.interactable)
      return;
    this.CanvasGroup.interactable = false;
    base.OnCancelButtonInput();
    this.Hide();
    AudioManager.Instance.PlayOneShot("event:/ui/close_menu");
  }

  public override void OnHideCompleted()
  {
    base.OnHideCompleted();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }
}
