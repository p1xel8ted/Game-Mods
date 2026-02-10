// Decompiled with JetBrains decompiler
// Type: Lamb.UI.FinalizeRecipeButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class FinalizeRecipeButton : BaseMonoBehaviour
{
  [SerializeField]
  public RectTransform _rectTransform;
  [SerializeField]
  public MMButton _button;
  [HideInInspector]
  public InventoryItem.ITEM_TYPE Recipe;
  public Vector2 _origin;

  public MMButton Button => this._button;

  public void Awake() => this._origin = this._rectTransform.anchoredPosition;

  public void Shake()
  {
    UIManager.PlayAudio("event:/ui/negative_feedback");
    this._rectTransform.DOKill();
    this._rectTransform.anchoredPosition = this._origin;
    this._rectTransform.DOShakePosition(1f, new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
  }
}
