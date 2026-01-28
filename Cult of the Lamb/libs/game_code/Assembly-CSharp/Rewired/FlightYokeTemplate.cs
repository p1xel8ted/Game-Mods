// Decompiled with JetBrains decompiler
// Type: Rewired.FlightYokeTemplate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;

#nullable disable
namespace Rewired;

public sealed class FlightYokeTemplate(object payload) : 
  ControllerTemplate(payload),
  IFlightYokeTemplate,
  IControllerTemplate
{
  public static Guid typeGuid = new Guid("f311fa16-0ccc-41c0-ac4b-50f7100bb8ff");
  public const int elementId_rotateYoke = 0;
  public const int elementId_yokeZ = 1;
  public const int elementId_leftPaddle = 59;
  public const int elementId_rightPaddle = 60;
  public const int elementId_lever1Axis = 2;
  public const int elementId_lever1MinDetent = 64 /*0x40*/;
  public const int elementId_lever2Axis = 3;
  public const int elementId_lever2MinDetent = 65;
  public const int elementId_lever3Axis = 4;
  public const int elementId_lever3MinDetent = 66;
  public const int elementId_lever4Axis = 5;
  public const int elementId_lever4MinDetent = 67;
  public const int elementId_lever5Axis = 6;
  public const int elementId_lever5MinDetent = 68;
  public const int elementId_leftGripButton1 = 7;
  public const int elementId_leftGripButton2 = 8;
  public const int elementId_leftGripButton3 = 9;
  public const int elementId_leftGripButton4 = 10;
  public const int elementId_leftGripButton5 = 11;
  public const int elementId_leftGripButton6 = 12;
  public const int elementId_rightGripButton1 = 13;
  public const int elementId_rightGripButton2 = 14;
  public const int elementId_rightGripButton3 = 15;
  public const int elementId_rightGripButton4 = 16 /*0x10*/;
  public const int elementId_rightGripButton5 = 17;
  public const int elementId_rightGripButton6 = 18;
  public const int elementId_centerButton1 = 19;
  public const int elementId_centerButton2 = 20;
  public const int elementId_centerButton3 = 21;
  public const int elementId_centerButton4 = 22;
  public const int elementId_centerButton5 = 23;
  public const int elementId_centerButton6 = 24;
  public const int elementId_centerButton7 = 25;
  public const int elementId_centerButton8 = 26;
  public const int elementId_wheel1Up = 53;
  public const int elementId_wheel1Down = 54;
  public const int elementId_wheel1Press = 55;
  public const int elementId_wheel2Up = 56;
  public const int elementId_wheel2Down = 57;
  public const int elementId_wheel2Press = 58;
  public const int elementId_leftGripHatUp = 27;
  public const int elementId_leftGripHatUpRight = 28;
  public const int elementId_leftGripHatRight = 29;
  public const int elementId_leftGripHatDownRight = 30;
  public const int elementId_leftGripHatDown = 31 /*0x1F*/;
  public const int elementId_leftGripHatDownLeft = 32 /*0x20*/;
  public const int elementId_leftGripHatLeft = 33;
  public const int elementId_leftGripHatUpLeft = 34;
  public const int elementId_rightGripHatUp = 35;
  public const int elementId_rightGripHatUpRight = 36;
  public const int elementId_rightGripHatRight = 37;
  public const int elementId_rightGripHatDownRight = 38;
  public const int elementId_rightGripHatDown = 39;
  public const int elementId_rightGripHatDownLeft = 40;
  public const int elementId_rightGripHatLeft = 41;
  public const int elementId_rightGripHatUpLeft = 42;
  public const int elementId_consoleButton1 = 43;
  public const int elementId_consoleButton2 = 44;
  public const int elementId_consoleButton3 = 45;
  public const int elementId_consoleButton4 = 46;
  public const int elementId_consoleButton5 = 47;
  public const int elementId_consoleButton6 = 48 /*0x30*/;
  public const int elementId_consoleButton7 = 49;
  public const int elementId_consoleButton8 = 50;
  public const int elementId_consoleButton9 = 51;
  public const int elementId_consoleButton10 = 52;
  public const int elementId_mode1 = 61;
  public const int elementId_mode2 = 62;
  public const int elementId_mode3 = 63 /*0x3F*/;
  public const int elementId_yoke = 69;
  public const int elementId_lever1 = 70;
  public const int elementId_lever2 = 71;
  public const int elementId_lever3 = 72;
  public const int elementId_lever4 = 73;
  public const int elementId_lever5 = 74;
  public const int elementId_leftGripHat = 75;
  public const int elementId_rightGripHat = 76;

  IControllerTemplateButton IFlightYokeTemplate.leftPaddle
  {
    get => this.GetElement<IControllerTemplateButton>(59);
  }

  IControllerTemplateButton IFlightYokeTemplate.rightPaddle
  {
    get => this.GetElement<IControllerTemplateButton>(60);
  }

  IControllerTemplateButton IFlightYokeTemplate.leftGripButton1
  {
    get => this.GetElement<IControllerTemplateButton>(7);
  }

  IControllerTemplateButton IFlightYokeTemplate.leftGripButton2
  {
    get => this.GetElement<IControllerTemplateButton>(8);
  }

  IControllerTemplateButton IFlightYokeTemplate.leftGripButton3
  {
    get => this.GetElement<IControllerTemplateButton>(9);
  }

  IControllerTemplateButton IFlightYokeTemplate.leftGripButton4
  {
    get => this.GetElement<IControllerTemplateButton>(10);
  }

  IControllerTemplateButton IFlightYokeTemplate.leftGripButton5
  {
    get => this.GetElement<IControllerTemplateButton>(11);
  }

  IControllerTemplateButton IFlightYokeTemplate.leftGripButton6
  {
    get => this.GetElement<IControllerTemplateButton>(12);
  }

  IControllerTemplateButton IFlightYokeTemplate.rightGripButton1
  {
    get => this.GetElement<IControllerTemplateButton>(13);
  }

  IControllerTemplateButton IFlightYokeTemplate.rightGripButton2
  {
    get => this.GetElement<IControllerTemplateButton>(14);
  }

  IControllerTemplateButton IFlightYokeTemplate.rightGripButton3
  {
    get => this.GetElement<IControllerTemplateButton>(15);
  }

  IControllerTemplateButton IFlightYokeTemplate.rightGripButton4
  {
    get => this.GetElement<IControllerTemplateButton>(16 /*0x10*/);
  }

  IControllerTemplateButton IFlightYokeTemplate.rightGripButton5
  {
    get => this.GetElement<IControllerTemplateButton>(17);
  }

  IControllerTemplateButton IFlightYokeTemplate.rightGripButton6
  {
    get => this.GetElement<IControllerTemplateButton>(18);
  }

  IControllerTemplateButton IFlightYokeTemplate.centerButton1
  {
    get => this.GetElement<IControllerTemplateButton>(19);
  }

  IControllerTemplateButton IFlightYokeTemplate.centerButton2
  {
    get => this.GetElement<IControllerTemplateButton>(20);
  }

  IControllerTemplateButton IFlightYokeTemplate.centerButton3
  {
    get => this.GetElement<IControllerTemplateButton>(21);
  }

  IControllerTemplateButton IFlightYokeTemplate.centerButton4
  {
    get => this.GetElement<IControllerTemplateButton>(22);
  }

  IControllerTemplateButton IFlightYokeTemplate.centerButton5
  {
    get => this.GetElement<IControllerTemplateButton>(23);
  }

  IControllerTemplateButton IFlightYokeTemplate.centerButton6
  {
    get => this.GetElement<IControllerTemplateButton>(24);
  }

  IControllerTemplateButton IFlightYokeTemplate.centerButton7
  {
    get => this.GetElement<IControllerTemplateButton>(25);
  }

  IControllerTemplateButton IFlightYokeTemplate.centerButton8
  {
    get => this.GetElement<IControllerTemplateButton>(26);
  }

  IControllerTemplateButton IFlightYokeTemplate.wheel1Up
  {
    get => this.GetElement<IControllerTemplateButton>(53);
  }

  IControllerTemplateButton IFlightYokeTemplate.wheel1Down
  {
    get => this.GetElement<IControllerTemplateButton>(54);
  }

  IControllerTemplateButton IFlightYokeTemplate.wheel1Press
  {
    get => this.GetElement<IControllerTemplateButton>(55);
  }

  IControllerTemplateButton IFlightYokeTemplate.wheel2Up
  {
    get => this.GetElement<IControllerTemplateButton>(56);
  }

  IControllerTemplateButton IFlightYokeTemplate.wheel2Down
  {
    get => this.GetElement<IControllerTemplateButton>(57);
  }

  IControllerTemplateButton IFlightYokeTemplate.wheel2Press
  {
    get => this.GetElement<IControllerTemplateButton>(58);
  }

  IControllerTemplateButton IFlightYokeTemplate.consoleButton1
  {
    get => this.GetElement<IControllerTemplateButton>(43);
  }

  IControllerTemplateButton IFlightYokeTemplate.consoleButton2
  {
    get => this.GetElement<IControllerTemplateButton>(44);
  }

  IControllerTemplateButton IFlightYokeTemplate.consoleButton3
  {
    get => this.GetElement<IControllerTemplateButton>(45);
  }

  IControllerTemplateButton IFlightYokeTemplate.consoleButton4
  {
    get => this.GetElement<IControllerTemplateButton>(46);
  }

  IControllerTemplateButton IFlightYokeTemplate.consoleButton5
  {
    get => this.GetElement<IControllerTemplateButton>(47);
  }

  IControllerTemplateButton IFlightYokeTemplate.consoleButton6
  {
    get => this.GetElement<IControllerTemplateButton>(48 /*0x30*/);
  }

  IControllerTemplateButton IFlightYokeTemplate.consoleButton7
  {
    get => this.GetElement<IControllerTemplateButton>(49);
  }

  IControllerTemplateButton IFlightYokeTemplate.consoleButton8
  {
    get => this.GetElement<IControllerTemplateButton>(50);
  }

  IControllerTemplateButton IFlightYokeTemplate.consoleButton9
  {
    get => this.GetElement<IControllerTemplateButton>(51);
  }

  IControllerTemplateButton IFlightYokeTemplate.consoleButton10
  {
    get => this.GetElement<IControllerTemplateButton>(52);
  }

  IControllerTemplateButton IFlightYokeTemplate.mode1
  {
    get => this.GetElement<IControllerTemplateButton>(61);
  }

  IControllerTemplateButton IFlightYokeTemplate.mode2
  {
    get => this.GetElement<IControllerTemplateButton>(62);
  }

  IControllerTemplateButton IFlightYokeTemplate.mode3
  {
    get => this.GetElement<IControllerTemplateButton>(63 /*0x3F*/);
  }

  IControllerTemplateYoke IFlightYokeTemplate.yoke => this.GetElement<IControllerTemplateYoke>(69);

  IControllerTemplateThrottle IFlightYokeTemplate.lever1
  {
    get => this.GetElement<IControllerTemplateThrottle>(70);
  }

  IControllerTemplateThrottle IFlightYokeTemplate.lever2
  {
    get => this.GetElement<IControllerTemplateThrottle>(71);
  }

  IControllerTemplateThrottle IFlightYokeTemplate.lever3
  {
    get => this.GetElement<IControllerTemplateThrottle>(72);
  }

  IControllerTemplateThrottle IFlightYokeTemplate.lever4
  {
    get => this.GetElement<IControllerTemplateThrottle>(73);
  }

  IControllerTemplateThrottle IFlightYokeTemplate.lever5
  {
    get => this.GetElement<IControllerTemplateThrottle>(74);
  }

  IControllerTemplateHat IFlightYokeTemplate.leftGripHat
  {
    get => this.GetElement<IControllerTemplateHat>(75);
  }

  IControllerTemplateHat IFlightYokeTemplate.rightGripHat
  {
    get => this.GetElement<IControllerTemplateHat>(76);
  }
}
