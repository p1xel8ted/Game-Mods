// Decompiled with JetBrains decompiler
// Type: Lamb.UI.JobBoardInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
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
