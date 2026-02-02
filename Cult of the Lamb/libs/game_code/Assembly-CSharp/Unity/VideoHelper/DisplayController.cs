// Decompiled with JetBrains decompiler
// Type: Unity.VideoHelper.DisplayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
