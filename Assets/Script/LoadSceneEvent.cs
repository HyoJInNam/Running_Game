using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class GameSetting
{
    public bool isTutorial;
    public float music_volum;
    public float sound_volum;

    public GameSetting() { }
    public GameSetting(bool isTutorial,float music_volum, float sound_volum)
    {
        this.isTutorial = isTutorial;
        this.music_volum = music_volum;
        this.sound_volum = sound_volum;
    }
}
public class LoadSceneEvent : MonoBehaviour
{
    public GameObject setting;
    public GameObject tutorial;

    public bool IsTutorial;
    public bool IsReady;

    public bool IsPlayCountDown;
    public GameObject ReadyCountNum;
    private int countDownNum;

    string filename = "GameSettingData.json";
    public GameSetting gs;
    public void Save()
    {
        Debug.Log("Load Game Setting Data:" + gs.isTutorial);
        gs.music_volum = GetComponent<SoundManager>().music.volume;
        gs.sound_volum = GetComponent<SoundManager>().sound.volume;
        File.WriteAllText(Application.dataPath + "/Data/" + filename, JsonUtility.ToJson(gs));
    }
    private void Load()
    {
        string str = File.ReadAllText(Application.dataPath + "/Data/" + filename);
        gs = new GameSetting();
        gs = JsonUtility.FromJson<GameSetting>(str);
        Debug.Log("Load Game Setting Data:" + gs.isTutorial);
    }

    private void Awake()
    {
        Debug.Log("Unity Event Funtion: Awake");
        Load();
        IsTutorial = gs.isTutorial;
        GetComponent<SoundManager>().Setting();
        GetComponent<SoundManager>().Initialize(gs.music_volum, gs.sound_volum);
        if (GetComponent<GameManager>()) GetComponent<GameManager>().Setting();
    }

    public void Initialize()
    {
        IsReady = false;
        IsPlayCountDown = false;
    }

    private void LateUpdate()
    {
        GetComponent<SoundManager>().SoundLateUpdate();
        Save();
    }

    public void LoadeTitleScene()
    {
        SceneManager.LoadScene("Title window");
    }
    public void LoadeRunningGameScene()
    {
        Save();
        SceneManager.LoadScene("Running Game window");
    }
    public void LoadMainSettingCanvas()
    {
        if(IsPlayCountDown)
        {
            ReadyCountNum.SetActive(false);
            countDownNum = 0;
        }
        IsReady = (!IsReady) ? true : false;
        setting.SetActive(IsReady);
    }
    public void LoadMainTutorialCanvas()
    {
        if (IsPlayCountDown)
        {
            ReadyCountNum.SetActive(false);
            countDownNum = 0;
        }
        IsTutorial = (!IsTutorial) ? true : false;
        tutorial.SetActive(IsTutorial);
        if (IsTutorial) gs.isTutorial = true;
    }


    public void LoadCountDown()
    {
        IsPlayCountDown = true;
        ReadyCountNum.SetActive(true);
        countDownNum = 0;
        StartCoroutine(CountDown());
    }
    private IEnumerator CountDown()
    {
        while (countDownNum < 3)
        {
            ReadyCountNum.GetComponent<Text>().text = (3 - (countDownNum++)).ToString();
            yield return new WaitForSeconds(1.0f);
        }

        IsPlayCountDown = false;
        ReadyCountNum.SetActive(false);
        countDownNum = 0;
    }
}
