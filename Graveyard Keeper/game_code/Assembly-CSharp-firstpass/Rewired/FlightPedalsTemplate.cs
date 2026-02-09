// Decompiled with JetBrains decompiler
// Type: Rewired.FlightPedalsTemplate
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Rewired;

public sealed class FlightPedalsTemplate(object payload) : 
  ControllerTemplate(payload),
  IFlightPedalsTemplate,
  IControllerTemplate
{
  public static Guid typeGuid = new Guid("f6fe76f8-be2a-4db2-b853-9e3652075913");
  public const int elementId_leftPedal = 0;
  public const int elementId_rightPedal = 1;
  public const int elementId_slide = 2;

  IControllerTemplateAxis IFlightPedalsTemplate.leftPedal
  {
    get => this.GetElement<IControllerTemplateAxis>(0);
  }

  IControllerTemplateAxis IFlightPedalsTemplate.rightPedal
  {
    get => this.GetElement<IControllerTemplateAxis>(1);
  }

  IControllerTemplateAxis IFlightPedalsTemplate.slide
  {
    get => this.GetElement<IControllerTemplateAxis>(2);
  }
}
