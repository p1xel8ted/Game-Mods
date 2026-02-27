// Decompiled with JetBrains decompiler
// Type: NotificationDynamicCursed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
