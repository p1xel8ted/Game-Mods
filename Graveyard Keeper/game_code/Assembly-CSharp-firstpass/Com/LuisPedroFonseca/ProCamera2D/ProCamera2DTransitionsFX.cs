// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.ProCamera2DTransitionsFX
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

[HelpURL("http://www.procamera2d.com/user-guide/extension-transitions-fx/")]
public class ProCamera2DTransitionsFX : BasePC2D
{
  public static string ExtensionName = "TransitionsFX";
  public Action OnTransitionEnterStarted;
  public Action OnTransitionEnterEnded;
  public Action OnTransitionExitStarted;
  public Action OnTransitionExitEnded;
  public Action OnTransitionStarted;
  public Action OnTransitionEnded;
  public static ProCamera2DTransitionsFX _instance;
  public TransitionsFXShaders TransitionShaderEnter;
  public float DurationEnter = 0.5f;
  public float DelayEnter;
  public EaseType EaseTypeEnter = EaseType.EaseOut;
  public Color BackgroundColorEnter = Color.black;
  public TransitionFXSide SideEnter;
  public TransitionFXDirection DirectionEnter;
  [Range(2f, 128f)]
  public int BlindsEnter = 16 /*0x10*/;
  public Texture TextureEnter;
  [Range(0.0f, 1f)]
  public float TextureSmoothingEnter = 0.2f;
  public TransitionsFXShaders TransitionShaderExit;
  public float DurationExit = 0.5f;
  public float DelayExit;
  public EaseType EaseTypeExit = EaseType.EaseOut;
  public Color BackgroundColorExit = Color.black;
  public TransitionFXSide SideExit;
  public TransitionFXDirection DirectionExit;
  [Range(2f, 128f)]
  public int BlindsExit = 16 /*0x10*/;
  public Texture TextureExit;
  [Range(0.0f, 1f)]
  public float TextureSmoothingExit = 0.2f;
  public bool StartSceneOnEnterState = true;
  public Coroutine _transitionCoroutine;
  public float _step;
  public Material _transitionEnterMaterial;
  public Material _transitionExitMaterial;
  public BasicBlit _blit;
  public int _material_StepID;
  public int _material_BackgroundColorID;
  public string _previousEnterShader = "";
  public string _previousExitShader = "";

  public static ProCamera2DTransitionsFX Instance
  {
    get
    {
      if (object.Equals((object) ProCamera2DTransitionsFX._instance, (object) null))
      {
        ProCamera2DTransitionsFX._instance = Com.LuisPedroFonseca.ProCamera2D.ProCamera2D.Instance.GetComponent<ProCamera2DTransitionsFX>();
        if (object.Equals((object) ProCamera2DTransitionsFX._instance, (object) null))
          throw new UnityException("ProCamera2D does not have a TransitionFX extension.");
      }
      return ProCamera2DTransitionsFX._instance;
    }
  }

  public override void Awake()
  {
    base.Awake();
    ProCamera2DTransitionsFX._instance = this;
    this._material_StepID = Shader.PropertyToID("_Step");
    this._material_BackgroundColorID = Shader.PropertyToID("_BackgroundColor");
    this._blit = this.gameObject.AddComponent<BasicBlit>();
    this._blit.enabled = false;
    this.UpdateTransitionsShaders();
    this.UpdateTransitionsProperties();
    this.UpdateTransitionsColor();
    if (!this.StartSceneOnEnterState)
      return;
    this._step = 1f;
    this._blit.CurrentMaterial = this._transitionEnterMaterial;
    this._blit.CurrentMaterial.SetFloat(this._material_StepID, this._step);
    this._blit.enabled = true;
  }

  public void TransitionEnter(float? time = null)
  {
    this.Transition(this._transitionEnterMaterial, (float) ((double) time ?? (double) this.DurationEnter), this.DelayEnter, 1f, 0.0f, this.EaseTypeEnter);
  }

  public void TransitionExit(float? time = null)
  {
    this.Transition(this._transitionExitMaterial, (float) ((double) time ?? (double) this.DurationExit), this.DelayExit, 0.0f, 1f, this.EaseTypeExit);
  }

  public void UpdateTransitionsShaders()
  {
    string str1 = this.TransitionShaderEnter.ToString();
    if (!this._previousEnterShader.Equals(str1))
    {
      this._transitionEnterMaterial = new Material(Shader.Find("Hidden/ProCamera2D/TransitionsFX/" + str1));
      this._previousEnterShader = str1;
    }
    string str2 = this.TransitionShaderExit.ToString();
    if (this._previousExitShader.Equals(str2))
      return;
    this._transitionExitMaterial = new Material(Shader.Find("Hidden/ProCamera2D/TransitionsFX/" + str2));
    this._previousExitShader = str2;
  }

  public void UpdateTransitionsProperties()
  {
    if (this.TransitionShaderEnter == TransitionsFXShaders.Wipe || this.TransitionShaderEnter == TransitionsFXShaders.Blinds)
    {
      this._transitionEnterMaterial.SetInt("_Direction", (int) this.SideEnter);
      this._transitionEnterMaterial.SetInt("_Blinds", this.BlindsEnter);
    }
    else if (this.TransitionShaderEnter == TransitionsFXShaders.Shutters)
      this._transitionEnterMaterial.SetInt("_Direction", (int) this.DirectionEnter);
    else if (this.TransitionShaderEnter == TransitionsFXShaders.Texture)
    {
      this._transitionEnterMaterial.SetTexture("_TransitionTex", this.TextureEnter);
      this._transitionEnterMaterial.SetFloat("_Smoothing", this.TextureSmoothingEnter);
    }
    if (this.TransitionShaderExit == TransitionsFXShaders.Wipe || this.TransitionShaderExit == TransitionsFXShaders.Blinds)
    {
      this._transitionExitMaterial.SetInt("_Direction", (int) this.SideExit);
      this._transitionExitMaterial.SetInt("_Blinds", this.BlindsExit);
    }
    else if (this.TransitionShaderExit == TransitionsFXShaders.Shutters)
    {
      this._transitionExitMaterial.SetInt("_Direction", (int) this.DirectionExit);
    }
    else
    {
      if (this.TransitionShaderExit != TransitionsFXShaders.Texture)
        return;
      this._transitionExitMaterial.SetTexture("_TransitionTex", this.TextureExit);
      this._transitionExitMaterial.SetFloat("_Smoothing", this.TextureSmoothingExit);
    }
  }

  public void UpdateTransitionsColor()
  {
    this._transitionEnterMaterial.SetColor(this._material_BackgroundColorID, this.BackgroundColorEnter);
    this._transitionExitMaterial.SetColor(this._material_BackgroundColorID, this.BackgroundColorExit);
  }

  public void Clear() => this._blit.enabled = false;

  public void Transition(
    Material material,
    float duration,
    float delay,
    float startValue,
    float endValue,
    EaseType easeType)
  {
    if ((UnityEngine.Object) this._transitionEnterMaterial == (UnityEngine.Object) null)
    {
      Debug.LogWarning((object) "TransitionsFX not initialized yet. You're probably calling TransitionEnter/Exit from an Awake method. Please call it from a Start method instead.");
    }
    else
    {
      if (this._transitionCoroutine != null)
        this.StopCoroutine(this._transitionCoroutine);
      this._transitionCoroutine = this.StartCoroutine(this.TransitionRoutine(material, duration, delay, startValue, endValue, easeType));
    }
  }

  public IEnumerator TransitionRoutine(
    Material material,
    float duration,
    float delay,
    float startValue,
    float endValue,
    EaseType easeType)
  {
    ProCamera2DTransitionsFX camera2DtransitionsFx = this;
    camera2DtransitionsFx._blit.enabled = true;
    camera2DtransitionsFx._step = startValue;
    camera2DtransitionsFx._blit.CurrentMaterial = material;
    camera2DtransitionsFx._blit.CurrentMaterial.SetFloat(camera2DtransitionsFx._material_StepID, camera2DtransitionsFx._step);
    if ((double) endValue == 0.0)
    {
      if (camera2DtransitionsFx.OnTransitionEnterStarted != null)
        camera2DtransitionsFx.OnTransitionEnterStarted();
    }
    else if ((double) endValue == 1.0 && camera2DtransitionsFx.OnTransitionExitStarted != null)
      camera2DtransitionsFx.OnTransitionExitStarted();
    if (camera2DtransitionsFx.OnTransitionStarted != null)
      camera2DtransitionsFx.OnTransitionStarted();
    if ((double) delay > 0.0)
      yield return (object) new WaitForSeconds(delay);
    float t = 0.0f;
    while ((double) t <= 1.0)
    {
      t += camera2DtransitionsFx.ProCamera2D.DeltaTime / duration;
      camera2DtransitionsFx._step = Utils.EaseFromTo(startValue, endValue, t, easeType);
      material.SetFloat(camera2DtransitionsFx._material_StepID, camera2DtransitionsFx._step);
      yield return (object) null;
    }
    camera2DtransitionsFx._step = endValue;
    material.SetFloat(camera2DtransitionsFx._material_StepID, camera2DtransitionsFx._step);
    if ((double) endValue == 0.0)
    {
      if (camera2DtransitionsFx.OnTransitionEnterEnded != null)
        camera2DtransitionsFx.OnTransitionEnterEnded();
    }
    else if ((double) endValue == 1.0 && camera2DtransitionsFx.OnTransitionExitEnded != null)
      camera2DtransitionsFx.OnTransitionExitEnded();
    if (camera2DtransitionsFx.OnTransitionEnded != null)
      camera2DtransitionsFx.OnTransitionEnded();
    if ((double) endValue == 0.0)
      camera2DtransitionsFx._blit.enabled = false;
  }
}
