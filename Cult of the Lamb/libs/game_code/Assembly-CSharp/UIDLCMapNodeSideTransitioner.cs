// Decompiled with JetBrains decompiler
// Type: UIDLCMapNodeSideTransitioner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

#nullable disable
public class UIDLCMapNodeSideTransitioner : UIDLCMapSideTransitioner
{
  [SerializeField]
  public CanvasGroup _outsideCanvasGroup;
  [SerializeField]
  public CanvasGroup _insideCanvasGroup;
  [SerializeField]
  public CanvasGroup _finalCanvasGroup;
  [SerializeField]
  public float _minDelay;
  [SerializeField]
  public float _maxDelay = 0.2f;
  [SerializeField]
  public float _minDuration = 0.15f;
  [SerializeField]
  public float _maxDuration = 0.25f;

  public CanvasGroup OutsideCanvasGroup => this._outsideCanvasGroup;

  public CanvasGroup InsideCanvasGroup => this._insideCanvasGroup;

  public CanvasGroup FinalCanvasGroup => this._finalCanvasGroup;

  public override void Initialise()
  {
    foreach (DungeonWorldMapIcon dungeonWorldMapIcon in this.NodesForSide(this.CurrentSide))
    {
      dungeonWorldMapIcon.SetDungeonSide(this.CurrentSide);
      dungeonWorldMapIcon.RefreshInteractability();
    }
    for (int index = 0; index < Enum.GetNames(typeof (UIDLCMapMenuController.DLCDungeonSide)).Length; ++index)
    {
      UIDLCMapMenuController.DLCDungeonSide side = (UIDLCMapMenuController.DLCDungeonSide) index;
      if (side != this.CurrentSide)
      {
        foreach (DungeonWorldMapIcon dungeonWorldMapIcon in this.NodesForSide(side))
        {
          dungeonWorldMapIcon.SetDungeonSide(side);
          dungeonWorldMapIcon.RefreshInteractability();
        }
      }
    }
    this.OutsideContainer.gameObject.SetActive(true);
    this.InsideContainer.gameObject.SetActive(true);
    this.OutsideCanvasGroup.alpha = this.CurrentSide == UIDLCMapMenuController.DLCDungeonSide.OutsideMountain ? 1f : 0.0f;
    this.InsideCanvasGroup.alpha = this.CurrentSide == UIDLCMapMenuController.DLCDungeonSide.InsideMountain ? 1f : 0.0f;
  }

  public IEnumerable<DungeonWorldMapIcon> NodesForSide(UIDLCMapMenuController.DLCDungeonSide side)
  {
    DungeonWorldMapIcon[] dungeonWorldMapIconArray = this.GetContainerForSide(side).GetComponentsInChildren<DungeonWorldMapIcon>(true);
    for (int index = 0; index < dungeonWorldMapIconArray.Length; ++index)
      yield return dungeonWorldMapIconArray[index];
    dungeonWorldMapIconArray = (DungeonWorldMapIcon[]) null;
  }

  public override async System.Threading.Tasks.Task ShowLayerAsync(
    UIDLCMapMenuController.DLCDungeonSide side)
  {
    Debug.Log((object) $"Showing nodes on side: {side}");
    this.CanvasGroupForSide(side).alpha = 1f;
    List<System.Threading.Tasks.Task> taskList;
    PooledObject<List<System.Threading.Tasks.Task>> pooledObject = CollectionPool<List<System.Threading.Tasks.Task>, System.Threading.Tasks.Task>.Get(out taskList);
    try
    {
      foreach (DungeonWorldMapIcon node in this.NodesForSide(side))
      {
        if (node.ShouldBeHidden)
          node.transform.localScale = Vector3.one * 0.5f;
        float delay = UnityEngine.Random.Range(this._minDelay, this._maxDelay);
        float duration = UnityEngine.Random.Range(this._minDuration, this._maxDuration);
        taskList.Add(this.FadeNodeInAsync(node, delay, duration));
      }
      await System.Threading.Tasks.Task.WhenAll((IEnumerable<System.Threading.Tasks.Task>) taskList);
    }
    finally
    {
      pooledObject.Dispose();
    }
    pooledObject = new PooledObject<List<System.Threading.Tasks.Task>>();
  }

  public async System.Threading.Tasks.Task FadeNodeInAsync(
    DungeonWorldMapIcon node,
    float delay,
    float duration)
  {
    await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds((double) delay));
    node.RectTransform.localScale = Vector3.zero;
    Vector3 scale = node.startingScale;
    if (node.IsLock && node.CurrentState == DungeonWorldMapIcon.IconState.Completed)
      scale = Vector3.zero;
    await node.transform.DOScale(scale, duration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).AsyncWaitForCompletion();
    node.RectTransform.localScale = scale;
    node.RefreshInteractability();
  }

  public override async System.Threading.Tasks.Task HideLayerAsync(
    UIDLCMapMenuController.DLCDungeonSide side)
  {
    Debug.Log((object) $"Hiding nodes on side: {side}");
    List<System.Threading.Tasks.Task> taskList;
    PooledObject<List<System.Threading.Tasks.Task>> pooledObject = CollectionPool<List<System.Threading.Tasks.Task>, System.Threading.Tasks.Task>.Get(out taskList);
    try
    {
      foreach (DungeonWorldMapIcon node in this.NodesForSide(side))
      {
        if (node.gameObject.activeSelf)
        {
          float delay = UnityEngine.Random.Range(this._minDelay, this._maxDelay);
          float duration = UnityEngine.Random.Range(this._minDuration, this._maxDuration);
          taskList.Add(this.FadeNodeOutAsync(node, delay, duration));
        }
      }
      await System.Threading.Tasks.Task.WhenAll((IEnumerable<System.Threading.Tasks.Task>) taskList);
    }
    finally
    {
      pooledObject.Dispose();
    }
    pooledObject = new PooledObject<List<System.Threading.Tasks.Task>>();
    this.CanvasGroupForSide(side).alpha = 0.0f;
    await System.Threading.Tasks.Task.Yield();
  }

  public async System.Threading.Tasks.Task FadeNodeOutAsync(
    DungeonWorldMapIcon node,
    float delay,
    float duration)
  {
    await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds((double) delay));
    await node.transform.DOScale(0.0f, duration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).AsyncWaitForCompletion();
    node.RefreshInteractability();
  }

  public CanvasGroup CanvasGroupForSide(UIDLCMapMenuController.DLCDungeonSide side)
  {
    CanvasGroup canvasGroup;
    switch (side)
    {
      case UIDLCMapMenuController.DLCDungeonSide.InsideMountain:
        canvasGroup = this.InsideCanvasGroup;
        break;
      case UIDLCMapMenuController.DLCDungeonSide.OutsideMountain:
        canvasGroup = this.OutsideCanvasGroup;
        break;
      default:
        canvasGroup = this.OutsideCanvasGroup;
        break;
    }
    return canvasGroup;
  }
}
