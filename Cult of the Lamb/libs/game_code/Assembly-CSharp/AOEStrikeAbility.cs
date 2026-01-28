// Decompiled with JetBrains decompiler
// Type: AOEStrikeAbility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class AOEStrikeAbility : StrikeAbility
{
  public float damageRadius = 1.5f;

  public AOEStrikeAbility(int maxStrikes)
    : base(maxStrikes)
  {
    this.vfxData = BiomeConstants.Instance.LightningSequenceData;
    this.SetAttackFlags(Health.AttackFlags.DoesntChargeRelics);
  }

  public AOEStrikeAbility(
    VFXAbilitySequenceData vfxData,
    int maxStrikes,
    InventoryItem.ITEM_TYPE drop,
    float dropChance)
    : base(vfxData, maxStrikes, drop, dropChance)
  {
    this.SetAttackFlags(Health.AttackFlags.DoesntChargeRelics);
  }

  public override void Play(
    GameObject owner,
    Health.Team enemyTeam,
    float damage,
    PlayerFarming playerFarming,
    bool ignoreImmuneEnemies = false,
    List<Health> customTargets = null,
    string customImpactSFX = "")
  {
    List<Health> targets = (List<Health>) null;
    List<Health> allTargets = this.GetTargets(enemyTeam);
    if (customTargets != null)
    {
      if (ignoreImmuneEnemies)
      {
        targets = this.GetVulnerableTargets(customTargets);
        if (targets.Count < this.maxStrikes)
        {
          targets.AddRange((IEnumerable<Health>) this.GetVulnerableTargets(allTargets));
          if (targets.Count < this.maxStrikes)
            targets.AddRange((IEnumerable<Health>) this.GetImmuneTargets(allTargets));
        }
      }
      else
        targets = customTargets;
    }
    else if (ignoreImmuneEnemies)
    {
      targets = this.GetVulnerableTargets(allTargets);
      if (targets.Count < this.maxStrikes)
        targets.AddRange((IEnumerable<Health>) this.GetImmuneTargets(allTargets));
    }
    else
      targets = allTargets;
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
        if (!string.IsNullOrEmpty(customImpactSFX))
          AudioManager.Instance.PlayOneShot(customImpactSFX, position);
        foreach (Health health in allTargets)
        {
          if ((UnityEngine.Object) health != (UnityEngine.Object) null && (double) Vector2.Distance((Vector2) health.transform.position, (Vector2) position) <= (double) this.damageRadius)
            targets[targetIndex].DealDamage(damage, owner, position, AttackType: Health.AttackTypes.Projectile, AttackFlags: this.attackFlags);
        }
      });
      sequence.OnComplete += (System.Action) (() =>
      {
        sequence.OnImpact -= (Action<VFXObject, int>) ((vfxObject, targetIndex) =>
        {
          if (targets.Count <= targetIndex || !((UnityEngine.Object) targetTransforms[targetIndex] != (UnityEngine.Object) null))
            return;
          Vector3 position = targets[targetIndex].transform.position;
          vfxObject.transform.SetPositionAndRotation(position, Quaternion.identity);
          if (!string.IsNullOrEmpty(customImpactSFX))
            AudioManager.Instance.PlayOneShot(customImpactSFX, position);
          foreach (Health health in allTargets)
          {
            if ((UnityEngine.Object) health != (UnityEngine.Object) null && (double) Vector2.Distance((Vector2) health.transform.position, (Vector2) position) <= (double) this.damageRadius)
              targets[targetIndex].DealDamage(damage, owner, position, AttackType: Health.AttackTypes.Projectile, AttackFlags: this.attackFlags);
          }
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
