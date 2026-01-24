// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.DynamicNotificationInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using UnityEngine.UI;

#nullable disable
namespace src.UI.InfoCards;

public class DynamicNotificationInfoCardController : 
  UIInfoCardController<DynamicNotificationInfoCard, DynamicNotificationData>
{
  public override bool IsSelectionValid(
    Selectable selectable,
    out DynamicNotificationData showParam)
  {
    showParam = (DynamicNotificationData) null;
    NotificationDynamicGeneric component;
    if (!selectable.TryGetComponent<NotificationDynamicGeneric>(out component))
      return false;
    showParam = component.Data;
    return true;
  }
}
