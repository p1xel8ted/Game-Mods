// Decompiled with JetBrains decompiler
// Type: src.VFXDeviceLighting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace src;

public class VFXDeviceLighting : MonoBehaviour
{
  [SerializeField]
  public VFXDeviceLighting.LightingType lightType;
  [SerializeField]
  public Color color;
  [SerializeField]
  public float duration = 0.5f;

  public void ShowLighting()
  {
    if (this.lightType != VFXDeviceLighting.LightingType.Flash)
      return;
    DeviceLightingManager.FlashColor(this.color, this.duration);
  }

  public enum LightingType
  {
    None,
    Flash,
  }
}
