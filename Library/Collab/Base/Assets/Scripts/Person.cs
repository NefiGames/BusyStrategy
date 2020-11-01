using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
/// <summary>
/// Класс бизнесов
/// </summary>
public class Business
{
    /// <summary> Название бизнеса </summary>
    public readonly string name;

    /// <summary> Описание бизнеса </summary>
    public readonly string description;

    /// <summary> Тип бизнеса </summary>
    public readonly string type;

    /// <summary> Картинка бизнеса </summary>
    readonly Sprite image;
    public int logoid;
    /// <summary> Доходы бизнеса </summary>
    private float _incomes;
    /// <summary> Расходы бизнеса </summary>
    private float _outcomes;
    /// <summary> Начальный бюджет бизнеса </summary>
    public readonly long budget;
    /// <summary> Словарь работников </summary>
    private Dictionary<string, int> mems;
    /// <summary>
    /// Лист работников бизнеса
    /// </summary>
    /// <typeparam name="Member">Класс работников бизнеса</typeparam>
    /// <returns></returns>
    private List<Member> members;
    public float incomes { get => _incomes;}
    public float outcomes { get => _outcomes;}

    /// <summary> Работники бизнеса </summary> 
    public class Member
    {
        /// <summary> Имя работника </summary>
        public readonly string name;
        /// <summary> Описание работника </summary>
        public readonly string description;
        /// <summary> Оценки "плюс" работника </summary>
        public int plus;
         /// <summary> Оценки "минус" работника </summary>
        public int minus;

        public Member(string name, string description)
        {
            this.name = name;
            this.description = description;
            this.plus = 0;
            this.minus = 0;
        }
    }
    /// <summary>
    /// Конструктор бизнеса
    /// </summary>
    /// <param name="name">Название бизнеса</param>
    /// <param name="type">Тип бизнеса</param>
    /// <param name="budget">Бюджет бизнеса</param>
    public Business(string name, string type, long budget)
    {
        this.name = name;
        this.type = type;
        this.budget = budget;

        this.image = GenerateImage(type);
        this.mems = new Dictionary<string, int>();
    }
    private Sprite GenerateImage(string type){return null;}
    /// <summary> Метод что генерирует доходы компании </summary> 
    private void GenerateIncomes(){}
    // метод что генерирует доходы компании
    /// <summary> Метод что генерирует расходы компании </summary> 
    private void GenerateOutcomes(){}
    // метод что генерирует расходы компании
    /// <summary> Метод что генерирует случайное событие внутри компании </summary> 

    public void update()
    {
        GenerateIncomes();
        GenerateOutcomes();
    }
    /// <summary> Метод добавления работника компании </summary> 
    public void AddMember(string name, string description)
    // метод добавления работника
    {
        members.Add(new Member(name, description));
        mems.Add(name, members.Count);
    }
    /// <summary> Метод удаления работника компании </summary> 
    public void RemoveMember(string name)
    // метод удаления работника компании
    {
        if (mems.ContainsKey(name))
        {
            members.RemoveAt(mems[name]);
            mems.Remove(name);
        }
        else throw new Exception("Member does not exist");
    }
    /// <summary> Метод добавления плюсика работнику </summary> 
    internal void AddPlus(string name)
    // метод добавления плюсика работнику
    {
        members[mems[name]].plus++;
    }
    /// <summary> Метод добавления минуса работнику </summary> 
    internal void AddMinus(string name)
    // метод добавления минуса работнику
    {
        members[mems[name]].minus++;
    }
    /// <summary> Геттер плюсиков работника </summary> 
    public int GetPlus(string name)
    // геттер плюсиков работника
    {
        return members[mems[name]].plus;
    }
    /// <summary> Геттер минусов работника </summary> 
    public int GetMinus(string name)
    // геттер минусов работника
    {
        return members[mems[name]].minus;
    }
}
/// <summary>
/// Класс главного игрока
/// </summary>
public class Person
{
    /// <summary> Деньги </summary>
    private long _cash;
    /// <summary> Сытость </summary>
    private float _hunger;
    /// <summary> Отдых </summary>
    private float _tiredness;
    /// <summary> Сон </summary>
    private float _sleep;
    /// <summary> Словарь бизнессов </summary>
    private Dictionary<string, int> busins;
    /// <summary> Лист бизнессов </summary>
    private List<Business> businesses;
    /// <summary> Заработок с роботы на текущем ходу </summary>
    private int workMoney;

    /// <summary> Деньги </summary>
    public long cash { get => _cash; set => _cash = value; }
    public float hunger
    {
      get => _hunger;
      private set => _hunger = value;
    }
    public float tiredness
    {
        get => _tiredness;
        private set => _tiredness = value;
    }
    public float sleep
    {
        get => _sleep;
        private set => _sleep = value;
    }

    // Инкапсуляция переменной cash

    /// <summary>
    /// Конструктор класса Person
    /// </summary>
    /// <param name="cash">Деньги</param>
    /// <param name="hunger">Сытость</param>
    /// <param name="tiredness">Отдых</param>
    /// <param name="sleep">Сон</param>
    /// <param name="businesses">Бизнесы</param>
    public Person(long cash = 0, float hunger = 5, float tiredness = 5, float sleep = 5, List<Business> businesses = null)
    // Конструктор класса Person
    {
        this.cash = cash;
        this.hunger = hunger;
        this.tiredness = tiredness;
        this.sleep = sleep;
        this.businesses = businesses == null ? new List<Business>() : businesses;
        busins = new Dictionary<string, int>();
        for(int i = 0; i <= businesses.Count; i++)
            busins.Add(businesses[i].name, i);
    }
    /// <summary>
    /// Обновить сытость, отдых, сон, и бизнесы (возможность вызова чрезвычайных ситуаций)
    /// </summary>
    public void update()
    // изменения что происходят во время нажатия кнопки ход
    {
        this.sleep /= 2f;
        this.tiredness /= 2f;
        this.tiredness /= 2f;
        this.workMoney = 0;
        foreach(var business in businesses)
            business.update();
    }

    public void AddBusiness(string name, string type, long budget)
    // метод добавления нового бизнеса персонажу
    {
        businesses.Add(new Business(name, type, budget));
        busins.Add(name, businesses.Count);
        cash -= budget;
    }

    public Business GetCurrentBusiness(string businessName)
    // метод возвращающий бизнес по имени
    {
        return businesses[busins[businessName]];
    }

    public bool IsBusiness() => businesses.Count > 0;
    // метод что проверяет есть ли у игрока бизнесы
    public void AddHunger(int hours) => this.hunger += hours / 8f * 5;
    // метод добавления значений к Сытости
    public void AddTiredness(int hours) => this.tiredness += hours / 2f * 5;
    // метод добавления значений к Отдыху
    public void AddSleep(int hours) => this.sleep += hours / 4f * 5;
    // метод добавления значений к Сну
    public void AddWork(int hours)
    // метод добавления роботы
    {
        workMoney = Random.Range(0, 30) * hours;
        tiredness -= 0.2f * hours;
        hunger -= 0.1f * hours;
        sleep -= 0.1f * hours;
        cash += workMoney;
    }

    private int GetColor(float num)
    {
        if (num < 2)
            return 2;
        else if (num < 4)
            return 1;
        else
            return 0;
    }

    public int GetHungerStatus(out int clr)
    {
        clr = this.GetColor(this.hunger);
        if (clr == 2)
            return Random.Range(6, 7);  // Большой голод
        else
            if (clr == 1)
            return Random.Range(3, 5);  // Средний голод
        else
            return Random.Range(0, 2);  // Сытость
    }

    public int GetTirednessStatus(out int clr)
    {
        clr = this.GetColor(this.tiredness);
        if (clr == 2)
            return 3;                   // Сильная усталость
        else
            if (clr == 1)
            return Random.Range(1, 2);  // Средняя усталость
        else
            return 0;                   // Нет усталости
    }

    public int GetSleepStatus(out int clr)
    {
        clr = this.GetColor(this.sleep);
        if (clr == 2)
            return Random.Range(11, 12);    // Очень хочется спать
        else
            if (clr == 1)
            return Random.Range(6, 8);      // Просто хочется спать
        else
            return Random.Range(0, 6);      // Всё ок
    }

    public int GetWorkStatus(out int clr)
    {
        if (workMoney < 50)
        {
            clr = 2;
            return Random.Range(3, 4);
        }
        else if (workMoney < 200)
        {
            clr = 1;
            return 2;
        }
        else
        {
            clr = 0;
            return Random.Range(0, 1);
        }
    }
}
