using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Web;
using System.IO;
using System.Reflection;
using System.Net;
using System.Text.RegularExpressions;

namespace Faicheck
{
    public partial class FaicheckLauncher : Form
    {
        static String VERSION = "v1.1.0";


        public FaicheckLauncher()
        {
            InitializeComponent();
            inicio();
            
        }

        public static void CopyStream(Stream input, Stream output)
        {
            // Insert null checking here for production
            byte[] buffer = new byte[8192];

            int bytesRead;
            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, bytesRead);
            }
        }

        void lanzar_deprecated()
        {

            try
            {
                Assembly _assembly;
                Stream _imageStream;

                if (System.IO.File.Exists(Environment.CurrentDirectory + @"\Faicheck.jar"))

                {

                    //delete

                    System.IO.File.Delete(Environment.CurrentDirectory + @"\Faicheck.jar");
                }

                _assembly = Assembly.GetExecutingAssembly();
                _imageStream = _assembly.GetManifestResourceStream("Faicheck.Faicheck.jar");

                using (Stream input = _imageStream)
                using (Stream output = File.Create(Environment.CurrentDirectory + @"\Faicheck.jar"))
                {
                    CopyStream(input, output);
                }

            }
            catch
            {
                MessageBox.Show("Error accessing resources!");
            }



            
            


            var processInfo = new ProcessStartInfo("java.exe", @"-jar Faicheck.jar")
            {
                CreateNoWindow = true,
                UseShellExecute = false
            };
            Process proc;

            if ((proc = Process.Start(processInfo)) == null)
            {
                throw new InvalidOperationException("??");
            }

            proc.WaitForExit();
            int exitCode = proc.ExitCode;
            // proc.Close();

            salir();

        }

        void lanzar()
        {

            lanzar(Environment.CurrentDirectory);// + @"\Faicheck");

           /* if (tipo() == 2)
            {
                lanzar(Environment.CurrentDirectory);// + @"\.faitic_checker");
            }
            else
            {
                lanzar(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+@"\.faitic_checker");
            }
            */
        }

       

        void lanzar(String path)
        {
            //MessageBox.Show("Lanzar: "+ "java.exe" + " " + @"-jar " + path + @"\Faicheck.jar");

            if (!File.Exists(path + @"\Faicheck\Faicheck.jar"))
            {
                iniciar_jar(path);
            }
            else {

                var processInfo = new ProcessStartInfo("java.exe", @"-jar " + path + @"\Faicheck\Faicheck.jar")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false
                };
                Process proc;

                if ((proc = Process.Start(processInfo)) == null)
                {
                    throw new InvalidOperationException("??");
                }

                //proc.WaitForExit();
                //int exitCode = proc.ExitCode;
                // proc.Close();

                salir();
            }

        }

        Boolean JAVA()
        {
            Boolean java = true;

            try
            {
                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo("java", "-version ");

                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.RedirectStandardError = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = true;
                System.Diagnostics.Process proc = new Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                proc.StandardError.ReadToEnd();

            }
            catch (Exception objException)
            {
                java = false;
            }

            return java;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://java.com/en/download/manual.jsp");
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        int tipo()
        {
            int res = 0;

            String path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\.faitic_checker\login.conf";

            if (System.IO.File.Exists(path)) {
                res = 1;
            } else {
                String path2 = Environment.CurrentDirectory + @"\faitic_checker-settings\login.conf";

                if (System.IO.File.Exists(path2))
                {
                    res = 2;
                }
                else {

                }
            }


            return res;

        }

        Boolean configurado()
        {
            return tipo()!=0;
        }

        void inicio()
        {
            if (JAVA()) {

                iniciar(Environment.CurrentDirectory);

                errorJava.Visible = false;
                Configuracion.Visible = false;

                /*if (!configurado())
                {
                    Configuracion.Visible = true;
                }
                else {
                    lanzar();
                }*/
            } else {
                errorJava.Visible = true;
                Configuracion.Visible = false;
            }
        }

        void salir()
        {
            /*if (File.Exists(Environment.CurrentDirectory + @"\Faicheck.jar"))
            {
                File.Delete(Environment.CurrentDirectory + @"\Faicheck.jar");
            }*/

            if (System.Windows.Forms.Application.MessageLoop)
            {
                // WinForms app
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                // Console app
                System.Environment.Exit(1);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            iniciar(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

            

        }

        private void button3_Click(object sender, EventArgs e)
        {
            iniciar(Environment.CurrentDirectory);
        }

        void iniciar(String path)
        {
            String nombre = "";


            /*if (path.Contains(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))) {
                nombre = @"\.faitic_checker\";
                path_selec = path + @"\.faitic_checker\";
            }
            else
            {
                nombre = @"\faitic_checker-settings";
                path_selec = path;
            }
            */

            nombre = @"\Faicheck";
            path_selec = path;

            bool exists = System.IO.Directory.Exists(path + nombre);

            if (!exists)
            {
                System.IO.Directory.CreateDirectory(path + nombre);
            }

            bool exists2 = System.IO.Directory.Exists(path + nombre + @"\faitic_checker-settings");

            if (!exists2)
            {
                System.IO.Directory.CreateDirectory(path + nombre + @"\faitic_checker-settings");
            }

            if (!File.Exists(path + nombre + @"\faitic_checker-settings\login.conf"))
            {

                System.IO.File.WriteAllLines(path + nombre + @"\faitic_checker-settings\login.conf", new string[] { "{\"Language\":\"es\" });//,\"Username\":\"\"}" });

                //File.Create(path + @"\.faitic_checker\login.conf");
                //TextWriter tw = new StreamWriter(path + @"\.faitic_checker\login.conf");
                //tw.WriteLine("{\"Language\":\"es\",\"Username\":\"00000000\"}");
                //tw.Close();
            }

            iniciar_jar(path);

            lanzar();

            
        }

        void iniciar_jar(String path) { 

            try
            {
                Assembly _assembly;
                Stream _imageStream;

                if (!System.IO.File.Exists(path_selec + @"\Faicheck\Faicheck.jar"))

                {

                    

                        _assembly = Assembly.GetExecutingAssembly();
                        _imageStream = _assembly.GetManifestResourceStream("Faicheck.Faicheck.jar");

                        using (Stream input = _imageStream)
                        using (Stream output = File.Create(Environment.CurrentDirectory + @"\Faicheck\Faicheck.jar"))
                        {
                            CopyStream(input, output);
                        }
                    

                }

                
            }
            catch
            {
                // MessageBox.Show("Error accessing resources!");
            }
        }

        String path_selec = "";

        void CONTINUAR_INSTALACION()
        {
            try { 

            string uno = Environment.CurrentDirectory + @"\Faicheck.jar";
            String dos = path_selec + @"\Faicheck.jar";

            if (!uno.Contains(dos))
            {
                File.Copy(uno, dos);
                System.IO.File.Delete(Environment.CurrentDirectory + @"\Faicheck.jar");
            }
        }
            catch
            {
                // MessageBox.Show("Error accessing resources!");
            }

            lanzar();
        }
       

        void DESCARGAR(String url, String path)
        {
           
            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            webClient.DownloadFileAsync(new Uri(url), path + @"\Faicheck.jar");
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            CONTINUAR_INSTALACION();
        }

        String NUEVA_VERSION()
        {
            String result = null;

            using (WebClient client = new WebClient()) 
            {
                //client.DownloadFile("https://davovoid.github.io/update-v1.json", @"C:\localfile.html");

                string htmlCode = client.DownloadString("https://davovoid.github.io/update-v1.json");

                if (!htmlCode.Contains("\"currentversion\":\"" + VERSION + "\""))
                {
                    result = Regex.Split(Regex.Split(htmlCode, "\"downloadurl\":\"")[1],"\"")[0].Replace("\\/", "/");
                }
            }

            return result;

        }
    }
}
