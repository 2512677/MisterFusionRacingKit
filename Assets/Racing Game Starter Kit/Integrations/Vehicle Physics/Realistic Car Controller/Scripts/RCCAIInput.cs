using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RGSK;
using System;

public class RCCAIInput : MonoBehaviour, IAiInput
{
    private RCC_CarControllerV3 rcc;

    void Start()
    {
        rcc = GetComponent<RCC_CarControllerV3>();
    }

    // Интерфейс требует четыре параметра, убираем boost
    public void SetInputValues(float throttle, float brake, float steer, float handbrake)
    {
        if (rcc != null)
        {
            // Проверяем, что гонка началась и обратный отсчёт завершён
            if (RaceManager.instance != null && RaceManager.instance.raceState == RaceState.Race)
            {
                // Включаем двигатель, если он ещё не запущен
                if (!rcc.engineRunning)
                {
                    rcc.engineRunning = true;
                }

                // Управляем машиной, если двигатель запущен
                if (rcc.engineRunning)
                {
                    rcc.gasInput = throttle;
                    rcc.brakeInput = brake;
                    rcc.steerInput = steer;
                    rcc.handbrakeInput = handbrake;
                }
            }
        }
    }

    public void ApplyBoost(int boost)
    {
        if (rcc != null)
        {
            rcc.boostInput = boost == 1 ? 2.5f : 1.0f;
        }
    }
}
