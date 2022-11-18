using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject[] weapons;
    private int index;
    public float switchDelay = 1f;
    private bool isSwitching;

    // Start is called before the first frame update
    void Start()
    {
        InitializeWeapons();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && !isSwitching)
        {
            index++;

            if (index >= weapons.Length)
            {
                index = 0;
            }

            StartCoroutine(SwitchWeaponDelay(index));
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && !isSwitching)
        {
            index--;

            if (index < 0)
            {
                index = weapons.Length - 1;
            }

            StartCoroutine(SwitchWeaponDelay(index));
        }
    }

    IEnumerator SwitchWeaponDelay(int newIndex)
    {
        isSwitching = true;
        yield return new WaitForSeconds(1f);
        isSwitching= false;
        SwitchWeapons(newIndex);
    }

    private void InitializeWeapons()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
        }

        weapons[0].SetActive(true);
    }

    private void SwitchWeapons(int newIndex)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
        }

        weapons[newIndex].SetActive(true);
    }
}
