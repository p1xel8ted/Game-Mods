// Decompiled with JetBrains decompiler
// Type: UIToggledComponents
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Interaction/Toggled Components")]
[RequireComponent(typeof (UIToggle))]
[ExecuteInEditMode]
public class UIToggledComponents : MonoBehaviour
{
  public List<MonoBehaviour> activate;
  public List<MonoBehaviour> deactivate;
  [HideInInspector]
  [SerializeField]
  public MonoBehaviour target;
  [HideInInspector]
  [SerializeField]
  public bool inverse;

  public void Awake()
  {
    if ((Object) this.target != (Object) null)
    {
      if (this.activate.Count == 0 && this.deactivate.Count == 0)
      {
        if (this.inverse)
          this.deactivate.Add(this.target);
        else
          this.activate.Add(this.target);
      }
      else
        this.target = (MonoBehaviour) null;
    }
    EventDelegate.Add(this.GetComponent<UIToggle>().onChange, new EventDelegate.Callback(this.Toggle));
  }

  public void Toggle()
  {
    if (!this.enabled)
      return;
    for (int index = 0; index < this.activate.Count; ++index)
      this.activate[index].enabled = UIToggle.current.value;
    for (int index = 0; index < this.deactivate.Count; ++index)
      this.deactivate[index].enabled = !UIToggle.current.value;
  }
}
