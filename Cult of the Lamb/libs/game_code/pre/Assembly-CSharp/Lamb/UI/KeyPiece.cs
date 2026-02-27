// Decompiled with JetBrains decompiler
// Type: Lamb.UI.KeyPiece
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private RectTransform _rectTransform;
  [SerializeField]
  private RectTransform _startPosition;
  [SerializeField]
  private Image _image;
  private Vector2 _idlePosition;
  private Vector2 _attachFromPosition;
  private Vector2 _moveVector;

  public Vector2 MoveVector => this._moveVector;

  private void Start()
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
