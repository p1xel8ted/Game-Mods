// Decompiled with JetBrains decompiler
// Type: BarricadeLine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BarricadeLine : BaseMonoBehaviour
{
  public List<GameObject> Barricades = new List<GameObject>();
  public BoxCollider2D Collider;
  public BarricadeLine.State StartingPosition = BarricadeLine.State.Closed;
  public float OpeningTime = 0.5f;
  public float ClosingTime = 0.5f;

  public void Start()
  {
    this.Collider = this.GetComponentInChildren<BoxCollider2D>();
    foreach (GameObject barricade in this.Barricades)
      barricade.SetActive(this.StartingPosition == BarricadeLine.State.Closed);
    this.Collider.enabled = this.StartingPosition == BarricadeLine.State.Closed;
  }

  public void Open() => this.StartCoroutine(this.OpenRoutine());

  public IEnumerator OpenRoutine()
  {
    this.Collider.enabled = false;
    foreach (GameObject barricade in this.Barricades)
    {
      barricade.SetActive(false);
      yield return (object) new WaitForSeconds(0.02f);
    }
  }

  public void Close() => this.StartCoroutine(this.CloseRoutine());

  public IEnumerator CloseRoutine()
  {
    this.Collider.enabled = true;
    foreach (GameObject barricade in this.Barricades)
    {
      barricade.SetActive(true);
      yield return (object) new WaitForSeconds(0.02f);
    }
  }

  public enum State
  {
    Open,
    Closed,
  }
}
