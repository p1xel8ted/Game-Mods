// Decompiled with JetBrains decompiler
// Type: AlignEncounter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class AlignEncounter : MonoBehaviour
{
  public GameObject containerToAlign;
  public float alignToClosestValue = 1f;

  public void DefaultSizedButton()
  {
    if ((Object) this.containerToAlign == (Object) null)
      this.containerToAlign = this.gameObject;
    foreach (Transform componentsInChild in this.containerToAlign.transform.GetComponentsInChildren<Transform>())
      componentsInChild.position = new Vector3((float) Mathf.RoundToInt(componentsInChild.position.x / this.alignToClosestValue) * this.alignToClosestValue, (float) Mathf.RoundToInt(componentsInChild.position.y / this.alignToClosestValue) * this.alignToClosestValue, (float) Mathf.RoundToInt(componentsInChild.position.z / this.alignToClosestValue) * this.alignToClosestValue);
  }
}
