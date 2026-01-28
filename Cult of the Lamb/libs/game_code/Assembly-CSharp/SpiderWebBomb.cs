// Decompiled with JetBrains decompiler
// Type: SpiderWebBomb
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class SpiderWebBomb : BaseMonoBehaviour
{
  public float Grav = 0.7f;
  public GameObject Rock;
  public Transform RotateRock;
  public SpriteRenderer Target;
  public CircleCollider2D circleCollider2D;
  public GameObject ToSpawn;
  public float Rotate;
  public Vector3 ShadowPosition;

  public void OnEnable()
  {
    this.Rock.SetActive(false);
    this.Target.gameObject.SetActive(false);
    this.Rotate = (float) UnityEngine.Random.Range(0, 360);
  }

  public void Play(Vector3 Position, GameObject ToSpawn)
  {
    this.ToSpawn = ToSpawn;
    this.StartCoroutine((IEnumerator) this.MoveRock(Position));
    this.StartCoroutine((IEnumerator) this.DoShadow());
  }

  public IEnumerator DoShadow()
  {
    yield return (object) new WaitForEndOfFrame();
    while (true)
    {
      this.Rotate += Time.deltaTime * (1f / 1000f);
      this.RotateRock.localEulerAngles += new Vector3(0.0f, 0.0f, this.Rotate);
      this.ShadowPosition = this.Rock.transform.localPosition;
      this.ShadowPosition.z = 0.0f;
      this.Target.transform.localPosition = this.ShadowPosition;
      this.Target.transform.localScale = Vector3.one * Mathf.Clamp(5f + this.Rock.transform.localPosition.z, 0.0f, 3f);
      yield return (object) null;
    }
  }

  public IEnumerator MoveRock(Vector3 Position)
  {
    SpiderWebBomb spiderWebBomb = this;
    spiderWebBomb.Rock.SetActive(true);
    spiderWebBomb.Target.gameObject.SetActive(true);
    spiderWebBomb.Rock.transform.position = Position;
    float Speed = 7f;
    float num1 = Vector2.Distance((Vector2) spiderWebBomb.Rock.transform.localPosition, (Vector2) Vector3.zero);
    float num2 = 0.0f;
    float num3 = 0.0f;
    while ((double) (num3 += Speed / 60f) < (double) num1)
      num2 += spiderWebBomb.Grav;
    float yVel = (float) (-(double) num2 / 2.0);
    float z = Position.z;
    while ((double) (z -= yVel * Time.deltaTime) >= 0.0)
      yVel += spiderWebBomb.Grav * GameManager.DeltaTime;
    while ((double) Vector2.Distance((Vector2) spiderWebBomb.Rock.transform.localPosition, (Vector2) Vector3.zero) > (double) Speed * (double) Time.deltaTime)
    {
      float f = Utils.GetAngle(spiderWebBomb.Rock.transform.localPosition, Vector3.zero) * ((float) Math.PI / 180f);
      yVel += spiderWebBomb.Grav * GameManager.DeltaTime;
      Vector3 vector3 = new Vector3(Speed * Mathf.Cos(f), Speed * Mathf.Sin(f), yVel) * Time.deltaTime;
      spiderWebBomb.Rock.transform.localPosition += vector3;
      yield return (object) null;
    }
    if (spiderWebBomb.Target.isVisible)
      CameraManager.shakeCamera(0.2f, (float) UnityEngine.Random.Range(0, 360));
    UnityEngine.Object.Instantiate<GameObject>(spiderWebBomb.ToSpawn, spiderWebBomb.transform.position, Quaternion.identity, spiderWebBomb.transform.parent);
    UnityEngine.Object.Destroy((UnityEngine.Object) spiderWebBomb.gameObject);
  }
}
