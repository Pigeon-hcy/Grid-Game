using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIToolTip : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    ToolTipM toolM;
    [SerializeField]
    Sprite image;
    [SerializeField]
    string Unit_name;
    [SerializeField]
    string description;

    public void OnPointerEnter(PointerEventData eventData)
    {
        toolM.changeTheDes(Unit_name, image, description);
    }
}
