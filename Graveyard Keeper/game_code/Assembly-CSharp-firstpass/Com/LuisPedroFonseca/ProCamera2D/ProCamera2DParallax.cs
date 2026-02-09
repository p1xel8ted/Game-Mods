// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.ProCamera2DParallax
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

[ExecuteInEditMode]
[HelpURL("http://www.procamera2d.com/user-guide/extension-parallax/")]
public class ProCamera2DParallax : BasePC2D, IPostMover
{
  public static string ExtensionName = "Parallax";
  public List<ProCamera2DParallaxLayer> ParallaxLayers = new List<ProCamera2DParallaxLayer>();
  public bool ParallaxHorizontal = true;
  public bool ParallaxVertical = true;
  public bool ParallaxZoom = true;
  public Vector3 RootPosition = Vector3.zero;
  public int FrontDepthStart = 1;
  public int BackDepthStart = -1;
  public float _initialOrtographicSize;
  public float[] _initialSpeeds;
  public Coroutine _animateCoroutine;
  public int _pmOrder = 1000;

  public override void Awake()
  {
    base.Awake();
    if ((Object) this.ProCamera2D == (Object) null)
      return;
    if (Application.isPlaying)
      this.CalculateParallaxObjectsOffset();
    foreach (ProCamera2DParallaxLayer parallaxLayer in this.ParallaxLayers)
    {
      if ((Object) parallaxLayer.ParallaxCamera != (Object) null)
        parallaxLayer.CameraTransform = parallaxLayer.ParallaxCamera.transform;
    }
    this._initialSpeeds = new float[this.ParallaxLayers.Count];
    for (int index = 0; index < this._initialSpeeds.Length; ++index)
      this._initialSpeeds[index] = this.ParallaxLayers[index].Speed;
    if ((Object) this.ProCamera2D.GameCamera != (Object) null)
    {
      this._initialOrtographicSize = this.ProCamera2D.GameCamera.orthographicSize;
      if (!this.ProCamera2D.GameCamera.orthographic)
        this.enabled = false;
    }
    Com.LuisPedroFonseca.ProCamera2D.ProCamera2D.Instance.AddPostMover((IPostMover) this);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.ProCamera2D.RemovePostMover((IPostMover) this);
  }

  public void PostMove(float deltaTime)
  {
    if (!this.enabled)
      return;
    this.Move();
  }

  public int PMOrder
  {
    get => this._pmOrder;
    set => this._pmOrder = value;
  }

  public void CalculateParallaxObjectsOffset()
  {
    ProCamera2DParallaxObject[] objectsOfType = Object.FindObjectsOfType<ProCamera2DParallaxObject>();
    Dictionary<int, ProCamera2DParallaxLayer> dictionary = new Dictionary<int, ProCamera2DParallaxLayer>();
    for (int key = 0; key <= 31 /*0x1F*/; ++key)
    {
      foreach (ProCamera2DParallaxLayer parallaxLayer in this.ParallaxLayers)
      {
        if ((int) parallaxLayer.LayerMask == ((int) parallaxLayer.LayerMask | 1 << key))
          dictionary[key] = parallaxLayer;
      }
    }
    for (int index = 0; index < objectsOfType.Length; ++index)
    {
      Vector3 vector3 = objectsOfType[index].transform.position - this.RootPosition;
      float num1 = this.Vector3H(vector3) * dictionary[objectsOfType[index].gameObject.layer].Speed;
      float num2 = this.Vector3V(vector3) * dictionary[objectsOfType[index].gameObject.layer].Speed;
      objectsOfType[index].transform.position = this.VectorHVD(num1, num2, this.Vector3D(vector3)) + this.RootPosition;
    }
  }

  public void Move()
  {
    Vector3 vector3 = this._transform.position - this.RootPosition;
    for (int index = 0; index < this.ParallaxLayers.Count; ++index)
    {
      if ((Object) this.ParallaxLayers[index].CameraTransform != (Object) null)
      {
        float num1 = this.ParallaxHorizontal ? this.Vector3H(vector3) * this.ParallaxLayers[index].Speed : this.Vector3H(vector3);
        float num2 = this.ParallaxVertical ? this.Vector3V(vector3) * this.ParallaxLayers[index].Speed : this.Vector3V(vector3);
        this.ParallaxLayers[index].CameraTransform.position = this.RootPosition + this.VectorHVD(num1, num2, this.Vector3D(this._transform.position));
        if (this.ParallaxZoom)
          this.ParallaxLayers[index].ParallaxCamera.orthographicSize = this._initialOrtographicSize + (this.ProCamera2D.GameCamera.orthographicSize - this._initialOrtographicSize) * this.ParallaxLayers[index].Speed;
      }
    }
  }

  public void ToggleParallax(bool value, float duration = 2f, EaseType easeType = EaseType.EaseInOut)
  {
    if (this._initialSpeeds == null)
      return;
    if (this._animateCoroutine != null)
      this.StopCoroutine(this._animateCoroutine);
    this._animateCoroutine = this.StartCoroutine(this.Animate(value, duration, easeType));
  }

  public IEnumerator Animate(bool value, float duration, EaseType easeType)
  {
    ProCamera2DParallax camera2Dparallax = this;
    float[] currentSpeeds = new float[camera2Dparallax.ParallaxLayers.Count];
    for (int index = 0; index < currentSpeeds.Length; ++index)
      currentSpeeds[index] = camera2Dparallax.ParallaxLayers[index].Speed;
    float t = 0.0f;
    while ((double) t <= 1.0)
    {
      t += camera2Dparallax.ProCamera2D.DeltaTime / duration;
      for (int index = 0; index < camera2Dparallax.ParallaxLayers.Count; ++index)
        camera2Dparallax.ParallaxLayers[index].Speed = !value ? Utils.EaseFromTo(currentSpeeds[index], 1f, t, easeType) : Utils.EaseFromTo(currentSpeeds[index], camera2Dparallax._initialSpeeds[index], t, easeType);
      yield return (object) camera2Dparallax.ProCamera2D.GetYield();
    }
  }
}
