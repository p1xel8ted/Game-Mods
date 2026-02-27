// Decompiled with JetBrains decompiler
// Type: LumberjackLocationManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
