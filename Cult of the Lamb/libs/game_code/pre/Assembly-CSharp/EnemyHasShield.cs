// Decompiled with JetBrains decompiler
// Type: EnemyHasShield
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyHasShield : BaseMonoBehaviour
{
  [Range(0.0f, 1f)]
  public float Probability = 1f;
  private bool Randomised;
  private bool Activate;
  private Health health;
  public SkeletonAnimation Spine;
  [SpineSkin("", "", true, false, false, dataField = "Spine")]
  public string SkinWithShield;
  [SpineSkin("", "", true, false, false, dataField = "Spine")]
  public string SkinNoShield;
  public float CameraShake = 2f;
  public int maxParticles = 10;
  public List<Sprite> ParticleChunks;

  private void OnEnable()
  {
    this.health = this.GetComponent<Health>();
    if (!CrownAbilities.CrownAbilityUnlocked(CrownAbilities.TYPE.Combat_HeavyAttack))
      return;
    if (!this.Randomised)
    {
      if ((double) Random.value <= (double) this.Probability)
        this.Activate = true;
      this.Randomised = true;
    }
    if (!this.Activate)
      return;
    this.health.HasShield = true;
    this.health.OnHit += new Health.HitAction(this.Health_OnHit);
  }

  private void Start()
  {
    if (!this.Activate)
      return;
    this.SetShieldSkin();
  }

  private void SetShieldSkin()
  {
    Skin newSkin = new Skin("New Skin");
    newSkin.AddSkin(this.Spine.Skeleton.Data.FindSkin(this.SkinWithShield));
    this.Spine.Skeleton.SetSkin(newSkin);
    this.Spine.skeleton.SetSlotsToSetupPose();
  }

  private void SetNoShieldSkin()
  {
    new Skin("New Skin").AddSkin(this.Spine.Skeleton.Data.FindSkin(this.SkinWithShield));
    this.Spine.Skeleton.SetSkin(this.SkinNoShield);
    this.Spine.skeleton.SetSlotsToSetupPose();
  }

  private void OnDisable() => this.health.OnHit -= new Health.HitAction(this.Health_OnHit);

  private void Health_OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (AttackType != Health.AttackTypes.Heavy || !this.health.HasShield)
      return;
    this.health.OnHit -= new Health.HitAction(this.Health_OnHit);
    this.SetNoShieldSkin();
    this.DestroyShieldFX(Attacker);
  }

  private void DestroyShieldFX(GameObject Attacker)
  {
    CameraManager.shakeCamera(this.CameraShake);
    int num = -1;
    if (this.ParticleChunks.Count <= 0)
      return;
    while (++num < this.maxParticles)
      Particle_Chunk.AddNew(this.transform.position, Utils.GetAngle(Attacker.transform.position, this.transform.position) + (float) Random.Range(-20, 20), this.ParticleChunks);
  }
}
