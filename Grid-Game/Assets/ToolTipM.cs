using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipM : MonoBehaviour
{
    [SerializeField]
    TMP_Text UIname;
    [SerializeField]
    Image UIImage;
    [SerializeField]
    GameObject GOImage;
    [SerializeField]
    TMP_Text des;

    private void Start()
    {
        UIname.text = null;
        UIImage.sprite = null;
        GOImage.SetActive(false);
        des.text = null; 
    }

    public void changeTheDes(string Uname, Sprite Image, string Udes)
    {
        if (Udes != null)
        {
            string originalText = Udes;
            string newText = originalText.Replace(".", ".\n");
            des.text = newText;
        }
        GOImage.SetActive(true);
        UIname.text = Uname;
        UIImage.sprite = Image;
    }
}
