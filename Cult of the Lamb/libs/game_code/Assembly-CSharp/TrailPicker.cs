// Decompiled with JetBrains decompiler
// Type: TrailPicker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Ara;
using UnityEngine;

#nullable disable
public class TrailPicker : MonoBehaviour
{
  [SerializeField]
  public AraTrail araTrail;
  [SerializeField]
  public TrailRenderer lowQualityTrail;

  public void Awake()
  {
    if (!((Object) this.lowQualityTrail != (Object) null))
      return;
    this.araTrail.enabled = true;
    this.lowQualityTrail.enabled = false;
  }

  public void ClearTrails() => this.araTrail.Clear();
}
