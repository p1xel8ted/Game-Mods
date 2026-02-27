// Decompiled with JetBrains decompiler
// Type: EnemyBomb
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class EnemyBomb : BaseMonoBehaviour
{
  public GameObject BombVisual;
  public SpriteRenderer Target;
  public SpriteRenderer TargetWarning;
  public CircleCollider2D circleCollider2D;
  public GameObject BombShadow;
  private float moveDuration = 1f;
  public float bombArcHeight = 2f;
  public AnimationCurve bombArcCurve;
  public bool RotateBomb;
  public Transform rotationTransform;
  public float RotationSpeed = 1f;
  protected Health health;

  private void OnEnable()
  {
    this.StartCoroutine((IEnumerator) this.ScaleCircle());
    this.BombVisual.SetActive(false);
    this.BombShadow.SetActive(false);
  }

  private void OnDisable() => Object.Destroy((Object) this.gameObject);

  public virtual void Play(Vector3 Position, float moveDuration)
  {
    this.moveDuration = moveDuration;
    this.StartCoroutine((IEnumerator) this.MoveRock(Position));
    this.StartCoroutine((IEnumerator) this.FlashCircle());
  }

  private void Update()
  {
    this.Target.transform.Rotate(new Vector3(0.0f, 0.0f, 150f) * Time.deltaTime);
  }

  private IEnumerator MoveRock(Vector3 startPos)
  {
    EnemyBomb enemyBomb = this;
    enemyBomb.BombVisual.SetActive(true);
    enemyBomb.BombShadow.SetActive(true);
    enemyBomb.BombVisual.transform.position = startPos;
    Vector2 targetPos = (Vector2) enemyBomb.transform.position;
    float t = 0.0f;
    while ((double) t < (double) enemyBomb.moveDuration)
    {
      t += Time.deltaTime;
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
      yield return (object) null;
    }
    enemyBomb.BombLanded();
    Object.Destroy((Object) enemyBomb.gameObject);
  }

  protected virtual void BombLanded()
  {
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

  private IEnumerator FlashCircle()
  {
    while ((double) Vector2.Distance((Vector2) this.BombVisual.transform.localPosition, (Vector2) Vector3.zero) >= 6.0)
      yield return (object) null;
    Color white = new Color(1f, 1f, 1f, 1f);
    Color color = white;
    while ((double) Vector2.Distance((Vector2) this.BombVisual.transform.localPosition, (Vector2) Vector3.zero) < 6.0)
    {
      if (Time.frameCount % 5 == 0)
      {
        this.Target.material.SetColor("_Color", color = color == white ? Color.red : white);
        this.TargetWarning.material.SetColor("_Color", color);
      }
      yield return (object) null;
    }
  }
}
