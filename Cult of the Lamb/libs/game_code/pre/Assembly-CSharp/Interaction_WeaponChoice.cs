// Decompiled with JetBrains decompiler
// Type: Interaction_WeaponChoice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_WeaponChoice : Interaction_WeaponSelectionPodium
{
  [SerializeField]
  private UnityEvent onInteract;
  [SerializeField]
  private Interaction_WeaponSelectionPodium[] otherWeaponOptions;
  private bool hasTriedFallback;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    for (int index = this.otherWeaponOptions.Length - 1; index >= 0; --index)
    {
      this.otherWeaponOptions[index].Interactable = false;
      this.otherWeaponOptions[index].Lighting.SetActive(false);
      this.otherWeaponOptions[index].IconSpriteRenderer.enabled = false;
      this.otherWeaponOptions[index].weaponBetterIcon.enabled = false;
      this.otherWeaponOptions[index].podiumOn.SetActive(false);
      this.otherWeaponOptions[index].podiumOff.SetActive(true);
      this.otherWeaponOptions[index].particleEffect.Stop();
      this.otherWeaponOptions[index].AvailableGoop.Play("Hide");
      this.otherWeaponOptions[index].enabled = false;
    }
    this.onInteract?.Invoke();
  }

  protected override void SetWeapon()
  {
    Debug.Log((object) "SetWeapon()");
    this.IconSpriteRenderer.material = this.WeaponMaterial;
    for (int index = 0; index < 50; ++index)
    {
      this.TypeOfWeapon = DataManager.Instance.GetRandomWeaponInPool();
      bool flag = false;
      foreach (Interaction_WeaponSelectionPodium otherWeaponOption in this.otherWeaponOptions)
      {
        if (otherWeaponOption.TypeOfWeapon == this.TypeOfWeapon)
          flag = true;
      }
      if (flag)
      {
        if (index >= 49)
        {
          if (!this.hasTriedFallback && DataManager.Instance.EnabledSpells)
          {
            this.hasTriedFallback = true;
            this.SetCurse();
            return;
          }
          this.gameObject.SetActive(false);
        }
      }
      else
        break;
    }
    this.Type = Interaction_WeaponSelectionPodium.Types.Weapon;
    this.WeaponLevel = DataManager.Instance.CurrentRunWeaponLevel + 1;
    this.weaponBetterIcon.enabled = false;
    if (DataManager.Instance.ForcedStartingWeapon != EquipmentType.None)
    {
      this.TypeOfWeapon = DataManager.Instance.ForcedStartingWeapon;
      DataManager.Instance.ForcedStartingWeapon = EquipmentType.None;
    }
    if (DataManager.Instance.CurrentWeapon != EquipmentType.None)
      return;
    this.WeaponLevel += DataManager.StartingEquipmentLevel;
  }

  protected override void SetCurse()
  {
    Debug.Log((object) "SetCurse()");
    this.IconSpriteRenderer.material = this.CurseMaterial;
    for (int index = 0; index < 50; ++index)
    {
      this.TypeOfWeapon = DataManager.Instance.GetRandomCurseInPool();
      bool flag = false;
      foreach (Interaction_WeaponSelectionPodium otherWeaponOption in this.otherWeaponOptions)
      {
        if (otherWeaponOption.TypeOfWeapon == this.TypeOfWeapon)
          flag = true;
      }
      if (flag)
      {
        if (index >= 49)
        {
          if (!this.hasTriedFallback)
          {
            this.hasTriedFallback = true;
            this.SetWeapon();
            return;
          }
          this.gameObject.SetActive(false);
        }
      }
      else
        break;
    }
    this.Type = Interaction_WeaponSelectionPodium.Types.Curse;
    this.WeaponLevel = DataManager.Instance.CurrentRunCurseLevel + 1;
    this.weaponBetterIcon.enabled = false;
    if (DataManager.Instance.ForcedStartingCurse != EquipmentType.None)
    {
      this.TypeOfWeapon = DataManager.Instance.ForcedStartingCurse;
      DataManager.Instance.ForcedStartingCurse = EquipmentType.None;
    }
    if (DataManager.Instance.CurrentCurse != EquipmentType.None)
      return;
    this.WeaponLevel += DataManager.StartingEquipmentLevel;
  }
}
