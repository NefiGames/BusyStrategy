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


public class TimeLinePanel : Father
{
    [SerializeField] private NextDayPanel nextDayScript;
    [SerializeField] private TextMeshProUGUI outputMoneyInTimeLine;
    private List<News> allNews = new List<News>();
    private List<GameObject> allNewsObj = new List<GameObject>();
    private TranslateWord tr;
    private bool TLWasCalled;
    private GameObject newsObj;
    private GameObject mainScript;
    /// <summary> Лента новостей </summary>
    [Tooltip("Лента новостей")] [BoxGroup("Окно Timeline")] public GameObject TLContent;
    /// <summary> Префаб новостей в ленте </summary>
    [Tooltip("Префаб новостей в ленте")] public GameObject NewPrefab;

    [Tooltip("Обект панели TimeLine")] GameObject TimeLineObject;

    private enum plan { sleep = 1, hunger, tiredness, work, business}

    public enum Mark { Bad = 1, Normal, Good }

    void Awake() 
    { 
        tr = GameObject.FindGameObjectWithTag("mainScript").GetComponent<Translate>().tr; 
        mainScript = GameObject.FindGameObjectWithTag("mainScript");
        TimeLineObject = GameObject.FindGameObjectWithTag("allpanels").transform.Find("TimeLine").gameObject; // Обозначаем панель TimeLine 
    }


    public void AddNew(Plan[] plans)
    {
        string sconfuse = ""; string hconfuse = ""; string tconfuse = "";
        Person.Cash -= plans[0].deltaCash;
        var todayNews = new News(Main.Day);
        for (int i = 1; i <= 5; i++)
        {
            switch (plans[i].type)
            {
                case (int)plan.sleep:
                    if (Random.Range(0, 100) > 10)
                    {
                        Person.AddSleep(plans[i].hours);
                        Debug.Log(plans[i].hours);
                    }
                    else
                    {
                        sconfuse = tr.confuses[Random.Range(0, 1)];
                    }
                    break;
                case (int)plan.hunger:
                    if (Random.Range(0, 100) > 10)
                    {
                        Person.AddSleep(plans[i].hours);
                    }
                    else
                    {
                        hconfuse = tr.confuses[Random.Range(2, 3)];
                    }
                    break;
                case (int)plan.tiredness:
                    if (Random.Range(0, 100) > 10)
                    {
                        Person.AddTiredness(plans[i].hours);
                    }
                    else
                    {
                        tconfuse = tr.confuses[Random.Range(4, 5)];
                    }
                    break;
                case (int)plan.work:
                    Person.AddWork(plans[i].hours);
                    if (Person.WorkMoney < 50)
                        todayNews.pnews.Add(new PNews(tr.scales[3], tr.work[Random.Range(3, 4)], Main.GetMarkColor((int)Mark.Bad)));
                    else if (Person.WorkMoney < 200)
                        todayNews.pnews.Add(new PNews(tr.scales[3], tr.work[2], Main.GetMarkColor((int)Mark.Normal)));
                    else
                        todayNews.pnews.Add(new PNews(tr.scales[3], tr.work[Random.Range(0, 1)], Main.GetMarkColor((int)Mark.Good)));
                    break;
                case (int)plan.business:
                    {
                        Person.AddBusiness(plans[i].GetBusiness());
                        todayNews.pnews.Add(new PNews(tr.create + " " + plans[i].GetBusiness().name, tr.newbusiness[0], Main.GetMarkColor((int)Mark.Good)));
                    }
                    break;
                default:
                    break;
            }
        }
        int rang; int clr = 0;

        rang = Person.GetSleepStatus(out clr);
        todayNews.pnews.Add(new PNews(tr.scales[0], sconfuse + tr.sleepiness[rang], Main.GetMarkColor(clr)));
        clr = 0;

        rang = Person.GetHungerStatus(out clr);
        todayNews.pnews.Add(new PNews(tr.scales[1], hconfuse + tr.hunger[rang], Main.GetMarkColor(clr)));
        clr = 0;

        rang = Person.GetTirednessStatus(out clr);
        todayNews.pnews.Add(new PNews(tr.scales[2], tconfuse + tr.tiredness[rang], Main.GetMarkColor(clr)));
    }
    void OnEnable()
    {
        if (nextDayScript.LastCash != Person.Cash)
            StartCoroutine(MoneyChange());
    }
    IEnumerator MoneyChange()
    {
        float coin = 0;
        while ((nextDayScript.LastCash < Person.Cash) ? nextDayScript.LastCash < Person.Cash : nextDayScript.LastCash > Person.Cash)
        {
            coin += 0.02f;
            nextDayScript.LastCash = (long)Mathf.Lerp(nextDayScript.LastCash, Person.Cash, coin);
            outputMoneyInTimeLine.text = ConvertMoney(nextDayScript.LastCash) + "$";
            yield return new WaitForSeconds(0.01f);
        }
    }

    ///<summary> Обновление ленты новостей после каждого дня </summary> 
    public void TimeLineRefresh(SheduleInTimeLine shedule, bool fromNext)
    {
        TLContent.transform.position = new Vector2(TLContent.transform.position.x, 0); // Возвращаем ленту вверх
        for (int i = TLContent.transform.childCount - 1; i < allNews.Count; i++) // новости пишутся с последней
        {
            CreateNews(tr, allNews[i]);
        }
        shedule.SUpdate();
        if (fromNext)
        {
            TLWasCalled = false;
            StartCoroutine(Kostil());
        }
    }

    ///<summary> Костыль, написанный для оптимизированного ScrollView </summary>
    public IEnumerator Kostil() // Вызывается, когда открываем меню таймлайна
    {
        int tlnum = 1;
        yield return new WaitForSeconds(0.001f);
        if (!TLWasCalled)
        {
            for (int i = allNews.Count; i < TLContent.transform.childCount; i++)
            {
                TLContent.transform.GetChild(i).gameObject.SetActive(true); // Активируем каждую новость
                allNewsObj.Add(TLContent.transform.GetChild(i).gameObject); // Сохраняем в список
            }
            TLWasCalled = true;
            for (int i = 0; i < 4; i++)
            {
                TLContent.GetComponent<ContentSizeFitter>().enabled = !TLContent.GetComponent<ContentSizeFitter>().enabled;
                TLContent.GetComponent<VerticalLayoutGroup>().enabled = !TLContent.GetComponent<VerticalLayoutGroup>().enabled;
                yield return new WaitForSeconds(0.001f);
            }
        }
        TLContent.transform.localPosition = new Vector2(TLContent.transform.localPosition.x, 0);
        TimeLineObject.transform.Find("Scroll View").GetComponent<ScrollRect>().velocity = Vector2.zero;
        if (allNews.Count > 5)
        {
            StartCoroutine(TLRT(tlnum));
        }
    }
    ///<summary> Оставляем только 6 новостей </summary> 
    IEnumerator TLRT(int tlnum)
    {
                float TLHeight = 0;
        for (int i = TLContent.transform.childCount - 1; i >= 0; i--) // Сначала деактивируем все нижние новости кроме шести
        {
            TLContent.transform.GetChild(i).gameObject.SetActive((i > 5) ? false : true);
        }
        while (TimeLineObject.activeSelf) // иенумератор повторяется, пока окно TimeLine открыто
        {
            if (TLContent.transform.localPosition.y > (allNewsObj[tlnum - 1].GetComponent<RectTransform>().sizeDelta.y * 3 + TLHeight) && tlnum + 5 <= allNews.Count)
            { // Если позиция контента ниже чем три новости (примерно), и направление движения вниз, то...
                TLHeight += allNewsObj[tlnum].GetComponent<RectTransform>().sizeDelta.y;
                allNewsObj[tlnum - 1].SetActive(false);// отключаем верхнюю новость (tlnum - запоминаем, с какой новости мы оставляем 6 новостей)
                if (tlnum + 5 < allNewsObj.Count)
                    allNewsObj[tlnum + 5].SetActive(true); // Прибавляем
                tlnum++; // прибавляем
            }
            else if (TLContent.transform.localPosition.y < allNewsObj[tlnum - 1].GetComponent<RectTransform>().sizeDelta.y + TLHeight && tlnum > 1)
            { // если позиция контента меньше высоты новости, tlnum больше единицы, и направление вверх, то...
                tlnum--; // отнимаем
                TLHeight -= allNewsObj[tlnum].GetComponent<RectTransform>().sizeDelta.y;
                allNewsObj[tlnum - 1].SetActive(true); // активируем верхнюю
                if (tlnum + 5 < allNewsObj.Count)
                    allNewsObj[tlnum + 5].SetActive(false); // отключаем нижнюю
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void CreateNews(TranslateWord tr, News news)
    {
        this.newsObj = Instantiate(NewPrefab, TLContent.transform);
        this.newsObj.transform.SetSiblingIndex(1);
        this.newsObj.transform.Find("DayNumber").GetComponent<TextMeshProUGUI>().text = tr.day + " " + news.day; // ставим номер дня
        SUpdate(news);
    }

    public void SUpdate(News news)
    {
        foreach (var pNews in news.pnews)
        {
            MakeNews(pNews); // Все новости расписываем
        }
    }

    void MakeNews(PNews pNews)
    {
        var logo = Instantiate(newsObj.transform.Find("Logo").gameObject, newsObj.transform); // Спавним новое лого
        logo.transform.SetSiblingIndex(newsObj.transform.childCount - 2); // Ставим его перед картинкой
        logo.SetActive(true); // Делаем активным
        logo.GetComponent<TextMeshProUGUI>().color = pNews.Color;
        logo.GetComponent<TextMeshProUGUI>().text = pNews.Heading; // Пишем локализирвоаный текст - логотип новости.
        var description = Instantiate(newsObj.transform.Find("Description").gameObject, newsObj.transform); // Повторяем те же действия с описанием
        description.SetActive(true);
        description.transform.SetSiblingIndex(newsObj.transform.childCount - 2);
        description.GetComponent<TextMeshProUGUI>().text = pNews.Description;
    }
}
