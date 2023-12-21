using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleController : MonoBehaviour
{
  //  private void StartTime

    public void Pausegame ()
    {
        Time.timeScale = 0;
    }

    public void PlayGame ()
    {
        Time.timeScale = 1;
    }
}
