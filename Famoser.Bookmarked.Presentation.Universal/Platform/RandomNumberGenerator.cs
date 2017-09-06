using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using PCLCrypto;
using Windows.Security.Cryptography;
using Famoser.Bookmarked.Business.Services.Interfaces;

namespace Famoser.Bookmarked.Presentation.Universal.Platform
{
    public class RandomNumberService : IRandomNumberService
    {
        /// <summary>
        /// generates a random number between min & max
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public uint GenerateRandomNumber(uint min, uint max)
        {
            if (min > max)
            {
                throw new ArgumentException("min must be strictly smaller than max");
            }
            var range = max - min + 1;
            Contract.Requires(min <= max);
            uint myVal = 0;
            do
            {
                myVal = CryptographicBuffer.GenerateRandomNumber();
            } while (!IsFair(myVal, range));
            myVal = myVal % range;
            Contract.Ensures(myVal >= min);
            Contract.Ensures(myVal <= max);
            return myVal;
        }

        private bool IsFair(uint res, uint range)
        {
            // following code / reasoning is from msdn, adapted for uint

            // There are MaxValue / range full sets of numbers that can come up
            // in a single uint.  For instance, if we have a 6 sided die, there are
            // 42 full sets of 1-6 that come up.  The 43rd set is incomplete.
            uint fullSetsOfValues = uint.MaxValue / range;

            // If the roll is within this range of fair values, then we let it continue.
            // In the 6 sided die case, a roll between 0 and 251 is allowed.  (We use
            // < rather than <= since the = portion allows through an extra 0 value).
            // 252 through 255 would provide an extra 0, 1, 2, 3 so they are not fair
            // to use.
            return res < range * fullSetsOfValues;
        }
    }
}
