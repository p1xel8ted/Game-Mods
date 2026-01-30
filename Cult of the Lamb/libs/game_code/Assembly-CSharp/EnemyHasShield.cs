// Decompiled with JetBrains decompiler
// Type: EnemyHasShield
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using Spine;
using Spine.Unity;
using src.UI.Overlays.TutorialOverlay;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class EnemyHasShield : BaseMonoBehaviour
{
  public System.Action OnDestroyShield;
  public bool forceShieldChance;
  public float layer1ShieldChanceOverride = -1f;
  public float layer2ShieldChanceOverride = -1f;
  public bool allowBeforeGotHeavyAttack;
  public bool dropBlackSoulsWhileShielded = true;
  public Health health;
  public SkeletonAnimation Spine;
  [SpineSkin("", "", true, false, false, dataField = "Spine")]
  public string SkinWithShield;
  [SpineSkin("", "", true, false, false, dataField = "Spine")]
  public string SkinNoShield;
  public float CameraShake = 2f;
  public int maxParticles = 10;
  public List<Sprite> ParticleChunks;
  public DropLootOnHit lootDropper;
  public static System.Action OnTutorialShown;

  public void Start()
  {
    if (!this.forceShieldChance || PlayerFleeceManager.FleeceSwapsWeaponForCurse())
      return;
    float num = UnityEngine.Random.value;
    bool flag = false;
    if (GameManager.Layer2 && (double) num < (double) this.layer2ShieldChanceOverride)
      flag = true;
    else if ((double) num < (double) this.layer1ShieldChanceOverride)
      flag = true;
    if (!flag)
      return;
    this.AddShield();
  }

  public void OnEnable()
  {
    this.health = this.GetComponent<Health>();
    if (!this.health.HasShield)
      return;
    this.health.OnPenetrationHit += new Health.HitAction(this.Health_OnPenetrationHit);
    this.health.OnHit += new Health.HitAction(this.Health_OnHit);
  }

  public void AddShield()
  {
    if ((bool) (UnityEngine.Object) this.health && this.health.HasShield || !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HeavyAttacks))
      return;
    if (this.SetShieldSkin())
    {
      this.health.HasShield = true;
      this.health.BlackSoulOnHit = true;
      this.health.OnPenetrationHit += new Health.HitAction(this.Health_OnPenetrationHit);
      this.health.OnHit += new Health.HitAction(this.Health_OnHit);
      Debug.Log((object) "Enemy given shield");
      if (!this.dropBlackSoulsWhileShielded)
        return;
      this.lootDropper = this.gameObject.GetComponent<DropLootOnHit>();
      if (!((UnityEngine.Object) this.lootDropper == (UnityEngine.Object) null))
        return;
      this.lootDropper = this.gameObject.AddComponent<DropLootOnHit>();
      this.lootDropper.LootToDrop = InventoryItem.ITEM_TYPE.BLACK_SOUL;
      this.lootDropper.DontDropOnPlayerFullAmmo = true;
      Debug.Log((object) "Enemy given black soul loot dropper");
    }
    else
      Debug.Log((object) "Enemy not given shield");
  }

  public bool SetShieldSkin()
  {
    Skin newSkin = new Skin("New Skin");
    if (newSkin == null || this.SkinWithShield == null || this.Spine.Skeleton == null || this.Spine.Skeleton.Data == null || this.Spine.Skeleton.Data.FindSkin(this.SkinWithShield) == null)
      return false;
    newSkin.AddSkin(this.Spine.Skeleton.Data.FindSkin(this.SkinWithShield));
    this.Spine.Skeleton.SetSkin(newSkin);
    this.Spine.skeleton.SetSlotsToSetupPose();
    return true;
  }

  public void SetNoShieldSkin()
  {
    new Skin("New Skin").AddSkin(this.Spine.Skeleton.Data.FindSkin(this.SkinWithShield));
    this.Spine.Skeleton.SetSkin(this.SkinNoShield);
    this.Spine.skeleton.SetSlotsToSetupPose();
  }

  public void OnDisable()
  {
    this.health.OnPenetrationHit -= new Health.HitAction(this.Health_OnPenetrationHit);
    this.health.OnHit -= new Health.HitAction(this.Health_OnHit);
  }

  public void Health_OnPenetrationHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (AttackType != Health.AttackTypes.Heavy || !this.health.HasShield)
      return;
    this.health.OnPenetrationHit -= new Health.HitAction(this.Health_OnPenetrationHit);
    this.health.OnHit -= new Health.HitAction(this.Health_OnHit);
    this.health.HasShield = false;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.lootDropper);
    this.SetNoShieldSkin();
    this.DestroyShieldFX(Attacker);
    GameManager.GetInstance().HitStop();
    System.Action onDestroyShield = this.OnDestroyShield;
    if (onDestroyShield == null)
      return;
    onDestroyShield();
  }

  public void Health_OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (!this.health.HasShield || !DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.ShieldedEnemies))
      return;
    this.StartCoroutine((IEnumerator) this.WaitForEndOfFrame());
  }

  public IEnumerator WaitForEndOfFrame()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyHasShield enemyHasShield = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      UITutorialOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.ShieldedEnemies);
      overlayController.OnHidden = overlayController.OnHidden + new System.Action(enemyHasShield.\u003CWaitForEndOfFrame\u003Eb__23_0);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    enemyHasShield.health.untouchable = true;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSecondsRealtime(0.1f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void DestroyShieldFX(GameObject Attacker)
  {
    AudioManager.Instance.PlayOneShot("event:/enemy/shield_break", this.gameObject);
    CameraManager.shakeCamera(this.CameraShake);
    int num = -1;
    if (this.ParticleChunks == null || this.ParticleChunks.Count <= 0)
      return;
    while (++num < this.maxParticles)
      Particle_Chunk.AddNew(this.transform.position, Utils.GetAngle(Attacker.transform.position, this.transform.position) + (float) UnityEngine.Random.Range(-20, 20), this.ParticleChunks);
  }

  [CompilerGenerated]
  public void \u003CWaitForEndOfFrame\u003Eb__23_0()
  {
    this.health.untouchable = false;
    System.Action onTutorialShown = EnemyHasShield.OnTutorialShown;
    if (onTutorialShown == null)
      return;
    onTutorialShown();
  }
}
