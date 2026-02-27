// Decompiled with JetBrains decompiler
// Type: Lamb.UI.FinalizedNotification
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Xml.Serialization;

#nullable disable
namespace Lamb.UI;

[XmlType(Namespace = "FinalizedNotifications")]
[Serializable]
public class FinalizedNotification
{
  public string LocKey = "";
  public string[] LocalisedParameters = new string[0];
  public string[] NonLocalisedParameters = new string[0];
}
