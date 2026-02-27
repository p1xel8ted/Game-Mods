// Decompiled with JetBrains decompiler
// Type: NotificationData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
