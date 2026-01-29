// Decompiled with JetBrains decompiler
// Type: Rotator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
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
