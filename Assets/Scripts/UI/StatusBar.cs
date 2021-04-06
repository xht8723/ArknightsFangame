using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    public bool healthBarEnabled = true;

    public GameObject healthBar;
    public GameObject innerHealthBar;

    public Unit unit;

    // Start is called before the first frame update
    void Start()
    {
        if (!healthBarEnabled) {
            disableHealthBar();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (unit == null) {
            return;
        }

        if (healthBarEnabled) {
            updateHealthBar();
        }

    }

    private void updateHealthBar() {
        int curHealth = unit.Status.hp;
        int maxHealth = unit.Status.maxHp;

        if (maxHealth <= 0) {
            innerHealthBar.GetComponent<Image>().fillAmount = 0f;
        } else {
            innerHealthBar.GetComponent<Image>().fillAmount = (float)curHealth / (float)maxHealth;
        }
    }


    public void enableHealthBar() {
        healthBarEnabled = true;
        healthBar.SetActive(true);
    }

    public void disableHealthBar() {
        healthBarEnabled = false;
        healthBar.SetActive(false);
    }
}
