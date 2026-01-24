// Decompiled with JetBrains decompiler
// Type: ErrorMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src.UINavigator;
using UnityEngine;

#nullable disable
public class ErrorMessage : MonoBehaviour
{
  public float TimeHolder;
  public bool InMenuHolder;

  public void OnEnable()
  {
    this.TimeHolder = Time.timeScale;
    Time.timeScale = 0.0f;
    this.InMenuHolder = GameManager.InMenu;
    GameManager.InMenu = true;
    MonoSingleton<UINavigatorNew>.Instance.LockNavigation = true;
    MonoSingleton<UINavigatorNew>.Instance.LockInput = true;
  }

  public void OnDisable()
  {
    Time.timeScale = this.TimeHolder;
    GameManager.InMenu = this.InMenuHolder;
    MonoSingleton<UINavigatorNew>.Instance.LockNavigation = false;
    MonoSingleton<UINavigatorNew>.Instance.LockInput = false;
  }
}
