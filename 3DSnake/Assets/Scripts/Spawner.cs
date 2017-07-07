using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public FieldManager fieldManager;
    public GameObject foodPrefab;
    public GameObject icePrefab;
    public int iceNumber = 5;
    private GameObject food;
    private List<GameObject> ice = new List<GameObject>();
    private List<FieldPosition> iceLocalPositions = new List<FieldPosition>();

    // Use this for initialization
    void Start () {
        food = Instantiate(foodPrefab);
        for (int i = 0; i < iceNumber; i ++)
        {
            GameObject _ice = Instantiate(icePrefab);
            ice.Add(_ice);
            iceLocalPositions.Add(new FieldPosition());
            sendIceMessage(_ice, null);
        }
        sendFoodMessage(null);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void sendFoodMessage(FieldPosition old)
    {
        UpdateMessage msg = new UpdateMessage();
        msg._object = MetaField.FOOD;
        msg.obj = food;
        if (old != null)
        {
            msg.oldPos = old;
        }
        msg.isNewRandom = true;
        fieldManager.enqueueMsg(msg);
        print("add food");
    }

    public void sendIceMessage(GameObject _ice, FieldPosition old)
    {
        UpdateMessage msg = new UpdateMessage();
        msg._object = MetaField.ICE;
        msg.obj = _ice;
        if (old != null)
        {
            msg.oldPos = old;
        }
        msg.isNewRandom = true;
        fieldManager.enqueueMsg(msg);
        print("add ice");
    }

    public void sendIceMessage(FieldPosition old) {
        int index = findIceByPosition(old);
        if (index == -1) return;
        sendIceMessage(ice[index], old);
    }

    public void setIcePosition(GameObject _ice, FieldPosition pos)
    {
        int i = 0;
        for (; i < ice.Count; i++)
        {
            if (ice[i].GetInstanceID() == _ice.GetInstanceID())
                break;
        }
        iceLocalPositions[i] = pos;
    }

    private int findIceByPosition(FieldPosition pos)
    {
        for (int i = 0; i < iceLocalPositions.Count; i++)
        {
            if (iceLocalPositions[i].x == pos.x &&
                iceLocalPositions[i].y == pos.y &&
                iceLocalPositions[i].z == pos.z &&
                iceLocalPositions[i].objIndex == pos.objIndex)
                return i;
        }
        return -1;
    }
}
