// Decompiled with JetBrains decompiler
// Type: GUIElementsVisibilityFixer
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[DefaultExecutionOrder(-200)]
public class GUIElementsVisibilityFixer : MonoBehaviour
{
  public List<GameObject> dont_disable;
  public List<GameObject> force_disable = new List<GameObject>();

  public void Awake()
  {
    foreach (BaseGUI componentsInChild in this.GetComponentsInChildren<BaseGUI>())
    {
      if (!this.dont_disable.Contains(componentsInChild.gameObject))
        componentsInChild.gameObject.SetActive(false);
    }
    foreach (GameObject gameObject in this.force_disable)
      gameObject.SetActive(false);
    GUIElements.me.speech_bubble.gameObject.SetActive(false);
  }
}
