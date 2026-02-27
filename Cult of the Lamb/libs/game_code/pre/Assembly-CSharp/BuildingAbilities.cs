// Decompiled with JetBrains decompiler
// Type: BuildingAbilities
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System.Collections.Generic;
using TMPro;

#nullable disable
public class BuildingAbilities : BaseMonoBehaviour
{
  public TextMeshProUGUI BuildingNameTxt;
  public TextMeshProUGUI BuildingDescriptionTxt;
  public StructureBrain.TYPES BuildingType;
  public List<BuildingAbilities.BuildingAbility> BuildingAbilityList = new List<BuildingAbilities.BuildingAbility>();

  public bool HasChanged { get; protected set; }

  public virtual void GetBuildingName()
  {
    this.BuildingNameTxt.text = StructuresData.GetLocalizedNameStatic(this.BuildingType);
  }

  public virtual void GetBuildingDescription()
  {
    this.BuildingDescriptionTxt.text = StructuresData.GetLocalizedNameStatic(this.BuildingType);
  }

  private void OnEnable()
  {
    this.OnEnableInteraction();
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.UpdateLocalisation);
  }

  public virtual void UpdateLocalisation()
  {
    this.GetBuildingName();
    this.GetBuildingDescription();
  }

  public virtual void OnEnableInteraction()
  {
  }

  private void OnDisable()
  {
    this.OnDisableInteraction();
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.UpdateLocalisation);
  }

  public virtual void OnDisableInteraction()
  {
  }

  public enum BuildingAbility
  {
    NONE,
    Shrine_Buff_0,
    Shrine_Buff_1,
  }
}
