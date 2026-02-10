// Decompiled with JetBrains decompiler
// Type: StrikeAbility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public abstract class StrikeAbility
{
  [SerializeField]
  public VFXAbilitySequenceData vfxData;
  [SerializeField]
  public int maxStrikes = 5;
  [SerializeField]
  public InventoryItem.ITEM_TYPE drop;
  [SerializeField]
  public float dropChance;
  public Health.AttackFlags attackFlags;

  public StrikeAbility(int maxStrikes) => this.maxStrikes = maxStrikes;

  public StrikeAbility(
    VFXAbilitySequenceData vfxData,
    int maxStrikes,
    InventoryItem.ITEM_TYPE drop,
    float dropChance)
  {
    this.vfxData = vfxData;
    this.maxStrikes = maxStrikes;
    this.drop = drop;
    this.dropChance = dropChance;
  }

  public virtual void Play(
    GameObject owner,
    Health.Team enemyTeam,
    float damage,
    PlayerFarming playerFarming,
    bool ignoreImmuneEnemies = false,
    List<Health> customTargets = null,
    string customImpactSFX = "")
  {
  }

  public void SetAttackFlags(Health.AttackFlags attackFlags) => this.attackFlags = attackFlags;

  public void AddAttackFlags(Health.AttackFlags attackFlags) => this.attackFlags |= attackFlags;

  public List<Health> GetTargets(Health.Team team)
  {
    List<Health> source = new List<Health>();
    switch (team)
    {
      case Health.Team.Neutral:
        source = new List<Health>((IEnumerable<Health>) Health.neutralTeam);
        break;
      case Health.Team.PlayerTeam:
        source = new List<Health>((IEnumerable<Health>) Health.playerTeam);
        break;
      case Health.Team.Team2:
        source = new List<Health>((IEnumerable<Health>) Health.team2);
        break;
      case Health.Team.KillAll:
        source = new List<Health>((IEnumerable<Health>) Health.playerTeam);
        source.AddRange((IEnumerable<Health>) Health.team2);
        source.AddRange((IEnumerable<Health>) Health.neutralTeam);
        break;
    }
    for (int index = source.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) source[index] == (UnityEngine.Object) null)
        source.RemoveAt(index);
    }
    List<Health> list = source.OrderBy<Health, float>((Func<Health, float>) (x => x.HP)).Reverse<Health>().ToList<Health>();
    list.RemoveAll((Predicate<Health>) (t => t.InanimateObject));
    return list;
  }

  public List<Health> GetImmuneTargets(List<Health> targets)
  {
    List<Health> immuneTargets = new List<Health>((IEnumerable<Health>) targets);
    for (int index = immuneTargets.Count - 1; index >= 0; --index)
    {
      if (!this.IsImmune(targets[index]))
        immuneTargets.RemoveAt(index);
    }
    return immuneTargets;
  }

  public List<Health> GetVulnerableTargets(List<Health> targets)
  {
    List<Health> vulnerableTargets = new List<Health>((IEnumerable<Health>) targets);
    for (int index = vulnerableTargets.Count - 1; index >= 0; --index)
    {
      if (this.IsImmune(targets[index]))
        vulnerableTargets.RemoveAt(index);
    }
    return vulnerableTargets;
  }

  public bool IsImmune(Health target)
  {
    return (UnityEngine.Object) target == (UnityEngine.Object) null || target.invincible || target.untouchable || !target.enabled || !target.gameObject.activeInHierarchy;
  }
}
