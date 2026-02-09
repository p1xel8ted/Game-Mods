// Decompiled with JetBrains decompiler
// Type: Rotator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using UnityEngine;

#nullable disable
public class Rotator : MonoBehaviour
{
  public Vector3 rotationDegrees;
  public bool localSpace;
  public SkeletonAnimation parentSpine;

  public void SetParentSpine(SkeletonAnimation spine) => this.parentSpine = spine;

  public void Update()
  {
    this.transform.Rotate(this.rotationDegrees * Time.deltaTime * ((Object) this.parentSpine != (Object) null ? this.parentSpine.timeScale : 1f), this.localSpace ? Space.Self : Space.World);
  }
}
