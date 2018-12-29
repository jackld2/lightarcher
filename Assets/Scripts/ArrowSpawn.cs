using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRStandardAssets.ShootingGallery
{
    public class ArrowSpawn : MonoBehaviour
    {

        private const float ARROWSPEEDSCALAR = 50;
        private const float BOWSTRINGSCALAR = 7;

        public GameObject arrow;
        public static ArrowSpawn arrowInstance;
        private GameObject currentArrow;
        public GameObject arrowHand;
        public GameObject bowHand;
        public GameObject bowstring;
        public GameObject bow;
        public AudioSource firesound;
        public AudioSource pullbackSound;

        public GameObject arrowStartPoint;

        public GameObject stringStartPoint;

        private bool arrowAttached = false;

        void Awake()
        {
            if (arrowInstance == null)
            {
                arrowInstance = this;
            }
        }
        void OnDestroy()
        {
            if (arrowInstance == this)
            {
                arrowInstance = null;
            }
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            AttachArrow();
            Pull();
        }

        //Attaches the arrow to the hand of the player
        public void AttachArrow()
        {
            if (currentArrow == null)
            {
                currentArrow = Instantiate(arrow);
                currentArrow.transform.position = arrowHand.transform.position;
                currentArrow.transform.parent = arrowHand.transform;
                currentArrow.transform.localPosition = new Vector3(0f, 0f, 0.334f);
                currentArrow.transform.localRotation = Quaternion.identity;

            }
        }


        //Attaches the arrow to the bow (or bow to the arrow)
        public void AttachBowToArrow()
        {
            currentArrow.transform.parent = bowstring.transform;
            currentArrow.transform.position = arrowStartPoint.transform.position;
            currentArrow.transform.rotation = arrowStartPoint.transform.rotation;
            arrowAttached = true;
            pullbackSound.Play();
        }

        //***FOR FUTURE: SET BOW STRING PULL DISTANCE LIMIT
        //Called when the player pulls the string
        private void Pull()
        {
            if (arrowAttached)
            {
                bow.transform.rotation = Quaternion.LookRotation(bowHand.transform.position - arrowHand.transform.position, bowHand.transform.up);
                //Debug.Log(-bowHand.transform.forward);
                float distance = Vector3.Distance(stringStartPoint.transform.position, arrowHand.transform.position);
                //Debug.Log(distance);
                pullbackSound.pitch = distance / 0.5f;
                bowstring.transform.localPosition = stringStartPoint.transform.localPosition + new Vector3(BOWSTRINGSCALAR * distance, 0f, 0f); //Scales how far the bow string pulls back
                float velocity = distance * ARROWSPEEDSCALAR; //determines power of the arrow

                if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
                {
                    FireArrow(velocity);
                }

            }
        }

        private void FireArrow(float velocity)
        {
            currentArrow.transform.parent = null;
            TrailRenderer trail = currentArrow.GetComponent<TrailRenderer>();
            trail.enabled = true;
            firesound.Play();
            pullbackSound.Stop();
            //trail.widthMultiplier = 0.1f;
            //trail.startColor = new Color(30, 191, 116);
            currentArrow.GetComponent<Arrow>().SetIsFlyingTrue();
            Rigidbody arrowRB = currentArrow.GetComponent<Rigidbody>();
            arrowRB.velocity = currentArrow.transform.forward * velocity;
            arrowRB.useGravity = true;
            currentArrow = null;
            arrowAttached = false;
            bowstring.transform.localPosition = stringStartPoint.transform.localPosition;
            bow.transform.localRotation = Quaternion.Euler(0, 0, 0);

        }
    }
}