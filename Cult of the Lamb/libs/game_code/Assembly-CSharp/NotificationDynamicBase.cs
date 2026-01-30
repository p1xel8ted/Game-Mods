// Decompiled with JetBrains decompiler
// Type: NotificationDynamicBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using Lamb.UI;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public abstract class NotificationDynamicBase : MonoBehaviour
{
  public const float kDuration = 3f;
  [SerializeField]
  public RectTransform _container;
  [SerializeField]
  public MMSelectable _selectable;
  [SerializeField]
  public Image _progressBar;
  [SerializeField]
  public GameObject _extraWarningSignal;
  public DynamicNotificationData _data;
  public float _progress;
  public bool _enabledWarning;
  public Vector3 _punchAmount = new Vector3(0.1f, 0.1f, 0.1f);
  public CanvasGroup _canvasGroup;
  [CompilerGenerated]
  public bool \u003CClosing\u003Ek__BackingField;

  public DynamicNotificationData Data => this._data;

  public RectTransform Container => this._container;

  public MMSelectable Selectable => this._selectable;

  public bool Closing
  {
    get => this.\u003CClosing\u003Ek__BackingField;
    set => this.\u003CClosing\u003Ek__BackingField = value;
  }

  public abstract Color FullColour { get; }

  public abstract Color EmptyColour { get; }

  public void Awake() => this._canvasGroup = this.GetComponent<CanvasGroup>();

  public virtual void Configure(DynamicNotificationData data)
  {
    this._data = data;
    this._data.DataChanged += new System.Action(this.OnDataChanged);
    this.OnDataChanged();
    this.Show();
  }

  public void OnEnable()
  {
    this._extraWarningSignal.SetActive(false);
    if (this._data == null)
      return;
    this._data.DataChanged += new System.Action(this.OnDataChanged);
  }

  public void OnDisable()
  {
    if (this._data == null)
      return;
    this._data.DataChanged -= new System.Action(this.OnDataChanged);
  }

  public void Update()
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

  public virtual Color GetColor(float norm) => Color.Lerp(this.EmptyColour, this.FullColour, norm);

  public void OnDataChanged()
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

  public abstract void UpdateIcon();

  public virtual void Show() => this.StartCoroutine((IEnumerator) this.DoShow());

  public IEnumerator DoShow()
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

  public virtual void Hide()
  {
    this.Closing = true;
    this.StartCoroutine((IEnumerator) this.DoHide());
  }

  public IEnumerator DoHide()
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

  public virtual void OnDestroy() => UIDynamicNotificationCenter.NotificationsDynamic.Remove(this);
}
