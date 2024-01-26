using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Bombarder
{
    internal class Textures
    {
        public Texture2D White { get; set; }
        public Texture2D WhiteCircle { get; set; }
        public Texture2D HalfWhiteCirlce { get; set; }
        public Texture2D HollowCircle { get; set; }
        public Texture2D Cursor { get; set; }
        public (Texture2D, Texture2D) DemonEye { get; set; }

        public Texture2D HitMarker { get; set; }
    }
}
