/**
 * Jonathan Wong
 * CS 780 - Cryptography: Custom-Designed LFSR Random Bit Generator
 **/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSR
{
    public class Generator
    {
        int[] tap_0, tap_1, tap_2;
        Queue<int> lfsr_0, lfsr_1, lfsr_2;

        /**
        * Constructor takes in three integer arrays representing initial fills and three integer
        * arrays indicating tap positions for each register.
        **/
        public Generator(int[] fill_0, int[] fill_1, int[] fill_2, int[] tapPositions_0, int[] tapPositions_1, int[] tapPositions_2)
        {
            lfsr_0 = new Queue<int>();
            lfsr_1 = new Queue<int>();
            lfsr_2 = new Queue<int>();
            tap_0 = tapPositions_0;
            tap_1 = tapPositions_1;
            tap_2 = tapPositions_2;

            for (int i = 0; i < fill_0.Length; i++)
                lfsr_0.Enqueue(fill_0[i]);
            for (int i = 0; i < fill_1.Length; i++)
                lfsr_1.Enqueue(fill_1[i]);
            for (int i = 0; i < fill_2.Length; i++)
                lfsr_2.Enqueue(fill_2[i]);

            for (int i = 0; i < tap_0.Length; i++)
                tap_0[i] = fill_0.Length - tap_0[i];  // adjust tap position to access array indices
            for (int i = 0; i < tap_1.Length; i++)
                tap_1[i] = fill_1.Length - tap_1[i];  // adjust tap position to access array indices
            for (int i = 0; i < tap_2.Length; i++)
                tap_2[i] = fill_2.Length - tap_2[i];  // adjust tap position to access array indices
        } // constructor

        /**
        * Generates a binary output of 0 or 1. Implements the algorithm used
        * in custom-designed random bit generator.
        **/
        public int generate()
        {
            int output;
            int decider = clock(lfsr_0, tap_0);
            int first = clock(lfsr_1, tap_1);
            int second = clock(lfsr_2, tap_2);

            if (first == second) {
                output = decider;
                return output;
            }

            if (decider == 1)
            {
                output = first;
                clock(lfsr_2, tap_2);
            }
            else
            {
                output = second;
                clock(lfsr_1, tap_1);
            }
            return output;
        } // generate

        /**
        * Clocks a register once, using tapSpots to indicate tapping
        * positions.
        **/
        public int clock(Queue<int> q, int[] tapSpots)
        {
            int output, sum = 0;
            int[] temp = q.ToArray();
            for (int i = 0; i < tapSpots.Length; i++)
                sum += temp[tapSpots[i]];
            sum %= 2;
            output = q.Dequeue();
            q.Enqueue(sum);
            return output;
        } // clock

        /**
        * A method written for testing purposes. Tests the registers by
        * clocking them each once, and writing their configurations and
        * outputs to the console.
        **/
        public void test() {

            Console.Write("LFSR 0: ");
            int output, sum = 0;
            int[] temp = lfsr_0.ToArray();
            for (int i = 0; i < temp.Length; i++)
                Console.Write(temp[i]);
            for (int i = 0; i < tap_0.Length; i++)
                sum += temp[tap_0[i]];
            sum %= 2;
            output = lfsr_0.Dequeue();
            lfsr_0.Enqueue(sum);
            Console.WriteLine(", Output =  " + output);

            Console.Write("LFSR 1: ");
            sum = 0;
            temp = lfsr_1.ToArray();
            for (int i = 0; i < temp.Length; i++)
                Console.Write(temp[i]);
            for (int i = 0; i < tap_1.Length; i++)
                sum += temp[tap_1[i]];
            sum %= 2;
            output = lfsr_1.Dequeue();
            lfsr_1.Enqueue(sum);
            Console.WriteLine(", Output =  " + output);

            Console.Write("LFSR 2: ");
            sum = 0;
            temp = lfsr_2.ToArray();
            for (int i = 0; i < temp.Length; i++)
                Console.Write(temp[i]);
            for (int i = 0; i < tap_2.Length; i++)
                sum += temp[tap_2[i]];
            sum %= 2;
            output = lfsr_2.Dequeue();
            lfsr_2.Enqueue(sum);
            Console.WriteLine(", Output =  " + output);
        } // test
    } // class Generator

    class Program
    {
        static void Main(string[] args)
        {        //tap positions <-- { 20,19,18,17,16,15,14,13,12,11,10, 9, 8, 7, 6, 5, 4, 3, 2, 1 }
            int[] fill_0 = new int[] { 0, 0, 1, 1, 1, 1, 0, 1, 0, 1, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0 };
            int[] fill_1 = new int[] { 1, 1, 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 1, 1, 1, 0, 1, 1, 0, 1 };
            int[] fill_2 = new int[] { 1, 1, 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 1, 1, 1, 0, 1, 1, 0, 1 };

            // configure your tap positions for each register here
            int[] tapPositions_0 = new int[] { 20, 1 };
            int[] tapPositions_1 = new int[] { 20, 19 };
            int[] tapPositions_2 = new int[] { 20, 1 };

            Generator g = new Generator(fill_0, fill_1, fill_2, tapPositions_0, tapPositions_1, tapPositions_2);

            using (System.IO.StreamWriter file = new System.IO.StreamWriter("out.txt"))
            {
                int n;
                for (int i = 0; i < 1000000; i++)
                {
                    n = g.generate();
                    file.Write(n);
                }
                Console.WriteLine("\nNumbers released to out.txt.");
                file.Close();
            }
        } // Main
    } // class Program
} // LFSR