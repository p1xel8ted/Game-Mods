// Decompiled with JetBrains decompiler
// Type: KnuckleBones.Dice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace KnuckleBones;

public class Dice : MonoBehaviour
{
  public RectTransform rectTransform;
  public List<Sprite> DiceFaces = new List<Sprite>();
  public Image image;
  public int Num;
  public Canvas canvas;
  public bool matched;
  public Vector2 ShakeMultiplier = new Vector2(20f, 5f);

  public void OnEnable()
  {
    this.rectTransform = this.GetComponent<RectTransform>();
    this.canvas = this.GetComponentInParent<Canvas>();
  }

  public void Roll(float Duration) => this.StartCoroutine((IEnumerator) this.RollRoutine(Duration));

  public IEnumerator RollRoutine(float Duration, int luckyRoll = -1)
  {
    this.rectTransform.DOMove(this.rectTransform.transform.position + new Vector3((float) Random.Range(-115, 115), (float) Random.Range(-15, 15), 0.0f) * 1f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    float Progress = 0.0f;
    while ((double) Progress < (double) Duration)
    {
      this.Num = Random.Range(1, 7);
      this.image.sprite = this.DiceFaces[this.Num - 1];
      Progress += 0.1f;
      yield return (object) new WaitForSecondsRealtime((float) (0.0099999997764825821 + 0.039999999105930328 * ((double) Progress / (double) Duration)));
    }
    this.Num = luckyRoll != -1 ? luckyRoll + 1 : Random.Range(1, 7);
    this.image.sprite = this.DiceFaces[this.Num - 1];
  }

  public IEnumerator GoToLocationRoutine(Vector3 Position)
  {
    float Progress = 0.0f;
    float Duration = 0.3f;
    Vector3 StartPosition = this.rectTransform.position;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      this.rectTransform.position = Vector3.Lerp(StartPosition, Position, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
    this.rectTransform.position = Position;
    AudioManager.Instance.PlayOneShot("event:/knuckle_bones/die_place");
  }

  public void Shake() => this.StartCoroutine((IEnumerator) this.ShakeRoutine());

  public IEnumerator ShakeRoutine()
  {
    float Shake = 0.5f;
    Vector3 Position = this.image.rectTransform.position;
    while ((double) (Shake -= Time.unscaledDeltaTime) > 0.0)
    {
      this.image.rectTransform.position = Position + new Vector3(this.ShakeMultiplier.x * Random.Range(-Shake, Shake), this.ShakeMultiplier.y * Random.Range(-Shake, Shake));
      this.image.color = Color.Lerp(Color.white, Color.red, Shake / 0.5f);
      this.image.rectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, Shake / 0.5f);
      yield return (object) null;
    }
    this.image.rectTransform.position = Position;
  }

  public void Scale() => this.StartCoroutine((IEnumerator) this.ScaleRoutine());

  public IEnumerator ScaleRoutine()
  {
    float Progress = 0.0f;
    float Duration = 1f;
    Vector3 Scale = Vector3.one;
    Vector3 ScaleSpeed = new Vector3(0.5f, 0.5f);
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      ScaleSpeed.x += (float) ((1.0 - (double) Scale.x) * 0.20000000298023224);
      Scale.x += (ScaleSpeed.x *= 0.6f);
      ScaleSpeed.y += (float) ((1.0 - (double) Scale.y) * 0.20000000298023224);
      Scale.y += (ScaleSpeed.y *= 0.6f);
      this.rectTransform.localScale = Scale;
      yield return (object) null;
    }
    this.rectTransform.localScale = Vector3.one;
  }
}
