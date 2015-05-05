using UnityEngine;
using System.Collections;

public class HPcont : MonoBehaviour {

    GameObject centerEyeAnchor;
    GameObject playerController;

    public float threshold_accell;
    public float threshold_staring;
    public float breaking;
    public float speed_accell;
    public float steering;


    private Vector3 localPosition_centerEyeAnchor;
    private Vector3 position_playerController;

    private bool intialized_standard;//基準が決定したか
    Vector3 localposition_standard;//基準相対位置

    Quaternion localRotation_centerEyeAnchor;
    Quaternion localRotation_standard;//基準回転

    bool fow, back;
    bool rot;

	// Use this for initialization
	void Start () {
        centerEyeAnchor = GameObject.Find("CenterEyeAnchor");
        playerController = GameObject.Find("OVRPlayerController");

       
	}

    Vector3 differencePosition_from_standard;
    //Quaternion differenceRotation_from_standard;
    float difRotRoll;
    private float difPosRot;

	// Update is called once per frame
	void Update () {
        localPosition_centerEyeAnchor = centerEyeAnchor.transform.localPosition;
        localRotation_centerEyeAnchor = centerEyeAnchor.transform.localRotation;
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            intialized_standard = true;
            localposition_standard = localPosition_centerEyeAnchor;
            localRotation_standard = localRotation_centerEyeAnchor;
        }
        if (intialized_standard)
        {
            differencePosition_from_standard = localposition_standard - localPosition_centerEyeAnchor;

            //Back or Forward
            if (differencePosition_from_standard.z * 100 >= threshold_accell)
            {
                //back
                fow = false;
                back = true;

                    //this.GetComponent<Rigidbody>().AddForce(this.transform.forward * breaking * -1.0f, ForceMode.Impulse);
                this.GetComponent<Rigidbody>().AddForce(new Vector3(0,0,0), ForceMode.VelocityChange);
                
            }
            else if(differencePosition_from_standard.z * 100 <= threshold_accell * -1.0)
            {
                //foward
                fow = true;
                back = false;

                this.GetComponent<Rigidbody>().AddForce(this.transform.forward * differencePosition_from_standard.z * speed_accell * -1.0f, ForceMode.VelocityChange);
                
            }
            else
            {
                fow = false;
                back = false;
            }

            difRotRoll = localRotation_centerEyeAnchor.z - localRotation_standard.z;
            difPosRot = Mathf.Abs(differencePosition_from_standard.x * 50 + difRotRoll * 30);

            if (difPosRot >= threshold_staring)
            {
                if (differencePosition_from_standard.x > 0)
                {
                    this.transform.Rotate(Vector3.up, difPosRot * -0.2f);
                    rot = true;
                }
                else if (differencePosition_from_standard.x < 0)
                {
                    this.transform.Rotate(Vector3.up, difPosRot * 0.2f);
                    rot = true;
                }
            }
            else
            {
                rot = false;
            }
        }

        
	}

   

}
