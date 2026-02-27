// Decompiled with JetBrains decompiler
// Type: FX_CrackController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  [Range(0.0f, 1f)]
  public float Length;
  private float hLength;
  [Range(0.0f, 1f)]
  public float Width = 0.3f;
  [Range(0.0f, 1f)]
  public float Falloff = 0.2f;
  private float timer;
  public bool playOnEnable = true;
  public bool destroyOnFinish = true;
  public bool animate = true;
  public static readonly int OffsetID = Shader.PropertyToID("_Offset");
  public static readonly int WidthID = Shader.PropertyToID("_Width");
  public static readonly int LengthID = Shader.PropertyToID("_Length");
  public static readonly int FalloffID = Shader.PropertyToID("_Falloff");
  private MaterialPropertyBlock m_propBlock;

  public MaterialPropertyBlock PropBlock
  {
    get
    {
      if (this.m_propBlock == null)
        this.m_propBlock = new MaterialPropertyBlock();
      return this.m_propBlock;
    }
  }

  private void OnEnable() => this.timer = 0.0f;

  private void Update()
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

  private void OnDisable()
  {
    if (!this.destroyOnFinish)
      return;
    Object.Destroy((Object) this.gameObject);
  }

  private void OnValidate() => this.ApplyProperties();

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
