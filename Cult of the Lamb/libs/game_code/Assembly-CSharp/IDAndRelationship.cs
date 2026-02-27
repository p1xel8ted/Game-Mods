// Decompiled with JetBrains decompiler
// Type: IDAndRelationship
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
