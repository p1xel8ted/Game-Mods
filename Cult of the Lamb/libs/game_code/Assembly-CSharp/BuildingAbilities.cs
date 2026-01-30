// Decompiled with JetBrains decompiler
// Type: BuildingAbilities
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;

#nullable disable
public class BuildingAbilities : BaseMonoBehaviour
{
  public TextMeshProUGUI BuildingNameTxt;
  public TextMeshProUGUI BuildingDescriptionTxt;
  public StructureBrain.TYPES BuildingType;
  public List<BuildingAbilities.BuildingAbility> BuildingAbilityList = new List<BuildingAbilities.BuildingAbility>();
  [CompilerGenerated]
  public bool \u003CHasChanged\u003Ek__BackingField;

  public bool HasChanged
  {
    get => this.\u003CHasChanged\u003Ek__BackingField;
    set => this.\u003CHasChanged\u003Ek__BackingField = value;
  }

  public virtual void GetBuildingName()
  {
    this.BuildingNameTxt.text = StructuresData.GetLocalizedNameStatic(this.BuildingType);
  }

  public virtual void GetBuildingDescription()
  {
    this.BuildingDescriptionTxt.text = StructuresData.GetLocalizedNameStatic(this.BuildingType);
  }

  public void OnEnable()
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

  public void OnDisable()
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
