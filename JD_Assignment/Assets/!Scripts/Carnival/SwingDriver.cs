using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SwingDriver : MonoBehaviour
{

    private Swing swing = null;
    [SerializeField] private SwingType type;

    //Merry Go Round fields
    [Range(0.1f, 150f)]
    [SerializeField] private float rotationSpeed;
    [Space()]
    [Space()]
    [Header("Merry Go Round , Umbrella")]
    [SerializeField] private bool isClockwise;
    [SerializeField] private Transform rotationObject;

    [Header("Wheel")]
    [SerializeField] private Animator wheelAnim;

    [Header("Show")]
    [SerializeField] private Transform showPlacePointHolder;
    [Range(0.1f, 150f)]
    [SerializeField] private float moveSpeed;

    private void OnEnable()
    {


        switch (type)
        {
            case SwingType.MERRY_GO_ROUND:
                swing = new Merry_Go_Round(rotationSpeed, isClockwise , (rotationObject == null) ? (this.transform) : (rotationObject));
                break;
            case SwingType.WHEEL:
                swing = new Wheel(wheelAnim);
                break;
            case SwingType.UMBRELLA:
                swing = new Umbrella(rotationSpeed, isClockwise, (rotationObject == null) ? (this.transform) : (rotationObject));
                break;
            case SwingType.SHOW:
                swing = new Show(moveSpeed , showPlacePointHolder);
                break;
            default:
                break;
        }

        
    }


    void Start()
    {
        swing.EnableSwing();
    }

    // Update is called once per frame
    void Update()
    {
        swing.UpdateSwing();
    }

    private void OnDisable()
    {
        swing.DisableSwing();
    }

    public SwingType GetSwingType() => type;




}


[System.Serializable]
public enum SwingType
{
    NONE,
    MERRY_GO_ROUND,
    WHEEL,
    SHOW,
    TRACK,
    UMBRELLA
}


