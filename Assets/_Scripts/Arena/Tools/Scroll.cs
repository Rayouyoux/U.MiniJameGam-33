using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    public void Move()
    {
        transform.position += Vector3.right * GameManager.ScrollSpeed * Time.deltaTime;
    }
}
