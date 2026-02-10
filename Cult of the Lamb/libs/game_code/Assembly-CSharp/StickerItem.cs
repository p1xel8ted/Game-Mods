// Decompiled with JetBrains decompiler
// Type: StickerItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class StickerItem : MonoBehaviour
{
  [SerializeField]
  public Image image;
  [SerializeField]
  public MMButton button;
  [SerializeField]
  public Image _outline;
  [CompilerGenerated]
  public StickerData \u003CStickerData\u003Ek__BackingField;
  public UIEditPhotoOverlayController _editOverlay;
  [SerializeField]
  public RectTransform _transform;

  public Image Image => this.image;

  public MMButton Button => this.button;

  public StickerData StickerData
  {
    get => this.\u003CStickerData\u003Ek__BackingField;
    set => this.\u003CStickerData\u003Ek__BackingField = value;
  }

  public RectTransform Transform => this._transform;

  public void Configure(StickerData stickerData, UIEditPhotoOverlayController editOverlay)
  {
    this.StickerData = stickerData;
    this._editOverlay = editOverlay;
    this.image.sprite = stickerData.Sticker;
    this._outline.sprite = stickerData.Sticker;
    this._outline.gameObject.SetActive(false);
    this.button.OnSelected += new System.Action(this.SelectSticker);
    this.button.OnDeselected += new System.Action(this.DeSelectSticker);
    this.transform.localScale = Vector3.one * 0.8f;
  }

  public void OnDisable()
  {
    if (!((UnityEngine.Object) this.button != (UnityEngine.Object) null))
      return;
    this.button.OnSelected -= new System.Action(this.SelectSticker);
    this.button.OnDeselected -= new System.Action(this.DeSelectSticker);
  }

  public void SelectSticker()
  {
    this._outline.gameObject.SetActive(true);
    this.transform.DOKill();
    this.transform.DOScale(Vector3.one, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
  }

  public void DeSelectSticker()
  {
    this._outline.gameObject.SetActive(false);
    this.transform.DOKill();
    this.transform.DOScale(Vector3.one * 0.8f, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
  }

  public virtual void Flip()
  {
    Vector3 localScale = this.Transform.localScale;
    localScale.x *= -1f;
    this.Transform.localScale = localScale;
  }

  public virtual void OnStickerPlaced()
  {
  }
}
