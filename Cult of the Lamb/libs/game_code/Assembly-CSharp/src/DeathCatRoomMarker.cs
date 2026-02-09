// Decompiled with JetBrains decompiler
// Type: src.DeathCatRoomMarker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace src;

public class DeathCatRoomMarker : MonoBehaviour
{
  [CompilerGenerated]
  public static bool \u003CIsDeathCatRoom\u003Ek__BackingField;

  public static bool IsDeathCatRoom
  {
    set => DeathCatRoomMarker.\u003CIsDeathCatRoom\u003Ek__BackingField = value;
    get => DeathCatRoomMarker.\u003CIsDeathCatRoom\u003Ek__BackingField;
  }

  public void OnEnable() => DeathCatRoomMarker.IsDeathCatRoom = true;

  public void OnDisable() => DeathCatRoomMarker.IsDeathCatRoom = false;
}
