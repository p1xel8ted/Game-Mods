// Decompiled with JetBrains decompiler
// Type: MimicAnimationController
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public static class MimicAnimationController
{
  public static System.Action _event = (System.Action) null;
  public static long _last_id = 0;
  public static Dictionary<long, System.Action> _listeners_in_progress = new Dictionary<long, System.Action>();
  public static List<long> _dont_timer = new List<long>();

  public static void Init()
  {
    MimicAnimationController._event = (System.Action) null;
    MimicAnimationController._listeners_in_progress.Clear();
    MimicAnimationController._dont_timer.Clear();
  }
}
