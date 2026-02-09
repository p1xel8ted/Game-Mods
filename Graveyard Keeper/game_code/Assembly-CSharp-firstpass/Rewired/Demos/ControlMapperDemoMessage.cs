// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.ControlMapperDemoMessage
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
namespace Rewired.Demos;

[AddComponentMenu("")]
public class ControlMapperDemoMessage : MonoBehaviour
{
  public Rewired.UI.ControlMapper.ControlMapper controlMapper;
  public Selectable defaultSelectable;

  public void Awake()
  {
    if (!((UnityEngine.Object) this.controlMapper != (UnityEngine.Object) null))
      return;
    this.controlMapper.ScreenClosedEvent += new Action(this.OnControlMapperClosed);
    this.controlMapper.ScreenOpenedEvent += new Action(this.OnControlMapperOpened);
  }

  public void Start() => this.SelectDefault();

  public void OnControlMapperClosed()
  {
    this.gameObject.SetActive(true);
    this.StartCoroutine(this.SelectDefaultDeferred());
  }

  public void OnControlMapperOpened() => this.gameObject.SetActive(false);

  public void SelectDefault()
  {
    if ((UnityEngine.Object) EventSystem.current == (UnityEngine.Object) null || !((UnityEngine.Object) this.defaultSelectable != (UnityEngine.Object) null))
      return;
    EventSystem.current.SetSelectedGameObject(this.defaultSelectable.gameObject);
  }

  public IEnumerator SelectDefaultDeferred()
  {
    yield return (object) null;
    this.SelectDefault();
  }
}
