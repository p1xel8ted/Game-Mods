// Decompiled with JetBrains decompiler
// Type: BillboardFacingCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BillboardFacingCamera : BaseMonoBehaviour
{
  public Camera m_Camera;

  public void OnEnable()
  {
    this.m_Camera = Camera.main;
    Canvas component = this.GetComponent<Canvas>();
    if (!((Object) component != (Object) null))
      return;
    component.worldCamera = this.m_Camera;
  }

  public void Update()
  {
    if ((Object) this.m_Camera == (Object) null)
      return;
    this.transform.LookAt(this.transform.position + this.m_Camera.transform.rotation * Vector3.forward, this.m_Camera.transform.rotation * Vector3.up);
  }
}
