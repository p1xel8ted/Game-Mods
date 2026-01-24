// Decompiled with JetBrains decompiler
// Type: Unity.VideoHelper.DisplayControllerInternal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Unity.VideoHelper;

public class DisplayControllerInternal : IDisplayController
{
  public Vector2 anchorMin;
  public Vector2 anchorMax;
  public Vector2 offsetMin;
  public Vector2 offsetMax;
  public Vector3 scale;
  public GameObject fullscreenCanvas;
  public RectTransform target;
  public RectTransform targetParent;
  public bool isAlwaysFullscreen;
  public int targetDisplay;

  public DisplayControllerInternal(int display) => this.targetDisplay = display;

  public bool IsFullscreen
  {
    get => (UnityEngine.Object) this.fullscreenCanvas != (UnityEngine.Object) null && this.fullscreenCanvas.activeSelf;
  }

  public void ToFullscreen(RectTransform rectTransform)
  {
    if ((UnityEngine.Object) this.fullscreenCanvas == (UnityEngine.Object) null)
      this.Setup();
    this.target = rectTransform;
    this.targetParent = this.target.parent as RectTransform;
    this.anchorMax = this.target.anchorMax;
    this.anchorMin = this.target.anchorMin;
    this.offsetMax = this.target.offsetMax;
    this.offsetMin = this.target.offsetMin;
    this.scale = this.target.localScale;
    this.fullscreenCanvas.SetActive(true);
    this.target.SetParent(this.fullscreenCanvas.transform);
    this.target.anchorMin = this.target.offsetMin = Vector2.zero;
    this.target.anchorMax = this.target.offsetMax = Vector2.one;
    this.target.localScale = Vector3.one;
    this.isAlwaysFullscreen = Screen.fullScreen;
    Screen.fullScreen = true;
  }

  public void ToNormal()
  {
    if ((UnityEngine.Object) this.target == (UnityEngine.Object) null)
      return;
    this.target.SetParent((Transform) this.targetParent);
    this.target.anchorMax = this.anchorMax;
    this.target.anchorMin = this.anchorMin;
    this.target.offsetMax = this.offsetMax;
    this.target.offsetMin = this.offsetMin;
    this.target.localScale = this.scale;
    this.fullscreenCanvas.SetActive(false);
    Screen.fullScreen = this.isAlwaysFullscreen;
  }

  public void Setup()
  {
    this.fullscreenCanvas = new GameObject("_DisplayController_ForDisplay_" + this.targetDisplay.ToString(), (System.Type[]) new System.Type[3]
    {
      typeof (Canvas),
      typeof (CanvasScaler),
      typeof (GraphicRaycaster)
    });
    Canvas component = this.fullscreenCanvas.GetComponent<Canvas>();
    component.renderMode = RenderMode.ScreenSpaceOverlay;
    component.targetDisplay = this.targetDisplay;
    this.fullscreenCanvas.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
  }
}
