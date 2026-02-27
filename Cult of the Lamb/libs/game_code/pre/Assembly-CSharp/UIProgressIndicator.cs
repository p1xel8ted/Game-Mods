// Decompiled with JetBrains decompiler
// Type: UIProgressIndicator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class UIProgressIndicator : MonoBehaviour, IPoolListener
{
  public System.Action OnHidden;
  [SerializeField]
  private SpriteRenderer _iconEffective;
  [SerializeField]
  private SpriteRenderer _iconIneffective;
  [SerializeField]
  private SpriteRenderer _progressBar;
  [SerializeField]
  private SpriteRenderer _background;
  private static readonly int _radialProgress = Shader.PropertyToID("_Progress");
  private Material _radialMaterial;
  private bool _showing;
  private bool _hiding;
  private Coroutine _showHideCoroutine;
  private Coroutine _progressCoroutine;
  private Color _invisible = new Color(1f, 1f, 1f, 0.0f);
  private Color _visible = Color.white;
  private float _ineffectiveTimestamp;

  public static List<UIProgressIndicator> ProgressIndicators { private set; get; } = new List<UIProgressIndicator>();

  private void Awake()
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

  private void OnEnable()
  {
    this.transform.localScale = Vector3.one;
    UIProgressIndicator.ProgressIndicators.Add(this);
  }

  private void OnDisable() => UIProgressIndicator.ProgressIndicators.Remove(this);

  private void OnDestroy()
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

  private IEnumerator DoShow(float duration)
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

  private IEnumerator DoHide(float duration, float delay)
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

  private IEnumerator DoSetProgress(float progress, float duration)
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

  private void LateUpdate()
  {
    if (!LetterBox.IsPlaying)
      return;
    this.Hide(0.0f);
  }

  private void SetRadialWheelProgress(float progress)
  {
    this._radialMaterial.SetFloat(UIProgressIndicator._radialProgress, progress);
  }

  private void KillAllTweens()
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
