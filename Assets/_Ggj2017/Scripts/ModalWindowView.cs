using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


[System.Serializable]
public class UnityEventBoolean : UnityEvent<bool> { }

public class ModalWindowView : MonoBehaviour
{
    public Text titleLabel;
    public Text questionLabel;

    public Button[] buttons;
}