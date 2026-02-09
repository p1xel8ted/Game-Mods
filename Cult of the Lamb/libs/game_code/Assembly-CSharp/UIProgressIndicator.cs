// Decompiled with JetBrains decompiler
// Type: UIProgressIndicator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class UIProgressIndicator : MonoBehaviour, IPoolListener
{
  public System.Action OnHidden;
  [SerializeField]
  public SpriteRenderer _iconEffective;
  [SerializeField]
  public SpriteRenderer _iconIneffective;
  [SerializeField]
  public SpriteRenderer _progressBar;
  [SerializeField]
  public SpriteRenderer _background;
  [CompilerGenerated]
  public static List<UIProgressIndicator> \u003CProgressIndicators\u003Ek__BackingField = new List<UIProgressIndicator>();
  public static int _radialProgress = Shader.PropertyToID("_Progress");
  public Material _radialMaterial;
  public bool _showing;
  public bool _hiding;
  public Coroutine _showHideCoroutine;
  public Coroutine _progressCoroutine;
  public Color _invisible = new Color(1f, 1f, 1f, 0.0f);
  public Color _visible = Color.white;
  public float _ineffectiveTimestamp;

  public static List<UIProgressIndicator> ProgressIndicators
  {
    set => UIProgressIndicator.\u003CProgressIndicators\u003Ek__BackingField = value;
    get => UIProgressIndicator.\u003CProgressIndicators\u003Ek__BackingField;
  }

  public void Awake()
  {
    if ((UnityEngine.Object) this._radialMaterial == (UnityEngine.Object) null)
    {
      this._radialMaterial = new Material(this._progressBar.material);
      this._progressBar.material = this._radialMaterial;
    }
    this._iconIneffective.gameObject.SetActive(false);
    this._iconEffective.color = this._invisible;
    this._iconIneffective.color = this._invisible;
    this._progressBar.color = this._invisible;
    this._background.color = this._invisible;
  }

  public void OnEnable()
  {
    this.transform.localScale = Vector3.one;
    UIProgressIndicator.ProgressIndicators.Add(this);
  }

  public void OnDisable() => UIProgressIndicator.ProgressIndicators.Remove(this);

  public void OnDestroy()
  {
    UnityEngine.Object.Destroy((UnityEngine.Object) this._radialMaterial);
    this._radialMaterial = (Material) null;
  }

  public void Show(float progress, float duration = 0.5f)
  {
    if (this._showing || LetterBox.IsPlaying)
      return;
    this._hiding = false;
    if (this._showHideCoroutine != null)
      this.StopCoroutine(this._showHideCoroutine);
    this._showHideCoroutine = this.StartCoroutine((IEnumerator) this.DoShow(duration));
    this.SetProgress(progress, 0.0f);
  }

  public IEnumerator DoShow(float duration)
  {
    this._showing = true;
    this.KillAllTweens();
    this._iconEffective.DOColor(this._visible, duration);
    this._iconIneffective.DOColor(this._visible, duration);
    this._progressBar.DOColor(this._visible, duration);
    this._background.DOColor(this._visible, duration);
    yield return (object) new WaitForSeconds(duration);
    this._showing = false;
  }

  public void Hide(float duration = 0.5f, float delay = 0.1f)
  {
    if (this._hiding)
      return;
    this._showing = false;
    if (this._showHideCoroutine != null)
      this.StopCoroutine(this._showHideCoroutine);
    this._showHideCoroutine = this.StartCoroutine((IEnumerator) this.DoHide(duration, delay));
  }

  public IEnumerator DoHide(float duration, float delay)
  {
    UIProgressIndicator progressIndicator = this;
    progressIndicator._hiding = true;
    yield return (object) new WaitForSeconds(delay);
    progressIndicator.KillAllTweens();
    progressIndicator._iconEffective.DOColor(progressIndicator._invisible, duration).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    progressIndicator._iconIneffective.DOColor(progressIndicator._invisible, duration).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    progressIndicator._progressBar.DOColor(progressIndicator._invisible, duration).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    progressIndicator._background.DOColor(progressIndicator._invisible, duration).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(duration);
    progressIndicator._hiding = false;
    System.Action onHidden = progressIndicator.OnHidden;
    if (onHidden != null)
      onHidden();
    progressIndicator.gameObject.Recycle();
  }

  public void SetProgress(float progress, float duration = 0.33f, bool ineffective = false)
  {
    if (this._progressCoroutine != null)
      this.StopCoroutine(this._progressCoroutine);
    this._progressCoroutine = this.StartCoroutine((IEnumerator) this.DoSetProgress(progress, duration));
    if (ineffective)
    {
      this._iconEffective.gameObject.SetActive(false);
      this._iconIneffective.gameObject.SetActive(true);
      this._ineffectiveTimestamp = Time.time;
    }
    else
    {
      if ((double) Time.time - (double) this._ineffectiveTimestamp <= 1.0)
        return;
      this._iconEffective.gameObject.SetActive(true);
      this._iconIneffective.gameObject.SetActive(false);
    }
  }

  public IEnumerator DoSetProgress(float progress, float duration)
  {
    if (this._hiding)
      this.Show(progress);
    this._radialMaterial.DOKill();
    if ((double) duration <= 0.0)
      this.SetRadialWheelProgress(progress);
    else
      this._radialMaterial.DOFloat(progress, UIProgressIndicator._radialProgress, duration).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutQuart).OnKill<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this.SetRadialWheelProgress(progress)));
    yield return (object) new WaitForSeconds(duration);
    yield return (object) new WaitForSeconds(5f);
    this.Hide();
  }

  public void LateUpdate()
  {
    if (!LetterBox.IsPlaying)
      return;
    this.Hide(0.0f);
  }

  public void SetRadialWheelProgress(float progress)
  {
    this._radialMaterial.SetFloat(UIProgressIndicator._radialProgress, progress);
  }

  public void KillAllTweens()
  {
    this._iconEffective.DOKill();
    this._iconIneffective.DOKill();
    this._progressBar.DOKill();
    this._background.DOKill();
  }

  public void OnRecycled()
  {
    this._showing = false;
    this._hiding = false;
    this.KillAllTweens();
    this._iconEffective.color = this._invisible;
    this._iconIneffective.color = this._invisible;
    this._progressBar.color = this._invisible;
    this._background.color = this._invisible;
    this._radialMaterial.DOKill();
    this.SetRadialWheelProgress(0.0f);
    this.StopAllCoroutines();
    this._showHideCoroutine = (Coroutine) null;
    this._progressCoroutine = (Coroutine) null;
    this._iconEffective.gameObject.SetActive(true);
    this._iconIneffective.gameObject.SetActive(false);
  }
}
