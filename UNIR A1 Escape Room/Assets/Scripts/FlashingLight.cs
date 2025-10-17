using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingLight : MonoBehaviour
{   
    public Light luz;
    public float tiempoEntreParpadeos = 3f;
    public float variacionTiempo = 2f;
    private float temporizador;
    private bool parpadeando;

    void Start()
    {
        if (luz == null)
            luz = GetComponent<Light>();

        ProgramarSiguienteParpadeo();
    }

    void Update()
    {
        temporizador -= Time.deltaTime;

        if (!parpadeando && temporizador <= 0f)
            StartCoroutine(Parpadear());
    }

    System.Collections.IEnumerator Parpadear()
    {
        parpadeando = true;

        int parpadeos = Random.Range(2, 5);
        for (int i = 0; i < parpadeos; i++)
        {
            luz.enabled = !luz.enabled;
            yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));
        }

        luz.enabled = true;
        parpadeando = false;
        ProgramarSiguienteParpadeo();
    }

    void ProgramarSiguienteParpadeo()
    {
        temporizador = tiempoEntreParpadeos + Random.Range(-variacionTiempo, variacionTiempo);
    }
}
