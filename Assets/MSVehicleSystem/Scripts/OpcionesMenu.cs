using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class OpcionesMenu : MonoBehaviour
{


	public void Info()
	{
		SceneManager.LoadScene ("Info");
	}

	public void Salir()
	{
		Application.Quit();
	}

    public void ModoEntrenamiento()
    {
        SceneManager.LoadScene("SimuladorJaviXue");

    }

    public void ModoAutonomo()
    {
        SceneManager.LoadScene("SimuladorJaviXueAuto");
    }

}
