// Decompiled with JetBrains decompiler
// Type: DLCMapConnection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using Lamb.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class DLCMapConnection : MonoBehaviour
{
  [SerializeField]
  public DungeonWorldMapIcon _from;
  [SerializeField]
  public DungeonWorldMapIcon _to;
  [SerializeField]
  public Transform _lineRendererNode;
  [SerializeField]
  public MMUILineRenderer _normalLineRenderer;
  [SerializeField]
  public MMUILineRenderer _selectableLineRenderer;
  [SerializeField]
  public MMUILineRenderer _visitedLineRenderer;
  [SerializeField]
  public MMUILineRenderer _highlightedLineRenderer;
  [SerializeField]
  public float _revealDuration = 1f;
  [SerializeField]
  public float _revealDelay = 1f;
  [SerializeField]
  public Color _visitedColour = Color.green;
  [SerializeField]
  public Color _normalColour = Color.grey;
  [SerializeField]
  public Color _highlightedColour = Color.red;
  [SerializeField]
  public Color _selectableColour = Color.white;
  [SerializeField]
  public Material _scrollingMaterial;
  [SerializeField]
  public Material _idleMaterial;
  [CompilerGenerated]
  public DLCMapConnection.ConnectionState \u003CState\u003Ek__BackingField;

  public DungeonWorldMapIcon From => this._from;

  public DungeonWorldMapIcon To => this._to;

  public bool Visible
  {
    get
    {
      Transform lineRendererNode = this._lineRendererNode;
      return lineRendererNode != null && lineRendererNode.gameObject.activeSelf;
    }
  }

  public bool ShouldBeHidden => this.From.ShouldBeHidden || this.To.ShouldBeHidden;

  public DLCMapConnection.ConnectionState State
  {
    get => this.\u003CState\u003Ek__BackingField;
    set => this.\u003CState\u003Ek__BackingField = value;
  }

  public MMUILineRenderer CurrentLineRenderer => this.GetLineRendererForState(this.State);

  public IEnumerable<MMUILineRenderer> LineRenderers
  {
    get
    {
      IEnumerator enumerator = (IEnumerator) this._lineRendererNode.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          MMUILineRenderer component;
          if (((Component) enumerator.Current).gameObject.TryGetComponent<MMUILineRenderer>(out component))
            yield return component;
        }
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
      enumerator = (IEnumerator) null;
    }
  }

  public void SetEndpoints(DungeonWorldMapIcon from, DungeonWorldMapIcon to)
  {
    this._from = from;
    this._to = to;
    Vector3 fromPos = this.From.RectTransform.localPosition;
    Vector3 toPos = this.To.RectTransform.localPosition;
    this.ForEachLineRenderer((Action<MMUILineRenderer>) (lineRenderer => lineRenderer.Points = new List<MMUILineRenderer.BranchPoint>()
    {
      new MMUILineRenderer.BranchPoint((Vector2) fromPos),
      new MMUILineRenderer.BranchPoint((Vector2) toPos)
    }));
  }

  public void UpdateLineEndpoints()
  {
    this.ForEachLineRenderer((Action<MMUILineRenderer>) (lineRenderer =>
    {
      if (lineRenderer.Points == null || lineRenderer.Points.Count != 2)
      {
        lineRenderer.Points = new List<MMUILineRenderer.BranchPoint>()
        {
          new MMUILineRenderer.BranchPoint((Vector2) this.From.RectTransform.localPosition),
          new MMUILineRenderer.BranchPoint((Vector2) this.To.RectTransform.localPosition)
        };
      }
      else
      {
        lineRenderer.Points[0].Point = (Vector2) this.From.RectTransform.localPosition;
        lineRenderer.Points[1].Point = (Vector2) this.To.RectTransform.localPosition;
        lineRenderer.UpdateValues();
        lineRenderer.UpdateRendering();
      }
    }));
  }

  public void RefreshState(bool instant)
  {
    DLCMapConnection.ConnectionState desiredState = this.GetDesiredState();
    if (this.State == desiredState)
      return;
    this.State = desiredState;
    this.ForEachLineRenderer((Action<MMUILineRenderer>) (lr => lr.gameObject.SetActive(false)));
    this.CurrentLineRenderer.gameObject.SetActive(true);
    Color color = this.CurrentLineRenderer.Color with
    {
      a = 1f
    };
    if (this.State == DLCMapConnection.ConnectionState.Greyed)
      color.a = 0.5f;
    if (this.State == DLCMapConnection.ConnectionState.Uninitialised)
      color.a = 0.0f;
    this.CurrentLineRenderer.Color = color;
  }

  public void GetColourAndMaterialForState(
    DLCMapConnection.ConnectionState state,
    out Color colour,
    out Material material)
  {
    colour = new Color();
    material = (Material) null;
    switch (state)
    {
      case DLCMapConnection.ConnectionState.Normal:
        colour = this._normalColour;
        material = this._idleMaterial;
        break;
      case DLCMapConnection.ConnectionState.Visited:
        colour = this._visitedColour;
        material = (Material) null;
        break;
      case DLCMapConnection.ConnectionState.Selectable:
        colour = this._selectableColour;
        material = this._idleMaterial;
        break;
      case DLCMapConnection.ConnectionState.Highlighted:
        colour = this._highlightedColour;
        material = this._scrollingMaterial;
        break;
    }
  }

  public DLCMapConnection.ConnectionState GetDesiredState()
  {
    DungeonWorldMapIcon.IconState currentState1 = this.From.CurrentState;
    DungeonWorldMapIcon.IconState currentState2 = this.To.CurrentState;
    if (currentState1 == DungeonWorldMapIcon.IconState.Locked || currentState2 == DungeonWorldMapIcon.IconState.Locked)
      return DLCMapConnection.ConnectionState.Greyed;
    DLCMapConnection.ConnectionState desiredState;
    switch (currentState1)
    {
      case DungeonWorldMapIcon.IconState.Preview:
        if (currentState2 == DungeonWorldMapIcon.IconState.Unrevealed)
        {
          desiredState = DLCMapConnection.ConnectionState.Greyed;
          break;
        }
        goto default;
      case DungeonWorldMapIcon.IconState.Selectable:
        switch (currentState2)
        {
          case DungeonWorldMapIcon.IconState.Preview:
            desiredState = DLCMapConnection.ConnectionState.Normal;
            break;
          case DungeonWorldMapIcon.IconState.Selectable:
            desiredState = DLCMapConnection.ConnectionState.Normal;
            break;
          default:
            goto label_11;
        }
        break;
      case DungeonWorldMapIcon.IconState.Completed:
        switch (currentState2)
        {
          case DungeonWorldMapIcon.IconState.Selectable:
            desiredState = this.To.IsHighlighted ? DLCMapConnection.ConnectionState.Highlighted : DLCMapConnection.ConnectionState.Selectable;
            break;
          case DungeonWorldMapIcon.IconState.Completed:
            desiredState = DLCMapConnection.ConnectionState.Visited;
            break;
          default:
            goto label_11;
        }
        break;
      default:
label_11:
        desiredState = DLCMapConnection.ConnectionState.Uninitialised;
        break;
    }
    return desiredState;
  }

  public System.Threading.Tasks.Task Reveal()
  {
    return this.Reveal(this._revealDelay, this._revealDuration);
  }

  public async System.Threading.Tasks.Task Reveal(float delay, float duration)
  {
    this.SetFill(0.0f);
    this._lineRendererNode.gameObject.SetActive(true);
    await this.TweenPath(0.0f, 1f, duration, delay);
  }

  public async System.Threading.Tasks.Task HideAsync(float delay, float duration)
  {
    await this.TweenPath(1f, 0.0f, duration, delay);
    this.SetFill(0.0f);
  }

  public void SetFill(float fill, MMUILineRenderer lineRenderer = null)
  {
    this._lineRendererNode.gameObject.SetActive((double) fill != 0.0);
    if ((UnityEngine.Object) lineRenderer != (UnityEngine.Object) null)
    {
      lineRenderer.gameObject.SetActive(true);
      this.ForEachLineRenderer((Action<MMUILineRenderer>) (lr =>
      {
        lr.Fill = 0.0f;
        foreach (MMUILineRenderer.BranchPoint point in lr.Points)
        {
          foreach (MMUILineRenderer.Branch branch in point.Branches)
            branch.Fill = 0.0f;
        }
      }));
    }
    this.ForEachLineRenderer((Action<MMUILineRenderer>) (lr =>
    {
      if ((UnityEngine.Object) lineRenderer != (UnityEngine.Object) null && (UnityEngine.Object) lineRenderer != (UnityEngine.Object) lr)
        return;
      lr.Fill = fill;
      foreach (MMUILineRenderer.BranchPoint point in lr.Points)
      {
        foreach (MMUILineRenderer.Branch branch in point.Branches)
          branch.Fill = fill;
      }
    }));
  }

  public override int GetHashCode() => HashCode.Combine<int, int>(this.From.ID, this.To.ID);

  public void Update() => this.UpdateLineEndpoints();

  public async System.Threading.Tasks.Task TweenPath(
    float from,
    float to,
    float duration,
    float delay,
    MMUILineRenderer lineRenderer = null)
  {
    this.SetFill(from);
    if (Mathf.Approximately(from, to))
      return;
    await DOVirtual.Float(from, to, duration, (TweenCallback<float>) (f => this.SetFill(f, lineRenderer))).SetDelay<Tweener>(delay).SetUpdate<Tweener>(true).OnComplete<Tweener>((TweenCallback) (() =>
    {
      if ((double) to != 0.0)
        return;
      this._lineRendererNode.gameObject.SetActive(false);
    })).SetEase<Tweener>(Ease.Linear).AsyncWaitForCompletion();
  }

  public MMUILineRenderer GetLineRendererForState(DLCMapConnection.ConnectionState state)
  {
    if (!this._from.IsLock && this.To.IsLock && this.To.CurrentState != DungeonWorldMapIcon.IconState.Completed)
      return this._normalLineRenderer;
    switch (state)
    {
      case DLCMapConnection.ConnectionState.Greyed:
        return this._normalLineRenderer;
      case DLCMapConnection.ConnectionState.Normal:
        return this._normalLineRenderer;
      case DLCMapConnection.ConnectionState.Visited:
        return this._visitedLineRenderer;
      case DLCMapConnection.ConnectionState.Selectable:
        return this._selectableLineRenderer;
      case DLCMapConnection.ConnectionState.Highlighted:
        return this._highlightedLineRenderer;
      default:
        return this._normalLineRenderer;
    }
  }

  public void ForEachLineRenderer(Action<MMUILineRenderer> action)
  {
    foreach (MMUILineRenderer lineRenderer in this.LineRenderers)
    {
      if (action != null)
        action(lineRenderer);
    }
  }

  [CompilerGenerated]
  public void \u003CUpdateLineEndpoints\u003Eb__33_0(MMUILineRenderer lineRenderer)
  {
    if (lineRenderer.Points == null || lineRenderer.Points.Count != 2)
    {
      lineRenderer.Points = new List<MMUILineRenderer.BranchPoint>()
      {
        new MMUILineRenderer.BranchPoint((Vector2) this.From.RectTransform.localPosition),
        new MMUILineRenderer.BranchPoint((Vector2) this.To.RectTransform.localPosition)
      };
    }
    else
    {
      lineRenderer.Points[0].Point = (Vector2) this.From.RectTransform.localPosition;
      lineRenderer.Points[1].Point = (Vector2) this.To.RectTransform.localPosition;
      lineRenderer.UpdateValues();
      lineRenderer.UpdateRendering();
    }
  }

  public enum ConnectionState
  {
    Uninitialised,
    Greyed,
    Normal,
    Visited,
    Selectable,
    Highlighted,
  }
}
