// Decompiled with JetBrains decompiler
// Type: EnemyBomb
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class EnemyBomb : BaseMonoBehaviour, ISpellOwning
{
  public GameObject BombVisual;
  public SpriteRenderer Target;
  public SpriteRenderer TargetWarning;
  public CircleCollider2D circleCollider2D;
  public GameObject BombShadow;
  public float moveDuration = 1f;
  public float bombArcHeight = 2f;
  public AnimationCurve bombArcCurve;
  public bool RotateBomb;
  public Transform rotationTransform;
  public float RotationSpeed = 1f;
  public Health.Team team;
  public GameObject owner;
  public SkeletonAnimation parentSpine;

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

  public virtual void Play(
    Vector3 Position,
    float moveDuration,
    Health.Team team = Health.Team.Team2,
    SkeletonAnimation parentSpine = null)
  {
    this.moveDuration = moveDuration;
    this.team = team;
    this.parentSpine = parentSpine;
    this.StartCoroutine((IEnumerator) this.MoveRock(Position));
    this.StartCoroutine((IEnumerator) this.FlashCircle());
  }

  public void Update()
  {
    this.Target.transform.Rotate(new Vector3(0.0f, 0.0f, 150f) * Time.deltaTime);
  }

  public IEnumerator MoveRock(Vector3 startPos)
  {
    EnemyBomb enemyBomb = this;
    enemyBomb.BombVisual.SetActive(true);
    enemyBomb.BombShadow.SetActive(true);
    enemyBomb.BombVisual.transform.position = startPos;
    Vector2 targetPos = (Vector2) enemyBomb.transform.position;
    float t = 0.0f;
    while ((double) t < (double) enemyBomb.moveDuration)
    {
      if (!PlayerRelic.TimeFrozen || enemyBomb.team == Health.Team.PlayerTeam)
      {
        t += Time.deltaTime * enemyBomb.SpineTimeScale;
        enemyBomb.BombVisual.transform.position = Vector3.Lerp(startPos, (Vector3) targetPos, t / enemyBomb.moveDuration);
        enemyBomb.BombShadow.transform.position = Vector3.Lerp(startPos, (Vector3) targetPos, t / enemyBomb.moveDuration);
        enemyBomb.BombShadow.transform.localScale = Vector3.one * (float) (1.5 - (double) Mathf.Clamp01(enemyBomb.bombArcCurve.Evaluate(t / enemyBomb.moveDuration)) * 0.5);
        enemyBomb.BombVisual.transform.position += new Vector3(0.0f, 0.0f, -enemyBomb.bombArcCurve.Evaluate(t / enemyBomb.moveDuration) * enemyBomb.bombArcHeight);
        if (enemyBomb.RotateBomb && (Object) enemyBomb.rotationTransform != (Object) null)
        {
          Vector3 eulerAngles = Quaternion.LookRotation(new Vector3(targetPos.x, targetPos.y, 0.0f) - startPos, Vector3.up).eulerAngles;
          enemyBomb.rotationTransform.transform.rotation = Quaternion.Euler(eulerAngles);
          enemyBomb.rotationTransform.transform.Rotate(Vector3.up, 500f * enemyBomb.RotationSpeed * t, Space.Self);
        }
      }
      yield return (object) null;
    }
    enemyBomb.BombLanded();
    ObjectPool.Recycle(enemyBomb.gameObject);
  }

  public virtual void BombLanded()
  {
  }

  public IEnumerator ScaleCircle()
  {
    float Scale = 0.0f;
    while ((double) (Scale += Time.deltaTime * 8f * this.SpineTimeScale) <= (double) this.circleCollider2D.radius)
    {
      this.Target.transform.localScale = Vector3.one * Scale;
      this.TargetWarning.transform.localScale = Vector3.one * Scale;
      yield return (object) null;
    }
  }

  public IEnumerator FlashCircle()
  {
    while ((double) Vector2.Distance((Vector2) this.BombVisual.transform.localPosition, (Vector2) Vector3.zero) >= 6.0)
      yield return (object) null;
    float flashTickTimer = 0.0f;
    Color white = new Color(1f, 1f, 1f, 1f);
    Color color = white;
    while ((double) Vector2.Distance((Vector2) this.BombVisual.transform.localPosition, (Vector2) Vector3.zero) < 6.0)
    {
      if ((double) flashTickTimer >= 0.11999999731779099 && BiomeConstants.Instance.IsFlashLightsActive)
      {
        this.Target.material.SetColor("_Color", color = color == white ? Color.red : white);
        this.TargetWarning.material.SetColor("_Color", color);
        flashTickTimer = 0.0f;
      }
      flashTickTimer += Time.deltaTime * this.SpineTimeScale;
      yield return (object) null;
    }
  }

  public GameObject GetOwner() => this.owner;

  public void SetOwner(GameObject owner) => this.owner = owner;
}
