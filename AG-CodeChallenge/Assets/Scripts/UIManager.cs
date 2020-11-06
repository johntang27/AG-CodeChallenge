using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject targetPolygon;

    [Header("UI Elements")]
    public InputField sidesInputField;
    public InputField radiusInputField;
    public Button customShapeButton;

    private int sidesInput;
    private float radiusInput;

    public void CreateRandomShape()
    {
        ICreatePolygon target = targetPolygon.GetComponent<ICreatePolygon>();
        target.CreateRandomPolygon();
    }

    public void SidesInputOnEnd()
    {
        if (sidesInputField.text == "") return;

        sidesInput = int.Parse(sidesInputField.text);

        if(sidesInput < 3)
        {
            UpdatePlaceholderMSG(sidesInputField, "Not enough sides, min 3", true);
            sidesInput = 0;

            return;
        }

        UpdatePlaceholderMSG(sidesInputField, "Enter # of Sides...");
        customShapeButton.interactable = radiusInput >= 1;
    }

    public void RadiusInputOnEnd()
    {
        if (radiusInputField.text == "") return;

        radiusInput = float.Parse(radiusInputField.text);

        if(radiusInput < 1)
        {
            UpdatePlaceholderMSG(radiusInputField, "Radius too small, min 1", true);
            radiusInput = 0;

            return;
        }

        UpdatePlaceholderMSG(radiusInputField, "Enter Radius...");
        customShapeButton.interactable = sidesInput >= 3;
    }

    public void CreateCustomShape()
    {
        ICreatePolygon target = targetPolygon.GetComponent<ICreatePolygon>();
        target.CreatePolygon(sidesInput, radiusInput);
    }

    private void UpdatePlaceholderMSG(InputField field, string msg, bool isInValid = false)
    {
        field.placeholder.GetComponent<Text>().text = msg;

        if (isInValid)
        {
            field.placeholder.GetComponent<Text>().color = Color.red;
            field.text = "";
            customShapeButton.interactable = false;
        }
        else
        {
            field.placeholder.GetComponent<Text>().color = Color.grey;
        }        
    }

    public void SetTarget(GameObject target)
    {
        targetPolygon.SetActive(false);
        targetPolygon = target;
        targetPolygon.SetActive(true);
    }

    public void ToGameDemo()
    {
        SceneManager.LoadScene(1);
    }
}
