// Decompiled with JetBrains decompiler
// Type: Rewired.IRacingWheelTemplate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
namespace Rewired;

public interface IRacingWheelTemplate : IControllerTemplate
{
  IControllerTemplateAxis wheel { get; }

  IControllerTemplateAxis accelerator { get; }

  IControllerTemplateAxis brake { get; }

  IControllerTemplateAxis clutch { get; }

  IControllerTemplateButton shiftDown { get; }

  IControllerTemplateButton shiftUp { get; }

  IControllerTemplateButton wheelButton1 { get; }

  IControllerTemplateButton wheelButton2 { get; }

  IControllerTemplateButton wheelButton3 { get; }

  IControllerTemplateButton wheelButton4 { get; }

  IControllerTemplateButton wheelButton5 { get; }

  IControllerTemplateButton wheelButton6 { get; }

  IControllerTemplateButton wheelButton7 { get; }

  IControllerTemplateButton wheelButton8 { get; }

  IControllerTemplateButton wheelButton9 { get; }

  IControllerTemplateButton wheelButton10 { get; }

  IControllerTemplateButton consoleButton1 { get; }

  IControllerTemplateButton consoleButton2 { get; }

  IControllerTemplateButton consoleButton3 { get; }

  IControllerTemplateButton consoleButton4 { get; }

  IControllerTemplateButton consoleButton5 { get; }

  IControllerTemplateButton consoleButton6 { get; }

  IControllerTemplateButton consoleButton7 { get; }

  IControllerTemplateButton consoleButton8 { get; }

  IControllerTemplateButton consoleButton9 { get; }

  IControllerTemplateButton consoleButton10 { get; }

  IControllerTemplateButton shifter1 { get; }

  IControllerTemplateButton shifter2 { get; }

  IControllerTemplateButton shifter3 { get; }

  IControllerTemplateButton shifter4 { get; }

  IControllerTemplateButton shifter5 { get; }

  IControllerTemplateButton shifter6 { get; }

  IControllerTemplateButton shifter7 { get; }

  IControllerTemplateButton shifter8 { get; }

  IControllerTemplateButton shifter9 { get; }

  IControllerTemplateButton shifter10 { get; }

  IControllerTemplateButton reverseGear { get; }

  IControllerTemplateButton select { get; }

  IControllerTemplateButton start { get; }

  IControllerTemplateButton systemButton { get; }

  IControllerTemplateButton horn { get; }

  IControllerTemplateDPad dPad { get; }
}
