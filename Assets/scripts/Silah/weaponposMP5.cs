using UnityEngine;

public class weaponposMP5 : MonoBehaviour, IWeapon
{
    public Vector3 WeaponPosition => new Vector3(0.081f, 1.067f, 0.336f);
    public Quaternion WeaponRotation => Quaternion.Euler(0f, 0f, 0f);
    public string WeaponType => "automatic";
    //public GameObject gameObject => this.gameObject;

}
