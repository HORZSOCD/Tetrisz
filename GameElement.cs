using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Féléves_beadandó_Tetrisz_PBVGD1
{
    // a játéktérben csak fal, üres tér vagy a figura egy darabja található (filled)
    public enum ElementType
    {
        wall, empty, filled
    }
    class GameElement
    {
        
        public ElementType EType { get; set; }
        public bool IsDown { get; set; }

        // Alapértelmezett kitöltési tipus
        public GameElement()
        {
            this.EType = ElementType.empty;
        }


        public GameElement(ElementType elemetnType)
        {
            this.EType = elemetnType;
        }


        public GameElement(ElementType elementType, bool isDown)
        {
            this.EType = elementType;
            this.IsDown = isDown;
        }
    }
}
