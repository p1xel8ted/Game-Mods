// Decompiled with JetBrains decompiler
// Type: ErrorMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using src.UINavigator;
using UnityEngine;

#nullable disable
public class ErrorMessage : MonoBehaviour
{
  private float TimeHolder;
  private bool InMenuHolder;

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
