using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnitLoader : MonoBehaviour
{
    public static UnitLoader Instance; 

    [SerializeField]
    public List<NewUnit> Playersunits = new List<NewUnit>();
    [SerializeField]
    public List<NewUnit> Enemyunits = new List<NewUnit>();

    [SerializeField]
    GameObject readyButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        { 
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Playersunits.Count == 5 && readyButton != null)
        {
            readyButton.SetActive(true);
        }
    }

    public void loadNextScene(string target)
    { 
        SceneManager.LoadScene(target);
    }
}
