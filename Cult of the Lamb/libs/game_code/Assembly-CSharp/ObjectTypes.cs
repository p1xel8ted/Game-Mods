// Decompiled with JetBrains decompiler
// Type: ObjectTypes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

#nullable disable
[Serializable]
public class ObjectTypes
{
  public AssetReferenceGameObject[] ObjectsAddr;
  public List<FollowerLocation> Location = new List<FollowerLocation>();
  public Vector2 PercentageChanceToSpawn;
}
