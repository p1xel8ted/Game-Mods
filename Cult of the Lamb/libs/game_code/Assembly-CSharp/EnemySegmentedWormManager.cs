// Decompiled with JetBrains decompiler
// Type: EnemySegmentedWormManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class EnemySegmentedWormManager : MonoBehaviour
{
  public float MoveForce = 1000f;
  public float MoveForceAgitated = 1500f;
  public float MoveFrequency = 2f;
  public float MoveFrequencyAgitated = 1.5f;
  public float TelegraphTime = 1f;
  public float TelegraphTimeAgitated = 0.8f;
  public float StunnedDuration = 1f;
  [Range(0.0f, 1f)]
  public float ChanceToMoveTowardsPlayer = 0.5f;
  public int TurnAngle = 45;
  public Vector2Int DirectionChangeCountRange = new Vector2Int(3, 5);
  public Vector3 TelegraphScale = new Vector3(1.3f, 1.3f, 1.3f);
  [Header("Single")]
  public float SingleTelegraphTime = 0.25f;
  public float SingleKnockbackModifier = 1f;
  public float SingleKnockbackDuration = 0.5f;
  public float TailDistance = 0.7f;
  public EnemySegmentedWorm[] wormSegments;

  public EnemySegmentedWorm[] WormSegments => this.wormSegments;

  public void Awake()
  {
    this.wormSegments = this.GetComponentsInChildren<EnemySegmentedWorm>();
    foreach (EnemySegmentedWorm wormSegment in this.WormSegments)
    {
      wormSegment.CurrentMoveForce = this.MoveForce;
      wormSegment.CurrentMoveFrequency = this.MoveFrequency;
      wormSegment.CurrentTelegraphTime = this.TelegraphTime;
      wormSegment.CurrentSplitStunnedDuration = this.StunnedDuration;
    }
  }

  public void Start() => this.SetupWorm();

  public void SetupWorm()
  {
    if ((Object) this.wormSegments[0] != (Object) null)
    {
      this.wormSegments[0].SetAsHead(false, false);
      this.wormSegments[0].Children = this.WormSegments.Length - 1;
    }
    for (int index = 1; index < this.wormSegments.Length; ++index)
    {
      if ((Object) this.wormSegments[index] != (Object) null)
      {
        this.wormSegments[0].TailSegments.Add(this.wormSegments[index]);
        this.wormSegments[index].SetAsTail(this.wormSegments[index - 1], this.wormSegments[0]);
      }
    }
  }

  public void RecalculateSegments()
  {
    int num = 0;
    EnemySegmentedWorm head = (EnemySegmentedWorm) null;
    for (int index = 0; index < this.wormSegments.Length; ++index)
    {
      if ((Object) this.wormSegments[index] == (Object) null && index + 1 < this.wormSegments.Length && (Object) this.wormSegments[index + 1] != (Object) null)
      {
        this.wormSegments[index + 1].SetAsHead(true, true);
        this.wormSegments[index + 1].TailSegments.Clear();
        head = this.wormSegments[index + 1];
        ++num;
      }
      else if (num == 0 && (Object) this.wormSegments[index] != (Object) null && index + 1 < this.wormSegments.Length)
      {
        this.wormSegments[index].SetAsHead(true, true);
        this.wormSegments[index].TailSegments.Clear();
        head = this.wormSegments[index];
        ++num;
      }
      else
      {
        EnemySegmentedWorm wormSegment = this.wormSegments[index];
        if ((Object) wormSegment != (Object) null && (double) wormSegment.health.HP > 0.0 && !wormSegment.IsHead())
        {
          wormSegment.SetAsTail(this.wormSegments[index - 1], head);
          head.TailSegments.Add(wormSegment);
        }
      }
    }
    int count = 0;
    for (int index = this.wormSegments.Length - 1; index >= 0; --index)
    {
      if (!((Object) this.wormSegments[index] == (Object) null))
      {
        if (this.wormSegments[index].IsHead())
        {
          this.wormSegments[index].SetChildrenCountOnSplit(count);
          count = -1;
        }
        ++count;
      }
    }
  }
}
