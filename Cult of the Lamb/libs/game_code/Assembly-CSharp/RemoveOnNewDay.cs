// Decompiled with JetBrains decompiler
// Type: RemoveOnNewDay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class RemoveOnNewDay : BaseMonoBehaviour
{
  public RemoveOnNewDay.State CurrentState;
  public float TransitionProgress;
  public GameObject TransitionObject;
  public Vector3 StartingPosition;
  public float RandomOffset;
  [SerializeField]
  public SimpleInventory inventory;

  public void OnEnable()
  {
    if (TimeManager.CurrentPhase != DayPhase.Night)
      return;
    this.CurrentState = RemoveOnNewDay.State.Entering;
    this.RandomOffset = UnityEngine.Random.Range(0.0f, 3f);
    this.TransitionObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.5f);
  }

  public void Start()
  {
    TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStarted);
    this.CurrentState = RemoveOnNewDay.State.Entering;
    this.RandomOffset = UnityEngine.Random.Range(0.0f, 3f);
    this.TransitionObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.5f);
  }

  public void Update()
  {
    switch (this.CurrentState)
    {
      case RemoveOnNewDay.State.Entering:
        if ((double) (this.RandomOffset -= Time.deltaTime) < 0.0)
          this.TransitionObject.transform.localPosition = Vector3.Lerp(new Vector3(0.0f, 0.0f, 0.5f), Vector3.zero, Mathf.SmoothStep(0.0f, 1f, (this.TransitionProgress += Time.deltaTime) / 1f));
        if ((double) this.TransitionProgress >= 1.0)
        {
          this.TransitionObject.transform.localPosition = Vector3.zero;
          this.TransitionProgress = 0.0f;
          this.CurrentState = RemoveOnNewDay.State.Idle;
          break;
        }
        break;
      case RemoveOnNewDay.State.Leaving:
        if ((double) (this.RandomOffset -= Time.deltaTime) < 0.0)
          this.TransitionObject.transform.localPosition = Vector3.Lerp(Vector3.zero, new Vector3(0.0f, 0.0f, 0.5f), Mathf.SmoothStep(0.0f, 1f, (this.TransitionProgress += Time.deltaTime) / 1f));
        if ((double) this.TransitionProgress >= 1.0)
          this.gameObject.Recycle();
        if ((UnityEngine.Object) this.inventory != (UnityEngine.Object) null && DataManager.AllNecklaces.Contains(this.inventory.Item))
        {
          this.inventory.DropItem();
          break;
        }
        break;
    }
    if (TimeManager.CurrentPhase == DayPhase.Night || this.CurrentState == RemoveOnNewDay.State.Leaving)
      return;
    this.RandomOffset = UnityEngine.Random.Range(0.0f, 3f);
    this.CurrentState = RemoveOnNewDay.State.Leaving;
  }

  public void OnNewPhaseStarted()
  {
    if (TimeManager.CurrentPhase == DayPhase.Night)
      return;
    this.RandomOffset = UnityEngine.Random.Range(0.0f, 3f);
    this.CurrentState = RemoveOnNewDay.State.Leaving;
  }

  public void OnDestroy() => TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhaseStarted);

  public enum State
  {
    Entering,
    Leaving,
    Idle,
  }
}
