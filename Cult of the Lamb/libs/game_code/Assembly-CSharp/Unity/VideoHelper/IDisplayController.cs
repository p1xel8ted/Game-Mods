// Decompiled with JetBrains decompiler
// Type: Unity.VideoHelper.IDisplayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Unity.VideoHelper;

public interface IDisplayController
{
  bool IsFullscreen { get; }

  void ToFullscreen(RectTransform target);

  void ToNormal();
}
