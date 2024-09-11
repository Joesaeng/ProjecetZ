using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class JPSNode
{
    public bool walkable;

    public Vector2 WorldPos;

    public int gridX;
    public int gridY;

    public int H { get; private set; }
    public int G { get; private set; }
    public int F => H + G;

    public bool IsOpen { get; set; }
    public bool IsClose { get; set; }

    public bool IsObstacle { get; set; }

    public JPSNode Parent { get; set; }

    public JPSNode(bool walkable, Vector2 worldPos, int x, int y)
    {
        this.walkable = walkable;
        WorldPos = worldPos;
        gridX = x;
        gridY = y;
    }
}

public enum Direct
{
    Start = -1,
    Left,
    Right,
    Up,
    Down,
    End = 4,
}

public enum DiagonalDirect
{
    Start = -1,
    LeftUp,
    RightUp,
    LeftDown,
    RightDown,
    End = 4,
}

public class JPSPathFinder : MonoBehaviour
{
    readonly int[] dtX = {0,0,-1,1 };
    readonly int[] dtY = {1,-1,0,0};
    readonly bool[] dirOpen = {false,false,false,false};

    readonly int[] dgX = {-1,1,-1,1};
    readonly int[] dgY = {1,1,-1,-1};
    readonly (int, int)[] dgB =
    {
        ((int)Direct.Left, (int)Direct.Up),
        ((int)Direct.Right, (int)Direct.Up),
        ((int)Direct.Left, (int)Direct.Down),
        ((int)Direct.Right, (int)Direct.Down),
    };

    private List<JPSNode> nearNodeResult = new();

    private List<JPSNode> FindNearNode(JPSNode curNode)
    {
        nearNodeResult.Clear();

        Vector2 curPos = curNode.WorldPos;

        for(Direct i = Direct.Start +1; i < Direct.End; i++)
        {
            int index = (int)i;
            int x = curNode.gridX + dtX[index];
            int y = curNode.gridY + dtY[index];
            
            Vector2 pos = new Vector2(curPos.x + dtX[index],curPos.y + dtY[index]);

            // dirOpen[index] = IsOpenableNode(x, y);
        }

        return nearNodeResult;
    }

    // private bool IsOpenableNode(int x, int y)
    // {
    //     
    // }
}
