/*
Help with Design: NikitaKartinki, RayAksu.
Special thanks: SeaEagle, iUshanka, RayTrake.
*/
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

public class MainS : MonoBehaviour
{
    /// <summary> Пустой обьект, внутри которого все панели. </summary>
    GameObject allPanels;
    /// <summary> Панель выбора плана. </summary> 
    [Tooltip("Панель выбора плана")] [BoxGroup("Окно NextDay")] public GameObject choosePanel;
    /// <summary> Объект, в котором находятся 5 ячеек плана. </summary>
    [Tooltip("Объект, в котором находятся 5 ячеек плана")] [BoxGroup("Окно NextDay")] public GameObject listOfPlans;
    /// <summary> Шкала времени. </summary>
    [Tooltip("Шкала времени")] [BoxGroup("Окно NextDay")] public GameObject timeScale;
    /// <summary> Текстовый объект дней. </summary>
    [Tooltip("Текстовый объект дней")] [BoxGroup("Окно NextDay")] public TextMeshProUGUI day;
    /// <summary> Текстовый объект часов. </summary>
    [Tooltip("Текстовый объект часов")] [BoxGroup("Окно NextDay")] public TextMeshProUGUI hour;
    /// <summary> Панель выбора количества времени на каждый план. </summary>
    [Tooltip("Панель выбора количества времени на каждый план")] [BoxGroup("Окно NextDay")] public GameObject chooseTime;
    /// <summary> Слайдер (в chooseTime). </summary>
    [Tooltip("Слайдер (в chooseTime)")] [BoxGroup("Окно NextDay")] public Slider sliderTime;
    /// <summary> Кнопка ОК в chooseTime. </summary>
    [Tooltip("Кнопка ОК в chooseTime")] [BoxGroup("Окно NextDay")] public Button okButton;
    /// <summary> Текстовый объект, показывающий, сколько времени мы решили потратить на план. </summary>
    [Tooltip("Текстовый объект, показывающий, сколько времени мы решили потратить на план")] [BoxGroup("Окно NextDay")] public Text hourShowText;
    /// <summary> Кнопка *Работать* </summary>
    [Tooltip("Кнопка *Работать*")] [BoxGroup("Окно NextDay")] public GameObject work;
    /// <summary> Префаб элемента статистики </summary>
    [Tooltip("Префаб элемента статистики")] [BoxGroup("Окно WorldStatistics")] public GameObject onestat;
    ///<summary> Панель статистики </summary>
    [Tooltip("Панель статистики")] [BoxGroup("Окно WorldStatistics")] public GameObject statPanel;
    /// <summary> Список компаний </summary>
    [Tooltip("Список компаний")] [BoxGroup("Окно WorldStatistics")] public List<Stats> stats = new List<Stats>();
    /// <summary> Панель создания нового бизнеса. </summary>
    [Tooltip("Панель создания нового бизнеса")] [BoxGroup("Окно NextDay")] public GameObject newBusiness;
    /// <summary> Минимальный бюджет для создания конкретной компании. </summary>
    [Tooltip("Минимальный бюджет для создания конкретной компании")] [BoxGroup("Окно NextDay")] public int[] minBudget = new int[8];
    /// <summary> Временные переменные для создания бизнеса, сохраняет под индексом плана. </summary>
    Saveall.Businesses[] bustemp = new Saveall.Businesses[6];
    /// <summary> Текстовый объект денег в панели ленты новостей. </summary>
    [Tooltip("Текстовый объект денег в панели ленты новостей")] [BoxGroup("Окно Timeline")] public TextMeshProUGUI moneyInTimeline;
    /// <summary> Текст с вашими очками в панели статистики </summary>
    [Tooltip("Текст с вашими очками в панели статистики")] [BoxGroup("Окно WorldStatistics")] public TextMeshProUGUI yourScore;
    /// <summary> Мощность компании игрока. </summary>
    [Tooltip("Мощность компании игрока")] [BoxGroup("Окно WorldStatistics")] public float yourVolume;
    /// <summary> Все цвета, используемые в коде </summary>
    Color[] colors = new Color[7];
    /// <summary> Префаб новостей в ленте </summary>
    [Tooltip("Префаб новостей в ленте")] [BoxGroup("Окно Timeline")] public GameObject NewPrefab;
    /// <summary> Лента новостей </summary>
    [Tooltip("Лента новостей")] [BoxGroup("Окно Timeline")] public GameObject TLContent;
    /// <summary> Класс, сохраняющий всю инфу </summary>
    [Tooltip("Класс, сохраняющий всю инфу")] public Saveall saveall = new Saveall();
    /// <summary> Количество новостей, показаных за раз  </summary>
    int newShowed;
    /// <summary> Воспомогательная переменная для newShowed </summary>
    int newWas;
    /// <summary> ID выбранной кнопки в панели "Необходимости" </summary>
    int needsButtonID;
    /// <summary> 0 index - общие затраты из всех планов, 1-5 - отдельно конкретно для каждого плана. </summary>
    long[] deltaСash = new long[6];
    /// <summary> Вчерашние деньги, для анимации изменения количества в панели таймлайна </summary>
    long lastCash = 0;
    /// <summary> Виды бизнесов </summary>
    List<OptionData> typeOfBusiness = new List<OptionData>();
    /// <summary> Переменная, содержащая в себе все строки, способные на локализацию </summary>
    Translate tr;
    /// <summary> Кнопки возможных планов, которые перестают быть активными при нажатии </summary>
    Button[] choose_button_plan = new Button[6];
    /// <summary> Номер плюсика(выбора плана) по счёту </summary>
    int idplan;
    /// <summary> Заполнение часов (от 0 до 24) </summary>
    int hours;
    /// <summary> Позиция игрока в статистике </summary>
    int yourpos;
    /// <summary> Айди всех планов </summary>
    float[] plans = new float[6];
    /// <summary> Запоминает, какому плану сколько времени выделяется. </summary>
    int[] allhours = new int[6];
    /// <summary> Заработанные деньги </summary>
    int workMoney;
    /// <summary> Текст новостей (h - голод, t - усталость, s - сонливость). </summary>
    string hconfuse, tconfuse, sconfuse;
    /// <summary> Текст кнопки, на которую нажимает игрок при выборе плана. </summary>
    string btext;
    /// <summary> Воспомогательная переменная для оптимизации новостей. Обозначает номер первой новости, которая является активной. </summary>
    int tlnum = 1;
    /// <summary> Ещё одна воспомогательная переменная. Собирает общую высоту окна новостей. </summary>
    float TLHeight;
    /// <summary> Воспомогательная переменная для ленты новостей. Проверяет, как мы попали на ленту новостей, чтобы не было случайно два раза запущенных иенумераторов </summary>
    bool fromNext;
    /// <summary> Слайдер в окне создания нового бизнеса. </summary>
    Slider newBusinessSlider;
    /// <summary> Для ускорения запомианем все новости в ленте, чтобы потом ими пользоваться </summary>
    List<GameObject> allNews = new List<GameObject>();
    /// <summary> Панель TimeLine. </summary>
    GameObject TimeLineObject;
    /// <summary> Шкалы потребностей. </summary>
    public Image[] infoscale;
    /// <summary> Зеленый, желтый, и красные цвета </summary>
    public Color[] gbColor;
    /// <summary> Проверяем, не вызывали ли мы TimeLine, чтобы лишний раз не тратить память на ненужные действия </summary>
    bool TLWasCalled = false;
    /// <summary> Начало игры </summary>

    void Awake()
    {
        allPanels = GameObject.FindGameObjectWithTag("allpanels"); // Находим объект со всеми панелями по тэгу
        TimeLineObject = allPanels.transform.Find("TimeLine").gameObject; // Обозначаем панель TimeLine
        newBusinessSlider = newBusiness.transform.Find("Slider").gameObject.GetComponent<Slider>(); // Обозначаем слайдер
    }
    void Start()
    {
        tr = TranslateManager.tr; // Присваиваем tr всю локализацию из скрипта TranslateManager
        saveall = (PlayerPrefs.HasKey("BusySave")) ? JsonUtility.FromJson<Saveall>(PlayerPrefs.GetString("BusySave")) : new Saveall(); // Если есть сохранение - присвоить, иначе - создать новый экземпляр

        //////////////////////////////////////////////////////////////////////////////////////////
        colors[0] = new Color(0.41f, 0.46f, 0.48f); colors[1] = new Color(0.61f, 0.85f, 0.98f); // Список цветов для планов и для шкалы
        colors[2] = new Color(0.84f, 0.93f, 0.98f); colors[3] = new Color(0.27f, 0.39f, 0.47f); //
        colors[4] = new Color(0.66f, 0.74f, 0.78f); colors[5] = new Color(0.34f, 0.49f, 0.77f); //
        colors[6] = new Color(0.76f, 0.77f, 0.77f);                                             //
        //////////////////////////////////////////////////////////////////////////////////////////
        minBudget[0] = 40000; minBudget[1] = 20000; minBudget[2] = 3000; minBudget[3] = 100000; minBudget[4] = 10000; minBudget[5] = 5000; minBudget[6] = 2000; minBudget[7] = 1000; // Минимальный бюджет для них

        allPanels.transform.Find("NextDay").gameObject.SetActive(true); // Находим среди дочерних обьектов панель "NextDay", и делаем активной
        GameObject.Find("NextDay").GetComponent<Button>().interactable = false; // Отключаем центральную кнопку
        GameObject.Find("NextDayc").GetComponent<Animator>().SetBool("opened", true); // Запускаем анимацию
        saveall.cash = 10000; // Временно
        Shedule();
        Translation(); //Перевод
        RefreshAll(); //Обновляем всё
    }
    ///<summary> Обновляем шкалы голода, сна, и усталости </summary> 
    void InfoRefresh()
    {
        infoscale[0].fillAmount = 0.2f * saveall.hunger;
        infoscale[1].fillAmount = 0.2f * saveall.sleep;
        infoscale[2].fillAmount = 0.2f * saveall.tiredness;
    }

    ///<summary> Обновление ленты новостей после каждого дня </summary> 
    void TimeLineRefresh()
    {
        TLContent.transform.position = new Vector2(TLContent.transform.position.x, 0); // Возвращаем ленту вверх
        for (int i = TLContent.transform.childCount; i < saveall.timeline.Count; i++) // новости пишутся с последней
        {
            var NewTL = Instantiate(NewPrefab, TLContent.transform); // создаём экземпляр новости
            NewTL.transform.SetAsFirstSibling();
            NewTL.transform.Find("DayNumber").GetComponent<TextMeshProUGUI>().text = tr.day + " " + saveall.timeline[i].day; // ставим номер дня
            for (int k = 0; k < saveall.timeline[i].news.Count; k++) // Все новости расписываем
            {
                var logo = Instantiate(NewTL.transform.Find("Logo").gameObject, NewTL.transform); // Спавним новое лого
                logo.transform.SetSiblingIndex(NewTL.transform.childCount - 2); // Ставим его перед картинкой
                logo.SetActive(true); // Делаем активным
                logo.GetComponent<TextMeshProUGUI>().color = saveall.timeline[i].news[k].color;
                logo.GetComponent<TextMeshProUGUI>().text = saveall.timeline[i].news[k].logo; // Пишем локализирвоаный текст - логотип новости.
                var description = Instantiate(NewTL.transform.Find("Description").gameObject, NewTL.transform); // Повторяем те же действия с описанием
                description.SetActive(true);
                description.transform.SetSiblingIndex(NewTL.transform.childCount - 2);
                description.GetComponent<TextMeshProUGUI>().text = saveall.timeline[i].news[k].description;
            }
        }
        if (fromNext)
        {
            TLWasCalled = false;
            StartCoroutine(Kostil());
        }
    }
    ///<summary> Костыль, написанный для оптимизированного ScrollView </summary>
    IEnumerator Kostil() // Вызывается, когда открываем меню таймлайна
    {
        tlnum = 1;
        TLHeight = 0;
        yield return new WaitForSeconds(0.001f);
        if (!TLWasCalled)
        {
            for (int i = allNews.Count; i < TLContent.transform.childCount; i++)
            {
                TLContent.transform.GetChild(i).gameObject.SetActive(true); // Активируем каждую новость
                allNews.Add(TLContent.transform.GetChild(i).gameObject); // Сохраняем в список
            }
            TLWasCalled = true;
            for (int i = 0; i < 2; i++)
            {
                TLContent.GetComponent<ContentSizeFitter>().enabled = !TLContent.GetComponent<ContentSizeFitter>().enabled;
                TLContent.GetComponent<VerticalLayoutGroup>().enabled = !TLContent.GetComponent<VerticalLayoutGroup>().enabled;
                yield return new WaitForSeconds(0.001f);
            }
        }
        TLContent.transform.localPosition = new Vector2(TLContent.transform.localPosition.x, 0);
        TimeLineObject.transform.Find("Scroll View").GetComponent<ScrollRect>().velocity = Vector2.zero;
        if (saveall.timeline.Count > 5)
        {
            StartCoroutine(TLRT());
        }
    }
    ///<summary> Оставляем только 6 новостей </summary> 
    IEnumerator TLRT()
    {
        for (int i = TLContent.transform.childCount - 1; i >= 0; i--) // Сначала деактивируем все нижние новости кроме шести
        {
            TLContent.transform.GetChild(i).gameObject.SetActive((i > 5) ? false : true);
        }
        while (TimeLineObject.activeSelf) // иенумератор повторяется, пока окно TimeLine открыто
        {
            if (TLContent.transform.localPosition.y > (allNews[tlnum - 1].GetComponent<RectTransform>().sizeDelta.y * 3 + TLHeight) && tlnum + 5 <= allNews.Count)
            { // Если позиция контента ниже чем три новости (примерно), и направление движения вниз, то...
                TLHeight += allNews[tlnum].GetComponent<RectTransform>().sizeDelta.y;
                allNews[tlnum - 1].SetActive(false);// отключаем верхнюю новость (tlnum - запоминаем, с какой новости мы оставляем 6 новостей)
                if (tlnum + 5 < allNews.Count)
                    allNews[tlnum + 5].SetActive(true); // Прибавляем
                tlnum++; // прибавляем
            }
            else if (TLContent.transform.localPosition.y < allNews[tlnum - 1].GetComponent<RectTransform>().sizeDelta.y + TLHeight && tlnum > 1)
            { // если позиция контента меньше высоты новости, tlnum больше единицы, и направление вверх, то...
                tlnum--; // отнимаем
                TLHeight -= allNews[tlnum].GetComponent<RectTransform>().sizeDelta.y;
                allNews[tlnum - 1].SetActive(true); // активируем верхнюю
                if (tlnum + 5 < allNews.Count)
                    allNews[tlnum + 5].SetActive(false); // отключаем нижнюю
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    void Translation()
    {
        for (int i = 0; i < 3; i++)
        {
            choosePanel.transform.Find("Needs").Find("Needs").GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = tr.buttons[i];
        }
        choosePanel.transform.Find("Logos").Find("Needs").GetComponent<TextMeshProUGUI>().text = tr.needs;
        choosePanel.transform.Find("Logos").Find("Business").GetComponent<TextMeshProUGUI>().text = tr.business;
        for (int i = 1; i < 6; i++)
        {
            listOfPlans.transform.Find(i + " plan").GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = tr.chooseplan;
        }
        day.text = saveall.day + tr.day;
        hour.text = hours + "/24" + tr.hour[0];
    }

    private void OnApplicationFocus(bool pause)
    {
        if (!pause)
        {
            PlayerPrefs.SetString("BusySave", JsonUtility.ToJson(saveall));
            PlayerPrefs.Save();
        }

    }
    public void Bottom(GameObject but) // Метод для всех кнопок в нижней панели экрана. Получаем обьект кнопки, что бы получить его имя
    {
        GameObject oneofpanels = GameObject.FindGameObjectWithTag("Oneofpanels"); // Находим ранее запущенную панель
        oneofpanels.SetActive(false); // Закрыть ранее активную панель для оптимизации
        GameObject.Find(oneofpanels.name).GetComponent<Button>().interactable = true; // Активируем кнопку прошлой панели
        allPanels.transform.Find(but.name).gameObject.SetActive(true); // Находим дочерний обьект внутри allPanels с именем обьекта, совпадающий с именем кнопки
        switch (but.name)
        {
            case "NextDay":
                for (int i = 1; i < 6; i++)
                {
                    if (plans[i] != 0)
                    {
                        /// <summary> Локальная переменная плана </summary>
                        GameObject xPlan = listOfPlans.transform.Find(i + " plan").gameObject;
                        xPlan.transform.Find("ButtonPlus").localRotation = Quaternion.Euler(0, 0, 45);
                        xPlan.GetComponent<Animator>().SetBool("s", true);
                        xPlan.GetComponent<Animator>().SetBool("Check", true);
                    }
                }
                break;
            case "YourBusinesses":
                if (saveall.businesses.Count == 0)
                {
                    allPanels.transform.Find("YourBusinesses").Find("Scroll View").gameObject.SetActive(false);
                    allPanels.transform.Find("YourBusinesses").Find("Empty").gameObject.SetActive(true);
                }
                else
                {
                    allPanels.transform.Find("YourBusinesses").Find("Scroll View").gameObject.SetActive(true);
                    allPanels.transform.Find("YourBusinesses").Find("Empty").gameObject.SetActive(false);
                }
                break;
            case "TimeLine":
                if (lastCash != saveall.cash)
                    StartCoroutine(MoneyChange());
                if (!fromNext)
                {
                    StartCoroutine(Kostil());
                }
                fromNext = false;
                break;
        }
        but.GetComponent<Button>().interactable = false; // Отключаем кнопку выбраной панели
        GameObject.Find(oneofpanels.name + "c").GetComponent<Animator>().SetBool("opened", false); // Анимация 1
        GameObject.Find(but.name + "c").GetComponent<Animator>().SetBool("opened", true); // Анимация 2
    }

    public void ChooseSection(int id) // Переход между разделами в панели выбора плана
    {
        choosePanel.GetComponent<Animator>().SetInteger("state", 2 - id);
    }

    public void Back() // если нажать на задний фон в панели, то она закроется
    {
        idplan = 0; // Обнуляем переменную
        choosePanel.transform.Find("Business").Find("ChooseShow").gameObject.SetActive(false);
        newBusiness.SetActive(false);
        choosePanel.GetComponent<Animator>().SetInteger("state", 5);
        StartCoroutine(CloseAfterAnim());
    }

    IEnumerator CloseAfterAnim()
    {
        yield return new WaitForSeconds(0.34f);

        choosePanel.SetActive(false); // Выключаем панель

    }

    public void PlusButton(int id) // Кнопка плюсика
    {
        if (listOfPlans.transform.Find(id + " plan").Find("ButtonPlus").localRotation != Quaternion.Euler(0, 0, 45)) // Если эта кнопка не крестик, тогда запустить панель
        {
            idplan = id; // Запоминаем айди кнопки
            choosePanel.transform.Find("money").Find("Money").gameObject.GetComponent<TextMeshProUGUI>().text = MoneyConvert(saveall.cash - deltaСash[0]) + "$";
            choosePanel.transform.Find("money").Find("Delta").gameObject.GetComponent<TextMeshProUGUI>().text = "(" + MoneyConvert(deltaСash[0]) + "$)";
            choosePanel.SetActive(true); // Запускаем панель
            if (hours > 16)
            {
                choosePanel.transform.Find("Business").Find("Scroll View").GetChild(0).GetChild(0).Find("NewBusiness").gameObject.GetComponent<Button>().interactable = false;
            }
            else
            {
                choosePanel.transform.Find("Business").Find("Scroll View").GetChild(0).GetChild(0).Find("NewBusiness").gameObject.GetComponent<Button>().interactable = true;
            }
        }
        else /// TODO
        {
            hours -= allhours[id];
            hour.text = hours + "/24" + tr.hour[0]; // Меняем текст количества часов
            allhours[id] = 0;
            plans[id] = 0;
            choose_button_plan[id].interactable = true;
            deltaСash[0] -= deltaСash[idplan];
            deltaСash[idplan] = 0;
            choose_button_plan[id] = null;
            GameObject plan = listOfPlans.transform.Find(id + " plan").Find("Plan").gameObject; // Находим план
            listOfPlans.transform.Find(id + " plan").gameObject.GetComponent<Animator>().SetBool("Check", false); // Перевернуть крестик в плюсик
            plan.GetComponentInChildren<TextMeshProUGUI>().text = tr.chooseplan; // возвращаем текст...
            plan.GetComponent<Image>().color = colors[5]; // ...и цвет
            ChangeScale();
        }
    }

    public void Choose(GameObject button) // Когда выбрали план
    {
        btext = button.GetComponent<TextMeshProUGUI>().text; // Запоминаем текст кнопки плана
        needsButtonID = (choosePanel.GetComponent<Animator>().GetInteger("state") == 1) ? button.transform.parent.GetSiblingIndex() : 4;
        choose_button_plan[idplan] = button.transform.GetComponentInParent<Button>();
        choose_button_plan[idplan].interactable = false;
        chooseTime.SetActive(true); // Включаем окно выбора времени
        sliderTime.value = 0; // Значение слайдера на 0
        sliderTime.maxValue = (needsButtonID != 1) ? 24 - hours : (saveall.cash < 10000) ? saveall.cash - deltaСash[0] : 10000; // Ограничиваем слайдер
        hourShowText.text = (needsButtonID != 1) ? "0 " + tr.hour[3] : "0$"; // Текст меняем на 0
        okButton.interactable = false;
    }

    public string RUSHourConvert(int hour)
    {
        if (hour == 1)
        {
            return tr.hour[1];
        }
        else if (hour == 2 || hour == 3 || hour == 4)
        {
            return tr.hour[2];
        }
        else return tr.hour[3];
    }

    public void PMSScale(int mp) // Кнопки Плюс и минус
    {
        sliderTime.value += mp; // Значение слайдера меняем
        hourShowText.text = (needsButtonID != 1) ? sliderTime.value + " " + RUSHourConvert((int)sliderTime.value) : sliderTime.value + "$"; // Текст меняем
        if (sliderTime.value != 0)
            okButton.interactable = true;
        else
            okButton.interactable = false;
    }

    public void Slider() // Слайдер
    {
        hourShowText.text = (needsButtonID != 1) ? sliderTime.value + " " + RUSHourConvert((int)sliderTime.value) : sliderTime.value + "$"; // Текст меняем
        if (sliderTime.value != 0)
            okButton.interactable = true;
        else
            okButton.interactable = false;
    }

    public void ChooseBack()
    {
        choose_button_plan[idplan].interactable = true;
        chooseTime.SetActive(false); // Включаем окно выбора времени
    }

    public void OK() // Кнопка в выборе времени
    {
        chooseTime.SetActive(false); // Закрываем окошко
        GameObject plan = listOfPlans.transform.Find(idplan + " plan").Find("Plan").gameObject; // находим, на какой план мы нажимали ранее
        plan.GetComponentInChildren<TextMeshProUGUI>().text = btext; // присваиваем ему текст кнопки
        plans[idplan] = needsButtonID + 1;
        listOfPlans.transform.Find(idplan + " plan").gameObject.GetComponent<Animator>().SetBool("Check", true); // запускаем анимацию плюсик-крестик
        plan.GetComponent<Image>().color = colors[idplan - 1]; // меняем цвет плану
        allhours[idplan] = (needsButtonID != 1) ? (int)sliderTime.value : 3;
        if (needsButtonID == 1)
        {
            deltaСash[idplan] = (int)sliderTime.value;
            deltaСash[0] += deltaСash[idplan];
        }
        hours += allhours[idplan]; // для часов
        ChangeScale();
        hour.text = hours + "/24" + tr.hour[0]; // Меняем текст количества часов
        Back(); // выходим из папки
    }

    void ChangeScale()
    {
        int k = 0;
        for (int i = 1; i <= 5; i++) // Выводим истинное положение шкалы
        {
            for (int l = k; l < (k + allhours[i]); l++)
            {
                timeScale.transform.Find((l + 1).ToString()).gameObject.GetComponent<Image>().color = colors[i - 1]; // Шкала
            }
            k += allhours[i];
        }
        for (int i = k; i < 24; i++)
        {
            timeScale.transform.Find((i + 1).ToString()).gameObject.GetComponent<Image>().color = colors[6]; // Шкала
        }
        for (int i = 1; i <= 5; i++) // Выключаем кнопки, если максимальное количество часов, и включаем, если нет
        {
            if (hours == 24)
            {
                if (allhours[i] == 0)
                {
                    listOfPlans.transform.Find(i + " plan").Find("ButtonPlus").gameObject.GetComponent<Button>().interactable = false;
                }
            }
            else
            {
                listOfPlans.transform.Find(i + " plan").Find("ButtonPlus").gameObject.GetComponent<Button>().interactable = true;
            }
        }
    }

    ///<summary>
    ///Конвертирует число в деньги (к,м,б).
    ///</summary>
    ///<returns>string</returns>
    ///<param name="value">Число</param>
    public static string MoneyConvert(long value)
    {
        return (value < 1000) ? value.ToString() // Если меньше 1к - вернуть просто число
        : (value < 1000000) ? Math.Round(value / 1000f, 1).ToString() + "k" // Меньше миллиона - с буковкой к
        : (value < 1000000000) ? Math.Round(value / 1000000f, 1).ToString() + "m" // меньше миллиарда - с буковкой м
        : Math.Round(value / 1000000000f, 1).ToString() + "b"; // иначе с буковкой б
    }

    IEnumerator MoneyChange()
    {
        float coin = 0;
        while ((lastCash < saveall.cash) ? lastCash < saveall.cash : lastCash > saveall.cash)
        {
            coin += 0.02f;
            lastCash = (long)Mathf.Lerp(lastCash, saveall.cash, coin);
            moneyInTimeline.text = MoneyConvert(lastCash) + "$";
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void YourStats() // Найти твоё место в топе статистики
    {
        if (Mathf.Abs(yourpos - statPanel.transform.childCount) < 8)
        {
            statPanel.transform.localPosition = new Vector3(0, 150 * (statPanel.transform.childCount - 8), 0);
        }
        else
        {
            statPanel.transform.localPosition = new Vector3(0, yourpos * 150, 0);
        }
        statPanel.transform.parent.parent.gameObject.GetComponent<ScrollRect>().velocity = new Vector3(0, 0, 0);
    }

    void StatsShow() // Показать статистику
    {
        float allvolume = 0; // Общая мощность компаний
        float maxvol = 0;
        for (int i = 0; i < statPanel.transform.childCount; i++)
        {
            Destroy(statPanel.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < stats.Count; i++) // Цикл, который считает всю мощность  
        {
            if (stats[i].volume > maxvol)
            {
                maxvol = stats[i].volume;
            }
            allvolume += stats[i].volume;
        }
        if (yourVolume > maxvol)
        {
            maxvol = yourVolume;
        }
        allvolume += yourVolume;
        maxvol = 100f / allvolume * maxvol; // максимальный процент будет 700 пикселей

        float referenceSize = -1f;

        for (int i = 0; i < stats.Count; i++) // Создаём все префабы
        {
            float pc = (float)Math.Round(100f / allvolume * stats[i].volume, 1); // Узнаём мощность этой компании
            var a = Instantiate(onestat, statPanel.transform); // Создаём стат для неё
            a.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text = pc + "%"; // добавляем мощность в текст
            RectTransform logos = a.transform.Find("Logo").gameObject.GetComponent<RectTransform>();
            RectTransform texts = a.transform.Find("Text").gameObject.GetComponent<RectTransform>();
            logos.GetComponent<Image>().sprite = stats[i].logo; // вставляем логотив компании
            a.transform.Find("NAME").gameObject.GetComponent<Text>().text = stats[i].name;

            RectTransform percents = a.transform.Find("Percents").GetComponent<RectTransform>();
            if (referenceSize == -1f)
            {
                referenceSize = percents.rect.center.x * 0.5f + percents.rect.width;
            }

            float perw = referenceSize / maxvol * pc; // ширина полосы процентов
            percents.offsetMax = new Vector2(-referenceSize + perw, 0);
            logos.offsetMin = new Vector2(percents.offsetMax.x, 0);
            logos.offsetMax = new Vector2(-logos.offsetMax.x + percents.offsetMax.x, 0);
            texts.offsetMin = new Vector2(logos.offsetMax.x, 0);
            texts.offsetMax = new Vector2(-texts.offsetMax.x + logos.offsetMax.x, 0);

        }

        //////////////////////////////////// СТАТИСТИКА ИГРОКА
        var b = Instantiate(onestat, statPanel.transform);
        float p = (float)Math.Round(100f / allvolume * yourVolume, 1);
        yourScore.text = p + "%";
        RectTransform textsy = b.transform.Find("Text").gameObject.GetComponent<RectTransform>();
        RectTransform yous = b.transform.Find("YOU").gameObject.GetComponent<RectTransform>();
        RectTransform percentsy = b.transform.Find("Percents").GetComponent<RectTransform>();
        textsy.GetComponent<TextMeshProUGUI>().text = p + "%"; // добавляем мощность в текст
        b.transform.Find("NAME").gameObject.GetComponent<Text>().text = tr.you;
        b.transform.Find("Logo").gameObject.SetActive(false);
        float per = referenceSize / maxvol * p;
        yous.gameObject.SetActive(true);
        percentsy.offsetMax = new Vector2(-referenceSize + per, 0);
        yous.offsetMin = new Vector2(percentsy.offsetMax.x, 0);
        yous.offsetMax = new Vector2(-yous.offsetMax.x + percentsy.offsetMax.x, 0);
        textsy.offsetMin = new Vector2(yous.offsetMax.x, 0);
        textsy.offsetMax = new Vector2(-textsy.offsetMax.x + yous.offsetMax.x, 0);
        b.GetComponent<Image>().color = new Color(b.GetComponent<Image>().color.r, b.GetComponent<Image>().color.g, b.GetComponent<Image>().color.b, 0.4f);
        ////////////////////////////////////

        for (int i = 1; i < statPanel.transform.childCount; i++)
        { // Сортируем
            if (float.Parse(statPanel.transform.GetChild(i).Find("Text").GetComponent<TextMeshProUGUI>().text.Trim(new char[] { '%' })) >
            float.Parse(statPanel.transform.GetChild(i - 1).Find("Text").GetComponent<TextMeshProUGUI>().text.Trim(new char[] { '%' })))
            {

                statPanel.transform.GetChild(i).SetSiblingIndex(i - 1);
                i = 0;
            }
        }
        for (int i = 1; i <= statPanel.transform.childCount; i++)
        { // Нумеруем
            statPanel.transform.GetChild(i - 1).Find("Number").gameObject.GetComponent<TextMeshProUGUI>().text = i + ".";
        }
        for (int i = 0; i < statPanel.transform.childCount; i++)
        {
            if (statPanel.transform.GetChild(i).Find("NAME").gameObject.GetComponent<Text>().text == tr.you)
            {
                yourpos = i;
            }
        }
    }

    void BusinessesShow()
    {
        Transform bus_con = choosePanel.transform.Find("Business").Find("Scroll View").GetChild(0).GetChild(0);
        Transform ybus_con = allPanels.transform.Find("YourBusinesses").Find("Scroll View").GetChild(0).GetChild(0);
        for (int i = bus_con.childCount - 2; i > 1; i--)
        {
            Destroy(bus_con.GetChild(i).gameObject);
        }
        for (int i = ybus_con.childCount - 1; i > 0; i--)
        {
            Destroy(ybus_con.GetChild(i).gameObject);
        }
        //   Debug.Log(saveall.businesses.Count);
        for (int i = 0; i + 1 <= saveall.businesses.Count; i++)
        {
            var BusNex = Instantiate(bus_con.Find("Business").gameObject, bus_con);
            var BusPan = Instantiate(ybus_con.GetChild(0).gameObject, ybus_con);
            BusNex.SetActive(true);
            BusPan.SetActive(true);
            BusNex.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = saveall.businesses[i].name;
            BusNex.transform.Find("Image").GetComponent<Image>().sprite = saveall.businesses[i].image;
            BusNex.transform.SetSiblingIndex(1);
            BusPan.transform.Find("Business name").GetComponent<TextMeshProUGUI>().text = saveall.businesses[i].name;
            BusPan.transform.Find("Image").GetComponent<Image>().sprite = saveall.businesses[i].image;
            BusPan.transform.SetSiblingIndex(1);
        }
    }


    // Меню бизнесов
    public void BusinessShow(TextMeshProUGUI tex)
    {
        Transform chooses = tex.transform.parent.parent.parent.parent.parent.Find("ChooseShow");
        chooses.gameObject.SetActive(true);
    }

    public void BusinessBack(GameObject showbus)
    {
        showbus.SetActive(false);
    }




    ////////////////////НОВЫЙ БИЗНЕС
    public void NewBusinessButton()
    {
        newBusiness.SetActive(true);
        newBusinessSlider.maxValue = saveall.cash;
        newBusiness.transform.Find("InputField (TMP)").gameObject.GetComponent<TMP_InputField>().text = "";
        newBusiness.transform.Find("Dropdown").gameObject.GetComponent<TMP_Dropdown>().value = 0;
        typeOfBusiness = new List<OptionData>();
        for (int i = 0; i < 8; i++)
        {
            if (minBudget[i] <= saveall.cash)
                typeOfBusiness.Add(new OptionData(tr.bustype[i]));
        }
        newBusiness.transform.Find("Dropdown").gameObject.GetComponent<TMP_Dropdown>().AddOptions(typeOfBusiness);
        ChangeTypeNew();
    }

    public void RCNBB() // Refresh Create New Business Button
    {
        if (saveall.cash < minBudget[newBusiness.transform.Find("Dropdown").gameObject.GetComponent<TMP_Dropdown>().value] || newBusiness.transform.Find("InputField (TMP)").gameObject.GetComponent<TMP_InputField>().text == ""
        || newBusiness.transform.Find("InputField (TMP)").gameObject.GetComponent<TMP_InputField>().text.Length < 5)
        {
            newBusiness.transform.Find("Create").gameObject.GetComponent<Button>().interactable = false;
        }
        else
        {
            newBusiness.transform.Find("Create").gameObject.GetComponent<Button>().interactable = true;
        }
    }

    public void ChangeValueNew()
    {
        newBusiness.transform.Find("InputField (TMP) (1)").gameObject.GetComponent<TMP_InputField>().text = MoneyConvert((long)newBusiness.transform.Find("Slider").gameObject.GetComponent<Slider>().value);
        RCNBB();
    }

    public void ChangeTypeNew()
    {
        newBusinessSlider.minValue = minBudget[newBusiness.transform.Find("Dropdown").gameObject.GetComponent<TMP_Dropdown>().value];
        newBusinessSlider.value = newBusiness.transform.Find("Slider").gameObject.GetComponent<Slider>().minValue;
        newBusiness.transform.Find("InputField (TMP) (1)").gameObject.GetComponent<TMP_InputField>().text = MoneyConvert(minBudget[newBusiness.transform.Find("Dropdown").gameObject.GetComponent<TMP_Dropdown>().value]) + "$";
        RCNBB();
    }
    public void CancelNew()
    {
        newBusiness.SetActive(false);
    }
    public void CreateNew()
    {
        bustemp[idplan] = new Saveall.Businesses();
        bustemp[idplan].name = newBusiness.transform.Find("InputField (TMP)").gameObject.GetComponent<TMP_InputField>().text;
        bustemp[idplan].type = newBusiness.transform.Find("Dropdown").Find("Label").gameObject.GetComponent<TextMeshProUGUI>().text;
        bustemp[idplan].budget = (long)newBusinessSlider.value;
        newBusiness.SetActive(false);
        GameObject plan = listOfPlans.transform.Find(idplan + " plan").Find("Plan").gameObject; // находим, на какой план мы нажимали ранее
        plan.GetComponentInChildren<TextMeshProUGUI>().text = tr.create + " " + bustemp[idplan].name; // присваиваем ему текст кнопки
        plans[idplan] = 100;
        choose_button_plan[idplan] = choosePanel.transform.Find("Business").GetChild(0).GetChild(0).GetChild(0).Find("NewBusiness").GetComponent<Button>();
        listOfPlans.transform.Find(idplan + " plan").gameObject.GetComponent<Animator>().SetBool("Check", true); // запускаем анимацию плюсик-крестик
        plan.GetComponent<Image>().color = colors[idplan - 1]; // меняем цвет плану
        allhours[idplan] = 8;
        deltaСash[idplan] = (long)newBusinessSlider.value;
        deltaСash[0] += deltaСash[idplan];
        hours += allhours[idplan]; // для часов
        ChangeScale();
        hour.text = hours + "/24" + tr.hour[0]; // Меняем текст количества часов
        Back(); // выходим из папки
    }

    void RefreshAll()
    {
        day.text = saveall.day + " " + tr.day;
        StatsShow();
        BusinessesShow();
        TimeLineRefresh();
        InfoRefresh();
    }


    public void NextDay()
    {
        saveall.cash -= deltaСash[0];
        saveall.timeline.Add(new Saveall.TimeLine());
        saveall.timeline.Last().day = saveall.day;
        saveall.timeline.Last().image = null; // TODO
        saveall.day++;
        for (int i = 1; i <= 5; i++)
        {
            switch (plans[i])
            {
                case 1:
                    if (Random.Range(0, 100) > 10)
                    {
                        saveall.sleep = (saveall.sleep + allhours[i] / 8f * 5);
                        Debug.Log(allhours[i]);
                    }
                    else
                    {
                        sconfuse = tr.confuses[Random.Range(0, 1)];
                    }
                    break;
                case 2:
                    if (Random.Range(0, 100) > 10)
                    {
                        saveall.hunger = (saveall.hunger + allhours[i] / 2f * 5);
                    }
                    else
                    {
                        hconfuse = tr.confuses[Random.Range(2, 3)];
                    }
                    break;
                case 3:
                    if (Random.Range(0, 100) > 10)
                    {
                        saveall.tiredness = (saveall.tiredness + allhours[i] / 4f * 5);
                    }
                    else
                    {
                        tconfuse = tr.confuses[Random.Range(4, 5)];
                    }
                    break;
                case 5:
                    workMoney = Random.Range(0, 30) * allhours[i];
                    saveall.tiredness -= 0.2f * allhours[i];
                    saveall.hunger -= 0.1f * allhours[i];
                    saveall.sleep -= 0.1f * allhours[i];
                    saveall.cash += workMoney;
                    if (workMoney < 50)
                        saveall.timeline.Last().news.Add(new Saveall.TimeLine.News(tr.scales[3], tr.work[Random.Range(3, 4)], gbColor[2]));// : (workMoney < 200) ? 2 : Random.Range(0, 1)]));
                    else if (workMoney < 200)
                        saveall.timeline.Last().news.Add(new Saveall.TimeLine.News(tr.scales[3], tr.work[2], gbColor[1]));
                    else
                        saveall.timeline.Last().news.Add(new Saveall.TimeLine.News(tr.scales[3], tr.work[Random.Range(0, 1)], gbColor[0]));
                    break;
                case 100:
                    {
                        saveall.businesses.Add(new Saveall.Businesses());
                        saveall.businesses.Last().name = bustemp[i].name;
                        saveall.businesses.Last().type = bustemp[i].type;
                        saveall.businesses.Last().budget = bustemp[i].budget;
                        saveall.timeline.Last().news.Add(new Saveall.TimeLine.News(tr.create + " " + bustemp[i].name, tr.newbusiness[0], gbColor[0]));
                        bustemp[i] = null;
                    }
                    break;
                default:
                    break;
            }
        }

        saveall.sleep /= 2f;
        saveall.hunger /= 2f;
        saveall.tiredness /= 2f;

        int rang; int clr = 0;
        if (saveall.sleep < 2)
        {
            rang = Random.Range(11, 12);    // Очень хочется спать
            clr = 2;
        }
        else if (saveall.sleep < 4)
        {
            rang = Random.Range(6, 8); // Просто хочется спать
            clr = 1;
        }
        else rang = Random.Range(0, 6);                // Всё ок

        saveall.timeline[saveall.timeline.Count - 1].news.Add(new Saveall.TimeLine.News(tr.scales[0], sconfuse + tr.sleepiness[rang], gbColor[clr]));
        clr = 0;
        if (saveall.hunger < 2)
        {
            rang = Random.Range(6, 7);       // Большой голод
            clr = 2;
        }
        else if (saveall.hunger < 4)
        {
            rang = Random.Range(3, 5); // Средний голод
            clr = 1;
        }
        else rang = Random.Range(0, 2);                 // Сытость

        saveall.timeline[saveall.timeline.Count - 1].news.Add(new Saveall.TimeLine.News(tr.scales[1], hconfuse + tr.hunger[rang], gbColor[clr]));
        clr = 0;
        if (saveall.tiredness < 2)
        {
            rang = 3;                       // Сильная усталость
            clr = 2;
        }
        else if (saveall.tiredness < 4)
        {
            rang = Random.Range(1, 2); // Средняя усталость
            clr = 1;
        }
        else rang = 0;                                     // Нет усталости

        saveall.timeline[saveall.timeline.Count - 1].news.Add(new Saveall.TimeLine.News(tr.scales[2], tconfuse + tr.tiredness[rang], gbColor[clr]));


        sconfuse = null; hconfuse = null; tconfuse = null;
        hours = 0;
        hour.text = "0/24" + tr.hour[0]; // Меняем текст количества часов
        for (int i = 1; i < 6; i++)
        {
            if (choose_button_plan[i] != null)
            {
                choose_button_plan[i].interactable = true;
                choose_button_plan[i] = null;
            }
            allhours[i] = 0;
            plans[i] = 0;
            GameObject plan = listOfPlans.transform.Find(i + " plan").Find("Plan").gameObject; // Находим план
            listOfPlans.transform.Find(i + " plan").gameObject.GetComponent<Animator>().SetBool("Check", false); // Перевернуть крестик в плюсик
            plan.GetComponentInChildren<TextMeshProUGUI>().text = tr.chooseplan; // возвращаем текст...
            plan.GetComponent<Image>().color = colors[5]; // ...и цвет
            listOfPlans.transform.Find(i + " plan").Find("ButtonPlus").localRotation = Quaternion.Euler(0, 0, 0);
        }
        ChangeScale();
        fromNext = true;
        RefreshAll();
        Bottom(allPanels.transform.parent.Find("Bottom panel").Find("TimeLine").gameObject);
    }
    ///////////////////////////////////TimeLine
    public void InfoTL()
    {

        TimeLineObject.GetComponent<Animator>().SetBool("opened", !allPanels.transform.Find("TimeLine").GetComponent<Animator>().GetBool("opened"));

    }

    public void Shedule()
    {
        Debug.DrawLine(new Vector2(1,1),new Vector2(100,100),colors[6]);
    }
}

[Serializable]
public class Stats
{
    [Tooltip("Название компании")]
    public string name;
    [Tooltip("Логотип компании")]
    public Sprite logo;
    [Tooltip("Мощность компании (Средний заработок в год)")]
    public float volume;
}

[Serializable]
public class Saveall
{
    public int language = 1;
    public int day = 1;

    public long cash = 0;
    public float hunger = 5;

    public float tiredness = 5;

    public float sleep = 5;
    public List<Businesses> businesses = new List<Businesses>();

    public List<TimeLine> timeline = new List<TimeLine>();

    [Serializable]
    public class TimeLine
    {
        [Tooltip("Номер дня")]
        public int day;
        public Sprite image;
        public List<News> news = new List<News>();

        public List<Changes> changes = new List<Changes>();

        [Serializable]
        public class News
        {
            public string logo;
            public string description;

            public Color color = new Color(0.34f, 0.49f, 0.77f);

            public News(string newLogo, string newDes, Color newColor)
            {
                logo = newLogo;
                description = newDes;
                color = newColor;
            }
            public News(string newLogo, string newDes)
            {
                logo = newLogo;
                description = newDes;
            }
        }
        [Serializable]
        public class Changes
        {
            public string change;
            public Sprite image;
        }
    }
    [Serializable]
    public class Businesses
    {
        [Tooltip("Название бизнеса")]
        public string name;
        [Tooltip("Описание бизнеса")]
        public string description;
        [Tooltip("Тип бизнеса")]
        public string type;
        [Tooltip("Номер логотипа")]

        public Sprite image;
        public int logoid;
        [Tooltip("Доходы")]
        public float incomes;
        [Tooltip("Расходы")]
        public float costs;
        [Tooltip("Бюджет")]
        public long budget;
        [Tooltip("Работники")]
        public List<Members> member = new List<Members>();
        [Serializable]
        public class Members
        {
            [Tooltip("Имя работника")]
            public string name;
            [Tooltip("Описание работника")]
            public string description;
            [Tooltip("Плюсы")]
            public int plus;
            [Tooltip("Минусы")]
            public int minus;
        }
    }
}