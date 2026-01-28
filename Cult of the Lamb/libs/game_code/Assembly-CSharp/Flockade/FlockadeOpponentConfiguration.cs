// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeOpponentConfiguration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Flockade;

[CreateAssetMenu(fileName = "FlockadeOpponentConfiguration", menuName = "COTL/Flockade/FlockadeOpponentConfiguration")]
public class FlockadeOpponentConfiguration : ScriptableObject
{
  [SerializeField]
  public FlockadeOpponentConfiguration.OpponentType _type;
  [SerializeField]
  public FlockadeNpcConfiguration _npcConfiguration;

  public FlockadeOpponentConfiguration.OpponentType Type => this._type;

  public FlockadeNpcConfiguration NpcConfiguration
  {
    get
    {
      return this._type != FlockadeOpponentConfiguration.OpponentType.Npc ? (FlockadeNpcConfiguration) null : this._npcConfiguration;
    }
  }

  public enum OpponentType
  {
    CoopPlayer,
    Npc,
    Twitch,
  }
}
