using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketHandler : MonoBehaviour
{
    // Start is called before the first frame update
    float speedFactor = 1;
    bool rocketUp = false;
    bool calledForLaunch = false;
    bool rocketDown = false;
    float initialHeight = 0;
    public GameObject DownButton;
    public GameObject upButton;
    static GameObject UpButton;
    GameObject ShadowPlane1;
    bool isCounting = false;
    float timer = 0;
    float duration = 0;
    void Start()
    {
        DownButton.SetActive(false);
        upButton.SetActive(false);
        UpButton = upButton;
    }

    // Update is called once per frame
    void Update()
    {
        if(ShadowPlane1 == null)
        {
            ShadowPlane1 = GameObject.FindWithTag("ShadowPlane");
            if (ShadowPlane1 != null)
            {
                Debug.Log(ShadowPlane1.transform.name);
            }

        }

        if (ShadowPlane1 != null)
        {
            if (PlaceOnPlane.isObjectPlaced)
            {
                ShadowPlane1.transform.parent = null;
                ShadowPlane1.transform.localScale = new Vector3(1, 1, 1);
                ShadowPlane1.transform.rotation = Quaternion.Euler(0, 0, 0);
                ShadowPlane1.SetActive(true);
                if (!calledForLaunch)
                {
                    EnableButton();
                }
                ShadowPlane1.transform.position = new Vector3(PlaceOnPlane.spawnedObject.transform.position.x, -0.6f, PlaceOnPlane.spawnedObject.transform.position.z);
            }
            rocketUpDown();
        }
 
    }

    public void RocketUp()
    {
        calledForLaunch = true;
        initialHeight = PlaceOnPlane.spawnedObject.transform.position.y;
        ShadowPlane1.transform.GetChild(1).gameObject.SetActive(true);
        ShadowPlane1.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Play();
        upButton.SetActive(false);
        isCounting = true;
        rocketUp = true;
        duration = 5f;
    }
    public void RocketDown()
    {
        rocketDown = true;
        DownButton.SetActive(false);
    }



    public void rocketUpDown()
    {
        wait();
        if (PlaceOnPlane.isObjectPlaced)
        {
            if (rocketUp &&  !isCounting)
            {
                PlaceOnPlane.spawnedObject.transform.GetChild(1).gameObject.SetActive(true);
                PlaceOnPlane.spawnedObject.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Play();
                if (PlaceOnPlane.spawnedObject.transform.position.y < initialHeight + 120  )
                {
                    if(PlaceOnPlane.spawnedObject.transform.position.y > initialHeight + 4)
                    {
                        PlaceOnPlane.spawnedObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                        ShadowPlane1.transform.GetChild(0).gameObject.SetActive(true);
                        if (ShadowPlane1.transform.GetChild(0).gameObject.transform.localScale.x <= 0.25f)
                        {
                            ShadowPlane1.transform.GetChild(0).gameObject.transform.localScale += new Vector3(Time.deltaTime/10, Time.deltaTime/10, Time.deltaTime/10);
                        }
                    }
                    else
                    {
                        PlaceOnPlane.spawnedObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    }

                    if (PlaceOnPlane.spawnedObject.transform.position.y > initialHeight + 10)
                    {
                        speedFactor = Mathf.Lerp(1, 4, 2);
                        PlaceOnPlane.spawnedObject.transform.position += new Vector3(0, Time.deltaTime* speedFactor, 0);
                    }
                    else
                    {
                        speedFactor = Mathf.Lerp(6, 1, 6);
                        PlaceOnPlane.spawnedObject.transform.position += new Vector3(0, Time.deltaTime * speedFactor, 0);
                    }
                }
                else
                {
                    Vector3.Lerp(PlaceOnPlane.spawnedObject.transform.localScale, new Vector3(0, 0, 0), 5);
                    if (PlaceOnPlane.spawnedObject.transform.localScale.magnitude <= 0)
                    {
                        rocketUp = false;
                    }    
                }
            }
            else if (rocketDown)
            {
                if (PlaceOnPlane.spawnedObject.transform.position.y > initialHeight + 120)
                {
                    speedFactor = Mathf.Lerp(1, 2.5f, 2);
                    PlaceOnPlane.spawnedObject.transform.position -= new Vector3(0, Time.deltaTime * speedFactor, 0);
                }
                else if (PlaceOnPlane.spawnedObject.transform.position.y > initialHeight)
                {
                    speedFactor = Mathf.Lerp(2.5f, 1, 3);
                    PlaceOnPlane.spawnedObject.transform.position -= new Vector3(0, Time.deltaTime * speedFactor, 0);
                    ShadowPlane1.transform.GetChild(2).gameObject.GetComponent<ParticleSystem>().Play();
                }
                else
                {
                    upButton.SetActive(true);
                    rocketDown = false;
                }
            }
        }   
    }

    void wait()
    {
        if (isCounting)
        {
            Debug.Log(timer);
            if (timer < duration)
            {
                if (timer > (duration-0.2f) && !PlaceOnPlane.spawnedObject.transform.GetChild(2).gameObject.GetComponent<ParticleSystem>().isPlaying)
                {
                    PlaceOnPlane.spawnedObject.transform.GetChild(2).gameObject.SetActive(true);
                    PlaceOnPlane.spawnedObject.transform.GetChild(2).gameObject.GetComponent<ParticleSystem>().Play();
                    PlaceOnPlane.spawnedObject.transform.GetChild(1).gameObject.SetActive(true);
                    PlaceOnPlane.spawnedObject.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Play();
                }
                timer += Time.deltaTime;
            }
            else
            {
                PlaceOnPlane.spawnedObject.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Stop();
                isCounting = false;
                timer = 0;
            }
        }
       
    }

    public static void EnableButton()
    {
        UpButton.SetActive(true);

    }

}
