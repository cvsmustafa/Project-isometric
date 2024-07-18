using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject player; // Player nesnesi
    private GameObject currentWeapon; // Þu anda kullanýlan silah

    void Start()
    {
        // Baþlangýçta hiçbir silah aktif deðil
        currentWeapon = null;
    }

    void Update()
    {
        foreach (Transform child in player.transform)
        {
            if (child.gameObject.CompareTag("Weapon"))
            {
                if (currentWeapon != child.gameObject)
                {
                    if (currentWeapon != null)
                    {
                        var semiAuto = currentWeapon.GetComponent<semiautomatic>();
                        if (semiAuto != null)
                        {
                            semiAuto.isActive = false;
                        }
                        else
                        {
                            var auto = currentWeapon.GetComponent<automatic>();
                            if (auto != null)
                            {
                                auto.isActive = false;
                            }
                        }
                    }

                    currentWeapon = child.gameObject;
                    var newSemiAuto = currentWeapon.GetComponent<semiautomatic>();
                    if (newSemiAuto != null)
                    {
                        newSemiAuto.isActive = true;
                    }
                    else
                    {
                        var newAuto = currentWeapon.GetComponent<automatic>();
                        if (newAuto != null)
                        {
                            newAuto.isActive = true;
                        }
                    }
                }

                break;
            }
        }
    }
}