using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField]
    private float normalSensitivity;
    [SerializeField]
    private float aimSensitivity;
    [SerializeField]
    private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private Transform spawnBulletPosition;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Animator animator;

    private void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector3 aimWorldPosition = Vector3.zero;
        Vector2 screenCentrePoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCentrePoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            aimWorldPosition = raycastHit.point;
        }

        if (starterAssetsInputs.aim)
        {
            animator.SetBool("Aim", true);
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);

            Vector3 worldAimTarget = aimWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);

        }
        else
        {
            animator.SetBool("Aim", false);
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
        }

        if (starterAssetsInputs.shoot)
        {
            Vector3 aimDir = (aimWorldPosition - spawnBulletPosition.position).normalized;
            Instantiate(bullet, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
            starterAssetsInputs.shoot = false;
        }

        if(starterAssetsInputs.punch)
        {
            animator.SetBool("Punch", true);
            starterAssetsInputs.punch = false;
        }
        else
        {
            animator.SetBool("Punch", false);
        }

    }
}
