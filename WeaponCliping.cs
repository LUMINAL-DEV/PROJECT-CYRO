using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.LightTransport.PostProcessing;

public class WeaponCliping : MonoBehaviour
{
[Header("setup")]
   public GameObject clipProjector;
   public float checkDistance;
   public Vector3 newDirection;

   float lerpPos;
   RaycastHit hit;

    // updated is called once per frame

    void Update()
    {
        if
        (Physics.Raycast(clipProjector.transform.position, clipProjector.transform.forward, out hit, checkDistance))
       {
        //get a percentage from 0  to maxdistance
        lerpPos = 1 - (hit.distance / checkDistance);
       } else
       {
        // if we hit nothing do nothing
        lerpPos = 0;
       }

       Mathf.Clamp01(lerpPos);

       transform.localRotation = 
        Quaternion.Lerp(
        Quaternion.Euler(Vector3.zero), // pointing strait ahead
        Quaternion.Euler(newDirection), //pointing off to the e side
        lerpPos // percent possition between the two
        );

    }
}
