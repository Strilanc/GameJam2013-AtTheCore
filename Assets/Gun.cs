using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {
    public GameObject projectile;
    public AudioSource gunSound;
    public bool wasPressed;
    private float gunCooldown = 0;
    public void Update() {
        gunCooldown -= Time.deltaTime;
        var pressed = Input.GetKey(KeyCode.JoystickButton2) || Input.GetKey(KeyCode.Return);
        var doShoot = gunCooldown <= 0 && pressed;
        if (doShoot) {}

        if (pressed != wasPressed) {
            if (pressed) {
                gunSound.Play();
            } else {
                gunSound.Stop();

            }
        }

        if (doShoot) {
            gunCooldown += 0.2f;
            var go = (GameObject)Instantiate(projectile, transform.position, transform.rotation);
            go.rigidbody.velocity = transform.parent.rigidbody.velocity + transform.forward*250;
        }
        gunCooldown = Mathf.Max(gunCooldown, 0);
        wasPressed = pressed;
    }
}
