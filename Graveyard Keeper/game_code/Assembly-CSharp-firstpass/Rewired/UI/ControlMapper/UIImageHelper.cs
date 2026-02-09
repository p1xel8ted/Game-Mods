// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.UIImageHelper
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Rewired.UI.ControlMapper;

[AddComponentMenu("")]
[RequireComponent(typeof (Image))]
public class UIImageHelper : MonoBehaviour
{
  [SerializeField]
  public UIImageHelper.State enabledState;
  [SerializeField]
  public UIImageHelper.State disabledState;
  public bool currentState;

  public void SetEnabledState(bool newState)
  {
    this.currentState = newState;
    UIImageHelper.State state = newState ? this.enabledState : this.disabledState;
    if (state == null)
      return;
    Image component = this.gameObject.GetComponent<Image>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      Debug.LogError((object) "Image is missing!");
    else
      state.Set(component);
  }

  public void SetEnabledStateColor(Color color) => this.enabledState.color = color;

  public void SetDisabledStateColor(Color color) => this.disabledState.color = color;

  public void Refresh()
  {
    UIImageHelper.State state = this.currentState ? this.enabledState : this.disabledState;
    Image component = this.gameObject.GetComponent<Image>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    state.Set(component);
  }

  [Serializable]
  public class State
  {
    [SerializeField]
    public Color color;

    public void Set(Image image)
    {
      if ((UnityEngine.Object) image == (UnityEngine.Object) null)
        return;
      image.color = this.color;
    }
  }
}
