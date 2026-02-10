// Decompiled with JetBrains decompiler
// Type: BlockingDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BlockingDoor : BaseMonoBehaviour
{
  public static List<BlockingDoor> BlockingDoors = new List<BlockingDoor>();
  public BoxCollider2D Collider;
  public BlockingDoor.State StartingPosition = BlockingDoor.State.Closed;
  public float OpeningTime = 4f;
  public float ClosingTime = 4f;

  public void OnEnable() => BlockingDoor.BlockingDoors.Add(this);

  public void OnDisable() => BlockingDoor.BlockingDoors.Remove(this);

  public void Start()
  {
    this.Collider = this.GetComponentInChildren<BoxCollider2D>();
    this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, this.StartingPosition == BlockingDoor.State.Closed ? 0.0f : 2f);
    this.Collider.enabled = this.StartingPosition == BlockingDoor.State.Closed;
    Bounds bounds = this.Collider.bounds;
    AstarPath.active.UpdateGraphs(bounds);
  }

  public static void OpenAll()
  {
    foreach (BlockingDoor blockingDoor in BlockingDoor.BlockingDoors)
      blockingDoor.Open();
  }

  public void Open()
  {
    ActivateMiniMap.DisableTeleporting = false;
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.OpenRoutine());
  }

  public IEnumerator OpenRoutine()
  {
    BlockingDoor blockingDoor = this;
    Bounds bounds = blockingDoor.Collider.bounds;
    blockingDoor.Collider.enabled = false;
    AstarPath.active.UpdateGraphs(bounds);
    float Progress = 0.0f;
    Vector3 StartPosition = blockingDoor.transform.localPosition;
    Vector3 TargetPosition = new Vector3(blockingDoor.transform.localPosition.x, blockingDoor.transform.localPosition.y, 2f);
    Vector3 Position = blockingDoor.transform.localPosition;
    float x = blockingDoor.transform.localPosition.x;
    while ((double) (Progress += Time.deltaTime) < (double) blockingDoor.OpeningTime)
    {
      Position.z = Mathf.SmoothStep(StartPosition.z, TargetPosition.z, Progress / blockingDoor.OpeningTime);
      Position.x = x + Random.Range(-0.02f, 0.02f);
      blockingDoor.transform.localPosition = Position;
      yield return (object) null;
    }
    blockingDoor.transform.localPosition = new Vector3(x, blockingDoor.transform.localPosition.y, Position.z);
  }

  public static void CloseAll()
  {
    ActivateMiniMap.DisableTeleporting = true;
    foreach (BlockingDoor blockingDoor in BlockingDoor.BlockingDoors)
      blockingDoor.Close();
  }

  public void Close()
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.CloseRoutine());
  }

  public IEnumerator CloseRoutine()
  {
    BlockingDoor blockingDoor = this;
    blockingDoor.Collider.enabled = true;
    Bounds bounds = blockingDoor.Collider.bounds;
    AstarPath.active.UpdateGraphs(bounds);
    float Progress = 0.0f;
    Vector3 StartPosition = blockingDoor.transform.localPosition;
    Vector3 TargetPosition = new Vector3(blockingDoor.transform.localPosition.x, blockingDoor.transform.localPosition.y, 0.0f);
    Vector3 Position = blockingDoor.transform.localPosition;
    float x = blockingDoor.transform.localPosition.x;
    while ((double) (Progress += Time.deltaTime) < (double) blockingDoor.ClosingTime)
    {
      Position.z = Mathf.SmoothStep(StartPosition.z, TargetPosition.z, Progress / blockingDoor.ClosingTime);
      Position.x = x + Random.Range(-0.02f, 0.02f);
      blockingDoor.transform.localPosition = Position;
      yield return (object) null;
    }
    blockingDoor.transform.localPosition = new Vector3(x, blockingDoor.transform.localPosition.y, Position.z);
  }

  public enum State
  {
    Open,
    Closed,
  }
}
