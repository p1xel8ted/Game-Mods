// Decompiled with JetBrains decompiler
// Type: NotificationData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class NotificationData
{
  public string Notification = "";
  public float DeltaDisplay;
  public int FollowerID = -1;
  public NotificationBase.Flair Flair;
  public string[] ExtraText;

  public NotificationData(
    string Notification,
    float DeltaDisplay,
    int FollowerID,
    NotificationBase.Flair Flair,
    params string[] ExtraText)
  {
    this.Notification = Notification;
    this.DeltaDisplay = DeltaDisplay;
    this.FollowerID = FollowerID;
    this.Flair = Flair;
    this.ExtraText = ExtraText;
  }
}
