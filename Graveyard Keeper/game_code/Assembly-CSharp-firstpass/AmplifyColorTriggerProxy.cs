// Decompiled with JetBrains decompiler
// Type: AmplifyColorTriggerProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("")]
[RequireComponent(typeof (Rigidbody))]
[RequireComponent(typeof (SphereCollider))]
public class AmplifyColorTriggerProxy : AmplifyColorTriggerProxyBase
{
  public SphereCollider sphereCollider;
  public Rigidbody rigidBody;

  public void Start()
  {
    this.sphereCollider = this.GetComponent<SphereCollider>();
    this.sphereCollider.radius = 0.01f;
    this.sphereCollider.isTrigger = true;
    this.rigidBody = this.GetComponent<Rigidbody>();
    this.rigidBody.useGravity = false;
    this.rigidBody.isKinematic = true;
  }

  public void LateUpdate()
  {
    this.transform.position = this.Reference.position;
    this.transform.rotation = this.Reference.rotation;
  }
}
