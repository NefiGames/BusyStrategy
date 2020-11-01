using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using static TMPro.TMP_Dropdown;
using Random = UnityEngine.Random;
using System.Linq;
using NaughtyAttributes;

public class Main : MonoBehaviour
{
    [Tooltip("Все цвета, используемые в коде")] [SerializeField] private Color[] colors = new Color[7];

    private static Color[] color;

    public GameObject allPanels;

    private static int cash;
    public static int Cash { get => cash; set { } }
    private static int day;
    public static int Day { get => day; set { } }

    public static Color GetColor(int index)
    {
        try
        {
            return color[index];
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Debug.LogError(ex + " Индекс вышел за границы массива");
            return UnityEngine.Color.black;
        }
    }

    private void Awake()
    {
        color = colors;
        allPanels.transform.Find("NextDay").gameObject.SetActive(true); // Находим среди дочерних обьектов панель "NextDay", и делаем активной
        
    }
}
