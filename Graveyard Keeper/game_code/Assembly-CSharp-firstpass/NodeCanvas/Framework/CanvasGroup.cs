// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.CanvasGroup
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Framework;

[Serializable]
public class CanvasGroup
{
  public string name;
  public Rect rect;
  public Color color;

  public CanvasGroup()
  {
  }

  public CanvasGroup(Rect rect, string name)
  {
    this.rect = rect;
    this.name = name;
  }
}
