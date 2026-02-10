// Decompiled with JetBrains decompiler
// Type: WarriorTrioManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMODUnity;
using I2.Loc;
using MMBiomeGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class WarriorTrioManager : UnitObject
{
  [SerializeField]
  public EnemyWolfGuardian[] warriors;
  public WarriorTrioManager.States[] states = new WarriorTrioManager.States[5]
  {
    new WarriorTrioManager.States(0.9f, 1),
    new WarriorTrioManager.States(0.75f, 1),
    new WarriorTrioManager.States(0.55f, 2),
    new WarriorTrioManager.States(0.35f, 2),
    new WarriorTrioManager.States(0.2f, 2)
  };
  [SerializeField]
  public int finalPhaseHP = 30;
  public List<EnemyWolfGuardian> ActiveWarriors = new List<EnemyWolfGuardian>();
  public List<EnemyWolfGuardian> InActiveWarriors = new List<EnemyWolfGuardian>();
  public List<int> warriorsBeenActive = new List<int>();
  public Vector3[] spawnPositions = new Vector3[3]
  {
    new Vector3(0.0f, 0.0f, 0.0f),
    new Vector3(3f, 0.0f, 0.0f),
    new Vector3(-3f, 0.0f, 0.0f)
  };
  public Vector3[] inActivePositions = new Vector3[2]
  {
    new Vector3(7f, 0.0f, -3f),
    new Vector3(-7f, 0.0f, -3f)
  };
  public int currentState = -1;
  public float healthBarRefillTime = 2f;
  public int falseDeathCount;
  public int finalPhaseDeathCount;
  public bool IsInFinalPhase;
  public bool isActive;
  [CompilerGenerated]
  public EnemyWolfGuardian \u003CLastWarriorHit\u003Ek__BackingField;
  public Coroutine currentCombatRoutine;
  [EventRef]
  public string IntroSwoopOutSFX = "event:/dlc/dungeon05/enemy/miniboss_wolf_guardian/intro_swoop_out";
  [EventRef]
  public string PhaseTwoStartSFX = "event:/dlc/dungeon05/enemy/miniboss_wolf_guardian/phase_02_start";
  public string bossName = "NAMES/MiniBoss/Dungeon5/MiniBoss4";

  public EnemyWolfGuardian LastWarriorHit
  {
    get => this.\u003CLastWarriorHit\u003Ek__BackingField;
    set => this.\u003CLastWarriorHit\u003Ek__BackingField = value;
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if (!this.isActive || (double) this.health.HP <= 0.0)
      return;
    if (this.currentCombatRoutine != null)
      this.StopCoroutine(this.currentCombatRoutine);
    if (!this.IsInFinalPhase)
      this.currentCombatRoutine = this.StartCoroutine((IEnumerator) this.CombatRoutine());
    UIBossHUD.Play(this.health, LocalizationManager.GetTranslation(this.bossName));
  }

  public void StartCombat() => this.StartCoroutine((IEnumerator) this.PreCombatSequence());

  public override void OnDieEarly(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDieEarly(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    if (this.IsInFinalPhase)
      return;
    this.SetAllWarriorsInvincible(true);
  }

  public void OnWarriorDamaged(EnemyWolfGuardian warrior)
  {
    this.LastWarriorHit = warrior;
    float Damage = warrior.health.totalHP - warrior.health.HP;
    if (this.currentState == this.states.Length - 1 || this.IsInFinalPhase)
    {
      float num = 0.0f;
      foreach (EnemyWolfGuardian activeWarrior in this.ActiveWarriors)
        num += activeWarrior.health.HP;
      this.health.HP = num;
      UIBossHUD.Instance?.OnBossHit(this.gameObject, this.transform.position, Health.AttackTypes.Bullet);
      if ((double) this.health.HP > 0.0 && this.finalPhaseDeathCount < 3)
        return;
      this.transform.position = warrior.transform.position;
      this.health.DealDamage(999f, this.gameObject, this.transform.position);
    }
    else
    {
      warrior.health.HP = warrior.health.totalHP;
      this.health.DealDamage(Damage, this.gameObject, this.transform.position);
    }
  }

  public void IncrementFalseDeaths()
  {
    ++this.falseDeathCount;
    if (this.falseDeathCount < 2 || this.IsInFinalPhase)
      return;
    this.IsInFinalPhase = true;
    this.StartCoroutine((IEnumerator) this.FinalPhaseSequence());
  }

  public void IncrementFinalPhaseDeaths()
  {
    if (!this.IsInFinalPhase)
      return;
    ++this.finalPhaseDeathCount;
  }

  public bool IsAttackInProgress(EnemyWolfGuardian.StateType attackType)
  {
    foreach (EnemyWolfGuardian warrior in this.warriors)
    {
      if ((double) warrior.health.HP > 0.0 && warrior.IsAttackInProgress(attackType))
        return true;
    }
    return false;
  }

  public void SetAllWarriorsInvincible(bool state)
  {
    this.health.invincible = state;
    foreach (EnemyWolfGuardian warrior in this.warriors)
    {
      warrior.health.invincible = state;
      warrior.isImmuneToKnockback = state;
    }
  }

  public IEnumerator PreCombatSequence()
  {
    WarriorTrioManager warriorTrioManager = this;
    EnemyWolfGuardian startingWarrior = warriorTrioManager.warriors[UnityEngine.Random.Range(0, warriorTrioManager.warriors.Length)];
    warriorTrioManager.InActiveWarriors = new List<EnemyWolfGuardian>((IEnumerable<EnemyWolfGuardian>) warriorTrioManager.GetOtherWarriors(startingWarrior));
    warriorTrioManager.ActiveWarriors.Add(startingWarrior);
    warriorTrioManager.warriorsBeenActive.Add(warriorTrioManager.warriors.IndexOf<EnemyWolfGuardian>(startingWarrior));
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    GameManager.GetInstance().CamFollowTarget.ClearAllTargets();
    GameManager.GetInstance().AddPlayersToCamera();
    foreach (EnemyWolfGuardian warrior in warriorTrioManager.warriors)
    {
      GameManager.GetInstance().AddToCamera(warrior.gameObject);
      warrior.enabled = true;
    }
    for (int index = 0; index < warriorTrioManager.InActiveWarriors.Count; ++index)
    {
      warriorTrioManager.InActiveWarriors[index].StopAttack();
      warriorTrioManager.InActiveWarriors[index].GoToInActiveState(warriorTrioManager.inActivePositions[index], 0.5f);
      warriorTrioManager.InActiveWarriors[index].InActivePositionIndex = index;
    }
    if (!string.IsNullOrEmpty(warriorTrioManager.IntroSwoopOutSFX))
      AudioManager.Instance.PlayOneShot(warriorTrioManager.IntroSwoopOutSFX);
    float tauntAnimDuration = startingWarrior.GetTauntAnimDuration();
    AudioManager.Instance.PlayOneShot(startingWarrior.IntroDrawWeaponSFX);
    startingWarrior.GoToTauntState();
    MMVibrate.RumbleContinuous(1.5f, 1.75f);
    yield return (object) new WaitForSeconds(tauntAnimDuration);
    MMVibrate.StopRumble();
    startingWarrior.GoToIdleState();
    warriorTrioManager.isActive = true;
    warriorTrioManager.currentCombatRoutine = warriorTrioManager.StartCoroutine((IEnumerator) warriorTrioManager.CombatRoutine());
  }

  public IEnumerator CombatRoutine()
  {
    WarriorTrioManager warriorTrioManager = this;
    for (int i = 0; i < warriorTrioManager.states.Length; ++i)
    {
      while ((double) warriorTrioManager.health.HP > (double) warriorTrioManager.health.totalHP * (double) warriorTrioManager.states[i].hpPercentage)
        yield return (object) null;
      if (i > warriorTrioManager.currentState)
      {
        warriorTrioManager.currentState = i;
        warriorTrioManager.SwapWarriors(warriorTrioManager.states[i].activeWarriors);
      }
    }
    float num = warriorTrioManager.health.HP / 2f;
    foreach (EnemyWolfGuardian activeWarrior in warriorTrioManager.ActiveWarriors)
    {
      activeWarrior.health.totalHP = num;
      activeWarrior.health.HP = num;
    }
  }

  public IEnumerator FinalPhaseSequence()
  {
    WarriorTrioManager warriorTrioManager = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.SetGlobalOcclusionActive(false);
    warriorTrioManager.ClearStatusEffects();
    EnemyWolfGuardian finalWarrior = warriorTrioManager.InActiveWarriors[0];
    List<EnemyWolfGuardian> downedWarriors = warriorTrioManager.GetOtherWarriors(finalWarrior);
    yield return (object) warriorTrioManager.StartCoroutine((IEnumerator) warriorTrioManager.BringInFinalWarrior(finalWarrior));
    float tauntAnimDuration = finalWarrior.GetTauntAnimDuration();
    finalWarrior.GoToTauntState();
    MMVibrate.RumbleContinuous(1.5f, 1.75f);
    yield return (object) new WaitForSeconds(tauntAnimDuration);
    MMVibrate.StopRumble();
    yield return (object) warriorTrioManager.StartCoroutine((IEnumerator) warriorTrioManager.ReactivateFalseDeathWarriors());
    warriorTrioManager.ActiveWarriors.Add(finalWarrior);
    warriorTrioManager.health.totalHP = (float) warriorTrioManager.finalPhaseHP;
    warriorTrioManager.health.HP = (float) warriorTrioManager.finalPhaseHP;
    foreach (EnemyWolfGuardian enemyWolfGuardian in downedWarriors)
      enemyWolfGuardian.GoToTauntState();
    yield return (object) warriorTrioManager.StartCoroutine((IEnumerator) warriorTrioManager.RefillHealthBar());
    float num = warriorTrioManager.health.HP / 3f;
    foreach (EnemyWolfGuardian warrior in warriorTrioManager.warriors)
    {
      warrior.health.totalHP = num;
      warrior.health.HP = num;
    }
    warriorTrioManager.InActiveWarriors.Clear();
    warriorTrioManager.ActiveWarriors.Clear();
    foreach (EnemyWolfGuardian warrior in warriorTrioManager.warriors)
      warriorTrioManager.ActiveWarriors.Add(warrior);
    warriorTrioManager.SetAllWarriorsToIdleState();
    warriorTrioManager.SetAllWarriorsInvincible(false);
    GameManager.GetInstance().OnConversationEnd();
    GameManager.SetGlobalOcclusionActive(true);
  }

  public void ClearStatusEffects()
  {
    foreach (EnemyWolfGuardian warrior in this.warriors)
    {
      warrior.health.RemoveBurn();
      warrior.health.ClearIce();
      warrior.health.ClearPoison();
      warrior.health.ClearElectrified();
    }
  }

  public void SwapWarriors(int warriorsToActivate)
  {
    List<EnemyWolfGuardian> enemyWolfGuardianList1 = new List<EnemyWolfGuardian>((IEnumerable<EnemyWolfGuardian>) this.warriors);
    List<EnemyWolfGuardian> enemyWolfGuardianList2 = new List<EnemyWolfGuardian>();
    if (this.warriorsBeenActive.Count >= this.warriors.Length)
      this.warriorsBeenActive.Clear();
    for (int index = 0; index < this.ActiveWarriors.Count; ++index)
      enemyWolfGuardianList1.Remove(this.ActiveWarriors[index]);
    for (int index = 0; index < this.warriors.Length; ++index)
    {
      if (this.warriorsBeenActive.Contains(index) && enemyWolfGuardianList1.Contains(this.warriors[index]))
        enemyWolfGuardianList1.Remove(this.warriors[index]);
    }
    for (int index = 0; index < warriorsToActivate; ++index)
    {
      if (enemyWolfGuardianList1.Count > 0)
      {
        enemyWolfGuardianList2.Add(enemyWolfGuardianList1[0]);
        enemyWolfGuardianList1.RemoveAt(0);
      }
      else
      {
        bool flag = false;
        foreach (EnemyWolfGuardian inActiveWarrior in this.InActiveWarriors)
        {
          if (!enemyWolfGuardianList2.Contains(inActiveWarrior))
          {
            enemyWolfGuardianList2.Add(inActiveWarrior);
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          EnemyWolfGuardian warrior;
          do
          {
            warrior = this.warriors[UnityEngine.Random.Range(0, this.warriors.Length)];
          }
          while (enemyWolfGuardianList2.Contains(warrior) || !((UnityEngine.Object) this.LastWarriorHit != (UnityEngine.Object) warrior));
          enemyWolfGuardianList2.Add(warrior);
        }
      }
    }
    List<int> intList = new List<int>() { 0, 1 };
    for (int index = 0; index < this.InActiveWarriors.Count; ++index)
      intList.Remove(this.InActiveWarriors[index].InActivePositionIndex);
    for (int index = 0; index < enemyWolfGuardianList2.Count; ++index)
    {
      if (!this.ActiveWarriors.Contains(enemyWolfGuardianList2[index]) && !intList.Contains(enemyWolfGuardianList2[index].InActivePositionIndex))
        intList.Add(enemyWolfGuardianList2[index].InActivePositionIndex);
    }
    int index1 = 0;
    for (int index2 = 0; index2 < this.ActiveWarriors.Count; ++index2)
    {
      if (!enemyWolfGuardianList2.Contains(this.ActiveWarriors[index2]))
      {
        this.ActiveWarriors[index2].StopAttack();
        if (index1 < intList.Count)
        {
          this.ActiveWarriors[index2].GoToInActiveState(this.inActivePositions[intList[index1]], 0.5f);
          this.ActiveWarriors[index2].InActivePositionIndex = intList[index1];
          int num = index1 + 1;
          break;
        }
        break;
      }
    }
    for (int index3 = 0; index3 < enemyWolfGuardianList2.Count; ++index3)
    {
      if (this.InActiveWarriors.Contains(enemyWolfGuardianList2[index3]))
      {
        enemyWolfGuardianList2[index3].GoToActiveState();
        if (!this.warriorsBeenActive.Contains(this.warriors.IndexOf<EnemyWolfGuardian>(enemyWolfGuardianList2[index3])))
          this.warriorsBeenActive.Add(this.warriors.IndexOf<EnemyWolfGuardian>(enemyWolfGuardianList2[index3]));
      }
    }
    this.ActiveWarriors = enemyWolfGuardianList2;
    this.InActiveWarriors.Clear();
    for (int index4 = 0; index4 < this.warriors.Length; ++index4)
    {
      if (!enemyWolfGuardianList2.Contains(this.warriors[index4]))
        this.InActiveWarriors.Add(this.warriors[index4]);
    }
  }

  public IEnumerator BringInFinalWarrior(EnemyWolfGuardian finalWarrior)
  {
    finalWarrior.GoToWaitStateForCutscene(this.GetEmptySpaceNearPosition(Vector3.zero));
    finalWarrior.health.invincible = true;
    AudioManager.Instance.PlayOneShot(this.PhaseTwoStartSFX);
    yield return (object) new WaitForSeconds(1.5f);
  }

  public Vector3 GetEmptySpaceNearPosition(Vector3 positionToCheck)
  {
    Vector3 closestPoint = positionToCheck;
    float radius = 1.5f;
    float num1 = 1f;
    float num2 = 100f;
    while ((double) --num2 > 0.0)
    {
      if ((UnityEngine.Object) Physics2D.OverlapCircle((Vector2) closestPoint, radius, (int) this.layerToCheck) == (UnityEngine.Object) null)
        return closestPoint;
      float f = (float) UnityEngine.Random.Range(0, 360) * ((float) Math.PI / 180f);
      closestPoint += new Vector3(Mathf.Cos(f), Mathf.Sin(f)) * num1;
      BiomeGenerator.PointWithinIsland(closestPoint, out closestPoint);
    }
    return positionToCheck;
  }

  public IEnumerator ReactivateFalseDeathWarriors()
  {
    foreach (EnemyWolfGuardian activeWarrior in this.ActiveWarriors)
    {
      if (activeWarrior.IsInFalseDeathState())
        activeWarrior.GoToFalseDeathRecoveryState();
    }
    yield return (object) new WaitForSeconds(this.warriors[0].GetRecoveryAnimDuration());
  }

  public IEnumerator RefillHealthBar()
  {
    UIBossHUD.Instance?.ForceHealthAmount(1f, this.healthBarRefillTime);
    yield return (object) new WaitForSeconds(this.healthBarRefillTime);
  }

  public void SetAllWarriorsToIdleState()
  {
    foreach (EnemyWolfGuardian activeWarrior in this.ActiveWarriors)
      activeWarrior.GoToIdleState();
  }

  public List<EnemyWolfGuardian> GetOtherWarriors(EnemyWolfGuardian warrior)
  {
    List<EnemyWolfGuardian> otherWarriors = new List<EnemyWolfGuardian>((IEnumerable<EnemyWolfGuardian>) this.warriors);
    otherWarriors.Remove(warrior);
    return otherWarriors;
  }

  [Serializable]
  public struct States(float hpPercentage, int activeWarriors)
  {
    public float hpPercentage = hpPercentage;
    public int activeWarriors = activeWarriors;
  }
}
