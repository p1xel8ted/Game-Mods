// Decompiled with JetBrains decompiler
// Type: FX_CrackController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FX_CrackController : BaseMonoBehaviour
{
  public float Offset;
  public AnimationCurve OffsetOverDuration = new AnimationCurve(new Keyframe[2]
  {
    new Keyframe(0.0f, 0.0f),
    new Keyframe(1f, 1f)
  });
  public float duration = 2f;
  public AnimationCurve WidthOverLength = new AnimationCurve(new Keyframe[3]
  {
    new Keyframe(0.0f, 0.0f),
    new Keyframe(0.5f, 1f),
    new Keyframe(1f, 0.0f)
  });
  public float Length;
  public float hLength;
  public float Width = 0.3f;
  public float Falloff = 0.2f;
  public float timer;
  public bool playOnEnable = true;
  public bool destroyOnFinish = true;
  public bool animate = true;
  public static int OffsetID = Shader.PropertyToID("_Offset");
  public static int WidthID = Shader.PropertyToID("_Width");
  public static int LengthID = Shader.PropertyToID("_Length");
  public static int FalloffID = Shader.PropertyToID("_Falloff");
  public MaterialPropertyBlock m_propBlock;

  public MaterialPropertyBlock PropBlock
  {
    get
    {
      if (this.m_propBlock == null)
        this.m_propBlock = new MaterialPropertyBlock();
      return this.m_propBlock;
    }
  }

  public void Awake() => this.ApplyProperties();

  public void OnEnable()
  {
    this.timer = 0.0f;
    this.ApplyProperties();
  }

  public void Update()
  {
    if (!this.isActiveAndEnabled || !this.playOnEnable)
      return;
    this.timer += Time.deltaTime;
    if ((double) this.timer > (double) this.duration)
    {
      if (this.destroyOnFinish)
        Object.Destroy((Object) this.gameObject);
      else
        this.timer = 0.0f;
    }
    if (this.animate)
      this.Offset = this.OffsetOverDuration.Evaluate(Mathf.Clamp01(this.timer / this.duration));
    this.ApplyProperties();
  }

  public void OnDisable()
  {
    if (!this.destroyOnFinish)
      return;
    Object.Destroy((Object) this.gameObject);
  }

  public void ApplyProperties()
  {
    foreach (Renderer componentsInChild in this.GetComponentsInChildren<Renderer>())
    {
      this.PropBlock.SetFloat(FX_CrackController.OffsetID, this.Offset);
      this.PropBlock.SetFloat(FX_CrackController.WidthID, this.Width);
      this.PropBlock.SetFloat(FX_CrackController.LengthID, this.Length);
      this.PropBlock.SetFloat(FX_CrackController.FalloffID, this.Falloff);
      MaterialPropertyBlock propBlock = this.PropBlock;
      componentsInChild.SetPropertyBlock(propBlock);
      this.PropBlock.Clear();
    }
  }
}
