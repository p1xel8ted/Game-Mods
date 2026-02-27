// Decompiled with JetBrains decompiler
// Type: LookAtAxis
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
