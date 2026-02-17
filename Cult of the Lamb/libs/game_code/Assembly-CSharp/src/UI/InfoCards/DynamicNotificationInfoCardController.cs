// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.DynamicNotificationInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
