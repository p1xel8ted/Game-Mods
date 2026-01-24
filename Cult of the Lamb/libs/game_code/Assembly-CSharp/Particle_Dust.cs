// Decompiled with JetBrains decompiler
// Type: Particle_Dust
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Particle_Dust : BaseMonoBehaviour
{
  public void Update()
  {
    if ((double) ((Vector2) Camera.main.WorldToScreenPoint(this.transform.position)).x >= 0.0)
      return;
    this.transform.position = this.transform.position + Vector3.right * 10f;
  }
}
