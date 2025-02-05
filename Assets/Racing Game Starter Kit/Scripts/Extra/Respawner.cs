using UnityEngine;
using System.Collections;

namespace RGSK
{
    public class Respawner : MonoBehaviour
    {
        public RespawnSettings respawnSettings;
        private bool isSafe;
        private Sensor respawnSensor;
        private MeshRenderer[] renderers;
        private float flickerRate;
        private Rigidbody rigid;
        private RacerStatistics racerStatistics;
        private float lastRespawn;
        private bool isFlipped;
        private float respawnWaitTimer;


        void Awake()
        {
            //Get components
            rigid = GetComponent<Rigidbody>();
            racerStatistics = GetComponent<RacerStatistics>();

            //Get all renderers
            renderers = transform.GetComponentsInChildren<MeshRenderer>();

            //Create the sensor
            BoxCollider sensor = new GameObject("RespawnSensor").AddComponent<BoxCollider>();
            sensor.isTrigger = true;
            sensor.transform.SetParent(transform, false);
            sensor.size = Helper.GetTotalMeshFilterBounds(transform).size;
            respawnSensor = sensor.gameObject.AddComponent<Sensor>();
            respawnSensor.AddLayer(LayerMask.NameToLayer("Vehicle"));
        }


        void Update()
        {
            if (isFlipped)
            {
                respawnWaitTimer += Time.deltaTime;
                if (respawnWaitTimer > respawnSettings.respawnWait)
                {
                    Respawn();
                }
            }
            else
            {
                respawnWaitTimer = 0;
            }
        }


        void FixedUpdate()
        {
            isFlipped = transform.localEulerAngles.z > 80 && transform.localEulerAngles.z < 280;
            isSafe = respawnSensor.collidersInRange.Count == 0;
        }


        public void Respawn()
        {
            if (RaceManager.instance != null && !RaceManager.instance.raceStarted)
                return;

            if (Time.time > lastRespawn)
            {
                lastRespawn = Time.time + 5;

                if (rigid != null)
                {
                    rigid.velocity = Vector3.zero;
                    rigid.angularVelocity = Vector3.zero;
                }

                if (racerStatistics != null)
                {
                    Transform node = racerStatistics.GetPreviousNode();

                    if (node != null)
                    {
                        //Move the vehicle to the position
                        transform.position = new Vector3(node.position.x, node.position.y + 1.0f, node.position.z);
                        transform.rotation = node.rotation;

                        //Revert distance
                        racerStatistics.RevertTotalDistance();
                    }
                }

                SendMessage("ResetValues", SendMessageOptions.DontRequireReceiver);
                StartCoroutine(RespawnRoutine());
            }
        }


        IEnumerator RespawnRoutine()
        {
            gameObject.SetColliderLayer("IgnoreCollision");

            float timer = 0;

            while (timer < respawnSettings.ignoreCollisionDuration)
            {
                if (isSafe)
                {
                    timer += Time.deltaTime;
                }

                if (respawnSettings.meshFlicker)
                {
                    Flicker();
                }

                yield return null;
            }

            gameObject.SetColliderLayer("Vehicle");

            if (respawnSettings.meshFlicker)
            {
                foreach (Renderer r in renderers)
                {
                    if (!r.GetComponent<ParticleSystem>())
                        r.enabled = true;
                }
            }
        }


        void Flicker()
        {
            flickerRate = Mathf.Repeat(Time.time * 4.0f, 2);

            foreach (Renderer r in renderers)
            {
                if (!r.GetComponent<ParticleSystem>())
                    r.enabled = flickerRate < 1;
            }
        }
    }

    [System.Serializable]
    public class RespawnSettings
    {
        public bool enableRespawns = true; //are the racers able to respawn
        public bool autoRespawn; //respawn when flipped
        public bool meshFlicker; //should the mesh flicker when respawning
        public float ignoreCollisionDuration = 3; //how long should the vehicle ignore collisions when repsawning
        public float respawnWait = 5; //how long to wait when stuck or flipped over
    }
}