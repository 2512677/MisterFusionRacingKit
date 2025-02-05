using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RGSK;

public class RCCPlayerInput : MonoBehaviour
{
    private IInputManager inputManager;
    private RCC_CarControllerV3 rcc;

    void Start()
    {
        // Получаем ссылку на менеджер ввода
        inputManager = InputManager.instance;

        // Получаем ссылки на компоненты для передачи ввода
        rcc = GetComponent<RCC_CarControllerV3>();
    }

    private void Update()
    {
        // Проверяем, что гонка началась и обратный отсчёт завершён
        if (RaceManager.instance != null && RaceManager.instance.raceState == RaceState.Race)
        {
            // Включаем двигатель, если он ещё не запущен
            if (!rcc.engineRunning)
            {
                rcc.engineRunning = false;
            }

            // Логика управления автомобилем, если двигатель включен
            HandleCarInput();
        }
    }

    void HandleCarInput()
    {
        if (rcc.engineRunning)  // Управляем машиной только если двигатель включен
        {
            float throttle = Mathf.Clamp01(inputManager.GetAxis(0, InputAction.Throttle));
            float brake = Mathf.Clamp01(inputManager.GetAxis(0, InputAction.Brake));
            float steer = inputManager.GetAxis(0, InputAction.SteerLeft) - inputManager.GetAxis(0, InputAction.SteerRight);
            float handbrake = inputManager.GetAxis(0, InputAction.Handbrake);

            rcc.gasInput = throttle;
            rcc.brakeInput = brake;
            rcc.steerInput = steer;
            rcc.handbrakeInput = handbrake;
        }
    }
}