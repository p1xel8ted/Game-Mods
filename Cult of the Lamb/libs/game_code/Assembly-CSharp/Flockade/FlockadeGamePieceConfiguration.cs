// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeGamePieceConfiguration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
namespace Flockade;

[CreateAssetMenu(fileName = "FlockadeGamePieceConfiguration", menuName = "COTL/Flockade/FlockadeGamePieceConfiguration")]
public class FlockadeGamePieceConfiguration : ScriptableObject
{
  [SerializeField]
  public FlockadeGamePieceBaseConfiguration _baseConfiguration;
  [SerializeField]
  public FlockadeBlessingConfiguration _blessingConfiguration;
  [Header("Unique")]
  [SerializeField]
  public FlockadePieceType _type;
  [SerializeField]
  public Sprite _imageOverride;
  [SerializeField]
  public Sprite _attackingImageOverride;
  [SerializeField]
  public Sprite _flinchingImageOverride;
  [SerializeField]
  public Sprite _outlineOverride;
  [SerializeField]
  [TermsPopup("")]
  public string _name;
  [SerializeField]
  [TermsPopup("")]
  public string _description;
  [SerializeField]
  [TermsPopup("")]
  public string _hint;

  public FlockadeGamePieceBaseConfiguration BaseConfiguration => this._baseConfiguration;

  public FlockadeBlessingConfiguration BlessingConfiguration => this._blessingConfiguration;

  public FlockadePieceType Type => this._type;

  public Sprite Image
  {
    get
    {
      return !(bool) (Object) this._imageOverride ? this._baseConfiguration.Image : this._imageOverride;
    }
  }

  public Sprite AttackingImage
  {
    get
    {
      return !(bool) (Object) this._attackingImageOverride ? this._baseConfiguration.AttackingImage : this._attackingImageOverride;
    }
  }

  public Sprite FlinchingImage
  {
    get
    {
      return !(bool) (Object) this._flinchingImageOverride ? this._baseConfiguration.FlinchingImage : this._flinchingImageOverride;
    }
  }

  public Sprite Outline
  {
    get
    {
      return !(bool) (Object) this._outlineOverride ? this._baseConfiguration.Outline : this._outlineOverride;
    }
  }

  public string Name => this._name;

  public string Description => this._description;

  public string Hint => this._hint;
}
