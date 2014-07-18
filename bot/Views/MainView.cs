using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using System.Security.Cryptography;
using System.IO;
using System.Drawing.Imaging;
using System.Xml.Serialization;

namespace bot
{
    public partial class MainView : Form
    {
        private readonly DotaBot dotabot = new DotaBot();

        public MainView()
        {
            InitializeComponent();
            dgvWorkers.DataSource = dotabot.Workers;
            dotabot.OnWorkerAdded += (PID) => { AddMsg(String.Format("Worker added for PID {0}", PID)); };
            dotabot.OnWorkerRemoved += (PID) => { AddMsg(String.Format("Worker with PID {0} removed", PID)); };
            dotabot.OnWorkerMsg += (PID, msg) => { AddMsg(String.Format("PID{0}: {1}", PID, msg)); };
        }


        private void AddMsg(string msg)
        {
            this.Invoke((MethodInvoker)delegate
            {
                listBox1.Items.Add(msg);
                listBox1.TopIndex = listBox1.Items.Count - 1;
            });
        }

        private void MainFunc(object manualEvent)
        {
            //217,126,5,20 - Кнопка Найти
            //170,219,5,20 - Кнопка Принять
            //322,219,5,20 - Кнопка Отклонить
            //220,30,5,15 - Признак экрана загрузки всех игроков
            //3,337,5,20 - Кнопка Случайный герой
            //502,461,11,8 - Кнопка смайлика в меню выбора героя
            //436,345,4,4 - Доступна кнопка репика
            //229,283,5,10 - Кнопка Покинуть
            //270,216,5,10 - Кнопка Отмена при отключении от сервера после игры(CancelButton)
            //276,297,5,10 - Кнопка Нет спасибо
            //145,361,4,10 - Кнопка Уровень+1

            //147,472,52,4 - Регион опыта
            //320,406,2,10 - Регион проверки на текущее здоровье
        }

        private uint GetSum(IntPtr hwnd, int x1, int y1, int x2, int y2)
        {
            long result = 0xFFFFFF;
            for (int i = x1; i < x2; i++)
            {
                for (int j = y1; j < y2; j++)
                {
                    result = result ^ _GetPixelColor(hwnd, i, j);
                }
            }
            return (uint)result;
        }

        private uint _GetPixelColor(IntPtr hwnd, int x, int y)
        {
            
            uint result = 0;
            /*
            IntPtr dc = GetDC(hwnd);
            result = GetPixel(dc, x, y);
            if (!ReleaseDC(hwnd, dc))
            {
                throw new Exception("No ReleaseDC");
            }*/
            return result;
        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            dotabot.Dispose();
        }


        private void btnAddWindow_Click(object sender, EventArgs e)
        {
            dotabot.AddWorkerByPID(tbPID.Text);
        }

        private void dgvWorkers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvWorkers.Columns["StartWorker"].Index && e.RowIndex >= 0)
            {
                (dgvWorkers.Rows[e.RowIndex].DataBoundItem as DotaBotWorker).Start();
            }
            else if (e.ColumnIndex == dgvWorkers.Columns["StopWorker"].Index && e.RowIndex >= 0)
            {
                (dgvWorkers.Rows[e.RowIndex].DataBoundItem as DotaBotWorker).Stop();
            }
            else if (e.ColumnIndex == dgvWorkers.Columns["RemoveWorker"].Index && e.RowIndex >= 0)
            {
                dotabot.RemoveWorker(dgvWorkers.Rows[e.RowIndex].DataBoundItem as DotaBotWorker);
            }
        }

    }
}
