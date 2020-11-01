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
    public int index { get; private set; }
    /// <summary> Индекс бизнесса </summary>
    public static int numOfBusiness { get; protected set; }

    /// <summary> Название бизнеса </summary>
    public readonly string name;

    /// <summary> Описание бизнеса </summary>
    public readonly string description;

    /// <summary> Тип бизнеса </summary>
    public readonly string type;

    /// <summary> Картинка бизнеса </summary>
    public readonly Sprite image;
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
    public float incomes { get => _incomes; }
    public float outcomes { get => _outcomes; }

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
    static Business()
    {
        numOfBusiness = 0;
    }
    public Business()
    {
        this.index = numOfBusiness;
        numOfBusiness++;
    }
    public Business(string name, string type, long budget) : this()
    {
        this.name = name;
        this.type = type;
        this.budget = budget;

        this.image = GenerateImage(type);
        this.mems = new Dictionary<string, int>();
    }
    /// <summary> Конструктор копирования обекта </summary> 
    public Business(Business other) : this(other.name, other.type, other.budget) { }
    /// <summary> Конструктор пустого обекта </summary> 
    public Business(int numOfBusiness)
    {
        this.name = "deleted";
        this.type = "0";  
    }
    private Sprite GenerateImage(string type) { return null; }
    /// <summary> Метод что генерирует доходы компании </summary> 
    private void GenerateIncomes() { }
    // метод что генерирует доходы компании
    /// <summary> Метод что генерирует расходы компании </summary> 
    private void GenerateOutcomes() { }
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
public static class Person
{
    private static long _cash;

    /// <summary> Сытость </summary>
    private static float _hunger;
    /// <summary> Отдых </summary>
    private static float _tiredness;
    /// <summary> Сон </summary>
    private static float _sleep;
    /// <summary> Лист бизнессов </summary>
    private static List<Business> businesses;
    /// <summary> Заработок с роботы на текущем ходу </summary>
    private static int workMoney;

    /// <summary> Деньги </summary>
    public static long cash { get => _cash; set => _cash = value; }
    public static float hunger
    {
        get => _hunger;
        private set => _hunger = value;
    }
    public static float tiredness
    {
        get => _tiredness;
        private set => _tiredness = value;
    }
    public static float sleep
    {
        get => _sleep;
        private set => _sleep = value;
    }

    // Инкапсуляция переменной cash
    /// <summary>
    /// Обновить сытость, отдых, сон, и бизнесы (возможность вызова чрезвычайных ситуаций)
    /// </summary>
    public static void update()
    // изменения что происходят во время нажатия кнопки ход
    {
        sleep /= 2f;
        tiredness /= 2f;
        hunger /= 2f;
        workMoney = 0;
        foreach (var business in businesses)
            business.update();
    }

    public static void AddBusiness(Business business)
    // метод добавления нового бизнеса персонажу
    {
        businesses.Add(business);
        cash -= business.budget;
    }

    public static void AddBusiness(string name, string type, long budget)
    // метод добавления нового бизнеса персонажу
    {
        businesses.Add(new Business(name, type, budget));
        cash -= budget;
    }

    public static void RemoveBusiness(int num) => businesses[num] = new Business(num);
    // метод удаляющий бизнес по индексу

    public static void RemoveLastBusiness() => RemoveBusiness(businesses.Count);
    // метод удаляющий последний бизнес

    public static Business GetCurrentBusiness(int num) => businesses[num];
    // метод возвращающий бизнес по индексу
    public static Business LastBusiness => GetCurrentBusiness(businesses.Count);
    // свойство что возвращает количество бизнесов
    public static int CountBusiness => businesses.Count;
    // свойство что возвращает количество бизнесов
    public static bool IsBusiness => businesses.Count > 0;

    public static int WorkMoney { get => workMoney; set => workMoney = value; }

    // свойство что проверяет есть ли у игрока бизнесы
    public static void AddHunger(int hours) => hunger += hours / 8f * 5;
    // метод добавления значений к Сытости
    public static void AddTiredness(int hours) => tiredness += hours / 2f * 5;
    // метод добавления значений к Отдыху
    public static void AddSleep(int hours) => sleep += hours / 4f * 5;
    // метод добавления значений к Сну
    public static void AddWork(int hours)
    // метод добавления роботы
    {
        workMoney = Random.Range(0, 30) * hours;
        tiredness -= 0.2f * hours;
        hunger -= 0.1f * hours;
        sleep -= 0.1f * hours;
    }

    private static int GetColor(float num)
    {
        if (num < 2)
            return 2;
        else if (num < 4)
            return 1;
        else
            return 0;
    }

    public static int GetHungerStatus(out int clr)
    {
        clr = GetColor(hunger);
        if (clr == 2)
            return Random.Range(6, 8);  // Большой голод
        else
            if (clr == 1)
            return Random.Range(3, 6);  // Средний голод
        else
            return Random.Range(0, 3);  // Сытость
    }

    public  static int GetTirednessStatus(out int clr)
    {
        clr = GetColor(tiredness);
        if (clr == 2)
            return 3;                   // Сильная усталость
        else
            if (clr == 1)
            return Random.Range(1, 3);  // Средняя усталость
        else
            return 0;                   // Нет усталости
    }

    public static int GetSleepStatus(out int clr)
    {
        clr = GetColor(sleep);
        if (clr == 2)
            return Random.Range(11, 13);    // Очень хочется спать
        else
            if (clr == 1)
            return Random.Range(6, 9);      // Просто хочется спать
        else
            return Random.Range(0, 7);      // Всё ок
    }

    public static int GetWorkStatus(out int clr)
    {
        if (workMoney < 50)
        {
            clr = 2;
            return Random.Range(3, 5);
        }
        else if (workMoney < 200)
        {
            clr = 1;
            return 2;
        }
        else
        {
            clr = 0;
            return Random.Range(0, 2);
        }
    }
}
