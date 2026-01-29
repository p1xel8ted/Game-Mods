// Decompiled with JetBrains decompiler
// Type: BruteRock
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class BruteRock : BaseMonoBehaviour
{
  public float Grav = 0.7f;
  public GameObject Rock;
  public SpriteRenderer Target;
  public SpriteRenderer TargetWarning;
  public CircleCollider2D circleCollider2D;

  public void OnEnable()
  {
    this.StartCoroutine((IEnumerator) this.ScaleCircle());
    this.Rock.SetActive(false);
  }

  public void Play(Vector3 Position)
  {
    this.StartCoroutine((IEnumerator) this.MoveRock(Position));
    this.StartCoroutine((IEnumerator) this.FlashCircle());
  }

  public IEnumerator ScaleCircle()
  {
    float Scale = 0.0f;
    while ((double) (Scale += Time.deltaTime * 5f) <= (double) this.circleCollider2D.radius)
    {
      this.Target.transform.localScale = Vector3.one * Scale;
      this.TargetWarning.transform.localScale = Vector3.one * Scale;
      yield return (object) null;
    }
  }

  public void Update()
  {
    this.Target.transform.Rotate(new Vector3(0.0f, 0.0f, 150f) * Time.deltaTime);
  }

  public IEnumerator FlashCircle()
  {
    while ((double) Vector2.Distance((Vector2) this.Rock.transform.localPosition, (Vector2) Vector3.zero) >= 6.0)
      yield return (object) null;
    float flashTickTimer = 0.0f;
    Color white = new Color(1f, 1f, 1f, 1f);
    Color color = white;
    while ((double) Vector2.Distance((Vector2) this.Rock.transform.localPosition, (Vector2) Vector3.zero) < 6.0)
    {
      if ((double) flashTickTimer >= 0.11999999731779099 && BiomeConstants.Instance.IsFlashLightsActive)
      {
        this.Target.material.SetColor("_Color", color = color == white ? Color.red : white);
        this.TargetWarning.material.SetColor("_Color", color);
        flashTickTimer = 0.0f;
      }
      flashTickTimer += Time.deltaTime;
      yield return (object) null;
    }
  }

  public IEnumerator MoveRock(Vector3 Position)
  {
    BruteRock bruteRock = this;
    bruteRock.Rock.SetActive(true);
    bruteRock.Rock.transform.position = Position;
    float Speed = 7f;
    float num1 = Vector2.Distance((Vector2) bruteRock.Rock.transform.localPosition, (Vector2) Vector3.zero);
    float num2 = 0.0f;
    float num3 = 0.0f;
    while ((double) (num3 += Speed / 60f) < (double) num1)
      num2 += bruteRock.Grav;
    float num4 = (float) (-(double) num2 / 2.0);
    float z = Position.z;
    while ((double) (z -= num4 * Time.deltaTime) >= 0.0)
      num4 += bruteRock.Grav * GameManager.DeltaTime;
    while ((double) Vector2.Distance((Vector2) bruteRock.Rock.transform.localPosition, (Vector2) Vector3.zero) > (double) Speed * (double) Time.deltaTime)
    {
      float f = Utils.GetAngle(bruteRock.Rock.transform.localPosition, Vector3.zero) * ((float) Math.PI / 180f);
      Vector3 vector3 = new Vector3(Speed * Mathf.Cos(f), Speed * Mathf.Sin(f), 0.0f) * Time.deltaTime;
      bruteRock.Rock.transform.localPosition += vector3;
      yield return (object) null;
    }
    if (bruteRock.Target.isVisible)
      CameraManager.instance.ShakeCameraForDuration(0.2f, 0.5f, 0.3f);
    foreach (Component component1 in Physics2D.OverlapCircleAll((Vector2) bruteRock.transform.position, bruteRock.circleCollider2D.radius))
    {
      Health component2 = component1.gameObject.GetComponent<Health>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        component2.DealDamage(1f, bruteRock.gameObject, Vector3.Lerp(bruteRock.transform.position, component2.transform.position, 0.8f));
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) bruteRock.gameObject);
  }
}
