// Decompiled with JetBrains decompiler
// Type: AmplifyColorVolume
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (BoxCollider))]
[AddComponentMenu("Image Effects/Amplify Color Volume")]
public class AmplifyColorVolume : AmplifyColorVolumeBase
{
  public void OnTriggerEnter(Collider other)
  {
    AmplifyColorTriggerProxy component = other.GetComponent<AmplifyColorTriggerProxy>();
    if (!((Object) component != (Object) null) || !component.OwnerEffect.UseVolumes || ((int) component.OwnerEffect.VolumeCollisionMask & 1 << this.gameObject.layer) == 0)
      return;
    component.OwnerEffect.EnterVolume((AmplifyColorVolumeBase) this);
  }

  public void OnTriggerExit(Collider other)
  {
    AmplifyColorTriggerProxy component = other.GetComponent<AmplifyColorTriggerProxy>();
    if (!((Object) component != (Object) null) || !component.OwnerEffect.UseVolumes || ((int) component.OwnerEffect.VolumeCollisionMask & 1 << this.gameObject.layer) == 0)
      return;
    component.OwnerEffect.ExitVolume((AmplifyColorVolumeBase) this);
  }
}
