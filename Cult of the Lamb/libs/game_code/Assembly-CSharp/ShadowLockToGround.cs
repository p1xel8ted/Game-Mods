// Decompiled with JetBrains decompiler
// Type: ShadowLockToGround
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
