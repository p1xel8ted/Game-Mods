// Decompiled with JetBrains decompiler
// Type: DungeonWorldMapIconEventListener
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using UnityEngine;

#nullable disable
public class DungeonWorldMapIconEventListener : MonoBehaviour
{
  public virtual void Initialise(UIDLCMapMenuController mapController, DungeonWorldMapIcon icon)
  {
  }

  public virtual void OnVisible(DungeonWorldMapIcon icon)
  {
  }

  public virtual System.Threading.Tasks.Task BeforeStateChange(
    UIDLCMapMenuController mapController,
    DungeonWorldMapIcon node,
    DungeonWorldMapIcon.IconState previousState)
  {
    return System.Threading.Tasks.Task.CompletedTask;
  }

  public virtual System.Threading.Tasks.Task AfterStateChange(
    UIDLCMapMenuController mapController,
    DungeonWorldMapIcon node,
    DungeonWorldMapIcon.IconState previousState)
  {
    return System.Threading.Tasks.Task.CompletedTask;
  }
}
