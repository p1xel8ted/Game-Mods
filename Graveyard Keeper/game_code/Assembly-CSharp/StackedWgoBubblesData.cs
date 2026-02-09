// Decompiled with JetBrains decompiler
// Type: StackedWgoBubblesData
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class StackedWgoBubblesData
{
  public long id;
  public WorldGameObject wgo;
  public GameRes res;
  public int frames_delay;
  public float period_delay;
  public Vector3? custom_pos;

  public StackedWgoBubblesData(long id, WorldGameObject wgo, GameRes res, Vector3? custom_pos = null)
  {
    this.id = id;
    this.wgo = wgo;
    this.res = res;
    if (!custom_pos.HasValue)
      return;
    this.custom_pos = custom_pos;
  }

  public void TryToShowBubble()
  {
    if (this.res.IsEmpty())
      return;
    EffectBubblesManager.ShowImmediately(this.wgo, this.res);
    this.res.Clear();
    this.res.durability = 0.0f;
    this.period_delay = 0.5f * Time.timeScale;
  }

  public void AddRes(GameRes res)
  {
    if (res.IsEmpty())
      return;
    this.res += res;
    if (this.frames_delay != 0 || (double) this.period_delay >= 0.0)
      return;
    this.frames_delay = 3;
    this.period_delay = 0.0f;
  }
}
