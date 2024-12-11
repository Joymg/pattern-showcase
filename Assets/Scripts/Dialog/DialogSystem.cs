using Joymg.VisualElements;
using Shapes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor.Hardware;
using UnityEngine;

// Author : JOy
namespace Joymg.Patterns.Core
{
    [ExecuteAlways]
    public class DialogSystem : ImmediateModeShapeDrawer
    {
        private const int TextSize = 50;
        #region Enums
        #endregion

        #region Consts
        #endregion

        #region Fields
        [SerializeField, TextArea] private string[] dialogs;
        [SerializeField] private Block textBlock;
        private int dialogIndex = -1;
        private Coroutine dialogCoroutine;
        #endregion

        #region Unity Methods
        #endregion

        #region Methods

        private void Start()
        {
            textBlock.Init();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if(dialogCoroutine != null)
                    StopCoroutine(dialogCoroutine);
                ++dialogIndex;
                dialogCoroutine = StartCoroutine(ShowText());
            }
        }

        private IEnumerator ShowText()
        {
            string text = dialogs[dialogIndex];
            int index = 0;
            string subText;
            while (index < text.Length)
            {
                subText = text.Substring(0, index);
                if (index % TextSize == 0)
                {
                    subText = subText + "\n\n";
                    Debug.Log("Next");
                }
                textBlock.Title.Text = subText;
                index++;
                yield return new WaitForSeconds(0.05f);
            }
        }

        public override void DrawShapes(Camera cam)
        {
            using (Draw.Command(cam))
            {
                Draw.ResetAllDrawStates(); // this makes sure no static draw states "leak" over to this scene

                Draw.LineGeometry = LineGeometry.Flat2D;
                Draw.ThicknessSpace = ThicknessSpace.Pixels;


                textBlock.DrawElement();

            }
        }
        #endregion
    }
}
