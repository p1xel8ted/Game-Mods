// Decompiled with JetBrains decompiler
// Type: CrownMapMarker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

#nullable disable
public class CrownMapMarker : MonoBehaviour
{
  [SerializeField]
  public RectTransform _crownSpineRectTransform;
  [SerializeField]
  public RectTransform _eyeSpineRectTransform;
  [SerializeField]
  public float _normalScale = 0.5f;
  [SerializeField]
  public float _initialMoveDuration = 2f;
  [SerializeField]
  public float _moveDurationSpeedUpFactor = 0.9f;
  [SerializeField]
  public float _pauseDurationBetweenHops = 0.1f;
  [SerializeField]
  public float _fadeInDuration = 0.5f;
  [SerializeField]
  public Ease _fadeInEaseType = Ease.OutQuart;
  [SerializeField]
  public float _fadeOutDuration = 0.5f;
  [SerializeField]
  public Ease _fadeOutEaseType = Ease.OutQuart;
  [SerializeField]
  public Ease _hopEaseType = Ease.OutExpo;
  [SerializeField]
  public DungeonWorldMapIcon _worldMapIcon;
  [SerializeField]
  public bool _hasMoved;
  [SerializeField]
  public Vector3 _savedPos;
  public DungeonWorldMapIcon CurrentNode;
  public DungeonWorldMapIcon SelectNode;

  public event Action<Vector3> CrownMoved;

  public RectTransform CrownSpine => this._crownSpineRectTransform;

  public RectTransform Eye => this._eyeSpineRectTransform;

  public void SetSelectedNode(DungeonWorldMapIcon node)
  {
    this.SelectNode = node;
    this.CheckIfOnNode();
  }

  public void CheckIfOnNode()
  {
    if ((UnityEngine.Object) this.CurrentNode == (UnityEngine.Object) this.SelectNode)
    {
      if (this._hasMoved)
        return;
      this._savedPos = this.transform.position;
      this.transform.DOMoveY(this.CurrentNode.Content._selectIconTransform.position.y, 0.5f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart);
      this._hasMoved = true;
    }
    else
    {
      if (!this._hasMoved)
        return;
      this.transform.DOMoveY(this.CurrentNode.transform.position.y, 0.5f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart);
      this._hasMoved = false;
    }
  }

  public void LookAt(Vector3 point)
  {
    this.Eye.DOKill();
    Vector2 zero = Vector2.zero;
    this.Eye.DOLocalMove((Vector3) (Vector2) ((point - this._eyeSpineRectTransform.position).normalized * 25f), 0.75f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart);
  }

  public void Teleport(Vector3 point, DungeonWorldMapIcon icon)
  {
    if ((UnityEngine.Object) icon != (UnityEngine.Object) null)
    {
      this._worldMapIcon = icon;
      this.CurrentNode = icon;
    }
    this.CrownSpine.position = point;
    this.CheckIfOnNode();
    Action<Vector3> crownMoved = this.CrownMoved;
    if (crownMoved == null)
      return;
    crownMoved(point);
  }

  public void OnDestroy() => this.KillAllTweens();

  public void KillAllTweens()
  {
    this._crownSpineRectTransform.DOKill();
    this.Eye.DOKill();
    this.CrownSpine.DOKill();
  }

  public async System.Threading.Tasks.Task ShowAsync()
  {
    if ((double) this._crownSpineRectTransform.localScale.x > 0.019999999552965164)
      return;
    Debug.Log((object) "Starting to Show Crown");
    await this._crownSpineRectTransform.DOScale(Vector3.one * this._normalScale, this._fadeInDuration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(this._fadeInEaseType).From<Vector3, Vector3, VectorOptions>(Vector3.zero).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).AsyncWaitForCompletion();
    Debug.Log((object) "Finished Showing Crown");
  }

  public void Show()
  {
    this._crownSpineRectTransform.DOScale(Vector3.one * this._normalScale, 0.5f).From<Vector3, Vector3, VectorOptions>(Vector3.zero).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart);
  }

  public async System.Threading.Tasks.Task HideAsync()
  {
    Debug.Log((object) "Starting to Hide Crown");
    await this._crownSpineRectTransform.DOScale(Vector3.zero, this._fadeOutDuration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(this._fadeOutEaseType).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.5f).AsyncWaitForCompletion();
    Debug.Log((object) "Finished Hiding Crown");
  }

  public async System.Threading.Tasks.Task MoveToParentAsync(DungeonWorldMapIcon node)
  {
    DungeonWorldMapIcon targetNode = node.ParentNodes.First<DungeonWorldMapIcon>();
    await this.MoveToAsync(node.RectTransform.position, targetNode.RectTransform.position, targetNode);
  }

  public async System.Threading.Tasks.Task MoveToAsync(
    Vector3 from,
    Vector3 to,
    DungeonWorldMapIcon targetNode)
  {
    List<Vector3> path;
    PooledObject<List<Vector3>> pooledObject = CollectionPool<List<Vector3>, Vector3>.Get(out path);
    try
    {
      path.Add(from);
      path.Add(to);
      await this.MoveToAsync(path, targetNode);
      Action<Vector3> crownMoved = this.CrownMoved;
      if (crownMoved != null)
        crownMoved(to);
    }
    finally
    {
      pooledObject.Dispose();
    }
    pooledObject = new PooledObject<List<Vector3>>();
  }

  public async System.Threading.Tasks.Task MoveToAsync(
    List<Vector3> path,
    DungeonWorldMapIcon targetNode)
  {
    bool first = true;
    float duration = this._initialMoveDuration;
    float pauseInterval = this._pauseDurationBetweenHops;
    foreach (Vector3 vector3 in path)
    {
      Vector3 p = vector3;
      if (first)
      {
        first = false;
        this.Teleport(p, (DungeonWorldMapIcon) null);
      }
      else
      {
        this.LookAt(p);
        this.CrownSpine.transform.DOShakeScale(0.5f, Vector3.one * 0.05f).SetLoops<Tweener>(-1).SetUpdate<Tweener>(true).SetEase<Tweener>(Ease.InOutQuart).SetLoops<Tweener>(-1);
        UIManager.PlayAudio("event:/dlc/ui/map/node_move");
        await this.CrownSpine.transform.DOMove(p, duration).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart).AsyncWaitForCompletion();
        Action<Vector3> crownMoved = this.CrownMoved;
        if (crownMoved != null)
          crownMoved(p);
        if ((double) pauseInterval > 1.0 / 1000.0)
          await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds((double) pauseInterval));
        p = new Vector3();
      }
    }
    this.CrownSpine.transform.DOKill();
    this.LookAt(this.CrownSpine.transform.position);
    if (!((UnityEngine.Object) targetNode != (UnityEngine.Object) null))
      return;
    this._worldMapIcon = targetNode;
    this.CurrentNode = targetNode;
  }
}
