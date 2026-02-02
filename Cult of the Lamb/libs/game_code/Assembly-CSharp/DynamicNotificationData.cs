// Decompiled with JetBrains decompiler
// Type: DynamicNotificationData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
