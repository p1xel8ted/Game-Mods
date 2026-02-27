// Decompiled with JetBrains decompiler
// Type: MortarBomb
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class MortarBomb : BaseMonoBehaviour
{
  public GameObject BombVisual;
  public SpriteRenderer Target;
  public SpriteRenderer TargetWarning;
  public CircleCollider2D circleCollider2D;
  public GameObject BombShadow;
  public ParticleSystem SmokeParticles;
  private float moveDuration = 1f;
  public float arcHeight = 2f;
  public AnimationCurve arcCurve;
  private Health.Team bombTeam;

  private void OnEnable()
  {
    this.StartCoroutine((IEnumerator) this.ScaleCircle());
    this.BombVisual.SetActive(false);
    this.BombShadow.SetActive(false);
  }

  private void OnDisable() => Object.Destroy((Object) this.gameObject);

  public void Play(Vector3 Position, float moveDuration, Health.Team bombTeam)
  {
    this.moveDuration = moveDuration;
    this.bombTeam = bombTeam;
    this.StartCoroutine((IEnumerator) this.MoveRock(Position));
    this.StartCoroutine((IEnumerator) this.FlashCircle());
    AudioManager.Instance.PlayOneShot("event:/enemy/fly_spawn", this.gameObject);
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/frog_large/attack", this.gameObject);
  }

  private IEnumerator ScaleCircle()
  {
    float Scale = 0.0f;
    while ((double) (Scale += Time.deltaTime * 8f) <= (double) this.circleCollider2D.radius)
    {
      this.Target.transform.localScale = Vector3.one * Scale;
      this.TargetWarning.transform.localScale = Vector3.one * Scale;
      yield return (object) null;
    }
  }

  private void Update()
  {
    this.Target.transform.Rotate(new Vector3(0.0f, 0.0f, 150f) * Time.deltaTime);
  }

  private IEnumerator FlashCircle()
  {
    while ((double) Vector2.Distance((Vector2) this.BombVisual.transform.localPosition, (Vector2) Vector3.zero) >= 6.0)
      yield return (object) null;
    Color white = new Color(1f, 1f, 1f, 1f);
    Color color = white;
    while ((double) Vector2.Distance((Vector2) this.BombVisual.transform.localPosition, (Vector2) Vector3.zero) < 6.0)
    {
      if (Time.frameCount % 5 == 0 && (double) Time.timeScale == 1.0)
      {
        this.Target.material.SetColor("_Color", color = color == white ? Color.red : white);
        this.TargetWarning.material.SetColor("_Color", color);
      }
      yield return (object) null;
    }
  }

  private IEnumerator MoveRock(Vector3 startPos)
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
      t += Time.deltaTime;
      mortarBomb.BombVisual.transform.position = Vector3.Lerp(startPos, (Vector3) targetPos, t / mortarBomb.moveDuration);
      mortarBomb.BombShadow.transform.position = Vector3.Lerp(startPos, (Vector3) targetPos, t / mortarBomb.moveDuration);
      mortarBomb.BombShadow.transform.localScale = Vector3.one * (float) (1.5 - (double) Mathf.Clamp01(mortarBomb.arcCurve.Evaluate(t / mortarBomb.moveDuration)) * 0.5);
      mortarBomb.BombVisual.transform.position += new Vector3(0.0f, 0.0f, -mortarBomb.arcCurve.Evaluate(t / mortarBomb.moveDuration) * mortarBomb.arcHeight);
      yield return (object) null;
    }
    Explosion.CreateExplosion(mortarBomb.transform.position, mortarBomb.bombTeam, (Health) null, mortarBomb.circleCollider2D.radius, 1f);
    if ((Object) mortarBomb.SmokeParticles != (Object) null)
    {
      mortarBomb.SmokeParticles.transform.parent = mortarBomb.transform.parent;
      mortarBomb.SmokeParticles.Stop();
    }
    Object.Destroy((Object) mortarBomb.gameObject);
  }
}
