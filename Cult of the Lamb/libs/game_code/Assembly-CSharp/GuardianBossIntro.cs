// Decompiled with JetBrains decompiler
// Type: GuardianBossIntro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
