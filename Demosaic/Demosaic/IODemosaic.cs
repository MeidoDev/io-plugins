using System;
using BepInEx;
using UnityEngine;

namespace Demosaic
{
    [BepInPlugin(GUID: "meidodev.io.demosaic", Name: "IO Demosaic (trial)", Version: "0.2")]
    public class IODemosaic : BaseUnityPlugin
    {
        void OnLevelWasLoaded()
        {
            // Remove references to female mosaic meshes
            CostumeSetUp costumeSetUp = GameObject.FindObjectOfType<CostumeSetUp>();
            foreach (GameObject gameObj in costumeSetUp.MeshObj)
            {
                if (gameObj.name.Contains("moza"))
                {
                    costumeSetUp.MeshObj.Remove(gameObj);
                }
            }
        }

        void Update()
        {
            // Disable male mosaic
            MozaicSetUp mozaicSetUp = GameObject.FindObjectOfType<MozaicSetUp>();
            mozaicSetUp.MozaObj.enabled = false;

            // Disable danmen (xray) mosaic
            Renderer PC00_ute05_moza = GameObject.Find("PC00/PC0000/PC00_ute05_moza").gameObject.GetComponent<Renderer>();
            if (PC00_ute05_moza)
            {
                PC00_ute05_moza.enabled = false;
            }

            Renderer PC00_ute05_moza_ANA = GameObject.Find("PC00/PC0000/PC00_ute05_moza_ANA").gameObject.GetComponent<Renderer>();
            if (PC00_ute05_moza_ANA)
            {
                PC00_ute05_moza_ANA.enabled = false;
            }
        }
    }
}
