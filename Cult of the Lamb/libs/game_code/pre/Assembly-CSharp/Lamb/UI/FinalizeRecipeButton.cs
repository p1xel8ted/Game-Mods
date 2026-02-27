// Decompiled with JetBrains decompiler
// Type: Lamb.UI.FinalizeRecipeButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class FinalizeRecipeButton : BaseMonoBehaviour
{
  [SerializeField]
  private RectTransform _rectTransform;
  [SerializeField]
  private MMButton _button;
  [HideInInspector]
  public InventoryItem.ITEM_TYPE Recipe;
  private Vector2 _origin;

  public MMButton Button => this._button;

  private void Awake() => this._origin = this._rectTransform.anchoredPosition;

  public void Shake()
  {
    UIManager.PlayAudio("event:/ui/negative_feedback");
    this._rectTransform.DOKill();
    this._rectTransform.anchoredPosition = this._origin;
    this._rectTransform.DOShakePosition(1f, new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
  }
}
