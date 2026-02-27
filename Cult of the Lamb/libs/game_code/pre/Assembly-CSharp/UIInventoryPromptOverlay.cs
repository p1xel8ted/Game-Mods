// Decompiled with JetBrains decompiler
// Type: UIInventoryPromptOverlay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using UnityEngine;

#nullable disable
public class UIInventoryPromptOverlay : BaseMonoBehaviour
{
  public static bool Showing;
  private float kTransitionTime = 1f;
  [SerializeField]
  private RectTransform _promptRectTransform;
  [SerializeField]
  private CanvasGroup _promptCanvasGroup;

  private void Awake()
  {
    UIInventoryPromptOverlay.Showing = true;
    this._promptRectTransform.localScale = Vector3.zero;
    this._promptCanvasGroup.alpha = 0.0f;
  }

  private IEnumerator ScaleButton()
  {
    while (true)
    {
      yield return (object) new WaitForSeconds(1f);
      this._promptRectTransform.transform.DOPunchScale(new Vector3(0.12f, 0.12f), 0.5f).SetUpdate<Tweener>(true);
      yield return (object) null;
    }
  }

  private IEnumerator Start()
  {
    UIInventoryPromptOverlay inventoryPromptOverlay = this;
    inventoryPromptOverlay.StartCoroutine((IEnumerator) inventoryPromptOverlay.ScaleButton());
    inventoryPromptOverlay._promptRectTransform.DOScale(Vector3.one, inventoryPromptOverlay.kTransitionTime).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    inventoryPromptOverlay._promptCanvasGroup.DOFade(1f, inventoryPromptOverlay.kTransitionTime * 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    while (!InputManager.Gameplay.GetMenuButtonDown())
      yield return (object) null;
    inventoryPromptOverlay.StopCoroutine((IEnumerator) inventoryPromptOverlay.ScaleButton());
    Object.Destroy((Object) inventoryPromptOverlay.gameObject);
  }
}
