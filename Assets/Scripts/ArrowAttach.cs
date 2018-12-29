using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VRStandardAssets.ShootingGallery
{
    public class ArrowAttach : MonoBehaviour
    {

        private bool isAttachedToBow = false;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerStay()
        {
            Attach();
        }

        private void Attach()
        {
            if (!isAttachedToBow && OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
            {
                //Debug.Log("Pressed Trigger");
                ArrowSpawn.arrowInstance.AttachBowToArrow();
                isAttachedToBow = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("handle collision " + other.tag);
            if (other.tag != "Enemy" && other.tag != "Ground")
            {
                Attach();
            }
        }
    }
}