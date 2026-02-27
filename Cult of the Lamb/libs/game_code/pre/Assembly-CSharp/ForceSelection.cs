// Decompiled with JetBrains decompiler
// Type: ForceSelection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using Unify.Input;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
public class ForceSelection : MonoBehaviour
{
  [SerializeField]
  [Tooltip("The default selected object")]
  private GameObject DefaultOption;
  [SerializeField]
  [Tooltip("The other possible selected object")]
  private GameObject OtherOption;
  private GameObject previouslySelected;
  [SerializeField]
  [Tooltip("Other Options that could be selected.")]
  private GameObject[] OtherOptions;

  private void OnEnable()
  {
    if ((Object) EventSystem.current.currentSelectedGameObject != (Object) null)
    {
      Debug.Log((object) ("previouslySelected = " + EventSystem.current.currentSelectedGameObject.name));
      this.previouslySelected = EventSystem.current.currentSelectedGameObject;
    }
    this.StartCoroutine((IEnumerator) this.DelaySelection());
  }

  private void Update()
  {
    if (RewiredInputManager.MainPlayer == null || !((Object) EventSystem.current.currentSelectedGameObject != (Object) this.DefaultOption) || !((Object) EventSystem.current.currentSelectedGameObject != (Object) this.OtherOption) || this.AnyOtherOptionSelected())
      return;
    EventSystem.current.SetSelectedGameObject(this.DefaultOption);
  }

  private void OnDisable()
  {
    if (!((Object) this.previouslySelected != (Object) null))
      return;
    Debug.Log((object) ("OnDisable SetSelected to " + this.previouslySelected.name));
    EventSystem.current.SetSelectedGameObject(this.previouslySelected);
  }

  private bool AnyOtherOptionSelected()
  {
    foreach (Object otherOption in this.OtherOptions)
    {
      if ((Object) EventSystem.current.currentSelectedGameObject == otherOption)
        return true;
    }
    return false;
  }

  private IEnumerator DelaySelection()
  {
    yield return (object) new WaitForSeconds(0.1f);
    EventSystem.current.SetSelectedGameObject((GameObject) null);
    EventSystem.current.SetSelectedGameObject(this.DefaultOption);
    Debug.Log((object) ("Delay Set Selected to " + this.DefaultOption.name));
  }
}
