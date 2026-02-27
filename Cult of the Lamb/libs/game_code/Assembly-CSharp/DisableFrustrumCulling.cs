// Decompiled with JetBrains decompiler
// Type: DisableFrustrumCulling
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DisableFrustrumCulling : BaseMonoBehaviour
{
  public bool DisableCulling;
  public Vector3 myMaxBoundsCenter;
  public Vector3 myMaxBoundsSize;
  public SkinnedMeshRenderer skinnedMeshRenderer;

  public void OnEnable()
  {
    if (!this.DisableCulling)
      return;
    this.skinnedMeshRenderer = this.gameObject.GetComponent<SkinnedMeshRenderer>();
    this.skinnedMeshRenderer.localBounds = new Bounds(this.myMaxBoundsCenter, this.myMaxBoundsSize);
  }
}
