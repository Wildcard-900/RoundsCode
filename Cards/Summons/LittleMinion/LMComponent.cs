using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MyRoundsMods.Cards.Summons.LittleMinion
{
    class LMComponent : MonoBehaviour
    {
        private void Start()
        {
            Texture2D texture = new Texture2D(100, 100);
            Color color = Color.black;
            Color[] pixels = new Color[200 * 200];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }
            texture.SetPixels(pixels);
            texture.Apply();
        }
    }
}
