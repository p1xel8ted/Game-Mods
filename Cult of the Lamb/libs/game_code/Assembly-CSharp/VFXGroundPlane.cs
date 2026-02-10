// Decompiled with JetBrains decompiler
// Type: VFXGroundPlane
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class VFXGroundPlane : MonoBehaviour
{
  public static VFXGroundPlane _instance;

  public static VFXGroundPlane Instance => VFXGroundPlane._instance;

  public void Awake() => VFXGroundPlane._instance = this;

  public void OnDestroy() => VFXGroundPlane._instance = (VFXGroundPlane) null;
}
