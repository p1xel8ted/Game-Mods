// Decompiled with JetBrains decompiler
// Type: TypeAndPlacementObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class TypeAndPlacementObject
{
  public StructureBrain.TYPES Type;
  [HideInInspector]
  public StructureBrain.Categories Category;
  public GameObject PlacementObject;
  public Sprite IconImage;
  [HideInInspector]
  public TypeAndPlacementObjects.Tier Tier;
}
