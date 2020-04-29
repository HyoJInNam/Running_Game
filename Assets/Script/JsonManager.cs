using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;





[System.Serializable]
public class Serializtion<T>
{
    [SerializeField]
    List<T> target;
    public List<T> ToList() { return target; }
    public Serializtion(List<T> target)
    {
        this.target = target;
    }
}

public class JsonManager<T>
{
    public List<T> list = new List<T>();
    public void Save(string filename)
    {
        File.WriteAllText(Application.dataPath + "/Data/" + filename,
            JsonUtility.ToJson(new Serializtion<T>(list)));
    }

    public void Load(string filename)
    {
        string str = File.ReadAllText(Application.dataPath + "/Data/" + filename);
        list = JsonUtility.FromJson<Serializtion<T>>(str).ToList();
        //foreach (T data in list) Debug.Log(data);
    }
};