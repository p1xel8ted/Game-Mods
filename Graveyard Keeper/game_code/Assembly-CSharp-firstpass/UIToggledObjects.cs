// Decompiled with JetBrains decompiler
// Type: UIToggledObjects
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Interaction/Toggled Objects")]
public class UIToggledObjects : MonoBehaviour
{
  public List<GameObject> activate;
  public List<GameObject> deactivate;
  [SerializeField]
  [HideInInspector]
  public GameObject target;
  [SerializeField]
  [HideInInspector]
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
        this.target = (GameObject) null;
    }
    EventDelegate.Add(this.GetComponent<UIToggle>().onChange, new EventDelegate.Callback(this.Toggle));
  }

  public void Toggle()
  {
    bool state = UIToggle.current.value;
    if (!this.enabled)
      return;
    for (int index = 0; index < this.activate.Count; ++index)
      this.Set(this.activate[index], state);
    for (int index = 0; index < this.deactivate.Count; ++index)
      this.Set(this.deactivate[index], !state);
  }

  public void Set(GameObject go, bool state)
  {
    if (!((Object) go != (Object) null))
      return;
    NGUITools.SetActive(go, state);
  }
}
