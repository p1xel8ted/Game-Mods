// Decompiled with JetBrains decompiler
// Type: ShaderTimeScale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (MeshRenderer))]
public class ShaderTimeScale : BaseMonoBehaviour
{
  public float TimeStep = 1f;
  public float timer;

  public MeshRenderer _rend => this.GetComponent<MeshRenderer>();

  public void Update()
  {
    this.timer += this.TimeStep * Time.deltaTime;
    foreach (Material material in this._rend.materials)
      material.SetFloat("_shaderTime", this.timer);
  }
}
