// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeBlessingConfiguration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
namespace Flockade;

[CreateAssetMenu(fileName = "FlockadeBlessingConfiguration", menuName = "COTL/Flockade/FlockadeBlessingConfiguration")]
public class FlockadeBlessingConfiguration : ScriptableObject
{
  [SerializeReference]
  public IFlockadeBlessing _blessing;
  [SerializeField]
  public Sprite _background;
  [SerializeField]
  public Color _color;
  [SerializeField]
  public Sprite _icon;
  [SerializeField]
  public Sprite _outline;
  [SerializeField]
  [TermsPopup("")]
  public string _description;
  [SerializeField]
  public FlockadeFight.Doodle[] _attackDoodles;

  public IFlockadeBlessing Blessing => this._blessing;

  public string Description => this._description;

  public Sprite Background => this._background;

  public Color Color => this._color;

  public Sprite Icon => this._icon;

  public Sprite Outline => this._outline;

  public FlockadeFight.Doodle[] AttackDoodles => this._attackDoodles;
}
