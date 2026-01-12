using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public static class MoaiUtils
{
    public static float GetMaxFromTable(float[][] table)
    {
        float max = 0f;
        for (int i = 0; i < table.Length; i++)
        {
            for (int j = 0; j < table[i].Length; j++)
            {
                if (table[i][j] > max)
                {
                    max = table[i][j];
                }
            }
        }
        return max;
    }

    public static void CsvLogMessage(string message, bool date, bool addEmptyLine)
    {
        using (StreamWriter writer = new(logFilePath, append: true))
        {
            if(date)
            {
                writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            }
            writer.WriteLine($"{message}");
            if (addEmptyLine)
            {
                writer.WriteLine();
            }
        }
    }

    public static void CsvLogTable(float[][] table, string additionalLog, bool date, bool addEmptyLine)
    {
        int maxX = table.Length;
        int maxY = table[0].Length;

        using (StreamWriter writer = new(logFilePath, append: true))
        {
            if (date)
            {
                writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            }
            if (additionalLog != "")
            {
                writer.WriteLine($"{additionalLog}");
            }

            for (int i = 0; i < maxX; i++)
            {
                string[] row = new string[maxY];
                for (int j = 0; j < maxY; j++)
                {
                    row[j] = table[i][j].ToString("0.####");
                }
                writer.WriteLine(string.Join(";", row));
            }

            if (addEmptyLine)
            {
                writer.WriteLine();
            }
        }
    }

    private const string logFilePath = "MoaiLearningLog.csv";   
}
