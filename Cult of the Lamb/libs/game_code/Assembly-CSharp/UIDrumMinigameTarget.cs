// Decompiled with JetBrains decompiler
// Type: UIDrumMinigameTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIDrumMinigameTarget : MonoBehaviour
{
  public Image Background;
  public Image Ring;
  public Color left;
  public Color middle;
  public Color right;
  [CompilerGenerated]
  public int \u003CButton\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CLane\u003Ek__BackingField;
  public float time;
  public float duration;
  public Vector3 endPos;
  public bool _Ready;
  public bool _Failed;

  public bool Ready => (double) this.time / (double) this.duration >= 0.75;

  public bool Failed => (double) this.time / (double) this.duration >= 1.0;

  public int Button
  {
    get => this.\u003CButton\u003Ek__BackingField;
    set => this.\u003CButton\u003Ek__BackingField = value;
  }

  public int Lane
  {
    get => this.\u003CLane\u003Ek__BackingField;
    set => this.\u003CLane\u003Ek__BackingField = value;
  }

  public void Configure(
    int button,
    int lane,
    Vector3 startPos,
    Vector3 endPos,
    Vector3 missOffset,
    float duration)
  {
    this.Button = button;
    this.transform.localPosition = startPos;
    this.duration = duration;
    this.endPos = endPos;
    this.Lane = lane;
    switch (lane)
    {
      case 0:
        this.Background.color = this.left;
        break;
      case 1:
        this.Background.color = this.middle;
        break;
      case 2:
        this.Background.color = this.right;
        break;
      default:
        this.Background.color = Color.white;
        this.Ring.color = Color.white;
        break;
    }
    this.transform.DOLocalMove(endPos + missOffset, duration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear);
    this.Ring.gameObject.SetActive(false);
  }

  public void Update()
  {
    if (this.Ready && !this._Ready && !this.Failed && !this._Failed)
    {
      this._Ready = true;
      this.Ring.transform.localScale = Vector3.zero;
      this.Ring.transform.DOScale(Vector3.one * 1.1f, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      this.Ring.gameObject.SetActive(true);
    }
    if (this.Failed || this._Failed)
      return;
    this.time += Time.deltaTime;
  }

  public void Success()
  {
    this.transform.DOKill();
    this.transform.localPosition = this.endPos;
    this.Ring.DOKill();
    this.Ring.gameObject.SetActive(false);
    DOTweenModuleUI.DOFade(this.Background, 0.0f, 0.4f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this.transform.DOScale(Vector3.one * 2f, 0.4f).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => Object.Destroy((Object) this.gameObject))).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this.Background.color = StaticColors.GreenColor;
  }

  public void Fail()
  {
    this._Failed = true;
    this.Ring.DOKill();
    this.Ring.transform.DOScale(Vector3.zero, 0.2f).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.Ring.gameObject.SetActive(false)));
    this.transform.DOKill();
    this.transform.DOShakePosition(1f, new Vector3(20f, 0.0f), randomness: 0.0f);
    this.Background.color = StaticColors.RedColor;
    DG.Tweening.Sequence s = DOTween.Sequence();
    s.AppendInterval(0.2f);
    s.Append((Tween) DOTweenModuleUI.DOFade(this.Background, 0.0f, 0.5f).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() => Object.Destroy((Object) this.gameObject))));
  }

  [CompilerGenerated]
  public void \u003CSuccess\u003Eb__24_0() => Object.Destroy((Object) this.gameObject);

  [CompilerGenerated]
  public void \u003CFail\u003Eb__25_0() => this.Ring.gameObject.SetActive(false);

  [CompilerGenerated]
  public void \u003CFail\u003Eb__25_1() => Object.Destroy((Object) this.gameObject);
}
