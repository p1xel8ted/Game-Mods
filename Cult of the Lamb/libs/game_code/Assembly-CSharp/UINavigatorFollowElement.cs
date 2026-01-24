// Decompiled with JetBrains decompiler
// Type: UINavigatorFollowElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using src.UINavigator;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UINavigatorFollowElement : MonoBehaviour
{
  [SerializeField]
  public Image _image;
  [SerializeField]
  public RectTransform _rectTransform;
  [SerializeField]
  public Image _imageAlt;
  [SerializeField]
  public RectTransform _rectTransformAlt;
  public Vector3 _velocity = Vector3.zero;
  public Selectable _previousSelectable;
  public bool _forceInstantNext;
  public Vector2 _cachedSize;
  public bool _sizeCached;
  public Image _baseImage;
  public RectTransform _baseRectTransform;

  public void Awake()
  {
    this.CacheSizeIfRequired();
    this._baseImage = this._image;
    this._baseRectTransform = this._rectTransform;
  }

  public void CacheSizeIfRequired()
  {
    if (this._sizeCached)
      return;
    this._cachedSize = this._rectTransform.sizeDelta;
    this._sizeCached = true;
  }

  public void Start()
  {
    DOTween.Init((bool?) new bool?(), (bool?) new bool?(), (LogBehaviour?) new LogBehaviour?());
  }

  public void OnEnable()
  {
    MonoSingleton<UINavigatorNew>.Instance.OnDefaultSetComplete += new Action<Selectable>(this.OnDefaultSelectableSet);
    MonoSingleton<UINavigatorNew>.Instance.OnSelectionChanged += new Action<Selectable, Selectable>(this.OnSelectableChanged);
  }

  public void OnDisable()
  {
    if (!((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance != (UnityEngine.Object) null))
      return;
    MonoSingleton<UINavigatorNew>.Instance.OnDefaultSetComplete -= new Action<Selectable>(this.OnDefaultSelectableSet);
    MonoSingleton<UINavigatorNew>.Instance.OnSelectionChanged -= new Action<Selectable, Selectable>(this.OnSelectableChanged);
  }

  public void OnDefaultSelectableSet(Selectable selectable) => this.DoMoveButton(selectable, true);

  public void OnSelectableChanged(Selectable newSelectable, Selectable previous)
  {
    this.DoMoveButton(newSelectable);
  }

  public void DoMoveButton(Selectable to, bool instant = false)
  {
    this.CacheSizeIfRequired();
    this.DoShakeScale();
    this.StopAllCoroutines();
    if ((bool) (UnityEngine.Object) to.GetComponent<UIIgnoreFollowElement>())
    {
      this._rectTransform.localPosition = Vector3.one * float.MaxValue;
      this._forceInstantNext = true;
    }
    else
      this.StartCoroutine((IEnumerator) this.MoveButtonRoutine(to, instant));
  }

  public IEnumerator MoveButtonRoutine(Selectable moveTo, bool instant = false)
  {
    yield return (object) null;
    UINavigatorFollowElementSizer component;
    if (moveTo.TryGetComponent<UINavigatorFollowElementSizer>(out component))
    {
      this._rectTransform.sizeDelta = component.SizeToMe.sizeDelta;
      this._image.preserveAspect = false;
    }
    else
    {
      this._rectTransform.sizeDelta = this._cachedSize;
      this._image.preserveAspect = true;
    }
    Vector3 targetLocalPosition = this._rectTransform.parent.InverseTransformPoint(moveTo.transform.position);
    Vector3 currentLocalPosition = this._rectTransform.localPosition;
    if (!instant && !this._forceInstantNext)
    {
      float progress = 0.0f;
      while ((double) (progress += Time.unscaledDeltaTime * 5f) <= 1.0)
      {
        this._rectTransform.localPosition = Vector3.SmoothDamp(targetLocalPosition, currentLocalPosition, ref this._velocity, progress);
        yield return (object) null;
      }
    }
    this._forceInstantNext = false;
    this._rectTransform.localPosition = targetLocalPosition;
    this._rectTransform.localScale = Vector3.one;
  }

  public void DoShakeScale()
  {
    DOTween.Kill((object) this._rectTransform);
    this._rectTransform.localScale = Vector3.one;
    this._rectTransform.DOShakeScale(0.1f, new Vector3(-0.1f, 0.1f, 1f), 5, fadeOut: false).SetUpdate<Tweener>(true);
  }

  public void SetAltImage(bool alt = false)
  {
    if ((UnityEngine.Object) this._image == (UnityEngine.Object) null)
      return;
    this._image.enabled = false;
    if (alt)
    {
      this._image = this._imageAlt;
      this._rectTransform = this._rectTransformAlt;
    }
    else
    {
      this._image = this._baseImage;
      this._rectTransform = this._baseRectTransform;
    }
    this._image.enabled = true;
  }
}
