// Decompiled with JetBrains decompiler
// Type: NotificationDynamicBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using Lamb.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public abstract class NotificationDynamicBase : MonoBehaviour
{
  private const float kDuration = 3f;
  [SerializeField]
  private RectTransform _container;
  [SerializeField]
  private MMSelectable _selectable;
  [SerializeField]
  private Image _progressBar;
  [SerializeField]
  private GameObject _extraWarningSignal;
  private DynamicNotificationData _data;
  private float _progress;
  private bool _enabledWarning;
  private Vector3 _punchAmount = new Vector3(0.1f, 0.1f, 0.1f);
  private CanvasGroup _canvasGroup;

  public DynamicNotificationData Data => this._data;

  public RectTransform Container => this._container;

  public MMSelectable Selectable => this._selectable;

  public bool Closing { get; private set; }

  public abstract Color FullColour { get; }

  public abstract Color EmptyColour { get; }

  private void Awake() => this._canvasGroup = this.GetComponent<CanvasGroup>();

  public virtual void Configure(DynamicNotificationData data)
  {
    this._data = data;
    this._data.DataChanged += new System.Action(this.OnDataChanged);
    this.OnDataChanged();
    this.Show();
  }

  private void OnEnable()
  {
    this._extraWarningSignal.SetActive(false);
    if (this._data == null)
      return;
    this._data.DataChanged += new System.Action(this.OnDataChanged);
  }

  private void OnDisable()
  {
    if (this._data == null)
      return;
    this._data.DataChanged -= new System.Action(this.OnDataChanged);
  }

  private void Update()
  {
    this._progressBar.fillAmount = this.Data.CurrentProgress;
    this._progressBar.color = this.GetColor(this.Data.CurrentProgress);
    if ((double) this.Data.CurrentProgress > 0.5)
    {
      if ((double) (this._progress += Time.deltaTime) > 3.0)
      {
        if (!this._enabledWarning)
        {
          this._extraWarningSignal.SetActive(true);
          this._enabledWarning = true;
        }
        this._container.DOKill();
        this._container.DOPunchScale(this._punchAmount, 1f);
        this._progress = 0.0f;
      }
    }
    else if (this._enabledWarning)
    {
      this._extraWarningSignal.SetActive(false);
      this._enabledWarning = false;
    }
    if (LetterBox.IsPlaying && (double) this._canvasGroup.alpha == 1.0)
    {
      this._canvasGroup.DOKill();
      this._canvasGroup.DOFade(0.0f, 1f);
    }
    else
    {
      if (LetterBox.IsPlaying || (double) this._canvasGroup.alpha != 0.0)
        return;
      this._canvasGroup.DOKill();
      this._canvasGroup.DOFade(1f, 1f);
    }
  }

  protected virtual Color GetColor(float norm)
  {
    return Color.Lerp(this.EmptyColour, this.FullColour, norm);
  }

  private void OnDataChanged()
  {
    if (!this.Data.IsEmpty)
    {
      this.UpdateIcon();
    }
    else
    {
      this.UpdateIcon();
      if (this.Closing)
        return;
      this.Hide();
    }
  }

  protected abstract void UpdateIcon();

  protected virtual void Show() => this.StartCoroutine((IEnumerator) this.DoShow());

  private IEnumerator DoShow()
  {
    float progress = 0.0f;
    float duration = 0.3f;
    while ((double) (progress += Time.deltaTime) < (double) duration)
    {
      this._container.localScale = Vector3.one * Mathf.SmoothStep(2f, 1f, progress / duration);
      yield return (object) null;
    }
    this._container.localScale = Vector3.one;
  }

  protected virtual void Hide()
  {
    this.Closing = true;
    this.StartCoroutine((IEnumerator) this.DoHide());
  }

  private IEnumerator DoHide()
  {
    NotificationDynamicBase notificationDynamicBase = this;
    notificationDynamicBase.Closing = true;
    float progress = 0.0f;
    float duration = 0.5f;
    while ((double) (progress += Time.deltaTime) < (double) duration)
    {
      notificationDynamicBase._container.localScale = Vector3.one * Mathf.SmoothStep(1f, 0.0f, progress / duration);
      yield return (object) null;
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) notificationDynamicBase.gameObject);
  }

  protected virtual void OnDestroy()
  {
    UIDynamicNotificationCenter.NotificationsDynamic.Remove(this);
  }
}
