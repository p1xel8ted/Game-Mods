// Decompiled with JetBrains decompiler
// Type: SimpleMoveToPosition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class SimpleMoveToPosition : MonoBehaviour
{
  [SerializeField]
  public Vector3 position;
  [SerializeField]
  public float delay;
  [SerializeField]
  public GameObject lookPosition;
  [SerializeField]
  public bool onEnable;
  public bool triggered;

  public void OnEnable()
  {
    if (this.triggered)
      return;
    this.MoveToPosition(this.delay);
  }

  public void MoveToPosition() => this.MoveToPosition(0.0f);

  public void MoveToPosition(float delay)
  {
    GameManager.GetInstance().WaitForSeconds(delay, (System.Action) (() =>
    {
      if (!this.gameObject.activeInHierarchy)
        return;
      this.triggered = true;
      PlayerFarming.Instance.GoToAndStop(this.position, this.lookPosition);
    }));
  }

  public void OnDrawGizmos()
  {
    Gizmos.color = Color.blue;
    Gizmos.DrawWireSphere(this.position, 0.5f);
  }

  [CompilerGenerated]
  public void \u003CMoveToPosition\u003Eb__7_0()
  {
    if (!this.gameObject.activeInHierarchy)
      return;
    this.triggered = true;
    PlayerFarming.Instance.GoToAndStop(this.position, this.lookPosition);
  }
}
