// Decompiled with JetBrains decompiler
// Type: KeyOnHud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMBiomeGeneration;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class KeyOnHud : BaseMonoBehaviour
{
  public AnimationCurve curve = AnimationCurve.Linear(0.0f, 0.0f, 1f, 1f);
  public Image image;

  private void Start()
  {
    this.image.enabled = false;
    BiomeGenerator.OnGetKey += new BiomeGenerator.GetKey(this.Reveal);
    BiomeGenerator.OnUseKey += new BiomeGenerator.GetKey(this.Hide);
  }

  private void OnDisable()
  {
    BiomeGenerator.OnGetKey -= new BiomeGenerator.GetKey(this.Reveal);
    BiomeGenerator.OnUseKey -= new BiomeGenerator.GetKey(this.Hide);
  }

  private void Hide()
  {
    this.StopAllCoroutines();
    this.image.enabled = false;
  }

  private void Reveal() => this.StartCoroutine((IEnumerator) this.RevealRoutine());

  private IEnumerator RevealRoutine()
  {
    yield return (object) new WaitForSeconds(3.5f);
    this.image.enabled = true;
    float Progress = 0.0f;
    float Duration = 0.5f;
    float ScaleStart = 3f;
    float ScaleEnd = 1f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      float num = Mathf.Lerp(ScaleStart, ScaleEnd, this.curve.Evaluate(Progress / Duration));
      this.image.rectTransform.localScale = new Vector3(num, num);
      yield return (object) null;
    }
  }
}
