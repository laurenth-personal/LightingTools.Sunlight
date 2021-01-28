using UnityEngine;
using GameplayIngredients;
using NaughtyAttributes;

[ManagerDefaultPrefab("TimeOfDayManager")]
public class TimeOfDayManager : Manager
{
    public float initialTimeOfDay = 10.0f;
    public float dayDuration = 30.0f;
    public float timeOfDay { get; private set; }
    public bool isNight { get; private set; }
    private bool previousIsNight;
    public bool isPaused { get; private set; }

    [Header("Day Phases")]
    public float nightTime = 18.0f;
    public float dayTime = 6.0f;

    [Header("Events")]
    [ReorderableList]
    public Callable[] OnDayEvent;
    [ReorderableList]
    public Callable[] OnNightEvent;

    private void Start()
    {
        timeOfDay = initialTimeOfDay;
    }

    private void Update()
    {
        //if paused, no update
        if (isPaused == true)
            return;

        //if not paused, update
        timeOfDay += Time.deltaTime * 24 / dayDuration;

        if (timeOfDay > 24)
            timeOfDay %= 24;

        //Debug.Log(timeOfDay);
        Globals.SetFloat("TimeOfDay", timeOfDay, Globals.Scope.Global);

        //Manage time of day events
        if (timeOfDay > dayTime && timeOfDay < nightTime)
            isNight = false;
        else
            isNight = true;

        if (isNight != previousIsNight)
            DayPhaseChanged();

        previousIsNight = isNight;
    }

    public void PauseTime()
    {
        isPaused = true;
    }

    public void UnpauseTime()
    {
        isPaused = false;
    }

    private void DayPhaseChanged()
    {
        if (isNight)
            ExecuteNight();
        else
            ExecuteDay();
    }

    public void ExecuteDay()
    {
        Callable.Call(OnDayEvent);
    }

    public void ExecuteNight()
    {
        Callable.Call(OnNightEvent);
    }
}
