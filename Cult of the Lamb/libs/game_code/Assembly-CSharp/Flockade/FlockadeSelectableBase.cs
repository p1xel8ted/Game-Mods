// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeSelectableBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
namespace Flockade;

public abstract class FlockadeSelectableBase : 
  MonoBehaviour,
  ISelectHandler,
  IEventSystemHandler,
  IDeselectHandler
{
  public const Ease _ENTRY_STATE_CHANGE_EASING = Ease.OutCubic;
  public const Ease _EXIT_STATE_CHANGE_EASING = Ease.InCubic;
  public const float _STATE_CHANGE_DURATION = 0.25f;
  [SerializeField]
  public Image[] _highlights;
  [SerializeField]
  public MMButton _selectable;
  public bool _configured;
  public Color? _highlightColor;
  public bool _started;
  [CompilerGenerated]
  public FlockadeSelectableBase.FlockadeHighlight[] \u003CHighlights\u003Ek__BackingField;

  public virtual FlockadeSelectableBase.FlockadeHighlight[] Highlights
  {
    get => this.\u003CHighlights\u003Ek__BackingField;
    set => this.\u003CHighlights\u003Ek__BackingField = value;
  }

  public MMButton Selectable => this._selectable;

  public void Configure()
  {
    this._configured = true;
    if (!this._started)
      return;
    this.StartCoroutine((IEnumerator) this.LateConfigure());
  }

  public IEnumerator LateConfigure()
  {
    yield return (object) new WaitForEndOfFrame();
    this.OnLateConfigure();
  }

  public virtual void OnLateConfigure()
  {
  }

  public virtual void Awake()
  {
    this.Highlights = ((IEnumerable<Image>) this._highlights).Select<Image, FlockadeSelectableBase.FlockadeHighlight>((Func<Image, FlockadeSelectableBase.FlockadeHighlight>) (highlight => new FlockadeSelectableBase.FlockadeHighlight(highlight))).ToArray<FlockadeSelectableBase.FlockadeHighlight>();
    this.SetInteractable(false);
  }

  public virtual IEnumerator Start()
  {
    this._started = true;
    if (this._configured)
      yield return (object) this.LateConfigure();
  }

  void ISelectHandler.OnSelect(BaseEventData eventData)
  {
    this.TryPerformSelectAction(this._highlightColor.GetValueOrDefault());
  }

  public void TryPerformSelectAction(Color highlightColor)
  {
    this.SetHighlighted(true, highlightColor);
    this.OnSelect();
  }

  public virtual void OnSelect()
  {
  }

  void IDeselectHandler.OnDeselect(BaseEventData eventData) => this.TryPerformDeselectAction();

  public void TryPerformDeselectAction()
  {
    this.SetHighlighted(false);
    this.OnDeselect();
  }

  public virtual void OnDeselect()
  {
  }

  public void SetHighlighted(bool highlighted, Color color = default (Color))
  {
    foreach (FlockadeSelectableBase.FlockadeHighlight highlight in this.Highlights)
    {
      bool active = highlight.Active;
      highlight.Active = highlighted;
      if (!(!highlighted | active))
      {
        highlight.Color = color;
        highlight.Show();
      }
    }
  }

  public void SetInteractable(bool interactable, Color highlight = default (Color))
  {
    this.Selectable.interactable = interactable;
    this._highlightColor = interactable ? new Color?(highlight) : new Color?();
  }

  public class FlockadeHighlight
  {
    public Image _image;
    public RectTransform _rectTransform;
    public Color _originColor;
    public Vector3 _originScale;

    public FlockadeHighlight(Image image)
    {
      this._image = image;
      this._rectTransform = image.GetComponent<RectTransform>();
      this._originColor = image.color;
      this._originScale = this._rectTransform.localScale;
      this.Active = false;
    }

    public bool Active
    {
      get => this._image.gameObject.activeSelf;
      set => this._image.gameObject.SetActive(value);
    }

    public Color Color
    {
      set => this._image.color = value != new Color() ? value : this._originColor;
    }

    public void Show()
    {
      this._rectTransform.DOKill();
      this._rectTransform.DOScale(this._originScale, 0.25f).From<Vector3, Vector3, VectorOptions>(0.5f * this._originScale).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutCubic);
    }
  }
}
