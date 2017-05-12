using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameOfLife
{
    public class Achse : DrawableGameComponent
    {
        public const int AchseWidth = 400;
        public const int AchseHeight = 400;

        public const int UpdateInterval = 1000;

        public bool[,,] Cells;
        public Rectangle[,] Rechts;

        public SpriteBatch SpriteBatch;
        public Texture2D LivingCellTexture;
        public Input Input;


        public Achse(Game game, SpriteBatch spriteBatch, Input input) : base(game)
        {
            Cells = new bool[2, AchseWidth + 2, AchseHeight + 2];
            Rechts = new Rectangle[AchseWidth, AchseHeight];
            this.SpriteBatch = spriteBatch;
            this.Input = input;
        }

        protected override void LoadContent()
        {
            LivingCellTexture = new Texture2D(GraphicsDevice, 1, 1);
            var colors = new Color[1];
            colors[0] = Color.White;

            LivingCellTexture.SetData(0, new Rectangle(0, 0, 1, 1), colors, 0, 1);

            base.LoadContent();
        }

        public int MillisecondsSinceUpdated;
        public int CurrentIndex;
        public int FutureIndex = 1;

        public int OldWidth;
        public int OldHeight;

        public override void Update(GameTime gameTime)
        {
            if (Input.SpaceTrigger)
            {
                CreateRandomCells(20);
            }
            if (Input.ResetTrigger)
            {
                ResetCells();
            }

          MillisecondsSinceUpdated += gameTime.ElapsedGameTime.Milliseconds;

             if (MillisecondsSinceUpdated >= UpdateInterval)
             {
                MillisecondsSinceUpdated = 0;
                for (var y = 1; y < AchseHeight + 1; y++)
                {
                    for (var x = 1; x < AchseWidth + 1; x++)
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
                        {
                            Cells[FutureIndex, x, y] = true;
                        }
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

            var width = GraphicsDevice.Viewport.Width;
            var heigth = GraphicsDevice.Viewport.Height;

            if (OldWidth != width || OldHeight != heigth)
            {
                var zellenWidth = width / AchseWidth;
                var zellenHeigth = heigth / AchseHeight;

                var zellenSize = Math.Min(zellenWidth, zellenHeigth);

                var offSetX = (width - (zellenSize * AchseWidth)) / 2;
                var offSetY = (width - (zellenSize * AchseHeight)) / 2;

                for (var y = 0; y < AchseHeight; y++)
                {
                    for (var x = 0; x < AchseWidth; x++)
                    {
                        Rechts[x, y] = new Rectangle(offSetX + x * zellenSize, offSetY + y * zellenSize, zellenSize,
                            zellenSize);
                    }
                }
                OldHeight = heigth;
                OldWidth = width;
            }


            base.Update(gameTime);

        }



        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            for (var y = 1; y < AchseHeight + 1; y++)
            {
                for (var x = 1; x < AchseWidth + 1; x++)
                {
                    if (Cells[CurrentIndex, x, y])
                        SpriteBatch.Draw(LivingCellTexture, Rechts[x - 1, y - 1], Color.White);
                }
            }

            SpriteBatch.End();

            base.Draw(gameTime);
        }

        private void CreateRandomCells(int probability)
        {
            var r = new Random();

            for (var x = 1; x < AchseWidth + 1; x++)
            {
                for (var y = 1; y < AchseHeight + 1; y++)
                {
                    if (r.Next(0, probability) == 0)
                        Cells[CurrentIndex, x, y] = true;
                    else
                        Cells[CurrentIndex, x, y] = false;
                }
            }
        }

        private void ResetCells()
        {
            for (var y = 1; y < AchseHeight + 1; y++)
            {
                for (var x = 1; x < AchseWidth + 1; x++)
                {
                    Cells[CurrentIndex, x, y] = false;
                }
            }
        }
    }
}
