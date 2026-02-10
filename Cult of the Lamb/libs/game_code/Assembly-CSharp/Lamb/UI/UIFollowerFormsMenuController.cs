// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIFollowerFormsMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI.Alerts;
using src.Extensions;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UIFollowerFormsMenuController : UIMenuBase
{
  [Header("Follower Forms menu")]
  [SerializeField]
  public IndoctrinationFormItem _formItemTemplate;
  [SerializeField]
  public MMScrollRect _scrollRect;
  [Header("Content")]
  [SerializeField]
  public RectTransform _miscContent;
  [SerializeField]
  public RectTransform _specialEventsContent;
  [SerializeField]
  public RectTransform _dlcContent;
  [SerializeField]
  public RectTransform _majorDlcContent;
  [SerializeField]
  public RectTransform _darkwoodContent;
  [SerializeField]
  public RectTransform _anuraContent;
  [SerializeField]
  public RectTransform _anchorDeepContent;
  [SerializeField]
  public RectTransform _silkCradleContent;
  [SerializeField]
  public RectTransform _dungeon5Content;
  [SerializeField]
  public RectTransform _dungeon6Content;
  [Header("Counts")]
  [SerializeField]
  public TextMeshProUGUI _miscUnlocked;
  [SerializeField]
  public TextMeshProUGUI _specialEventsUnlocked;
  [SerializeField]
  public TextMeshProUGUI _dlcUnlocked;
  [SerializeField]
  public TextMeshProUGUI _majorDlcUnlocked;
  [SerializeField]
  public TextMeshProUGUI _darkwoodUnlocked;
  [SerializeField]
  public TextMeshProUGUI _anuraUnlocked;
  [SerializeField]
  public TextMeshProUGUI _anchorDeepUnlocked;
  [SerializeField]
  public TextMeshProUGUI _silkCradleUnlocked;
  [SerializeField]
  public TextMeshProUGUI _dungeon5Unlocked;
  [SerializeField]
  public TextMeshProUGUI _dungeon6Unlocked;
  [Header("Headers")]
  [SerializeField]
  public GameObject _dlcHeader;
  [SerializeField]
  public GameObject _specialEventsHeader;
  [Header("Woolhaven DLC")]
  [SerializeField]
  public TextMeshProUGUI _requiresDLCText;
  [SerializeField]
  public AlertBadgeBase _requiresDLCAlert;
  public List<IndoctrinationFormItem> _formItems = new List<IndoctrinationFormItem>();
  public static string[] DLCOrder = new string[25]
  {
    "Cthulhu",
    "Bee",
    "Tapir",
    "Turtle",
    "Monkey",
    "Narwal",
    "Moose",
    "Gorilla",
    "Mosquito",
    "Goldfish",
    "Possum",
    "Hammerhead",
    "Llama",
    "Tiger",
    "Sphynx",
    "LadyBug",
    "TwitchMouse",
    "TwitchCat",
    "TwitchDog",
    "TwitchDogAlt",
    "TwitchPoggers",
    "Lion",
    "Penguin",
    "Pelican",
    "Kiwi"
  };
  public static string[] MiscOrder = new string[21]
  {
    "Deer",
    "Pig",
    "Dog",
    "Cat",
    "Fox",
    "Night Wolf",
    "Fish",
    "Pangolin",
    "Shrew",
    "Unicorn",
    "Axolotl",
    "Starfish",
    "Red Panda",
    "Poop",
    "Massive Monster",
    "Crab",
    "Snail",
    "Owl",
    "Butterfly",
    "Koala",
    "Shrimp"
  };
  public static string[] DarkwoodOrder = new string[12]
  {
    "Cow",
    "Horse",
    "Deer_ritual",
    "Hedgehog",
    "Rabbit",
    "Chicken",
    "Squirrel",
    "Boss Mama Worm",
    "Boss Mama Maggot",
    "Boss Burrow Worm",
    "Boss Beholder 1",
    "CultLeader 1"
  };
  public static string[] AnuraOrder = new string[12]
  {
    "Giraffe",
    "Bison",
    "Frog",
    "Capybara",
    "Fennec Fox",
    "Rhino",
    "Eagle",
    "Boss Flying Burp Frog",
    "Boss Egg Hopper",
    "Boss Mortar Hopper",
    "Boss Beholder 2",
    "CultLeader 2"
  };
  public static string[] AnchordeepOrder = new string[11]
  {
    "Crocodile",
    "Elephant",
    "Hippo",
    "Otter",
    "Seahorse",
    "Duck",
    "Boss Spiker",
    "Boss Charger",
    "Boss Scuttle Turret",
    "Boss Beholder 3",
    "CultLeader 3"
  };
  public static string[] SilkCradleOrder = new string[10]
  {
    "Bear",
    "Bat",
    "Beetle",
    "Raccoon",
    "Badger",
    "Boss Spider Jump",
    "Boss Millipede Poisoner",
    "Boss Scorpion",
    "Boss Beholder 4",
    "CultLeader 4"
  };
  public static string[] Dungeon5Order = new string[1]
  {
    "null"
  };
  public static string[] Dungeon6Order = new string[1]
  {
    "null"
  };
  public static string[] SpecialEventsOrder = new string[4]
  {
    "Crow",
    "DeerSkull",
    "BatDemon",
    "StarBunny"
  };

  public override void OnShowStarted()
  {
    UIManager.PlayAudio("event:/followers/appearance_menu_appear");
    this._scrollRect.normalizedPosition = Vector2.one;
    this._scrollRect.enabled = false;
    if (this._formItems.Count == 0)
    {
      this._scrollRect.enabled = false;
      bool flag1 = false;
      foreach (string dlcSkin in DataManager.Instance.DLCSkins)
      {
        if (DataManager.GetFollowerSkinUnlocked(dlcSkin))
        {
          flag1 = true;
          break;
        }
      }
      this._dlcContent.gameObject.SetActive(flag1);
      this._dlcHeader.SetActive(flag1);
      bool flag2 = false;
      foreach (string specialEventSkin in DataManager.Instance.SpecialEventSkins)
      {
        if (DataManager.GetFollowerSkinUnlocked(specialEventSkin))
        {
          flag2 = true;
          break;
        }
      }
      this._specialEventsContent.gameObject.SetActive(flag2);
      this._specialEventsHeader.SetActive(flag2);
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.Other), this._miscContent, UIFollowerFormsMenuController.MiscOrder));
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.SpecialEvents), this._specialEventsContent, UIFollowerFormsMenuController.SpecialEventsOrder));
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.Dungeon1), this._darkwoodContent, UIFollowerFormsMenuController.DarkwoodOrder));
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.Dungeon2), this._anuraContent, UIFollowerFormsMenuController.AnuraOrder));
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.Dungeon3), this._anchorDeepContent, UIFollowerFormsMenuController.AnchordeepOrder));
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.Dungeon4), this._silkCradleContent, UIFollowerFormsMenuController.SilkCradleOrder));
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.DLC), this._dlcContent, UIFollowerFormsMenuController.DLCOrder));
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.Major_DLC), this._majorDlcContent, UIFollowerFormsMenuController.DLCOrder));
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.Dungeon5), this._dungeon5Content, UIFollowerFormsMenuController.Dungeon5Order));
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.Dungeon6), this._dungeon6Content, UIFollowerFormsMenuController.Dungeon6Order));
      this.SetUnlockedText(this._miscUnlocked, WorshipperData.DropLocation.Other);
      this.SetUnlockedText(this._specialEventsUnlocked, WorshipperData.DropLocation.SpecialEvents);
      this.SetUnlockedText(this._darkwoodUnlocked, WorshipperData.DropLocation.Dungeon1);
      this.SetUnlockedText(this._anuraUnlocked, WorshipperData.DropLocation.Dungeon2);
      this.SetUnlockedText(this._anchorDeepUnlocked, WorshipperData.DropLocation.Dungeon3);
      this.SetUnlockedText(this._silkCradleUnlocked, WorshipperData.DropLocation.Dungeon4);
      this.SetUnlockedText(this._dlcUnlocked, WorshipperData.DropLocation.DLC);
      this.SetUnlockedText(this._majorDlcUnlocked, WorshipperData.DropLocation.Major_DLC);
      this.SetUnlockedText(this._dungeon5Unlocked, WorshipperData.DropLocation.Dungeon5);
      this.SetUnlockedText(this._dungeon6Unlocked, WorshipperData.DropLocation.Dungeon6);
      foreach (IndoctrinationFormItem formItem in this._formItems)
      {
        formItem.SetAsDefault();
        formItem.Button.OnConfirmDenied = (System.Action) null;
        formItem.Button.Confirmable = false;
        formItem.Button._confirmDeniedSFX = "";
      }
    }
    this._scrollRect.enabled = true;
    this.OverrideDefaultOnce((Selectable) this._formItems[0].Button);
    this.ActivateNavigation();
  }

  public List<IndoctrinationFormItem> Populate(
    List<WorshipperData.SkinAndData> types,
    RectTransform contentContainer,
    string[] order = null)
  {
    if (order != null)
      types.Sort((Comparison<WorshipperData.SkinAndData>) ((a, b) => order.IndexOf<string>(a.Title).CompareTo(order.IndexOf<string>(b.Title))));
    List<IndoctrinationFormItem> indoctrinationFormItemList = new List<IndoctrinationFormItem>();
    foreach (WorshipperData.SkinAndData type in types)
    {
      IndoctrinationFormItem indoctrinationFormItem = this._formItemTemplate.Instantiate<IndoctrinationFormItem>((Transform) contentContainer);
      indoctrinationFormItem.Configure(type);
      indoctrinationFormItem.Button.Confirmable = false;
      this._formItems.Add(indoctrinationFormItem);
    }
    return indoctrinationFormItemList;
  }

  public void SetUnlockedText(TextMeshProUGUI target, WorshipperData.DropLocation dropLocation)
  {
    int num = 0;
    List<WorshipperData.SkinAndData> skinsFromLocation = WorshipperData.Instance.GetSkinsFromLocation(dropLocation);
    foreach (WorshipperData.SkinAndData skinAndData in skinsFromLocation)
    {
      if (DataManager.GetFollowerSkinUnlocked(skinAndData.Skin[0].Skin))
        ++num;
    }
    target.text = string.Format(LocalizationManager.GetTranslation("UI/Collected"), (object) $"{num}/{skinsFromLocation.Count}");
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  public override void OnHideStarted()
  {
    this._scrollRect.enabled = false;
    UIManager.PlayAudio("event:/upgrade_statue/upgrade_statue_close");
  }

  public override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
}
