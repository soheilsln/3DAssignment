using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using System.Linq;

public class ThirdPersonShooterController : MonoBehaviour
{
    public GameObject bombPrefab;

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
    [SerializeField]
    private GameObject aimImage;
    [SerializeField]
    private float shootCooldownTime = 10f;
    private float shootTimeStamp = 0f;
    [SerializeField]
    private float punchCooldownTime = 10f;
    private float punchTimeStamp = 0f;
    private bool isWalking = false;
    [SerializeField]
    private float walkTime = 10f;
    [SerializeField]
    private float bombCooldownTime = 10f;
    private float bombTimeStamp = 0f;
    private bool keyCollected = false;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Animator animator;
    private AIController AI;

    private void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        AI = GameManager.instance.AI.GetComponent<AIController>();
        shootTimeStamp = Time.time + shootCooldownTime;
        bombTimeStamp = Time.time + bombCooldownTime;
        punchTimeStamp = Time.time + punchCooldownTime;
        keyCollected = false;
    }

    private void Update()
    {
        Vector3 aimWorldPosition = Vector3.zero;
        Vector2 screenCentrePoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCentrePoint);
        if (Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderLayerMask))
        {
            aimWorldPosition = hit.point;
        }

        if (starterAssetsInputs.aim)
        {
            animator.SetBool("Aim", true);
            aimImage.SetActive(true);
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
            aimImage.SetActive(false);
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
        }

        if (starterAssetsInputs.shoot)
        {
            starterAssetsInputs.shoot = false;
            if (shootTimeStamp <= Time.time)
            {
                Vector3 aimDir = (aimWorldPosition - spawnBulletPosition.position).normalized;
                Instantiate(bullet, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
                shootTimeStamp = Time.time + shootCooldownTime;
            }
        }

        if (starterAssetsInputs.punch)
        {
            starterAssetsInputs.punch = false;
            if (punchTimeStamp <= Time.time)
            {
                animator.SetTrigger("Punch");
                punchTimeStamp = Time.time + punchCooldownTime;
                PunchAttack();
            }
        }

        if (starterAssetsInputs.bomb)
        {
            starterAssetsInputs.bomb = false;
            if (bombTimeStamp <= Time.time)
            {
                DropBomb();
                bombTimeStamp = Time.time + bombCooldownTime;
            }
        }

        if (isWalking)
        {
            starterAssetsInputs.sprint = false;
        }

    }

    public void OnPunchAnimEnds()
    {
        //does nothing
    }

    public void SetIsWalking()
    {
        StartCoroutine(StartWalkProcess());
    }

    private IEnumerator StartWalkProcess()
    {
        if (!isWalking)
        {
            isWalking = true;
            yield return new WaitForSeconds(walkTime);
            isWalking = false;
        }
    }

    private void PunchAttack()
    {
        if (GameManager.instance.ConvertLocationToCell(transform.position).SequenceEqual(
            GameManager.instance.ConvertLocationToCell(AI.transform.position)))
        {
            AI.SetIsWalking();
        }
    }

    private void DropBomb()
    {
        Instantiate(bombPrefab, transform.position + transform.forward, Quaternion.identity);
    }

    public void CollectKey()
    {
        keyCollected = true;
    }

    public bool GetKeyCollected()
    {
        return keyCollected;
    }

}
