using Microsoft.Xna.Framework;

namespace Mobius {
    public class Camera {
        // IN PROGRESS
        public Matrix viewMatrix;
        public Vector2 position;
        private Vector2 halfViewSize;
        private Vector2 fullViewSize;
        private int width;
        private int height;

        private Matrix prevView;
        private Vector2 cameraAdjust;
        private Vector2 prevPos;
        private Character target;

        public int scrollValue;

        public Camera(int screenWidth, int screenHeight, int worldWidth, int worldHeight) //ref Object theTarget)
        {
            fullViewSize = new Vector2((float)screenWidth, (float)screenHeight);
            halfViewSize = new Vector2(screenWidth * 0.5f, screenHeight * 0.5f);
            viewMatrix = Matrix.CreateTranslation(0.0f, 0.0f, 0.0f);
            width = worldWidth;
            height = worldHeight;

            cameraAdjust = new Vector2(0, 0);
            prevView = viewMatrix;
            prevPos = Vector2.Zero; //temp
            scrollValue = 2;
            position = new Vector2(screenWidth/2, screenHeight/2);
        }

        private void UpdateViewMatrix() {
            viewMatrix = Matrix.CreateTranslation(-position.X + halfViewSize.X, -position.Y + halfViewSize.Y, 0.0f);
        }

        public void Update() {
            position.X += scrollValue;
            UpdateViewMatrix();
            prevView = viewMatrix;
        }


        public Vector2 topLeft() {
            return (position - halfViewSize);
        }

        public Vector2 topRight() {
            return (position + halfViewSize);
        }

        public void moveCamera(Vector2 Movement) {
            position += Movement;
        }
    }
}
