using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct CreationInfo
{
    public TILE.TYPE type; 
    public int maxCount;
    public Vector3 pos;
    public OBJECT.TYPE objectType;
    public Vector3 objectPos;
    public Vector3 objectRot;
};
[System.Serializable]
public class TileLevelData
{
    public int level;
    public int floor1_max;
    public int floor2_max;
    public int trap_max;
    public int hole_max;

    public TileLevelData(int level, int floor1_max, int floor2_max, int trap_max, int hole_max)
    {
        this.level = level;
        this.floor1_max = floor1_max;
        this.floor2_max = floor2_max;
        this.trap_max = trap_max;
        this.hole_max = hole_max;
    }
}

public class TileLevelControl : MonoBehaviour
{
    public CreationInfo previous; // 이전에 어떤 블록을 만들었는가.
    public CreationInfo current; // 지금 어떤 블록을 만들어야 하는가.
    public CreationInfo next; // 다음에 어떤 블록을 만들어야 하는가.

    public TileLevelData tiledata;
    public Vector3 latelyPos;
    private Vector3 StraightDir;
    private bool isStraight;
    private bool isLast;
    private int noviceLevel;

    private int maxCount;
    private int curCount;
    public Slider progressSilder;

    public void Setting()
    {
        progressSilder = GameObject.FindGameObjectWithTag("GameManager").gameObject.transform.Find("game").gameObject.transform.Find("ProgressSlider").GetComponent<Slider>();
    }
    public void Initialize()
    {
        maxCount = 300;
        curCount = 0;
        noviceLevel = 3;
        latelyPos = new Vector3(0, 0, -1);

        previous.type = TILE.TYPE.FLOOR1;
        previous.pos = latelyPos;
        latelyPos.z++;

        current.type = TILE.TYPE.FLOOR1;
        current.pos = latelyPos;
        latelyPos.z++;

        next.type = TILE.TYPE.FLOOR1;
        next.pos = latelyPos;
    }
    public bool UpdateTile()
    {
        if (curCount < maxCount)
        {
            GetStraightDir();
            previous = current;
            current = next;
            ClearNextBlock(ref next, current);
            CreatObjectOnBlock(ref next, current);
            curCount += 1;
            return true;
        }
        return false; 
    }

    private void ClearNextBlock(ref CreationInfo current, CreationInfo previous)
    {
        int random_num = Random.Range(0, 1024) % 100;
        
        if (random_num < tiledata.hole_max)
        {
            current.type = ((previous.type != TILE.TYPE.HOLE) && (previous.objectType != OBJECT.TYPE.TRAP))? TILE.TYPE.HOLE : TILE.TYPE.FLOOR2;
            isStraight = true;
        }
        else if (random_num < tiledata.floor2_max)
        {
            current.type = TILE.TYPE.FLOOR2;
            isStraight = ((previous.type == TILE.TYPE.HOLE) || (previous.objectType == OBJECT.TYPE.TRAP)) ? true : false;
        }
        else
        {
            current.type = TILE.TYPE.FLOOR1;
            isStraight = ((previous.type == TILE.TYPE.HOLE) || (previous.objectType == OBJECT.TYPE.TRAP))? true : false;
        }

        isStraight = ((current.type == TILE.TYPE.HOLE) || (previous.type == TILE.TYPE.HOLE) || (previous.objectType == OBJECT.TYPE.TRAP)) ? true : false;

        if (isStraight)
        {
            latelyPos += StraightDir;
            current.pos = latelyPos;
        }
        else current.pos = RandomPosition(random_num * 3);
    }
    private void CreatObjectOnBlock(ref CreationInfo current, CreationInfo previous)
    {
        current.objectPos = current.pos;
        current.objectRot = Vector3.zero;

        if (curCount == (maxCount-2))
        {
            current.objectType = OBJECT.TYPE.GOAL;
            return;
        }
        else if (curCount % (maxCount / 3) == (maxCount / 6))
        {
            current.objectType = OBJECT.TYPE.STAR;
            Vector3 dir = current.pos - previous.pos;
            current.objectRot = new Vector3(20, ((dir == Vector3.forward) ? 0 : 90), 0);
            return;
        }
        else
        {
            int random_num = Random.Range(0, 1024) % 100;
            if (random_num < tiledata.trap_max)
            {
                current.objectType = ((previous.type != TILE.TYPE.HOLE) && (previous.objectType != OBJECT.TYPE.TRAP)) ? OBJECT.TYPE.TRAP : OBJECT.TYPE.NONE;
                return;
            }
            else current.objectType = OBJECT.TYPE.NONE; 
        }        
    }
    void GetStraightDir()
    {
        Vector3 dir = next.pos - previous.pos;
        if (dir.x == 2)
        {
            StraightDir = Vector3.right;
        }
        else if (dir.z == 2)
        {
            StraightDir = Vector3.forward;
        }
        else StraightDir = dir - (current.pos - previous.pos);
    }
    private Vector3 RandomPosition(float number)
    {
        //초보자 레벨
        if(tiledata.level < noviceLevel)
        {
            if (curCount % (noviceLevel*2) < noviceLevel) latelyPos.z++; 
            else latelyPos.x++;
            return latelyPos;
        }
        if (number % 16 < 7) latelyPos.z++;
        else latelyPos.x++;
        return latelyPos;
    }

    public void OnTileProgassSlider(float cur)
    {
        progressSilder.value = cur / maxCount;
    }
}
