// Decompiled with JetBrains decompiler
// Type: SmartController
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SmartController : MonoBehaviour
{
  public float min;
  public float max = 1f;
  public float value = 1f;
  public List<SmartControllerParameter> parameters = new List<SmartControllerParameter>();

  public void Update()
  {
    for (int index = 0; index < this.parameters.Count; ++index)
      this.parameters[index].Evaluate(this.value);
  }

  public SmartControllerParameter FindParameterOfType(SmartControllerParameter.Action action)
  {
    foreach (SmartControllerParameter parameter in this.parameters)
    {
      if (parameter.action == action)
        return parameter;
    }
    Debug.LogError((object) ("Couldn't find parameter with action = " + action.ToString()));
    return (SmartControllerParameter) null;
  }
}
