// Decompiled with JetBrains decompiler
// Type: ShadowLockToGround
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ShadowLockToGround : BaseMonoBehaviour
{
  public Vector3 Position;
  public Transform LockXToObject;

  public void LateUpdate()
  {
    this.Position = this.transform.position;
    this.Position.z = 0.0f;
    if ((Object) this.LockXToObject != (Object) null)
      this.Position.x = this.LockXToObject.position.x;
    this.transform.position = this.Position;
  }
}
