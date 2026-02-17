// Decompiled with JetBrains decompiler
// Type: SpineMirror
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using UnityEngine;

#nullable disable
public class SpineMirror : BaseMonoBehaviour
{
  public Shader shader;
  public Material material;
  public Color OverlayColor = new Color(0.1607843f, 0.2509804f, 0.2784314f, 0.8470588f);
  public SkeletonAnimation MySpine;
  public SkeletonAnimation MirrorSpine;
  public MeshRenderer meshRenderer;
  public MeshRenderer mirrorMeshRenderer;

  public void Init(SkeletonAnimation MirrorSpine) => this.MirrorSpine = MirrorSpine;

  public void Start()
  {
    this.MySpine.skeletonDataAsset = this.MirrorSpine.skeletonDataAsset;
    this.MySpine.Initialize(true);
    this.MySpine.skeleton.SetSkin(this.MirrorSpine.skeleton.Skin);
    this.MySpine.AnimationState.SetAnimation(0, this.MirrorSpine.AnimationName, this.MirrorSpine.AnimationState.GetCurrent(0).Loop);
    this.meshRenderer = this.GetComponent<MeshRenderer>();
    this.mirrorMeshRenderer = this.MirrorSpine.gameObject.GetComponent<MeshRenderer>();
    this.material = Object.Instantiate<Material>(this.mirrorMeshRenderer.material);
    this.material.shader = this.shader;
    this.MirrorSpine.AnimationState.Start += new Spine.AnimationState.TrackEntryDelegate(this.MirrorAnimation);
    Vector3 eulerAngles = this.MirrorSpine.transform.eulerAngles;
    eulerAngles.z *= -1f;
    this.transform.eulerAngles = eulerAngles;
  }

  public void OnWillRenderObject()
  {
  }

  public void MirrorAnimation(TrackEntry trackEntry)
  {
    this.MySpine.AnimationState.SetAnimation(0, this.MirrorSpine.AnimationName, this.MirrorSpine.AnimationState.GetCurrent(0).Loop);
  }

  public void LateUpdate()
  {
    if (this.meshRenderer.enabled != this.mirrorMeshRenderer.enabled)
      this.meshRenderer.enabled = this.mirrorMeshRenderer.enabled;
    if ((double) this.MySpine.skeleton.ScaleX == (double) this.MirrorSpine.skeleton.ScaleX)
      return;
    this.MySpine.skeleton.ScaleX = this.MirrorSpine.skeleton.ScaleX;
  }
}
