using UnityEngine;
using System.Collections;
using static CarClass;

namespace RGSK
{
    // Атрибут позволяет создавать объект данных чемпионата через меню в Unity Editor
    [CreateAssetMenu(fileName = "База данных автомобиля", menuName = "MRFE/Vehicle Database", order = 1)]
    public class VehicleDatabase : ScriptableObject
    {
        public VehicleData[] vehicles; // Массив данных автомобилей

        // Метод для получения данных автомобиля по уникальному идентификатору
        public VehicleData GetVehicle(string id)
        {
            for (int i = 0; i < vehicles.Length; i++)
            {
                if (vehicles[i].uniqueID == id)
                {
                    return vehicles[i];
                }
            }

            return null; // Возвращает null, если автомобиль не найден
        }

        /// <summary>
        /// Синхронизирует статус блокировки автомобилей с данными игрока.
        /// </summary>
        /// <param name="playerData">Данные игрока для синхронизации.</param>
        public void SyncVehicleData(PlayerData playerData)
        {
            if (playerData == null)
            {
                Debug.LogError("Экземпляр PlayerData равен null. Невозможно синхронизировать данные автомобилей.");
                return;
            }

            string defaultVehicleID = "vaz2101"; // Укажите уникальный идентификатор для разблокированной машины

            foreach (var vehicle in vehicles)
            {
                if (vehicle.uniqueID == defaultVehicleID)
                {
                    vehicle.isLocked = false; // Эта машина всегда разблокирована
                    Debug.Log($"Машина {vehicle.ModelName} с ID {vehicle.uniqueID} разблокирована по умолчанию.");
                }
                else
                {
                    // Остальные машины блокируются или разблокируются на основе данных игрока
                    vehicle.isLocked = !playerData.IsItemUnlocked(vehicle.uniqueID);
                }
            }
        }

        // Внутренний класс для хранения данных автомобиля
        [System.Serializable]
        public class VehicleData
        {

            
            public string ModelName; // Модель автомобиля
            public string BrandName; // Марка автомобиля
            public string year; // Год выпуска
            public string price; // Цена автомобиля
            public string TopSpeed; // Максимальная скорость
            public string Accel; // Ускорение
            public string BHP; // Лошадиные силы
            public string Mass; // Масса автомобиля
            public string EngineType; // Тип двигателя
            public string DriveTrain; // Тип привода
            public string CarClassNames; // Класс Авто
            

            // Новое поле для класса автомобиля
            public VehicleClass CarClass;

            [Space(10)]
            public string uniqueID; // Уникальный идентификатор автомобиля

            [Header("Прифаб")]
            public GameObject vehicle; // Префаб автомобиля
            public GameObject aiVehicle; // Префаб автомобиля для ИИ
            public GameObject menuVehicle; // Префаб автомобиля для меню

            [Header("Технические характеристики")]
            [Range(0, 1)]
            public float topSpeed; // Максимальная скорость (нормализованное значение от 0 до 1)

            [Range(0, 1)]
            public float acceleration; // Ускорение (нормализованное значение от 0 до 1)

            [Range(0, 1)]
            public float handling; // Управляемость (нормализованное значение от 0 до 1)

            [Range(0, 1)]
            public float braking; // Тормозные характеристики (нормализованное значение от 0 до 1)

            [Header("Прочее")]
            public Material[] bodyMaterials; // Материалы кузова автомобиля

            // Новые свойства для отображения массы, мощности и описания автомобиля
            public float mass; // Масса автомобиля
            public float power; // Мощность двигателя

            public string vehicleDescription; // Описание автомобиля

            [Header("Разблокировать систему")]
            public bool isLocked = true; // Флаг, указывающий, заблокирована ли машина
            public float unlockCost; // Стоимость разблокировки автомобиля
        }
    }
}
