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
        //    public static string ConvertToString(this CardValue v)
        //    {
        //        switch (v)
        //        {
        //            case CardValue.Seven:
        //                return "7";
        //            case CardValue.Eight:
        //                return "8";
        //            case CardValue.Nine:
        //                return "9";
        //            case CardValue.Ten:
        //                return "10";
        //            case CardValue.J:
        //                return "jack";
        //            case CardValue.Q:
        //                return "queen";
        //            case CardValue.K:
        //                return "king";
        //            case CardValue.A:
        //                return "ace";
        //            default:
        //                return "";

        //        }
        //    }

        //    public static CardSuit ToCardSuit(this string s)
        //    {
        //        switch (s)
        //        {
        //            case "diamonds":
        //                return CardSuit.Diamonds;
        //            case "clubs":
        //                return CardSuit.Clubs;
        //            case "hearts":
        //                return CardSuit.Hearts;
        //            case "spades":
        //                return CardSuit.Spades;
        //            default:
        //                throw new ArgumentException("Could not handle CardSuit from value: " + s);
        //        }
        //    }
        //    public static CardValue ToCardValue(this string v)
        //    {
        //        switch (v)
        //        {
        //            case "7":
        //                return CardValue.Seven;
        //            case "8":
        //                return CardValue.Eight;
        //            case "9":
        //                return CardValue.Eight;
        //            case "10":
        //                return CardValue.Ten;
        //            case "jack":
        //                return CardValue.J;
        //            case "queen":
        //                return CardValue.Q;
        //            case "king":
        //                return CardValue.K;
        //            case "ace":
        //                return CardValue.A;
        //            default:
        //                throw new ArgumentException("Wrong string value: " + v);

        //        }
        //    }
    }

}