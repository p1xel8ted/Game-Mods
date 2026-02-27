// Decompiled with JetBrains decompiler
// Type: DisableFrustrumCulling
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DisableFrustrumCulling : BaseMonoBehaviour
{
  public bool DisableCulling;
  public Vector3 myMaxBoundsCenter;
  public Vector3 myMaxBoundsSize;
  private SkinnedMeshRenderer skinnedMeshRenderer;

  private void OnEnable()
  {
    if (!this.DisableCulling)
      return;
    this.skinnedMeshRenderer = this.gameObject.GetComponent<SkinnedMeshRenderer>();
    this.skinnedMeshRenderer.localBounds = new Bounds(this.myMaxBoundsCenter, this.myMaxBoundsSize);
  }
}
