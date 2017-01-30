using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssignMaterial : ScriptableWizard
{
    public Material theMaterial;
    String strHelp = "Select Game Objects";
    GameObject[] gos;

    void OnWizardUpdate()
    {
        helpString = strHelp;
        isValid = (theMaterial != null);
    }

    void OnWizardCreate()
    {
        gos = Selection.gameObjects;
        foreach (GameObject go in gos)
        {
            go.GetComponent<Renderer>().material = theMaterial;
        }
    }

    [MenuItem("Custom/Assign Material", false, 4)]
    static void assignMaterial()
    {
        ScriptableWizard.DisplayWizard("Assign Material", typeof(AssignMaterial), "Assign");
    }
}
