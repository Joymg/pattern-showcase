using Joymg.Utils;
using Joymg.VisualElements;
using Shapes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Author : 
namespace Joymg.Patterns.Command
{
    [ExecuteAlways]
    public class CommandPatternShowcase : ImmediateModeShapeDrawer
    {
        #region Enums
        #endregion

        #region Consts
        #endregion

        #region Fields
        public Rectangle background;
        public Block inputBlock;
        public Block commandBlock;
        public Block undoListBlock;
        public Block redoListBlock;
        public List<VisualElement> temporalVisualElements;
        public bool activate;

        #endregion

        #region Unity Methods

        private void Start()
        {
            temporalVisualElements = new List<VisualElement>();
            inputBlock.Init();
            commandBlock.Init();
            undoListBlock.Init();
            redoListBlock.Init();
            commandBlock.Hide();
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                StartCoroutine(Execute());
            if (Input.GetMouseButtonDown(1))
            {
                commandBlock.transform.position = inputBlock.Position.AddToY(-0.3f);
                commandBlock.transform.localScale = Vector3.one;
            }
            //StartCoroutine(inputBlock.FadeOut(3f,Utils.EasingType.Bounce));
        }
        #endregion

        #region Methods
        public override void DrawShapes(Camera cam)
        {

            using (Draw.Command(cam))
            {
                Draw.ResetAllDrawStates(); // this makes sure no static draw states "leak" over to this scene

                Draw.LineGeometry = LineGeometry.Flat2D;
                Draw.ThicknessSpace = ThicknessSpace.Pixels;


                inputBlock.DrawElement();
                commandBlock.DrawElement();
                undoListBlock.DrawElement();
                redoListBlock.DrawElement();

                foreach (var tmp in temporalVisualElements)
                {
                    tmp.DrawElement();
                }
            }
        }

        private IEnumerator Execute()
        {
            StartCoroutine(commandBlock.FadeIn(1));
            yield return StartCoroutine(commandBlock.Scale(Vector3.one * 0.8f, 1/0.8f, 2f, EasingType.EaseOutCubic));

            Vector2 downPosition = new Vector3(commandBlock.transform.position.x, undoListBlock.transform.position.y + 1f);
            Vector2 rightPosition = new Vector3(commandBlock.transform.position.x + 1f, undoListBlock.transform.position.y);
            yield return StartCoroutine(commandBlock.MoveTo(
                downPosition.AddToY(-0.3f),
                1f,
                Utils.EasingType.EaseInQuad));
            yield return StartCoroutine(commandBlock.MoveCircularTo(
                rightPosition.AddToY(-0.3f),
                .1f));

            Block dupCommand = (Block)commandBlock.Duplicate();
            dupCommand.transform.SetPositionAndRotation(commandBlock.transform.position, commandBlock.transform.rotation);
            temporalVisualElements.Add(dupCommand);

            StartCoroutine(commandBlock.MoveTo(
                undoListBlock.Position.AddToY(-0.3f),
                .1f,
                Utils.EasingType.EaseOutQuad));
            yield return StartCoroutine(dupCommand.MoveTo(
                redoListBlock.Position.AddToY(-0.3f),
                .8f,
                Utils.EasingType.EaseOutBack));


            StartCoroutine(commandBlock.Scale(commandBlock.transform.localScale, 1.2f, 0.2f, EasingType.EaseInOutExp));
            yield return StartCoroutine(dupCommand.Scale(dupCommand.transform.localScale, 1.2f, 0.2f, EasingType.EaseInOutExp));

            
            /*StartCoroutine(undoListBlock.Scale(undoListBlock.transform.localScale, 1.3f, 0.1f, EasingType.EaseInBack));
            yield return StartCoroutine(redoListBlock.Scale(redoListBlock.transform.localScale, 1.3f, 0.1f, EasingType.EaseInBack));

          
            StartCoroutine(undoListBlock.Scale(undoListBlock.transform.localScale, 1 / 1.3f, 0.1f, EasingType.EaseOutBack));
            yield return StartCoroutine(redoListBlock.Scale(redoListBlock.transform.localScale, 1 / 1.3f, 0.1f, EasingType.EaseOutBack));*/

            StartCoroutine(commandBlock.FadeOut(1,EasingType.EaseInOutCubic));
            StartCoroutine(dupCommand.FadeOut(1,EasingType.EaseInOutCubic));
            yield return new WaitForSeconds(0.2f);

            StartCoroutine(undoListBlock.Bounce(undoListBlock.transform.localScale, 1.2f, 0.4f, EasingType.EaseInQuad));
            yield return StartCoroutine(redoListBlock.Bounce(redoListBlock.transform.localScale, 1.2f, 0.4f, EasingType.EaseInQuad));

            temporalVisualElements.Remove(dupCommand);
            Destroy(dupCommand.gameObject);

        }

        #endregion
    }
}
