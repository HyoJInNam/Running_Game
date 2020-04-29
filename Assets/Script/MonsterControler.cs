using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterControler : MonoBehaviour
{
    PlayerControler player;
    float targetDir;
    float speed;
    public void Setting()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControler>();
    }

    public void Initialize()
    {
        targetDir = 0;
        speed = 2.0f;
    }


    public void Movement()
    {
        transform.position = Vector3.Slerp(transform.position,
            (3 - player.damagedCount) * ((player.direction.y == 0)? Vector3.back : Vector3.left), speed * Time.deltaTime);
        transform.LookAt(player.transform);
    }

} 