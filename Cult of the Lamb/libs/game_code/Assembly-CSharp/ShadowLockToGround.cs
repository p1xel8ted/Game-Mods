// Decompiled with JetBrains decompiler
// Type: ShadowLockToGround
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
