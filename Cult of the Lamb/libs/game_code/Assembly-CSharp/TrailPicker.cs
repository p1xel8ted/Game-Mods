// Decompiled with JetBrains decompiler
// Type: TrailPicker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
