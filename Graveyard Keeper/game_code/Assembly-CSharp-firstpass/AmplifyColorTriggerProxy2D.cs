// Decompiled with JetBrains decompiler
// Type: AmplifyColorTriggerProxy2D
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (Rigidbody2D))]
[RequireComponent(typeof (CircleCollider2D))]
[AddComponentMenu("")]
public class AmplifyColorTriggerProxy2D : AmplifyColorTriggerProxyBase
{
  public CircleCollider2D circleCollider;
  public Rigidbody2D rigidBody;

  public void Start()
  {
    this.circleCollider = this.GetComponent<CircleCollider2D>();
    this.circleCollider.radius = 0.01f;
    this.circleCollider.isTrigger = true;
    this.rigidBody = this.GetComponent<Rigidbody2D>();
    this.rigidBody.gravityScale = 0.0f;
    this.rigidBody.isKinematic = true;
  }

  public void LateUpdate()
  {
    this.transform.position = this.Reference.position;
    this.transform.rotation = this.Reference.rotation;
  }
}
