using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bombarder
{
    internal class Object
    {
        public string Type { get; set; }

        public bool HasDuration { get; set; }
        public int Duration { get; set; }

        public int TextureTag { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
    }
}
