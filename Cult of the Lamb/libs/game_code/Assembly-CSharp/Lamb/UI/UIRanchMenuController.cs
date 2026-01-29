// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIRanchMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class UIRanchMenuController : UIMenuBase
{
  public Action<StructuresData.Ranchable_Animal> OnAddAnimal;
  [SerializeField]
  public RanchMenuItem ranchMenuItem;
  [SerializeField]
  public RectTransform content;
  [SerializeField]
  public RectTransform overCapacityContent;
  [SerializeField]
  public RectTransform overCapacityText;
  [SerializeField]
  public RectTransform divider;
  [Header("Ranch Size & Capacity")]
  [SerializeField]
  public RectTransform ranchSize;
  [SerializeField]
  public TextMeshProUGUI ranchSizeText;
  [SerializeField]
  public TextMeshProUGUI capacityText;
  [Header("Ranch Rituals")]
  [SerializeField]
  public GameObject ritualBox;
  [SerializeField]
  public GameObject harvestRitualNotifcation;
  [SerializeField]
  public GameObject meatRitualNotifcation;
  [SerializeField]
  public RanchMenuInfoCardController infoCardController;
  [SerializeField]
  public GameObject acceptPrompt;
  public Structures_Ranch structuresBrain;
  public StructuresData structuresData;
  public bool didCancel;
  public List<RanchMenuItem> items = new List<RanchMenuItem>();

  public void Show(Structures_Ranch structuresBrain, StructuresData structuresData, bool instant = false)
  {
    this.structuresBrain = structuresBrain;
    this.structuresData = structuresData;
    this.Show(instant);
  }

  public override void OnShowStarted()
  {
    base.OnShowStarted();
    this.UpdateCapacity();
    this.CreateRanchMenuItems();
    this.CreateEmptyItem();
    this.UpdateActiveRituals();
    this.acceptPrompt.gameObject.SetActive(false);
    this.StartCoroutine((IEnumerator) this.PickDefaultSelectable());
  }

  public IEnumerator PickDefaultSelectable()
  {
    bool foundone = false;
    yield return (object) new WaitForSecondsRealtime(0.2f);
    foreach (RanchMenuItem ranchMenuItem in this.items)
    {
      if (ranchMenuItem.AnimalInfo == null)
      {
        MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) ranchMenuItem.Button);
        foundone = true;
        break;
      }
    }
    if (!foundone)
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this.items[0].Button);
  }

  public void CreateRanchMenuItems()
  {
    this.overCapacityContent.gameObject.SetActive(false);
    foreach (StructuresData.Ranchable_Animal animal in this.structuresData.Animals)
    {
      if (animal.State != Interaction_Ranchable.State.Dead)
      {
        RanchMenuItem ranchMenuItem;
        if (this.items.Count < this.structuresBrain.Capacity)
        {
          ranchMenuItem = UnityEngine.Object.Instantiate<RanchMenuItem>(this.ranchMenuItem, (Transform) this.content);
        }
        else
        {
          ranchMenuItem = UnityEngine.Object.Instantiate<RanchMenuItem>(this.ranchMenuItem, (Transform) this.overCapacityContent);
          this.overCapacityContent.gameObject.SetActive(true);
        }
        ranchMenuItem.Configure(animal);
        ranchMenuItem.OnHighlighted += new Action<RanchMenuItem>(this.OnMenuItemHightlighted);
        ranchMenuItem.OnSelected += new Action<RanchMenuItem>(this.OnMenuItemSelected);
        this.items.Add(ranchMenuItem);
      }
    }
    this.items = this.items.OrderBy<RanchMenuItem, int>((Func<RanchMenuItem, int>) (x => x.AnimalInfo.Age)).ToList<RanchMenuItem>();
    for (int index = 0; index < this.items.Count; ++index)
      this.items[index].transform.SetSiblingIndex(index);
  }

  public void CreateEmptyItem()
  {
    if (this.items.Count > this.structuresBrain.Capacity)
      return;
    RanchMenuItem ranchMenuItem1 = UnityEngine.Object.Instantiate<RanchMenuItem>(this.ranchMenuItem, (Transform) this.content);
    ranchMenuItem1.Configure((StructuresData.Ranchable_Animal) null);
    ranchMenuItem1.OnHighlighted += new Action<RanchMenuItem>(this.OnEmptyMenuItemHightlighted);
    ranchMenuItem1.OnSelected += new Action<RanchMenuItem>(this.OnEmptyMenuItemSelected);
    this.items.Add(ranchMenuItem1);
    foreach (RanchMenuItem ranchMenuItem2 in this.items)
    {
      if (ranchMenuItem2.AnimalInfo == null)
        ranchMenuItem2.transform.SetAsFirstSibling();
    }
  }

  public void OnMenuItemSelected(RanchMenuItem item)
  {
  }

  public void OnMenuItemHightlighted(RanchMenuItem item)
  {
    this.infoCardController.ShowCardWithParam(item);
    this.acceptPrompt.gameObject.SetActive(false);
  }

  public void OnEmptyMenuItemSelected(RanchMenuItem item)
  {
    UIRanchAssignMenuController menu = MonoSingleton<UIManager>.Instance.ShowRanchAssignMenu(this.structuresBrain);
    this.PushInstance<UIRanchAssignMenuController>(menu);
    menu.OnAssigned += this.OnAddAnimal;
  }

  public void OnEmptyMenuItemHightlighted(RanchMenuItem item)
  {
    this.acceptPrompt.gameObject.SetActive(true);
  }

  public override void OnCancelButtonInput()
  {
    this.didCancel = true;
    if (!this._canvasGroup.interactable)
      return;
    AudioManager.Instance.PlayOneShot("event:/ui/close_menu");
    this.Hide();
  }

  public void UpdateCapacity()
  {
    int num;
    if (this.structuresBrain.IsOvercrowded)
    {
      this.capacityText.text = $"<color=#FF0000>{this.structuresData.Animals.Count.ToString()}/{this.structuresBrain.Capacity.ToString()}</color>";
    }
    else
    {
      TextMeshProUGUI capacityText = this.capacityText;
      num = this.structuresData.Animals.Count;
      string str1 = num.ToString();
      num = this.structuresBrain.Capacity;
      string str2 = num.ToString();
      string str3 = $"{str1}/{str2}";
      capacityText.text = str3;
    }
    this.overCapacityText.gameObject.SetActive(this.structuresBrain.IsOvercrowded);
    TextMeshProUGUI ranchSizeText = this.ranchSizeText;
    num = this.structuresBrain.RanchingTiles.Count;
    string str = num.ToString();
    ranchSizeText.text = str;
  }

  public void UpdateActiveRituals()
  {
    bool flag = false;
    if (FollowerBrainStats.IsRanchHarvest)
    {
      this.harvestRitualNotifcation.SetActive(true);
      flag = true;
    }
    else
      this.harvestRitualNotifcation.SetActive(false);
    if (FollowerBrainStats.IsRanchMeat)
    {
      this.meatRitualNotifcation.SetActive(true);
      flag = true;
    }
    else
      this.meatRitualNotifcation.SetActive(false);
    this.ritualBox.gameObject.SetActive(flag);
  }

  public override void OnHideCompleted()
  {
    if (this.didCancel)
    {
      System.Action onCancel = this.OnCancel;
      if (onCancel != null)
        onCancel();
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }
}
