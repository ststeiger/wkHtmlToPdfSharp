using System;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;
using wkHtmlToPdfSharp;
using wkHtmlToPdfSharp.Synchronized;

namespace Html2PdfTestApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            Text += " v" + wkHtmlToPdfSharpStatic.Version;
        }

        private void OnConvertButtonClick(object sender, EventArgs e)
        {
            PerformanceCollector pc = new PerformanceCollector("PDF creation");

            //wkHtmlToPdfSharpStatic.InitLib(false);

            pc.FinishAction("Library initialized");

            SynchronizedwkHtmlToPdfSharp sc = new SynchronizedwkHtmlToPdfSharp(new GlobalConfig().SetMargins(new Margins(0, 0, 0, 0))
                .SetDocumentTitle("Ololo").SetCopyCount(1).SetImageQuality(50)
                .SetLosslessCompression(true).SetMaxImageDpi(20).SetOutlineGeneration(true).SetOutputDpi(1200).SetPaperOrientation(true)
                .SetPaperSize(250, 200)
                //.SetPaperSize(PaperKind.Letter)
            );

            pc.FinishAction("Converter created");

            sc.Begin += OnScBegin;
            sc.Error += OnScError;
            sc.Warning += OnScWarning;
            sc.PhaseChanged += OnScPhase;
            sc.ProgressChanged += OnScProgress;
            sc.Finished += OnScFinished;

            pc.FinishAction("Event handlers installed");

            byte[] buf = sc.Convert(new ObjectConfig(), htmlText.Text);
                /*sc.Convert(new ObjectConfig().SetPrintBackground(true).SetProxyString("http://localhost:8888")
                .SetAllowLocalContent(true).SetCreateExternalLinks(false).SetCreateForms(false).SetCreateInternalLinks(false)
                .SetErrorHandlingType(ObjectConfig.ContentErrorHandlingType.Ignore).SetFallbackEncoding(Encoding.ASCII)
                .SetIntelligentShrinking(false).SetJavascriptDebugMode(true).SetLoadImages(true).SetMinFontSize(16)
                .SetRenderDelay(2000).SetRunJavascript(true).SetIncludeInOutline(true).SetZoomFactor(2.2), htmlText.Text);*/

            pc.FinishAction("conversion finished");

            if (buf == null)
            {
                MessageBox.Show("Error converting!");

                return;
            }

            //for (int i = 0; i < 1000; i++)
            {
                buf = sc.Convert(new ObjectConfig(), htmlText.Text);

                if (buf == null)
                {
                    MessageBox.Show("Error converting!");

                    return;
                }
            }

            pc.FinishAction("1 more conversions finished");

            try
            {
                string fn = Path.GetTempFileName() + ".pdf";

                FileStream fs = new FileStream(fn, FileMode.Create);
                fs.Write(buf, 0, buf.Length);
                fs.Close();

                pc.FinishAction("dumped file to disk");

                Process myProcess = new Process();
                myProcess.StartInfo.FileName = fn;
                myProcess.Start();

                pc.FinishAction("opened it");
            } catch { }

            // pc.ShowInMessageBox(null);
        }

        public void SetText(string text)
        {
            Text = text;
        }



        //public delegate void Action<in T>(T obj);

        private void OnScProgress(SimplewkHtmlToPdfSharp converter, int progress, string progressdescription)
        {
            if (InvokeRequired)
            {
                // simple Invoke WILL NEVER SUCCEDE, because we're in the button click handler. Invoke will simply deadlock everything

                //BeginInvoke((Action<string>)SetText, "Progress " + progress + ": " + progressdescription);
            }
            else
            {
                Text = ("Progress " + progress + ": " + progressdescription);
            }
        }

        private void OnScPhase(SimplewkHtmlToPdfSharp converter, int phasenumber, string phasedescription)
        {
            if (InvokeRequired)
            {
                BeginInvoke((Action)(() => { Text = ("New Phase " + phasenumber + ": " + phasedescription); }));
            }
            else
            {
                Text = ("New Phase " + phasenumber + ": " + phasedescription);
            }
        }

        private void OnScFinished(SimplewkHtmlToPdfSharp converter, bool success)
        {
            if (InvokeRequired)
            {
                BeginInvoke((Action)(() => { Text = ("Finished, Success: " + success); }));
            }
            else
            {
                Text = ("Finished, Success: " + success);
            }
        }

        private void OnScWarning(SimplewkHtmlToPdfSharp converter, string warningtext)
        {
            if (InvokeRequired)
            {
                BeginInvoke((Action)(() => { MessageBox.Show("Warning: " + warningtext); }));
            }
            else
            {
                MessageBox.Show("Warning: " + warningtext);
            }
        }


        private void OnScBegin(SimplewkHtmlToPdfSharp converter, int expectedphasecount)
        {
            if (InvokeRequired)
            {
                BeginInvoke((Action)(() => { Text = ("Begin, PhaseCount: " + expectedphasecount); }));
            }
            else
            {
                Text = ("Begin, PhaseCount: " + expectedphasecount);
            }
        }


        private void OnScError(SimplewkHtmlToPdfSharp converter, string errorText)
        {
            if (InvokeRequired)
            {
                BeginInvoke((Action)(() => { MessageBox.Show("Error: " + errorText); }));
            }
            else
            {
                MessageBox.Show("Error: " + errorText);
            }
        }
        

        private void htmlText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                this.htmlText.SelectAll();
            }
        }


    }


}
