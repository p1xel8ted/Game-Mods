// Decompiled with JetBrains decompiler
// Type: UIRandomWheel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using Lamb.UI.Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIRandomWheel : UIMenuBase
{
  public const string kXPositionProperty = "_XPosition";
  public const string kYPositionProperty = "_YPosition";
  [SerializeField]
  public RectTransform _container;
  [SerializeField]
  public Transform _arrowPivot;
  [SerializeField]
  public InventoryIconMapping _inventoryIconMapping;
  [SerializeField]
  public UIRandomWheel.WheelSegment[] _wheelSegments;
  [SerializeField]
  public AnimationCurve _curve;
  [SerializeField]
  public MMRadialLayoutGroup _radialLayoutGroup;
  [SerializeField]
  public MMUIRadialGraphic _radialGraphic;
  [SerializeField]
  public ParticleSystem _confettiLeft;
  [SerializeField]
  public ParticleSystem _confettiRight;
  public UIRandomWheel.Segment[] _segments;
  public Vector2[] _segmentMinMaxes;
  public Material _radialMaterial;
  public List<InventoryItem.ITEM_TYPE> excludedItems;
  [CompilerGenerated]
  public UIRandomWheel.Segment \u003CChosenSegment\u003Ek__BackingField;
  [CompilerGenerated]
  public float \u003CSpeedMultiplier\u003Ek__BackingField = 1f;

  public UIRandomWheel.Segment ChosenSegment
  {
    get => this.\u003CChosenSegment\u003Ek__BackingField;
    set => this.\u003CChosenSegment\u003Ek__BackingField = value;
  }

  public UIRandomWheel.WheelSegment[] WheelSegments => this._wheelSegments;

  public float SpeedMultiplier
  {
    get => this.\u003CSpeedMultiplier\u003Ek__BackingField;
    set => this.\u003CSpeedMultiplier\u003Ek__BackingField = value;
  }

  public override void Awake()
  {
    base.Awake();
    for (int index = 0; index < this._wheelSegments.Length; ++index)
    {
      this._wheelSegments[index].ImageCanvasGroup = this._wheelSegments[index].ImageFill.GetComponent<CanvasGroup>();
      this._wheelSegments[index].RectTransform.gameObject.SetActive(false);
      this._wheelSegments[index].Icon.gameObject.SetActive(false);
      this._wheelSegments[index].IconCanvasGroup = this._wheelSegments[index].Icon.GetComponent<CanvasGroup>();
    }
    this._radialMaterial = new Material(this._radialGraphic.material);
    this._radialGraphic.material = this._radialMaterial;
    if ((bool) (UnityEngine.Object) this._confettiLeft)
    {
      this._confettiLeft.Stop();
      this._confettiLeft.Clear();
    }
    if (!(bool) (UnityEngine.Object) this._confettiRight)
      return;
    this._confettiRight.Stop();
    this._confettiRight.Clear();
  }

  public void Show(
    UIRandomWheel.Segment[] segments,
    bool instant = false,
    List<InventoryItem.ITEM_TYPE> excludedItems = null)
  {
    this.excludedItems = excludedItems;
    this.gameObject.SetActive(true);
    this._segments = segments;
    this._segmentMinMaxes = new Vector2[this._segments.Length];
    float num = 0.0f;
    for (int index = 0; index < segments.Length; ++index)
    {
      this._wheelSegments[index].ImageFill.fillAmount = segments[index].probability;
      this._wheelSegments[index].RectTransform.gameObject.SetActive(true);
      this._wheelSegments[index].Icon.gameObject.SetActive(true);
      this._wheelSegments[index].RectTransform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, num * -360f));
      num += segments[index].probability;
      this._segmentMinMaxes[index] = new Vector2((float) (((double) num - (double) segments[index].probability) * 360.0), num * 360f);
      if (index == 0)
        this._radialLayoutGroup.Offset = (float) ((double) num * 0.5 * 360.0);
      this._wheelSegments[index].Icon.sprite = this._inventoryIconMapping.GetImage(segments[index].reward);
      if (segments[index].reward == InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION)
        this._wheelSegments[index].ImageFill.color = StaticColors.TwitchPurple;
      else
        this._wheelSegments[index].ImageFill.color = StaticColors.GreyColor;
    }
    this.transform.localScale = Vector3.zero;
    this.Show(instant);
  }

  public override IEnumerator DoShow()
  {
    yield return (object) this.\u003C\u003En__0();
    yield return (object) new WaitForSecondsRealtime(0.5f);
  }

  public override void OnShowCompleted()
  {
    base.OnShowCompleted();
    this.SpinWheel();
  }

  public override IEnumerator DoHide()
  {
    int rand = UnityEngine.Random.Range(0, 2);
    if (rand == 0 && (bool) (UnityEngine.Object) this._confettiLeft)
      this._confettiLeft?.Play();
    else if ((bool) (UnityEngine.Object) this._confettiRight)
      this._confettiRight?.Play();
    yield return (object) new WaitForSecondsRealtime(0.1f / this.SpeedMultiplier);
    if (rand == 0 && (bool) (UnityEngine.Object) this._confettiRight)
      this._confettiRight?.Play();
    else if ((bool) (UnityEngine.Object) this._confettiLeft)
      this._confettiLeft?.Play();
    yield return (object) new WaitForSecondsRealtime(1.5f / this.SpeedMultiplier);
    yield return (object) this.\u003C\u003En__1();
  }

  public override void OnHideCompleted()
  {
    base.OnHideCompleted();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void Update()
  {
    Vector2 vector2 = Utils.DegreeToVector2(this._arrowPivot.rotation.eulerAngles.z + 270f);
    vector2.Normalize();
    this._radialMaterial.SetFloat("_XPosition", vector2.x);
    this._radialMaterial.SetFloat("_YPosition", vector2.y);
    Quaternion quaternion = this._arrowPivot.rotation;
    quaternion = quaternion.normalized;
    float message = (float) (360.0 - (double) quaternion.eulerAngles.z);
    UnityEngine.Debug.Log((object) message);
    for (int index = 0; index < this._segmentMinMaxes.Length; ++index)
    {
      if ((double) message > (double) this._segmentMinMaxes[index].x && (double) message < (double) this._segmentMinMaxes[index].y)
      {
        this._wheelSegments[index].IconCanvasGroup.alpha = 1f;
        this._wheelSegments[index].ImageCanvasGroup.alpha = 1f;
      }
      else
      {
        this._wheelSegments[index].IconCanvasGroup.alpha = 0.6f;
        this._wheelSegments[index].ImageCanvasGroup.alpha = 0.6f;
      }
    }
  }

  public void SpinWheel()
  {
    this.ChosenSegment = (UIRandomWheel.Segment) null;
    while (this.ChosenSegment == null)
    {
      UIRandomWheel.Segment segment = this._segments[UnityEngine.Random.Range(0, this._segments.Length)];
      if ((double) UnityEngine.Random.value < (double) segment.probability && (this.excludedItems == null || !this.excludedItems.Contains(segment.reward)))
        this.ChosenSegment = segment;
    }
    Vector2 angleFromSegment = this.GetMinMaxAngleFromSegment(this.ChosenSegment);
    this._arrowPivot.transform.DORotate(new Vector3(0.0f, 0.0f, UnityEngine.Random.Range(angleFromSegment.x, angleFromSegment.y) * -360f - (float) (360 * UnityEngine.Random.Range(6, 9))), UnityEngine.Random.Range(4.5f, 5.5f) / this.SpeedMultiplier, RotateMode.LocalAxisAdd).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(this._curve).SetUpdate<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(true).OnComplete<TweenerCore<Quaternion, Vector3, QuaternionOptions>>((TweenCallback) (() => this.Hide()));
  }

  public Vector2 GetMinMaxAngleFromSegment(UIRandomWheel.Segment segment)
  {
    int num = this._segments.IndexOf<UIRandomWheel.Segment>(segment);
    float x = 0.0f;
    float y = 0.0f;
    for (int index = 0; index < this._segments.Length; ++index)
    {
      if (index < num)
      {
        x += this._segments[index].probability;
      }
      else
      {
        y = x + this._segments[index].probability;
        break;
      }
    }
    return new Vector2(x, y);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (!((UnityEngine.Object) this._radialMaterial != (UnityEngine.Object) null))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this._radialMaterial);
    this._radialMaterial = (Material) null;
  }

  [CompilerGenerated]
  [DebuggerHidden]
  public IEnumerator \u003C\u003En__0() => base.DoShow();

  [CompilerGenerated]
  [DebuggerHidden]
  public IEnumerator \u003C\u003En__1() => base.DoHide();

  [CompilerGenerated]
  public void \u003CSpinWheel\u003Eb__34_0() => this.Hide();

  public class Segment
  {
    public InventoryItem.ITEM_TYPE reward;
    public float probability;

    public Segment()
    {
    }

    public Segment(InventoryItem.ITEM_TYPE reward, float probability)
    {
      this.reward = reward;
      this.probability = probability;
    }
  }

  [Serializable]
  public struct WheelSegment
  {
    public Image ImageFill;
    [HideInInspector]
    public CanvasGroup ImageCanvasGroup;
    public RectTransform RectTransform;
    public Image Icon;
    [HideInInspector]
    public CanvasGroup IconCanvasGroup;
  }
}
