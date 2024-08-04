using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float Speed = 2.0f;
    public PlayerController Player;
    public GameObject RightWall;
    public GameObject Hand;
    public GameObject BackGround1;
    public GameObject BackGround2;
    [NonSerialized] public bool MoveCamera = false;

    private float _backgroundSize;

    private void Start()
    {
        _backgroundSize = BackGround1.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        if (transform.position.x >= BackGround1.transform.position.x && BackGround1.transform.position.x > BackGround2.transform.position.x)
        {
            BackGround2.transform.position += new Vector3(_backgroundSize * 2,0,0);
        }
        else if (transform.position.x >= BackGround2.transform.position.x && BackGround2.transform.position.x > BackGround1.transform.position.x)
        {
            BackGround1.transform.position += new Vector3(_backgroundSize * 2, 0, 0);
        }

        if (!MoveCamera) return;

        transform.position += Vector3.right * Speed * Time.deltaTime;
        RightWall.transform.position += Vector3.right * Speed * Time.deltaTime;
        Hand.transform.position += Vector3.right * Speed * Time.deltaTime;
    }
}
