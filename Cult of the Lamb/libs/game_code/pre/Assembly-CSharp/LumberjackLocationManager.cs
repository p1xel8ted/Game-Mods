// Decompiled with JetBrains decompiler
// Type: LumberjackLocationManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Serialization;

#nullable disable
public class LumberjackLocationManager : LocationManager
{
  [FormerlySerializedAs("UnitLayer")]
  [SerializeField]
  private Transform _unitLayer;

  public override FollowerLocation Location => FollowerLocation.Lumberjack;

  public override Transform UnitLayer => this._unitLayer;

  protected override Vector3 GetStartPosition(FollowerLocation prevLocation) => Vector3.zero;
}
