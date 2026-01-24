// Decompiled with JetBrains decompiler
// Type: TrapFleshGrass
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using MMBiomeGeneration;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TrapFleshGrass : BaseMonoBehaviour
{
  [SerializeField]
  public TrapFleshGrassNode node;
  [SerializeField]
  public int initialNodesAmount;
  [SerializeField]
  public float initialNodesDistance;
  [SerializeField]
  public float randomAngle;
  [SerializeField]
  public List<TrapFleshGrassNode> children = new List<TrapFleshGrassNode>();
  [SerializeField]
  public float checkChildrenRate;
  [SerializeField]
  public LayerMask obstaclesLayermask;
  [SerializeField]
  public float obstaclesRadiusCheck;
  public float creationTimeStamp;

  public void OnEnable()
  {
    DOVirtual.DelayedCall(2f, new TweenCallback(this.CreateInitialNodes));
    this.creationTimeStamp = GameManager.GetInstance().CurrentTime;
  }

  public void Update()
  {
    if ((double) GameManager.GetInstance().TimeSince(this.creationTimeStamp) <= (double) this.checkChildrenRate)
      return;
    this.CreateMissingChildren();
  }

  public void CreateInitialNodes()
  {
    float num1 = 360f / (float) this.initialNodesAmount;
    float num2 = UnityEngine.Random.value * 360f;
    for (int index = 0; index < this.initialNodesAmount; ++index)
    {
      float num3 = num2 + num1 * (float) index + UnityEngine.Random.Range(-this.randomAngle, this.randomAngle);
      Vector2 direction = new Vector2(Mathf.Cos(num3 * ((float) Math.PI / 180f)), Mathf.Sin(num3 * ((float) Math.PI / 180f))) * this.initialNodesDistance;
      TrapFleshGrassNode trapFleshGrassNode = ObjectPool.Spawn<TrapFleshGrassNode>(this.node, this.transform.parent, (Vector3) ((Vector2) this.transform.position + direction), Quaternion.identity);
      trapFleshGrassNode.Init(direction, (TrapFleshGrassNode) null, true);
      trapFleshGrassNode.GetComponent<Health>().OnDie += new Health.DieAction(this.OnChildrenNodeDie);
      this.children.Add(trapFleshGrassNode);
    }
  }

  public void CreateMissingChildren()
  {
    int num1 = this.initialNodesAmount - this.children.Count;
    for (int index = 0; index < num1; ++index)
    {
      float num2 = UnityEngine.Random.value * 360f;
      Vector2 direction = new Vector2(Mathf.Cos(num2 * ((float) Math.PI / 180f)), Mathf.Sin(num2 * ((float) Math.PI / 180f))) * this.initialNodesDistance;
      Vector3 vector3 = (Vector3) ((Vector2) this.transform.position + direction);
      if (this.CanSpawnAtPoint((Vector2) vector3))
      {
        TrapFleshGrassNode trapFleshGrassNode = ObjectPool.Spawn<TrapFleshGrassNode>(this.node, this.transform.parent, vector3, Quaternion.identity);
        trapFleshGrassNode.Init(direction, (TrapFleshGrassNode) null, true);
        trapFleshGrassNode.GetComponent<Health>().OnDie += new Health.DieAction(this.OnChildrenNodeDie);
        this.children.Add(trapFleshGrassNode);
      }
    }
    this.creationTimeStamp = GameManager.GetInstance().CurrentTime;
  }

  public bool CanSpawnAtPoint(Vector2 point)
  {
    Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(point, this.obstaclesRadiusCheck, (int) this.obstaclesLayermask);
    return BiomeGenerator.PointWithinIsland((Vector3) point, out Vector3 _) && collider2DArray.Length == 0;
  }

  public void OnChildrenNodeDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    TrapFleshGrassNode component = Victim.GetComponent<TrapFleshGrassNode>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !this.children.Contains(component))
      return;
    this.children.Remove(component);
    this.creationTimeStamp = GameManager.GetInstance().CurrentTime;
  }
}
