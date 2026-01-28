// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.DynamicNotificationInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
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
