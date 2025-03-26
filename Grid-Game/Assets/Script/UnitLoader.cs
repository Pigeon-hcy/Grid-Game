using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class UnitLoader : MonoBehaviour
{
    public static UnitLoader Instance; 

    [SerializeField]
    public List<NewUnit> Playersunits = new List<NewUnit>();
    [SerializeField]
    public List<NewUnit> Enemyunits = new List<NewUnit>();

    public GameObject readyButton;
    public bool gameStart;

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

    private void Start()
    {
        readyButton = GameObject.FindGameObjectWithTag("RB");
        Debug.Log(readyButton);

    }

    private void Update()
    {
        if (Playersunits.Count == 5 && gameStart == false)
        {
            SceneManager.LoadScene("SampleScene");
            gameStart = true;   
        }
    }

    public void loadNextScene(string target)
    { 
        SceneManager.LoadScene(target);
    }
}
