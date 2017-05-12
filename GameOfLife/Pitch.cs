using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameOfLife
{
    class Pitch : DrawableGameComponent
    {
        const int PitchWidth = 100;
        const int PitchHeight = 100;

        const int UpdateInterval = 33;

        bool[,,] _cells;
        Rectangle[,] _rechts;

        SpriteBatch _spriteBatch;
        Texture2D _livingCellTexture;


        public Pitch(Game game, SpriteBatch spriteBatch) : base(game)
        {
            _cells = new bool[2, PitchWidth + 2, PitchHeight + 2];
            _rechts = new Rectangle[PitchWidth, PitchHeight];
            this._spriteBatch = spriteBatch;
        }

        protected override void LoadContent()
        {
            _livingCellTexture = new Texture2D(GraphicsDevice, 1, 1);
            var colors = new Color[1];
            colors[0] = Color.White;
            _livingCellTexture.SetData(0, new Rectangle(0 , 0, 1, 1), colors, 0, 1);
        }

        int _millisecindsSinceUpdated = 0;
        int _currentIndex = 0, _futureIndex = 1;

        int _oldWidth, _oldHeight; 

        public override void Update(GameTime gameTime)
        {
            #region SpielLogik
            if (_millisecindsSinceUpdated >= UpdateInterval)
            {
                _millisecindsSinceUpdated = 0;
                for (var y = 1; y < PitchHeight; y++)
                {
                    for (var x = 0; x < PitchWidth; x++)
                    {
                        var neighboursCount = 0;

                        //link
                        if (_cells[_currentIndex, x - 1, y])
                          neighboursCount++;

                        //rechts
                        if (_cells[_currentIndex, x + 1, y])
                            neighboursCount++;

                        //oben
                        if (_cells[_currentIndex, x, y - 1])
                            neighboursCount++;

                        //unten
                        if (_cells[_currentIndex, x, y + 1])
                            neighboursCount++;

                        //links oben
                        if (_cells[_currentIndex, x - 1, y - 1])
                            neighboursCount++;

                        //links unten
                        if (_cells[_currentIndex, x - 1, y + 1])
                            neighboursCount++;

                        //rechts unten
                        if (_cells[_currentIndex, x + 1, y + 1])
                            neighboursCount++;

                        //rechts oben
                        if (_cells[_currentIndex, x + 1, y - 1])
                            neighboursCount++;

                        if (neighboursCount == 3)
                            _cells[_futureIndex, x, y] = true;
                        else if(neighboursCount < 2)
                            _cells[_futureIndex, x, y] = false;
                        else if (neighboursCount > 3)
                            _cells[_futureIndex, x, y] = false;
                        else if (neighboursCount == 2 && _cells[_futureIndex, x, y])
                            _cells[_futureIndex, x, y] = true;



                    }
                }

                if (_currentIndex == 0)
                {
                    _currentIndex = 1;
                    _futureIndex = 0;
                }
                else
                {
                    _currentIndex = 0;
                    _futureIndex = 1;
                }
            }
            #endregion

            #region Rechtecksberechnung

            var width = GraphicsDevice.Viewport.Width;
            var heigth = GraphicsDevice.Viewport.Height;

            if (_oldWidth != width || _oldHeight != heigth)
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
                        _rechts[x, y] = new Rectangle(offSetX + x * zellenSize, offSetY + y * zellenSize, zellenSize,
                            zellenSize);
                    }

                    _oldHeight = heigth;
                    _oldWidth = width;
                }
            }

            #endregion

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
