// Decompiled with JetBrains decompiler
// Type: Lamb.UI.JobBoardInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class JobBoardInfoCardController : UIInfoCardController<JobBoardInfoCard, JobBoardMenuItem>
{
  public override bool IsSelectionValid(Selectable selectable, out JobBoardMenuItem showParam)
  {
    showParam = (JobBoardMenuItem) null;
    JobBoardMenuItem component;
    if (!selectable.TryGetComponent<JobBoardMenuItem>(out component))
      return false;
    showParam = component;
    return showParam.Objective != null;
  }
}
