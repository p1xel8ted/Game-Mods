// Decompiled with JetBrains decompiler
// Type: UIInventoryPromptOverlay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using UnityEngine;

#nullable disable
public class UIInventoryPromptOverlay : BaseMonoBehaviour
{
  public static bool Showing;
  public float kTransitionTime = 1f;
  [SerializeField]
  public RectTransform _promptRectTransform;
  [SerializeField]
  public CanvasGroup _promptCanvasGroup;

  public void Awake()
  {
    UIInventoryPromptOverlay.Showing = true;
    this._promptRectTransform.localScale = Vector3.zero;
    this._promptCanvasGroup.alpha = 0.0f;
  }

  public IEnumerator ScaleButton()
  {
    while (true)
    {
      yield return (object) new WaitForSeconds(1f);
      this._promptRectTransform.transform.DOPunchScale(new Vector3(0.12f, 0.12f), 0.5f).SetUpdate<Tweener>(true);
      yield return (object) null;
    }
  }

  public IEnumerator Start()
  {
    UIInventoryPromptOverlay inventoryPromptOverlay = this;
    inventoryPromptOverlay.StartCoroutine((IEnumerator) inventoryPromptOverlay.ScaleButton());
    inventoryPromptOverlay._promptRectTransform.DOScale(Vector3.one, inventoryPromptOverlay.kTransitionTime).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    inventoryPromptOverlay._promptCanvasGroup.DOFade(1f, inventoryPromptOverlay.kTransitionTime * 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    if (CoopManager.CoopActive && PlayerFarming.playersCount > 1)
      PlayerFarming.players[1].state.CURRENT_STATE = StateMachine.State.InActive;
    while (!InputManager.Gameplay.GetMenuButtonDown() || (double) Time.deltaTime == 0.0)
      yield return (object) null;
    if (CoopManager.CoopActive && PlayerFarming.playersCount > 1)
      PlayerFarming.players[1].state.CURRENT_STATE = StateMachine.State.Idle;
    inventoryPromptOverlay.StopCoroutine((IEnumerator) inventoryPromptOverlay.ScaleButton());
    Object.Destroy((Object) inventoryPromptOverlay.gameObject);
  }
}
