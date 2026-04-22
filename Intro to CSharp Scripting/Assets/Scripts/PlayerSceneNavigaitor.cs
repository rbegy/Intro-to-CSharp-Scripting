using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSceneNavagaitor : MonoBehaviour
{
    private Boolean win = false;
    public BoxCollider2D boxCollider;

    private void Start()
    {
        win = false;
        boxCollider.enabled = true;
    }

    private void Update()
    {
        GameEventDispatcher.EnemiesAllDefeated += Exiter_EnemiesAllDefeated;
        if (win)
        {
            boxCollider.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var exiter = other.transform.GetComponent<ExitArea>();



        if (exiter)
        {
            if (win)
            {
                SceneManager.LoadScene(exiter.GetScene());
            }
        }
    }

    private void Exiter_EnemiesAllDefeated()
    {
        win = true;
    }
}
