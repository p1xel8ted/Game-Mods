// Decompiled with JetBrains decompiler
// Type: ResolutionHelper
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ResolutionHelper : MonoBehaviour
{
  public static int _w;
  public static int _h;

  public void Update()
  {
    if (ResolutionHelper._w == Screen.width && ResolutionHelper._h == Screen.height)
      return;
    Debug.Log((object) $"Resolution change detected [{ResolutionHelper._w}x{ResolutionHelper._h}] ==> [{Screen.width}x{Screen.height}]");
    this.Awake();
  }

  public void Awake()
  {
    ResolutionHelper._w = Screen.width;
    ResolutionHelper._h = Screen.height;
  }

  public void OnApplicationFocus(bool focus)
  {
    Debug.Log((object) $"OnApplicationFocus {focus}");
    if (!(GameSettings.me.screen_mode == 1 & focus))
      return;
    GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() =>
    {
      Debug.Log((object) $"Checking resolution on re-focus. Current: {Screen.width}x{Screen.height}");
      if (Screen.width == GameSettings.current_resolution.x && Screen.height == GameSettings.current_resolution.y)
        return;
      Debug.Log((object) "Restoring resolution...");
      GameSettings.me.ApplyScreenMode();
    }));
  }

  public static void OnResolutionChanged(int w, int h)
  {
    Debug.Log((object) $"OnResolutionChanged {w}x{h}");
    ResolutionHelper._w = w;
    ResolutionHelper._h = h;
  }
}
