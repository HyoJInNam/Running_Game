using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset = Vector3.zero;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        this.offset = this.transform.position - this.player.transform.position;
    }

    void LateUpdate()
    {
        Vector3 new_position = this.transform.position;
        this.transform.position = this.player.transform.position + this.offset;
    }
}
