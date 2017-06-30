//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateMessage
{
    public int _object;
    public GameObject obj;
    public FieldPosition oldPos;
    public FieldPosition newPos;
    public bool isNewRandom = false;
    public float gap;
}

public class FieldManager : MonoBehaviour
{
    public int frame = 0;
    static public int WorldSpeed = 30;
    public GameObject foodPrefab;
    public GameObject cubePrefab;
    public GameObject wholeFieldPrefab;
    private Transform fieldTransform;
    public int len = 7;
    private WholeFiledData fdata;
    public MetaField[,,] field;
    public Vector3 WorldCenter;

    public GameObject food;

    private Queue<UpdateMessage> updateQueue = new Queue<UpdateMessage>();

    //debug
    public List<int> xs = new List<int>();
    public List<int> ys = new List<int>();
    public List<int> zs = new List<int>();
    public List<int> os = new List<int>();

    // Use this for initialization
    void Awake()
    {
        food = Instantiate(foodPrefab);
        GameObject f = Instantiate(wholeFieldPrefab);
        fieldTransform = wholeFieldPrefab.transform.FindChild("Field");
        fdata = fieldTransform.GetComponent<WholeFiledData>();
        //print("fdata");
        //print(fdata.lenX);
        //print(fdata.lenY);
        //print(fdata.lenZ);
        field = new MetaField[fdata.lenX, fdata.lenY, fdata.lenZ];
        WorldCenter = new Vector3(fdata.lenX / 2.0f, fdata.lenY / 2.0f, fdata.lenZ / 2.0f);
        //field = new MetaField[len, len, len];
        //float half = (float)len / 2;
        //WorldCenter = new Vector3(half, half, half);

        for (int i = 0; i < fdata.lenX; i++)
        {
            for (int j = 0; j < fdata.lenY; j++)
            {
                for (int k = 0; k < fdata.lenZ; k++)
                {
                    field[i, j, k] = new MetaField();
                    field[i, j, k].LocalPos = new Vector3(i, j, k);
                    //if (i == 0) setObject(MetaField.EMPTY, new FieldPosition(i, j, k, MetaField.objectDirToIndex(Vector3.left)));
                    //if (i == len - 1) setObject(MetaField.EMPTY, new FieldPosition(i, j, k, MetaField.objectDirToIndex(Vector3.right)));
                    //if (j == 0) setObject(MetaField.EMPTY, new FieldPosition(i, j, k, MetaField.objectDirToIndex(Vector3.down)));
                    //if (j == len - 1) setObject(MetaField.EMPTY, new FieldPosition(i, j, k, MetaField.objectDirToIndex(Vector3.up)));
                    //if (k == 0) setObject(MetaField.EMPTY, new FieldPosition(i, j, k, MetaField.objectDirToIndex(Vector3.back)));
                    //if (k == len - 1) setObject(MetaField.EMPTY, new FieldPosition(i, j, k, MetaField.objectDirToIndex(Vector3.forward)));
                }
            }
        }

        foreach (Transform child in fieldTransform)
        {
            FieldData data = child.GetComponent<FieldData>();
            if (data.isFieldCube)
            {
                MetaField mf = field[(int)child.position.x, (int)child.position.y, (int)child.position.z];
                mf.isExist = true;
                mf.canWalk = data.canWalk;
            }

        }

        for (int i = 0; i < fdata.lenX; i++)
        {
            for (int j = 0; j < fdata.lenY; j++)
            {
                for (int k = 0; k < fdata.lenZ; k++)
                {
                    if (i == 6 && j == 0 && k == 1)
                    {
                        print("6 0 1");
                        print(field[i, j, k].isExist);
                        print(field[i, j, k].canWalk);
                    }
                    
                    if (field[i, j, k].isExist && field[i, j, k].canWalk)
                    {
                        for (int l = 0; l < 6; l++)
                        {
                            Vector3 v = MetaField.objectIndexToDir(l);
                            Vector3 newPos = new Vector3(i, j, k) + v;
                            if (newPos.x == -1 || newPos.x == fdata.lenX || newPos.y == -1 || newPos.y == fdata.lenY || newPos.z == -1 || newPos.z == fdata.lenZ)
                                setObject(MetaField.EMPTY, new FieldPosition(i, j, k, l));
                            else if (field[(int)newPos.x, (int)newPos.y, (int)newPos.z].isExist == false)
                                setObject(MetaField.EMPTY, new FieldPosition(i, j, k, l));
                        }
                        if (i == 6 && j == 0 && k ==1)
                        {
                            print("6 0 1");
                            for (int l = 0; l < 6; l++)
                            {
                                
                                print(field[i, j, k].objects[l]);
                            }
                        }
                    }
                }
            }
        }

        ////custom
        //for (int i = 0; i < len; i++)
        //{
        //    for (int j = 0; j < len; j++)
        //    {
        //        for (int k = 0; k < len; k++)
        //        {
        //            int _i = i;
        //            if (_i > half) _i = len - 1 - _i;
        //            int _j = j;
        //            if (_j > half) _j = len - 1 - _j;
        //            int _k = k;
        //            if (_k > half) _k = len - 1 - _k;
        //            if (_i + _j + _k <= 1) setMetaFieldEmpty(i, j, k);
        //        }
        //    }
        //}

        ////show gameobjects visible
        //for (int i = 0; i < len; i++)
        //{
        //    for (int j = 0; j < len; j++)
        //    {
        //        for (int k = 0; k < len; k++)
        //        {
        //            if (!field[i, j, k].isExist) continue;
        //            GameObject g = Instantiate(cubePrefab, new Vector3(i, j, k), new Quaternion());
        //            g.transform.parent = this.gameObject.transform;
        //        }
        //    }
        //}

    }

    void Start()
    {
        UpdateMessage msg = new UpdateMessage();
        msg._object = MetaField.FOOD;
        msg.obj = food;
        msg.isNewRandom = true;
        enqueueMsg(msg);
        print("add food");
    }

    void Update()
    {
        frame++;
        if (frame == WorldSpeed)
        {
            frame = 0;
            updateObject();
        }
    }

    private void updateObject()
    {
        Queue<UpdateMessage> old = new Queue<UpdateMessage>(updateQueue);
        updateQueue.Clear();
        while (old.Count != 0)
        {
            UpdateMessage msg = old.Dequeue();
            if (msg._object == MetaField.SNAKE)
            {
                //print("snake");
                //print("old: " + msg.oldPos.x + " " + msg.oldPos.y + " " + msg.oldPos.z);
                //print("new: " + msg.newPos.x + " " + msg.newPos.y + " " + msg.newPos.z);
            }

            if (msg.oldPos != null)
            {
                setObject(MetaField.EMPTY, msg.oldPos);
            }

            if (msg.newPos != null)
            {
                msg.obj.transform.position = localPosToWorld(msg.newPos, 0.5f);
                setObject(msg._object, msg.newPos);
            }
            else if (msg.isNewRandom == true)
            {
                List<FieldPosition> fs = new List<FieldPosition>();
                xs.Clear();
                ys.Clear();
                zs.Clear();
                os.Clear();
                for (int i = 0; i < fdata.lenX; i++)
                {
                    for (int j = 0; j < fdata.lenY; j++)
                    {
                        for (int k = 0; k < fdata.lenZ; k++)
                        {
                            for (int l = 0; l < 6; l++)
                            {
                                if (field[i, j, k].isExist == true && field[i, j, k].objects[l] == MetaField.EMPTY)
                                {
                                    fs.Add(new FieldPosition(i, j, k, l));
                                    xs.Add(i);
                                    ys.Add(j);
                                    zs.Add(k);
                                    os.Add(l);
                                }

                            }
                        }
                    }
                }
                int p = UnityEngine.Random.Range(0, fs.Count);
                print("snake: " + fs[p].x + " " + fs[p].y + " " + fs[p].z + " " + MetaField.objectIndexToDir(fs[p].objIndex));
                msg.obj.transform.position = localPosToWorld(fs[p], 0.5f);
                msg.obj.transform.up = MetaField.objectIndexToDir(fs[p].objIndex);
                setObject(msg._object, fs[p]);
            }


        }
    }

    private void setMetaFieldEmpty(int i, int j, int k)
    {
        field[i, j, k].isExist = false;
        //no matter if the neighbor is existing
        if (i != 0) setObject(MetaField.EMPTY, new FieldPosition(i - 1, j, k, MetaField.objectDirToIndex(Vector3.right)));
        if (i != len - 1) setObject(MetaField.EMPTY, new FieldPosition(i + 1, j, k, MetaField.objectDirToIndex(Vector3.left)));
        if (j != 0) setObject(MetaField.EMPTY, new FieldPosition(i, j - 1, k, MetaField.objectDirToIndex(Vector3.up)));
        if (j != len - 1) setObject(MetaField.EMPTY, new FieldPosition(i, j + 1, k, MetaField.objectDirToIndex(Vector3.down)));
        if (k != 0) setObject(MetaField.EMPTY, new FieldPosition(i, j, k - 1, MetaField.objectDirToIndex(Vector3.forward)));
        if (k != len - 1) setObject(MetaField.EMPTY, new FieldPosition(i, j, k + 1, MetaField.objectDirToIndex(Vector3.back)));
    }

    public Vector3 localPosToWorld(FieldPosition pos, float gap)
    {
        Vector3 x = Vector3.right * pos.x;
        Vector3 y = Vector3.up * pos.y;
        Vector3 z = Vector3.forward * pos.z;
        Vector3 objectDir = MetaField.objectIndexToDir(pos.objIndex);
        return x + y + z + gap * objectDir;
    }

    //next[0] : next-up
    //next[1] : next
    public void nextPos(FieldPosition headPositionInField, Vector3 forwardDir, Vector3 upDir, out MetaField[] next)
    {
        next = new MetaField[2];
        int next_x = headPositionInField.x + (int)forwardDir.x;
        int next_y = headPositionInField.y + (int)forwardDir.y;
        int next_z = headPositionInField.z + (int)forwardDir.z;
        if (next_x < 0 || next_x >= fdata.lenX || next_y < 0 || next_y >= fdata.lenY || next_z < 0 || next_z >= fdata.lenZ)
        {
            next[0] = null;
            next[1] = null;
            return;
        }

        next[1] = field[next_x, next_y, next_z];

        next_x += (int)upDir.x;
        next_y += (int)upDir.y;
        next_z += (int)upDir.z;

        if (next_x < 0 || next_x >= fdata.lenX || next_y < 0 || next_y >= fdata.lenY || next_z < 0 || next_z >= fdata.lenZ)
        {
            next[0] = null;
            return;
        }

        next[0] = field[next_x, next_y, next_z];
    }

    public FieldPosition getAnEmptyPosition()
    {

        return new FieldPosition();
    }

    public void setObject(int _object, FieldPosition f)
    {
        field[f.x, f.y, f.z].setObject(f.objIndex, _object);
    }

    public int getObject(FieldPosition f)
    {
        return getObject(f.x, f.y, f.z, f.objIndex);
    }

    public int getObject(int x, int y, int z, int objectIndex)
    {
        return field[x, y, z].objects[objectIndex];
    }

    public void enqueueMsg(UpdateMessage msg)
    {
        updateQueue.Enqueue(msg);
        //print(updateQueue.Count);
    }
}
