// Decompiled with JetBrains decompiler
// Type: WeaponMoundCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class WeaponMoundCard : BaseMonoBehaviour
{
  public Chain Chain;
  public Transform Card;
  public Transform CrownBone;
  public Vector3 CardOffset;
  public float xWobble;
  public float yWobble;

  public void Play(Transform CrownBone)
  {
    this.CrownBone = CrownBone;
    this.Chain.FixedPoint1 = CrownBone;
    this.Card.position = CrownBone.position;
    this.StartCoroutine((IEnumerator) this.DoRoutine());
    this.xWobble = (float) Random.Range(0, 180);
    this.yWobble = (float) Random.Range(0, 180);
  }

  public IEnumerator DoRoutine()
  {
    float Progress = 0.0f;
    bool Loop = true;
    while (Loop)
    {
      if ((double) Progress < 1.0)
        Progress += Time.deltaTime * 5f;
      this.Card.position = this.CrownBone.position + (Vector3.up * 2f + this.CardOffset) * Mathf.SmoothStep(0.0f, 1f, Progress) + new Vector3(0.1f * Mathf.Cos(this.xWobble += Time.deltaTime * 2f), 0.1f * Mathf.Cos(this.yWobble += Time.deltaTime * 2f));
      yield return (object) null;
    }
  }

  public void OnDisable() => this.StopAllCoroutines();
}
