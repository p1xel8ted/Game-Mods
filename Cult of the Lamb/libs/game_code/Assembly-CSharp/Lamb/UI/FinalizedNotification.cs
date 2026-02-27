// Decompiled with JetBrains decompiler
// Type: Lamb.UI.FinalizedNotification
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;

#nullable disable
namespace Lamb.UI;

[MessagePackObject(false)]
[Union(0, typeof (FinalizedFaithNotification))]
[Union(1, typeof (FinalizedItemNotification))]
[Union(2, typeof (FinalizedFollowerNotification))]
[Union(3, typeof (FinalizedRelationshipNotification))]
[Union(4, typeof (FinalizedNotificationSimple))]
[Serializable]
public abstract class FinalizedNotification
{
  [Key(0)]
  public string LocKey = "";
  [Key(1)]
  public string[] LocalisedParameters = new string[0];
  [Key(2)]
  public string[] NonLocalisedParameters = new string[0];
}
