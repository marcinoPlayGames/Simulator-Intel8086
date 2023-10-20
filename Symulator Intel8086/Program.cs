using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks.Dataflow;
using System.Xml;

namespace Ex_03_01
{
    class Program
    {
        ////////////////////////////
        //
        //         Console Program
        //
        ////////////////////////////

        static string EX;
        static string[,] rejestry = new string[2, 4];
        static string[,] podrejestry = new string[2, 8];
        static string[,] pamiec = new string[2, 65536];
        static string[,] pointery = new string[2, 4];
        public static void Main(string[] args)
        {

            for (int s = 0; s < 65536; s++)
            {
                pamiec[0, s] = "00";
               

                if (s < 16)
                {
                    pamiec[1, s] = "000" + s.ToString("X");
                }

                if (s > 15 && s < 256)
                {
                    pamiec[1, s] = "00" + s.ToString("X");
                }

                if (s > 255 && s < 4096)
                {
                    pamiec[1, s] = "0" + s.ToString("X");
                }

                if (s > 4095)
                {
                    pamiec[1, s] = s.ToString("X");
                }

                
            }

            rejestry[0, 0] = "AX";
            rejestry[0, 1] = "BX";
            rejestry[0, 2] = "CX";
            rejestry[0, 3] = "DX";
            for (int j = 0; j < 4; j++)
            {
                rejestry[1, j] = "0000";
            }

            podrejestry[0, 0] = "AH";
            podrejestry[0, 1] = "AL";
            podrejestry[0, 2] = "BH";
            podrejestry[0, 3] = "BL";
            podrejestry[0, 4] = "CH";
            podrejestry[0, 5] = "CL";
            podrejestry[0, 6] = "DH";
            podrejestry[0, 7] = "DL";
            for (int j = 0; j < 8; j++)
            {
                podrejestry[1, j] = "00";
            }

            pointery[0, 0] = "BP";
            pointery[0, 1] = "DI";
            pointery[0, 2] = "SI";
            pointery[0, 3] = "SP";
            for (int j = 0; j < 4; j++)
            {
                pointery[1, j] = "0000";
            }

            char przycisk = '0';
            while (przycisk != '1')
            {
                Console.Clear();
                Console.WriteLine("Registers   Pointers");
                Console.WriteLine("   H  L");
                Console.WriteLine("A " + podrejestry[1, 0] + " " + podrejestry[1, 1] + "    " + " BP " + pointery[1, 0]);
                Console.WriteLine("B " + podrejestry[1, 2] + " " + podrejestry[1, 3] + "    " + " DI " + pointery[1, 1]);
                Console.WriteLine("C " + podrejestry[1, 4] + " " + podrejestry[1, 5] + "    " + " SI " + pointery[1, 2]);
                Console.WriteLine("D " + podrejestry[1, 6] + " " + podrejestry[1, 7] + "    " + " SP " + pointery[1, 3] + '\n');

                for (int i = 0; i < 65536; i++)
                {
                    int ap = 1;
                    if (pamiec[0, i] != "00")
                    {
                        ap = 0;
                    }
                    if (ap == 0)
                    {
                        Console.WriteLine("Memory");
                        break;
                    }
                }
                for (int i = 0; i < 65536; i++)
                {
                    if (pamiec[0, i] != "00")
                    {
                        Console.WriteLine(pamiec[0, i] + " " + pamiec[1, i]);
                    }
                }

                Console.WriteLine('\n' + "Wpisz komende do wykonania: ");

                string komenda = Console.ReadLine();

                komenda = komenda.ToUpper();

                string[] komenda2 = komenda.Split(',', ' ');

                int st = 2;
                string[] komenda3 = { "2" };
                string[] komenda4 = { "2" };
                for (int b = 0; b < komenda2.Length; b++)
                {
                    if (komenda2[b].Contains(' '))
                    {
                        st = b;
                    }
                }

                if (komenda2[0] == "MOV")
                {
                    string mov = MOV(komenda2[1], komenda2[3]);
                }
                if (komenda2[0] == "XCHG")
                {
                    string mov = XCHG(komenda2[1], komenda2[3]);
                }
                if (komenda2[0] == "PUSH")
                {
                    string mov = PUSH(komenda2[1]);
                }
                if (komenda2[0] == "POP")
                {
                    string mov = POP(komenda2[1]);
                }
            }
           


        }

        ////////////////////////////
        //
        //         MOV Command
        //
        ////////////////////////////
        static string MOV(string c, string d)
        {
            string result = "0";
            int k = 0;

            bool e = false;
            bool f = false;
            bool f2 = false;
            
            for (int i = 0; i < 65536; i++)
            {
                if (d.Contains(pamiec[1, i]))
                {
                    e = true;
                }
                if (c.Contains(pamiec[1, i]) && c.Contains("[") && c.Contains("]"))
                {
                    f = true;
                }
                if (d.Contains(pamiec[1, i]) && d.Contains("[") && d.Contains("]"))
                {
                    f2 = true;
                }
            }


            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (rejestry[0, i].Contains(c) && rejestry[0, j].Contains(d))
                    {
                        k = 1;
                    }
                    if (rejestry[0, i].Contains(c) && pointery[0, j].Contains(d))
                    {
                        k = 2;
                    }
                    if (pointery[0, i].Contains(c) && pointery[0, j].Contains(d))
                    {
                        k = 3;
                    }
                    if (pointery[0, i].Contains(c) && rejestry[0, j].Contains(d))
                    {
                        k = 4;
                    }
                    if (rejestry[0, i].Contains(c) && e == true)
                    {
                        k = 5;
                    }
                    if (pointery[0, i].Contains(c) && e == true)
                    {
                        k = 6;
                    }
                    if (pointery[0, i].Contains(c) && e == true)
                    {
                        k = 6;
                    }
                    if (f == true && rejestry[0, i].Contains(d))
                    {
                        k = 8;
                    }
                    if (f2 == true && rejestry[0, i].Contains(c))
                    {
                        k = 9;
                    }

                    if (k != 0)
                    {
                        break;
                    }
                }
            }
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (podrejestry[0, i].Contains(c) && podrejestry[0, j].Contains(d))
                    {
                        k = 7;
                    }
                    if (f == true && podrejestry[0, j].Contains(d))
                    {
                        k = 10;
                    }
                    if (f2 == true && podrejestry[0, j].Contains(c))
                    {
                        k = 11;
                    }
                    if (k != 0)
                    {
                        break;
                    }
                }
            }

            if (k == 0)
            {
                throw new Exception("This is wrong command!");
            }


            while ((k > 0 && k < 5) || k == 8 || k == 9)
            {
                int mov2 = 0;
                int mov3 = 0;
                string mov4 = "0";
                for (int i = 0; i < 4; i++)
                {
                    if (rejestry[0, i].Contains(c))
                    {
                        mov2 = i;
                    }
                    if (rejestry[0, i].Contains(d))
                    {
                        mov3 = i;
                    }
                    if (pointery[0, i].Contains(c))
                    {
                        mov2 = i;
                    }
                    if (pointery[0, i].Contains(d))
                    {
                        mov3 = i;
                    }
                }
                //mov (register), (register)
                while (k == 1)
                {
                    mov4 = rejestry[1, mov3];
                    rejestry[1, mov2] = mov4;
                    char[] mov5 = rejestry[1, mov2].ToCharArray();
                    podrejestry[1, mov2 * 2] = "" + mov5[0] + mov5[1];
                    podrejestry[1, (mov2 * 2 + 1)] = "" + mov5[2] + mov5[3];
                    k = 0;
                }
                //mov (register), (pointer)
                while (k == 2)
                {
                    mov4 = pointery[1, mov3];
                    rejestry[1, mov2] = mov4;
                    char[] mov5 = rejestry[1, mov2].ToCharArray();
                    podrejestry[1, mov2 * 2] = "" + mov5[0] + mov5[1];
                    podrejestry[1, (mov2 * 2 + 1)] = "" + mov5[2] + mov5[3];
                    k = 0;
                }
                //mov (pointer), (pointer)
                while (k == 3)
                {
                    mov4 = pointery[1, mov3];
                    pointery[1, mov2] = mov4;
                    k = 0;
                }
                //mov (pointer), (register)
                while (k == 4)
                {
                    mov4 = rejestry[1, mov3];
                    pointery[1, mov2] = mov4;
                    k = 0;
                }
                //mov (offset), (register)
                while (k == 8)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (rejestry[0, i].Contains(d))
                        {
                            mov2 = i;
                        }
                    }
                    string[] offset = c.Split(new char[] { '+', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                    int offset2 = 0;
                    for (int i = 0; i < offset.Length; i++)
                    {
                        int err = 0;
                        if (offset[i].Contains("BX"))
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (rejestry[0, j].Contains(offset[i]))
                                {
                                    offset2 = offset2 + Convert.ToInt32(rejestry[1, j], 16);
                                    break;
                                }
                            }
                        }
                        if (offset[i].Contains("BP") || offset[i].Contains("DI") || offset[i].Contains("SI"))
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (pointery[0, j].Contains(offset[i]))
                                {
                                    offset2 = offset2 + Convert.ToInt32(pointery[1, j], 16);
                                    break;
                                }
                            }
                        }
                        if (!offset[i].Contains("BP") && !offset[i].Contains("DI") && !offset[i].Contains("SI") && !offset[i].Contains("BX"))
                        {
                            err++;
                        }
                        for (int j = 0; j < 65536; j++)
                        {
                            if (offset[i].Contains(pamiec[1, j]))
                            {
                                offset2 = offset2 + Convert.ToInt32(pamiec[1, j], 16);
                                break;
                            }
                            if (i == 65535)
                            {
                                if (!offset[i].Contains(pamiec[1, j]))
                                {
                                    err++;
                                }
                            }
                        }
                        if (err >= 2)
                        {
                            throw new Exception("This is not offset!");
                        }
                        err = 0;
                    }
                    string offset3 = "0";
                    char[] offset4 = rejestry[1, mov2].ToCharArray();
                    pamiec[0, offset2] = "" + offset4[2] + offset4[3];
                    pamiec[0, offset2 + 1] = "" + offset4[0] + offset4[1];

                    c = "0";
                    d = "0";
                    k = 0;
                    e = false;
                    f = false;
                }
                //mov (register), (offset)
                while (k == 9)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (rejestry[0, i].Contains(c))
                        {
                            mov2 = i;
                        }
                    }
                    string[] offset = d.Split(new char[] { '+', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                    int offset2 = 0;

                    for (int i = 0; i < offset.Length; i++)
                    {
                        int err = 0;
                        if (offset[i].Contains("BX"))
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (rejestry[0, j].Contains(offset[i]))
                                {
                                    offset2 = offset2 + Convert.ToInt32(rejestry[1, j], 16);
                                    break;
                                }
                            }
                        }
                        if (offset[i].Contains("BP") || offset[i].Contains("DI") || offset[i].Contains("SI"))
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (pointery[0, j].Contains(offset[i]))
                                {
                                    offset2 = offset2 + Convert.ToInt32(pointery[1, j], 16);
                                    break;
                                }
                            }
                        }
                        if (!offset[i].Contains("BP") && !offset[i].Contains("DI") && !offset[i].Contains("SI") && !offset[i].Contains("BX"))
                        {
                            err++;
                        }
                        for (int j = 0; j < 65536; j++)
                        {
                            if (offset[i].Contains(pamiec[1, j]))
                            {
                                offset2 = offset2 + Convert.ToInt32(pamiec[1, j], 16);
                                break;
                            }
                            if (j == 65535)
                            {
                                while (!offset[i].Contains(pamiec[1, j]))
                                {
                                    err++;
                                    break;
                                }
                            }
                        }
                        if (err >= 2)
                        {
                            throw new Exception("This is not offset!");
                        }
                        err = 0;
                    }
                    
                    rejestry[1, mov2] = pamiec[0, offset2 + 1] + pamiec[0, offset2];
                    podrejestry[1, mov2 * 2] = pamiec[0, offset2 + 1];
                    podrejestry[1, mov2 * 2 + 1] = pamiec[0, offset2];

                    c = "0";
                    d = "0";
                    k = 0;
                    e = false;
                    f = false;
                    f2 = false;
                }
                c = "0";
                d = "0";

            }

            
            // mov (register), (hex)
            // mov (pointer), (hex)
            while (k > 4 && k < 7)
            {
                if (d.Length > 4)
                {
                    break;
                }
                int mov2 = 0;
                string mov4 = "0";
                for (int i = 0; i < 4; i++)
                {
                    if (rejestry[0, i].Contains(c))
                    {
                        mov2 = i;
                    }
                    if (pointery[0, i].Contains(c))
                    {
                        mov2 = i;
                    }
                }
                while (k == 5)
                {
                    mov4 = d;
                    rejestry[1, mov2] = mov4;
                    char[] mov5 = rejestry[1, mov2].ToCharArray();
                    podrejestry[1, mov2 * 2] = "" + mov5[0] + mov5[1];
                    podrejestry[1, (mov2 * 2 + 1)] = "" + mov5[2] + mov5[3];
                    k = 0;
                }
                while (k == 6)
                {
                    mov4 = d;
                    pointery[1, mov2] = mov4;
                    k = 0;
                }
                c = "0";
                d = "0";
                e = false;
            }

            while ((k > 6 && k < 8) || k == 10 || k == 11)
            {
                //mov (subregister), (subregister)
                if (k == 7)
                {
                    if (d.Length > 2)
                    {
                        break;
                    }
                }
                int mov2 = 0;
                int mov3 = 0;
                string mov4 = "0";
                for (int i = 0; i < 8; i++)
                {
                    if (podrejestry[0, i].Contains(c))
                    {
                        mov2 = i;
                    }
                    if (podrejestry[0, i].Contains(d))
                    {
                        mov3 = i;
                    }
                }
                while (k == 7)
                {
                    mov4 = podrejestry[1, mov3];
                    podrejestry[1, mov2] = mov4;
                    if (mov2 % 2 == 1)
                    {
                        rejestry[1, (mov2 - 1) / 2] = "" + podrejestry[1, mov2 - 1] + podrejestry[1, mov2];
                    }
                    if (mov2 % 2 == 0)
                    {
                        rejestry[1, mov2 / 2] = "" + podrejestry[1, mov2] + podrejestry[1, mov2 + 1];
                    }
                    k = 0;
                }
                //mov (offset), (subregister)
                while (k == 10)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if (podrejestry[0, i].Contains(d))
                        {
                            mov2 = i;
                        }
                    }
                    string[] offset = c.Split('+', '[', ']');
                    int offset2 = 0;
                    for (int i = 0; i < offset.Length; i++)
                    {
                        int err = 0;
                        if (offset[i].Contains("BX"))
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (rejestry[0, j].Contains(offset[i]))
                                {
                                    offset2 = offset2 + Convert.ToInt32(rejestry[1, j], 16);
                                    break;
                                }
                            }
                        }
                        if (offset[i].Contains("BP") || offset[i].Contains("DI") || offset[i].Contains("SI"))
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (pointery[0, j].Contains(offset[i]))
                                {
                                    offset2 = offset2 + Convert.ToInt32(pointery[1, j], 16);
                                    break;
                                }
                            }
                        }
                        if (!offset[i].Contains("BP") && !offset[i].Contains("DI") && !offset[i].Contains("SI") && !offset[i].Contains("BX"))
                        {
                            err++;
                        }
                        for (int j = 0; j < 65536; j++)
                        {
                            if (offset[i].Contains(pamiec[1, j]))
                            {
                                offset2 = offset2 + Convert.ToInt32(pamiec[1, j], 16);
                                break;
                            }
                            if (i == 65535)
                            {
                                if (!offset[i].Contains(pamiec[1, j]))
                                {
                                    err++;
                                }
                            }
                        }
                        if (err >= 2)
                        {
                            throw new Exception("This is not offset!");
                        }
                        err = 0;
                    }
                    pamiec[0, offset2] = podrejestry[1, mov2];

                    c = "0";
                    d = "0";
                    k = 0;
                    e = false;
                    f = false;
                }
                //mov (subregister), (offset)
                while (k == 11)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if (podrejestry[0, i].Contains(c))
                        {
                            mov2 = i;
                        }
                    }
                    string[] offset = d.Split('+', '[', ']');
                    int offset2 = 0;
                    for (int i = 0; i < offset.Length; i++)
                    {
                        int err = 0;
                        if (offset[i].Contains("BX"))
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (rejestry[0, j].Contains(offset[i]))
                                {
                                    offset2 = offset2 + Convert.ToInt32(rejestry[1, j], 16);
                                    break;
                                }
                            }
                        }
                        if (offset[i].Contains("BP") || offset[i].Contains("DI") || offset[i].Contains("SI"))
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (pointery[0, j].Contains(offset[i]))
                                {
                                    offset2 = offset2 + Convert.ToInt32(pointery[1, j], 16);
                                    break;
                                }
                            }
                        }
                        if (!offset[i].Contains("BP") && !offset[i].Contains("DI") && !offset[i].Contains("SI") && !offset[i].Contains("BX"))
                        {
                            err++;
                        }
                        for (int j = 0; j < 65536; j++)
                        {
                            if (offset[i].Contains(pamiec[1, j]))
                            {
                                offset2 = offset2 + Convert.ToInt32(pamiec[1, j], 16);
                                break;
                            }
                            if (i == 65535)
                            {
                                if (!offset[i].Contains(pamiec[1, j]))
                                {
                                    err++;
                                }
                            }
                        }
                        if (err >= 2)
                        {
                            throw new Exception("This is not offset!");
                        }
                        err = 0;
                    }
                    podrejestry[1, mov2] = pamiec[0, offset2];

                    c = "0";
                    d = "0";
                    k = 0;
                    e = false;
                    f = false;
                }

                c = "0";
                d = "0";
                e = false;
            }
            
            return result;
        }

        ////////////////////////////
        //
        //         XCHG Command
        //
        ////////////////////////////
        static string XCHG(string c, string d)
        {
            string result2 = "0";

            bool e = false;
            bool f = false;
            bool f2 = false;
            int k = 0;

            for (int i = 0; i < 65536; i++)
            {
                if (d.Contains(pamiec[1, i]))
                {
                    e = true;
                }
                if (c.Contains(pamiec[1, i]) && c.Contains("[") && c.Contains("]"))
                {
                    f = true;
                }
                if (d.Contains(pamiec[1, i]) && d.Contains("[") && d.Contains("]"))
                {
                    f2 = true;
                }
            }


            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (rejestry[0, i].Contains(c) && rejestry[0, j].Contains(d))
                    {
                        k = 1;
                    }
                    if (rejestry[0, i].Contains(c) && pointery[0, j].Contains(d))
                    {
                        k = 2;
                    }
                    if (pointery[0, i].Contains(c) && pointery[0, j].Contains(d))
                    {
                        k = 3;
                    }
                    if (pointery[0, i].Contains(c) && rejestry[0, j].Contains(d))
                    {
                        k = 4;
                    }
                    if (rejestry[0, i].Contains(c) && e == true)
                    {
                        k = 5;
                    }
                    if (pointery[0, i].Contains(c) && e == true)
                    {
                        k = 6;
                    }
                    if (pointery[0, i].Contains(c) && e == true)
                    {
                        k = 6;
                    }
                    if (f == true && rejestry[0, i].Contains(d))
                    {
                        k = 8;
                    }
                    if (f2 == true && rejestry[0, i].Contains(c))
                    {
                        k = 9;
                    }

                    if (k != 0)
                    {
                        break;
                    }
                }
            }
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (podrejestry[0, i].Contains(c) && podrejestry[0, j].Contains(d))
                    {
                        k = 7;
                    }
                    if (f == true && podrejestry[0, j].Contains(d))
                    {
                        k = 10;
                    }
                    if (f2 == true && podrejestry[0, j].Contains(c))
                    {
                        k = 11;
                    }
                    if (k != 0)
                    {
                        break;
                    }
                }
            }

            if (k == 0)
            {
                throw new Exception("This is wrong command!");
            }

            while ((k > 0 && k < 5) || k == 8 || k == 9)
            {
                int mov2 = 0;
                int mov3 = 0;
                string mov7 = "0";
                string mov4 = "0";
                for (int i = 0; i < 4; i++)
                {
                    if (rejestry[0, i].Contains(c))
                    {
                        mov2 = i;
                    }
                    if (rejestry[0, i].Contains(d))
                    {
                        mov3 = i;
                    }
                    if (pointery[0, i].Contains(c))
                    {
                        mov2 = i;
                    }
                    if (pointery[0, i].Contains(d))
                    {
                        mov3 = i;
                    }
                }
                //xchg (register), (register)
                while (k == 1)
                {
                    mov4 = rejestry[1, mov3];
                    mov7 = rejestry[1, mov2];

                    rejestry[1, mov2] = mov4;
                    char[] mov5 = rejestry[1, mov2].ToCharArray();
                    podrejestry[1, mov2 * 2] = "" + mov5[0] + mov5[1];
                    podrejestry[1, (mov2 * 2 + 1)] = "" + mov5[2] + mov5[3];

                    rejestry[1, mov3] = mov7;
                    char[] mov6 = rejestry[1, mov3].ToCharArray();
                    podrejestry[1, mov3 * 2] = "" + mov6[0] + mov6[1];
                    podrejestry[1, (mov3 * 2 + 1)] = "" + mov6[2] + mov6[3];
                    k = 0;
                }
                //xchg (register), (pointer)
                while (k == 2)
                {
                    mov4 = pointery[1, mov3];
                    mov7 = rejestry[1, mov2];

                    rejestry[1, mov2] = mov4;
                    char[] mov5 = rejestry[1, mov2].ToCharArray();
                    podrejestry[1, mov2 * 2] = "" + mov5[0] + mov5[1];
                    podrejestry[1, (mov2 * 2 + 1)] = "" + mov5[2] + mov5[3];

                    pointery[1, mov3] = mov7;
                    k = 0;
                }
                //xchg (pointer), (pointer)
                while (k == 3)
                {
                    mov4 = pointery[1, mov3];
                    mov7 = pointery[1, mov2];

                    pointery[1, mov2] = mov4;

                    pointery[1, mov3] = mov7;
                    k = 0;
                }
                //xchg (pointer), (register)
                while (k == 4)
                {
                    mov4 = rejestry[1, mov3];
                    mov7 = pointery[1, mov2];

                    pointery[1, mov2] = mov4;

                    rejestry[1, mov3] = mov7;
                    char[] mov5 = pointery[1, mov2].ToCharArray();
                    podrejestry[1, mov3 * 2] = "" + mov5[0] + mov5[1];
                    podrejestry[1, (mov3 * 2 + 1)] = "" + mov5[2] + mov5[3];
                    k = 0;
                }
                //xchg (offset), (register)
                while (k == 8)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (rejestry[0, i].Contains(d))
                        {
                            mov2 = i;
                        }
                    }
                    string[] offset = c.Split(new char[] { '+', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                    int offset2 = 0;
                    for (int i = 0; i < offset.Length; i++)
                    {
                        int err = 0;
                        if (offset[i].Contains("BX"))
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (rejestry[0, j].Contains(offset[i]))
                                {
                                    offset2 = offset2 + Convert.ToInt32(rejestry[1, j], 16);
                                    break;
                                }
                            }
                        }
                        if (offset[i].Contains("BP") || offset[i].Contains("DI") || offset[i].Contains("SI"))
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (pointery[0, j].Contains(offset[i]))
                                {
                                    offset2 = offset2 + Convert.ToInt32(pointery[1, j], 16);
                                    break;
                                }
                            }
                        }
                        if (!offset[i].Contains("BP") && !offset[i].Contains("DI") && !offset[i].Contains("SI") && !offset[i].Contains("BX"))
                        {
                            err++;
                        }
                        for (int j = 0; j < 65536; j++)
                        {
                            if (offset[i].Contains(pamiec[1, j]))
                            {
                                offset2 = offset2 + Convert.ToInt32(pamiec[1, j], 16);
                                break;
                            }
                            if (i == 65535)
                            {
                                if (!offset[i].Contains(pamiec[1, j]))
                                {
                                    err++;
                                }
                            }
                        }
                        if (err >= 2)
                        {
                            throw new Exception("This is not offset!");
                        }
                        err = 0;
                    }
                    string offset3 = pamiec[0, offset2];
                    string offset5 = pamiec[0, offset2 + 1];


                    char[] offset4 = rejestry[1, mov2].ToCharArray();
                    pamiec[0, offset2] = "" + offset4[2] + offset4[3];
                    pamiec[0, offset2 + 1] = "" + offset4[0] + offset4[1];

                    rejestry[1, mov2] = "" + offset5 + offset3;
                    podrejestry[1, mov2 * 2] = "" + offset5;
                    podrejestry[1, mov2 * 2 + 1] = "" + offset3;

                    c = "0";
                    d = "0";
                    k = 0;
                    e = false;
                    f = false;
                }
                //xchg (register), (offset)
                while (k == 9)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (rejestry[0, i].Contains(c))
                        {
                            mov2 = i;
                        }
                    }
                    string[] offset = d.Split(new char[] { '+', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                    int offset2 = 0;
                    for (int i = 0; i < offset.Length; i++)
                    {
                        int err = 0;
                        if (offset[i].Contains("BX"))
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (rejestry[0, j].Contains(offset[i]))
                                {
                                    offset2 = offset2 + Convert.ToInt32(rejestry[1, j], 16);
                                    break;
                                }
                            }
                        }
                        if (offset[i].Contains("BP") || offset[i].Contains("DI") || offset[i].Contains("SI"))
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (pointery[0, j].Contains(offset[i]))
                                {
                                    offset2 = offset2 + Convert.ToInt32(pointery[1, j], 16);
                                    break;
                                }
                            }
                        }
                        if (!offset[i].Contains("BP") && !offset[i].Contains("DI") && !offset[i].Contains("SI") && !offset[i].Contains("BX"))
                        {
                            err++;
                        }
                        for (int j = 0; j < 65536; j++)
                        {
                            if (offset[i].Contains(pamiec[1, j]))
                            {
                                offset2 = offset2 + Convert.ToInt32(pamiec[1, j], 16);
                                break;
                            }
                            if (j == 65535)
                            {
                                while (!offset[i].Contains(pamiec[1, j]))
                                {
                                    err++;
                                    break;
                                }
                            }
                        }
                        if (err >= 2)
                        {
                            throw new Exception("This is not offset!");
                        }
                        err = 0;
                    }
                    char[] offset4 = rejestry[1, mov2].ToCharArray();
                    string offset3 = podrejestry[1, mov2 * 2];
                    string offset5 = podrejestry[1, mov2 * 2 + 1];

                    rejestry[1, mov2] = pamiec[0, offset2 + 1] + pamiec[0, offset2];
                    podrejestry[1, mov2 * 2] = pamiec[0, offset2 + 1];
                    podrejestry[1, mov2 * 2 + 1] = pamiec[0, offset2];

                    pamiec[0, offset2] = offset5;
                    pamiec[0, offset2 + 1] = offset3;

                    c = "0";
                    d = "0";
                    k = 0;
                    e = false;
                    f = false;
                    f2 = false;
                }
                c = "0";
                d = "0";

            }

            while ((k > 6 && k < 8) || k == 10 || k == 11)
            {
                //xchg (subregister), (subregister)
                if (k == 7)
                {
                    if (d.Length > 2)
                    {
                        break;
                    }
                }
                int mov2 = 0;
                int mov3 = 0;
                string mov4 = "0";
                string mov7 = "0";
                for (int i = 0; i < 8; i++)
                {
                    if (podrejestry[0, i].Contains(c))
                    {
                        mov2 = i;
                    }
                    if (podrejestry[0, i].Contains(d))
                    {
                        mov3 = i;
                    }
                }
                while (k == 7)
                {
                    mov4 = podrejestry[1, mov3];
                    mov7 = podrejestry[1, mov2];
                    
                    podrejestry[1, mov2] = mov4;
                    podrejestry[1, mov3] = mov7;


                    if (mov2 % 2 == 1)
                    {
                        rejestry[1, (mov2 - 1) / 2] = "" + podrejestry[1, mov2 - 1] + podrejestry[1, mov2];
                    }
                    if (mov2 % 2 == 0)
                    {
                        rejestry[1, mov2 / 2] = "" + podrejestry[1, mov2] + podrejestry[1, mov2 + 1];
                    }

                    if (mov3 % 2 == 1)
                    {
                        rejestry[1, (mov3 - 1) / 2] = "" + podrejestry[1, mov3 - 1] + podrejestry[1, mov3];
                    }
                    if (mov3 % 2 == 0)
                    {
                        rejestry[1, mov3 / 2] = "" + podrejestry[1, mov3] + podrejestry[1, mov3 + 1];
                    }
                    
                    k = 0;
                }
                //xchg (offset), (subregister)
                while (k == 10)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if (podrejestry[0, i].Contains(d))
                        {
                            mov2 = i;
                        }
                    }
                    string[] offset = c.Split('+', '[', ']');
                    int offset2 = 0;
                    for (int i = 0; i < offset.Length; i++)
                    {
                        int err = 0;
                        if (offset[i].Contains("BX"))
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (rejestry[0, j].Contains(offset[i]))
                                {
                                    offset2 = offset2 + Convert.ToInt32(rejestry[1, j], 16);
                                    break;
                                }
                            }
                        }
                        if (offset[i].Contains("BP") || offset[i].Contains("DI") || offset[i].Contains("SI"))
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (pointery[0, j].Contains(offset[i]))
                                {
                                    offset2 = offset2 + Convert.ToInt32(pointery[1, j], 16);
                                    break;
                                }
                            }
                        }
                        if (!offset[i].Contains("BP") && !offset[i].Contains("DI") && !offset[i].Contains("SI") && !offset[i].Contains("BX"))
                        {
                            err++;
                        }
                        for (int j = 0; j < 65536; j++)
                        {
                            if (offset[i].Contains(pamiec[1, j]))
                            {
                                offset2 = offset2 + Convert.ToInt32(pamiec[1, j], 16);
                                break;
                            }
                            if (i == 65535)
                            {
                                if (!offset[i].Contains(pamiec[1, j]))
                                {
                                    err++;
                                }
                            }
                        }
                        if (err >= 2)
                        {
                            throw new Exception("This is not offset!");
                        }
                        err = 0;
                    }
                    string offset3 = pamiec[0, offset2];
                    pamiec[0, offset2] = podrejestry[1, mov2];
                    podrejestry[1, mov2] = offset3;

                    c = "0";
                    d = "0";
                    k = 0;
                    e = false;
                    f = false;
                }
                //xchg (subregister), (offset)
                while (k == 11)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if (podrejestry[0, i].Contains(c))
                        {
                            mov2 = i;
                        }
                    }
                    string[] offset = d.Split('+', '[', ']');
                    int offset2 = 0;
                    for (int i = 0; i < offset.Length; i++)
                    {
                        int err = 0;
                        if (offset[i].Contains("BX"))
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (rejestry[0, j].Contains(offset[i]))
                                {
                                    offset2 = offset2 + Convert.ToInt32(rejestry[1, j], 16);
                                    break;
                                }
                            }
                        }
                        if (offset[i].Contains("BP") || offset[i].Contains("DI") || offset[i].Contains("SI"))
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (pointery[0, j].Contains(offset[i]))
                                {
                                    offset2 = offset2 + Convert.ToInt32(pointery[1, j], 16);
                                    break;
                                }
                            }
                        }
                        if (!offset[i].Contains("BP") && !offset[i].Contains("DI") && !offset[i].Contains("SI") && !offset[i].Contains("BX"))
                        {
                            err++;
                        }
                        for (int j = 0; j < 65536; j++)
                        {
                            if (offset[i].Contains(pamiec[1, j]))
                            {
                                offset2 = offset2 + Convert.ToInt32(pamiec[1, j], 16);
                                break;
                            }
                            if (i == 65535)
                            {
                                if (!offset[i].Contains(pamiec[1, j]))
                                {
                                    err++;
                                }
                            }
                        }
                        if (err >= 2)
                        {
                            throw new Exception("This is not offset!");
                        }
                        err = 0;
                    }
                    string offset3 = podrejestry[1, mov2];
                    podrejestry[1, mov2] = pamiec[0, offset2];
                    pamiec[0, offset2] = offset3;

                    c = "0";
                    d = "0";
                    k = 0;
                    e = false;
                    f = false;
                }

                c = "0";
                d = "0";
                e = false;
            }
            return result2;
        }

        ////////////////////////////
        //
        //         PUSH Command
        //
        ////////////////////////////
        static string PUSH(string s)
        {
            string result3 = "0";

            int res = 0;
            int push2 = 0;
            if (pointery[1, 3] == "FFFF" || pointery[1, 3] == "FFFE")
            {
                throw new Exception("Pointer SP is too high!");
            }
            if (s != "AX" && s != "BX" && s != "CX" && s != "DX")
            {
                throw new Exception("This is wrong command!"); ;
            }
            while (s == "AX" || s == "BX" || s == "CX" || s == "DX")
            {
                for (int i = 0; i < 65536; i++)
                {
                    if (pamiec[1, i] == pointery[1, 3])
                    {
                        res = i + 2;
                    }
                }
                for (int i = 0; i < 4; i++)
                {
                    if (rejestry[0, i].Contains(s))
                    {
                        push2 = i;
                    }
                }

                pointery[1, 3] = pamiec[1, res];

                pamiec[0, res] = podrejestry[1, push2 * 2];
                pamiec[0, res - 1] = podrejestry[1, push2 * 2 + 1];

                s = "0";
            }
            
            End:
            return result3;
        }

        ////////////////////////////
        //
        //         POP Command
        //
        ////////////////////////////
        static string POP(string s)
        {
            string result4 = "0";
            int res = 0;
            int pop2 = 0;
            if (pointery[1, 3] == "0001" || pointery[1, 3] == "0000")
            {
                throw new Exception("Pointer SP is too low!"); ;
            }
            if (s != "AX" && s != "BX" && s != "CX" && s != "DX")
            {
                throw new Exception("This is wrong command!"); ;
            }
            while (s == "AX" || s == "BX" || s == "CX" || s == "DX")
            {
                for (int i = 0; i < 65536; i++)
                {
                    if (pamiec[1, i] == pointery[1, 3])
                    {
                        res = i;
                    }
                }
                for (int i = 0; i < 4; i++)
                {
                    if (rejestry[0, i].Contains(s))
                    {
                        pop2 = i;
                    }
                }

                pointery[1, 3] = pamiec[1, res - 2];

                podrejestry[1, pop2 * 2] = pamiec[0, res];
                podrejestry[1, pop2 * 2 + 1] = pamiec[0, res - 1];

                s = "0";
            }

        End:
            return result4;
        }
    }
}
