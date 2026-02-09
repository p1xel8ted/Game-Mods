// Decompiled with JetBrains decompiler
// Type: TitleScreen
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TitleScreen : MonoBehaviour
{
  public TitleScreenCamera camera;
  public static TitleScreen me;

  public void Awake()
  {
    Debug.Log((object) "TitleScreen.Awake", (Object) this);
    TitleScreen.me = this;
  }

  public static void Show()
  {
    Debug.Log((object) "TitleScreen.Show", (Object) TitleScreen.me);
    if ((Object) TitleScreen.me != (Object) null)
      TitleScreen.me.gameObject.SetActive(true);
    RenderSettings.ambientLight = Color.white;
    RenderSettings.ambientIntensity = 1f;
  }

  public static void Hide()
  {
    Debug.Log((object) "TitleScreen.Hide", (Object) TitleScreen.me);
    if (!((Object) TitleScreen.me != (Object) null))
      return;
    TitleScreen.me.gameObject.SetActive(false);
  }
}
