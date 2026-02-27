// Decompiled with JetBrains decompiler
// Type: BarricadeLine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BarricadeLine : BaseMonoBehaviour
{
  public List<GameObject> Barricades = new List<GameObject>();
  private BoxCollider2D Collider;
  public BarricadeLine.State StartingPosition = BarricadeLine.State.Closed;
  public float OpeningTime = 0.5f;
  public float ClosingTime = 0.5f;

  private void Start()
  {
    this.Collider = this.GetComponentInChildren<BoxCollider2D>();
    foreach (GameObject barricade in this.Barricades)
      barricade.SetActive(this.StartingPosition == BarricadeLine.State.Closed);
    this.Collider.enabled = this.StartingPosition == BarricadeLine.State.Closed;
  }

  public void Open() => this.StartCoroutine((IEnumerator) this.OpenRoutine());

  private IEnumerator OpenRoutine()
  {
    this.Collider.enabled = false;
    foreach (GameObject barricade in this.Barricades)
    {
      barricade.SetActive(false);
      yield return (object) new WaitForSeconds(0.02f);
    }
  }

  public void Close() => this.StartCoroutine((IEnumerator) this.CloseRoutine());

  private IEnumerator CloseRoutine()
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
