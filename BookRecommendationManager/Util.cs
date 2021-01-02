using BookRecommendationManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookRecommendationManager
{
    class Util
    {
        #region Loading Cursor
        private static int loadingInstanceAmount = 0;
        public static void StartLoadingForCursor()
        {
            loadingInstanceAmount++;
            Cursor.Current = Cursors.WaitCursor;
        }
        public static void StopLoadingForCursor()
        {
            if (loadingInstanceAmount > 0)
                loadingInstanceAmount--;
            else
                Database.PostError("Unknown semaphore error at loading cursor mechanic.");// semaphore error, should report
            if (loadingInstanceAmount <= 0)
                Cursor.Current = Cursors.Default;
        }

        private static HashSet<Task> taskList = new HashSet<Task>();
        private const int GCRate = 20000; // 20 sec
        private static Task taskGarbageCollector = new Task(async()=>
        {
            await Task.Delay(GCRate);
            HashSet<Task> toRemove = new HashSet<Task>();
            foreach (Task task in taskList)
                if (task.IsCompleted)
                    toRemove.Add(task);
            foreach (Task task in toRemove)
                taskList.Remove(task);

            // Error correction if all else fails
            if (taskList.Count <= 0)
                loadingInstanceAmount = 0;
        });
        public static void MarkLongRunningTask(Task task)
        {
            StartLoadingForCursor();

            taskList.Add(task.ContinueWith((t) => { StopLoadingForCursor(); }));
        }
        #endregion

        #region Encrypt & Decrypt
        private const int offset = 0x40;
        private const int shift = 4;
        private static Encoding encoding = Encoding.UTF8;

        public static string Encrypt(byte[] data)
        {
            int len = data.Length;
            ushort[] data2 = new ushort[len];

            for (int i = 0; i < len; i++)
                data2[i] = (ushort)(data[i] << shift);

            byte[] data3 = new byte[len * 2];
            Buffer.BlockCopy(data2, 0, data3, 0, len * 2);

            for (int i = 0; i < len; i++)
            {
                data3[i * 2 + 1] = (byte)(data3[i * 2 + 1] + offset);
                data3[i * 2] = (byte)((data3[i * 2] >> shift) + offset);
            }

            string test = encoding.GetString(data3, 0, len * 2);
            return test;
        }

        public static byte[] Decrypt(string str)
        {
            byte[] data = encoding.GetBytes(str);
            int len = data.Length / 2;
            byte[] data2 = new byte[len];

            if (len * 2 != data.Length)
                throw new NotImplementedException();

            for (int i = 0; i < len; i++)
                data2[i] = (byte)(((data[2 * i + 1] - offset) << (8 - shift)) + (data[i * 2] - offset));

            return data2;
        }
        #endregion
    }
}
