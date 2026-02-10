// Decompiled with JetBrains decompiler
// Type: Lamb.UI.KeyPiece
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class KeyPiece : BaseMonoBehaviour
{
  [SerializeField]
  public RectTransform _rectTransform;
  [SerializeField]
  public RectTransform _startPosition;
  [SerializeField]
  public Image _image;
  public Vector2 _idlePosition;
  public Vector2 _attachFromPosition;
  public Vector2 _moveVector;

  public Vector2 MoveVector => this._moveVector;

  public void Start()
  {
    this._idlePosition = this._rectTransform.anchoredPosition;
    this._attachFromPosition = this._startPosition.anchoredPosition;
    this._moveVector = (this._attachFromPosition - this._idlePosition).normalized;
  }

  public void SetMaterial(Material material) => this._image.material = material;

  public void PrepareForAttach()
  {
    this._rectTransform.localScale = Vector3.zero;
    this._rectTransform.anchoredPosition = this._attachFromPosition;
  }

  public IEnumerator Attach()
  {
    UIManager.PlayAudio("event:/temple_key/fragment_move");
    this._rectTransform.DOScale(Vector3.one, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.15f);
    this._rectTransform.DOAnchorPos(this._idlePosition, 0.5f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.45f);
    UIManager.PlayAudio("event:/temple_key/fragment_into_place");
  }
}
