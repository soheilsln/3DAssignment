using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject fireVFX;
    private Transform player;
    private Transform AI;
    private GameManager gameManager;

    private bool onFloor = false;

    private void Awake()
    {
        gameManager = GameManager.instance;
        player = gameManager.player.transform;
        AI = gameManager.AI.transform;
        Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
        Physics.IgnoreCollision(AI.GetComponent<Collider>(), GetComponent<Collider>());
    }

    private void Update()
    {
        if (onFloor)
        {
            Explode();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Enemy"))
        {
            Explode();
            onFloor = true;
        }
    }

    private void Explode()
    {
        int[] currentCell = gameManager.ConvertLocationToCell(transform.position);

        if (!gameManager.ConvertLocationToCell(player.position).SequenceEqual(currentCell) &&
            !gameManager.ConvertLocationToCell(AI.position).SequenceEqual(currentCell))
        {
            GameObject fire = Instantiate(fireVFX, gameManager.ConvertCellToLocation(currentCell, transform.position.y), 
                Quaternion.identity);
            int scale = RandomMazeGenerator.instance.GetScale();
            fire.transform.localScale = new Vector3(scale, fire.transform.localScale.y, scale);
            fire.GetComponent<ParticleSystem>().Play(true);
            Destroy(gameObject);
        }
    }

}
