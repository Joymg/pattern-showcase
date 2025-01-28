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
        public Block wBlock;
        public Block aBlock;
        public Block sBlock;
        public Block dBlock;

        public Block commandBlock;
        public Block undoListBlock;
        public Block redoListBlock;
        public List<VisualElement> temporalVisualElements;
        public bool activate;

        public int differences;
        public bool FirstInput = true;
        public Direction CurrentTriggeredDirection;
        public Direction LastTriggeredDirection;

        public int test = 0;
        private Coroutine _startCoroutine;

        #endregion

        #region Unity Methods

        private void Start()
        {

            GameManager.Instance.upExecute += OnInputPressed;
            GameManager.Instance.rightExecute += OnInputPressed;
            GameManager.Instance.downExecute += OnInputPressed;
            GameManager.Instance.leftExecute += OnInputPressed;
            temporalVisualElements = new List<VisualElement>();
            inputBlock.Init();
            wBlock.Init();
            aBlock.Init();
            sBlock.Init();
            dBlock.Init();
            commandBlock.Init();
            undoListBlock.Init();
            redoListBlock.Init();

            commandBlock.Hide();
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                test++;
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
                wBlock.DrawElement();
                aBlock.DrawElement();
                sBlock.DrawElement();
                dBlock.DrawElement();
                undoListBlock.DrawElement();
                redoListBlock.DrawElement();

                commandBlock.DrawElement();

                foreach (var tmp in temporalVisualElements)
                {
                    tmp.DrawElement();
                }
            }
        }

        private IEnumerator Execute(Block block)
        {
            commandBlock.transform.position = block.Position;

            StartCoroutine(block.Bounce(block.transform.localScale, 1.2f, 0.5f, EasingType.EaseInExp));
            yield return new WaitForSeconds(0.25f);

            StartCoroutine(commandBlock.FadeIn(1, EasingType.EaseInOutCubic, false));
            yield return StartCoroutine(commandBlock.Scale(Vector3.one * 0.8f, Vector3.one / 0.8f, 1f, EasingType.EaseOutCubic));



            if (!RepeatingInput() && differences <= 1)
            {
                yield return StartCoroutine(commandBlock.MoveTo(
                commandBlock.Parent.transform.position,
                1f,
                EasingType.EaseOutQuad));

                StartCoroutine(commandBlock.Scale(commandBlock.transform.localScale, new Vector2(9, 10), 1f, EasingType.EaseOutBack));
                yield return new WaitForSeconds(0.2f);
                yield return StartCoroutine(commandBlock.Title.FadeIn(0.4f, EasingType.EaseInOutQuad));
                yield return new WaitForSeconds(0.4f);

                while (test < 1)
                {
                    yield return null;
                }

                StartCoroutine(commandBlock.Scale(commandBlock.transform.localScale, Vector3.one, 1f, EasingType.EaseInBack));
                yield return new WaitForSeconds(0.2f);
                yield return StartCoroutine(commandBlock.Title.FadeOut(0.4f, EasingType.EaseInOutQuad));
                yield return new WaitForSeconds(0.6f);
            }


            Vector2 downPosition = new Vector3(commandBlock.transform.position.x, undoListBlock.transform.position.y + 1f - 0.3f);
            Vector2 rightPosition = new Vector3(commandBlock.transform.position.x, undoListBlock.transform.position.y - 0.3f);

            yield return StartCoroutine(commandBlock.MoveTo(
                downPosition.AddToY(-0.3f),
                .8f,
                EasingType.EaseInQuad));

            Block dupCommand = (Block)commandBlock.Duplicate();
            dupCommand.transform.SetPositionAndRotation(commandBlock.transform.position, commandBlock.transform.rotation);
            temporalVisualElements.Add(dupCommand);
            StartCoroutine(commandBlock.MoveCircularTo(
                rightPosition.AddToX(-1f),
                .1f));
            yield return StartCoroutine(dupCommand.MoveCircularTo(
                rightPosition.AddToX(1f),
                .1f));


            StartCoroutine(commandBlock.MoveTo(
                undoListBlock.Position,
                .8f,
                EasingType.EaseOutQuad));
            yield return StartCoroutine(dupCommand.MoveTo(
                redoListBlock.Position,
                .8f,
                EasingType.EaseOutQuad));


            while (test < 3)
            {
                yield return null;
            }

            StartCoroutine(commandBlock.Scale(commandBlock.transform.localScale, commandBlock.transform.localScale * 1.2f, 0.2f, EasingType.EaseInOutExp));
            yield return StartCoroutine(dupCommand.Scale(dupCommand.transform.localScale, commandBlock.transform.localScale * 1.2f, 0.2f, EasingType.EaseInOutExp));


            /*StartCoroutine(undoListBlock.Scale(undoListBlock.transform.localScale, 1.3f, 0.1f, EasingType.EaseInBack));
            yield return StartCoroutine(redoListBlock.Scale(redoListBlock.transform.localScale, 1.3f, 0.1f, EasingType.EaseInBack));

          
            StartCoroutine(undoListBlock.Scale(undoListBlock.transform.localScale, 1 / 1.3f, 0.1f, EasingType.EaseOutBack));
            yield return StartCoroutine(redoListBlock.Scale(redoListBlock.transform.localScale, 1 / 1.3f, 0.1f, EasingType.EaseOutBack));*/

            StartCoroutine(commandBlock.FadeOut(1, EasingType.EaseInOutCubic, false));
            StartCoroutine(dupCommand.FadeOut(1, EasingType.EaseInOutCubic, false));
            yield return new WaitForSeconds(0.2f);

            StartCoroutine(undoListBlock.Bounce(undoListBlock.transform.localScale, 1.2f, 0.4f, EasingType.EaseInQuad));
            yield return StartCoroutine(redoListBlock.Bounce(redoListBlock.transform.localScale, 1.2f, 0.4f, EasingType.EaseInQuad));

            temporalVisualElements.Remove(dupCommand);
            Destroy(dupCommand.gameObject);
            FirstInput = false;
        }

        public void OnInputPressed(Direction direction)
        {
            LastTriggeredDirection = CurrentTriggeredDirection;
            CurrentTriggeredDirection = direction;
            
            Block block;
            switch (direction)
            {
                case Direction.Right:
                    block = dBlock;
                    break;
                case Direction.Down:
                    block = sBlock;
                    break;
                case Direction.Left:
                    block = aBlock;
                    break;
                default:
                    block = wBlock;
                    break;
            }
            if (_startCoroutine != null)
                StopCoroutine(_startCoroutine);
            _startCoroutine = StartCoroutine(Execute(block));
        }

        private bool RepeatingInput()
        {
            if (FirstInput)
                return false;
            bool sameInput = LastTriggeredDirection == CurrentTriggeredDirection;
            if (!sameInput)
                differences++;
            return sameInput;
        }

        #endregion
    }
}
