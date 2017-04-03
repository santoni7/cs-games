using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceShooter.GameSystem
{
    /// <summary>
    ///     Comparer for comparing two keys, handling equality as beeing greater
    ///     Use this Comparer e.g. with SortedLists or SortedDictionaries, that don't allow duplicate keys
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class DuplicateKeyComparer<TKey> :IComparer<TKey> where TKey : IComparable
    {
        #region IComparer<TKey> Members

        public int Compare(TKey x, TKey y)
        {
            var result = x.CompareTo(y);

            if (result == 0)
                return 1; // Handle equality as beeing greater
            return result;
        }

        #endregion
    }

    public static class Extensions
    {
        public static Rectangle CreateRectangle(this Vector2 Position, Vector2 Size)
        {
            return new Rectangle((int) Position.X, (int) Position.Y, (int) Size.X, (int) Size.Y);
        }
    }

}