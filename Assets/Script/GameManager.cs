using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class GameManager : MonoBehaviour
{
    LoadSceneEvent scene_manager;
    ScoreManager score_manager;
    SoundManager sound_manager;
    PlayerControler player_controler;
    MonsterControler monster_controler;
    MapCreator map_manager;


    public int level;
    private int player_max_level;
    private int map_max_level;

    public bool IsPause;
    private float step_timer = 0.0f;

    private bool isEndGame;
    private bool isSaveFinishedGameData;

    public void Setting()
    {
        scene_manager = gameObject.GetComponent<LoadSceneEvent>();
        sound_manager = gameObject.GetComponent<SoundManager>();
        score_manager = gameObject.GetComponent<ScoreManager>();
        map_manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapCreator>();
        player_controler = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControler>();
        monster_controler = GameObject.FindGameObjectWithTag("Enemy").GetComponent<MonsterControler>();

        score_manager.Setting();
        map_manager.Setting();
        player_controler.Setting();
        monster_controler.Setting();
    }
    private void Start()
    {
        Debug.Log("Unity Event Funtion: Start");
        scene_manager.Initialize();
        score_manager.Initialize();
        map_manager.Initialize();
        player_controler.Initialize();
        monster_controler.Initialize();


        if (score_manager.IsDrawScoreCanvas) score_manager.DrawScoreCanvas(false);

        if (!scene_manager.gs.isTutorial)
        {
            scene_manager.gs.isTutorial = true;
            scene_manager.LoadMainTutorialCanvas();
        }
        else
        {
            scene_manager.IsTutorial = false;
            scene_manager.LoadCountDown();
        }

        IsPause = true;
        isEndGame = false;
        isSaveFinishedGameData = false;
        scene_manager.IsPlayCountDown = true;
        level = -1;

        player_max_level = player_controler.GetMaxPlayerLevel();
        map_max_level = map_manager.GetMaxTileLevel();
        Debug.Log("Level Max (player: " + player_max_level + "), (map: " + map_max_level + ")");
    }
    void Update()
    {
        if (!IsRunGame()) Pause();
        else Running();

        UpdateLevel();
        UpdateMap();
        ControlPlayer();
        ControlMonster();
        UpdateScore();
    }


    private void Pause()
    {
        if (IsPause == false)
        {
            Time.timeScale = 0;
            IsPause = true;
            return;
        }
    }
    private void Running()
    {
        if (IsPause == true)
        {
            Time.timeScale = 1;
            IsPause = false;
            return;
        }
    }
    bool IsRunGame()
    {
        if (scene_manager.IsTutorial) return false;
        if (scene_manager.IsReady) return false;
        if (score_manager.IsDrawScoreCanvas) return false;
        return true;
    }
    private bool Ready()
    {
        if (scene_manager.IsPlayCountDown) return true;
        
        UpdateLevel();
        return false;
    }

    private int GetLevel()
    {
        return (int)(this.step_timer / 10.0f);
    }
    private bool UpdateLevel()
    {
        if (GetLevel() == level) return false;
        level = GetLevel();
        player_controler.UpdateLevelData((level < player_max_level) ? level : player_max_level - 1);
        map_manager.UpdateLevelData((level < map_max_level) ? level / 3 : map_max_level - 1);
        map_manager.speed = player_controler.GetSpeed();

        Debug.Log("Current Level: " + level);
        return true;
    }
    private void UpdateMap()
    {
        map_manager.CreateMap();
        map_manager.GetCurrentPlayerTilePos();
        if (!IsRunGame()) return;
        if (Ready()) return;
        if (player_controler.IsMoving()) map_manager.Movement(player_controler.direction.y);
    }
    
    private void ControlPlayer()
    {
        if (!IsRunGame()) return;
        if (Ready()) return;
        player_controler.Movement();
        FinishGame(player_controler.IsDead());
        FinishGame(player_controler.isGoal);
    }
    private void FinishGame(bool isEndGame)
    {
        if (!isEndGame) return;
        Pause();
        player_controler.finishGame();
        map_manager.speed = player_controler.GetSpeed();
        score_manager.SetResultScoreText(player_controler);
    }
    private void ControlMonster()
    {
        if (!IsRunGame()) return;
        if (Ready()) return;
        monster_controler.Movement();
    }
    private void UpdateScore()
    {
        if (!IsRunGame()) return;
        if (Ready()) return;
        score_manager.SetCurentScoreText(player_controler);
        this.step_timer += Time.deltaTime;
    }
}
