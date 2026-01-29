// Decompiled with JetBrains decompiler
// Type: ScreenSpaceOverlay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[ExecuteInEditMode]
[RequireComponent(typeof (Renderer))]
public class ScreenSpaceOverlay : BaseMonoBehaviour
{
  public static ScreenSpaceOverlay Instance;
  public static int blendAmountID = Shader.PropertyToID("_BlendAmount");
  public Renderer _ren;
  public Material dummyMat;
  public Material[] overlayMaterials = new Material[2];

  public void Init()
  {
    this._ren = this.GetComponent<Renderer>();
    this._ren.sharedMaterials = this.overlayMaterials;
    if ((UnityEngine.Object) this.dummyMat == (UnityEngine.Object) null)
      this.dummyMat = new Material(Shader.Find("Hidden/ScreenSpaceOverlayDummy"));
    this.SetMaterials((Material) null, (Material) null);
  }

  public void Awake()
  {
    this.Init();
    ScreenSpaceOverlay.Instance = this;
    this.SetWindSpeed(0.0f);
  }

  public void OnDestroy() => ScreenSpaceOverlay.Instance = (ScreenSpaceOverlay) null;

  public void OnEnable() => this.Init();

  public void SetMaterials(Material matA, Material matB)
  {
    if ((UnityEngine.Object) matA == (UnityEngine.Object) null)
      matA = this.dummyMat;
    if ((UnityEngine.Object) matB == (UnityEngine.Object) null)
      matB = this.dummyMat;
    this.overlayMaterials[0] = matA;
    this.overlayMaterials[1] = matB;
    this._ren.sharedMaterials = this.overlayMaterials;
  }

  public void SetWindSpeed(float windSpeed)
  {
  }

  public Renderer GetRenderer() => this._ren;

  public void TransitionMaterial(float blendAmount)
  {
    this._ren.sharedMaterials[0].SetFloat(ScreenSpaceOverlay.blendAmountID, blendAmount);
    this._ren.sharedMaterials[1].SetFloat(ScreenSpaceOverlay.blendAmountID, 1f - blendAmount);
  }

  public void Start() => this.OnWillRenderObject();

  public void OnWillRenderObject()
  {
    Camera main = Camera.main;
    if ((UnityEngine.Object) main == (UnityEngine.Object) null)
      return;
    float num1 = main.farClipPlane - 0.1f;
    Vector3 position = main.transform.position;
    Vector3 vector3_1 = main.transform.forward * num1;
    Vector3 vector3_2 = position + vector3_1;
    this.transform.position = vector3_2;
    Vector3 vector3_3 = (bool) (UnityEngine.Object) this.transform.parent ? this.transform.parent.localScale : Vector3.one;
    float num2 = main.orthographic ? main.orthographicSize * 2f : (float) ((double) Mathf.Tan((float) ((double) main.fieldOfView * (Math.PI / 180.0) * 0.5)) * (double) num1 * 2.0);
    this.transform.localScale = new Vector3(num2 * main.aspect / vector3_3.x, num2 / vector3_3.y, 0.0f);
    if (((UnityEngine.Object) Camera.current == (UnityEngine.Object) null ? 1 : ((UnityEngine.Object) Camera.current == (UnityEngine.Object) Camera.main ? 1 : 0)) != 0)
      this.transform.rotation = Quaternion.LookRotation(vector3_2 - position, main.transform.up);
    else
      this.transform.LookAt(position);
  }
}
