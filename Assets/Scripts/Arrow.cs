using System.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace VRStandardAssets.ShootingGallery
{
    public class Arrow : MonoBehaviour
    {

        private bool isFlying = false;
        //private ShootingGalleryController s;
        int score;

            // Use this for initialization
        void Start()
        {
            score = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if (isFlying)
            {
                transform.LookAt(transform.position + transform.GetComponent<Rigidbody>().velocity); //Sets the forward position of the arrow to the direction of the velocity.
            }
            //s.checkPerFrame();
        }

        void OnTriggerEnter(Collider other)
        {
            //Debug.Log(other);
            if (other.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<ShootingTarget>().m_InteractiveItem.Down();
                //other.gameObject.SetActive(false);
                this.isFlying = false;
                Destroy(this.gameObject);
                score++;

            }
            else if (other.CompareTag("Ground"))
            {
                this.isFlying = false;
                Destroy(this.gameObject);
            }
        }


        public void SetIsFlyingTrue()
        {
                isFlying = true;
        }

    }
}