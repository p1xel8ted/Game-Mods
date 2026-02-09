// Decompiled with JetBrains decompiler
// Type: SimpleTransformSpinner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SimpleTransformSpinner : MonoBehaviour
{
  public Transform targetTransform;
  public Vector3 rotateSpeed;

  public void Start()
  {
    if (!((Object) this.targetTransform == (Object) null))
      return;
    this.targetTransform = this.transform;
  }

  public void FixedUpdate()
  {
    this.targetTransform.transform.localRotation = Quaternion.EulerAngles(this.targetTransform.transform.localEulerAngles + this.rotateSpeed * Time.deltaTime);
  }
}
