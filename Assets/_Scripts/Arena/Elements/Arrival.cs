using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrival : MonoBehaviour
{
    public PlayerHitbox Player;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != Player.gameObject) return;

        GameManager.LockedControls = true;
        GameManager.IsScrolling = false;
        Player.GetComponentInParent<PlayerController>().Rb.constraints = RigidbodyConstraints2D.FreezeAll;
        GameManager.PlayerWin = true;
    }
}
