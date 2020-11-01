using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeLinePanel : Father
{
    [SerializeField] private NextDayPanel nextDayScript;
    [SerializeField] private TextMeshProUGUI outputMoneyInTimeLine;
    private List<News> allNews = new List<News>();
    [SerializeField] private Translate tr;

    private enum plan { sleep = 1, hunger, tiredness, work, business}

    public enum Mark { Bad = 1, Normal, Good }


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
                        sconfuse = tr.tr.confuses[Random.Range(0, 1)];
                    }
                    break;
                case (int)plan.hunger:
                    if (Random.Range(0, 100) > 10)
                    {
                        Person.AddSleep(plans[i].hours);
                    }
                    else
                    {
                        hconfuse = tr.tr.confuses[Random.Range(2, 3)];
                    }
                    break;
                case (int)plan.tiredness:
                    if (Random.Range(0, 100) > 10)
                    {
                        Person.AddTiredness(plans[i].hours);
                    }
                    else
                    {
                        tconfuse = tr.tr.confuses[Random.Range(4, 5)];
                    }
                    break;
                case (int)plan.work:
                    Person.AddWork(plans[i].hours);
                    if (Person.WorkMoney < 50)
                        todayNews.pnews.Add(new PNews(tr.tr.scales[3], tr.tr.work[Random.Range(3, 4)], Main.GetMarkColor((int)Mark.Bad)));
                    else if (Person.WorkMoney < 200)
                        todayNews.pnews.Add(new PNews(tr.tr.scales[3], tr.tr.work[2], Main.GetMarkColor((int)Mark.Normal)));
                    else
                        todayNews.pnews.Add(new PNews(tr.tr.scales[3], tr.tr.work[Random.Range(0, 1)], Main.GetMarkColor((int)Mark.Good)));
                    break;
                case (int)plan.business:
                    {
                        Person.AddBusiness(plans[i].GetBusiness());
                        todayNews.pnews.Add(new PNews(tr.tr.create + " " + plans[i].GetBusiness().name, tr.tr.newbusiness[0], Main.GetMarkColor((int)Mark.Good)));
                    }
                    break;
                default:
                    break;
            }
        }
        int rang; int clr = 0;

        rang = Person.GetSleepStatus(out clr);
        todayNews.pnews.Add(new PNews(tr.tr.scales[0], sconfuse + tr.tr.sleepiness[rang], Main.GetMarkColor(clr)));
        clr = 0;

        rang = Person.GetHungerStatus(out clr);
        todayNews.pnews.Add(new PNews(tr.tr.scales[1], hconfuse + tr.tr.hunger[rang], Main.GetMarkColor(clr)));
        clr = 0;

        rang = Person.GetTirednessStatus(out clr);
        todayNews.pnews.Add(new PNews(tr.tr.scales[2], tconfuse + tr.tr.tiredness[rang], Main.GetMarkColor(clr)));

        allNews.Add(todayNews);
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
}
