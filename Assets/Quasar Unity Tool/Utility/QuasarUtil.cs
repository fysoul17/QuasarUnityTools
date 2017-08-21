using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quasar.Util
{
    public static class QuasarRandom
    {
        public static int SelectRandomIndexWithProbability(float[] probabilities)
        {
            float total = 0;

            for (int index = 0; index < probabilities.Length; index++)
            {
                total += probabilities[index];
            }

            float randomPoint = Random.value * total;

            for (int index = 0; index < probabilities.Length; index++)
            {
                if (randomPoint < probabilities[index]) return index;
                else randomPoint -= probabilities[index];
            }

            return probabilities.Length - 1;    // Cuz of return value of 1 from Random.value.
        }
    }

    public static class BigNumberFormatter
    {
        public static string ToFormattedString(this double aNumber, string formatter = "n2")
        {
            if (double.IsInfinity(aNumber)) return "Infitity";

            string formattedNumber = string.Empty;
            int formatIndex = 0;

            while (aNumber >= 1000)
            {
                formatIndex++;

                aNumber *= 0.001f;
            }

            if (formatIndex == 0)
            {
                formattedNumber = aNumber.ToString("n0");
            }
            else
            {
                formattedNumber = aNumber.ToString(formatter) + (BigNumberFormat)formatIndex;
            }

            return formattedNumber;
        }

        public enum BigNumberFormat
        {
            A = 1,
            B,
            C,
            D,
            E,
            F,
            G,
            H,
            I,
            J,
            K,
            L,
            M,
            N,
            O,
            P,
            Q,
            R,
            S,
            T,
            U,
            V,
            W,
            X,
            Y,
            Z,

            COUNT

        }
    }
}