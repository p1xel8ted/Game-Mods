// Decompiled with JetBrains decompiler
// Type: LockScaleToWorld
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class LockScaleToWorld : MonoBehaviour
{
  public void Update()
  {
    this.transform.localScale = new Vector3(this.transform.parent.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
  }
}
