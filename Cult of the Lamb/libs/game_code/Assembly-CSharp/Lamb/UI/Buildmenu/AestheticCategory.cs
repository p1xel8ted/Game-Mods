// Decompiled with JetBrains decompiler
// Type: Lamb.UI.BuildMenu.AestheticCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI.BuildMenu;

public class AestheticCategory : BuildMenuCategory
{
  [Header("Content")]
  [SerializeField]
  public RectTransform _miscContent;
  [SerializeField]
  public RectTransform _dlcContent;
  [SerializeField]
  public RectTransform _majorDlcContent;
  [SerializeField]
  public RectTransform _majorDlcWoolhavenContent;
  [SerializeField]
  public RectTransform _majorDlcEwefallContent;
  [SerializeField]
  public RectTransform _majorDlcRotContent;
  [SerializeField]
  public RectTransform _specialEventsContent;
  [SerializeField]
  public RectTransform _pathsContent;
  [SerializeField]
  public RectTransform _darkwoodContent;
  [SerializeField]
  public RectTransform _anuraContent;
  [SerializeField]
  public RectTransform _anchorDeepContent;
  [SerializeField]
  public RectTransform _silkCradleContent;
  [Header("Counts")]
  [SerializeField]
  public TextMeshProUGUI _miscUnlocked;
  [SerializeField]
  public TextMeshProUGUI _dlcUnlocked;
  [SerializeField]
  public TextMeshProUGUI _majorDlcUnlocked;
  [SerializeField]
  public TextMeshProUGUI _majorDlcWoolhavenUnlocked;
  [SerializeField]
  public TextMeshProUGUI _majorDlcEwefallUnlocked;
  [SerializeField]
  public TextMeshProUGUI _majorDlcRotUnlocked;
  [SerializeField]
  public TextMeshProUGUI _specialEventsUnlocked;
  [SerializeField]
  public TextMeshProUGUI _pathsUnlocked;
  [SerializeField]
  public TextMeshProUGUI _darkwoodUnlocked;
  [SerializeField]
  public TextMeshProUGUI _anuraUnlocked;
  [SerializeField]
  public TextMeshProUGUI _anchorDeepUnlocked;
  [SerializeField]
  public TextMeshProUGUI _silkCradleUnlocked;
  [SerializeField]
  public GameObject _dlcHeader;
  [SerializeField]
  public GameObject _majorDlcHeader;
  [SerializeField]
  public GameObject _majorDlcWoolhavenHeader;
  [SerializeField]
  public GameObject _majorDlcEwefallHeader;
  [SerializeField]
  public GameObject _majorDlcRotHeader;
  [SerializeField]
  public GameObject _specialEventsHeader;

  public override void Populate()
  {
    bool flag1 = false;
    foreach (StructureBrain.TYPES Types in DataManager.DecorationsForType(DataManager.DecorationType.DLC))
    {
      if (StructuresData.GetUnlocked(Types))
      {
        flag1 = true;
        break;
      }
    }
    bool flag2 = false;
    foreach (StructureBrain.TYPES Types in DataManager.DecorationsForType(DataManager.DecorationType.Special_Events))
    {
      if (StructuresData.GetUnlocked(Types))
      {
        flag2 = true;
        break;
      }
    }
    bool flag3 = false;
    foreach (StructureBrain.TYPES Types in DataManager.MajorDLCDecorationsDisplay(DataManager.DecorationMajorDLCGrouping.Major_DLC))
    {
      if (StructuresData.GetUnlocked(Types))
      {
        flag3 = true;
        break;
      }
    }
    bool flag4 = false;
    foreach (StructureBrain.TYPES Types in DataManager.MajorDLCDecorationsDisplay(DataManager.DecorationMajorDLCGrouping.Woolhaven))
    {
      if (StructuresData.GetUnlocked(Types))
      {
        flag4 = true;
        break;
      }
    }
    bool flag5 = false;
    foreach (StructureBrain.TYPES Types in DataManager.MajorDLCDecorationsDisplay(DataManager.DecorationMajorDLCGrouping.Ewefall))
    {
      if (StructuresData.GetUnlocked(Types))
      {
        flag5 = true;
        break;
      }
    }
    bool flag6 = false;
    foreach (StructureBrain.TYPES Types in DataManager.MajorDLCDecorationsDisplay(DataManager.DecorationMajorDLCGrouping.Rot))
    {
      if (StructuresData.GetUnlocked(Types))
      {
        flag6 = true;
        break;
      }
    }
    this._dlcContent.gameObject.SetActive(flag1);
    this._dlcHeader.gameObject.SetActive(flag1);
    this._majorDlcContent.gameObject.SetActive(flag3);
    this._majorDlcHeader.gameObject.SetActive(flag3);
    this._majorDlcWoolhavenContent.gameObject.SetActive(flag4);
    this._majorDlcWoolhavenHeader.gameObject.SetActive(flag4);
    this._majorDlcEwefallContent.gameObject.SetActive(flag5);
    this._majorDlcEwefallHeader.gameObject.SetActive(flag5);
    this._majorDlcRotContent.gameObject.SetActive(flag6);
    this._majorDlcRotHeader.gameObject.SetActive(flag6);
    this._specialEventsContent.gameObject.SetActive(flag2);
    this._specialEventsHeader.gameObject.SetActive(flag2);
    this.Populate(DataManager.DecorationsForType(DataManager.DecorationType.All), this._miscContent);
    this.Populate(DataManager.MajorDLCDecorationsDisplay(DataManager.DecorationMajorDLCGrouping.Major_DLC), this._majorDlcContent);
    this.Populate(DataManager.MajorDLCDecorationsDisplay(DataManager.DecorationMajorDLCGrouping.Woolhaven), this._majorDlcWoolhavenContent);
    this.Populate(DataManager.MajorDLCDecorationsDisplay(DataManager.DecorationMajorDLCGrouping.Ewefall), this._majorDlcEwefallContent);
    this.Populate(DataManager.MajorDLCDecorationsDisplay(DataManager.DecorationMajorDLCGrouping.Rot), this._majorDlcRotContent);
    this.Populate(DataManager.DecorationsForType(DataManager.DecorationType.DLC), this._dlcContent);
    this.Populate(DataManager.DecorationsForType(DataManager.DecorationType.Special_Events), this._specialEventsContent);
    this.Populate(DataManager.DecorationsForType(DataManager.DecorationType.Path), this._pathsContent);
    this.Populate(DataManager.DecorationsForType(DataManager.DecorationType.Dungeon1), this._darkwoodContent);
    this.Populate(DataManager.DecorationsForType(DataManager.DecorationType.Mushroom), this._anuraContent);
    this.Populate(DataManager.DecorationsForType(DataManager.DecorationType.Crystal), this._anchorDeepContent);
    this.Populate(DataManager.DecorationsForType(DataManager.DecorationType.Spider), this._silkCradleContent);
    this.SetUnlockedText(this._miscUnlocked, DataManager.DecorationType.All);
    this.SetUnlockedTextMajorDLC(this._majorDlcUnlocked, DataManager.DecorationMajorDLCGrouping.Major_DLC);
    this.SetUnlockedTextMajorDLC(this._majorDlcWoolhavenUnlocked, DataManager.DecorationMajorDLCGrouping.Woolhaven);
    this.SetUnlockedTextMajorDLC(this._majorDlcEwefallUnlocked, DataManager.DecorationMajorDLCGrouping.Ewefall);
    this.SetUnlockedTextMajorDLC(this._majorDlcRotUnlocked, DataManager.DecorationMajorDLCGrouping.Rot);
    this.SetUnlockedText(this._dlcUnlocked, DataManager.DecorationType.DLC);
    this.SetUnlockedText(this._specialEventsUnlocked, DataManager.DecorationType.Special_Events);
    this.SetUnlockedText(this._pathsUnlocked, DataManager.DecorationType.Path);
    this.SetUnlockedText(this._darkwoodUnlocked, DataManager.DecorationType.Dungeon1);
    this.SetUnlockedText(this._anuraUnlocked, DataManager.DecorationType.Mushroom);
    this.SetUnlockedText(this._anchorDeepUnlocked, DataManager.DecorationType.Crystal);
    this.SetUnlockedText(this._silkCradleUnlocked, DataManager.DecorationType.Spider);
  }

  public void SetUnlockedText(TextMeshProUGUI target, DataManager.DecorationType decorationType)
  {
    int num1 = 0;
    int num2 = 0;
    foreach (StructureBrain.TYPES types in DataManager.DecorationsForType(decorationType))
    {
      if (StructuresData.GetUnlocked(types))
        ++num1;
      if (StructuresData.HiddenUntilUnlocked(types))
      {
        if (StructuresData.GetUnlocked(types))
          ++num2;
      }
      else
        ++num2;
    }
    string str = LocalizeIntegration.ReverseText($"{num1}/{num2}");
    target.text = string.Format(ScriptLocalization.UI.Collected, (object) str);
  }

  public void SetUnlockedTextMajorDLC(
    TextMeshProUGUI target,
    DataManager.DecorationMajorDLCGrouping decorationMajorDLCGrouping)
  {
    int num1 = 0;
    int num2 = 0;
    foreach (StructureBrain.TYPES types in DataManager.MajorDLCDecorationsDisplay(decorationMajorDLCGrouping))
    {
      if (StructuresData.GetUnlocked(types))
        ++num1;
      if (StructuresData.HiddenUntilUnlocked(types))
      {
        if (StructuresData.GetUnlocked(types))
          ++num2;
      }
      else
        ++num2;
    }
    string str = LocalizeIntegration.ReverseText($"{num1}/{num2}");
    target.text = string.Format(ScriptLocalization.UI.Collected, (object) str);
  }

  public static List<StructureBrain.TYPES> AllStructures()
  {
    List<StructureBrain.TYPES> typesList = new List<StructureBrain.TYPES>();
    typesList.AddRange((IEnumerable<StructureBrain.TYPES>) DataManager.DecorationsForType(DataManager.DecorationType.All));
    typesList.AddRange((IEnumerable<StructureBrain.TYPES>) DataManager.DecorationsForType(DataManager.DecorationType.DLC));
    typesList.AddRange((IEnumerable<StructureBrain.TYPES>) DataManager.DecorationsForType(DataManager.DecorationType.Major_DLC));
    typesList.AddRange((IEnumerable<StructureBrain.TYPES>) DataManager.DecorationsForType(DataManager.DecorationType.Special_Events));
    typesList.AddRange((IEnumerable<StructureBrain.TYPES>) DataManager.DecorationsForType(DataManager.DecorationType.Path));
    typesList.AddRange((IEnumerable<StructureBrain.TYPES>) DataManager.DecorationsForType(DataManager.DecorationType.Dungeon1));
    typesList.AddRange((IEnumerable<StructureBrain.TYPES>) DataManager.DecorationsForType(DataManager.DecorationType.Mushroom));
    typesList.AddRange((IEnumerable<StructureBrain.TYPES>) DataManager.DecorationsForType(DataManager.DecorationType.Crystal));
    typesList.AddRange((IEnumerable<StructureBrain.TYPES>) DataManager.DecorationsForType(DataManager.DecorationType.Spider));
    return typesList;
  }

  public void ShowMajorDLCCategory()
  {
    this._majorDlcContent.gameObject.SetActive(true);
    this._majorDlcHeader.gameObject.SetActive(true);
    this.SetUnlockedTextMajorDLC(this._majorDlcUnlocked, DataManager.DecorationMajorDLCGrouping.Major_DLC);
  }

  public void ShowMajorDLCWoolhavenCategory()
  {
    this._majorDlcWoolhavenContent.gameObject.SetActive(true);
    this._majorDlcWoolhavenHeader.gameObject.SetActive(true);
    this.SetUnlockedTextMajorDLC(this._majorDlcWoolhavenUnlocked, DataManager.DecorationMajorDLCGrouping.Woolhaven);
  }

  public void ShowMajorDLCEwefallCategory()
  {
    this._majorDlcEwefallContent.gameObject.SetActive(true);
    this._majorDlcEwefallHeader.gameObject.SetActive(true);
    this.SetUnlockedTextMajorDLC(this._majorDlcEwefallUnlocked, DataManager.DecorationMajorDLCGrouping.Ewefall);
  }

  public void ShowMajorDLCRotCategory()
  {
    this._majorDlcRotContent.gameObject.SetActive(true);
    this._majorDlcRotHeader.gameObject.SetActive(true);
    this.SetUnlockedTextMajorDLC(this._majorDlcRotUnlocked, DataManager.DecorationMajorDLCGrouping.Rot);
  }
}
