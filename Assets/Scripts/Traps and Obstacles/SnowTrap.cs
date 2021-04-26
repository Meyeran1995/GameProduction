using UnityEngine;

public class SnowTrap : ATrap
{
    private bool playerIsInsideTrap;
    private static MainCharacterMovement MainCharacter;

    private void Awake()
    {
        if (MainCharacter == null)
            MainCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<MainCharacterMovement>();
    }

    protected override void TriggerTrap(Collider2D collision)
    {
        if(playerIsInsideTrap)
        {
            MainCharacter.RegainSpeed();
        }
        else if(collision.gameObject.CompareTag("Player"))
        {
            MainCharacter.SlowDownCharacter();
            playerIsInsideTrap = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            MainCharacter.RegainSpeed();
            playerIsInsideTrap = false;
        }
        else if (playerIsInsideTrap)
        {
            MainCharacter.SlowDownCharacter();
        }
    }
}
