using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerCounter : MonoBehaviour
{
    public UnityEngine.UI.Slider MySlider;
    public TextMeshProUGUI MyText;
    public void Update()
    {
        MyText.text = MySlider.value.ToString();
    }
}
