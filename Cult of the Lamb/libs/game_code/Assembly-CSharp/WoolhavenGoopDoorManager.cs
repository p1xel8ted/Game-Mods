// Decompiled with JetBrains decompiler
// Type: WoolhavenGoopDoorManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class WoolhavenGoopDoorManager : BaseMonoBehaviour
{
  [SerializeField]
  public GameObject graveParent;
  public List<Interaction_Generic> graves = new List<Interaction_Generic>();
  public BaseGoopDoor _goopDoor;

  public void OnEnable()
  {
    this._goopDoor = this.GetComponent<BaseGoopDoor>();
    this.graves = ((IEnumerable<Interaction_Generic>) this.graveParent.GetComponentsInChildren<Interaction_Generic>()).ToList<Interaction_Generic>();
    this.graves.ForEach((Action<Interaction_Generic>) (grave => grave.OnInteraction += new Interaction.InteractionEvent(this.OnGraveInteraction)));
    if (!((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null))
      return;
    GameManager.GetInstance().WaitForSeconds(0.05f, (System.Action) (() =>
    {
      if (!this.IsAnyGraveInteractable())
        return;
      BaseGoopDoor.BlockGoopDoor("GoopDoor/Woolhaven/RescueNPCBlockedDoorMessage");
      BaseGoopDoor.DoorUp();
    }));
  }

  public void OnDisable()
  {
    this.graves.ForEach((Action<Interaction_Generic>) (grave => grave.OnInteraction -= new Interaction.InteractionEvent(this.OnGraveInteraction)));
  }

  public bool IsAnyGraveInteractable()
  {
    return this.graves.Any<Interaction_Generic>((Func<Interaction_Generic, bool>) (grave => grave.Interactable));
  }

  public void OnGraveInteraction(StateMachine state)
  {
    BaseGoopDoor.WoolhavenDoor.CheckWoolhavenDoor();
  }

  [CompilerGenerated]
  public void \u003COnEnable\u003Eb__3_0(Interaction_Generic grave)
  {
    grave.OnInteraction += new Interaction.InteractionEvent(this.OnGraveInteraction);
  }

  [CompilerGenerated]
  public void \u003COnEnable\u003Eb__3_1()
  {
    if (!this.IsAnyGraveInteractable())
      return;
    BaseGoopDoor.BlockGoopDoor("GoopDoor/Woolhaven/RescueNPCBlockedDoorMessage");
    BaseGoopDoor.DoorUp();
  }

  [CompilerGenerated]
  public void \u003COnDisable\u003Eb__4_0(Interaction_Generic grave)
  {
    grave.OnInteraction -= new Interaction.InteractionEvent(this.OnGraveInteraction);
  }
}
