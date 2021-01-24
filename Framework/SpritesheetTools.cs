using System.Collections.Generic;
using System.Text;

namespace CG_Projekt.Framework
{
    internal static class SpriteSheetTools
    {
        /// <summary>
        /// Calculates the texture coordinates for a sprite inside a sprite sheet.
        /// </summary>
        /// <param name="spriteId">The sprite number. Starts with 0 in the upper left corner and increase in western reading direction up to #sprites - 1.</param>
        /// <param name="columns">Number of sprites per row.</param>
        /// <param name="rows">Number of sprites per column.</param>
        /// <returns>Texture coordinates for a single sprite</returns>
        internal static Rect CalcTexCoords(uint spriteId, uint columns, uint rows)
        {
            //TODO: Calculate texture coordinates for an animation frame (look at the method summary for details!)

            uint row = spriteId / columns;
            uint col = spriteId % columns;

            float x = col / (float)columns;
            float y = 1f - (row + 1f) / rows;

            float width = 1f / columns;
            float heigth = 1f / rows;

            return new Rect(x, y, width, heigth);
        }

        internal static IEnumerable<uint> StringToSpriteIds(string text, uint firstCharacter)
        {
            byte[] asciiBytes = Encoding.ASCII.GetBytes(text);
            foreach (var asciiCharacter in asciiBytes)
            {
                yield return asciiCharacter - firstCharacter;
            }
        }
    }
}