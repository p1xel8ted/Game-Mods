// Decompiled with JetBrains decompiler
// Type: EnemyRounds
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemyRounds : EnemyRoundsBase
{
  public List<Health> EnemyList = new List<Health>();
  public Gradient GizmoColorGradient;
  public List<EnemyRounds.RoundsOfEnemies> Rounds = new List<EnemyRounds.RoundsOfEnemies>();
  public UnityEvent Callback;
  public int currentRound = -1;
  public bool allEnemiesSpawned = true;
  public List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();

  public override int TotalRounds => this.Rounds.Count + this.EnemyList.Count <= 0 ? 0 : 1;

  public override int CurrentRound => this.currentRound + 1;

  public void OnDestroy()
  {
    if (this.loadedAddressableAssets == null)
      return;
    foreach (AsyncOperationHandle<GameObject> addressableAsset in this.loadedAddressableAssets)
      Addressables.Release((AsyncOperationHandle) addressableAsset);
    this.loadedAddressableAssets.Clear();
  }

  public override void BeginCombat(bool showRoundsUI, System.Action ActionCallback)
  {
    base.BeginCombat(showRoundsUI, ActionCallback);
    this.StartCoroutine((IEnumerator) this.SpawnNewEnemies());
  }

  public new void OnEnable()
  {
    if (!this.combatBegan)
      return;
    this.BeginCombat(this.showRoundsUI, this.actionCallback);
  }

  public IEnumerator SpawnNewEnemies()
  {
    EnemyRounds enemyRounds1 = this;
    EnemyRoundsBase.Instance = (EnemyRoundsBase) enemyRounds1;
    bool allEnemiesKilled;
    if (enemyRounds1.currentRound == -1)
    {
      do
      {
        allEnemiesKilled = true;
        foreach (UnityEngine.Object enemy in enemyRounds1.EnemyList)
        {
          if (enemy != (UnityEngine.Object) null)
            allEnemiesKilled = false;
        }
        yield return (object) null;
      }
      while (!allEnemiesKilled);
    }
    enemyRounds1.RoundStarted(1, enemyRounds1.TotalRounds);
    Debug.Log((object) "AA");
    if (enemyRounds1.Rounds != null && enemyRounds1.Rounds.Count > 0)
    {
      while (enemyRounds1.currentRound < enemyRounds1.Rounds.Count)
      {
        if (enemyRounds1.currentRound != -1)
        {
          do
          {
            allEnemiesKilled = true;
            foreach (EnemyRounds.EnemyAndPosition enemyAndPosition in enemyRounds1.Rounds[enemyRounds1.currentRound].Round)
            {
              if ((UnityEngine.Object) enemyAndPosition.Enemy != (UnityEngine.Object) null)
                allEnemiesKilled = false;
            }
            yield return (object) null;
          }
          while (!allEnemiesKilled || !enemyRounds1.allEnemiesSpawned);
        }
        ++enemyRounds1.currentRound;
        enemyRounds1.RoundStarted(enemyRounds1.currentRound + (enemyRounds1.EnemyList.Count > 0 ? 1 : 0), enemyRounds1.TotalRounds);
        if (enemyRounds1.currentRound < enemyRounds1.Rounds.Count)
        {
          EnemyRounds enemyRounds = enemyRounds1;
          enemyRounds1.allEnemiesSpawned = false;
          int count = 0;
          for (int i = 0; i < enemyRounds1.Rounds[enemyRounds1.currentRound].Round.Count; ++i)
          {
            GameObject g;
            Addressables.LoadAssetAsync<GameObject>((object) enemyRounds1.Rounds[enemyRounds1.currentRound].Round[i].EnemyTarget).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
            {
              closure_1.loadedAddressableAssets.Add(obj);
              g = ObjectPool.Spawn(obj.Result, closure_1.transform.parent);
              g.transform.position = closure_1.transform.position + closure_1.Rounds[closure_1.currentRound].Round[i].Position;
              g.GetComponent<UnitObject>().RemoveModifier();
              g.GetComponent<UnitObject>().CanHaveModifier = false;
              g.SetActive(false);
              EnemySpawner.CreateWithAndInitInstantiatedEnemy(closure_1.transform.position + closure_1.Rounds[closure_1.currentRound].Round[i].Position, closure_1.transform.parent, g);
              closure_1.Rounds[closure_1.currentRound].Round[i].Enemy = g.GetComponent<Health>();
              ++count;
              closure_1.allEnemiesSpawned = count >= closure_1.Rounds[closure_1.currentRound].Round.Count;
            });
            yield return (object) new WaitForSeconds(enemyRounds1.Rounds[enemyRounds1.currentRound].Round[i].Delay);
          }
        }
      }
    }
    Debug.Log((object) "DO CALLBACKS!");
    enemyRounds1.Callback?.Invoke();
    System.Action actionCallback = enemyRounds1.actionCallback;
    if (actionCallback != null)
      actionCallback();
    UIEnemyRoundsHUD.Hide();
    AudioManager.Instance.SetMusicCombatState(false);
    Debug.Log((object) "Chest Reveal!");
    Interaction_Chest.Instance?.Reveal();
    EnemyRoundsBase.Instance = (EnemyRoundsBase) null;
  }

  public override void AddEnemyToRound(Health e)
  {
    foreach (EnemyRounds.EnemyAndPosition enemyAndPosition in this.Rounds[this.currentRound].Round)
    {
      if ((UnityEngine.Object) enemyAndPosition.Enemy == (UnityEngine.Object) e)
        return;
    }
    this.Rounds[this.currentRound].Round.Add(new EnemyRounds.EnemyAndPosition(e, Vector3.zero, 0.0f));
    base.AddEnemyToRound(e);
  }

  public void OnDrawGizmos()
  {
    foreach (Health enemy in this.EnemyList)
    {
      if ((UnityEngine.Object) enemy != (UnityEngine.Object) null)
        Utils.DrawLine(this.transform.position, enemy.transform.position, Color.yellow);
    }
    int index = -1;
    while (++index < this.Rounds.Count)
    {
      EnemyRounds.RoundsOfEnemies round = this.Rounds[index];
      if (round.DisplayGizmo)
      {
        foreach (EnemyRounds.EnemyAndPosition enemyAndPosition in round.Round)
          Utils.DrawCircleXY(this.transform.position + enemyAndPosition.Position, 0.2f, this.GizmoColorGradient.Evaluate((float) index / ((float) this.Rounds.Count - 1f)));
      }
    }
  }

  [Serializable]
  public class RoundsOfEnemies
  {
    public bool DisplayGizmo;
    public List<EnemyRounds.EnemyAndPosition> Round = new List<EnemyRounds.EnemyAndPosition>();
  }

  [Serializable]
  public class EnemyAndPosition
  {
    public AssetReferenceGameObject EnemyTarget;
    public Vector3 Position;
    [CompilerGenerated]
    public Health \u003CEnemy\u003Ek__BackingField;
    public float Delay = 1f;

    public Health Enemy
    {
      get => this.\u003CEnemy\u003Ek__BackingField;
      set => this.\u003CEnemy\u003Ek__BackingField = value;
    }

    public EnemyAndPosition(Health e, Vector3 p, float d)
    {
      this.Enemy = e;
      this.Position = p;
      this.Delay = d;
    }
  }
}
