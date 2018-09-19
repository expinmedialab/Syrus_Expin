using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlBanners : MonoBehaviour {

    [Header("Imagenes de información del proyecto")]
    public Image[] _image;
    public float _fadeSpeed = 3f;
    int contImg = 0;
    bool pasarImg = false;
    void Start()
    {
        StartCoroutine(Fadeout(_image[contImg], _fadeSpeed));
    }

    void Update()
    {
        if (pasarImg)
        {

            pasarImg = false;
            StartCoroutine(Fadeout(_image[contImg], _fadeSpeed));
        }
        // Update is empty for this purpose.
    }

    // You can use it for multiple images, just pass that image.
    IEnumerator Fadeout(Image image, float speed)
    {
        // Will run only until image's alpha becomes completely 0, will stop after that.
        yield return new WaitForSeconds(4f);
        while (image.color.a > 0)
        {
            // You can replace WaitForEndOfFrame with WaitForSeconds.
            yield return new WaitForSeconds(0.1f);
            Color colorWithNewAlpha = image.color;
            colorWithNewAlpha.a -= speed;
            image.color = colorWithNewAlpha;

        }
        yield return new WaitForSeconds(4f);
        pasarImg = true;
        contImg++;
        if (contImg >= 3)
        {
            contImg = 0;
            Color colorSImga = image.color;
            colorSImga.a = 1f;
            _image[2].color = colorSImga;
            _image[1].color = colorSImga;
        }
        Color colorSImg = image.color;
        colorSImg.a = 1f;
        _image[contImg].color = colorSImg;

        

        //[SerializeField]
        //public Sprite[] ImagenesInformacion;
        //float fadeSpeed;
        //float targetAlpha;
        //int indexImagenInformacion;

        //[SerializeField]
        //float TiempoCambio;

        ////public Image miImagen2,miImagen3;

        //public Image[] _image;
        //public float _fadeSpeed = 0.05f;
        //bool pasarImagen = false;
        //int cont=0, contB=0;

        //public CanvasGroup[] banners;

        //bool desvanecer = false, aparecer = false;

        //int contador = 0;
        //void Start()
        //{
        //    StartCoroutine(FadeCanvasGroup(banners[contador],1,0));
        //}

        //void Update()
        //{
        //    if (desvanecer)
        //    {
        //        desvanecer = false;
        //        contador++;
        //        if (contador >= 3)
        //        {
        //            contador = 0;
        //            aparecer = true;
        //        }            
        //        StartCoroutine(FadeCanvasGroup(banners[contador],1,0));
        //    }

        //    if (aparecer)
        //    {
        //        aparecer = false;
        //    }

        //}

        //IEnumerator FadeCanvasGroup(CanvasGroup canvas, float inicioValorAlfa, float finValorAlfa, float velocidad = 0.05f)
        //{

        //    float tiempoCOmienzoDesvanecimiento = Time.time;
        //    float tiempoDesdeIniciar = Time.time - tiempoCOmienzoDesvanecimiento;
        //    float porcentajeCompletado = tiempoDesdeIniciar / velocidad;
        //    while (true)
        //    {
        //        tiempoDesdeIniciar = Time.time - tiempoCOmienzoDesvanecimiento;
        //        porcentajeCompletado = tiempoDesdeIniciar / velocidad;

        //        float valorNuevoAlfa = Mathf.Lerp(inicioValorAlfa,finValorAlfa,porcentajeCompletado);

        //        canvas.alpha = valorNuevoAlfa;

        //        if (porcentajeCompletado >= 1) break;

        //        yield return new WaitForEndOfFrame();
        //    }
        //    print("finalizado transición");

        //    if (aparecer == false && desvanecer == false)
        //    {
        //        desvanecer = true;
        //    }
        //    else if(aparecer == true)
        //    {
        //        desvanecer = false;
        //    }

    }

  


























    //void Start()
    //{

    //    StartCoroutine(Fadeout(_image[contB], _fadeSpeed));
    //}

    //void Update()
    //{
    //    // Update is empty for this purpose.
    //    if (pasarImagen)
    //    {
    //        pasarImagen = false;


    //        print(cont);           
    //        _image[cont].sprite = ImagenesInformacion[cont];
    //        Color nuevo = _image[cont].color;
    //        nuevo.a = 1;

    //        int nuevoColor = contB + 1;
    //        if (nuevoColor>=3)
    //        {
    //            nuevoColor = 0;
    //            _image[nuevoColor].color = nuevo;
    //        }
    //        else
    //        {
    //            print("valor nuevo color en pos: " + nuevoColor);
    //            _image[nuevoColor].color = nuevo;
    //        }

    //        StartCoroutine(Fadeout(_image[contB++], _fadeSpeed));

    //        if (contB >= 3)
    //        {
    //            contB = 0;
    //        }
    //        if (cont >= 3)
    //        {
    //            cont = 0;
    //        }
    //        //if (cont == 1)
    //        //{

    //        //}

    //    }

    //}

    //// You can use it for multiple images, just pass that image.
    //IEnumerator Fadeout(Image image, float speed )
    //{
    //    print(contB);
    //    // Will run only until image's alpha becomes completely 0, will stop after that.
    //    while (image.color.a > 0)
    //    {

    //        // You can replace WaitForEndOfFrame with WaitForSeconds.
    //        yield return new WaitForSeconds(0.03f);
    //        Color colorWithNewAlpha = image.color;
    //        colorWithNewAlpha.a -= speed;
    //        image.color = colorWithNewAlpha;
    //    }

    //    pasarImagen = true;
    //    print(pasarImagen+" "+cont);     
    //}

}
