using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class skipButtom : MonoBehaviour
{
    TurnManager turnManager;

    private void Start()
    {
        turnManager = GameObject.FindObjectOfType<TurnManager>();
    }



}
