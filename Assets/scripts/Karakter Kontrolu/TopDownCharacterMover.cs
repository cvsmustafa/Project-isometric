using UnityEngine;

[RequireComponent(typeof(InputHandler))]
public class TopDownCharacterMover : MonoBehaviour
{
    private InputHandler _input;

    private Animator _animator;

    [SerializeField]
    private bool RotateTowardMouse;

    [SerializeField]
    private float MovementSpeed;
    [SerializeField]
    private float RotationSpeed;

    [SerializeField]
    private Camera Camera;

    public IWeapon[] inventory = new IWeapon[5]; // Envanter

    private void Awake()
    {
        _input = GetComponent<InputHandler>();
        _animator = GetComponent<Animator>();

    }
    private void Start()
    {
        _animator.SetFloat("MoveY", 0);
    }

    void Update()
    {
        //silah deðiþtirme
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EquipWeapon(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            EquipWeapon(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            EquipWeapon(4);
        }

        var targetVector = new Vector3(_input.InputVector.x, 0, _input.InputVector.y);
        var movementVector = MoveTowardTarget(targetVector);

        // Karakterin yerel koordinatlarýna çevir
        movementVector = transform.InverseTransformDirection(movementVector);

        _animator.SetFloat("MoveX", movementVector.x);
        _animator.SetFloat("MoveY", movementVector.z);

        Vector3 mouseDirection = RotateFromMouseVector();

        // Hareket yönünü belirle
        Vector3 movementDirection = new Vector3(_input.InputVector.x, 0, _input.InputVector.y);

        // Mouse'un baktýðý yönle hareket yönünün açýsýný hesapla
        var angle = Vector3.Angle(mouseDirection, movementDirection);

        Debug.DrawRay(transform.position, transform.forward * 4, Color.black);
    }

    private Vector3 RotateFromMouseVector()
    {
        Ray ray = Camera.ScreenPointToRay(_input.MousePosition);
        Vector3 direction = Vector3.zero;

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f))
        {
            var target = hitInfo.point;
            target.y = transform.position.y;
            direction = target - transform.position;
            LookAtTarget(Quaternion.LookRotation(direction));
        }

        return direction.normalized;
    }

    private Vector3 MoveTowardTarget(Vector3 targetVector)
    {
        var speed = MovementSpeed * Time.deltaTime;
        targetVector = Quaternion.Euler(0, Camera.gameObject.transform.rotation.eulerAngles.y, 0) * targetVector;
        targetVector.Normalize();
        var targetPosition = transform.position + targetVector * speed;
        transform.position = targetPosition;
        return targetVector;
    }

    private void RotateTowardMovementVector(Vector3 movementDirection)
    {
        if (movementDirection.magnitude == 0) { return; }
        var rotation = Quaternion.LookRotation(movementDirection);
        LookAtTarget(rotation);
    }

    private void LookAtTarget(Quaternion targetRotation)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            other.transform.SetParent(this.transform);
            IWeapon weapon = other.GetComponent<IWeapon>();
            if (weapon != null)
            {
                other.transform.localPosition = weapon.WeaponPosition;
                other.transform.localRotation = weapon.WeaponRotation;
                AddToInventory(weapon);
            }
        }
    }

    void AddToInventory(IWeapon weapon)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = weapon;
                weapon.gameObject.SetActive(false); // Silahý görünmez yap
                break;
            }
        }
    }

    void EquipWeapon(int slot)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] != null)
            {
                if (i == slot)
                {
                    // Silahý donanýmla
                    inventory[i].gameObject.transform.SetParent(this.transform);
                    inventory[i].gameObject.transform.localPosition = inventory[i].WeaponPosition;
                    inventory[i].gameObject.transform.localRotation = inventory[i].WeaponRotation;
                    inventory[i].gameObject.SetActive(true); // Silahý görünür yap

                    // Silahýn türüne göre animasyonu güncelle
                    _animator.SetBool("IsAutomatic", inventory[i].WeaponType == "automatic");

                    // Silahý aktif yap
                    if (inventory[i].gameObject.GetComponent<automatic>() != null)
                    {
                        inventory[i].gameObject.GetComponent<automatic>().isActive = true;
                        Debug.Log("Automatic weapon is now active.");
                    }
                    else if (inventory[i].gameObject.GetComponent<semiautomatic>() != null)
                    {
                        inventory[i].gameObject.GetComponent<semiautomatic>().isActive = true;

                    }
                }
                else
                {
                    // Silahý donanýmdan çýkar
                    inventory[i].gameObject.transform.SetParent(null);
                    inventory[i].gameObject.SetActive(false); // Silahý görünmez yap

                    // Silahý pasif yap
                    if (inventory[i].gameObject.GetComponent<automatic>() != null)
                    {
                        inventory[i].gameObject.GetComponent<automatic>().isActive = false;

                    }
                    else if (inventory[i].gameObject.GetComponent<semiautomatic>() != null)
                    {
                        inventory[i].gameObject.GetComponent<semiautomatic>().isActive = false;

                    }
                }
            }
        }
    }
}