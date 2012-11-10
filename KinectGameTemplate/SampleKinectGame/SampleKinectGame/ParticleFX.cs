using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using KinectExplorer;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using ProjectMercury.Renderers;
using ProjectMercury.Emitters;
using ProjectMercury.Modifiers;
using ProjectMercury;

namespace SampleKinectGame
{
    public class ParticleFX
    {
        private SpriteBatchRenderer myRenderer;
        private ParticleEffect myEffect;
        private string name;

        //private float originalScale;

        //private float scale;
        //public float Scale
        //{
        //    get { return scale; }
        //    set { scale = value; myEffect[0].ReleaseScale = originalScale * scale; myEffect.Initialise(); }
        //}

        public bool Visible { get; set; }

        public Vector2 Position { get; set; }

        public ParticleFX(GraphicsDeviceManager graphics, String name)
        {
            myRenderer = new SpriteBatchRenderer
            {
                GraphicsDeviceService = graphics
            };
            this.name = name;
            Visible = true;
        }

        public void LoadContent(ContentLoader content)
        {
            myEffect = content.ContentManager.Load<ParticleEffect>(name);

            myEffect.LoadContent(content.ContentManager);
            myEffect.Initialise();
            myRenderer.LoadContent(content.ContentManager);
        }

        public void Trigger()
        {
            if (Visible)
                myEffect.Trigger(Position);
        }

        public void Update(GameTime gameTime)
        {
            // "Deltatime" ie, time since last update call
            float SecondsPassed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            myEffect.Update(SecondsPassed);
        }

        public void Draw()
        {
            myRenderer.RenderEffect(myEffect);
        }
    }
}
