// Decompiled with JetBrains decompiler
// Type: LookAtAxis
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class LookAtAxis : MonoBehaviour
{
  public Transform target;
  public Vector3 rotationAxis;
  public Vector3 rotationOffset;

  public void Update()
  {
    if (!((Object) this.target != (Object) null))
      return;
    float num = Vector2.SignedAngle(Vector2.right, (Vector2) (this.target.position - this.transform.position));
    this.transform.localRotation = Quaternion.AngleAxis((double) num < 0.0 ? 360f + num : num, this.rotationAxis) * Quaternion.Euler(this.rotationOffset);
  }
}
