using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace bot
{
    public class DotaBot : IDisposable
    {
        public BindingList<DotaBotWorker> Workers = new BindingList<DotaBotWorker>();

        public delegate void AddWorkerEventHandler(string PID);
        public delegate void RemoveWorkerEventHandler(string PID);
        public delegate void WorkerMsgEventHandler(string PID, string msg);

        public event AddWorkerEventHandler OnWorkerAdded = delegate { };
        public event RemoveWorkerEventHandler OnWorkerRemoved = delegate { };
        public event WorkerMsgEventHandler OnWorkerMsg = delegate { };

        public DotaBot()
        {

        }


        public void AddWorkerByPID(string PID)
        {
            try
            {
                var p = System.Diagnostics.Process.GetProcessById(Convert.ToInt32(PID));
            }
            catch
            {
                return;
            }
            if (!String.IsNullOrEmpty(PID) && Workers.Where(x => x.PID == PID).FirstOrDefault() == null)
            {
                var worker = new DotaBotWorker(PID);
                worker.PostMsg += (pid, msg) => { OnWorkerMsg(pid, msg); };
                worker.Exited += (sender, e) => { OnWorkerRemoved((sender as DotaBotWorker).PID); RemoveWorker(sender as DotaBotWorker); };
                Workers.Add(worker);
                OnWorkerAdded(PID);
            }
        }

        public void StartWorker(string PID)
        {
            var query = Workers.Where(x => x.PID == PID);
            try
            {
                query.First().Start();
            }
            catch
            {
                MessageBox.Show("Unable to start worker");
            }
        }


        public void StopWorker(string PID)
        {
            var query = Workers.Where(x => x.PID == PID);
            try
            {
                query.First().Stop();
            }
            catch
            {
                MessageBox.Show("Unable to stop worker");
            }
        }

        public void RemoveWorker(DotaBotWorker worker)
        {
            try
            {
                var PID = worker.PID;
                worker.Dispose();
                Workers.Remove(worker);
                OnWorkerRemoved(PID);
            }
            catch
            {
               //TODO: добавить обработку
            }
        }

        public void StartAllWorkers()
        {
            foreach (var worker in Workers)
            {
                worker.Start();
            }
        }

        public void StopAllWorkers()
        {
            foreach (var worker in Workers)
            {
                worker.Stop();
            }
        }

        ~DotaBot()
        {
            Dispose();
        }
        #region Реализация IDisposable
        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    StopAllWorkers();
                    foreach (var worker in Workers)
                    {
                        worker.Dispose();
                    }
                }
                disposed = true;
            }
        }
        #endregion

    }
}
