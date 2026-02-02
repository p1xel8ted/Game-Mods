// Decompiled with JetBrains decompiler
// Type: VFX_CoOpSpawnScreenEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class VFX_CoOpSpawnScreenEffect : MonoBehaviour
{
  [SerializeField]
  public Material mat;
  [SerializeField]
  public Image image;
  [SerializeField]
  public GameObject child;
  [SerializeField]
  public CanvasGroup blendImage;
  public static int Strength = Shader.PropertyToID("_Strength");
  public static int WaveHeight = Shader.PropertyToID("_WaveHeight");
  public static int WaveLength = Shader.PropertyToID("_WaveLength");

  public void Start() => this.gameObject.SetActive(false);

  public void Init()
  {
    if (!SettingsManager.Settings.Accessibility.FlashingLights)
      return;
    this.gameObject.SetActive(true);
    if ((Object) this.image == (Object) null)
      this.image = this.GetComponent<Image>();
    if ((Object) this.mat == (Object) null)
      this.mat = this.image.material;
    if ((Object) this.child == (Object) null)
      this.child = this.gameObject.transform.GetChild(0).gameObject;
    Image component = this.child.GetComponent<Image>();
    Material target = this.image.material = new Material(this.mat);
    target.SetFloat(VFX_CoOpSpawnScreenEffect.Strength, 500f);
    target.SetFloat(VFX_CoOpSpawnScreenEffect.WaveHeight, 1000f);
    target.SetFloat(VFX_CoOpSpawnScreenEffect.WaveLength, 0.0f);
    this.blendImage.alpha = 1f;
    this.blendImage.DOKill();
    this.blendImage.DOFade(0.0f, 2f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    this.image.color = Color.white;
    component.color = Color.black;
    component.DOKill();
    DOTweenModuleUI.DOFade(component, 0.0f, 0.15f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() => { }));
    target.DOKill();
    target.DOFloat(0.0f, VFX_CoOpSpawnScreenEffect.Strength, 2.5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutCubic).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).SetDelay<TweenerCore<float, float, FloatOptions>>(1f);
    target.DOFloat(100f, VFX_CoOpSpawnScreenEffect.WaveLength, 2.5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutCubic).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    target.DOFloat(0.0f, VFX_CoOpSpawnScreenEffect.WaveHeight, 2.5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutCubic).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).SetDelay<TweenerCore<float, float, FloatOptions>>(1f).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
    {
      this.image.DOKill();
      DOTweenModuleUI.DOFade(this.image, 0.0f, 1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      this.gameObject.SetActive(false);
    }));
  }

  [CompilerGenerated]
  public void \u003CInit\u003Eb__8_1()
  {
    this.image.DOKill();
    DOTweenModuleUI.DOFade(this.image, 0.0f, 1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this.gameObject.SetActive(false);
  }
}
