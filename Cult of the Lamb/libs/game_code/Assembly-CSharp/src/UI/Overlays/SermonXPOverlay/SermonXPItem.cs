// Decompiled with JetBrains decompiler
// Type: src.UI.Overlays.SermonXPOverlay.SermonXPItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

#nullable disable
namespace src.UI.Overlays.SermonXPOverlay;

public class SermonXPItem : MonoBehaviour
{
  [SerializeField]
  public RectTransform _rectTransform;
  [SerializeField]
  public TMP_Text _count;
  public FollowerBrain _followerBrain;
  public RectTransform _container;
  public Camera _camera;

  public FollowerBrain FollowerBrain => this._followerBrain;

  public RectTransform RectTransform => this._rectTransform;

  public void Configure(FollowerBrain followerBrain, RectTransform container)
  {
    this._followerBrain = followerBrain;
    this._container = container;
    this._camera = Camera.main;
    this._count.text = "+" + (object) followerBrain.Info.XPLevel;
    this._rectTransform.localScale = (Vector3) Vector2.zero;
  }

  public void Show()
  {
    this._rectTransform.DOKill();
    this._rectTransform.DOScale((Vector3) Vector2.one, 0.4f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
  }

  public void Hide()
  {
    this._rectTransform.DOScale((Vector3) Vector2.zero, 0.4f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.25f);
  }

  public void UpdateCount(int count)
  {
    this._count.text = "+" + (object) count;
    this._rectTransform.DOKill();
    this._rectTransform.localScale = Vector3.one;
    if (count <= 0)
      this.Hide();
    else
      this._rectTransform.DOPunchScale(Vector3.one * 0.15f, 0.2f);
  }

  public void Update()
  {
    Vector2 viewportPoint = (Vector2) this._camera.WorldToViewportPoint(this._followerBrain.LastPosition + new Vector3(0.0f, 0.0f, -1.75f));
    viewportPoint.x *= this._container.rect.width;
    viewportPoint.y *= this._container.rect.height;
    this._rectTransform.anchoredPosition = viewportPoint;
  }
}
