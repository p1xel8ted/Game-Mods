// Decompiled with JetBrains decompiler
// Type: ShaderTimeScale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (MeshRenderer))]
public class ShaderTimeScale : BaseMonoBehaviour
{
  public float TimeStep = 1f;
  public float timer;
  public MeshRenderer rend;
  public MaterialPropertyBlock mpb;

  public void Awake()
  {
    this.rend = this.GetComponent<MeshRenderer>();
    this.mpb = new MaterialPropertyBlock();
  }

  public void Update()
  {
    this.timer += this.TimeStep * Time.deltaTime;
    this.rend.GetPropertyBlock(this.mpb);
    this.mpb.SetFloat("_shaderTime", this.timer);
    this.rend.SetPropertyBlock(this.mpb);
  }
}
