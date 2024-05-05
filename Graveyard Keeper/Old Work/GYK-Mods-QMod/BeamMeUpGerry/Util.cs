using UnityEngine;

namespace BeamMeUpGerry;

internal static class Util
{
    public static GDPoint FindNearestGdPoint()
    {
        var pos = MainGame.me.player_pos;
        GDPoint point = null;
        var num2 = float.PositiveInfinity;
        foreach (var p in WorldMap.gd_points)
        {
            if (!(p == null) && p.node != null)
            {
                var sqrMagnitude = (pos - p.pos).sqrMagnitude;
                if (sqrMagnitude < num2)
                {
                    point = p;
                    num2 = sqrMagnitude;
                }
            }
        }

        if (point == null)
        {
            foreach (var p in WorldMap.gd_points)
            {
                Debug.Log("GD point " + p.name + ", area = " + p.node.Area, p);
            }

            point = new GDPoint
            {
                name = "NULL",
                transform =
                {
                    position = Vector3.zero
                }
            };
        }

        var array = new string[6];
        array[0] = "Nearest GD point: ";
        array[1] = point.name;
        array[2] = ", pos = ";
        array[3] = point.transform.position.ToString();
        array[4] = ", obj_pos = ";
        const int num3 = 5;
        var vector = pos;
        array[num3] = vector.ToString();
        return point;
    }
}