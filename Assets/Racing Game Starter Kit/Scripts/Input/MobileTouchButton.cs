using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using RGSK;

public class MobileTouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public InputAction inputAction;

    private float inputValue;
    public float InputValue
    {
        get
        {
            return inputValue;
        }
        set
        {
            inputValue = value;
        }
    }

    public bool pressed { get; private set; }
    public bool held { get; private set; }
    public bool released { get; private set; }

    public bool externalInput;
    public bool invert;

    // Ссылка на Respawner
    public Respawner respawner;

    void Awake()
    {
        // Автоматически найти Respawner, привязанный к машине с тегом "Player"
        if (respawner == null)
        {
            GameObject playerVehicle = GameObject.FindGameObjectWithTag("Player");
            if (playerVehicle != null)
            {
                respawner = playerVehicle.GetComponent<Respawner>();
                if (respawner == null)
                {
                   // Debug.LogError("Компонент Respawner не найден на машине игрока.");
                }
            }
            else
            {
                //Debug.LogError("Машина игрока с тегом 'Player' не найдена.");
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (externalInput)
            return;

        held = true;
        StartCoroutine(SetPressed());

        // Вызов Respawn, если он установлен
        if (respawner != null)
        {
            respawner.Respawn();
            Debug.Log("Кнопка вызвала Respawn.");
        }
        else
        {
            Debug.LogError("Respawner не привязан к кнопке.");
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (externalInput)
            return;

        held = false;
        StartCoroutine(SetReleased());
    }

    IEnumerator SetPressed()
    {
        pressed = true;
        yield return new WaitForEndOfFrame();
        pressed = false;
    }

    IEnumerator SetReleased()
    {
        released = true;
        yield return new WaitForEndOfFrame();
        released = false;
    }

    void Update()
    {
        InputValue = held ? Mathf.MoveTowards(InputValue, invert ? -1 : 1, Time.deltaTime * 5f)
                          : Mathf.MoveTowards(InputValue, 0, Time.deltaTime * 5f);
    }

    void OnDisable()
    {
        InputValue = 0;
        pressed = false;
        held = false;
        released = false;
    }
}
