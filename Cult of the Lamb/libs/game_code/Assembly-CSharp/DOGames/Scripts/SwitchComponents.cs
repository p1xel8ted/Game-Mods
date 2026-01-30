// Decompiled with JetBrains decompiler
// Type: DOGames.Scripts.SwitchComponents
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
