// Decompiled with JetBrains decompiler
// Type: EnemyStealthUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class EnemyStealthUI : BaseMonoBehaviour
{
  public SpriteRenderer RadialProgress;

  public void UpdateProgress(float Progress)
  {
    this.RadialProgress.material.SetFloat("_Arc2", (float) (360.0 - (double) Progress * 360.0));
  }
}
