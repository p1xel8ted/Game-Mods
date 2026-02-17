// Decompiled with JetBrains decompiler
// Type: CrownAbilitiesIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CrownAbilitiesIcon : BaseMonoBehaviour
{
  public Image Available;
  public Image Unavailable;
  public Image Revealed;
  public Image Selected;
  public GameObject Researched;
  public Image Icon;
  public RectTransform ShakeRectTransform;
  public GameObject locked;
  public Button selectable;
  public CrownAbilities.TYPE type;
  public Animator animator;
  public bool iconSet;
  public bool CanAfford;
  public bool hasRevealed;
  public CrownAbilitiesTypeObjects CrownAbilitiesTypeObjects;
  public CrownAbilitiesIcon dependancy;
  public bool dependancyBought;
  public bool multipleBuildingUnlocks;
  public List<StructureBrain.TYPES> otherBuildingsToUnlock = new List<StructureBrain.TYPES>();
  public RectTransform _rectTransform;
  public float Shaking;
  public float ShakeSpeed;

  public void SetType()
  {
    this.Init();
    this.SetIcon();
  }

  public CrownAbilityTypeObject GetByType(CrownAbilities.TYPE Type)
  {
    foreach (CrownAbilityTypeObject byType in this.CrownAbilitiesTypeObjects.TypeAndPlacementObject)
    {
      if (byType.Type == Type)
        return byType;
    }
    return (CrownAbilityTypeObject) null;
  }

  public RectTransform rectTransform
  {
    get
    {
      if ((Object) this._rectTransform == (Object) null)
        this._rectTransform = this.GetComponent<RectTransform>();
      return this._rectTransform;
    }
  }

  public void OnEnable() => this.Init();

  public void SetIcon() => this.Icon.sprite = this.GetByType(this.type).IconImage;

  public void Init()
  {
    this.Revealed.enabled = false;
    this.Researched.SetActive(false);
    this.CanAfford = true;
    if ((Object) this.dependancy != (Object) null && !CrownAbilities.CrownAbilityUnlocked(this.dependancy.type))
    {
      this.CanAfford = false;
      this.dependancyBought = false;
      this.locked.SetActive(true);
    }
    if (CrownAbilities.CrownAbilityUnlocked(this.type))
    {
      this.Researched.SetActive(false);
      this.CanAfford = false;
      this.locked.SetActive(false);
      this.Available.enabled = false;
      this.Unavailable.enabled = false;
    }
    else
      this.SetAfford();
  }

  public void SetAfford()
  {
    if (DataManager.Instance.AbilityPoints < CrownAbilities.GetCrownAbilitiesCost(this.type))
      this.CanAfford = false;
    if (this.CanAfford)
    {
      this.Available.enabled = true;
      this.Unavailable.enabled = false;
    }
    else
    {
      this.Available.enabled = false;
      this.Unavailable.enabled = true;
    }
  }

  public void Shake() => this.ShakeSpeed = (float) (25 * (Random.Range(0, 2) < 1 ? 1 : -1));

  public void Update()
  {
    this.ShakeSpeed += (float) ((0.0 - (double) this.Shaking) * 0.40000000596046448);
    this.Shaking += (this.ShakeSpeed *= 0.8f);
    this.ShakeRectTransform.localPosition = Vector3.left * this.Shaking;
  }
}
