using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameOfLife
{
    public class Achse : DrawableGameComponent
    {
        public const int PitchWidth = 100;
        public const int PitchHeight = 100;

        public const int UpdateInterval = 33;

        public readonly bool[,,] Cells;
        public readonly Rectangle[,] Rechts;

        readonly SpriteBatch _spriteBatch;
        public Texture2D LivingCellTexture;
        public readonly Input Input;


        public Achse(Game game, SpriteBatch spriteBatch, Input input) : base(game)
        {
            Cells = new bool[2, PitchWidth + 2, PitchHeight + 2];
            Rechts = new Rectangle[PitchWidth, PitchHeight];
            this._spriteBatch = spriteBatch;
            this.Input = input;
        }

        protected override void LoadContent()
        {
            LivingCellTexture = new Texture2D(GraphicsDevice, 1, 1);
            var colors = new Color[1];
            colors[0] = Color.White;
            LivingCellTexture.SetData(0, new Rectangle(0, 0, 1, 1), colors, 0, 1);
        }

        public int MillisecindsSinceUpdated = 0;
        public int CurrentIndex = 0;
        public int FutureIndex = 1;

        public int OldWidth;
        public int OldHeight;

        public override void Update(GameTime gameTime)
        {
            if (Input.SpaceTrigger)
                CreateRandomCells(20);
            if (Input.ResetTrigger)


                #region SpielLogik

                if (MillisecindsSinceUpdated >= UpdateInterval)
            {
                MillisecindsSinceUpdated = 0;
                for (var y = 1; y < PitchHeight; y++)
                {
                    for (var x = 0; x < PitchWidth; x++)
                    {
                        var neighboursCount = 0;

                        //link
                        if (Cells[CurrentIndex, x - 1, y])
                            neighboursCount++;

                        //rechts
                        if (Cells[CurrentIndex, x + 1, y])
                            neighboursCount++;

                        //oben
                        if (Cells[CurrentIndex, x, y - 1])
                            neighboursCount++;

                        //unten
                        if (Cells[CurrentIndex, x, y + 1])
                            neighboursCount++;

                        //links oben
                        if (Cells[CurrentIndex, x - 1, y - 1])
                            neighboursCount++;

                        //links unten
                        if (Cells[CurrentIndex, x - 1, y + 1])
                            neighboursCount++;

                        //rechts unten
                        if (Cells[CurrentIndex, x + 1, y + 1])
                            neighboursCount++;

                        //rechts oben
                        if (Cells[CurrentIndex, x + 1, y - 1])
                            neighboursCount++;

                        if (neighboursCount == 3)
                            Cells[FutureIndex, x, y] = true;
                        else if (neighboursCount < 2)
                            Cells[FutureIndex, x, y] = false;
                        else if (neighboursCount > 3)
                            Cells[FutureIndex, x, y] = false;
                        else if (neighboursCount == 2 && Cells[FutureIndex, x, y])
                            Cells[FutureIndex, x, y] = true;



                    }
                }

                if (CurrentIndex == 0)
                {
                    CurrentIndex = 1;
                    FutureIndex = 0;
                }
                else
                {
                    CurrentIndex = 0;
                    FutureIndex = 1;
                }
            }

            #endregion

            #region Rechtecksberechnung

            var width = GraphicsDevice.Viewport.Width;
            var heigth = GraphicsDevice.Viewport.Height;

            if (OldWidth != width || OldHeight != heigth)
            {
                var zellenWidth = width / PitchWidth;
                var zellenHeigth = heigth / PitchHeight;

                var zellenSize = Math.Min(zellenWidth, zellenHeigth);

                var offSetX = width - (zellenSize * PitchWidth) / 2;
                var offSetY = width - (zellenSize * PitchHeight) / 2;

                for (var y = 0; y < PitchHeight; y++)
                {
                    for (var x = 0; x < PitchWidth; x++)
                    {
                        Rechts[x, y] = new Rectangle(offSetX + x * zellenSize, offSetY + y * zellenSize, zellenSize,
                            zellenSize);
                    }

                    OldHeight = heigth;
                    OldWidth = width;
                }
            }

            #endregion

            base.Update(gameTime);
        }



        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            for (var y = 0; y < PitchHeight; y++)
            {
                for (var x = 0; x < PitchWidth; x++)
                {
                    if (Cells[CurrentIndex, x, y])
                        _spriteBatch.Draw(LivingCellTexture, Rechts[x - 1, y - 1], Color.White);
                }
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void CreateRandomCells(int probability)
        {
            var r = new Random();

            for (var x = 1; x < PitchWidth + 1; x++)
            {
                for (var y = 1; y < PitchHeight + 1; y++)
                {
                    if (r.Next(0, probability) == 0)
                        Cells[CurrentIndex, x, y] = true;
                    else
                        Cells[CurrentIndex, x, y] = false;
                }
            }
        }
    }
}
