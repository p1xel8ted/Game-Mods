// Decompiled with JetBrains decompiler
// Type: Tree
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Tree : BaseMonoBehaviour
{
  public Health health;
  public float rotateY;
  public float rotateSpeedY;
  public GameObject image;
  public float RotationToCamera = -60f;
  public GameObject lighting;
  public GameObject halo;
  public SkeletonAnimation Spine;
  public Sprite Image;
  public Sprite ImageStump;
  public SpriteRenderer spriteRenderer;
  public CircleCollider2D collider;
  [HideInInspector]
  public bool Dead;
  [HideInInspector]
  public GameObject TaskDoer;
  public static List<Tree> Trees = new List<Tree>();
  public ParticleSystem[] particleSystems;

  public void Start()
  {
    this.health = this.GetComponent<Health>();
    this.health.OnHit += new Health.HitAction(this.OnHit);
    this.health.OnDie += new Health.DieAction(this.OnDie);
  }

  public void OnEnable() => Tree.Trees.Add(this);

  public void OnDisable() => Tree.Trees.Remove(this);

  public void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if ((double) this.health.HP > (double) this.health.totalHP / 2.0)
      this.Spine.skeleton.SetSkin("normal-chop1");
    else
      this.Spine.skeleton.SetSkin("normal-chop2");
    BiomeConstants.Instance.EmitHitVFX(this.transform.position - Vector3.down * Random.Range(0.2f, 1.5f), Quaternion.identity.z, "HitFX_Weak");
    if (!((Object) this.Spine != (Object) null))
      return;
    this.Spine.AnimationState.SetAnimation(0, "hit", true);
    this.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
  }

  public void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if ((Object) this.lighting != (Object) null)
      Object.Destroy((Object) this.lighting);
    if ((Object) this.halo != (Object) null)
      Object.Destroy((Object) this.halo);
    this.rotateSpeedY = (float) ((5.0 + (double) Random.Range(-2, 2)) * ((double) Attacker.transform.position.x < (double) this.transform.position.x ? -1.0 : 1.0));
    CameraManager.shakeCamera(0.25f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
    if ((Object) DungeonDecorator.getInsance() != (Object) null)
      DungeonDecorator.getInsance().UpdateStructures(NavigateRooms.r, this.transform.position, 5);
    if ((Object) this.spriteRenderer != (Object) null)
      this.spriteRenderer.sprite = this.ImageStump;
    if ((Object) this.collider != (Object) null)
      this.collider.isTrigger = true;
    if ((Object) this.Spine != (Object) null)
    {
      this.Spine.skeleton.SetSkin("cut");
      this.Spine.skeleton.SetSlotsToSetupPose();
    }
    foreach (ParticleSystem particleSystem in this.particleSystems)
      particleSystem.Stop();
    this.Dead = true;
    this.TaskDoer = (GameObject) null;
  }

  public void StartAsStump()
  {
    Object.Destroy((Object) this.GetComponent<Health>());
    if ((Object) this.spriteRenderer != (Object) null)
      this.spriteRenderer.sprite = this.ImageStump;
    if ((Object) this.Spine != (Object) null)
    {
      this.Spine.skeleton.SetSkin("cut");
      this.Spine.skeleton.SetSlotsToSetupPose();
    }
    foreach (ParticleSystem particleSystem in this.particleSystems)
      particleSystem.Stop();
    this.Dead = true;
  }

  public void Update()
  {
    this.rotateSpeedY += (float) ((0.0 - (double) this.rotateY) * 0.10000000149011612);
    this.rotateY += (this.rotateSpeedY *= 0.8f);
    if ((Object) this.image != (Object) null)
      this.image.transform.eulerAngles = new Vector3(this.RotationToCamera, this.rotateY, 0.0f);
    if (!((Object) this.Spine != (Object) null))
      return;
    this.Spine.transform.eulerAngles = new Vector3(this.RotationToCamera, this.rotateY, 0.0f);
  }

  public void onDestroy()
  {
    this.health.OnHit -= new Health.HitAction(this.OnHit);
    this.health.OnDie -= new Health.DieAction(this.OnDie);
  }
}
