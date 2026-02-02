// Decompiled with JetBrains decompiler
// Type: ObjectTypes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
