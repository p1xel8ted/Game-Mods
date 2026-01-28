// Decompiled with JetBrains decompiler
// Type: DOGames.Scripts.SwitchComponents
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace DOGames.Scripts;

public class SwitchComponents : MonoBehaviour
{
  public List<Component> ComponentsToDisable;

  public void Start()
  {
    foreach (Object @object in this.ComponentsToDisable)
      Object.DestroyImmediate(@object);
  }
}
