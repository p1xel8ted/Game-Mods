// Decompiled with JetBrains decompiler
// Type: ShadowLockToGround
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ShadowLockToGround : BaseMonoBehaviour
{
  private Vector3 Position;
  public Transform LockXToObject;

  private void LateUpdate()
  {
    this.Position = this.transform.position;
    this.Position.z = 0.0f;
    if ((Object) this.LockXToObject != (Object) null)
      this.Position.x = this.LockXToObject.position.x;
    this.transform.position = this.Position;
  }
}
