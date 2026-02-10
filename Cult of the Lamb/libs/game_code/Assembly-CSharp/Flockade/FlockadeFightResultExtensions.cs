// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeFightResultExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
namespace Flockade;

public static class FlockadeFightResultExtensions
{
  public static FlockadeFight.Result Reverse(this FlockadeFight.Result self)
  {
    FlockadeFight.Result result;
    switch (self)
    {
      case FlockadeFight.Result.Defeat:
        result = FlockadeFight.Result.Win;
        break;
      case FlockadeFight.Result.Win:
        result = FlockadeFight.Result.Defeat;
        break;
      case FlockadeFight.Result.DefeatAndDuelPhaseEnd:
        result = FlockadeFight.Result.WinAndDuelPhaseEnd;
        break;
      case FlockadeFight.Result.WinAndDuelPhaseEnd:
        result = FlockadeFight.Result.DefeatAndDuelPhaseEnd;
        break;
      default:
        result = self;
        break;
    }
    return result;
  }
}
