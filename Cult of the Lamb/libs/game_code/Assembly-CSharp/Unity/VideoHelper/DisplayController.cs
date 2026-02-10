// Decompiled with JetBrains decompiler
// Type: Unity.VideoHelper.DisplayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Unity.VideoHelper;

public static class DisplayController
{
  public static IDisplayController[] helper = (IDisplayController[]) new DisplayControllerInternal[Display.displays.Length];

  public static IDisplayController Default => DisplayController.ForDisplay(0);

  public static IDisplayController ForDisplay(int display)
  {
    if (DisplayController.helper[display] == null)
      DisplayController.helper[display] = (IDisplayController) new DisplayControllerInternal(display);
    return DisplayController.helper[display];
  }
}
