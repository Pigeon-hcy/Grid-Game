using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class unitChoose : MonoBehaviour
{
    
    [SerializeField]
    UnitLoader unitLoader;
    [SerializeField]
    NewUnit unit;

    
    private void Start()
    {
        unitLoader = GameObject.FindGameObjectWithTag("Loader").GetComponent<UnitLoader>();
    }
    
    
    public void addPlayerUnit()
    {
        if (unitLoader.Playersunits.Count < 5)
        {
            unitLoader.Playersunits.Add(unit);
        }
    }

}
