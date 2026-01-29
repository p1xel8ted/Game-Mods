// Decompiled with JetBrains decompiler
// Type: IDAndRelationship
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class IDAndRelationship
{
  [Key(0)]
  public int ID;
  [Key(1)]
  public int Relationship;
  [Key(2)]
  public IDAndRelationship.RelationshipState CurrentRelationshipState = IDAndRelationship.RelationshipState.Strangers;

  public enum RelationshipState
  {
    Enemies,
    Strangers,
    Friends,
    Lovers,
  }
}
