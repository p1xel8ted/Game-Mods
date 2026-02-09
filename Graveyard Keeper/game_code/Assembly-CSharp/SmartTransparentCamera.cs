// Decompiled with JetBrains decompiler
// Type: SmartTransparentCamera
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class SmartTransparentCamera : MonoBehaviour
{
  public Transform character;
  public float z_shift = -0.7f;

  public void Update()
  {
    if ((Object) this.character == (Object) null)
      return;
    this.transform.position = this.transform.position with
    {
      z = this.character.position.z + this.z_shift
    };
  }
}
