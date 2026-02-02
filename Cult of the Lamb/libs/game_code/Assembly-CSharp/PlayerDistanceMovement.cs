// Decompiled with JetBrains decompiler
// Type: PlayerDistanceMovement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Runtime.CompilerServices;
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
  public bool resetting;

  public void Start() => this.StartPos = this.transform.position;

  public void Update()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if (!this.resetting && (Object) this.objectToMove != (Object) null)
      {
        this.dist = Vector3.Distance(player.transform.position, this.transform.position);
        if ((double) this.dist <= (double) this.distanceToMove)
        {
          this.distPercent = Mathf.Abs((float) ((double) this.dist / (double) this.distanceToMove - 1.0));
          this.objectToMove.transform.position = Vector3.Lerp(this.StartPos, this.MovePos.transform.position, Mathf.SmoothStep(0.0f, 1f, this.distPercent));
        }
      }
    }
  }

  public void ForceReset()
  {
    this.resetting = true;
    this.objectToMove.transform.DOMove(this.StartPos, 0.25f).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.resetting = false));
  }

  [CompilerGenerated]
  public void \u003CForceReset\u003Eb__9_0() => this.resetting = false;
}
