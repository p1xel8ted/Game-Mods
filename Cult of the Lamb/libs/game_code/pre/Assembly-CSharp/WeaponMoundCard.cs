// Decompiled with JetBrains decompiler
// Type: WeaponMoundCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class WeaponMoundCard : BaseMonoBehaviour
{
  public Chain Chain;
  public Transform Card;
  private Transform CrownBone;
  public Vector3 CardOffset;
  private float xWobble;
  private float yWobble;

  public void Play(Transform CrownBone)
  {
    this.CrownBone = CrownBone;
    this.Chain.FixedPoint1 = CrownBone;
    this.Card.position = CrownBone.position;
    this.StartCoroutine((IEnumerator) this.DoRoutine());
    this.xWobble = (float) Random.Range(0, 180);
    this.yWobble = (float) Random.Range(0, 180);
  }

  private IEnumerator DoRoutine()
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

  private void OnDisable() => this.StopAllCoroutines();
}
