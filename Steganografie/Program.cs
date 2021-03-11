using System;
using System.Collections.Generic;
using System.Drawing;

namespace Steganografie
{
    class Program
    {
        static void Main(string[] args)
        {
            Stega.Process(args);
        }
    }

    class Stega
    {
        public static void Process(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Nebyla zadaná práce");
            }
            else
            {
                switch (args[0])
                {
                    case "--hide":
                        if (CheckParams(args, "hide"))
                            Hide(args);
                        else 
                        {
                            Console.WriteLine("Špatný formát");
                            Console.WriteLine("Více info --helpme");
                        }
                        break;
                    case "--show":
                        if (CheckParams(args, "show"))
                            Show(args);
                        else
                        {
                            Console.WriteLine("Špatný formát");
                            Console.WriteLine("Více info --helpme");
                        }
                        break;
                    case "--helpme":
                        Console.WriteLine("Známé práce: [--hide}], [--show]");
                        Console.WriteLine("Vzory: --hide *zpráva **obrázek \n       --show **obrázek " +
                            "\n*zpráva kterou chcete schovat (BEZ háčků a čárek) \n**název obrázku s typem, kam se zpráva schová");
                        Console.WriteLine("Základní obrázek pro testy: winten.png (.jpg soubory nefungují)");
                        break;
                    default:
                        Console.WriteLine("Neznámá práce");
                        Console.WriteLine("Známé práce: [--hide], [--show]");
                        Console.WriteLine("Více info --helpme");
                        break;
                }
            }
        }

        private static void Hide(string[] args)
        {
            string message = args[1];
            string image = args[2];
            Bitmap bm = new Bitmap(image);
            Color pixel;
            Color withMessage;

            int index = 0;

            int w = bm.Width;
            int h = bm.Height;

            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    pixel = bm.GetPixel(j, i);
                    withMessage = Color.FromArgb(pixel.R, pixel.G, Convert.ToByte(message[index]));
                    bm.SetPixel(j, i, withMessage);

                    index++;
                    if (index >= message.Length)
                    {
                        int ni = i + 1;
                        int nj = j;

                        if (j < w) { ni = 0; nj++; }

                        bm.SetPixel(nj, ni, Color.FromArgb(69, 78, 68));
                        break;
                    }
                }
                if (index >= message.Length)
                    break;
            }

            bm.Save("H" + image);
            Console.WriteLine("Byl vytvořen obrázek se zprávou. Nový obrázek: H" + image);
        }

        private static void Show(string[] args)
        {
            string image = args[1];
            Bitmap bm = new Bitmap(image);
            Color pixel;
            string output = "";

            int w = bm.Width;
            int h = bm.Height;

            bool end = false;

            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    pixel = bm.GetPixel(j, i);
                    end = pixel == Color.FromArgb(69, 78, 68);
                    if (!end)
                        output += Convert.ToString((char)pixel.B);
                    else
                        break;
                }
                if (end) 
                    break;
            }

            Console.WriteLine(output);
        }

        private static bool CheckParams(string[] args, string option)
        {
            switch (option)
            {
                case "hide":
                    if (args.Length == 3)
                        try
                        {
                            Bitmap bm = new Bitmap(args[2]);
                            return true;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine($"Nenalezený obrázek: {args[2]}");
                        }
                    break;
                case "show":
                    if (args.Length == 2)
                        try
                        {
                            Bitmap bm = new Bitmap(args[1]);
                            return true;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine($"Nenalezený obrázek: {args[1]}");
                        }
                    break;
                default:
                    Console.WriteLine("Neexistující volba při kontrole");
                    return false;
            }
            return false;
        }
    }
}
