// Decompiled with JetBrains decompiler
// Type: TrapFleshGrassNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TrapFleshGrassNode : BaseMonoBehaviour
{
  [SerializeField]
  public SkeletonAnimation spine;
  [SerializeField]
  public ColliderEvents colliderEvents;
  [SerializeField]
  public Health health;
  [SerializeField]
  public TrapFleshGrassNode node;
  [SerializeField]
  public float damage;
  [SerializeField]
  public float randomAngle;
  [SerializeField]
  public float spawnDistance;
  [SerializeField]
  public float spawnTime;
  [SerializeField]
  public float spawnTimeRandomMargin;
  [SerializeField]
  public LayerMask obstaclesLayermask;
  [SerializeField]
  public float obstaclesRadiusCheck;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float probCreateAnotherNode;
  public TrapFleshGrassNode parent;
  public List<TrapFleshGrassNode> children = new List<TrapFleshGrassNode>();
  public bool isOriginal;
  public Vector2 spawnDirection;
  public float creationTimeStamp;

  public bool ShouldCreateChildren => this.children.Count == 0 && this.IsConnectedToTree();

  public Vector2 SpawnDirection
  {
    get => this.children.Count != 0 ? this.spawnDirection * -1f : this.spawnDirection;
  }

  public TrapFleshGrassNode Parent => this.parent;

  public void OnEnable()
  {
    this.colliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
  }

  public void OnDisable()
  {
    this.colliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    foreach (Component child in this.children)
      child.GetComponent<Health>().OnDie -= new Health.DieAction(this.OnChildrenNodeDie);
  }

  public void Update()
  {
    if (!this.ShouldCreateChildren || (double) GameManager.GetInstance().TimeSince(this.creationTimeStamp) <= (double) this.spawnTime)
      return;
    this.CreateNextNode();
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((Object) component != (Object) null))
      return;
    if (component.team != this.health.team)
    {
      component.DealDamage(this.damage, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
    }
    else
    {
      if (this.health.team != Health.Team.PlayerTeam || this.health.isPlayerAlly || component.isPlayer)
        return;
      component.DealDamage(this.damage, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
    }
  }

  public void Init(Vector2 direction, TrapFleshGrassNode parent, bool isOriginal = false)
  {
    this.spine.skeleton.ScaleX = (double) Random.value > 0.5 ? 1f : -1f;
    this.spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    this.spawnDirection = direction.normalized;
    this.creationTimeStamp = GameManager.GetInstance().CurrentTime;
    this.parent = parent;
    this.isOriginal = isOriginal;
    this.children = new List<TrapFleshGrassNode>();
  }

  public void CreateNextNode()
  {
    int num = (double) Random.value < (double) this.probCreateAnotherNode ? 2 : 1;
    List<TrapFleshGrassNode> collection = new List<TrapFleshGrassNode>();
    for (int index = 0; index < num; ++index)
    {
      float angle = Random.Range(-this.randomAngle, this.randomAngle);
      if (num > 1)
        angle = (float) (-(double) this.randomAngle + (double) (2 * index) * (double) this.randomAngle);
      Vector2 direction = (Vector2) (Quaternion.AngleAxis(angle, Vector3.forward) * (Vector3) this.SpawnDirection) * this.spawnDistance;
      Vector2 vector2 = (Vector2) this.transform.position + direction;
      if (this.CanSpawnAtPoint(vector2))
      {
        TrapFleshGrassNode trapFleshGrassNode = ObjectPool.Spawn<TrapFleshGrassNode>(this.node, this.transform.parent, (Vector3) vector2, Quaternion.identity);
        trapFleshGrassNode.Init(direction, this);
        trapFleshGrassNode.GetComponent<Health>().OnDie += new Health.DieAction(this.OnChildrenNodeDie);
        collection.Add(trapFleshGrassNode);
      }
    }
    this.children.AddRange((IEnumerable<TrapFleshGrassNode>) collection);
    this.creationTimeStamp = GameManager.GetInstance().CurrentTime;
  }

  public bool CanSpawnAtPoint(Vector2 point)
  {
    Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(point, this.obstaclesRadiusCheck, (int) this.obstaclesLayermask);
    foreach (Component component1 in collider2DArray)
    {
      TrapFleshGrassNode component2 = component1.GetComponent<TrapFleshGrassNode>();
      if ((bool) (Object) component2 && (Object) component2.parent == (Object) null)
      {
        component2.parent = this;
        if (!this.children.Contains(component2))
          this.children.Add(component2);
      }
    }
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
    if (!((Object) component != (Object) null) || !this.children.Contains(component))
      return;
    this.children.Remove(component);
    this.creationTimeStamp = GameManager.GetInstance().CurrentTime;
  }

  public bool IsConnectedToTree()
  {
    TrapFleshGrassNode trapFleshGrassNode = this;
    while ((Object) trapFleshGrassNode != (Object) null && !((Object) trapFleshGrassNode.Parent == (Object) null))
      trapFleshGrassNode = trapFleshGrassNode.Parent;
    return trapFleshGrassNode.isOriginal;
  }
}
