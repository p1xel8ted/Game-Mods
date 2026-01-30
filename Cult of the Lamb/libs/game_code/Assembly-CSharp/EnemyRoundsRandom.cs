// Decompiled with JetBrains decompiler
// Type: EnemyRoundsRandom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemyRoundsRandom : EnemyRoundsBase
{
  [SerializeField]
  public List<UnitObject> startingEnemies;
  [Space]
  [SerializeField]
  public EnemyRoundsRandom.Round[] rounds;
  [SerializeField]
  public EnemyRoundsRandom.SpawnPoints[] spawnPointSets = new EnemyRoundsRandom.SpawnPoints[0];
  [SerializeField]
  public EnemyRoundsRandom.EnemySet[] enemySets;
  public List<UnitObject> currentSpawnedEnemies = new List<UnitObject>();
  public bool beganRounds;
  public int roundIndex = -1;
  public bool allEnemiesSpawned = true;
  public List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();
  public Dictionary<int, List<UnitObject>> enemyRounds = new Dictionary<int, List<UnitObject>>();
  public bool isPreloaded;

  public override int TotalRounds => this.rounds.Length + (this.startingEnemies.Count > 0 ? 1 : 0);

  public override int CurrentRound => this.roundIndex + 1;

  public void OnDestroy()
  {
    if (this.loadedAddressableAssets == null)
      return;
    foreach (AsyncOperationHandle<GameObject> addressableAsset in this.loadedAddressableAssets)
      Addressables.Release((AsyncOperationHandle) addressableAsset);
    this.loadedAddressableAssets.Clear();
  }

  public void Update()
  {
    if (!this.beganRounds && this.AllEnemiesDead(this.startingEnemies))
      this.beganRounds = true;
    if (!this.combatBegan)
      return;
    if (this.beganRounds && this.roundIndex < this.rounds.Length - 1)
    {
      if (!this.AllEnemiesDead(this.currentSpawnedEnemies))
        return;
      this.currentSpawnedEnemies.Clear();
      ++this.roundIndex;
      this.RoundStarted(this.roundIndex + (this.startingEnemies.Count > 0 ? 1 : 0), this.TotalRounds);
      this.SpawnEnemies(this.roundIndex);
    }
    else
    {
      if (this.roundIndex < this.rounds.Length - 1 || this.Completed || !this.AllEnemiesDead(this.currentSpawnedEnemies))
        return;
      UIEnemyRoundsHUD.Hide();
      System.Action actionCallback = this.actionCallback;
      if (actionCallback != null)
        actionCallback();
      this.Completed = true;
    }
  }

  public override void AddEnemyToRound(Health e)
  {
    if (this.currentSpawnedEnemies == null || !((UnityEngine.Object) e != (UnityEngine.Object) null) || this.currentSpawnedEnemies.Contains(e.GetComponent<UnitObject>()))
      return;
    this.currentSpawnedEnemies.Add(e.GetComponent<UnitObject>());
    Interaction_Chest.Instance?.AddEnemy(e);
    base.AddEnemyToRound(e);
  }

  public void PreloadEnemies()
  {
    if (this.isPreloaded)
      return;
    this.isPreloaded = true;
    for (int i = 0; i < this.rounds.Length; i++)
    {
      List<AssetReferenceGameObject> enemyRound = this.GetEnemyRound(this.rounds[i]);
      if (enemyRound != null)
      {
        int targetAmount = enemyRound.Count;
        int count = 0;
        bool allRoundSpawned = false;
        List<UnitObject> units = new List<UnitObject>();
        for (int index = 0; index < enemyRound.Count; ++index)
        {
          if (ResourceManager.AssetExists(enemyRound[index].RuntimeKey))
          {
            AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>((object) enemyRound[index]);
            asyncOperationHandle.Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
            {
              this.loadedAddressableAssets.Add(obj);
              if ((UnityEngine.Object) obj.Result != (UnityEngine.Object) null)
              {
                UnitObject component = UnityEngine.Object.Instantiate<GameObject>(obj.Result, this.transform.parent).GetComponent<UnitObject>();
                component.RemoveModifier();
                component.CanHaveModifier = false;
                component.gameObject.SetActive(false);
                units.Add(component);
              }
              ++count;
              allRoundSpawned = count >= targetAmount;
              if (!allRoundSpawned)
                return;
              this.enemyRounds.Add(i, units);
            });
            asyncOperationHandle.WaitForCompletion();
          }
        }
      }
    }
  }

  public void SpawnEnemies(int round)
  {
    if (this.enemyRounds.Count <= 0)
    {
      Debug.LogWarning((object) "Preload was not called! Loading now");
      this.PreloadEnemies();
    }
    if (!this.enemyRounds.ContainsKey(round))
    {
      this.allEnemiesSpawned = true;
    }
    else
    {
      List<UnitObject> enemyRound = this.enemyRounds[round];
      this.allEnemiesSpawned = false;
      int count = enemyRound.Count;
      int num = 0;
      for (int index = 0; index < enemyRound.Count; ++index)
      {
        this.AddEnemyToRound(enemyRound[index].GetComponent<Health>());
        ++num;
        this.allEnemiesSpawned = num >= count;
        if (this.allEnemiesSpawned)
          this.StartCoroutine((IEnumerator) this.SetEnemyPositions(this.currentSpawnedEnemies));
      }
    }
  }

  public IEnumerator SetEnemyPositions(List<UnitObject> enemies)
  {
    EnemyRoundsRandom enemyRoundsRandom = this;
    foreach (EnemyRoundsRandom.SpawnPoints spawnPointSet in enemyRoundsRandom.spawnPointSets)
    {
      if (enemies.Count == spawnPointSet.Positions.Length)
      {
        List<Vector3> spawnPoints = ((IEnumerable<Vector3>) spawnPointSet.Positions).ToList<Vector3>();
        for (int i = 0; i < enemies.Count; ++i)
        {
          Vector3 vector3 = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];
          spawnPoints.Remove(vector3);
          enemies[i].transform.position = vector3;
          enemies[i].gameObject.SetActive(true);
          EnemySpawner.CreateWithAndInitInstantiatedEnemy(enemies[i].transform.position, enemyRoundsRandom.transform.parent, enemies[i].gameObject);
          if ((double) enemyRoundsRandom.SpawnDelay != 0.0)
            yield return (object) new WaitForSeconds(enemyRoundsRandom.SpawnDelay);
        }
        yield break;
      }
    }
    Debug.LogWarning((object) $"POSITION SET FOR '{enemies.Count.ToString()}' DOESN'T EXIST");
  }

  public bool AllEnemiesDead(List<UnitObject> enemies)
  {
    bool flag = true;
    foreach (UnityEngine.Object enemy in enemies)
    {
      if (enemy != (UnityEngine.Object) null)
      {
        flag = false;
        break;
      }
    }
    return flag && this.allEnemiesSpawned;
  }

  public List<AssetReferenceGameObject> GetEnemyRound(EnemyRoundsRandom.Round round)
  {
    List<AssetReferenceGameObject> enemyRound = new List<AssetReferenceGameObject>();
    List<EnemyRoundsRandom.EnemySet> enemySetBetweenScore = this.GetEnemySetBetweenScore(round.MinEnemyScore, round.MaxEnemyScore);
    int num1 = Mathf.Clamp(round.TargetScore + DifficultyManager.GetEnemyRoundsScoreOffset(), 2, int.MaxValue);
    int num2 = 0;
    int num3 = 0;
    if (enemySetBetweenScore.Count <= 0)
      return (List<AssetReferenceGameObject>) null;
    while (num2 != num1 && num3++ < 200)
    {
      EnemyRoundsRandom.EnemySet enemySet = enemySetBetweenScore[UnityEngine.Random.Range(0, enemySetBetweenScore.Count)];
      enemyRound.Add(enemySet.EnemyList[UnityEngine.Random.Range(0, enemySet.EnemyList.Length)]);
      num2 += enemySet.Score;
      if (num2 > num1 || enemyRound.Count > round.MaxEnemies || num2 == num1 && enemyRound.Count < Mathf.Max(round.MinEnemies, this.spawnPointSets[0].Positions.Length))
      {
        enemyRound.Clear();
        num2 = 0;
      }
    }
    if (enemyRound.Count < this.spawnPointSets[0].Positions.Length)
    {
      enemyRound.Clear();
      for (int index = 0; index < this.spawnPointSets[0].Positions.Length; ++index)
        enemyRound.Add(enemySetBetweenScore[UnityEngine.Random.Range(0, enemySetBetweenScore.Count)].EnemyList[0]);
    }
    return enemyRound;
  }

  public List<EnemyRoundsRandom.EnemySet> GetEnemySetBetweenScore(
    int minEnemyScore,
    int maxEnemyScore)
  {
    List<EnemyRoundsRandom.EnemySet> enemySetBetweenScore = new List<EnemyRoundsRandom.EnemySet>();
    foreach (EnemyRoundsRandom.EnemySet enemySet in this.enemySets)
    {
      if (enemySet.Score >= minEnemyScore && (maxEnemyScore == 0 || enemySet.Score <= maxEnemyScore))
        enemySetBetweenScore.Add(enemySet);
    }
    return enemySetBetweenScore;
  }

  public void OnDrawGizmosSelected()
  {
    foreach (EnemyRoundsRandom.SpawnPoints spawnPointSet in this.spawnPointSets)
    {
      if (spawnPointSet.Debug)
      {
        foreach (Vector3 position in spawnPointSet.Positions)
          Utils.DrawCircleXY(position, 0.5f, Color.blue);
      }
    }
  }

  [Serializable]
  public struct SpawnPoints
  {
    public Vector3[] Positions;
    public bool Debug;
  }

  [Serializable]
  public struct EnemySet
  {
    public AssetReferenceGameObject[] EnemyList;
    public int Score;
  }

  [Serializable]
  public struct Round
  {
    public int TargetScore;
    public int MinEnemyScore;
    public int MaxEnemyScore;
    public int MinEnemies;
    public int MaxEnemies;
  }
}
