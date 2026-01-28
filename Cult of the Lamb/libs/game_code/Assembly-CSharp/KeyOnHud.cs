// Decompiled with JetBrains decompiler
// Type: KeyOnHud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class KeyOnHud : BaseMonoBehaviour
{
  public AnimationCurve curve = AnimationCurve.Linear(0.0f, 0.0f, 1f, 1f);
  public Image image;

  public void Start()
  {
    this.image.enabled = false;
    BiomeGenerator.OnGetKey += new BiomeGenerator.GetKey(this.Reveal);
    BiomeGenerator.OnUseKey += new BiomeGenerator.GetKey(this.Hide);
  }

  public void OnDisable()
  {
    BiomeGenerator.OnGetKey -= new BiomeGenerator.GetKey(this.Reveal);
    BiomeGenerator.OnUseKey -= new BiomeGenerator.GetKey(this.Hide);
  }

  public void Hide()
  {
    this.StopAllCoroutines();
    this.image.enabled = false;
  }

  public void Reveal() => this.StartCoroutine((IEnumerator) this.RevealRoutine());

  public IEnumerator RevealRoutine()
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
