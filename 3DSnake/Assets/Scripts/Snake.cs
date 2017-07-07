using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FieldPosition
{
    public int x;
    public int y;
    public int z;
    public int objIndex;

    public FieldPosition(int _x = 0, int _y = 0, int _z = 0, int _objIndex = 0)
    {
        x = _x;
        y = _y;
        z = _z;
        objIndex = _objIndex;
    }
}

public class Snake : MonoBehaviour {
    public int len = 1; //init len had better be 1, so you only consider the position and rotation of head, no considering bodies
    public Vector3 initHeadPosition;
    public Vector3 initHeadRotation;
    public int speed = 10;
    public int maxHealth = 10;
    public int health;
    public GameObject head;
    public GameObject bodyPrefab;
    public RectTransform healthBar;
    public Chores chores;
    private List<GameObject> bodies = new List<GameObject>();
    private List<FieldPosition> bodies_pos = new List<FieldPosition>();

    private FieldManager fieldManager;
    public Spawner spawner;

    private FieldPosition headPositionInField;

    // Use this for initialization
    void Start () {
        if (fieldManager == null)
        {
            fieldManager = FindObjectOfType<FieldManager>();
        }

        head.transform.rotation = Quaternion.Euler(initHeadRotation.x, initHeadRotation.y, initHeadRotation.z);
        headPositionInField = new FieldPosition((int)initHeadPosition.x, (int)initHeadPosition.y, (int)initHeadPosition.z, MetaField.objectDirToIndex(head.transform.up));
        setHeadInWorld(headPositionInField);
        
        initBodies();
        health = maxHealth;
    }

    private void initBodies()
    {
        for (int i = 0; i < len - 1; i++)
        {
            FieldPosition pos = new FieldPosition();
            pos.x = headPositionInField.x - (int)head.transform.forward.x;
            pos.y = headPositionInField.y - (int)head.transform.forward.y;
            pos.z = headPositionInField.z - (int)head.transform.forward.z;
            pos.objIndex = headPositionInField.objIndex;
            GameObject b = Instantiate(bodyPrefab, head.transform.position - head.transform.forward, head.transform.rotation);
            fieldManager.setObject(MetaField.SNAKE, pos);
            bodies.Add(b);
            bodies_pos.Add(pos);
        }
    }

    public void move(Vector3 forwardDir)
    {
        MetaField[] next;
        if (forwardDir == -head.transform.forward)
        {
            return;
        }
        fieldManager.nextPos(headPositionInField, forwardDir, head.transform.up, out next);

        FieldPosition n;
        Quaternion oldRotation = head.transform.rotation;

        if (forwardDir == head.transform.forward)
            head.transform.Rotate(0, 0, 0, Space.Self);
        else if (forwardDir == head.transform.right)
            head.transform.Rotate(0, 90, 0, Space.Self);
        else if (forwardDir == -head.transform.right)
            head.transform.Rotate(0, -90, 0, Space.Self);


        if (next[0] == null || next[0].isExist == false)
        {
            if (next[1] == null || next[1].isExist == false)
            {
                n = new FieldPosition(headPositionInField.x, headPositionInField.y , headPositionInField.z,
                    MetaField.objectDirToIndex(forwardDir));
                if (fieldManager.getObject(n) == MetaField.BAN) return;
                head.transform.Rotate(90, 0, 0, Space.Self);

            }
            else
            {
                n = new FieldPosition((int)next[1].LocalPos.x, (int)next[1].LocalPos.y,(int)next[1].LocalPos.z,
                    MetaField.objectDirToIndex(head.transform.up));
                if (fieldManager.getObject(n) == MetaField.BAN) return;
                head.transform.Rotate(0, 0, 0, Space.Self);
            }

        }
        else
        {
            n = new FieldPosition ((int)next[0].LocalPos.x, (int)next[0].LocalPos.y,(int)next[0].LocalPos.z,
                    MetaField.objectDirToIndex(-forwardDir));
            if (fieldManager.getObject(n) == MetaField.BAN) return;
            head.transform.Rotate(-90, 0, 0, Space.Self);
        }
        Quaternion newRotation = head.transform.rotation;

        bool isAddBody = false;
        //judge the obj in n here
        if (fieldManager.getObject(n) == MetaField.FOOD)
        {
            print("meet food");
            spawner.sendFoodMessage(n);
            //UpdateMessage msg = new UpdateMessage();
            //msg._object = MetaField.FOOD;
            //msg.obj = fieldManager.food;
            //msg.oldPos = n;
            //msg.isNewRandom = true;
            //fieldManager.enqueueMsg(msg);
            isAddBody = true;
            changeHealth(1);
            chores.changePoint(1);

        }
        else if (fieldManager.getObject(n) == MetaField.SNAKE)
        {
            SceneManager.LoadScene("gameOver");
        }
        else if (fieldManager.getObject(n) == MetaField.ICE)
        {
            print("meet ice");
            spawner.sendIceMessage(n);
            changeHealth(-2);
            chores.changePoint(-1);
        }

        //end

        updateSnake(n, oldRotation, newRotation, isAddBody);
    }

    private void updateSnake(FieldPosition nextHeadPosition, Quaternion oldHeadrotation, Quaternion newHeadrotation, bool isAddBody)
    {
        head.transform.rotation = oldHeadrotation;

        var msgs = new List<UpdateMessage>();
        //var q_rot = new Queue<Quaternion>();

        UpdateMessage msg = new UpdateMessage();
        msg.obj = head;
        msg._object = MetaField.SNAKE;
        msg.oldPos = headPositionInField;
        msgs.Add(msg);
        //q_rot.Enqueue(oldHeadrotation);
        for (int i = 0; i < bodies.Count; i++)
        {
            UpdateMessage msgb = new UpdateMessage();
            msgb.obj = bodies[i];
            msgb._object = MetaField.SNAKE;
            msgb.oldPos = bodies_pos[i];
            msgs.Add(msgb);
            //q_rot.Enqueue(bodies[i].transform.rotation);
        }
        if (isAddBody)
        {
            print("add body");
            GameObject newBody = Instantiate(bodyPrefab, new Vector3(100, 100, 100), new Quaternion());
            bodies.Add(newBody);
            bodies_pos.Add(new FieldPosition());
            len++;
            UpdateMessage msgb = new UpdateMessage();
            msgb.obj = newBody;
            msgb._object = MetaField.SNAKE;
            msgs.Add(msgb);
        }

        msgs[0].newPos = nextHeadPosition;
        msgs[0].newRotation = newHeadrotation;
        headPositionInField = nextHeadPosition;
        for (int i = 1; i < bodies.Count + 1; i++)
        {
            msgs[i].newPos = msgs[i - 1].oldPos;
            //bodies[i].transform.rotation = q_rot.Dequeue();
            bodies_pos[i - 1] = msgs[i - 1].oldPos;
            if (i == 1) msgs[i].newRotation = oldHeadrotation;
            else msgs[i].newRotation = bodies[i - 2].transform.rotation;
        }

        for (int i = 0; i < msgs.Count; i++)
            fieldManager.enqueueMsg(msgs[i]);
    }

    private void setHeadInWorld(FieldPosition localPos)
    {
        head.transform.position = fieldManager.localPosToWorld(localPos, 0.5f);
        fieldManager.setObject(MetaField.SNAKE, localPos);
    }

    void changeHealth(int deltaHealth)
    {
        health += deltaHealth;
        healthBar.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, (float)health / maxHealth * 100);
        if (health <= 0)
            SceneManager.LoadScene("gameover");
    }
}
