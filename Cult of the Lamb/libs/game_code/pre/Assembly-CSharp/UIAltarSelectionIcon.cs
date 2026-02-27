// Decompiled with JetBrains decompiler
// Type: UIAltarSelectionIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

#nullable disable
public class UIAltarSelectionIcon : 
  BaseMonoBehaviour,
  ISelectHandler,
  IEventSystemHandler,
  IDeselectHandler
{
  [FormerlySerializedAs("SelectedImage")]
  public Image _selectedImage;

  private void OnEnable() => this._selectedImage.color = new Color(1f, 1f, 1f, 0.0f);

  public void OnDeselect(BaseEventData eventData)
  {
    this._selectedImage.DOKill();
    DOTweenModuleUI.DOColor(this._selectedImage, new Color(1f, 1f, 1f, 0.0f), 0.4f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this.transform.DOKill();
    this.transform.DOScale(Vector3.one, 0.4f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
  }

  public void OnSelect(BaseEventData eventData)
  {
    this._selectedImage.DOKill();
    DOTweenModuleUI.DOColor(this._selectedImage, Color.white, 0.2f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this.transform.DOKill();
    this.transform.DOScale(Vector3.one * 1.2f, 0.2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
  }
}
