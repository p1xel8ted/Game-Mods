// Decompiled with JetBrains decompiler
// Type: WeaponShopKeeperManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class WeaponShopKeeperManager : MonoBehaviour
{
  private EquipmentType TypeOfWeapon;
  private int WeaponLevel;
  public List<Interaction_WeaponItem> WeaponSlots;
  public GameObject SoldOutSign;
  private GameObject SoldOutSignObject;
  public GameObject BoughtItemBark;
  public GameObject NormalBark;
  public GameObject CantAffordBark;
  private bool foundOne;
  public List<EquipmentType> Weapons = new List<EquipmentType>()
  {
    EquipmentType.Axe,
    EquipmentType.Axe_Healing,
    EquipmentType.Axe_Critical,
    EquipmentType.Axe_Fervour,
    EquipmentType.Axe_Godly,
    EquipmentType.Axe_Nercomancy,
    EquipmentType.Axe_Poison,
    EquipmentType.Sword,
    EquipmentType.Sword_Critical,
    EquipmentType.Sword_Fervour,
    EquipmentType.Sword_Godly,
    EquipmentType.Sword_Healing,
    EquipmentType.Sword_Poison,
    EquipmentType.Sword_Nercomancy,
    EquipmentType.Dagger,
    EquipmentType.Dagger_Critical,
    EquipmentType.Dagger_Fervour,
    EquipmentType.Dagger_Godly,
    EquipmentType.Dagger_Healing,
    EquipmentType.Dagger_Nercomancy,
    EquipmentType.Dagger_Poison,
    EquipmentType.Hammer,
    EquipmentType.Hammer_Critical,
    EquipmentType.Hammer_Fervour,
    EquipmentType.Hammer_Godly,
    EquipmentType.Hammer_Healing,
    EquipmentType.Hammer_Nercomancy,
    EquipmentType.Hammer_Poison,
    EquipmentType.Gauntlet,
    EquipmentType.Gauntlet_Critical,
    EquipmentType.Gauntlet_Fervour,
    EquipmentType.Gauntlet_Godly,
    EquipmentType.Gauntlet_Healing,
    EquipmentType.Gauntlet_Poison,
    EquipmentType.Gauntlet_Nercomancy
  };
  public List<EquipmentType> Curses = new List<EquipmentType>()
  {
    EquipmentType.Fireball,
    EquipmentType.Fireball_Charm,
    EquipmentType.Fireball_Swarm,
    EquipmentType.Fireball_Triple,
    EquipmentType.ProjectileAOE,
    EquipmentType.ProjectileAOE_Charm,
    EquipmentType.ProjectileAOE_ExplosiveImpact,
    EquipmentType.ProjectileAOE_GoopTrail,
    EquipmentType.EnemyBlast,
    EquipmentType.EnemyBlast_Poison,
    EquipmentType.EnemyBlast_DeflectsProjectiles,
    EquipmentType.EnemyBlast_Ice,
    EquipmentType.Tentacles,
    EquipmentType.Tentacles_Necromancy,
    EquipmentType.Tentacles_Circular,
    EquipmentType.Tentacles_Ice,
    EquipmentType.MegaSlash,
    EquipmentType.MegaSlash_Necromancy,
    EquipmentType.MegaSlash_Charm,
    EquipmentType.MegaSlash_Ice
  };

  private void Start() => this.InitWeaponShop();

  private bool CheckWeaponIsntUsed(EquipmentType type)
  {
    foreach (Interaction_WeaponItem weaponSlot in this.WeaponSlots)
    {
      if (weaponSlot.TypeOfWeapon == type)
        return false;
    }
    return true;
  }

  private int GetWeaponCost(EquipmentType type, int level)
  {
    int num;
    switch (type)
    {
      case EquipmentType.Sword:
      case EquipmentType.Axe:
      case EquipmentType.Hammer:
      case EquipmentType.Dagger:
      case EquipmentType.Gauntlet:
      case EquipmentType.Tentacles:
      case EquipmentType.ProjectileAOE:
      case EquipmentType.Fireball:
      case EquipmentType.MegaSlash:
        num = 2;
        break;
      case EquipmentType.Sword_Poison:
      case EquipmentType.Axe_Poison:
      case EquipmentType.Hammer_Poison:
      case EquipmentType.Dagger_Poison:
      case EquipmentType.Gauntlet_Poison:
        num = 5;
        break;
      case EquipmentType.Sword_Critical:
      case EquipmentType.Axe_Critical:
      case EquipmentType.Hammer_Critical:
      case EquipmentType.Dagger_Critical:
      case EquipmentType.Gauntlet_Critical:
        num = 5;
        break;
      case EquipmentType.Sword_Healing:
      case EquipmentType.Axe_Healing:
      case EquipmentType.Hammer_Healing:
      case EquipmentType.Dagger_Healing:
      case EquipmentType.Gauntlet_Healing:
        num = 5;
        break;
      case EquipmentType.Sword_Fervour:
      case EquipmentType.Axe_Fervour:
      case EquipmentType.Hammer_Fervour:
      case EquipmentType.Dagger_Fervour:
      case EquipmentType.Gauntlet_Fervour:
        num = 5;
        break;
      case EquipmentType.Sword_Godly:
      case EquipmentType.Axe_Godly:
      case EquipmentType.Hammer_Godly:
      case EquipmentType.Dagger_Godly:
      case EquipmentType.Gauntlet_Godly:
        num = 5;
        break;
      case EquipmentType.Sword_Nercomancy:
      case EquipmentType.Axe_Nercomancy:
      case EquipmentType.Hammer_Nercomancy:
      case EquipmentType.Dagger_Nercomancy:
      case EquipmentType.Gauntlet_Nercomancy:
        num = 5;
        break;
      case EquipmentType.Tentacles_Circular:
      case EquipmentType.Tentacles_Ice:
      case EquipmentType.Tentacles_Necromancy:
        num = 5;
        break;
      case EquipmentType.EnemyBlast_DeflectsProjectiles:
      case EquipmentType.EnemyBlast_Ice:
      case EquipmentType.EnemyBlast_Poison:
        num = 5;
        break;
      case EquipmentType.ProjectileAOE_ExplosiveImpact:
      case EquipmentType.ProjectileAOE_GoopTrail:
      case EquipmentType.ProjectileAOE_Charm:
        num = 5;
        break;
      case EquipmentType.Fireball_Swarm:
      case EquipmentType.Fireball_Triple:
      case EquipmentType.Fireball_Charm:
        num = 5;
        break;
      case EquipmentType.MegaSlash_Necromancy:
      case EquipmentType.MegaSlash_Ice:
      case EquipmentType.MegaSlash_Charm:
        num = 5;
        break;
      default:
        num = 5;
        break;
    }
    return num + Mathf.Clamp((int) ((double) (level - DataManager.Instance.CurrentRunWeaponLevel) * 1.5), 0, 5);
  }

  private void InitWeaponShop()
  {
  }
}
