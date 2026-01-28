// Decompiled with JetBrains decompiler
// Type: SermonWheelCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class SermonWheelCategory : UIRadialWheelItem
{
  [SerializeField]
  public SermonCategoryTextIcon _textIcon;
  [SerializeField]
  public Image _topCircle;
  [SerializeField]
  public TextMeshProUGUI _progress;
  public bool _locked;
  public string _title;
  public string _description;
  public InventoryItem.ITEM_TYPE _currency;

  public SermonCategory SermonCategory => this._textIcon.SermonCategory;

  public void Configure(InventoryItem.ITEM_TYPE currency)
  {
    this._currency = currency;
    this._description = DoctrineUpgradeSystem.GetSermonCategoryLocalizedDescription(this.SermonCategory);
    if (!this.IsValidOption())
    {
      if (currency == InventoryItem.ITEM_TYPE.CRYSTAL_DOCTRINE_STONE)
      {
        this.SetAsLocked();
        this._title = DoctrineUpgradeSystem.GetRemainingDoctrines(this.SermonCategory).Count != 0 ? $"{DoctrineUpgradeSystem.GetSermonCategoryLocalizedName(this.SermonCategory)} - {(DoctrineUpgradeSystem.GetLevelBySermon(this.SermonCategory) + 1).ToNumeral()}" : $"{DoctrineUpgradeSystem.GetSermonCategoryLocalizedName(this.SermonCategory)} - {ScriptLocalization.UI_Generic.Max.Colour(StaticColors.BlueColor)}";
      }
      else
        this._title = $"{DoctrineUpgradeSystem.GetSermonCategoryLocalizedName(this.SermonCategory)} - {ScriptLocalization.UI_Generic.Max.Colour(StaticColors.RedColor)}";
    }
    else
      this._title = $"{DoctrineUpgradeSystem.GetSermonCategoryLocalizedName(this.SermonCategory)} - {(DoctrineUpgradeSystem.GetLevelBySermon(this.SermonCategory) + 1).ToNumeral()}";
    if (currency == InventoryItem.ITEM_TYPE.DOCTRINE_STONE)
      this._progress.text = $"{(object) DoctrineUpgradeSystem.GetLevelBySermon(this.SermonCategory)}/{(object) 4}";
    else if (this._locked)
    {
      this._progress.text = "";
      if (DoctrineUpgradeSystem.GetRemainingDoctrines(this.SermonCategory).Count == 0)
        this._description = string.Format(ScriptLocalization.UI_CrystalDoctrine.Max, (object) DoctrineUpgradeSystem.GetSermonCategoryLocalizedName(this.SermonCategory));
      else
        this._description = ScriptLocalization.UI_CrystalDoctrine.Locked;
    }
    else
      this._progress.text = $"{(object) (4 - DoctrineUpgradeSystem.GetRemainingDoctrines(this.SermonCategory).Count)}/{(object) 4}".Colour(StaticColors.BlueColourHex);
  }

  public void SetAsLocked()
  {
    this._locked = true;
    this._textIcon.SetLock();
  }

  public override string GetTitle() => this._title;

  public override bool IsValidOption()
  {
    if (this._currency == InventoryItem.ITEM_TYPE.DOCTRINE_STONE)
      return DoctrineUpgradeSystem.GetLevelBySermon(this.SermonCategory) < 4;
    return this._currency == InventoryItem.ITEM_TYPE.CRYSTAL_DOCTRINE_STONE && DoctrineUpgradeSystem.GetRemainingDoctrines(this.SermonCategory).Count > 0 && DoctrineUpgradeSystem.GetLevelBySermon(this.SermonCategory) == 4;
  }

  public override bool Visible() => true;

  public override string GetDescription() => this._description;
}
