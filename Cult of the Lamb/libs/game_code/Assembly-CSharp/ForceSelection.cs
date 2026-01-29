// Decompiled with JetBrains decompiler
// Type: ForceSelection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using Unify.Input;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
public class ForceSelection : MonoBehaviour
{
  [SerializeField]
  [Tooltip("The default selected object")]
  public GameObject DefaultOption;
  [SerializeField]
  [Tooltip("The other possible selected object")]
  public GameObject OtherOption;
  public GameObject previouslySelected;
  [SerializeField]
  [Tooltip("Other Options that could be selected.")]
  public GameObject[] OtherOptions;

  public void OnEnable()
  {
    if ((Object) EventSystem.current.currentSelectedGameObject != (Object) null)
    {
      Debug.Log((object) ("previouslySelected = " + EventSystem.current.currentSelectedGameObject.name));
      this.previouslySelected = EventSystem.current.currentSelectedGameObject;
    }
    this.StartCoroutine((IEnumerator) this.DelaySelection());
  }

  public void Update()
  {
    if (RewiredInputManager.MainPlayer == null || !((Object) EventSystem.current.currentSelectedGameObject != (Object) this.DefaultOption) || !((Object) EventSystem.current.currentSelectedGameObject != (Object) this.OtherOption) || this.AnyOtherOptionSelected())
      return;
    EventSystem.current.SetSelectedGameObject(this.DefaultOption);
  }

  public void OnDisable()
  {
    if (!((Object) this.previouslySelected != (Object) null))
      return;
    Debug.Log((object) ("OnDisable SetSelected to " + this.previouslySelected.name));
    EventSystem.current.SetSelectedGameObject(this.previouslySelected);
  }

  public bool AnyOtherOptionSelected()
  {
    foreach (Object otherOption in this.OtherOptions)
    {
      if ((Object) EventSystem.current.currentSelectedGameObject == otherOption)
        return true;
    }
    return false;
  }

  public IEnumerator DelaySelection()
  {
    yield return (object) new WaitForSeconds(0.1f);
    EventSystem.current.SetSelectedGameObject((GameObject) null);
    EventSystem.current.SetSelectedGameObject(this.DefaultOption);
    Debug.Log((object) ("Delay Set Selected to " + this.DefaultOption.name));
  }
}
