// Decompiled with JetBrains decompiler
// Type: WeedSpritePacker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class WeedSpritePacker : MonoBehaviour
{
  [Header("Assign your leaf sprite renderers here")]
  public List<SpriteRenderer> leafRenderers;
  [Header("This will receive the packed sprite")]
  public SpriteRenderer targetRenderer;
  public int margin = 1000;
  public static bool IsOptimized = true;

  public static void ToggleOptimization()
  {
    foreach (WeedSpritePacker weedSpritePacker in Object.FindObjectsOfType<WeedSpritePacker>())
    {
      foreach (Component leafRenderer in weedSpritePacker.leafRenderers)
        leafRenderer.gameObject.SetActive(WeedSpritePacker.IsOptimized);
      weedSpritePacker.targetRenderer.gameObject.SetActive(!WeedSpritePacker.IsOptimized);
    }
    WeedSpritePacker.IsOptimized = !WeedSpritePacker.IsOptimized;
  }
}
