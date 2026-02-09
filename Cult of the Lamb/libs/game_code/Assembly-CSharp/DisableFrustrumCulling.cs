// Decompiled with JetBrains decompiler
// Type: DisableFrustrumCulling
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
