using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaField {

    public bool isExist;
    public bool canWalk;
    public Vector3 LocalPos;
    public int[] objects;
    public const int BAN = -1;
    public const int EMPTY = 0;
    public const int SNAKE = 1;
    public const int FOOD = 2;

    public MetaField()
    {
        isExist = false;
        canWalk = false;
        LocalPos = Vector3.zero;
        objects = new int[6];
        for (int i = 0; i < 6; i++)
        {
            objects[i] = BAN;
        }
    }

    static public Vector3 objectIndexToDir(int index)
    {
        if (index == 0)
        {
            return Vector3.right;
        }
        if (index == 1)
        {
            return Vector3.left;
        }
        if (index == 2)
        {
            return Vector3.up;
        }
        if (index == 3)
        {
            return Vector3.down;
        }
        if (index == 4)
        {
            return Vector3.forward;
        }
        if (index == 5)
        {
            return Vector3.back;
        }
        return Vector3.zero;
    }

    static public int objectDirToIndex(Vector3 dir)
    {
        if (dir == Vector3.right)
        {
            return 0;
        }
        if (dir == Vector3.left)
        {
            return 1;
        }
        if (dir == Vector3.up)
        {
            return 2;
        }
        if (dir == Vector3.down)
        {
            return 3;
        }
        if (dir == Vector3.forward)
        {
            return 4;
        }
        if (dir == Vector3.back)
        {
            return 5;
        }
        return -1;
    }

    public int objectInDir(Vector3 dir)
    {
        int i = objectDirToIndex(dir);
        if (i != -1)
            return objects[i];
        else
            return BAN;
    }

    public void setObject(Vector3 dir, int _object)
    {
        int i = objectDirToIndex(dir);
        if (i != -1)
        {
            objects[i] = _object;
        }
        else
            return;
    }

    public void setObject(int index, int _object)
    {
        if (index >= 0 && index < 6)
        {
            objects[index] = _object;
        }
        else
            return;
    }
}
