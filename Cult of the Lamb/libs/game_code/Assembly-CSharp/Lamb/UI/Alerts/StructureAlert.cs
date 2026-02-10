// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Alerts.StructureAlert
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
namespace Lamb.UI.Alerts;

public class StructureAlert : AlertBadge<StructureBrain.TYPES>
{
  public override AlertCategory<StructureBrain.TYPES> _source
  {
    get => (AlertCategory<StructureBrain.TYPES>) DataManager.Instance.Alerts.Structures;
  }
}
