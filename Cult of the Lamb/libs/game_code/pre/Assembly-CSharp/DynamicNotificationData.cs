// Decompiled with JetBrains decompiler
// Type: DynamicNotificationData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

#nullable disable
public abstract class DynamicNotificationData
{
  public System.Action DataChanged;

  public abstract NotificationCentre.NotificationType Type { get; }

  public abstract bool IsEmpty { get; }

  public abstract bool HasProgress { get; }

  public abstract bool HasDynamicProgress { get; }

  public abstract float CurrentProgress { get; }

  public abstract float TotalCount { get; }

  public abstract string SkinName { get; }

  public abstract int SkinColor { get; }
}
