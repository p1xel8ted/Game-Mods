// Decompiled with JetBrains decompiler
// Type: LavaTrail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class LavaTrail : BaseMonoBehaviour
{
  [SerializeField]
  public GameObject trapLavaPrefab;
  [SerializeField]
  public float distanceBetweenPoints;
  [SerializeField]
  public Health owner;
  public Vector3 previousPlacedPosition = Vector3.zero;

  public void Update()
  {
    if (!(this.previousPlacedPosition == Vector3.zero) && (double) Vector3.Distance(this.transform.position, this.previousPlacedPosition) <= (double) this.distanceBetweenPoints)
      return;
    this.previousPlacedPosition = this.transform.position;
    TrapLava.CreateLava(this.trapLavaPrefab, this.transform.position, (Object) this.transform.parent == (Object) null ? (Transform) null : this.transform.parent.parent, this.owner);
  }
}
