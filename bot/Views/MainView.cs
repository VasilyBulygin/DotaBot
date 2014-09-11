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
        private Process[] processes;

        public MainView()
        {
            InitializeComponent();
            dgvWorkers.DataSource = dotabot.Workers;
            dotabot.OnWorkerAdded += (PID) => { AddMsg(String.Format("Worker added for PID {0}", PID)); };
            dotabot.OnWorkerRemoved += (PID) => { AddMsg(String.Format("Worker with PID {0} removed", PID)); };
            dotabot.OnWorkerMsg += (PID, msg) => { AddMsg(String.Format("PID{0}: {1}", PID, msg)); };
            processes = Process.GetProcessesByName("dota");
            cbPID.DataSource = processes;
            cbPID.ValueMember = "Id";
            //comboBox1.DataSource = processes;
        }


        private void AddMsg(string msg)
        {
            this.Invoke((MethodInvoker)delegate
            {
                lbLog.Items.Add(msg);
                lbLog.TopIndex = lbLog.Items.Count - 1;
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


            //93,385,3,3 - Трон Dire на минимапе в пике
            //147,472,52,4 - Регион опыта
            //320,406,2,10 - Регион проверки на текущее здоровье
        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            dotabot.Dispose();
        }


        private void btnAddWindow_Click(object sender, EventArgs e)
        {
            dotabot.AddWorkerByPID(((int)cbPID.SelectedValue).ToString());
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
        private void dgvWorkers_SelectionChanged(object sender, EventArgs e)
        {
            pgWorkerProperties.SelectedObject = ((DotaBotWorker)dgvWorkers.SelectedRows[0].DataBoundItem).Options;
        }

        private void btnRefreshProcesses_Click(object sender, EventArgs e)
        {
            processes = Process.GetProcessesByName("dota");
            cbPID.DataSource = processes;
            cbPID.ValueMember = "Id";
        }

    }
}
