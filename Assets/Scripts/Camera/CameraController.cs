using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    private Vector2 lookVector = new Vector2(0, 0);
    private float rotationPower = 0.5f;

    private void Update() {
        /*
         * Apply Look Vector
        */
        lookVector = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        /*
         * Camera Rotation Follow Target
        */
        transform.rotation *= Quaternion.AngleAxis(lookVector.x * rotationPower, Vector3.up);
        transform.rotation *= Quaternion.AngleAxis(lookVector.y * rotationPower, Vector3.right);

        var angles = transform.localEulerAngles;
        angles.z = 0;

        var angle = transform.localEulerAngles.x;

        /* Clamp the Up/Down rotation */
        if(angle > 180 && angle < 340) angles.x = 340;
        else if(angle < 180 && angle > 40) angles.x = 40;

        transform.localEulerAngles = angles;

        /*nextRotation = Quaternion.Lerp(transform.rotation, nextRotation, Time.deltaTime * rotationLerp);

        if(player.moveVector.x == 0 && player.moveVector.y == 0) {
            nextPosition = player.transform.position;

            if(aimValue == 1) {
                player.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                transform.localEulerAngles = new Vector3(angles.x, transform.rotation.eulerAngles.y, 0);
            }

            return;
        }*/

        player.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        transform.localEulerAngles = new Vector3(angles.x, transform.rotation.eulerAngles.y, 0);
    }
}
