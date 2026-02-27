// Decompiled with JetBrains decompiler
// Type: RunResults
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
public class RunResults : BaseMonoBehaviour
{
  public TextMeshProUGUI DayText;
  public GameObject TextObjectPrefab;
  public CanvasGroup canvasGroup;
  public RectTransform Container;
  public ScrollBarController scrollBarController;
  public GameObject scrollBar;
  public RunResultObject BlackSoulsResult;
  public RunResultObject KillsResult;
  public System.Action Callback;
  public static RunResults Instance;
  public List<InventoryItem.ITEM_TYPE> Blacklist = new List<InventoryItem.ITEM_TYPE>()
  {
    InventoryItem.ITEM_TYPE.SEEDS,
    InventoryItem.ITEM_TYPE.INGREDIENTS,
    InventoryItem.ITEM_TYPE.MEALS
  };
  private bool triggered;

  public float TimeScale { get; private set; } = 2f;

  private bool CheckOnBlacklist(InventoryItem.ITEM_TYPE type)
  {
    bool flag = false;
    foreach (InventoryItem.ITEM_TYPE itemType in this.Blacklist)
    {
      if (type == itemType)
        flag = true;
    }
    return flag;
  }

  public static void Play(System.Action Callback)
  {
    if ((UnityEngine.Object) RunResults.Instance == (UnityEngine.Object) null)
      RunResults.Instance = (UnityEngine.Object.Instantiate(UnityEngine.Resources.Load("Prefabs/UI/UI Run Results"), GameObject.FindWithTag("Canvas").transform) as GameObject).GetComponent<RunResults>();
    if (RunResults.Instance.triggered)
      return;
    RunResults.Instance.triggered = true;
    RunResults.Instance.Callback = Callback;
    RunResults.Instance.StartCoroutine((IEnumerator) RunResults.Instance.DisplayRunResultsRoutine());
  }

  private IEnumerator DisplayRunResultsRoutine()
  {
    RunResults runResults = this;
    Time.timeScale = 0.0f;
    double message = (double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.dungeonRunDuration;
    Debug.Log((object) (float) message);
    int num1 = Mathf.FloorToInt((float) (message / 60.0));
    int num2 = Mathf.FloorToInt((float) (message % 60.0));
    Debug.Log((object) ("MINUTES: " + (object) num1));
    Debug.Log((object) ("SECONDS: " + (object) num2));
    string str = $"{(object) num1}:{(num2 < 10 ? (object) "0" : (object) "")}{(object) num2}";
    runResults.DayText.text = $"<sprite name=\"icon_blackSoul\"> x{(object) DataManager.Instance.dungeonRunXPOrbs} | Time: {str}";
    float Progress = 0.0f;
    float Duration = 0.5f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      runResults.canvasGroup.alpha = Progress / Duration;
      yield return (object) null;
    }
    runResults.canvasGroup.alpha = 1f;
    float delay = 0.25f;
    foreach (InventoryItem inventoryItem in Inventory.itemsDungeon)
    {
      if (!runResults.CheckOnBlacklist((InventoryItem.ITEM_TYPE) inventoryItem.type))
      {
        UnityEngine.Object.Instantiate<GameObject>(runResults.TextObjectPrefab, (Transform) runResults.Container).GetComponent<RunResultObject>().Init(inventoryItem, delay);
        delay += 1.25f;
      }
    }
    float t = 0.0f;
    while (!InputManager.UI.GetAcceptButtonDown() && (double) t < (double) delay)
    {
      DOTween.ManualUpdate(Time.unscaledDeltaTime * runResults.TimeScale, Time.unscaledDeltaTime * runResults.TimeScale);
      t += Time.unscaledDeltaTime * runResults.TimeScale;
      yield return (object) null;
    }
    runResults.TimeScale = 7.5f;
    yield return (object) new WaitForSecondsRealtime(0.1f);
    while (!InputManager.UI.GetAcceptButtonDown())
    {
      DOTween.ManualUpdate(Time.unscaledDeltaTime * runResults.TimeScale, Time.unscaledDeltaTime * runResults.TimeScale);
      yield return (object) null;
    }
    runResults.scrollBarController.enabled = true;
    runResults.scrollBar.SetActive(true);
    Inventory.ClearDungeonItems();
    Progress = 0.0f;
    Duration = 0.5f;
    Time.timeScale = 1f;
    System.Action callback = runResults.Callback;
    if (callback != null)
      callback();
    runResults.Callback = (System.Action) null;
    UnityEngine.Object.Destroy((UnityEngine.Object) runResults.gameObject);
  }
}
