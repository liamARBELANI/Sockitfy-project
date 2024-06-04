using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesServer
{
    class WavFileUtils
    {
        // פונקציה לקיצוץ קובץ WAV
        public static void TrimWavFile(string inPath, string outPath, TimeSpan cutFromStart, TimeSpan cutFromEnd, int segment)
        {
            outPath += segment + ".wav"; // הוספת שם המקטע לקובץ הפלט
            using (WaveFileReader reader = new WaveFileReader(inPath)) // קריאת קובץ ה-WAV המקורי
            {
                using (WaveFileWriter writer = new WaveFileWriter(outPath, reader.WaveFormat))
                {
                    int bytesPerMillisecond = reader.WaveFormat.AverageBytesPerSecond / 1000; // חישוב מספר הבייטים במילישנייה

                    int startPos = (int)cutFromStart.TotalMilliseconds * bytesPerMillisecond; // חישוב מיקום ההתחלה
                    startPos = startPos - startPos % reader.WaveFormat.BlockAlign; // התאמת מיקום ההתחלה ליישור הבלוק
                    int endPos = (int)cutFromEnd.TotalMilliseconds * bytesPerMillisecond; // חישוב מיקום הסיום
                    endPos = endPos - endPos % reader.WaveFormat.BlockAlign; // התאמת מיקום הסיום ליישור הבלוק
                    
                    //קריאה לפונקציה שמבצעת את החיתוך בפועל
                    TrimWavFile(reader, writer, startPos, endPos);
                }
            }
        }

        // פונקציה פרטית שמבצעת את חיתוך הקובץ
        private static void TrimWavFile(WaveFileReader reader, WaveFileWriter writer, int startPos, int endPos)
        {
            reader.Position = (int)startPos; // הגדרת המיקום ההתחלתי בקובץ הקריאה
            byte[] buffer = new byte[1024]; // יצירת באפר לקריאת הנתונים
            while (reader.Position < endPos) // לולאה לקריאת וכתיבת הנתונים עד מיקום הסיום
            {
                int bytesRequired = (int)(endPos - reader.Position); // חישוב מספר הבייטים הנדרשים
                if (bytesRequired > 0)
                {
                    int bytesToRead = Math.Min(bytesRequired, buffer.Length); // חישוב מספר הבייטים לקריאה
                    int bytesRead = reader.Read(buffer, 0, bytesToRead); // קריאת הבייטים מהקובץ המקורי
                    if (bytesRead > 0)
                    {
                        //writer.WriteData(buffer, 0, bytesRead);
                        writer.Write(buffer, 0, bytesRead);  // כתיבת הבייטים לקובץ החדש
                    }
                }
            }
        }
    }
}
