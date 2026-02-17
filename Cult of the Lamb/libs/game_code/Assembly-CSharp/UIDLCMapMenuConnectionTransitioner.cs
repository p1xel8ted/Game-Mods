// Decompiled with JetBrains decompiler
// Type: UIDLCMapMenuConnectionTransitioner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

#nullable disable
public class UIDLCMapMenuConnectionTransitioner : UIDLCMapSideTransitioner
{
  [SerializeField]
  public float _minDelay;
  [SerializeField]
  public float _maxDelay = 0.2f;
  [SerializeField]
  public float _minDuration = 0.15f;
  [SerializeField]
  public float _maxDuration = 0.25f;

  public override void Initialise()
  {
    this.OutsideContainer.gameObject.SetActive(this.CurrentSide == UIDLCMapMenuController.DLCDungeonSide.OutsideMountain);
    this.InsideContainer.gameObject.SetActive(this.CurrentSide == UIDLCMapMenuController.DLCDungeonSide.InsideMountain);
  }

  public override async System.Threading.Tasks.Task ShowLayerAsync(
    UIDLCMapMenuController.DLCDungeonSide side)
  {
    UIDLCMapMenuConnectionTransitioner connectionTransitioner = this;
    RectTransform containerForSide = connectionTransitioner.GetContainerForSide(side);
    containerForSide.gameObject.SetActive(true);
    List<System.Threading.Tasks.Task> taskList;
    PooledObject<List<System.Threading.Tasks.Task>> pooledObject = CollectionPool<List<System.Threading.Tasks.Task>, System.Threading.Tasks.Task>.Get(out taskList);
    try
    {
      IEnumerator enumerator = (IEnumerator) containerForSide.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          DLCMapConnection component;
          if (((Component) enumerator.Current).TryGetComponent<DLCMapConnection>(out component) && !component.ShouldBeHidden)
          {
            float delay = UnityEngine.Random.Range(connectionTransitioner._minDelay, connectionTransitioner._maxDelay);
            float duration = UnityEngine.Random.Range(connectionTransitioner._minDuration, connectionTransitioner._maxDuration);
            taskList.Add(connectionTransitioner.TransitionConnectionInWithDelayAsync(component, delay, duration));
          }
        }
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
      await System.Threading.Tasks.Task.WhenAll((IEnumerable<System.Threading.Tasks.Task>) taskList);
    }
    finally
    {
      pooledObject.Dispose();
    }
    pooledObject = new PooledObject<List<System.Threading.Tasks.Task>>();
  }

  public override async System.Threading.Tasks.Task HideLayerAsync(
    UIDLCMapMenuController.DLCDungeonSide side)
  {
    UIDLCMapMenuConnectionTransitioner connectionTransitioner = this;
    RectTransform container = connectionTransitioner.GetContainerForSide(side);
    List<System.Threading.Tasks.Task> taskList;
    PooledObject<List<System.Threading.Tasks.Task>> pooledObject = CollectionPool<List<System.Threading.Tasks.Task>, System.Threading.Tasks.Task>.Get(out taskList);
    try
    {
      IEnumerator enumerator = (IEnumerator) container.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          DLCMapConnection component;
          if (((Component) enumerator.Current).TryGetComponent<DLCMapConnection>(out component) && !component.ShouldBeHidden)
          {
            float delay = UnityEngine.Random.Range(connectionTransitioner._minDelay, connectionTransitioner._maxDelay);
            float duration = UnityEngine.Random.Range(connectionTransitioner._minDuration, connectionTransitioner._maxDuration);
            taskList.Add(connectionTransitioner.TransitionConnectionOutWithDelayAsync(component, delay, duration));
          }
        }
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
      await System.Threading.Tasks.Task.WhenAll((IEnumerable<System.Threading.Tasks.Task>) taskList);
    }
    finally
    {
      pooledObject.Dispose();
    }
    pooledObject = new PooledObject<List<System.Threading.Tasks.Task>>();
    container.gameObject.SetActive(false);
    container = (RectTransform) null;
  }

  public async System.Threading.Tasks.Task TransitionConnectionOutWithDelayAsync(
    DLCMapConnection connection,
    float delay,
    float duration)
  {
    await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds((double) delay));
    if (!connection.Visible)
      return;
    await connection.HideAsync(delay, duration);
  }

  public async System.Threading.Tasks.Task TransitionConnectionInWithDelayAsync(
    DLCMapConnection connection,
    float delay,
    float duration)
  {
    await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds((double) delay));
    await connection.Reveal(delay, duration);
  }
}
