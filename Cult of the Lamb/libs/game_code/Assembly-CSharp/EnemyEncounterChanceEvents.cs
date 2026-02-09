// Decompiled with JetBrains decompiler
// Type: EnemyEncounterChanceEvents
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyEncounterChanceEvents : BaseMonoBehaviour
{
  public static List<Enemy> exludedGroupedEnemyList = new List<Enemy>()
  {
    Enemy.Woodman,
    Enemy.WolfGuardian,
    Enemy.WolfTurret,
    Enemy.DogDiveBomb
  };
  public bool affectAllRooms;
  [Header("Shielded Enemy options")]
  public float layer1AllShieldChance;
  public float layer2AllShieldChance = 0.05f;
  public float layer1IndividualShieldChance = 0.05f;
  public float layer2IndividualShieldChance = 0.05f;
  public bool allowShieldsBeforeLayer2;
  public bool shieldsPreHeavyAttackGained;
  public bool forceCanHaveShields;
  [Header("Grouped Enemy Options")]
  public float layer1ChanceOfGroupedEnemies = 0.05f;
  public float layer2ChanceOfGroupedEnemies = 0.05f;
  public float groupDamageProtectionMultiplier;
  public int minimumGroupSize = 3;
  public int maximumGroupSize = 4;
  public bool allowExtraNonGrouped;
  public bool alsoProtectedFromTraps = true;
  public Vector3 orderIndicatorOffset = Vector3.zero;
  public List<UnitObject> shieldUnits = new List<UnitObject>();
  public List<UnitObject> groupableUnits = new List<UnitObject>();
  public UnitObject[] allUnits;
  public int currentOrder;

  public void Start()
  {
    if (this.affectAllRooms)
    {
      BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.AssignShieldsAndGroups);
      if (!this.forceCanHaveShields)
        return;
      this.AssignShieldsAndGroups();
    }
    else
    {
      Debug.Log((object) ("Assigning shields and groups for this room " + Time.realtimeSinceStartup.ToString()));
      this.AssignShieldsAndGroups();
    }
  }

  public void OnDestroy()
  {
    Debug.Log((object) "Changed room removed as I've been destroyed");
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.AssignShieldsAndGroups);
  }

  public void AssignShieldsAndGroups()
  {
    Debug.Log((object) $"Assigning shields and groups for {this.gameObject.name} time: {Time.realtimeSinceStartup.ToString()}");
    UnityEngine.Random.State state = UnityEngine.Random.state;
    UnityEngine.Random.InitState(BiomeGenerator.Instance.CurrentRoom.Seed);
    this.FindEnemyGroups();
    if (this.shieldUnits.Count > 0)
      this.AssignShieldEnemies();
    if (this.groupableUnits.Count > 0)
      this.AssignGroupedEnemies();
    UnityEngine.Random.state = state;
  }

  public void FindEnemyGroups()
  {
    this.allUnits = UnityEngine.Object.FindObjectsOfType<UnitObject>();
    this.currentOrder = 0;
    this.shieldUnits.Clear();
    this.groupableUnits.Clear();
    for (int index = 0; index < this.allUnits.Length; ++index)
    {
      UnitObject allUnit = this.allUnits[index];
      if ((UnityEngine.Object) allUnit.health != (UnityEngine.Object) null && allUnit.health.team == Health.Team.Team2)
      {
        this.shieldUnits.Add(allUnit);
        if (this.groupableUnits.Count < this.maximumGroupSize && (UnityEngine.Object) allUnit.orderIndicator == (UnityEngine.Object) null && (UnityEngine.Object) allUnit.GetComponent<ShowHPBar>() != (UnityEngine.Object) null && !EnemyEncounterChanceEvents.exludedGroupedEnemyList.Contains(allUnit.EnemyType))
          this.groupableUnits.Add(allUnit);
      }
    }
    if (this.groupableUnits.Count >= this.minimumGroupSize && (this.groupableUnits.Count <= this.maximumGroupSize || this.allowExtraNonGrouped))
      return;
    this.groupableUnits.Clear();
  }

  public void AssignShieldEnemies()
  {
    if (PlayerFleeceManager.FleeceSwapsWeaponForCurse())
      return;
    float num1 = this.layer1AllShieldChance;
    float individualShieldChance = this.layer1IndividualShieldChance;
    if (GameManager.Layer2)
    {
      num1 = this.layer2AllShieldChance;
      individualShieldChance = this.layer2IndividualShieldChance;
    }
    bool flag1 = false;
    bool flag2 = DataManager.Instance.PlayerFleece != 6 && !DataManager.Instance.SpecialAttacksDisabled && (CrownAbilities.CrownAbilityUnlocked(CrownAbilities.TYPE.Combat_HeavyAttack) || UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HeavyAttacks) || this.shieldsPreHeavyAttackGained);
    bool flag3 = GameManager.Layer2 || this.allowShieldsBeforeLayer2;
    if (flag2 & flag3)
      flag1 = (double) UnityEngine.Random.value < (double) num1;
    if (this.forceCanHaveShields)
      flag2 = true;
    for (int index = 0; index < this.shieldUnits.Count; ++index)
    {
      EnemyHasShield component = this.shieldUnits[index].gameObject.GetComponent<EnemyHasShield>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        float num2 = 0.0f;
        if (flag1)
        {
          num2 = 1f;
        }
        else
        {
          if (flag2 & flag3 || component.allowBeforeGotHeavyAttack)
            num2 = individualShieldChance;
          if (flag2 && !GameManager.Layer2 && (double) component.layer1ShieldChanceOverride != -1.0)
            num2 = component.layer1ShieldChanceOverride;
          else if (flag2 && GameManager.Layer2 && (double) component.layer2ShieldChanceOverride != -1.0)
            num2 = component.layer2ShieldChanceOverride;
        }
        if ((double) UnityEngine.Random.value < (double) num2)
          component.AddShield();
      }
    }
  }

  public void AssignGroupedEnemies()
  {
    float ofGroupedEnemies = this.layer1ChanceOfGroupedEnemies;
    if (GameManager.Layer2)
      ofGroupedEnemies = this.layer2ChanceOfGroupedEnemies;
    if ((double) UnityEngine.Random.value >= (double) ofGroupedEnemies)
      return;
    for (int index = 0; index < this.groupableUnits.Count; ++index)
    {
      UnitObject groupableUnit = this.groupableUnits[index];
      if (!((UnityEngine.Object) groupableUnit == (UnityEngine.Object) null))
      {
        if (this.alsoProtectedFromTraps && index > 0)
          groupableUnit.health.ImmuneToTraps = true;
        if (index < this.groupableUnits.Count - 1)
        {
          MinionProtector minionProtector = groupableUnit.gameObject.AddComponent<MinionProtector>();
          UnitObject[] unitObjectArray = new UnitObject[1]
          {
            this.groupableUnits[index + 1]
          };
          minionProtector.protectedUnits = unitObjectArray;
          minionProtector.showLineToMinions = false;
          minionProtector.healthLineMaterial = (Material) null;
          minionProtector.damageMultiplier = this.groupDamageProtectionMultiplier;
          minionProtector.destroyedAction += new System.Action(this.protectorDestroyed);
        }
        ShowHPBar component1 = groupableUnit.GetComponent<ShowHPBar>();
        if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        {
          component1.Init();
          Debug.Log((object) "Positioning by the healthbar?");
          groupableUnit.orderIndicator = component1.hpBar.groupIndicator;
          component1.hpBar.gameObject.SetActive(true);
          groupableUnit.orderIndicator.gameObject.SetActive(true);
          groupableUnit.orderIndicator.SetIndicatorForOrder(index);
          EnemyScuttleSwiper component2 = groupableUnit.gameObject.GetComponent<EnemyScuttleSwiper>();
          if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.StartHidden == EnemyScuttleSwiper.StartingStates.Hidden)
          {
            groupableUnit.orderIndicator.gameObject.SetActive(false);
            component1.hpBar.gameObject.SetActive(false);
          }
        }
        else
          Debug.Log((object) "Problem finding the order indicator!");
      }
    }
  }

  public void protectorDestroyed()
  {
    ++this.currentOrder;
    if (this.groupableUnits == null || !((UnityEngine.Object) this.groupableUnits[this.currentOrder] != (UnityEngine.Object) null) || !((UnityEngine.Object) this.groupableUnits[this.currentOrder].orderIndicator != (UnityEngine.Object) null) || this.currentOrder >= this.groupableUnits.Count)
      return;
    Debug.Log((object) ("Protector has been destroyed updating next available order indicator to green " + this.currentOrder.ToString()));
    this.groupableUnits[this.currentOrder].orderIndicator.SetVulnerable(true);
    if (!this.alsoProtectedFromTraps)
      return;
    this.groupableUnits[this.currentOrder].health.ImmuneToTraps = false;
  }
}
