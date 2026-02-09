// Decompiled with JetBrains decompiler
// Type: AmplifyColor.VolumeEffectContainer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using System;
using System.Collections.Generic;

#nullable disable
namespace AmplifyColor;

[Serializable]
public class VolumeEffectContainer
{
  public List<VolumeEffect> volumes;

  public VolumeEffectContainer() => this.volumes = new List<VolumeEffect>();

  public void AddColorEffect(AmplifyColorBase colorEffect)
  {
    VolumeEffect volumeEffect1;
    if ((volumeEffect1 = this.FindVolumeEffect(colorEffect)) != null)
    {
      volumeEffect1.UpdateVolume();
    }
    else
    {
      VolumeEffect volumeEffect2 = new VolumeEffect(colorEffect);
      this.volumes.Add(volumeEffect2);
      volumeEffect2.UpdateVolume();
    }
  }

  public VolumeEffect AddJustColorEffect(AmplifyColorBase colorEffect)
  {
    VolumeEffect volumeEffect = new VolumeEffect(colorEffect);
    this.volumes.Add(volumeEffect);
    return volumeEffect;
  }

  public VolumeEffect FindVolumeEffect(AmplifyColorBase colorEffect)
  {
    for (int index = 0; index < this.volumes.Count; ++index)
    {
      if ((UnityEngine.Object) this.volumes[index].gameObject == (UnityEngine.Object) colorEffect)
        return this.volumes[index];
    }
    for (int index = 0; index < this.volumes.Count; ++index)
    {
      if ((UnityEngine.Object) this.volumes[index].gameObject != (UnityEngine.Object) null && this.volumes[index].gameObject.SharedInstanceID == colorEffect.SharedInstanceID)
        return this.volumes[index];
    }
    return (VolumeEffect) null;
  }

  public void RemoveVolumeEffect(VolumeEffect volume) => this.volumes.Remove(volume);

  public AmplifyColorBase[] GetStoredEffects()
  {
    return this.volumes.Select<VolumeEffect, AmplifyColorBase>((Func<VolumeEffect, AmplifyColorBase>) (r => r.gameObject)).ToArray<AmplifyColorBase>();
  }
}
