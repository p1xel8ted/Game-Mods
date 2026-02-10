// Decompiled with JetBrains decompiler
// Type: Rewired.Integration.UnityUI.RewiredEventSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
namespace Rewired.Integration.UnityUI;

[AddComponentMenu("Rewired/Rewired Event System")]
public class RewiredEventSystem : EventSystem
{
  [Tooltip("If enabled, the Event System will be updated every frame even if other Event Systems are enabled. Otherwise, only EventSystem.current will be updated.")]
  [SerializeField]
  public bool _alwaysUpdate;

  public bool alwaysUpdate
  {
    get => this._alwaysUpdate;
    set => this._alwaysUpdate = value;
  }

  public override void Update()
  {
    if (this.alwaysUpdate)
    {
      EventSystem current = EventSystem.current;
      if ((Object) current != (Object) this)
        EventSystem.current = (EventSystem) this;
      try
      {
        base.Update();
      }
      finally
      {
        if ((Object) current != (Object) this)
          EventSystem.current = current;
      }
    }
    else
      base.Update();
  }
}
