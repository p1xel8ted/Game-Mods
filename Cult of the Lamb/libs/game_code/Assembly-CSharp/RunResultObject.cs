// Decompiled with JetBrains decompiler
// Type: RunResultObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class RunResultObject : BaseMonoBehaviour
{
  [SerializeField]
  public ScrollRect scrollRect;
  [SerializeField]
  public PauseInventoryItem runItem;
  [SerializeField]
  public PauseInventoryItem baseItem;
  [SerializeField]
  public PauseInventoryItem totalItem;
  [SerializeField]
  public CanvasGroup canvasGroup;
  public const float moveDuration = 0.5f;
  public RunResults runResults;

  public void Init(InventoryItem item, float delay)
  {
    this.runResults = this.GetComponentInParent<RunResults>();
    int quantity = item.quantity;
    int Quantity = Inventory.GetItemByType(item.type) != null ? Inventory.GetItemByType(item.type).quantity - item.quantity : 0;
    this.runItem.Init((InventoryItem.ITEM_TYPE) item.type, quantity);
    this.baseItem.Init((InventoryItem.ITEM_TYPE) item.type, Quantity);
    this.scrollRect.StartCoroutine((IEnumerator) this.Merge(item, quantity + Quantity, delay));
  }

  public IEnumerator Merge(InventoryItem item, int total, float delay)
  {
    RunResultObject runResultObject = this;
    runResultObject.canvasGroup.alpha = 0.0f;
    float t = 0.0f;
    while ((double) t < (double) delay)
    {
      t += Time.unscaledDeltaTime * runResultObject.runResults.TimeScale;
      yield return (object) null;
    }
    runResultObject.gameObject.SetActive(true);
    yield return (object) new WaitForEndOfFrame();
    if ((double) runResultObject.scrollRect.verticalNormalizedPosition != 0.0)
    {
      runResultObject.scrollRect.DOVerticalNormalizedPos(0.0f, 0.25f).SetUpdate<Tweener>(UpdateType.Manual);
      t = 0.0f;
      while ((double) t < 0.25)
      {
        t += Time.unscaledDeltaTime * runResultObject.runResults.TimeScale;
        yield return (object) null;
      }
    }
    runResultObject.canvasGroup.DOFade(1f, 0.25f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    t = 0.0f;
    while ((double) t < 0.25)
    {
      t += Time.unscaledDeltaTime * runResultObject.runResults.TimeScale;
      yield return (object) null;
    }
    PauseInventoryItem movingRunItem = Object.Instantiate<PauseInventoryItem>(runResultObject.runItem, runResultObject.transform);
    movingRunItem.Init((InventoryItem.ITEM_TYPE) item.type, 1, false);
    ((RectTransform) movingRunItem.transform).DOAnchorPosX(0.0f, 0.5f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(UpdateType.Manual);
    runResultObject.runItem.SetGrey();
    PauseInventoryItem movingBaseItem = Object.Instantiate<PauseInventoryItem>(runResultObject.baseItem, runResultObject.transform);
    movingBaseItem.Init((InventoryItem.ITEM_TYPE) item.type, 1, false);
    ((RectTransform) movingBaseItem.transform).DOAnchorPosX(0.0f, 0.5f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(UpdateType.Manual);
    runResultObject.baseItem.SetGrey();
    t = 0.0f;
    while ((double) t < 0.5)
    {
      t += Time.unscaledDeltaTime * runResultObject.runResults.TimeScale;
      yield return (object) null;
    }
    movingRunItem.gameObject.SetActive(false);
    movingBaseItem.gameObject.SetActive(false);
    runResultObject.totalItem.gameObject.SetActive(true);
    runResultObject.totalItem.Init((InventoryItem.ITEM_TYPE) item.type, total);
    runResultObject.totalItem.transform.DOPunchScale(Vector3.one * 0.25f, 0.25f).SetUpdate<Tweener>(UpdateType.Manual);
  }
}
