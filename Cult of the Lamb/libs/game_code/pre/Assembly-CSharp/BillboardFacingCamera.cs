// Decompiled with JetBrains decompiler
// Type: BillboardFacingCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BillboardFacingCamera : BaseMonoBehaviour
{
  private Camera m_Camera;

  private void OnEnable()
  {
    this.m_Camera = Camera.main;
    Canvas component = this.GetComponent<Canvas>();
    if (!((Object) component != (Object) null))
      return;
    component.worldCamera = this.m_Camera;
  }

  private void Update()
  {
    if ((Object) this.m_Camera == (Object) null)
      return;
    this.transform.LookAt(this.transform.position + this.m_Camera.transform.rotation * Vector3.forward, this.m_Camera.transform.rotation * Vector3.up);
  }
}
