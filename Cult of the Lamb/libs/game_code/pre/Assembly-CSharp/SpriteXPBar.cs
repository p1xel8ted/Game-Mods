// Decompiled with JetBrains decompiler
// Type: SpriteXPBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#nullable disable
public class SpriteXPBar : MonoBehaviour
{
  public SpriteRenderer ProgressBar;
  public SpriteRenderer ProgressBarTmpFill;
  public Transform XPBar;
  public bool HideOnEmpty = true;
  private SpriteRenderer[] SpriteRenderers;
  private Quaternion _startRot;
  [SerializeField]
  private Color fullColor = new Color(0.9921569f, 0.1137255f, 0.01176471f, 1f);
  private float _value;
  private bool hidden;
  private bool lessValue;

  private void Awake()
  {
    this._startRot = this.XPBar.rotation;
    this.SpriteRenderers = this.GetComponentsInChildren<SpriteRenderer>();
    this.ProgressBarTmpFill.transform.DOScaleX(0.0f, 0.1f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this.ProgressBar.transform.DOScaleX(0.0f, 0.1f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
  }

  public void UpdateBar(float value)
  {
    this.lessValue = (double) value < (double) this._value;
    this._value = value;
    this.XPBar.DOComplete();
    this.XPBar.DOKill();
    this.ProgressBar.DOKill();
    this.ProgressBar.transform.DOKill();
    this.ProgressBarTmpFill.DOKill();
    this.ProgressBarTmpFill.transform.DOKill();
    if ((double) value <= 0.0)
    {
      this.ProgressBarTmpFill.transform.DOScaleX(0.0f, 0.1f);
      this.ProgressBar.transform.DOScaleX(0.0f, 0.1f);
      if (this.HideOnEmpty)
        this.Hide();
      this.hidden = true;
    }
    else
    {
      if (this.hidden || this.SpriteRenderers.Length != 0 && (double) this.SpriteRenderers[0].color.a == 0.0)
      {
        this.Show();
        this.hidden = false;
      }
      this.XPBar.DOPunchRotation(Vector3.back * 10f, 0.5f).SetEase<Tweener>(Ease.OutBounce);
      if ((double) this._value >= 1.0)
      {
        this.ProgressBar.transform.DOScaleX(value, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart);
        this.ProgressBar.DOColor(this.fullColor, 1f);
      }
      else if (!this.lessValue)
      {
        this.ProgressBarTmpFill.transform.DOScaleX(value, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart);
        this.ProgressBar.transform.DOScaleX(value, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(1f);
      }
      else
      {
        this.ProgressBarTmpFill.transform.DOScaleX(value, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart);
        this.ProgressBar.transform.DOScaleX(value, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart);
      }
    }
  }

  public void Hide(bool instant = false)
  {
    if (this.SpriteRenderers != null)
    {
      foreach (SpriteRenderer spriteRenderer in this.SpriteRenderers)
        spriteRenderer.DOFade(0.0f, instant ? 0.0f : 1f);
    }
    this.hidden = true;
  }

  public void Show(bool instant = false)
  {
    if (this.SpriteRenderers != null)
    {
      foreach (SpriteRenderer spriteRenderer in this.SpriteRenderers)
        spriteRenderer.DOFade(1f, instant ? 0.0f : 1f);
    }
    this.hidden = false;
  }
}
