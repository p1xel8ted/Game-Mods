// Decompiled with JetBrains decompiler
// Type: ShaderTimeScale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (MeshRenderer))]
public class ShaderTimeScale : BaseMonoBehaviour
{
  public float TimeStep = 1f;
  private float timer;

  private MeshRenderer _rend => this.GetComponent<MeshRenderer>();

  private void Update()
  {
    this.timer += this.TimeStep * Time.deltaTime;
    foreach (Material material in this._rend.materials)
      material.SetFloat("_shaderTime", this.timer);
  }
}
