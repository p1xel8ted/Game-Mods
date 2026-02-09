// Decompiled with JetBrains decompiler
// Type: EyeAnchor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EyeAnchor : MonoBehaviour
{
  public static List<EyeAnchor> All = new List<EyeAnchor>();
  [SerializeField]
  public Color gizmoColor = Color.cyan;
  [SerializeField]
  public float gizmoRadius = 0.1f;

  public void OnEnable()
  {
    if (EyeAnchor.All.Contains(this))
      return;
    EyeAnchor.All.Add(this);
  }

  public void OnDisable() => EyeAnchor.All.Remove(this);

  public void OnDrawGizmos()
  {
    Gizmos.color = this.gizmoColor;
    Gizmos.DrawWireSphere(this.transform.position, this.gizmoRadius);
  }
}
