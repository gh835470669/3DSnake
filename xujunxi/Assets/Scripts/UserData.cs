using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData {
    private int levelNum = 2;
    private int[] levelPoint;
    
    public void setLevelPoint(int p, int l)
    {
        levelPoint[l] = p;
        PlayerPrefs.SetInt("LEVEL" + l.ToString(), p);
    }
    public int getLevelPoint(int l)
    {
        return levelPoint[l];
    }
    4i
    public UserData()
    {
        levelPoint = new int[levelNum];
        for (int i = 0; i < levelNum; i++)
        {
            levelPoint[i] = PlayerPrefs.GetInt("LEVEL" + i.ToString(), 0);
        }
    }
}
