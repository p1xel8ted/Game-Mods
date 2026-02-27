// Decompiled with JetBrains decompiler
// Type: EasyCurvedLine.CurvedLinePoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace EasyCurvedLine;

public class CurvedLinePoint : BaseMonoBehaviour
{
  [HideInInspector]
  public bool showGizmo = true;
  [HideInInspector]
  public float gizmoSize = 0.1f;
  [HideInInspector]
  public Color gizmoColor = new Color(1f, 0.0f, 0.0f, 0.5f);
  public bool StayLocked;
  public GameObject lockToGameObject;
  public Vector3 position;
  public Vector3 offset;
  public AnimationCurve animationCurve;
  public bool shouldAnimate;
  public float curveDeltaTime;
  public float MaxDist = 1f;
  public Vector3 currentPosition;
  public Vector3 OriginlPosition;
  public float CosAngle;
  public float CosSpeed = 10f;

  public void Start()
  {
    this.OriginlPosition = this.transform.position;
    this.currentPosition = this.transform.position;
    this.CosAngle = (float) Random.Range(0, 360);
    if (!((Object) this.lockToGameObject != (Object) null))
      return;
    this.position = this.lockToGameObject.transform.position;
    this.gameObject.transform.position = this.position + this.offset;
  }

  public void Update()
  {
    if (this.StayLocked)
    {
      this.position = this.lockToGameObject.transform.position;
      this.gameObject.transform.position = this.position + this.offset;
    }
    else
    {
      if (this.animationCurve == null || !this.shouldAnimate)
        return;
      this.currentPosition = this.transform.position;
      this.currentPosition.z = this.OriginlPosition.z + this.MaxDist * Mathf.Cos(this.CosAngle += this.CosSpeed * Time.deltaTime);
      this.transform.position = this.currentPosition;
    }
  }

  public void OnDrawGizmos()
  {
    if (this.showGizmo)
    {
      Gizmos.color = this.gizmoColor;
      Gizmos.DrawSphere(this.transform.position, this.gizmoSize);
    }
    if (!((Object) this.lockToGameObject != (Object) null))
      return;
    this.position = this.lockToGameObject.transform.position;
    this.gameObject.transform.position = this.position + this.offset;
  }

  public void OnDrawGizmosSelected()
  {
    if ((Object) this.lockToGameObject != (Object) null)
    {
      this.position = this.lockToGameObject.transform.position;
      this.gameObject.transform.position = this.position + this.offset;
    }
    CurvedLineRenderer component = this.transform.parent.GetComponent<CurvedLineRenderer>();
    if (!((Object) component != (Object) null))
      return;
    component.Update();
  }
}
