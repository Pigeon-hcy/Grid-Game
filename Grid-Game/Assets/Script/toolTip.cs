using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class toolTip : MonoBehaviour
{
    [SerializeField]
    Unit unit;
    float Timer = 2f;

    private void Start()
    {
        unit = GetComponent<Unit>();
    }


    private void Update()
    {
       
    }



}
