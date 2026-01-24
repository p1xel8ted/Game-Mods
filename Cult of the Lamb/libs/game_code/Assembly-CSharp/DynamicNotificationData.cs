// Decompiled with JetBrains decompiler
// Type: DynamicNotificationData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
