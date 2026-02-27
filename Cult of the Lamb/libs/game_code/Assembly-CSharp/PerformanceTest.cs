// Decompiled with JetBrains decompiler
// Type: PerformanceTest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using LeTai.Asset.TranslucentImage;
using Spine.Unity;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.U2D;
using UnityEngine.UI;

#nullable disable
public class PerformanceTest : MonoBehaviour
{
  public static bool ReduceGUI;
  public static bool ReduceCPU;
  public static bool ReduceOnRender;
  public PostProcessVolume CurrentPostProcessVolume;
  public Vignette vignette;
  public ChromaticAberration chromaticAbberration;
  public Bloom bloom;
  public ColorGrading colorGrading;
  public VFXImpactFramePPSSettings vFXImpactFramePPSSettings;
  public ImpactFrameBlackPPSSettings impactFrameBlackPPSSettings;
  public Grain grain;
  public AmplifyPostEffect amp;
  public GameObject OptionTemplate;
  public Button DefaultSelect;
  public UINavigator _uiNavigator;

  public void AddToggle(string name, Action<TMP_Text> onToggle, Action<TMP_Text> onUpdate)
  {
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.OptionTemplate, this.OptionTemplate.transform.parent);
    gameObject.transform.SetAsLastSibling();
    gameObject.transform.GetChild(0).GetComponent<Text>().text = name;
    gameObject.name = name;
    Button componentInChildren = gameObject.transform.GetComponentInChildren<Button>();
    TMP_Text label = componentInChildren.transform.GetChild(0).GetComponent<TMP_Text>();
    onUpdate(label);
    this._uiNavigator.Buttons.Add(new Buttons()
    {
      Button = componentInChildren.gameObject,
      Index = 1
    });
    componentInChildren.onClick.AddListener((UnityAction) (() =>
    {
      onToggle(label);
      onUpdate(label);
    }));
  }

  public void OnEnable()
  {
    this._uiNavigator = this.OptionTemplate.transform.parent.GetComponent<UINavigator>();
    this.CurrentPostProcessVolume = (PostProcessVolume) UnityEngine.Object.FindObjectOfType(typeof (PostProcessVolume));
    PostProcessLayer ppLayer = (PostProcessLayer) UnityEngine.Object.FindObjectOfType(typeof (PostProcessLayer));
    this.AddToggle("Shadows Enabled", (Action<TMP_Text>) (label => QualitySettings.shadows = QualitySettings.shadows == ShadowQuality.Disable ? ShadowQuality.All : ShadowQuality.Disable), (Action<TMP_Text>) (label => label.text = QualitySettings.shadows == ShadowQuality.Disable ? "O" : "I"));
    this.AddToggle("Depth Texture", (Action<TMP_Text>) (label => Camera.main.depthTextureMode = Camera.main.depthTextureMode == DepthTextureMode.None ? DepthTextureMode.Depth : DepthTextureMode.None), (Action<TMP_Text>) (label => label.text = Camera.main.depthTextureMode == DepthTextureMode.None ? "O" : "I"));
    this.AddToggle("Reduce CPU", (Action<TMP_Text>) (label =>
    {
      PerformanceTest.ReduceCPU = !PerformanceTest.ReduceCPU;
      SkeletonAnimation.StopUpdates = SkeletonGraphic.StopUpdates = PerformanceTest.ReduceCPU;
    }), (Action<TMP_Text>) (label => label.text = PerformanceTest.ReduceCPU ? "I" : "O"));
    this.AddToggle("Reduce GUI", (Action<TMP_Text>) (label => PerformanceTest.ReduceGUI = !PerformanceTest.ReduceGUI), (Action<TMP_Text>) (label => label.text = PerformanceTest.ReduceGUI ? "I" : "O"));
    this.AddToggle("Reduce Render", (Action<TMP_Text>) (label => StencilLighting_MaskEffect.DisableRender = TranslucentImageSource.DisableRender = !StencilLighting_MaskEffect.DisableRender), (Action<TMP_Text>) (label => label.text = StencilLighting_MaskEffect.DisableRender ? "I" : "O"));
    this.AddToggle("Post Proc", (Action<TMP_Text>) (label => ppLayer.enabled = !ppLayer.enabled), (Action<TMP_Text>) (label => label.text = ppLayer.isActiveAndEnabled ? "I" : "O"));
    this.AddToggle("Vsync", (Action<TMP_Text>) (label => QualitySettings.vSyncCount ^= 1), (Action<TMP_Text>) (label => label.text = QualitySettings.vSyncCount == 0 ? "O" : "I"));
    this.AddToggle("No Lights", (Action<TMP_Text>) (label =>
    {
      foreach (Light light in UnityEngine.Object.FindObjectsOfType<Light>())
      {
        if (light.tag != null && !light.tag.StartsWith("Main"))
          light.enabled = false;
      }
    }), (Action<TMP_Text>) (label => label.text = " "));
    StencilLighting_DecalSprite[] StencilLighting_DecalSprites = UnityEngine.Object.FindObjectsOfType<StencilLighting_DecalSprite>();
    GameObject gameObject1 = UnityEngine.Object.Instantiate<GameObject>(this.OptionTemplate, this.OptionTemplate.transform.parent);
    gameObject1.transform.SetAsLastSibling();
    gameObject1.transform.GetChild(0).GetComponent<Text>().text = "StencilLighting_DecalSprite";
    gameObject1.name = "StencilLighting_DecalSprite";
    Button componentInChildren1 = gameObject1.transform.GetComponentInChildren<Button>();
    TMP_Text StencilLighting_DecalSpriteButtonlabel = componentInChildren1.transform.GetChild(0).GetComponent<TMP_Text>();
    StencilLighting_DecalSpriteButtonlabel.text = !StencilLighting_DecalSprites[0].enabled ? "O" : "I";
    this._uiNavigator.Buttons.Add(new Buttons()
    {
      Button = componentInChildren1.gameObject,
      Index = 2
    });
    componentInChildren1.onClick.AddListener((UnityAction) (() =>
    {
      for (int index = 0; index < StencilLighting_DecalSprites.Length; ++index)
      {
        StencilLighting_DecalSprites[index].enabled = !StencilLighting_DecalSprites[index].enabled;
        StencilLighting_DecalSprites[index].gameObject.SetActive(StencilLighting_DecalSprites[index].enabled);
      }
      if (StencilLighting_DecalSprites[0].enabled)
        StencilLighting_DecalSpriteButtonlabel.text = "I";
      else
        StencilLighting_DecalSpriteButtonlabel.text = "O";
    }));
    SpriteShapeController[] spriteShapeControllers = UnityEngine.Object.FindObjectsOfType<SpriteShapeController>();
    GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.OptionTemplate, this.OptionTemplate.transform.parent);
    gameObject2.transform.SetAsLastSibling();
    gameObject2.transform.GetChild(0).GetComponent<Text>().text = "spriteShapeController";
    gameObject2.name = "spriteShapeController";
    Button componentInChildren2 = gameObject2.transform.GetComponentInChildren<Button>();
    TMP_Text spriteShapeControllerButtonlabel = componentInChildren2.transform.GetChild(0).GetComponent<TMP_Text>();
    spriteShapeControllerButtonlabel.text = !spriteShapeControllers[0].enabled ? "O" : "I";
    this._uiNavigator.Buttons.Add(new Buttons()
    {
      Button = componentInChildren2.gameObject,
      Index = 3
    });
    componentInChildren2.onClick.AddListener((UnityAction) (() =>
    {
      for (int index = 0; index < spriteShapeControllers.Length; ++index)
      {
        spriteShapeControllers[index].enabled = !spriteShapeControllers[index].enabled;
        spriteShapeControllers[index].gameObject.SetActive(spriteShapeControllers[index].enabled);
      }
      if (StencilLighting_DecalSprites[0].enabled)
        spriteShapeControllerButtonlabel.text = "I";
      else
        spriteShapeControllerButtonlabel.text = "O";
    }));
    this.getPostProcessingItems();
    for (int index1 = 0; index1 < this.CurrentPostProcessVolume.profile.settings.Count; ++index1)
    {
      GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(this.OptionTemplate, this.OptionTemplate.transform.parent);
      gameObject3.transform.SetAsLastSibling();
      gameObject3.name = this.CurrentPostProcessVolume.profile.settings[index1].GetType().ToString();
      gameObject3.transform.GetChild(0).GetComponent<Text>().text = this.CurrentPostProcessVolume.profile.settings[index1].GetType().ToString().Replace("UnityEngine.Rendering.PostProcessing.", "");
      Button componentInChildren3 = gameObject3.transform.GetComponentInChildren<Button>();
      int index2 = index1;
      this._uiNavigator.Buttons.Add(new Buttons()
      {
        Button = componentInChildren3.gameObject,
        Index = 3 + index1
      });
      System.Type t = this.CurrentPostProcessVolume.profile.settings[index2].GetType();
      TMP_Text buttonlabel = componentInChildren3.transform.GetChild(0).GetComponent<TMP_Text>();
      buttonlabel.text = !this.CurrentPostProcessVolume.profile.settings[index1].active ? "O" : "I";
      Debug.Log((object) t.ToString());
      componentInChildren3.onClick.AddListener((UnityAction) (() => this.postprocessingpress(t, buttonlabel)));
    }
    this.DefaultSelect.Select();
    this.OptionTemplate.SetActive(false);
  }

  public void postprocessingpress(System.Type t, TMP_Text buttonlabel)
  {
    if (t == typeof (Vignette))
    {
      buttonlabel.text = !this.vignette.active ? "O" : "I";
      this.vignette.active = !this.vignette.active;
    }
    else if (t == typeof (ChromaticAberration))
    {
      buttonlabel.text = !this.chromaticAbberration.active ? "O" : "I";
      this.chromaticAbberration.active = !this.chromaticAbberration.active;
    }
    else if (t == typeof (Bloom))
    {
      buttonlabel.text = !this.bloom.active ? "O" : "I";
      this.bloom.active = !this.bloom.active;
    }
    else if (t == typeof (ColorGrading))
    {
      buttonlabel.text = !this.colorGrading.active ? "O" : "I";
      this.colorGrading.active = !this.colorGrading.active;
    }
    else if (t == typeof (VFXImpactFramePPSSettings))
    {
      buttonlabel.text = !this.vFXImpactFramePPSSettings.active ? "O" : "I";
      this.vFXImpactFramePPSSettings.active = !this.vFXImpactFramePPSSettings.active;
    }
    else if (t == typeof (ImpactFrameBlackPPSSettings))
    {
      buttonlabel.text = !this.impactFrameBlackPPSSettings.active ? "O" : "I";
      this.impactFrameBlackPPSSettings.active = !this.impactFrameBlackPPSSettings.active;
    }
    else if (t == typeof (Grain))
    {
      buttonlabel.text = !this.grain.active ? "O" : "I";
      this.grain.active = !this.grain.active;
    }
    else
    {
      if (!(t == typeof (AmplifyPostEffect)))
        return;
      buttonlabel.text = !this.amp.active ? "O" : "I";
      this.amp.active = !this.amp.active;
    }
  }

  public void getPostProcessingItems()
  {
    this.CurrentPostProcessVolume.profile.TryGetSettings<Vignette>(out this.vignette);
    this.CurrentPostProcessVolume.profile.TryGetSettings<ChromaticAberration>(out this.chromaticAbberration);
    this.CurrentPostProcessVolume.profile.TryGetSettings<Bloom>(out this.bloom);
    this.CurrentPostProcessVolume.profile.TryGetSettings<Grain>(out this.grain);
    this.CurrentPostProcessVolume.profile.TryGetSettings<ImpactFrameBlackPPSSettings>(out this.impactFrameBlackPPSSettings);
    this.CurrentPostProcessVolume.profile.TryGetSettings<VFXImpactFramePPSSettings>(out this.vFXImpactFramePPSSettings);
    this.CurrentPostProcessVolume.profile.TryGetSettings<AmplifyPostEffect>(out this.amp);
  }

  public void Close() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
}
