// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeGamePieceBaseConfiguration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
namespace Flockade;

[CreateAssetMenu(fileName = "FlockadeGamePieceBaseConfiguration", menuName = "COTL/Flockade/FlockadeGamePieceBaseConfiguration")]
public class FlockadeGamePieceBaseConfiguration : ScriptableObject
{
  [SerializeField]
  public FlockadeGamePiece.Kind _kind;
  [SerializeField]
  [TermsPopup("")]
  public string _kindName;
  [SerializeField]
  [TermsPopup("")]
  public string _kindDescription;
  [SerializeField]
  public Sprite _image;
  [SerializeField]
  public Sprite _attackingImage;
  [SerializeField]
  public string _attackingSound;
  [SerializeField]
  public Sprite _flinchingImage;
  [SerializeField]
  public Sprite _outline;
  [SerializeField]
  public Sprite _schematicsWithoutShepherd;
  [SerializeField]
  public Sprite _schematicsWithShepherd;
  [SerializeField]
  public FlockadeFight.Doodle[] _attackDoodles;

  public FlockadeGamePiece.Kind Kind => this._kind;

  public string KindName => this._kindName;

  public string KindDescription => this._kindDescription;

  public Sprite Image => this._image;

  public Sprite AttackingImage => this._attackingImage;

  public string AttackingSound => this._attackingSound;

  public Sprite FlinchingImage => this._flinchingImage;

  public Sprite Outline => this._outline;

  public Sprite Schematics
  {
    get
    {
      return !FlockadePieceManager.IsAnyPieceOfSameKindUnlocked(FlockadePieceType.Shepherd) ? this._schematicsWithoutShepherd : this._schematicsWithShepherd;
    }
  }

  public FlockadeFight.Doodle[] AttackDoodles => this._attackDoodles;
}
