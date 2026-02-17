// Decompiled with JetBrains decompiler
// Type: src.DeathCatRoomMarker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
