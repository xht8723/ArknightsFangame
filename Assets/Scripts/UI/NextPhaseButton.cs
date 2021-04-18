using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextPhaseButton : MonoBehaviour
{
    public Text phaseStatus;
    private LevelController GM;
    public void nextPhase()
    {
        LevelController.levelController.goNextPhase();
    }

    public void attack()
    {
        LevelController.levelController.excecuteAttacks();
    }

    private void FixedUpdate()
    {
        phaseStatus.text = LevelController.levelController.roundStatus.ToString();
    }

    private void Start()
    {
        if(phaseStatus != null)
        {
            phaseStatus.text = "Move";
        }
        GM = LevelController.levelController;
    }
}
