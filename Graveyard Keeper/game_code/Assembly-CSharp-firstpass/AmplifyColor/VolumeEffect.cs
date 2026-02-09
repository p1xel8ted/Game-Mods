// Decompiled with JetBrains decompiler
// Type: AmplifyColor.VolumeEffect
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#nullable disable
namespace AmplifyColor;

[Serializable]
public class VolumeEffect
{
  public AmplifyColorBase gameObject;
  public List<VolumeEffectComponent> components;

  public VolumeEffect(AmplifyColorBase effect)
  {
    this.gameObject = effect;
    this.components = new List<VolumeEffectComponent>();
  }

  public static VolumeEffect BlendValuesToVolumeEffect(
    VolumeEffectFlags flags,
    VolumeEffect volume1,
    VolumeEffect volume2,
    float blend)
  {
    VolumeEffect volumeEffect = new VolumeEffect(volume1.gameObject);
    foreach (VolumeEffectComponentFlags component in flags.components)
    {
      if (component.blendFlag)
      {
        VolumeEffectComponent effectComponent1 = volume1.FindEffectComponent(component.componentName);
        VolumeEffectComponent effectComponent2 = volume2.FindEffectComponent(component.componentName);
        if (effectComponent1 != null && effectComponent2 != null)
        {
          VolumeEffectComponent volumeEffectComponent = new VolumeEffectComponent(effectComponent1.componentName);
          foreach (VolumeEffectFieldFlags componentField in component.componentFields)
          {
            if (componentField.blendFlag)
            {
              VolumeEffectField effectField1 = effectComponent1.FindEffectField(componentField.fieldName);
              VolumeEffectField effectField2 = effectComponent2.FindEffectField(componentField.fieldName);
              if (effectField1 != null && effectField2 != null)
              {
                VolumeEffectField volumeEffectField = new VolumeEffectField(effectField1.fieldName, effectField1.fieldType);
                switch (volumeEffectField.fieldType)
                {
                  case "System.Single":
                    volumeEffectField.valueSingle = Mathf.Lerp(effectField1.valueSingle, effectField2.valueSingle, blend);
                    break;
                  case "System.Boolean":
                    volumeEffectField.valueBoolean = effectField2.valueBoolean;
                    break;
                  case "UnityEngine.Vector2":
                    volumeEffectField.valueVector2 = Vector2.Lerp(effectField1.valueVector2, effectField2.valueVector2, blend);
                    break;
                  case "UnityEngine.Vector3":
                    volumeEffectField.valueVector3 = Vector3.Lerp(effectField1.valueVector3, effectField2.valueVector3, blend);
                    break;
                  case "UnityEngine.Vector4":
                    volumeEffectField.valueVector4 = Vector4.Lerp(effectField1.valueVector4, effectField2.valueVector4, blend);
                    break;
                  case "UnityEngine.Color":
                    volumeEffectField.valueColor = Color.Lerp(effectField1.valueColor, effectField2.valueColor, blend);
                    break;
                }
                volumeEffectComponent.fields.Add(volumeEffectField);
              }
            }
          }
          volumeEffect.components.Add(volumeEffectComponent);
        }
      }
    }
    return volumeEffect;
  }

  public VolumeEffectComponent AddComponent(Component c, VolumeEffectComponentFlags compFlags)
  {
    if (compFlags == null)
    {
      VolumeEffectComponent volumeEffectComponent = new VolumeEffectComponent(c.GetType()?.ToString() ?? "");
      this.components.Add(volumeEffectComponent);
      return volumeEffectComponent;
    }
    string compName = c.GetType()?.ToString() ?? "";
    VolumeEffectComponent effectComponent;
    if ((effectComponent = this.FindEffectComponent(compName)) != null)
    {
      effectComponent.UpdateComponent(c, compFlags);
      return effectComponent;
    }
    VolumeEffectComponent volumeEffectComponent1 = new VolumeEffectComponent(c, compFlags);
    this.components.Add(volumeEffectComponent1);
    return volumeEffectComponent1;
  }

  public void RemoveEffectComponent(VolumeEffectComponent comp) => this.components.Remove(comp);

  public void UpdateVolume()
  {
    if ((UnityEngine.Object) this.gameObject == (UnityEngine.Object) null)
      return;
    foreach (VolumeEffectComponentFlags component1 in this.gameObject.EffectFlags.components)
    {
      if (component1.blendFlag)
      {
        Component component2 = this.gameObject.GetComponent(component1.componentName);
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
          this.AddComponent(component2, component1);
      }
    }
  }

  public void SetValues(AmplifyColorBase targetColor)
  {
    VolumeEffectFlags effectFlags = targetColor.EffectFlags;
    GameObject gameObject = targetColor.gameObject;
    foreach (VolumeEffectComponentFlags component1 in effectFlags.components)
    {
      if (component1.blendFlag)
      {
        Component component2 = gameObject.GetComponent(component1.componentName);
        VolumeEffectComponent effectComponent = this.FindEffectComponent(component1.componentName);
        if (!((UnityEngine.Object) component2 == (UnityEngine.Object) null) && effectComponent != null)
        {
          foreach (VolumeEffectFieldFlags componentField in component1.componentFields)
          {
            if (componentField.blendFlag)
            {
              FieldInfo field = component2.GetType().GetField(componentField.fieldName);
              VolumeEffectField effectField = effectComponent.FindEffectField(componentField.fieldName);
              if (!FieldInfo.op_Equality(field, (FieldInfo) null) && effectField != null)
              {
                switch (field.FieldType.FullName)
                {
                  case "System.Single":
                    field.SetValue((object) component2, (object) effectField.valueSingle);
                    continue;
                  case "System.Boolean":
                    field.SetValue((object) component2, (object) effectField.valueBoolean);
                    continue;
                  case "UnityEngine.Vector2":
                    field.SetValue((object) component2, (object) effectField.valueVector2);
                    continue;
                  case "UnityEngine.Vector3":
                    field.SetValue((object) component2, (object) effectField.valueVector3);
                    continue;
                  case "UnityEngine.Vector4":
                    field.SetValue((object) component2, (object) effectField.valueVector4);
                    continue;
                  case "UnityEngine.Color":
                    field.SetValue((object) component2, (object) effectField.valueColor);
                    continue;
                  default:
                    continue;
                }
              }
            }
          }
        }
      }
    }
  }

  public void BlendValues(AmplifyColorBase targetColor, VolumeEffect other, float blendAmount)
  {
    VolumeEffectFlags effectFlags = targetColor.EffectFlags;
    GameObject gameObject = targetColor.gameObject;
    for (int index1 = 0; index1 < effectFlags.components.Count; ++index1)
    {
      VolumeEffectComponentFlags component1 = effectFlags.components[index1];
      if (component1.blendFlag)
      {
        Component component2 = gameObject.GetComponent(component1.componentName);
        VolumeEffectComponent effectComponent1 = this.FindEffectComponent(component1.componentName);
        VolumeEffectComponent effectComponent2 = other.FindEffectComponent(component1.componentName);
        if (!((UnityEngine.Object) component2 == (UnityEngine.Object) null) && effectComponent1 != null && effectComponent2 != null)
        {
          for (int index2 = 0; index2 < component1.componentFields.Count; ++index2)
          {
            VolumeEffectFieldFlags componentField = component1.componentFields[index2];
            if (componentField.blendFlag)
            {
              FieldInfo field = component2.GetType().GetField(componentField.fieldName);
              VolumeEffectField effectField1 = effectComponent1.FindEffectField(componentField.fieldName);
              VolumeEffectField effectField2 = effectComponent2.FindEffectField(componentField.fieldName);
              if (!FieldInfo.op_Equality(field, (FieldInfo) null) && effectField1 != null && effectField2 != null)
              {
                switch (field.FieldType.FullName)
                {
                  case "System.Single":
                    field.SetValue((object) component2, (object) Mathf.Lerp(effectField1.valueSingle, effectField2.valueSingle, blendAmount));
                    continue;
                  case "System.Boolean":
                    field.SetValue((object) component2, (object) effectField2.valueBoolean);
                    continue;
                  case "UnityEngine.Vector2":
                    field.SetValue((object) component2, (object) Vector2.Lerp(effectField1.valueVector2, effectField2.valueVector2, blendAmount));
                    continue;
                  case "UnityEngine.Vector3":
                    field.SetValue((object) component2, (object) Vector3.Lerp(effectField1.valueVector3, effectField2.valueVector3, blendAmount));
                    continue;
                  case "UnityEngine.Vector4":
                    field.SetValue((object) component2, (object) Vector4.Lerp(effectField1.valueVector4, effectField2.valueVector4, blendAmount));
                    continue;
                  case "UnityEngine.Color":
                    field.SetValue((object) component2, (object) Color.Lerp(effectField1.valueColor, effectField2.valueColor, blendAmount));
                    continue;
                  default:
                    continue;
                }
              }
            }
          }
        }
      }
    }
  }

  public VolumeEffectComponent FindEffectComponent(string compName)
  {
    for (int index = 0; index < this.components.Count; ++index)
    {
      if (this.components[index].componentName == compName)
        return this.components[index];
    }
    return (VolumeEffectComponent) null;
  }

  public static Component[] ListAcceptableComponents(AmplifyColorBase go)
  {
    return (UnityEngine.Object) go == (UnityEngine.Object) null ? new Component[0] : ((IEnumerable<Component>) go.GetComponents(typeof (Component))).Where<Component>((Func<Component, bool>) (comp => (UnityEngine.Object) comp != (UnityEngine.Object) null && !(comp.GetType()?.ToString() ?? "").StartsWith("UnityEngine.") && !System.Type.op_Equality(comp.GetType(), typeof (AmplifyColorBase)))).ToArray<Component>();
  }

  public string[] GetComponentNames()
  {
    return this.components.Select<VolumeEffectComponent, string>((Func<VolumeEffectComponent, string>) (r => r.componentName)).ToArray<string>();
  }
}
