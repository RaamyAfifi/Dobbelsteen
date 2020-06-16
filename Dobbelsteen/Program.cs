using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

class RNGCSP
{
    private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
    // Main method.
    public static void Main()
    {
        // je mag zo vaak mag rollen: 1x per persoon
        const int totalRolls = 1;
        // de kant van de dobbelsteen, het heeft 6 kanten dus daarom 6
        int[] results = new int[6];

        // rol de dobbelsteen 1 x
        // the results to the console.
        // je rolt de dobbelsteen
        for (int x = 0; x < totalRolls; x++)
        {
            // er wordt een nieuwe byte aangemaakt en die heet 'roll'
            // functie RollDice aanroepen(deze staat onderaan)
            byte roll = RollDice((byte)results.Length);
            results[roll - 1]++;
            // result (bevat de groote van de int en daar 1 vanaf halen
        }
        // dit is om alle dobbel resultaten op te halen
        for (int i = 0; i < results.Length; ++i)
        {
            // indeze van result delen door het aantal rollen moet gelijk aan 1 zijn
            if (results[i] / totalRolls == 1)
            {
                // {0}Laat het getal 1x een getaal (1 -6) zien
                // {1}Laat het getal 1 zien, dit laat zien welk getal er gerolld is
                Console.WriteLine("{0}", i + 1, results[i], (double)results[i] / (double)totalRolls);
            }
            
        }
        // Hier ontdoe je je van de numbergenerator
        rngCsp.Dispose();
        // Geeft de gebruiker input
        Console.ReadLine();
    }

    // This method simulates a roll of the dice. The input parameter is the
    // number of sides of the dice.

    
    public static byte RollDice(byte numberSides)
    {
        if (numberSides <= 0)
            throw new ArgumentOutOfRangeException("numberSides");

        // Create a byte array to hold the random value.
        byte[] randomNumber = new byte[1];
        do
        {
            // Fill the array with a random value.
            rngCsp.GetBytes(randomNumber);
        }
        while (!IsFairRoll(randomNumber[0], numberSides));
        // Return the random number mod the number
        // of sides.  The possible values are zero-
        // based, so we add one.
        return (byte)((randomNumber[0] % numberSides) + 1);
    }

    private static bool IsFairRoll(byte roll, byte numSides)
    {
        // There are MaxValue / numSides full sets of numbers that can come up
        // in a single byte.  For instance, if we have a 6 sided die, there are
        // 42 full sets of 1-6 that come up.  The 43rd set is incomplete.
        int fullSetsOfValues = Byte.MaxValue / numSides;

        // If the roll is within this range of fair values, then we let it continue.
        // In the 6 sided die case, a roll between 0 and 251 is allowed.  (We use
        // < rather than <= since the = portion allows through an extra 0 value).
        // 252 through 255 would provide an extra 0, 1, 2, 3 so they are not fair
        // to use.
        return roll < numSides * fullSetsOfValues;
    }
}
