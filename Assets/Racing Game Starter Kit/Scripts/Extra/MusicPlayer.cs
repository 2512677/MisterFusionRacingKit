using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RGSK
{
    public class MusicPlayer : MonoBehaviour
    {
        private AudioSource musicAudioSource; // Источник звука для воспроизведения музыки
        public AudioClip[] musicTracks; // Массив музыкальных треков
        public bool randomize; // Флаг для случайного выбора треков
        public bool autoStartMusic; // Флаг для автоматического запуска музыки при старте
        private int index = 0; // Текущий индекс воспроизводимого трека
        private int lastIndex; // Индекс последнего воспроизведённого трека

        void Start()
        {
            // Создание и настройка AudioSource
            musicAudioSource = Helper.CreateAudioSource(gameObject, null, "Музыка", 0, 1, musicTracks.Length == 1, false);

            // Отключение случайного воспроизведения, если есть только один трек
            if (musicTracks.Length <= 1)
                randomize = false;

            // Автоматический запуск музыки, если включено
            if (autoStartMusic)
                StartMusic();
        }

        /// <summary>
        /// Запускает воспроизведение музыки.
        /// </summary>
        public void StartMusic()
        {
            // Проверка, что AudioSource существует, музыка не играет и есть треки для воспроизведения
            if (musicAudioSource == null || musicAudioSource.isPlaying || musicTracks.Length == 0)
                return;

            // Выбор начального трека: первый или случайный
            index = !randomize ? 0 : Random.Range(0, musicTracks.Length);
            PlayTrack(index);
        }

        /// <summary>
        /// Воспроизводит следующий трек в списке.
        /// </summary>
        public void PlayNext()
        {
            index++;

            // Циклический переход к первому треку после последнего
            index = index % musicTracks.Length;
            PlayTrack(index);
        }

        /// <summary>
        /// Воспроизводит случайный трек, отличающийся от последнего.
        /// </summary>
        public void PlayRandom()
        {
            int temp = 0;

            Init:
            while (true)
            {
                temp = Random.Range(0, musicTracks.Length);

                if (temp == lastIndex)
                {
                    goto Init; // Повторный выбор, если выбран тот же трек
                }

                goto Done;
            }
            Done:
            PlayTrack(temp);
        }

        /// <summary>
        /// Перезаписывает текущий музыкальный трек новым и настраивает его параметры.
        /// </summary>
        /// <param name="clip">Новый аудиоклип для воспроизведения.</param>
        /// <param name="loop">Флаг зацикливания трека.</param>
        public void OverrideMusicClip(AudioClip clip, bool loop)
        {
            if (musicAudioSource == null)
                return;

            musicAudioSource.clip = clip;
            musicAudioSource.loop = loop;
            musicAudioSource.Play();
        }

        /// <summary>
        /// Воспроизводит трек по указанному индексу.
        /// </summary>
        /// <param name="i">Индекс трека для воспроизведения.</param>
        void PlayTrack(int i)
        {
            musicAudioSource.clip = musicTracks[i];
            musicAudioSource.Play();
            lastIndex = i;

            // Запуск корутины для ожидания окончания трека
            StartCoroutine(WaitForMusic(musicAudioSource.clip.length));
        }

        /// <summary>
        /// Ожидает окончания воспроизведения трека и вызывает метод TrackFinished.
        /// </summary>
        /// <param name="duration">Длительность трека.</param>
        /// <returns></returns>
        IEnumerator WaitForMusic(float duration)
        {
            float time = 0;

            while (time < duration + 0.5f)
            {
                time += Time.deltaTime;
                yield return null;
            }

            TrackFinished();
        }

        /// <summary>
        /// Обрабатывает окончание воспроизведения трека, выбирая следующий трек для воспроизведения.
        /// </summary>
        void TrackFinished()
        {
            // Проверка, что музыка действительно не играет
            if (musicAudioSource.isPlaying)
                return;

            // Выбор следующего трека: последовательный или случайный
            if (!randomize)
            {
                PlayNext();
            }
            else
            {
                PlayRandom();
            }
        }


        public void SetVolume(float volume)
        {
            if (musicAudioSource != null)
            {
                musicAudioSource.volume = volume;
            }
        }

            /// <summary>
            /// Приостанавливает воспроизведение музыки.
            /// </summary>
            public void Pause()
        {
            if (musicAudioSource != null)
            {
                musicAudioSource.Pause();
            }
        }

        /// <summary>
        /// Возобновляет воспроизведение музыки после приостановки.
        /// </summary>
        public void UnPause()
        {
            if (musicAudioSource != null)
            {
                musicAudioSource.UnPause();
            }
        }
    }
}
