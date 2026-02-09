// Decompiled with JetBrains decompiler
// Type: MyFlowCanvasExtensions
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using FlowCanvas;
using UnityEngine;

#nullable disable
public static class MyFlowCanvasExtensions
{
  public static bool HasValue<T>(this ValueInput<T> v)
  {
    return (UnityEngine.Object) ((object) v.value as GameObject) != (UnityEngine.Object) null || System.Type.op_Equality(v.type, typeof (GameObject)) || System.Type.op_Equality(v.type, typeof (WorldGameObject)) ? v.isConnected && (object) v.value != null : v.isConnected || !v.isDefaultValue;
  }
}
