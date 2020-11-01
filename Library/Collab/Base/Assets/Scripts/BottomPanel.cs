using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomPanel : MonoBehaviour
{

    void Awake()
    {
        GameObject.Find("NextDay").GetComponent<Button>().interactable = false; // Отключаем центральную кнопку
        GameObject.Find("NextDayc").GetComponent<Animator>().SetBool("opened", true); // Запускаем анимацию
    }
    public void Button(GameObject button) // Метод для всех кнопок в нижней панели экрана. Получаем обьект кнопки, что бы получить его имя
    {
        GameObject oneofpanels = GameObject.FindGameObjectWithTag("Oneofpanels"); // Находим ранее запущенную активную панель
        oneofpanels.SetActive(false); // Закрыть ранее активную панель для оптимизации
        GameObject.Find(oneofpanels.name).GetComponent<Button>().interactable = true; // Активируем кнопку прошлой панели
        GameObject.FindGameObjectWithTag("mainScript").GetComponent<Main>().allPanels.transform.Find(button.name).gameObject.SetActive(true); // Находим дочерний обьект внутри allPanels с именем обьекта, совпадающий с именем кнопки
        button.GetComponent<Button>().interactable = false; // Отключаем кнопку выбраной панели
        GameObject.Find(oneofpanels.name + "c").GetComponent<Animator>().SetBool("opened", false); // Анимация 1
        GameObject.Find(button.name + "c").GetComponent<Animator>().SetBool("opened", true); // Анимация 2
    }
}
