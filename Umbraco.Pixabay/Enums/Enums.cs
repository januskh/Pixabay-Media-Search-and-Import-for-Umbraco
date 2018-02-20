using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbraco.Pixabay
{
    public class Enums
    {
        public enum EnumLanguage
        {
            cs, da, de, en, es, fr, id, it, hu, nl, no, pl, pt, ro, sk, fi, sv, tr, vi, th, bg, ru, el, ja, ko, zh
        }

        public enum EnumResponseGroup
        {
            Image_Details,
            High_Resolution
        }

        public enum EnumImageType
        {
            All,
            Photo,
            Illustration,
            Vector
        }

        public enum EnumOrientation
        {
            All,
            Horizontal,
            Vertical
        }

        public enum EnumCategory
        {
            All,
            Fashion,
            Nature,
            Backgrounds,
            Science,
            Education,
            People,
            Feelings,
            Religion,
            Health,
            Places,
            Animals,
            Industry,
            Food,
            Computer,
            Sports,
            Transportation,
            Travel,
            Buildings,
            Business,
            Music
        }

        public enum EnumVideoType
        {
            All,
            Film,
            Animation
        }

    }
}
