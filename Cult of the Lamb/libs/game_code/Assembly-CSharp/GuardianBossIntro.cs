// Decompiled with JetBrains decompiler
// Type: GuardianBossIntro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;

#nullable disable
public class GuardianBossIntro : BossIntro
{
  public override IEnumerator PlayRoutine(bool skipped = false)
  {
    this.Callback?.Invoke();
    yield return (object) null;
  }
}
