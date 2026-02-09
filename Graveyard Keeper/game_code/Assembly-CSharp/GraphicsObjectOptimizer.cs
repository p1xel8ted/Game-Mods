// Decompiled with JetBrains decompiler
// Type: GraphicsObjectOptimizer
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class GraphicsObjectOptimizer
{
  public GameObject _go;

  public GraphicsObjectOptimizer(GameObject obj)
  {
    this._go = obj;
    foreach (PixelPerfect componentsInChild in obj.GetComponentsInChildren<PixelPerfect>(true))
    {
      if (componentsInChild.only_in_editor)
        componentsInChild.enabled = false;
    }
  }
}
