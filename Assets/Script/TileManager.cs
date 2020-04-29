using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileManager : MonoBehaviour
{
    Camera camera;
    Transform target;

    Queue<GameObject> qObjs = new Queue<GameObject>();
    public GameObject[] tiles = new GameObject[3];
    public GameObject[] objects = new GameObject[3];

    GameObject tile;
    GameObject obj;


    public void Setting()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    public void CreateTile(CreationInfo block)
    {
        switch (block.type)
        {
            case TILE.TYPE.FLOOR1:
                tile = Instantiate<GameObject>(tiles[0], block.pos, new Quaternion (0, 0, 0, 0), transform);
                break;
            case TILE.TYPE.FLOOR2:
                tile = Instantiate<GameObject>(tiles[1], block.pos, new Quaternion(0, 0, 0, 0), transform);
                break;
            case TILE.TYPE.HOLE:
                tile = null;
                break;
        }
        if (tile){
            tile.transform.position = block.pos + transform.position;
            block.pos = tile.transform.position;
            qObjs.Enqueue(tile);
        }
    }
    public void CreatObjectOnBlock(CreationInfo block)
    {
        if (block.objectType == OBJECT.TYPE.NONE) return;

        switch (block.objectType)
        {
            case OBJECT.TYPE.GOAL:
                obj = Instantiate<GameObject>(objects[0], block.objectPos, new Quaternion(0, 0, 0, 0), transform);
                break;
            case OBJECT.TYPE.STAR:
                obj = Instantiate<GameObject>(objects[1], block.objectPos, Quaternion.Euler(block.objectRot), transform);
                break;
            case OBJECT.TYPE.TRAP:
                obj = Instantiate<GameObject>(objects[2], block.objectPos, new Quaternion(0, 0, 0, 0), transform);
                break;
        }
        if (obj)
        {
            obj.transform.position = block.objectPos + transform.position;
            qObjs.Enqueue(obj);
        }
    }

    private bool IsBetween(float value, float min, float max)
    {
        return ((value > min) && (value < max)) ? true : false;
    }
    public bool IsInsideCamera(Vector3 pos)
    {
        Vector3 viewPos = camera.WorldToViewportPoint(pos);
        return (IsBetween(viewPos.x, -0.3f, 1.3f) && IsBetween(viewPos.y, -0.3f, 1.3f)) ? true : false;
    }
    public void IsKeepObjectsAlive()
    {
        if (IsInsideCamera(qObjs.Peek().transform.position)) return;
        Destroy(qObjs.Dequeue(), 1.0f);
    }


    public GameObject GetFirstTile()
    {
        return qObjs.Peek();
    }
}
