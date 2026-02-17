// Decompiled with JetBrains decompiler
// Type: WolfArmPiece
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class WolfArmPiece : MonoBehaviour
{
  [SerializeField]
  public SkeletonAnimation spine;
  [SerializeField]
  public DamageCollider damageCollider;
  [SerializeField]
  public SimpleSpineFlash simpleSpineFlash;
  [SerializeField]
  public bool lookAtCamera = true;
  [SerializeField]
  public Vector3 rotationOffset;
  [SerializeField]
  public bool isHand;
  [SerializeField]
  public SpriteRenderer handAttachment;
  [SerializeField]
  public GameObject lightningEffect;
  public Rigidbody rigidbody;
  public HingeJoint joint;
  public Quaternion restingRotation;
  public Health health;
  public static Dictionary<int, WolfArmPiece> Lookup = new Dictionary<int, WolfArmPiece>();
  public int _id;
  public Renderer handAttachmentRenderer;
  public int nextAllowedFlashFrame = -1;
  public Coroutine handFlashCo;
  public MaterialPropertyBlock mpb;
  public static int ID_FillColor = Shader.PropertyToID("_FillColor");
  public static int ID_FillAlpha = Shader.PropertyToID("_FillAlpha");
  public static WaitForSeconds W06 = new WaitForSeconds(0.06f);
  public static WaitForSeconds W03 = new WaitForSeconds(0.03f);
  public static WaitForSeconds W02 = new WaitForSeconds(0.02f);

  public SkeletonAnimation Spine => this.spine;

  public Rigidbody Rigidbody => this.rigidbody;

  public Vector3 RotationOffset
  {
    get => this.rotationOffset;
    set => this.rotationOffset = value;
  }

  public Health Health => this.health;

  public void OnEnable()
  {
    this._id = this.gameObject.GetInstanceID();
    WolfArmPiece.Lookup[this._id] = this;
  }

  public void OnDisable() => WolfArmPiece.Lookup.Remove(this._id);

  public void Awake()
  {
    this.rigidbody = this.GetComponent<Rigidbody>();
    this.joint = this.GetComponent<HingeJoint>();
    this.health = this.GetComponentInChildren<Health>();
    this.health.OnHit += new Health.HitAction(this.Health_OnHit);
    this.handAttachmentRenderer = this.spine.gameObject.GetComponent<Renderer>();
    this.StartCoroutine((IEnumerator) this.WaitForResting());
    if (!this.isHand)
      return;
    this.SetHandAttachmentColor(new Color(1f, 1f, 1f, 0.0f));
  }

  public void OnDestroy() => this.health.OnHit -= new Health.HitAction(this.Health_OnHit);

  public IEnumerator WaitForResting()
  {
    WolfArmPiece wolfArmPiece = this;
    while (!wolfArmPiece.rigidbody.IsSleeping())
      yield return (object) null;
    wolfArmPiece.restingRotation = wolfArmPiece.transform.localRotation;
  }

  public void Update()
  {
    if (!this.lookAtCamera)
      return;
    this.spine.transform.LookAt(GameManager.GetInstance().CamFollowTarget.transform);
    this.spine.transform.eulerAngles += this.rotationOffset;
  }

  public void SetDamageCollider(bool enabled)
  {
    this.damageCollider.enabled = enabled;
    this.damageCollider.damageCollider.enabled = enabled;
  }

  public void SetSpine(bool enabled)
  {
    if (!enabled)
    {
      this.transform.localRotation = this.restingRotation;
      this.simpleSpineFlash.ResetColour();
    }
    this.transform.localRotation = this.restingRotation;
    this.Spine.gameObject.SetActive(enabled);
    this.health.invincible = !enabled;
    this.health.CircleCollider2D.enabled = enabled;
  }

  public void FlashWhite(float amount)
  {
    if ((double) amount <= 0.0)
      this.simpleSpineFlash.FlashWhite(false);
    else
      this.simpleSpineFlash.FlashWhite(amount);
    if (!this.isHand)
      return;
    this.SetHandAttachmentColor(new Color(1f, 1f, 1f, amount / 2f));
  }

  public void Health_OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    if ((bool) (Object) Attacker.GetComponentInParent<EnemyWolfBoss>())
      return;
    float Damage = (this.health.totalHP - this.health.HP) * 0.1f;
    this.health.HP = this.health.totalHP;
    EnemyWolfBoss.Instance.health.DealDamage(Damage, this.gameObject, AttackLocation, AttackType: Health.AttackTypes.NoHitStop);
    this.FlashFillRed();
  }

  public void ResetColour()
  {
    this.simpleSpineFlash.ResetColour();
    if (!this.isHand)
      return;
    this.SetHandAttachmentColor(new Color(0.0f, 0.0f, 0.0f, 0.0f));
  }

  public void FlashFillRed()
  {
    if (!SettingsManager.Settings.Accessibility.FlashingLights || Time.frameCount < this.nextAllowedFlashFrame)
      return;
    this.nextAllowedFlashFrame = Time.frameCount + 10;
    this.simpleSpineFlash.FlashFillRed();
    if (!this.isHand || this.isHand && !((Object) this.handAttachmentRenderer == (Object) null))
      return;
    if (this.handFlashCo == null)
    {
      this.StopCoroutine(this.handFlashCo);
      this.handFlashCo = (Coroutine) null;
    }
    this.handFlashCo = this.StartCoroutine((IEnumerator) this.FlashHandAttachmentRed(0.5f));
  }

  public IEnumerator FlashHandAttachmentRed(float opacity)
  {
    this.SetHandAttachmentColor(new Color(1f, 1f, 1f, opacity));
    yield return (object) WolfArmPiece.W06;
    this.SetHandAttachmentColor(new Color(0.0f, 0.0f, 0.0f, opacity));
    yield return (object) WolfArmPiece.W03;
    this.SetHandAttachmentColor(new Color(1f, 0.0f, 0.0f, opacity));
    yield return (object) WolfArmPiece.W02;
    this.SetHandAttachmentColor(new Color(0.0f, 0.0f, 0.0f, opacity));
    yield return (object) WolfArmPiece.W02;
    this.SetHandAttachmentColor(new Color(1f, 0.0f, 0.0f, opacity));
    yield return (object) WolfArmPiece.W02;
    this.SetHandAttachmentColor(new Color(1f, 0.0f, 0.0f, 0.0f));
    this.handFlashCo = (Coroutine) null;
  }

  public void EnsureMPB()
  {
    if (this.mpb != null)
      return;
    this.mpb = new MaterialPropertyBlock();
  }

  public void SetHandAttachmentColor(Color color)
  {
    if ((Object) this.handAttachmentRenderer == (Object) null)
      return;
    this.EnsureMPB();
    this.handAttachmentRenderer.GetPropertyBlock(this.mpb);
    this.mpb.SetColor(WolfArmPiece.ID_FillColor, color);
    this.mpb.SetFloat(WolfArmPiece.ID_FillAlpha, color.a);
    this.handAttachmentRenderer.SetPropertyBlock(this.mpb);
  }
}
