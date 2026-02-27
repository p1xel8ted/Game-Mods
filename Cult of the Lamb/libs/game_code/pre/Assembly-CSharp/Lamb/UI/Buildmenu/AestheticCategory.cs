// Decompiled with JetBrains decompiler
// Type: Lamb.UI.BuildMenu.AestheticCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private RectTransform _miscContent;
  [SerializeField]
  private RectTransform _dlcContent;
  [SerializeField]
  private RectTransform _specialEventsContent;
  [SerializeField]
  private RectTransform _pathsContent;
  [SerializeField]
  private RectTransform _darkwoodContent;
  [SerializeField]
  private RectTransform _anuraContent;
  [SerializeField]
  private RectTransform _anchorDeepContent;
  [SerializeField]
  private RectTransform _silkCradleContent;
  [Header("Counts")]
  [SerializeField]
  private TextMeshProUGUI _miscUnlocked;
  [SerializeField]
  private TextMeshProUGUI _dlcUnlocked;
  [SerializeField]
  private TextMeshProUGUI _specialEventsUnlocked;
  [SerializeField]
  private TextMeshProUGUI _pathsUnlocked;
  [SerializeField]
  private TextMeshProUGUI _darkwoodUnlocked;
  [SerializeField]
  private TextMeshProUGUI _anuraUnlocked;
  [SerializeField]
  private TextMeshProUGUI _anchorDeepUnlocked;
  [SerializeField]
  private TextMeshProUGUI _silkCradleUnlocked;
  [SerializeField]
  private GameObject _dlcHeader;
  [SerializeField]
  private GameObject _specialEventsHeader;

  protected override void Populate()
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
    this._dlcContent.gameObject.SetActive(flag1);
    this._dlcHeader.gameObject.SetActive(flag1);
    this._specialEventsContent.gameObject.SetActive(flag2);
    this._specialEventsHeader.gameObject.SetActive(flag2);
    this.Populate(DataManager.DecorationsForType(DataManager.DecorationType.All), this._miscContent);
    this.Populate(DataManager.DecorationsForType(DataManager.DecorationType.DLC), this._dlcContent);
    this.Populate(DataManager.DecorationsForType(DataManager.DecorationType.Special_Events), this._specialEventsContent);
    this.Populate(DataManager.DecorationsForType(DataManager.DecorationType.Path), this._pathsContent);
    this.Populate(DataManager.DecorationsForType(DataManager.DecorationType.Dungeon1), this._darkwoodContent);
    this.Populate(DataManager.DecorationsForType(DataManager.DecorationType.Mushroom), this._anuraContent);
    this.Populate(DataManager.DecorationsForType(DataManager.DecorationType.Crystal), this._anchorDeepContent);
    this.Populate(DataManager.DecorationsForType(DataManager.DecorationType.Spider), this._silkCradleContent);
    this.SetUnlockedText(this._miscUnlocked, DataManager.DecorationType.All);
    this.SetUnlockedText(this._dlcUnlocked, DataManager.DecorationType.DLC);
    this.SetUnlockedText(this._specialEventsUnlocked, DataManager.DecorationType.Special_Events);
    this.SetUnlockedText(this._pathsUnlocked, DataManager.DecorationType.Path);
    this.SetUnlockedText(this._darkwoodUnlocked, DataManager.DecorationType.Dungeon1);
    this.SetUnlockedText(this._anuraUnlocked, DataManager.DecorationType.Mushroom);
    this.SetUnlockedText(this._anchorDeepUnlocked, DataManager.DecorationType.Crystal);
    this.SetUnlockedText(this._silkCradleUnlocked, DataManager.DecorationType.Spider);
  }

  private void SetUnlockedText(TextMeshProUGUI target, DataManager.DecorationType decorationType)
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
    target.text = string.Format(ScriptLocalization.UI.Collected, (object) $"{num1}/{num2}");
  }

  public static List<StructureBrain.TYPES> AllStructures()
  {
    List<StructureBrain.TYPES> typesList = new List<StructureBrain.TYPES>();
    typesList.AddRange((IEnumerable<StructureBrain.TYPES>) DataManager.DecorationsForType(DataManager.DecorationType.All));
    typesList.AddRange((IEnumerable<StructureBrain.TYPES>) DataManager.DecorationsForType(DataManager.DecorationType.DLC));
    typesList.AddRange((IEnumerable<StructureBrain.TYPES>) DataManager.DecorationsForType(DataManager.DecorationType.Special_Events));
    typesList.AddRange((IEnumerable<StructureBrain.TYPES>) DataManager.DecorationsForType(DataManager.DecorationType.Path));
    typesList.AddRange((IEnumerable<StructureBrain.TYPES>) DataManager.DecorationsForType(DataManager.DecorationType.Dungeon1));
    typesList.AddRange((IEnumerable<StructureBrain.TYPES>) DataManager.DecorationsForType(DataManager.DecorationType.Mushroom));
    typesList.AddRange((IEnumerable<StructureBrain.TYPES>) DataManager.DecorationsForType(DataManager.DecorationType.Crystal));
    typesList.AddRange((IEnumerable<StructureBrain.TYPES>) DataManager.DecorationsForType(DataManager.DecorationType.Spider));
    return typesList;
  }
}
