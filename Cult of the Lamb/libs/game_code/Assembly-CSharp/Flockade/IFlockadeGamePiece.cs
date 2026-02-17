// Decompiled with JetBrains decompiler
// Type: Flockade.IFlockadeGamePiece
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;

#nullable disable
namespace Flockade;

public interface IFlockadeGamePiece
{
  FlockadeVirtualBlessingActivator Blessing => this.Core.Blessing;

  FlockadeGamePieceConfiguration Configuration => this.Core.Configuration;

  FlockadeVirtualGamePiece Core { get; }

  IFlockadeGameBoardTile Tile
  {
    get => this.Core.Tile;
    set => this.Core.Tile = value;
  }

  IFlockadeGamePiece.State Copy() => this.Core.Copy();

  FlockadeVirtualBlessingActivator CreateBlessingActivator(bool nullified = false)
  {
    return this.Core.CreateBlessingActivator(nullified);
  }

  FlockadeFight.Result Fight(IFlockadeGamePiece other) => this.Core.Fight(other);

  IFlockadeGamePiece.State Get() => this.Core.Get();

  IFlockadeGamePiece.State Pop() => this.Core.Pop();

  void Set(FlockadeGamePieceConfiguration gamePiece) => this.Core.Set(gamePiece);

  void Set(IFlockadeGamePiece.State gamePiece) => this.Core.Set(gamePiece);

  struct State(
    FlockadeGamePieceConfiguration configuration,
    FlockadeVirtualBlessingActivator blessing)
  {
    [CompilerGenerated]
    public FlockadeGamePieceConfiguration \u003CConfiguration\u003Ek__BackingField = configuration;
    [CompilerGenerated]
    public FlockadeVirtualBlessingActivator \u003CBlessing\u003Ek__BackingField = blessing;

    public readonly FlockadeGamePieceConfiguration Configuration
    {
      get => this.\u003CConfiguration\u003Ek__BackingField;
    }

    public readonly FlockadeVirtualBlessingActivator Blessing
    {
      get => this.\u003CBlessing\u003Ek__BackingField;
    }
  }
}
