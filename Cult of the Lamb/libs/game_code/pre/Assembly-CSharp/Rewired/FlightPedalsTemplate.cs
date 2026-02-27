// Decompiled with JetBrains decompiler
// Type: Rewired.FlightPedalsTemplate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Rewired;

public sealed class FlightPedalsTemplate(object payload) : 
  ControllerTemplate(payload),
  IFlightPedalsTemplate,
  IControllerTemplate
{
  public static readonly Guid typeGuid = new Guid("f6fe76f8-be2a-4db2-b853-9e3652075913");
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
