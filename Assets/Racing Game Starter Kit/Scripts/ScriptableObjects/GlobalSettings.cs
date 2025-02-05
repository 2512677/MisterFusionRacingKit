using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

namespace RGSK
{
    // Атрибут позволяет создавать объект глобальных настроек через меню в Unity Editor
    [CreateAssetMenu(fileName = "Глобальные настройки", menuName = "MRFE/Global Settings", order = 1)]
    public class GlobalSettings : ScriptableObject
    {
        private static GlobalSettings _instance;

        // Свойство для получения экземпляра GlobalSettings (паттерн Singleton)
        public static GlobalSettings Instance
        {
            get
            {
                if (_instance == null)
                    _instance = Resources.Load("RGSK/ScriptableObjects/GlobalSettings") as GlobalSettings;
                return _instance;
            }
        }

        [Header("Vehicle Database")]
        public VehicleDatabase vehicleDatabase; // База данных транспортных средств

        [Header("Audio Data")]
        public AudioMixer gameAudioMixer; // Аудиомикшер для игры
    }
}
