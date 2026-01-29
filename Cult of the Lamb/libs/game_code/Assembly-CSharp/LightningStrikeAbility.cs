// Decompiled with JetBrains decompiler
// Type: LightningStrikeAbility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class LightningStrikeAbility : StrikeAbility
{
  public LightningStrikeAbility(int maxStrikes)
    : base(maxStrikes)
  {
    this.vfxData = BiomeConstants.Instance.LightningSequenceData;
    this.SetAttackFlags(Health.AttackFlags.DoesntChargeRelics | Health.AttackFlags.Electrified);
  }

  public LightningStrikeAbility(
    VFXAbilitySequenceData vfxData,
    int maxStrikes,
    InventoryItem.ITEM_TYPE drop,
    float dropChance)
    : base(vfxData, maxStrikes, drop, dropChance)
  {
    this.SetAttackFlags(Health.AttackFlags.DoesntChargeRelics | Health.AttackFlags.Electrified);
  }

  public override void Play(
    GameObject owner,
    Health.Team enemyTeam,
    float damage,
    PlayerFarming playerFarming,
    bool avoidImmuneEnemies = true,
    List<Health> customTargets = null,
    string customImpactSFX = "")
  {
    List<Health> targets = (List<Health>) null;
    List<Health> targets1 = this.GetTargets(enemyTeam);
    if (customTargets != null)
    {
      if (avoidImmuneEnemies)
      {
        targets = this.GetVulnerableTargets(customTargets);
        if (targets.Count < this.maxStrikes)
        {
          targets.AddRange((IEnumerable<Health>) this.GetVulnerableTargets(targets1));
          if (targets.Count < this.maxStrikes)
            targets.AddRange((IEnumerable<Health>) this.GetImmuneTargets(targets1));
        }
      }
      else
        targets = customTargets;
    }
    else if (avoidImmuneEnemies)
    {
      targets = this.GetVulnerableTargets(targets1);
      if (targets.Count < this.maxStrikes)
        targets.AddRange((IEnumerable<Health>) this.GetImmuneTargets(targets1));
    }
    else
      targets = targets1;
    if (targets.Count > 0)
    {
      int length = Mathf.Min(this.maxStrikes, targets.Count);
      if ((double) this.dropChance > 0.0)
      {
        for (int index = 0; index < length; ++index)
        {
          DropMultipleLootOnDeath multipleLootOnDeath = targets[index].gameObject.AddComponent<DropMultipleLootOnDeath>();
          multipleLootOnDeath.chanceToDropLoot = this.dropChance;
          multipleLootOnDeath.RandomAmountToDrop = Vector2.one;
          multipleLootOnDeath.LootToDrop = new List<DropMultipleLootOnDeath.ItemAndProbability>(1)
          {
            new DropMultipleLootOnDeath.ItemAndProbability(this.drop, 100)
          };
        }
      }
      Transform[] targetTransforms = new Transform[length];
      for (int index = 0; index < length; ++index)
        targetTransforms[index] = targets[index].transform;
      VFXSequence sequence = this.vfxData.PlayNewSequence(playerFarming.transform, targetTransforms);
      sequence.OnImpact += (Action<VFXObject, int>) ((vfxObject, targetIndex) =>
      {
        if (targets.Count <= targetIndex || !((UnityEngine.Object) targetTransforms[targetIndex] != (UnityEngine.Object) null))
          return;
        Vector3 position = targets[targetIndex].transform.position;
        vfxObject.transform.SetPositionAndRotation(position, Quaternion.identity);
        IAttackResilient component = targets[targetIndex].GetComponent<IAttackResilient>();
        component?.StopResilience();
        targets[targetIndex].DealDamage(damage, owner, position, AttackType: Health.AttackTypes.Projectile, AttackFlags: this.attackFlags);
        component?.ResetResilience();
      });
      sequence.OnComplete += (System.Action) (() =>
      {
        sequence.OnImpact -= (Action<VFXObject, int>) ((vfxObject, targetIndex) =>
        {
          if (targets.Count <= targetIndex || !((UnityEngine.Object) targetTransforms[targetIndex] != (UnityEngine.Object) null))
            return;
          Vector3 position = targets[targetIndex].transform.position;
          vfxObject.transform.SetPositionAndRotation(position, Quaternion.identity);
          IAttackResilient component = targets[targetIndex].GetComponent<IAttackResilient>();
          component?.StopResilience();
          targets[targetIndex].DealDamage(damage, owner, position, AttackType: Health.AttackTypes.Projectile, AttackFlags: this.attackFlags);
          component?.ResetResilience();
        });
        sequence.OnComplete -= (System.Action) (() =>
        {
          // ISSUE: unable to decompile the method.
        });
      });
    }
    else
      this.vfxData.TestSequence(this.maxStrikes, false, playerFarming);
  }
}
