// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.ProCamera2DTriggerRails
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

[HelpURL("http://www.procamera2d.com/user-guide/trigger-rails/")]
public class ProCamera2DTriggerRails : BaseTrigger
{
  public static string TriggerName = "Rails Trigger";
  [HideInInspector]
  public ProCamera2DRails ProCamera2DRails;
  public TriggerRailsMode Mode;
  public float TransitionDuration = 1f;

  public void Start()
  {
    if ((Object) this.ProCamera2D == (Object) null)
      return;
    if ((Object) this.ProCamera2DRails == (Object) null)
      this.ProCamera2DRails = Object.FindObjectOfType<ProCamera2DRails>();
    if (!((Object) this.ProCamera2DRails == (Object) null))
      return;
    Debug.LogWarning((object) "Rails extension couldn't be found on ProCamera2D. Please enable it to use this trigger.");
  }

  public override void EnteredTrigger()
  {
    base.EnteredTrigger();
    if (this.Mode == TriggerRailsMode.Enable)
      this.ProCamera2DRails.EnableTargets(this.TransitionDuration);
    else
      this.ProCamera2DRails.DisableTargets(this.TransitionDuration);
  }
}
