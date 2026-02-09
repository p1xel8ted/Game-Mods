// Decompiled with JetBrains decompiler
// Type: AuraReceiveSubcomponent
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class AuraReceiveSubcomponent : AuraSubcomponentBase
{
  public AuraReceiver aura_receiver;
  public AuraDefinition aura_definition;
  public EventDelegate on_aura_appeared;
  public EventDelegate on_aura_disappeared;
  public float _aura_time;
  public float _last_update_time;

  public AuraReceiveSubcomponent(AuraReceiver aura_receiver, string aura_id)
  {
    this.aura_receiver = aura_receiver;
    this.aura_id = aura_id;
    this.aura_definition = GameBalance.me.GetData<AuraDefinition>(aura_id);
  }

  public virtual void OnAuraAppeared()
  {
    if (this.on_aura_appeared != null)
      this.on_aura_appeared.Execute();
    MainGame.me.gui_elements.buffs.Redraw();
    this._aura_time = 0.0f;
    this._last_update_time = Time.time;
    Debug.Log((object) ("OnAuraAppeared " + this.aura_id));
  }

  public virtual void OnAuraDisappeared()
  {
    if (this.on_aura_disappeared != null)
      this.on_aura_disappeared.Execute();
    MainGame.me.gui_elements.buffs.Redraw();
    Debug.Log((object) ("OnAuraDisappeared " + this.aura_id));
  }

  public override void Update()
  {
    base.Update();
    this._aura_time += Time.time - this._last_update_time;
    this._last_update_time = Time.time;
    if (this.aura == null || (double) this._aura_time <= (double) this.aura.time_tick)
      return;
    this._aura_time -= this.aura.time_tick;
    this.DoAuraTick();
  }

  public void DoAuraTick()
  {
    if (this.aura_receiver == null)
    {
      Debug.LogError((object) "AuraReceiveSubcomponent has no receiver");
    }
    else
    {
      float auraParam = this.aura_receiver.GetAuraParam(this.aura_id);
      if (auraParam.Equals(0.0f))
        return;
      WorldGameObject wgo = (WorldGameObject) null;
      Debug.LogException(new Exception("Auras not implemented"));
      if ((UnityEngine.Object) wgo == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "AuraReceiveSubcomponent receiver has no WorldGameObject");
      }
      else
      {
        float hp = wgo.hp;
        wgo.AddToParams(this.aura.res * auraParam);
        float a = wgo.hp - hp;
        if (a.EqualsTo(0.0f))
          return;
        EffectBubblesManager.ShowStackedHP(wgo, a);
      }
    }
  }
}
