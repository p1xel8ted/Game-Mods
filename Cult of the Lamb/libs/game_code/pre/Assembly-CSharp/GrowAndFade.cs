// Decompiled with JetBrains decompiler
// Type: GrowAndFade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class GrowAndFade : BaseMonoBehaviour
{
  public SpriteRenderer spriteRenderer;
  public Image image;
  public Color StartColor;
  public Color TargetColor;
  public float ScaleSpeed = 1f;
  public float Duration = 1f;

  private void Start()
  {
    if ((Object) this.spriteRenderer != (Object) null)
      this.spriteRenderer.enabled = false;
    if (!((Object) this.image != (Object) null))
      return;
    this.image.enabled = false;
  }

  public void Play() => this.StartCoroutine((IEnumerator) this.PlayRoutine());

  private IEnumerator PlayRoutine()
  {
    GrowAndFade growAndFade = this;
    if ((Object) growAndFade.spriteRenderer != (Object) null)
      growAndFade.spriteRenderer.enabled = true;
    if ((Object) growAndFade.image != (Object) null)
      growAndFade.image.enabled = true;
    float Progress = 0.0f;
    float Scale = 1f;
    while ((double) (Progress += Time.deltaTime) < (double) growAndFade.Duration)
    {
      Color color = Color.Lerp(growAndFade.StartColor, growAndFade.TargetColor, Progress / growAndFade.Duration);
      if ((Object) growAndFade.spriteRenderer != (Object) null)
        growAndFade.spriteRenderer.color = color;
      if ((Object) growAndFade.image != (Object) null)
        growAndFade.image.color = color;
      Scale += Time.deltaTime * growAndFade.ScaleSpeed;
      growAndFade.transform.localScale = Vector3.one * Scale;
      yield return (object) null;
    }
    if ((Object) growAndFade.spriteRenderer != (Object) null)
      growAndFade.spriteRenderer.enabled = false;
    if ((Object) growAndFade.image != (Object) null)
      growAndFade.image.enabled = false;
  }
}
