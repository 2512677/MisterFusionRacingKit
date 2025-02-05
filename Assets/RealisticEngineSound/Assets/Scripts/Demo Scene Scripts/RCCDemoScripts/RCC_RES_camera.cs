using UnityEngine;
using RGSK;

public class RESCamera : MonoBehaviour
{
    public string cameraName = "Race Camera"; // Имя камеры
    private RaceCamera raceCamera; // Ссылка на RaceCamera
    public GameObject exteriorSounds; // Звуки для внешнего вида
    public GameObject interiorSounds; // Звуки для внутреннего вида
    private CameraMode currentCameraMode; // Текущий режим камеры

    void Start()
    {
        // Ищем объект камеры по имени
        GameObject cameraObject = GameObject.Find(cameraName);

        if (cameraObject == null)
        {
            Debug.LogError($"Камера с именем {cameraName} не найдена! Проверьте имя в инспекторе.");
            return;
        }

        // Попытка получить компонент RaceCamera
        raceCamera = cameraObject.GetComponent<RaceCamera>();

        if (raceCamera == null)
        {
            Debug.LogError("RaceCamera компонент не найден на объекте! Убедитесь, что компонент RaceCamera добавлен.");
            return;
        }

        // Проверяем, назначены ли звуковые объекты
        if (exteriorSounds == null || interiorSounds == null)
        {
            Debug.LogError("ExteriorSounds или InteriorSounds не назначены! Проверьте инспектор.");
            return;
        }

        // Устанавливаем начальные звуки в зависимости от режима камеры
        UpdateSounds(raceCamera.cameraMode);
    }

    void LateUpdate()
    {
        // Проверяем, что raceCamera не равен null
        if (raceCamera == null)
        {
            return;
        }

        // Проверяем, изменился ли режим камеры
        if (raceCamera.cameraMode != currentCameraMode)
        {
            UpdateSounds(raceCamera.cameraMode);
        }
    }

    /// <summary>
    /// Обновляет звуки в зависимости от текущего режима камеры.
    /// </summary>
    /// <param name="cameraMode">Текущий режим камеры</param>
    private void UpdateSounds(CameraMode cameraMode)
    {
        currentCameraMode = cameraMode;

        // Проверяем, что звуковые объекты назначены
        if (interiorSounds == null || exteriorSounds == null)
        {
            Debug.LogError("Звуковые объекты не назначены! UpdateSounds не выполнен.");
            return;
        }

        // Включаем внутренние звуки только в режиме Cockpit
        if (cameraMode == CameraMode.Cockpit)
        {
            interiorSounds.SetActive(true);
            exteriorSounds.SetActive(false);
        }
        else
        {
            interiorSounds.SetActive(false);
            exteriorSounds.SetActive(true);
        }
    }
}
