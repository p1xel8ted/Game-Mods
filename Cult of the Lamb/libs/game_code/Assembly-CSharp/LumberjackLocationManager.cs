// Decompiled with JetBrains decompiler
// Type: LumberjackLocationManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Serialization;

#nullable disable
public class LumberjackLocationManager : LocationManager
{
  [FormerlySerializedAs("UnitLayer")]
  [SerializeField]
  public Transform _unitLayer;

  public override FollowerLocation Location => FollowerLocation.Lumberjack;

  public override Transform UnitLayer => this._unitLayer;

  public override Vector3 GetStartPosition(FollowerLocation prevLocation) => Vector3.zero;
}
