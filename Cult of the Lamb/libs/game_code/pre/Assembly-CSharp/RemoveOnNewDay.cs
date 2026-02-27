// Decompiled with JetBrains decompiler
// Type: RemoveOnNewDay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class RemoveOnNewDay : BaseMonoBehaviour
{
  private RemoveOnNewDay.State CurrentState;
  private float TransitionProgress;
  public GameObject TransitionObject;
  public Vector3 StartingPosition;
  private float RandomOffset;
  [SerializeField]
  private SimpleInventory inventory;

  private void Start()
  {
    TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStarted);
    this.CurrentState = RemoveOnNewDay.State.Entering;
    this.RandomOffset = UnityEngine.Random.Range(0.0f, 3f);
    this.TransitionObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.5f);
  }

  private void Update()
  {
    switch (this.CurrentState)
    {
      case RemoveOnNewDay.State.Entering:
        if ((double) (this.RandomOffset -= Time.deltaTime) < 0.0)
          this.TransitionObject.transform.localPosition = Vector3.Lerp(new Vector3(0.0f, 0.0f, 0.5f), Vector3.zero, Mathf.SmoothStep(0.0f, 1f, (this.TransitionProgress += Time.deltaTime) / 1f));
        if ((double) this.TransitionProgress < 1.0)
          break;
        this.TransitionObject.transform.localPosition = Vector3.zero;
        this.TransitionProgress = 0.0f;
        this.CurrentState = RemoveOnNewDay.State.Idle;
        break;
      case RemoveOnNewDay.State.Leaving:
        if ((double) (this.RandomOffset -= Time.deltaTime) < 0.0)
          this.TransitionObject.transform.localPosition = Vector3.Lerp(Vector3.zero, new Vector3(0.0f, 0.0f, 0.5f), Mathf.SmoothStep(0.0f, 1f, (this.TransitionProgress += Time.deltaTime) / 1f));
        if ((double) this.TransitionProgress >= 1.0)
          this.gameObject.Recycle();
        if (!((UnityEngine.Object) this.inventory != (UnityEngine.Object) null) || this.inventory.Item != InventoryItem.ITEM_TYPE.Necklace_1 && this.inventory.Item != InventoryItem.ITEM_TYPE.Necklace_2 && this.inventory.Item != InventoryItem.ITEM_TYPE.Necklace_3 && this.inventory.Item != InventoryItem.ITEM_TYPE.Necklace_4 && this.inventory.Item != InventoryItem.ITEM_TYPE.Necklace_5)
          break;
        this.inventory.DropItem();
        break;
    }
  }

  private void OnNewPhaseStarted()
  {
    if (TimeManager.CurrentPhase == DayPhase.Night)
      return;
    this.RandomOffset = UnityEngine.Random.Range(0.0f, 3f);
    this.CurrentState = RemoveOnNewDay.State.Leaving;
  }

  private void OnDestroy() => TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhaseStarted);

  private enum State
  {
    Entering,
    Leaving,
    Idle,
  }
}
