// Decompiled with JetBrains decompiler
// Type: ErrorMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
