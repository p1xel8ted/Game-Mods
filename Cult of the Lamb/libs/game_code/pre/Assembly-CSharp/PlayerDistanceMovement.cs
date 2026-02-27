// Decompiled with JetBrains decompiler
// Type: PlayerDistanceMovement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#nullable disable
public class PlayerDistanceMovement : BaseMonoBehaviour
{
  public GameObject MovePos;
  public GameObject objectToMove;
  public float distanceToMove;
  public float dist;
  public float distPercent;
  public Vector3 StartPos;
  private bool resetting;

  private void Start() => this.StartPos = this.transform.position;

  private void Update()
  {
    if (!((Object) PlayerFarming.Instance != (Object) null) || this.resetting)
      return;
    this.dist = Vector3.Distance(PlayerFarming.Instance.gameObject.transform.position, this.transform.position);
    if ((double) this.dist > (double) this.distanceToMove)
      return;
    this.distPercent = Mathf.Abs((float) ((double) this.dist / (double) this.distanceToMove - 1.0));
    this.objectToMove.transform.position = Vector3.Lerp(this.StartPos, this.MovePos.transform.position, Mathf.SmoothStep(0.0f, 1f, this.distPercent));
  }

  public void ForceReset()
  {
    this.resetting = true;
    this.objectToMove.transform.DOMove(this.StartPos, 0.25f).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.resetting = false));
  }
}
