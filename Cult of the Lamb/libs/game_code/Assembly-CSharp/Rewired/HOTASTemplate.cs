// Decompiled with JetBrains decompiler
// Type: Rewired.HOTASTemplate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;

#nullable disable
namespace Rewired;

public sealed class HOTASTemplate(object payload) : 
  ControllerTemplate(payload),
  IHOTASTemplate,
  IControllerTemplate
{
  public static Guid typeGuid = new Guid("061a00cf-d8c2-4f8d-8cb5-a15a010bc53e");
  public const int elementId_stickX = 0;
  public const int elementId_stickY = 1;
  public const int elementId_stickRotate = 2;
  public const int elementId_stickMiniStick1X = 78;
  public const int elementId_stickMiniStick1Y = 79;
  public const int elementId_stickMiniStick1Press = 80 /*0x50*/;
  public const int elementId_stickMiniStick2X = 81;
  public const int elementId_stickMiniStick2Y = 82;
  public const int elementId_stickMiniStick2Press = 83;
  public const int elementId_stickTrigger = 3;
  public const int elementId_stickTriggerStage2 = 4;
  public const int elementId_stickPinkyButton = 5;
  public const int elementId_stickPinkyTrigger = 154;
  public const int elementId_stickButton1 = 6;
  public const int elementId_stickButton2 = 7;
  public const int elementId_stickButton3 = 8;
  public const int elementId_stickButton4 = 9;
  public const int elementId_stickButton5 = 10;
  public const int elementId_stickButton6 = 11;
  public const int elementId_stickButton7 = 12;
  public const int elementId_stickButton8 = 13;
  public const int elementId_stickButton9 = 14;
  public const int elementId_stickButton10 = 15;
  public const int elementId_stickBaseButton1 = 18;
  public const int elementId_stickBaseButton2 = 19;
  public const int elementId_stickBaseButton3 = 20;
  public const int elementId_stickBaseButton4 = 21;
  public const int elementId_stickBaseButton5 = 22;
  public const int elementId_stickBaseButton6 = 23;
  public const int elementId_stickBaseButton7 = 24;
  public const int elementId_stickBaseButton8 = 25;
  public const int elementId_stickBaseButton9 = 26;
  public const int elementId_stickBaseButton10 = 27;
  public const int elementId_stickBaseButton11 = 161;
  public const int elementId_stickBaseButton12 = 162;
  public const int elementId_stickHat1Up = 28;
  public const int elementId_stickHat1UpRight = 29;
  public const int elementId_stickHat1Right = 30;
  public const int elementId_stickHat1DownRight = 31 /*0x1F*/;
  public const int elementId_stickHat1Down = 32 /*0x20*/;
  public const int elementId_stickHat1DownLeft = 33;
  public const int elementId_stickHat1Left = 34;
  public const int elementId_stickHat1Up_Left = 35;
  public const int elementId_stickHat2Up = 36;
  public const int elementId_stickHat2Up_right = 37;
  public const int elementId_stickHat2Right = 38;
  public const int elementId_stickHat2Down_Right = 39;
  public const int elementId_stickHat2Down = 40;
  public const int elementId_stickHat2Down_Left = 41;
  public const int elementId_stickHat2Left = 42;
  public const int elementId_stickHat2Up_Left = 43;
  public const int elementId_stickHat3Up = 84;
  public const int elementId_stickHat3Up_Right = 85;
  public const int elementId_stickHat3Right = 86;
  public const int elementId_stickHat3Down_Right = 87;
  public const int elementId_stickHat3Down = 88;
  public const int elementId_stickHat3Down_Left = 89;
  public const int elementId_stickHat3Left = 90;
  public const int elementId_stickHat3Up_Left = 91;
  public const int elementId_stickHat4Up = 92;
  public const int elementId_stickHat4Up_Right = 93;
  public const int elementId_stickHat4Right = 94;
  public const int elementId_stickHat4Down_Right = 95;
  public const int elementId_stickHat4Down = 96 /*0x60*/;
  public const int elementId_stickHat4Down_Left = 97;
  public const int elementId_stickHat4Left = 98;
  public const int elementId_stickHat4Up_Left = 99;
  public const int elementId_mode1 = 44;
  public const int elementId_mode2 = 45;
  public const int elementId_mode3 = 46;
  public const int elementId_throttle1Axis = 49;
  public const int elementId_throttle2Axis = 155;
  public const int elementId_throttle1MinDetent = 166;
  public const int elementId_throttle2MinDetent = 167;
  public const int elementId_throttleButton1 = 50;
  public const int elementId_throttleButton2 = 51;
  public const int elementId_throttleButton3 = 52;
  public const int elementId_throttleButton4 = 53;
  public const int elementId_throttleButton5 = 54;
  public const int elementId_throttleButton6 = 55;
  public const int elementId_throttleButton7 = 56;
  public const int elementId_throttleButton8 = 57;
  public const int elementId_throttleButton9 = 58;
  public const int elementId_throttleButton10 = 59;
  public const int elementId_throttleBaseButton1 = 60;
  public const int elementId_throttleBaseButton2 = 61;
  public const int elementId_throttleBaseButton3 = 62;
  public const int elementId_throttleBaseButton4 = 63 /*0x3F*/;
  public const int elementId_throttleBaseButton5 = 64 /*0x40*/;
  public const int elementId_throttleBaseButton6 = 65;
  public const int elementId_throttleBaseButton7 = 66;
  public const int elementId_throttleBaseButton8 = 67;
  public const int elementId_throttleBaseButton9 = 68;
  public const int elementId_throttleBaseButton10 = 69;
  public const int elementId_throttleBaseButton11 = 132;
  public const int elementId_throttleBaseButton12 = 133;
  public const int elementId_throttleBaseButton13 = 134;
  public const int elementId_throttleBaseButton14 = 135;
  public const int elementId_throttleBaseButton15 = 136;
  public const int elementId_throttleSlider1 = 70;
  public const int elementId_throttleSlider2 = 71;
  public const int elementId_throttleSlider3 = 72;
  public const int elementId_throttleSlider4 = 73;
  public const int elementId_throttleDial1 = 74;
  public const int elementId_throttleDial2 = 142;
  public const int elementId_throttleDial3 = 143;
  public const int elementId_throttleDial4 = 144 /*0x90*/;
  public const int elementId_throttleMiniStickX = 75;
  public const int elementId_throttleMiniStickY = 76;
  public const int elementId_throttleMiniStickPress = 77;
  public const int elementId_throttleWheel1Forward = 145;
  public const int elementId_throttleWheel1Back = 146;
  public const int elementId_throttleWheel1Press = 147;
  public const int elementId_throttleWheel2Forward = 148;
  public const int elementId_throttleWheel2Back = 149;
  public const int elementId_throttleWheel2Press = 150;
  public const int elementId_throttleWheel3Forward = 151;
  public const int elementId_throttleWheel3Back = 152;
  public const int elementId_throttleWheel3Press = 153;
  public const int elementId_throttleHat1Up = 100;
  public const int elementId_throttleHat1Up_Right = 101;
  public const int elementId_throttleHat1Right = 102;
  public const int elementId_throttleHat1Down_Right = 103;
  public const int elementId_throttleHat1Down = 104;
  public const int elementId_throttleHat1Down_Left = 105;
  public const int elementId_throttleHat1Left = 106;
  public const int elementId_throttleHat1Up_Left = 107;
  public const int elementId_throttleHat2Up = 108;
  public const int elementId_throttleHat2Up_Right = 109;
  public const int elementId_throttleHat2Right = 110;
  public const int elementId_throttleHat2Down_Right = 111;
  public const int elementId_throttleHat2Down = 112 /*0x70*/;
  public const int elementId_throttleHat2Down_Left = 113;
  public const int elementId_throttleHat2Left = 114;
  public const int elementId_throttleHat2Up_Left = 115;
  public const int elementId_throttleHat3Up = 116;
  public const int elementId_throttleHat3Up_Right = 117;
  public const int elementId_throttleHat3Right = 118;
  public const int elementId_throttleHat3Down_Right = 119;
  public const int elementId_throttleHat3Down = 120;
  public const int elementId_throttleHat3Down_Left = 121;
  public const int elementId_throttleHat3Left = 122;
  public const int elementId_throttleHat3Up_Left = 123;
  public const int elementId_throttleHat4Up = 124;
  public const int elementId_throttleHat4Up_Right = 125;
  public const int elementId_throttleHat4Right = 126;
  public const int elementId_throttleHat4Down_Right = 127 /*0x7F*/;
  public const int elementId_throttleHat4Down = 128 /*0x80*/;
  public const int elementId_throttleHat4Down_Left = 129;
  public const int elementId_throttleHat4Left = 130;
  public const int elementId_throttleHat4Up_Left = 131;
  public const int elementId_leftPedal = 168;
  public const int elementId_rightPedal = 169;
  public const int elementId_slidePedals = 170;
  public const int elementId_stick = 171;
  public const int elementId_stickMiniStick1 = 172;
  public const int elementId_stickMiniStick2 = 173;
  public const int elementId_stickHat1 = 174;
  public const int elementId_stickHat2 = 175;
  public const int elementId_stickHat3 = 176 /*0xB0*/;
  public const int elementId_stickHat4 = 177;
  public const int elementId_throttle1 = 178;
  public const int elementId_throttle2 = 179;
  public const int elementId_throttleMiniStick = 180;
  public const int elementId_throttleHat1 = 181;
  public const int elementId_throttleHat2 = 182;
  public const int elementId_throttleHat3 = 183;
  public const int elementId_throttleHat4 = 184;

  IControllerTemplateButton IHOTASTemplate.stickTrigger
  {
    get => this.GetElement<IControllerTemplateButton>(3);
  }

  IControllerTemplateButton IHOTASTemplate.stickTriggerStage2
  {
    get => this.GetElement<IControllerTemplateButton>(4);
  }

  IControllerTemplateButton IHOTASTemplate.stickPinkyButton
  {
    get => this.GetElement<IControllerTemplateButton>(5);
  }

  IControllerTemplateButton IHOTASTemplate.stickPinkyTrigger
  {
    get => this.GetElement<IControllerTemplateButton>(154);
  }

  IControllerTemplateButton IHOTASTemplate.stickButton1
  {
    get => this.GetElement<IControllerTemplateButton>(6);
  }

  IControllerTemplateButton IHOTASTemplate.stickButton2
  {
    get => this.GetElement<IControllerTemplateButton>(7);
  }

  IControllerTemplateButton IHOTASTemplate.stickButton3
  {
    get => this.GetElement<IControllerTemplateButton>(8);
  }

  IControllerTemplateButton IHOTASTemplate.stickButton4
  {
    get => this.GetElement<IControllerTemplateButton>(9);
  }

  IControllerTemplateButton IHOTASTemplate.stickButton5
  {
    get => this.GetElement<IControllerTemplateButton>(10);
  }

  IControllerTemplateButton IHOTASTemplate.stickButton6
  {
    get => this.GetElement<IControllerTemplateButton>(11);
  }

  IControllerTemplateButton IHOTASTemplate.stickButton7
  {
    get => this.GetElement<IControllerTemplateButton>(12);
  }

  IControllerTemplateButton IHOTASTemplate.stickButton8
  {
    get => this.GetElement<IControllerTemplateButton>(13);
  }

  IControllerTemplateButton IHOTASTemplate.stickButton9
  {
    get => this.GetElement<IControllerTemplateButton>(14);
  }

  IControllerTemplateButton IHOTASTemplate.stickButton10
  {
    get => this.GetElement<IControllerTemplateButton>(15);
  }

  IControllerTemplateButton IHOTASTemplate.stickBaseButton1
  {
    get => this.GetElement<IControllerTemplateButton>(18);
  }

  IControllerTemplateButton IHOTASTemplate.stickBaseButton2
  {
    get => this.GetElement<IControllerTemplateButton>(19);
  }

  IControllerTemplateButton IHOTASTemplate.stickBaseButton3
  {
    get => this.GetElement<IControllerTemplateButton>(20);
  }

  IControllerTemplateButton IHOTASTemplate.stickBaseButton4
  {
    get => this.GetElement<IControllerTemplateButton>(21);
  }

  IControllerTemplateButton IHOTASTemplate.stickBaseButton5
  {
    get => this.GetElement<IControllerTemplateButton>(22);
  }

  IControllerTemplateButton IHOTASTemplate.stickBaseButton6
  {
    get => this.GetElement<IControllerTemplateButton>(23);
  }

  IControllerTemplateButton IHOTASTemplate.stickBaseButton7
  {
    get => this.GetElement<IControllerTemplateButton>(24);
  }

  IControllerTemplateButton IHOTASTemplate.stickBaseButton8
  {
    get => this.GetElement<IControllerTemplateButton>(25);
  }

  IControllerTemplateButton IHOTASTemplate.stickBaseButton9
  {
    get => this.GetElement<IControllerTemplateButton>(26);
  }

  IControllerTemplateButton IHOTASTemplate.stickBaseButton10
  {
    get => this.GetElement<IControllerTemplateButton>(27);
  }

  IControllerTemplateButton IHOTASTemplate.stickBaseButton11
  {
    get => this.GetElement<IControllerTemplateButton>(161);
  }

  IControllerTemplateButton IHOTASTemplate.stickBaseButton12
  {
    get => this.GetElement<IControllerTemplateButton>(162);
  }

  IControllerTemplateButton IHOTASTemplate.mode1 => this.GetElement<IControllerTemplateButton>(44);

  IControllerTemplateButton IHOTASTemplate.mode2 => this.GetElement<IControllerTemplateButton>(45);

  IControllerTemplateButton IHOTASTemplate.mode3 => this.GetElement<IControllerTemplateButton>(46);

  IControllerTemplateButton IHOTASTemplate.throttleButton1
  {
    get => this.GetElement<IControllerTemplateButton>(50);
  }

  IControllerTemplateButton IHOTASTemplate.throttleButton2
  {
    get => this.GetElement<IControllerTemplateButton>(51);
  }

  IControllerTemplateButton IHOTASTemplate.throttleButton3
  {
    get => this.GetElement<IControllerTemplateButton>(52);
  }

  IControllerTemplateButton IHOTASTemplate.throttleButton4
  {
    get => this.GetElement<IControllerTemplateButton>(53);
  }

  IControllerTemplateButton IHOTASTemplate.throttleButton5
  {
    get => this.GetElement<IControllerTemplateButton>(54);
  }

  IControllerTemplateButton IHOTASTemplate.throttleButton6
  {
    get => this.GetElement<IControllerTemplateButton>(55);
  }

  IControllerTemplateButton IHOTASTemplate.throttleButton7
  {
    get => this.GetElement<IControllerTemplateButton>(56);
  }

  IControllerTemplateButton IHOTASTemplate.throttleButton8
  {
    get => this.GetElement<IControllerTemplateButton>(57);
  }

  IControllerTemplateButton IHOTASTemplate.throttleButton9
  {
    get => this.GetElement<IControllerTemplateButton>(58);
  }

  IControllerTemplateButton IHOTASTemplate.throttleButton10
  {
    get => this.GetElement<IControllerTemplateButton>(59);
  }

  IControllerTemplateButton IHOTASTemplate.throttleBaseButton1
  {
    get => this.GetElement<IControllerTemplateButton>(60);
  }

  IControllerTemplateButton IHOTASTemplate.throttleBaseButton2
  {
    get => this.GetElement<IControllerTemplateButton>(61);
  }

  IControllerTemplateButton IHOTASTemplate.throttleBaseButton3
  {
    get => this.GetElement<IControllerTemplateButton>(62);
  }

  IControllerTemplateButton IHOTASTemplate.throttleBaseButton4
  {
    get => this.GetElement<IControllerTemplateButton>(63 /*0x3F*/);
  }

  IControllerTemplateButton IHOTASTemplate.throttleBaseButton5
  {
    get => this.GetElement<IControllerTemplateButton>(64 /*0x40*/);
  }

  IControllerTemplateButton IHOTASTemplate.throttleBaseButton6
  {
    get => this.GetElement<IControllerTemplateButton>(65);
  }

  IControllerTemplateButton IHOTASTemplate.throttleBaseButton7
  {
    get => this.GetElement<IControllerTemplateButton>(66);
  }

  IControllerTemplateButton IHOTASTemplate.throttleBaseButton8
  {
    get => this.GetElement<IControllerTemplateButton>(67);
  }

  IControllerTemplateButton IHOTASTemplate.throttleBaseButton9
  {
    get => this.GetElement<IControllerTemplateButton>(68);
  }

  IControllerTemplateButton IHOTASTemplate.throttleBaseButton10
  {
    get => this.GetElement<IControllerTemplateButton>(69);
  }

  IControllerTemplateButton IHOTASTemplate.throttleBaseButton11
  {
    get => this.GetElement<IControllerTemplateButton>(132);
  }

  IControllerTemplateButton IHOTASTemplate.throttleBaseButton12
  {
    get => this.GetElement<IControllerTemplateButton>(133);
  }

  IControllerTemplateButton IHOTASTemplate.throttleBaseButton13
  {
    get => this.GetElement<IControllerTemplateButton>(134);
  }

  IControllerTemplateButton IHOTASTemplate.throttleBaseButton14
  {
    get => this.GetElement<IControllerTemplateButton>(135);
  }

  IControllerTemplateButton IHOTASTemplate.throttleBaseButton15
  {
    get => this.GetElement<IControllerTemplateButton>(136);
  }

  IControllerTemplateAxis IHOTASTemplate.throttleSlider1
  {
    get => this.GetElement<IControllerTemplateAxis>(70);
  }

  IControllerTemplateAxis IHOTASTemplate.throttleSlider2
  {
    get => this.GetElement<IControllerTemplateAxis>(71);
  }

  IControllerTemplateAxis IHOTASTemplate.throttleSlider3
  {
    get => this.GetElement<IControllerTemplateAxis>(72);
  }

  IControllerTemplateAxis IHOTASTemplate.throttleSlider4
  {
    get => this.GetElement<IControllerTemplateAxis>(73);
  }

  IControllerTemplateAxis IHOTASTemplate.throttleDial1
  {
    get => this.GetElement<IControllerTemplateAxis>(74);
  }

  IControllerTemplateAxis IHOTASTemplate.throttleDial2
  {
    get => this.GetElement<IControllerTemplateAxis>(142);
  }

  IControllerTemplateAxis IHOTASTemplate.throttleDial3
  {
    get => this.GetElement<IControllerTemplateAxis>(143);
  }

  IControllerTemplateAxis IHOTASTemplate.throttleDial4
  {
    get => this.GetElement<IControllerTemplateAxis>(144 /*0x90*/);
  }

  IControllerTemplateButton IHOTASTemplate.throttleWheel1Forward
  {
    get => this.GetElement<IControllerTemplateButton>(145);
  }

  IControllerTemplateButton IHOTASTemplate.throttleWheel1Back
  {
    get => this.GetElement<IControllerTemplateButton>(146);
  }

  IControllerTemplateButton IHOTASTemplate.throttleWheel1Press
  {
    get => this.GetElement<IControllerTemplateButton>(147);
  }

  IControllerTemplateButton IHOTASTemplate.throttleWheel2Forward
  {
    get => this.GetElement<IControllerTemplateButton>(148);
  }

  IControllerTemplateButton IHOTASTemplate.throttleWheel2Back
  {
    get => this.GetElement<IControllerTemplateButton>(149);
  }

  IControllerTemplateButton IHOTASTemplate.throttleWheel2Press
  {
    get => this.GetElement<IControllerTemplateButton>(150);
  }

  IControllerTemplateButton IHOTASTemplate.throttleWheel3Forward
  {
    get => this.GetElement<IControllerTemplateButton>(151);
  }

  IControllerTemplateButton IHOTASTemplate.throttleWheel3Back
  {
    get => this.GetElement<IControllerTemplateButton>(152);
  }

  IControllerTemplateButton IHOTASTemplate.throttleWheel3Press
  {
    get => this.GetElement<IControllerTemplateButton>(153);
  }

  IControllerTemplateAxis IHOTASTemplate.leftPedal => this.GetElement<IControllerTemplateAxis>(168);

  IControllerTemplateAxis IHOTASTemplate.rightPedal
  {
    get => this.GetElement<IControllerTemplateAxis>(169);
  }

  IControllerTemplateAxis IHOTASTemplate.slidePedals
  {
    get => this.GetElement<IControllerTemplateAxis>(170);
  }

  IControllerTemplateStick IHOTASTemplate.stick => this.GetElement<IControllerTemplateStick>(171);

  IControllerTemplateThumbStick IHOTASTemplate.stickMiniStick1
  {
    get => this.GetElement<IControllerTemplateThumbStick>(172);
  }

  IControllerTemplateThumbStick IHOTASTemplate.stickMiniStick2
  {
    get => this.GetElement<IControllerTemplateThumbStick>(173);
  }

  IControllerTemplateHat IHOTASTemplate.stickHat1 => this.GetElement<IControllerTemplateHat>(174);

  IControllerTemplateHat IHOTASTemplate.stickHat2 => this.GetElement<IControllerTemplateHat>(175);

  IControllerTemplateHat IHOTASTemplate.stickHat3
  {
    get => this.GetElement<IControllerTemplateHat>(176 /*0xB0*/);
  }

  IControllerTemplateHat IHOTASTemplate.stickHat4 => this.GetElement<IControllerTemplateHat>(177);

  IControllerTemplateThrottle IHOTASTemplate.throttle1
  {
    get => this.GetElement<IControllerTemplateThrottle>(178);
  }

  IControllerTemplateThrottle IHOTASTemplate.throttle2
  {
    get => this.GetElement<IControllerTemplateThrottle>(179);
  }

  IControllerTemplateThumbStick IHOTASTemplate.throttleMiniStick
  {
    get => this.GetElement<IControllerTemplateThumbStick>(180);
  }

  IControllerTemplateHat IHOTASTemplate.throttleHat1
  {
    get => this.GetElement<IControllerTemplateHat>(181);
  }

  IControllerTemplateHat IHOTASTemplate.throttleHat2
  {
    get => this.GetElement<IControllerTemplateHat>(182);
  }

  IControllerTemplateHat IHOTASTemplate.throttleHat3
  {
    get => this.GetElement<IControllerTemplateHat>(183);
  }

  IControllerTemplateHat IHOTASTemplate.throttleHat4
  {
    get => this.GetElement<IControllerTemplateHat>(184);
  }
}
