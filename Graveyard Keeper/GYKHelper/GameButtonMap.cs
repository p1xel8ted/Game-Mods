using System.Collections.Generic;

namespace GYKHelper;

public static class GameButtonMap
{
    public readonly static Dictionary<GamePadButton, int> Bindings = new()
    {
        {
            GamePadButton.A, 2
        },
        {
            GamePadButton.B,
            3
        },
        {
            GamePadButton.X,
            4
        },
        {
            GamePadButton.Y,
            5
        },
        {
             GamePadButton.LB,
            6
        },
        {
            GamePadButton.RB,
            7
        },
        {
            GamePadButton.Back,
            8
        },
        {
            GamePadButton.Start,
            9
        },
        {
            GamePadButton.DUp,
            10
        },
        {
            GamePadButton.DDown,
            11
        },
        {
            GamePadButton.DLeft,
            12
        },
        {
            GamePadButton.DRight,
            13
        },
        {
            GamePadButton.RT,
            19
        },
        {
            GamePadButton.LT,
            20
        }
    };
}