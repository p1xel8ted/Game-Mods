// Decompiled with JetBrains decompiler
// Type: AmplifyColor.VolumeEffectFlags
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace AmplifyColor;

[Serializable]
public class VolumeEffectFlags
{
  public List<VolumeEffectComponentFlags> components;

  public VolumeEffectFlags() => this.components = new List<VolumeEffectComponentFlags>();

  public void AddComponent(Component c)
  {
    VolumeEffectComponentFlags effectComponentFlags;
    if ((effectComponentFlags = this.components.Find((Predicate<VolumeEffectComponentFlags>) (s => s.componentName == (c.GetType()?.ToString() ?? "")))) != null)
      effectComponentFlags.UpdateComponentFlags(c);
    else
      this.components.Add(new VolumeEffectComponentFlags(c));
  }

  public void UpdateFlags(VolumeEffect effectVol)
  {
    foreach (VolumeEffectComponent component in effectVol.components)
    {
      VolumeEffectComponent comp = component;
      VolumeEffectComponentFlags effectComponentFlags;
      if ((effectComponentFlags = this.components.Find((Predicate<VolumeEffectComponentFlags>) (s => s.componentName == comp.componentName))) == null)
        this.components.Add(new VolumeEffectComponentFlags(comp));
      else
        effectComponentFlags.UpdateComponentFlags(comp);
    }
  }

  public static void UpdateCamFlags(AmplifyColorBase[] effects, AmplifyColorVolumeBase[] volumes)
  {
    foreach (AmplifyColorBase effect in effects)
    {
      effect.EffectFlags = new VolumeEffectFlags();
      foreach (AmplifyColorVolumeBase volume in volumes)
      {
        VolumeEffect volumeEffect = volume.EffectContainer.FindVolumeEffect(effect);
        if (volumeEffect != null)
          effect.EffectFlags.UpdateFlags(volumeEffect);
      }
    }
  }

  public VolumeEffect GenerateEffectData(AmplifyColorBase go)
  {
    VolumeEffect effectData = new VolumeEffect(go);
    foreach (VolumeEffectComponentFlags component1 in this.components)
    {
      if (component1.blendFlag)
      {
        Component component2 = go.GetComponent(component1.componentName);
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
          effectData.AddComponent(component2, component1);
      }
    }
    return effectData;
  }

  public VolumeEffectComponentFlags FindComponentFlags(string compName)
  {
    for (int index = 0; index < this.components.Count; ++index)
    {
      if (this.components[index].componentName == compName)
        return this.components[index];
    }
    return (VolumeEffectComponentFlags) null;
  }

  public string[] GetComponentNames()
  {
    return this.components.Where<VolumeEffectComponentFlags>((Func<VolumeEffectComponentFlags, bool>) (r => r.blendFlag)).Select<VolumeEffectComponentFlags, string>((Func<VolumeEffectComponentFlags, string>) (r => r.componentName)).ToArray<string>();
  }
}
