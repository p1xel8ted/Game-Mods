// Decompiled with JetBrains decompiler
// Type: MortarBomb
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using FMOD.Studio;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class MortarBomb : BaseMonoBehaviour, ISpellOwning
{
  public GameObject BombVisual;
  public SpriteRenderer Target;
  public SpriteRenderer TargetWarning;
  public CircleCollider2D circleCollider2D;
  public GameObject BombShadow;
  public ParticleSystem SmokeParticles;
  public DOTweenAnimation rotationAnimation;
  public float moveDuration = 1f;
  public float arcHeight = 2f;
  public AnimationCurve arcCurve;
  public Health.Team bombTeam;
  public Health owner;
  public SkeletonAnimation parentSpine;
  public bool doHaptics;
  public EventInstance customInstanceSFX;
  public Health Origin;
  public bool destroyOnFinish = true;

  public float SpineTimeScale
  {
    get => !(bool) (Object) this.parentSpine ? 1f : this.parentSpine.timeScale;
  }

  public void OnEnable()
  {
    this.StartCoroutine((IEnumerator) this.ScaleCircle());
    this.BombVisual.SetActive(false);
    this.BombShadow.SetActive(false);
  }

  public void OnDisable()
  {
    AudioManager.Instance.StopOneShotInstanceEarly(this.customInstanceSFX, STOP_MODE.IMMEDIATE);
    if (!this.destroyOnFinish)
      return;
    Object.Destroy((Object) this.gameObject);
  }

  public void Play(
    Vector3 Position,
    float moveDuration,
    Health.Team bombTeam,
    Health owner = null,
    bool PlayDefaultSFX = true,
    string customSFX = "",
    SkeletonAnimation parentSpine = null,
    bool doHaptics = false)
  {
    this.moveDuration = moveDuration;
    this.bombTeam = bombTeam;
    this.owner = owner;
    this.parentSpine = parentSpine;
    this.doHaptics = doHaptics;
    this.StartCoroutine((IEnumerator) this.MoveRock(Position));
    this.StartCoroutine((IEnumerator) this.FlashCircle());
    if (PlayDefaultSFX)
    {
      AudioManager.Instance.PlayOneShot("event:/enemy/fly_spawn", this.gameObject);
      AudioManager.Instance.PlayOneShot("event:/enemy/vocals/frog_large/attack", this.gameObject);
    }
    if (string.IsNullOrEmpty(customSFX))
      return;
    this.customInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(customSFX, this.transform);
  }

  public IEnumerator ScaleCircle()
  {
    float Scale = 0.0f;
    while ((double) (Scale += Time.deltaTime * 8f * this.SpineTimeScale) <= 1.0)
    {
      this.Target.transform.localScale = Vector3.one * this.circleCollider2D.radius * Scale;
      this.TargetWarning.transform.localScale = Vector3.one * this.circleCollider2D.radius * Scale;
      yield return (object) null;
    }
  }

  public void Awake() => this.rotationAnimation = this.BombVisual.GetComponent<DOTweenAnimation>();

  public void Update()
  {
    this.HandleRotationAnimation();
    this.Target.transform.Rotate(new Vector3(0.0f, 0.0f, 150f) * Time.deltaTime * this.SpineTimeScale);
  }

  public void HandleRotationAnimation()
  {
    if ((Object) this.rotationAnimation == (Object) null)
      return;
    if (PlayerRelic.TimeFrozen)
    {
      this.rotationAnimation.DOPause();
    }
    else
    {
      if (this.rotationAnimation.tween.IsPlaying())
        return;
      this.rotationAnimation.DOPlay();
    }
  }

  public IEnumerator FlashCircle()
  {
    while ((double) Vector2.Distance((Vector2) this.BombVisual.transform.localPosition, (Vector2) Vector3.zero) >= 6.0)
      yield return (object) null;
    Color white = new Color(1f, 1f, 1f, 1f);
    Color color = white;
    float flashTickTimer = 0.0f;
    while ((double) Vector2.Distance((Vector2) this.BombVisual.transform.localPosition, (Vector2) Vector3.zero) < 6.0)
    {
      if ((double) flashTickTimer >= 0.11999999731779099 && (double) Time.timeScale == 1.0 && BiomeConstants.Instance.IsFlashLightsActive)
      {
        this.Target.material.SetColor("_Color", color = color == white ? Color.red : white);
        this.TargetWarning.material.SetColor("_Color", color);
        flashTickTimer = 0.0f;
      }
      flashTickTimer += Time.deltaTime * this.SpineTimeScale;
      yield return (object) null;
    }
  }

  public virtual IEnumerator MoveRock(Vector3 startPos)
  {
    MortarBomb mortarBomb = this;
    mortarBomb.BombVisual.SetActive(true);
    mortarBomb.BombShadow.SetActive(true);
    mortarBomb.BombVisual.transform.position = startPos;
    Vector2 targetPos = (Vector2) mortarBomb.transform.position;
    double num = (double) Mathf.Max(mortarBomb.moveDuration, (float) ((double) mortarBomb.moveDuration * (double) Vector2.Distance((Vector2) startPos, targetPos) / 3.0));
    float t = 0.0f;
    while ((double) t < (double) mortarBomb.moveDuration)
    {
      if (!PlayerRelic.TimeFrozen)
      {
        t += Time.deltaTime * mortarBomb.SpineTimeScale;
        mortarBomb.BombVisual.transform.position = Vector3.Lerp(startPos, (Vector3) targetPos, t / mortarBomb.moveDuration);
        mortarBomb.BombShadow.transform.position = Vector3.Lerp(startPos, (Vector3) targetPos, t / mortarBomb.moveDuration);
        mortarBomb.BombShadow.transform.localScale = Vector3.one * mortarBomb.circleCollider2D.radius * (float) (1.5 - (double) Mathf.Clamp01(mortarBomb.arcCurve.Evaluate(t / mortarBomb.moveDuration)) * 0.5);
        mortarBomb.BombVisual.transform.position += new Vector3(0.0f, 0.0f, -mortarBomb.arcCurve.Evaluate(t / mortarBomb.moveDuration) * mortarBomb.arcHeight);
      }
      yield return (object) null;
    }
    AudioManager.Instance.StopOneShotInstanceEarly(mortarBomb.customInstanceSFX, STOP_MODE.IMMEDIATE);
    if (mortarBomb.doHaptics)
      MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
    Explosion.CreateExplosion(mortarBomb.transform.position, mortarBomb.bombTeam, mortarBomb.owner, mortarBomb.circleCollider2D.radius, 1f);
    if ((Object) mortarBomb.SmokeParticles != (Object) null)
    {
      mortarBomb.SmokeParticles.transform.parent = mortarBomb.transform.parent;
      mortarBomb.SmokeParticles.Stop();
    }
    if (mortarBomb.destroyOnFinish)
      Object.Destroy((Object) mortarBomb.gameObject);
    else if ((Object) mortarBomb != (Object) null)
      mortarBomb.gameObject.Recycle();
  }

  public GameObject GetOwner()
  {
    return !((Object) this.Origin != (Object) null) ? (GameObject) null : this.Origin.gameObject;
  }

  public void SetOwner(GameObject owner) => this.Origin = owner.GetComponent<Health>();
}
