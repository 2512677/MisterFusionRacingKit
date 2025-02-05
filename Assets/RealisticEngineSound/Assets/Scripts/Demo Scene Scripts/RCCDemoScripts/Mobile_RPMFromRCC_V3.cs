using UnityEngine;

public class Mobile_RPMFromRCC_V3 : MonoBehaviour
{
    private RCC_CarControllerV3 rccV3;
    private RealisticEngineSound_mobile res_mob;
    private AudioClip noneClip;
    private GameObject car;

    private AudioSource rccEngineHigh;
    private AudioSource rccEngineIdle;
    private AudioSource rccEngineReverse;

    private void Start()
    {
        car = base.gameObject.transform.root.gameObject;
        rccV3 = car.GetComponent<RCC_CarControllerV3>();
        res_mob = base.gameObject.GetComponent<RealisticEngineSound_mobile>();

        // Установка значений для RealisticEngineSound_mobile
        res_mob.maxRPMLimit = rccV3.maxEngineRPM;
        res_mob.carMaxSpeed = rccV3.maxspeed;

        // Отключение стандартных звуков двигателя RCC
        rccV3.engineSoundHigh.clip = noneClip;
        rccV3.engineSoundIdle.clip = noneClip;

        // Настройка новых звуков
        rccEngineHigh = rccV3.engineSoundHigh;
        rccEngineIdle = rccV3.engineSoundIdle;
        rccEngineReverse = car.transform.Find("All Audio Sources/Reverse Sound AudioSource").GetComponent<AudioSource>();
        rccEngineReverse.clip = noneClip;
    }

    private void Update()
    {
        if (rccV3 != null)
        {
            res_mob.engineCurrentRPM = rccV3.engineRPM;
            res_mob.carCurrentSpeed = rccV3.speed;
            res_mob.isShifting = rccV3.changingGear;

            // Логика реверса
            if (res_mob.enableReverseGear)
            {
                res_mob.isReversing = rccV3.direction == -1;
            }

            // Логика газа
            res_mob.gasPedalPressing = rccV3.gasInput >= 0.1f && !rccV3.changingGear;
        }
        else
        {
            rccV3 = car.GetComponent<RCC_CarControllerV3>();
        }
    }
}
