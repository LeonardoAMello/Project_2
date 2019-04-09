using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class WeaponHolder : MonoBehaviour
{
    public Animator animator;
    private bool scoped = false;
    private bool reloading = false;
    private float timeToStopReloading;
    private float timeToStopMelee;
    public Image crosshair;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update ()
    {
        animator.SetBool("Running", Input.GetButton("Running"));

        if (reloading && timeToStopReloading < Time.time)
            FinishReload();
        
        if(!Input.GetButton("Running"))
        {
            if (Input.GetButtonDown("Fire2") && !reloading)
            {
                scoped = !scoped;
                crosshair.enabled = !scoped;
                animator.SetBool("Scoped", scoped);
            }

            if (Input.GetButtonDown("Melee") && !reloading && Time.time > timeToStopMelee)
            {
                crosshair.enabled = false;
                Invoke("ActivateCrosshair", 1f); // Ativar crosshair quando a animação acabar
                                                 //O player atacará depois de meio segundo, pois agora só está sendo executado a animação
                Invoke("MeleeHit", .5f);
                timeToStopMelee = Time.time + 1f;
                scoped = false;
                animator.SetBool("Scoped", scoped);

                animator.SetTrigger("Melee");
            }
        }
	}

    public void ReloadAnimation(float reloadingTime)
    {
        crosshair.enabled = false;
        GetComponent<AudioSource>().Play();
        timeToStopReloading = Time.time + reloadingTime;
        reloading = true;
        GetComponentInChildren<Gun>().enabled = false;

        animator.SetBool("Scoped", false);
        animator.SetBool("Reloading", true);
    }

    private void FinishReload()
    {
        reloading = false;
        GetComponentInChildren<Gun>().enabled = enabled;
        animator.SetBool("Reloading", false);
        animator.SetBool("Scoped", scoped);
        crosshair.enabled = !scoped;
    }

    // Aqui o player executa o ataque melee de fato
    private void MeleeHit()
    {
        GetComponentInChildren<Gun>().Melee();
    }

    private void ActivateCrosshair()
    {
        crosshair.enabled = true;
    }
}
