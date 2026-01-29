// Decompiled with JetBrains decompiler
// Type: PuzzleSlot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class PuzzleSlot : MonoBehaviour
{
  public static List<PuzzleSlot> PuzzleSlots = new List<PuzzleSlot>();
  [SerializeField]
  public Interaction_PuzzleItem targetItem;
  [Header("Pickupable")]
  [SerializeField]
  public UnityEvent onCorrect;
  [SerializeField]
  public UnityEvent onIncorrect;
  [SerializeField]
  public UnityEvent onEmpty;
  [Header("Spikes")]
  [SerializeField]
  public int startingOffset;
  [SerializeField]
  public bool isSpikes;
  [Header("Charger")]
  [SerializeField]
  public float rotMultiplier = 1f;
  [CompilerGenerated]
  public bool \u003CCorrect\u003Ek__BackingField;
  public int counter;
  public bool boundsSet;
  public Bounds collisionGraphBounds;
  public EventInstance loopingSound;

  public bool IsSpikes => this.isSpikes;

  public bool Correct
  {
    get => this.\u003CCorrect\u003Ek__BackingField;
    set => this.\u003CCorrect\u003Ek__BackingField = value;
  }

  public void Start()
  {
    this.counter += this.startingOffset;
    this.TrySetBounds();
  }

  public void OnEnable() => PuzzleSlot.PuzzleSlots.Add(this);

  public void OnDisable()
  {
    PuzzleSlot.PuzzleSlots.Remove(this);
    AudioManager.Instance.StopLoop(this.loopingSound);
  }

  public void OnDestroy() => AudioManager.Instance.StopLoop(this.loopingSound);

  public bool IsCorrect(Interaction_PuzzleItem puzzleItem)
  {
    return (UnityEngine.Object) this.targetItem == (UnityEngine.Object) puzzleItem;
  }

  public void Added(Interaction_PuzzleItem puzzleItem)
  {
    if ((UnityEngine.Object) this.targetItem == (UnityEngine.Object) puzzleItem)
    {
      ++this.counter;
      if (this.counter == 0)
        ++this.counter;
      if (this.counter > 0)
        this.onCorrect?.Invoke();
    }
    else if ((UnityEngine.Object) puzzleItem == (UnityEngine.Object) null)
      this.onEmpty?.Invoke();
    else
      this.onIncorrect?.Invoke();
    this.Correct = (UnityEngine.Object) this.targetItem == (UnityEngine.Object) puzzleItem;
    bool allCorrect = true;
    foreach (PuzzleSlot puzzleSlot in PuzzleSlot.PuzzleSlots)
    {
      if (!puzzleSlot.Correct && puzzleSlot.targetItem.Type != Interaction_PuzzleItem.PuzzleItemType.Switch)
        allCorrect = false;
    }
    if (this.Correct && (bool) (UnityEngine.Object) puzzleItem.GetComponent<TrapCharger>())
    {
      TrapCharger component = puzzleItem.GetComponent<TrapCharger>();
      component.enabled = false;
      component.AdditionalLine.gameObject.SetActive(true);
      component.AdditionalLine.material.SetFloat("_FillAmount", 0.0f);
      component.AdditionalLine.material.SetFloat("_Multiplier", this.rotMultiplier);
      component.AdditionalLine.material.DOFloat(1f, "_FillAmount", 3f).SetDelay<TweenerCore<float, float, FloatOptions>>(1.5f).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
      {
        if (!allCorrect || this.targetItem.Type == Interaction_PuzzleItem.PuzzleItemType.Switch)
          return;
        PuzzleController.Instance.Complete();
      }));
      AudioManager.Instance.PlayOneShot("event:/dlc/env/puzzle_room/effigy_complete", component.transform.position);
      this.loopingSound = AudioManager.Instance.CreateLoop("event:/dlc/env/puzzle_room/effigy_active_loop", this.gameObject, true);
    }
    else
    {
      if (!allCorrect || this.targetItem.Type == Interaction_PuzzleItem.PuzzleItemType.Switch)
        return;
      GameManager.GetInstance().WaitForSeconds(2f, (System.Action) (() => PuzzleController.Instance.Complete()));
    }
  }

  public void Removed()
  {
    --this.counter;
    if (this.counter == 0)
      --this.counter;
    if (this.counter >= 0)
      return;
    this.onIncorrect?.Invoke();
    this.Correct = false;
  }

  public void OnTriggerEnter2D(Collider2D collision)
  {
    Interaction_PuzzleItem component = collision.GetComponent<Interaction_PuzzleItem>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.Added(component);
  }

  public void OnTriggerExit2D(Collider2D collision)
  {
    if (!((UnityEngine.Object) collision.GetComponent<Interaction_PuzzleItem>() != (UnityEngine.Object) null))
      return;
    this.Added((Interaction_PuzzleItem) null);
  }

  public void UpdateGraph()
  {
    this.TrySetBounds();
    if (!this.boundsSet)
      return;
    AstarPath.active.UpdateGraphs(this.collisionGraphBounds);
  }

  public void TrySetBounds()
  {
    if (this.boundsSet)
      return;
    BoxCollider2D component = this.GetComponent<BoxCollider2D>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.enabled)
      return;
    this.collisionGraphBounds = component.bounds;
    this.boundsSet = true;
  }
}
