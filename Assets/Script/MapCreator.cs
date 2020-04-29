using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TILE
{
    public enum TYPE
    {
        NONE = -1,
        FLOOR1 = 0,
        FLOOR2,
        HOLE,
        NUM, // 블록이 몇 종류인지 나타낸다
    };
};

public class OBJECT
{
    public enum TYPE
    {
        NONE = 0,
        GOAL,
        STAR,
        TRAP,
        NUM, // 블록이 몇 종류인지 나타낸다
    };
};

public class MapCreator : MonoBehaviour
{
    public bool IsMakingTile;
    TileLevelControl levelControl;
    TileManager block;
    public float speed;

    JsonManager<TileLevelData> tld = new JsonManager<TileLevelData>();
    private TileLevelData GetLevelData(int level)
    {
        return tld.list.Find(x => x.level == level);
    }
    public void UpdateLevelData(int level)
    {   
        levelControl.tiledata = GetLevelData(level);
    }
    public int GetMaxTileLevel() { return tld.list.Count; }
    

    public void Setting()
    {
        tld.Load("LevelTileData.json");
        levelControl = this.gameObject.GetComponent<TileLevelControl>();
        block = this.gameObject.GetComponent<TileManager>();
        levelControl.Setting();
        block.Setting();

    }
    public void Initialize()
    {
        UpdateLevelData(0);
        levelControl.Initialize(); 
        block.CreateTile(levelControl.previous);
        block.CreateTile(levelControl.current);
        IsMakingTile = false;
    }
    public void CreateMap()
    {
        if (block.IsInsideCamera(levelControl.latelyPos + transform.position))
        {
            IsMakingTile = levelControl.UpdateTile();
            if (IsMakingTile)
            {
                block.CreateTile(levelControl.current);
                block.CreatObjectOnBlock(levelControl.current);
            }
        }
        block.IsKeepObjectsAlive();
    }
    public void Movement(float playerDir)
    {
        int direction = (int)(playerDir / 90.0f);
        if (direction % 2 == 0) {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        } else //if (direction % 2 == 1)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
    }
    public void GetCurrentPlayerTilePos()
    {
        Vector3 gapTilePos = transform.position + levelControl.latelyPos;
        Vector3 curTilePos = levelControl.latelyPos - gapTilePos;
        levelControl.OnTileProgassSlider((curTilePos.x + curTilePos.z));
    }
}
