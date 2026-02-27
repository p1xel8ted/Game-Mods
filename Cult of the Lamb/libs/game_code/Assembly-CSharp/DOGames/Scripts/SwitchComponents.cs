// Decompiled with JetBrains decompiler
// Type: DOGames.Scripts.SwitchComponents
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
