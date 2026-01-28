// Decompiled with JetBrains decompiler
// Type: WeaponShopKeeperManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class WeaponShopKeeperManager : MonoBehaviour
{
  public EquipmentType TypeOfWeapon;
  public int WeaponLevel;
  public List<Interaction_WeaponItem> WeaponSlots;
  public GameObject SoldOutSign;
  public GameObject SoldOutSignObject;
  public GameObject BoughtItemBark;
  public GameObject NormalBark;
  public GameObject CantAffordBark;
  [SerializeField]
  public Interaction_SimpleConversation introConversation;
  [SerializeField]
  public Interaction_SimpleConversation winterIntroConversation;
  [SerializeField]
  public Interaction_SimpleConversation rotIntroConversation;
  [SerializeField]
  public Interaction_SimpleConversation relicRiddleConvo;
  [Header("Legendary Weapons")]
  [SerializeField]
  public Interaction_SimpleConversation legenadaryHammerConversation;
  [SerializeField]
  public Interaction_SimpleConversation legendarySwordConversation;
  [SerializeField]
  public Interaction_SimpleConversation legendaryDaggerConversation;
  [SerializeField]
  public Interaction_SimpleConversation legendaryGauntletsConversation;
  [SerializeField]
  public Interaction_SimpleConversation legendaryBlunderbussConversation;
  [SerializeField]
  public Interaction_SimpleConversation legendaryAxeConversation;
  [SerializeField]
  public Interaction_SimpleConversation legendaryChainConversation;
  public bool foundOne;
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

  public void Start()
  {
    this.InitWeaponShop();
    if (DataManager.Instance.GivenRelicFishRiddle)
      this.SetRelicRiddleManual();
    else if (DataManager.Instance.OnboardedRelics && !DungeonSandboxManager.Active)
      this.relicRiddleConvo.OnInteraction += (Interaction.InteractionEvent) (state => ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/KudaiiRelic", Objectives.CustomQuestTypes.FindKudaiiRelic), true, true));
    if (!DungeonSandboxManager.Active && PlayerFarming.AnyPlayerHasLegendaryWeapon())
      this.SetLegendaryWeaponsConvos();
    if (DataManager.Instance.BossesCompleted.Count >= 2 || !((UnityEngine.Object) this.relicRiddleConvo != (UnityEngine.Object) null))
      return;
    this.relicRiddleConvo.gameObject.SetActive(false);
  }

  public void OnDestroy()
  {
    this.legenadaryHammerConversation.Callback.RemoveAllListeners();
    this.legendarySwordConversation.Callback.RemoveAllListeners();
    this.legendaryDaggerConversation.Callback.RemoveAllListeners();
    this.legendaryGauntletsConversation.Callback.RemoveAllListeners();
    this.legendaryBlunderbussConversation.Callback.RemoveAllListeners();
    this.legendaryAxeConversation.Callback.RemoveAllListeners();
    this.legendaryChainConversation.Callback.RemoveAllListeners();
  }

  public void SetRelicRiddleManual()
  {
    this.relicRiddleConvo.ActivateDistance = 1.25f;
    this.relicRiddleConvo.AutomaticallyInteract = false;
    GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() =>
    {
      this.relicRiddleConvo.Finished = false;
      this.relicRiddleConvo.Spoken = false;
    }));
  }

  public int GetWeaponCost(EquipmentType type, int level)
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

  public void InitWeaponShop()
  {
    bool flag1 = (UnityEngine.Object) this.winterIntroConversation != (UnityEngine.Object) null && DataManager.Instance.CurrentSeason == SeasonsManager.Season.Winter && PlayerFarming.Location == FollowerLocation.Dungeon1_5 && !DataManager.Instance.SpokenToKudaiiWinter;
    bool flag2 = (UnityEngine.Object) this.rotIntroConversation != (UnityEngine.Object) null && PlayerFarming.Location == FollowerLocation.Dungeon1_6 && !DataManager.Instance.SpokenToKudaiiRot;
    if (!(flag1 | flag2))
      return;
    this.introConversation.gameObject.SetActive(false);
    this.rotIntroConversation.gameObject.SetActive(flag2);
    this.winterIntroConversation.gameObject.SetActive(flag1);
  }

  public void SetLegendaryWeaponsConvos()
  {
    EquipmentType equipmentType = EquipmentType.None;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if (player.playerWeapon.IsLegendaryWeapon() && !DataManager.Instance.KudaaiLegendaryWeaponsResponses.Contains(player.currentWeapon))
      {
        equipmentType = player.currentWeapon;
        break;
      }
    }
    switch (equipmentType)
    {
      case EquipmentType.Sword_Legendary:
        this.legendarySwordConversation.gameObject.SetActive(true);
        this.legendarySwordConversation.Callback.AddListener((UnityAction) (() => DataManager.Instance.KudaaiLegendaryWeaponsResponses.Add(EquipmentType.Sword_Legendary)));
        break;
      case EquipmentType.Axe_Legendary:
        this.legendaryAxeConversation.gameObject.SetActive(true);
        this.legendaryAxeConversation.Callback.AddListener((UnityAction) (() => DataManager.Instance.KudaaiLegendaryWeaponsResponses.Add(EquipmentType.Axe_Legendary)));
        break;
      case EquipmentType.Hammer_Legendary:
        this.legenadaryHammerConversation.gameObject.SetActive(true);
        this.legenadaryHammerConversation.Callback.AddListener((UnityAction) (() => DataManager.Instance.KudaaiLegendaryWeaponsResponses.Add(EquipmentType.Hammer_Legendary)));
        break;
      case EquipmentType.Dagger_Legendary:
        this.legendaryDaggerConversation.gameObject.SetActive(true);
        this.legendaryDaggerConversation.Callback.AddListener((UnityAction) (() => DataManager.Instance.KudaaiLegendaryWeaponsResponses.Add(EquipmentType.Dagger_Legendary)));
        break;
      case EquipmentType.Gauntlet_Legendary:
        this.legendaryGauntletsConversation.gameObject.SetActive(true);
        this.legendaryGauntletsConversation.Callback.AddListener((UnityAction) (() => DataManager.Instance.KudaaiLegendaryWeaponsResponses.Add(EquipmentType.Gauntlet_Legendary)));
        break;
      case EquipmentType.Blunderbuss_Legendary:
        this.legendaryBlunderbussConversation.gameObject.SetActive(true);
        this.legendaryBlunderbussConversation.Callback.AddListener((UnityAction) (() => DataManager.Instance.KudaaiLegendaryWeaponsResponses.Add(EquipmentType.Blunderbuss_Legendary)));
        break;
      case EquipmentType.Chain_Legendary:
        this.legendaryChainConversation.gameObject.SetActive(true);
        this.legendaryChainConversation.Callback.AddListener((UnityAction) (() => DataManager.Instance.KudaaiLegendaryWeaponsResponses.Add(EquipmentType.Chain_Legendary)));
        break;
    }
  }

  [CompilerGenerated]
  public void \u003CSetRelicRiddleManual\u003Eb__21_0()
  {
    this.relicRiddleConvo.Finished = false;
    this.relicRiddleConvo.Spoken = false;
  }
}
