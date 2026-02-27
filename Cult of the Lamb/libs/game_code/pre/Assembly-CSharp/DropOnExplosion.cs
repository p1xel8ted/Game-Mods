// Decompiled with JetBrains decompiler
// Type: DropOnExplosion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class DropOnExplosion : MonoBehaviour
{
  public EnemyExploder EnemyExploder;
  [SerializeField]
  private GameObject poisonPrefab;
  [SerializeField]
  private int amount = 3;
  [SerializeField]
  private float radius = 1f;
  public AssetReferenceGameObject[] EnemyList;
  public int NumToSpawn = 5;
  [SerializeField]
  private float growSpeed = 0.3f;
  [SerializeField]
  private Ease growEase = Ease.OutCubic;
  [SerializeField]
  private float spawnSpitOutForce = 0.7f;

  private void OnEnable() => this.EnemyExploder.OnExplode += new System.Action(this.OnExplode);

  private void OnDisable() => this.EnemyExploder.OnExplode -= new System.Action(this.OnExplode);

  private void OnExplode()
  {
    for (int index = 0; index < this.amount; ++index)
      UnityEngine.Object.Instantiate<GameObject>(this.poisonPrefab, this.transform.position + (Vector3) (UnityEngine.Random.insideUnitCircle * this.radius), Quaternion.identity, this.transform.parent);
    for (int index = 0; index < this.NumToSpawn; ++index)
    {
      Health.team2.Add((Health) null);
      Interaction_Chest.Instance?.Enemies.Add((Health) null);
      Vector3 position = this.transform.position;
      float f = (float) (360.0 / (double) this.NumToSpawn * (double) index * (Math.PI / 180.0));
      Vector3 direction = new Vector3(Mathf.Cos(f), Mathf.Sin(f));
      Addressables.InstantiateAsync((object) this.EnemyList[UnityEngine.Random.Range(0, this.EnemyList.Length)], position, Quaternion.identity, this.transform.parent).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        if (Health.team2.Contains((Health) null))
        {
          Health.team2.Remove((Health) null);
          Interaction_Chest.Instance?.Enemies.Remove((Health) null);
        }
        EnemyExploder component1 = obj.Result.GetComponent<EnemyExploder>();
        component1.givePath(component1.transform.position + direction * 2f);
        EnemyRoundsBase.Instance?.AddEnemyToRound(component1.GetComponent<Health>());
        Interaction_Chest.Instance?.AddEnemy(obj.Result.GetComponent<Health>());
        if ((double) this.growSpeed != 0.0)
        {
          component1.Spine.transform.localScale = Vector3.zero;
          component1.Spine.transform.DOScale(1f, this.growSpeed).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(this.growEase);
        }
        float angle = Utils.GetAngle(this.transform.position, this.transform.position + direction) * ((float) Math.PI / 180f);
        component1.DoKnockBack(angle, this.spawnSpitOutForce, 0.75f);
        component1.chase = true;
        DropLootOnDeath component2 = component1.GetComponent<DropLootOnDeath>();
        if (!(bool) (UnityEngine.Object) component2)
          return;
        component2.GiveXP = false;
      });
    }
  }
}
