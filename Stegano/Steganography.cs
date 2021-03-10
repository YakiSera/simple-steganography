using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stegano
{
    static class BitString
    {
        /// <summary>
        /// Кодирует строку в биты
        /// </summary>
        /// <param name="message">Кодируемая строка</param>
        /// <param name="char_size">Размер одного символа в битах</param>
        /// <returns>Возвращает лист состоящий из нулей и единиц</returns>
        static public List<byte> EncodeToList(string message, int char_size)
        {
            List<byte> result = new List<byte>();
            int mask = 1;

            foreach (char letter in message)
            {
                int temp = letter;
                for (int i = 0; i < char_size; i++)
                {
                    result.Add((byte)(temp & mask));
                    temp = temp >> 1;
                }
            }

            return result;
        }
    }

    static class Steganography
    {
        /// <summary>
        /// Дешифровка текста из изображения
        /// </summary>
        /// <param name="image">Изображение, содержащее закодированный текст</param>
        /// <param name="char_size">Размер одного символа в битах</param>
        /// <returns>Дешифрованное сообщение</returns>
        static public string Decrypt(Bitmap image, int char_size)
        {
            string result = "";
            int currentLetter = 0;
            int bitIterator = 0;
            Color currentPixel;
            bool isLetterReady = false;
            bool endOfMessage = false;

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    isLetterReady = bitIterator == char_size - 1;
                    if (isLetterReady)
                    {
                        isLetterReady = false;
                        bitIterator = -1;
                        endOfMessage = (char)currentLetter == '$';

                        if (endOfMessage)
                        {
                            break;
                        }
                        result += (char)currentLetter;
                        currentLetter = 0;
                    }
                    
                    currentPixel = image.GetPixel(i, j);
                    currentLetter += (int)Math.Pow(2, bitIterator) * (currentPixel.R % 2);
                    bitIterator++;
                }

                if (endOfMessage)
                {
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Зашифровка текста в изображение
        /// </summary>
        /// <param name="image">Изображение, в которое будет зашифровано сообщение</param>
        /// <param name="message">Исходное сообщение</param>
        /// <param name="char_size">Размер одного символа в битах</param>
        /// <returns>Изображение с зашифрованной информацией</returns>
        static public Bitmap Encrypt(Bitmap image, string message, int char_size)
        {
            Bitmap result = new Bitmap(image);
            List<byte> bitString = BitString.EncodeToList(message, char_size);
            Color currentPixel;
            int listIter = 0;

            for(int i = 0; i < result.Width; i++)
            {
                for(int j = 0; j < result.Height; j++)
                {
                    if(listIter >= bitString.Count - 1)
                    {
                        break;
                    }

                    currentPixel = result.GetPixel(i, j);
                    if((result.GetPixel(i, j).R % 2) == 0)
                    {
                        if(bitString[listIter] == 1)
                        {
                            result.SetPixel(i, j, Color.FromArgb(currentPixel.R + 1, currentPixel.G, currentPixel.B));
                            Console.WriteLine("+1");
                        }
                    }
                    else
                    {
                        if (bitString[listIter] == 0)
                        {
                            result.SetPixel(i, j, Color.FromArgb(currentPixel.R - 1, currentPixel.G, currentPixel.B));
                            Console.WriteLine("-1");
                        }
                    }
                    listIter++;
                }

                if (listIter > bitString.Count)
                {
                    break;
                }
            }

            return result;
        }
    }
}
