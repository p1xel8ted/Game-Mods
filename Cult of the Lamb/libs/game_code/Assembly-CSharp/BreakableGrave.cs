// Decompiled with JetBrains decompiler
// Type: BreakableGrave
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BreakableGrave : MonoBehaviour
{
  [HideInInspector]
  public Health health;

  public void Awake() => this.health = this.GetComponentInChildren<Health>();
}
