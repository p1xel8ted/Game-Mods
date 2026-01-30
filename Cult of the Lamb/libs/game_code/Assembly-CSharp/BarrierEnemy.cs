// Decompiled with JetBrains decompiler
// Type: BarrierEnemy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BarrierEnemy : MonoBehaviour
{
  public GameObject barrierPrefab;
  [HideInInspector]
  public GameObject barrierGameObject;
  public UnitObject barrierPartner;
  public float barrierLength = 1f;
  public float lerpSpeed;
  public bool barrierDestroyed;

  public void Start()
  {
    if (!((Object) this.barrierPartner != (Object) null))
      return;
    Debug.Log((object) "Barrier partner added");
    this.barrierGameObject = Object.Instantiate<GameObject>(this.barrierPrefab, this.transform);
    this.barrierGameObject.name = "BARRIER";
  }

  public void FixedUpdate()
  {
    if (this.barrierDestroyed)
      return;
    if ((Object) this.barrierPartner == (Object) null)
    {
      Debug.Log((object) "Barrier partner was null");
      Object.Destroy((Object) this.barrierGameObject);
      this.barrierDestroyed = true;
    }
    else
    {
      Vector3 vector3 = this.transform.position + (this.barrierPartner.transform.position - this.transform.position) / 2f;
      float num = Vector3.Distance(this.transform.position, this.barrierPartner.transform.position);
      this.barrierGameObject.transform.position = vector3;
      this.barrierGameObject.transform.LookAt(this.barrierPartner.transform.position, Vector3.forward);
      this.barrierGameObject.transform.Rotate(-90f, 0.0f, 90f);
      this.barrierGameObject.transform.localScale = this.barrierGameObject.transform.localScale with
      {
        x = num * this.barrierLength
      };
    }
  }
}
