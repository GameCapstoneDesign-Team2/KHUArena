using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeItems : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Sheild;
    public GameObject Sword;

    public GameObject Warrior_Sheild;
    public GameObject Warrior_Sword;

    public Transform SheildTr;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.V))
        {
            Warrior_Sword.SetActive(true);
            Warrior_Sheild.SetActive(true);

            Sheild.SetActive(false);
            Sword.SetActive(false);
            StartCoroutine("ShieldRotate");
        }
    }

    IEnumerator ShieldRotate()
    {
        yield return new WaitForSeconds(0.1f);
        
        //SheildTr.transform.rotation = new Quaternion(318, 300, 313, 1);

       // Shield.transform.rotation.x = 328;
        //∏Ò«• 348 211 373
        //308 322 292
    }
}
