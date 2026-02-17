// Decompiled with JetBrains decompiler
// Type: NotificationDynamicCursed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class NotificationDynamicCursed : NotificationDynamicGeneric
{
  public Gradient ColourGradient;

  public override Color GetColor(float norm)
  {
    if (float.IsNaN(norm))
      norm = 0.0f;
    return this.ColourGradient.Evaluate(Mathf.Clamp01(norm));
  }
}
