// Decompiled with JetBrains decompiler
// Type: EnemyStealthUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
