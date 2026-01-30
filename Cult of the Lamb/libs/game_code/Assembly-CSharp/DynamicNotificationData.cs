// Decompiled with JetBrains decompiler
// Type: DynamicNotificationData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
