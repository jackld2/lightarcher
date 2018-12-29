using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRStandardAssets.ShootingGallery
{
    public class TargetController : MonoBehaviour
    {

        public GameObject prefab;
        private ShootingTarget[] targetWaves;
        private List<ShootingTarget> targets;
        private GameObject[] startMarkers;
        private int waveOne = 3;
        private float startTime;
        public Transform playerPosition;


        ///////////Leveling Controls////////
        public int movementType;
        public float rotationSpeed;
        int wave;

        private float[] journeyLength;

        ////////////spherical//////////////
        private int radius = 20;
        private int numTargets = 5;
        private float speed = 1.0f;
        private int targetsDestroyed = 0;
        // Use this for initialization
        void Start()
        {
            wave = 0;
            targets = new List<ShootingTarget>();
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log(targets.Count);
            if (targets.Count < numTargets)
            {
                Spawn_spherical();
            }
            else
            {
                for (int i = targets.Count - 1; i >= 0; --i)
                {
                    if (!targets[i].GetComponent<Renderer>().enabled)
                    {
                        targets.RemoveAt(i);
                        targetsDestroyed++;
                    }
                }
                Debug.Log(targets.Count);
            }
            moveTargetsSpherical();
            //if (waveOver())
            //{
            //    Spawn();
            //}
            //else
            //{
            //    moveTargets();
            //}
        }

        private void moveTargetsSpherical()
        {
            for (int i = 0; i < targets.Count; ++i)
            {
                targets[i].transform.LookAt(playerPosition);
                targets[i].GetComponent<Rigidbody>().velocity = targets[i].transform.forward * speed;
                Debug.Log(targets[i].transform.forward);

            }
        }

        private void Spawn_spherical()
        {
            Vector3 coord = new Vector3();
            coord.x = Random.Range(-.5f, .5f);
            coord.x += coord.x <= 0 ? -.5f : .5f;
            coord.z = Random.Range(-.5f, .5f);
            coord.z += coord.z <= 0 ? -.5f : .5f;
            coord.y = Mathf.Sqrt(1 - coord.x * coord.x + coord.y * coord.y) + .5f;

            coord *= radius;

            ShootingTarget temp = Instantiate(prefab, coord, Quaternion.identity).GetComponent<ShootingTarget>();
            temp.Restart();
            temp.GetComponent<Renderer>().enabled = true;
            temp.GetComponent<Collider>().enabled = true;

            targets.Add(temp);
        }

        void moveTargets()
        {
            if (movementType == 0)
            {
                float distCovered = (Time.time - startTime) * speed;
                //Debug.Log(Time.time - startTime);
                float[] fracJourney = new float[journeyLength.Length];
                for (int i = 0; i < fracJourney.Length; ++i)
                {
                    //Debug.Log(journeyLength[i]);
                    fracJourney[i] = distCovered / journeyLength[i];
                }

                for (int i = 0; i < targetWaves.Length; ++i)
                {
                    targetWaves[i].transform.position = Vector3.Lerp(startMarkers[i].transform.position, playerPosition.position, fracJourney[i]);
                    targetWaves[i].transform.LookAt(playerPosition);

                    if (fracJourney[i] >= 0.9)
                    {
                        //game over condition
                        Quit();
                    }
                }
            }
            else if (movementType == 1)
            {
                for (int i = 0; i < targetWaves.Length; ++i)
                {
                    targetWaves[i].transform.RotateAround(playerPosition.position, Vector3.up, rotationSpeed * Time.deltaTime);
                    targetWaves[i].transform.LookAt(playerPosition);

                }
            }
        }




        private void Spawn()
        {
            // Get a reference to a target instance from the object pool.
            targetWaves = new ShootingTarget[waveOne];
            startMarkers = new GameObject[waveOne];

            targetWaves[0] = Instantiate(prefab, new Vector3(-5, 3, 8), Quaternion.identity).GetComponent<ShootingTarget>();
            //targetWaves[0].transform.position = new Vector3(-5, 3, 8);
            startMarkers[0] = new GameObject();
            startMarkers[0].transform.position = new Vector3(-5, 3, 8);
            //targetWaves[0].GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -1);


            targetWaves[0].GetComponent<ShootingTarget>().Restart();



            for (int i = 1; i < targetWaves.Length; i++)
            {
                Vector3 prevPos = targetWaves[i - 1].transform.position;

                targetWaves[i] = Instantiate(prefab, new Vector3(prevPos.x + 3, prevPos.y, prevPos.z), Quaternion.identity).GetComponent<ShootingTarget>();
                startMarkers[i] = new GameObject();
                startMarkers[i].transform.position = new Vector3(prevPos.x + 3, prevPos.y, prevPos.z);


                // Find a reference to the ShootingTarget script on the target gameobject and call it's Restart function.
                //ShootingTarget shootingTargetloop = targetWaves[i].GetComponent<ShootingTarget>();
                targetWaves[i].GetComponent<ShootingTarget>().Restart();
                targetWaves[i].GetComponent<Renderer>().enabled = true;
                targetWaves[i].GetComponent<Collider>().enabled = true;
                // Subscribe to the OnRemove event.
                //shootingTargetloop.OnRemove += HandleTargetRemoved;
            }

            // Keep a note of the time the movement started.
            startTime = Time.time;

            // Calculate the journey length.
            journeyLength = new float[targetWaves.Length];
            for (int i = 0; i < journeyLength.Length; i++)
            {
                //Debug.Log(startMarkers[i].transform.position);
                journeyLength[i] = Vector3.Distance(startMarkers[i].transform.position, playerPosition.position);

            }
        }


        private bool waveOver()
        {
            if (targetWaves == null)
            {
                return true;
            }
            for (int i = 0; i < targetWaves.Length; ++i)
            {
                if (targetWaves[i].enabled)
                    return false;
            }
            wave++;
            return true;
        }

        private void Quit()
        {
#if UNITY_STANDALONE
            //Quit the application
            Application.Quit();
#endif

            //If we are running in the editor
#if UNITY_EDITOR
            //Stop playing the scene
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }

}