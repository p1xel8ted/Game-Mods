// Decompiled with JetBrains decompiler
// Type: Rewired.IGamepadTemplate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
namespace Rewired;

public interface IGamepadTemplate : IControllerTemplate
{
  IControllerTemplateButton actionBottomRow1 { get; }

  IControllerTemplateButton a { get; }

  IControllerTemplateButton actionBottomRow2 { get; }

  IControllerTemplateButton b { get; }

  IControllerTemplateButton actionBottomRow3 { get; }

  IControllerTemplateButton c { get; }

  IControllerTemplateButton actionTopRow1 { get; }

  IControllerTemplateButton x { get; }

  IControllerTemplateButton actionTopRow2 { get; }

  IControllerTemplateButton y { get; }

  IControllerTemplateButton actionTopRow3 { get; }

  IControllerTemplateButton z { get; }

  IControllerTemplateButton leftShoulder1 { get; }

  IControllerTemplateButton leftBumper { get; }

  IControllerTemplateAxis leftShoulder2 { get; }

  IControllerTemplateAxis leftTrigger { get; }

  IControllerTemplateButton rightShoulder1 { get; }

  IControllerTemplateButton rightBumper { get; }

  IControllerTemplateAxis rightShoulder2 { get; }

  IControllerTemplateAxis rightTrigger { get; }

  IControllerTemplateButton center1 { get; }

  IControllerTemplateButton back { get; }

  IControllerTemplateButton center2 { get; }

  IControllerTemplateButton start { get; }

  IControllerTemplateButton center3 { get; }

  IControllerTemplateButton guide { get; }

  IControllerTemplateThumbStick leftStick { get; }

  IControllerTemplateThumbStick rightStick { get; }

  IControllerTemplateDPad dPad { get; }
}
