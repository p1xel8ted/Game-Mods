// Decompiled with JetBrains decompiler
// Type: LambGhostEnter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine.Unity;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class LambGhostEnter : MonoBehaviour
{
  [SerializeField]
  public SkeletonAnimation spine;
  [SerializeField]
  public Vector3 fromLocalPosition;
  [SerializeField]
  public Vector3 endScale = Vector3.one;
  [SerializeField]
  public bool onEnable = true;
  [SerializeField]
  public float delay;
  [SerializeField]
  public UnityEvent onStart;
  [SerializeField]
  public UnityEvent onComplete;
  public Vector3 toLocalPosition;

  public void Awake() => this.toLocalPosition = this.spine.transform.localPosition;

  public void OnEnable()
  {
    if (!this.onEnable || !((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null))
      return;
    this.spine.transform.localPosition = this.fromLocalPosition;
    this.spine.transform.DOKill();
    GameManager.GetInstance().WaitForSeconds(this.delay, (System.Action) (() =>
    {
      this.onStart?.Invoke();
      this.spine.transform.DOLocalMove(this.toLocalPosition, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
      {
        if (PlayerFarming.Location == this.GetComponentInParent<HubLocationManager>().Location)
          this.onComplete?.Invoke();
        this.spine.transform.localScale = this.endScale;
      }));
    }));
  }

  public void Play()
  {
    this.spine.transform.localPosition = this.fromLocalPosition;
    this.spine.transform.DOKill();
    GameManager.GetInstance().WaitForSeconds(this.delay, (System.Action) (() =>
    {
      this.onStart?.Invoke();
      this.spine.transform.DOLocalMove(this.toLocalPosition, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
      {
        if (PlayerFarming.Location == this.GetComponentInParent<HubLocationManager>().Location)
          this.onComplete?.Invoke();
        this.spine.transform.localScale = this.endScale;
      }));
    }));
  }

  [CompilerGenerated]
  public void \u003COnEnable\u003Eb__9_0()
  {
    this.onStart?.Invoke();
    this.spine.transform.DOLocalMove(this.toLocalPosition, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      if (PlayerFarming.Location == this.GetComponentInParent<HubLocationManager>().Location)
        this.onComplete?.Invoke();
      this.spine.transform.localScale = this.endScale;
    }));
  }

  [CompilerGenerated]
  public void \u003COnEnable\u003Eb__9_1()
  {
    if (PlayerFarming.Location == this.GetComponentInParent<HubLocationManager>().Location)
      this.onComplete?.Invoke();
    this.spine.transform.localScale = this.endScale;
  }

  [CompilerGenerated]
  public void \u003CPlay\u003Eb__10_0()
  {
    this.onStart?.Invoke();
    this.spine.transform.DOLocalMove(this.toLocalPosition, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      if (PlayerFarming.Location == this.GetComponentInParent<HubLocationManager>().Location)
        this.onComplete?.Invoke();
      this.spine.transform.localScale = this.endScale;
    }));
  }

  [CompilerGenerated]
  public void \u003CPlay\u003Eb__10_1()
  {
    if (PlayerFarming.Location == this.GetComponentInParent<HubLocationManager>().Location)
      this.onComplete?.Invoke();
    this.spine.transform.localScale = this.endScale;
  }
}
