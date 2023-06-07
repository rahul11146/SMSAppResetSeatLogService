using SMSApp.Models.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Web;

namespace ResetSeatLogTodaySrvc
{
    public partial class ResetSeatLogToday : ServiceBase
    {

        Timer timer = new Timer(); // name space(using System.Timers;)
        public ResetSeatLogToday()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            WriteToFile("Service is started at " + DateTime.Now);
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["TimeInterval"]); //number in milisecinds
            timer.Enabled = true;
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            try
            {
                timer.Enabled = false;

                //if (DateTime.Now.ToString("hh:mm") == ConfigurationManager.AppSettings["LogInterval"].ToString())
                {
                    SeatResetDAL mSeatResetDAL = new SeatResetDAL();
                    string mId = string.Empty;

                    mId = mSeatResetDAL.ResetSPLog("0", "N");

                    mSeatResetDAL.ResetSeatLogToday();

                    mId = mSeatResetDAL.ResetSPLog(mId, "Y");
                }

                timer.Enabled = true;
            }
            catch (Exception ex)
            {
                WriteToFile(DateTime.Now.ToString("dd_MMM_yyyy hh:mm") + ":" + ex.Message.ToString() + Environment.NewLine);
                WriteToFile(DateTime.Now.ToString("dd_MMM_yyyy hh:mm") + ":" + ex.StackTrace.ToString() + Environment.NewLine);
            }
        }

        public void OnDebug()
        {
            try
            {
                timer.Enabled = false;

                if (DateTime.Now.ToString("hh:mm") == "00:00")
                {
                    SeatResetDAL mSeatResetDAL = new SeatResetDAL();
                    string mId = string.Empty;

                    mId = mSeatResetDAL.ResetSPLog("0", "N");

                    mSeatResetDAL.ResetSeatLogToday();

                    mId = mSeatResetDAL.ResetSPLog(mId, "Y");
                }

                timer.Enabled = true;
            }
            catch (Exception ex)
            {
                WriteToFile(DateTime.Now.ToString("dd_MMM_yyyy hh:mm") + ":" + ex.Message.ToString() + Environment.NewLine);
                WriteToFile(DateTime.Now.ToString("dd_MMM_yyyy hh:mm") + ":" + ex.StackTrace.ToString() + Environment.NewLine);
            }
        }


        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to. 
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
    }
}
