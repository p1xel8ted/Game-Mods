// Decompiled with JetBrains decompiler
// Type: UITwitchTotemWheel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using Lamb.UI.Assets;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UITwitchTotemWheel : UIMenuBase
{
  private const string kXPositionProperty = "_XPosition";
  private const string kYPositionProperty = "_YPosition";
  [SerializeField]
  private RectTransform _container;
  [SerializeField]
  private Transform _arrowPivot;
  [SerializeField]
  private InventoryIconMapping _inventoryIconMapping;
  [SerializeField]
  private UITwitchTotemWheel.WheelSegment[] _wheelSegments;
  [SerializeField]
  private AnimationCurve _curve;
  [SerializeField]
  private MMRadialLayoutGroup _radialLayoutGroup;
  [SerializeField]
  private MMUIRadialGraphic _radialGraphic;
  [SerializeField]
  private ParticleSystem _confettiLeft;
  [SerializeField]
  private ParticleSystem _confettiRight;
  private UITwitchTotemWheel.Segment[] _segments;
  private Vector2[] _segmentMinMaxes;
  private Material _radialMaterial;

  public UITwitchTotemWheel.Segment ChosenSegment { get; private set; }

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
    this._confettiLeft.Stop();
    this._confettiLeft.Clear();
    this._confettiRight.Stop();
    this._confettiRight.Clear();
  }

  public void Show(UITwitchTotemWheel.Segment[] segments, bool instant = false)
  {
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

  protected override IEnumerator DoShow()
  {
    yield return (object) base.DoShow();
    yield return (object) new WaitForSecondsRealtime(0.5f);
  }

  protected override void OnShowCompleted()
  {
    base.OnShowCompleted();
    this.SpinWheel();
  }

  protected override IEnumerator DoHide()
  {
    int rand = UnityEngine.Random.Range(0, 2);
    if (rand == 0)
      this._confettiLeft.Play();
    else
      this._confettiRight.Play();
    yield return (object) new WaitForSecondsRealtime(0.1f);
    if (rand == 0)
      this._confettiRight.Play();
    else
      this._confettiLeft.Play();
    yield return (object) new WaitForSecondsRealtime(1.5f);
    yield return (object) base.DoHide();
  }

  protected override void OnHideCompleted()
  {
    base.OnHideCompleted();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  private void Update()
  {
    Vector2 vector2 = Utils.DegreeToVector2(this._arrowPivot.rotation.eulerAngles.z + 270f);
    vector2.Normalize();
    this._radialMaterial.SetFloat("_XPosition", vector2.x);
    this._radialMaterial.SetFloat("_YPosition", vector2.y);
    Quaternion quaternion = this._arrowPivot.rotation;
    quaternion = quaternion.normalized;
    float message = (float) (360.0 - (double) quaternion.eulerAngles.z);
    Debug.Log((object) message);
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
    this.ChosenSegment = (UITwitchTotemWheel.Segment) null;
    while (this.ChosenSegment == null)
    {
      UITwitchTotemWheel.Segment segment = this._segments[UnityEngine.Random.Range(0, this._segments.Length)];
      if ((double) UnityEngine.Random.value < (double) segment.probability)
        this.ChosenSegment = segment;
    }
    Vector2 angleFromSegment = this.GetMinMaxAngleFromSegment(this.ChosenSegment);
    this._arrowPivot.transform.DORotate(new Vector3(0.0f, 0.0f, UnityEngine.Random.Range(angleFromSegment.x, angleFromSegment.y) * -360f - (float) (360 * UnityEngine.Random.Range(6, 9))), UnityEngine.Random.Range(4.5f, 5.5f), RotateMode.LocalAxisAdd).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(this._curve).SetUpdate<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(true).OnComplete<TweenerCore<Quaternion, Vector3, QuaternionOptions>>((TweenCallback) (() => this.Hide()));
  }

  private Vector2 GetMinMaxAngleFromSegment(UITwitchTotemWheel.Segment segment)
  {
    int num = this._segments.IndexOf<UITwitchTotemWheel.Segment>(segment);
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

  protected override void OnDestroy()
  {
    base.OnDestroy();
    if (!((UnityEngine.Object) this._radialMaterial != (UnityEngine.Object) null))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this._radialMaterial);
    this._radialMaterial = (Material) null;
  }

  public class Segment
  {
    public InventoryItem.ITEM_TYPE reward;
    public float probability;
  }

  [Serializable]
  private struct WheelSegment
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
