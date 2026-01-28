// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.UIImageHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
