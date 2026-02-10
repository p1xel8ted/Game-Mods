// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIDLCMapCameraController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class UIDLCMapCameraController : MonoBehaviour
{
  [Header("References")]
  [SerializeField]
  public RectTransform _cameraRoot;
  [SerializeField]
  public RectTransform _mapBounds;
  [SerializeField]
  public RectTransform _viewport;
  [Header("Panning")]
  [SerializeField]
  public float _panSpeed = 5f;
  [SerializeField]
  public float _bufferDistance = 100f;
  public Vector2 _targetCenterPosition;

  public void Awake() => this._targetCenterPosition = this._cameraRoot.anchoredPosition;

  public void Update()
  {
    this._cameraRoot.anchoredPosition = Vector2.Lerp(this._cameraRoot.anchoredPosition, this.ClampAroundCenter(this._targetCenterPosition), Time.unscaledDeltaTime * this._panSpeed);
  }

  public async Task Zoom(float amount, float duration = 0.5f, float delay = 0.0f)
  {
    await this._viewport.transform.DOScale((Vector3) (Vector2.one * amount), duration).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(delay).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutCubic).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).AsyncWaitForCompletion();
  }

  public async Task ResetPan(float duration = 0.5f, float delay = 0.0f)
  {
    this._targetCenterPosition = Vector2.zero;
    await this._cameraRoot.DOAnchorPos(this.ClampAroundCenter(this._targetCenterPosition), duration).SetDelay<TweenerCore<Vector2, Vector2, VectorOptions>>(delay).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InOutCubic).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true).AsyncWaitForCompletion();
  }

  public async Task PanTo(Vector3 position, float duration = 0.5f, float delay = 0.0f)
  {
    Vector2 localPoint;
    if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(this._cameraRoot.parent as RectTransform, RectTransformUtility.WorldToScreenPoint((Camera) null, position), (Camera) null, out localPoint))
      return;
    await this._cameraRoot.DOAnchorPos(this.ClampAroundCenter(-localPoint), duration).SetDelay<TweenerCore<Vector2, Vector2, VectorOptions>>(delay).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InOutCubic).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true).AsyncWaitForCompletion();
  }

  public async Task PanToAll(List<Vector3> worldPositions, float duration = 0.5f, float delay = 0.0f)
  {
    List<Vector2> vector2List = new List<Vector2>();
    foreach (Vector3 worldPosition in worldPositions)
    {
      Vector2 localPoint;
      if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this._cameraRoot.parent as RectTransform, RectTransformUtility.WorldToScreenPoint((Camera) null, worldPosition), (Camera) null, out localPoint))
      {
        Vector2 vector2 = this.ClampAroundCenter(-localPoint);
        vector2List.Add(vector2);
      }
    }
    foreach (Vector2 endValue in vector2List)
      await this._cameraRoot.DOAnchorPos(endValue, duration).SetDelay<TweenerCore<Vector2, Vector2, VectorOptions>>(delay).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InOutCubic).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true).AsyncWaitForCompletion();
  }

  public Vector2 ClampAroundCenter(Vector2 desiredPosition)
  {
    float width = this._viewport.rect.width;
    double height = (double) this._viewport.rect.height;
    float num1 = width / 2f;
    float num2 = (float) (height / 2.0);
    float min1 = (float) -((double) this._mapBounds.rect.width - (double) num1) + this._bufferDistance;
    float max1 = num1 - this._bufferDistance;
    float min2 = (float) -((double) this._mapBounds.rect.height - (double) num2) + this._bufferDistance;
    float max2 = num2 - this._bufferDistance;
    return new Vector2(Mathf.Clamp(desiredPosition.x, min1, max1), Mathf.Clamp(desiredPosition.y, min2, max2));
  }
}
