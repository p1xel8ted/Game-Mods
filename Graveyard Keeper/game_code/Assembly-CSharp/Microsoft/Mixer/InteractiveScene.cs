// Decompiled with JetBrains decompiler
// Type: Microsoft.Mixer.InteractiveScene
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
namespace Microsoft.Mixer;

[Serializable]
public class InteractiveScene
{
  [CompilerGenerated]
  public string \u003CSceneID\u003Ek__BackingField;
  public string _etag;

  public string SceneID
  {
    get => this.\u003CSceneID\u003Ek__BackingField;
    set => this.\u003CSceneID\u003Ek__BackingField = value;
  }

  public IList<InteractiveButtonControl> Buttons
  {
    get
    {
      List<InteractiveButtonControl> buttons = new List<InteractiveButtonControl>();
      foreach (InteractiveButtonControl button in InteractivityManager.SingletonInstance.Buttons as List<InteractiveButtonControl>)
      {
        if (button._sceneID == this.SceneID)
          buttons.Add(button);
      }
      return (IList<InteractiveButtonControl>) buttons;
    }
  }

  public IList<InteractiveJoystickControl> Joysticks
  {
    get
    {
      List<InteractiveJoystickControl> joysticks = new List<InteractiveJoystickControl>();
      foreach (InteractiveJoystickControl joystick in InteractivityManager.SingletonInstance.Joysticks as List<InteractiveJoystickControl>)
      {
        if (joystick._sceneID == this.SceneID)
          joysticks.Add(joystick);
      }
      return (IList<InteractiveJoystickControl>) joysticks;
    }
  }

  public IList<InteractiveGroup> Groups
  {
    get
    {
      List<InteractiveGroup> groups = new List<InteractiveGroup>();
      foreach (InteractiveGroup group in InteractivityManager.SingletonInstance.Groups as List<InteractiveGroup>)
      {
        if (group.SceneID == this.SceneID)
          groups.Add(group);
      }
      return (IList<InteractiveGroup>) groups;
    }
  }

  public InteractiveButtonControl GetButton(string controlID)
  {
    return InteractivityManager.SingletonInstance.GetButton(controlID);
  }

  public InteractiveJoystickControl GetJoystick(string controlID)
  {
    return InteractivityManager.SingletonInstance.GetJoystick(controlID);
  }

  public InteractiveScene(string sceneID = "", string newEtag = "")
  {
    this.SceneID = sceneID;
    this._etag = newEtag;
  }
}
