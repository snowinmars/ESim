using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESim.Config;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SandS.Algorithm.CommonNamespace;
using SandS.Algorithm.Library.PositionNamespace;

namespace ESim.Entities
{
    public class Creature
    {
        public Creature(Position position)
        {
            this.Position = position;
            this.Dna = new Dna();
        }

        public Position Position { get; set; }

        public Dna Dna { get; set; }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var texturePosition = this.Position * Configuration.CreatureTextureSize;

            spriteBatch.Draw(Configuration.DefaultCreatureTexture, texturePosition.ToVector2(), Color.White);
        }

        public bool WillHaveChild()
        {
            return CommonValues.Random.Next(0, 4) == 0;
        }

        public void Mutate()
        {
            if (!this.WillOrganismMutate())
            {
                return;
            }

            bool[] nucleotides = this.Dna.Values;

            for (int i = 0; i < nucleotides.Length; i++)
            {
                if (this.WillNucleotideMutate())
                {
                    nucleotides[i] = !nucleotides[i];
                }
            }
        }

        private bool WillNucleotideMutate()
        {
            return true;
        }

        private bool WillOrganismMutate()
        {
            return CommonValues.Random.Next(0, 16) == 0;
        }
    }
}
