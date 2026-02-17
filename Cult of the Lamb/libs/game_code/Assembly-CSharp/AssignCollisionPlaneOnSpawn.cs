// Decompiled with JetBrains decompiler
// Type: AssignCollisionPlaneOnSpawn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (ParticleSystem))]
public class AssignCollisionPlaneOnSpawn : MonoBehaviour
{
  public ParticleSystem _particleSystem;

  public void Start()
  {
    this._particleSystem = this.GetComponent<ParticleSystem>();
    VFXGroundPlane instance = VFXGroundPlane.Instance;
    if ((Object) instance != (Object) null)
      (this._particleSystem.collision with
      {
        enabled = true,
        type = ParticleSystemCollisionType.Planes
      }).SetPlane(0, instance.transform);
    else
      Debug.LogWarning((object) "Could not find a 'VFXGroundPlane' in the scene. Particles will not have a specific ground to collide with.", (Object) this);
  }
}
