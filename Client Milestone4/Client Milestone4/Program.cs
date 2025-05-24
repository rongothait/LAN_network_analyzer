using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Windows.Automation;
using System.Text.RegularExpressions;


namespace Client_Milestone4
{
    class Program
    {
        static List<string> illegal_apps = new List<string>();

        public static void Connect()
        {
            Console.WriteLine("enter server's ip address");
            string ip = Console.ReadLine();
            Console.WriteLine("enter server's port number");
            int port = int.Parse(Console.ReadLine());

            bool connected = false;

            while (!connected)
            {
                try
                {
                    ClientSock clisock = new ClientSock(ip, port);
                    connected = true;
                    Console.WriteLine("connected! - waiting for further instructions");
                    Thread t = new Thread(() => communicator(clisock));
                    t.Start();
                    Thread y = new Thread(() => running_monitor(clisock));
                    y.Start();
                    clisock.Send("normal client");
                    Console.WriteLine("OSVersion: {0}", Environment.OSVersion.ToString());
                    clisock.Send(Environment.OSVersion.ToString());
                }

                catch { }
            }
        }

        

        public static void communicator(ClientSock clisock)
        {
            while(true)
            {
                string instructions = clisock.Receive();

                if (instructions.Contains("illegal list update-") && instructions.Contains("ports domain request"))
                {
                    Console.WriteLine("something is wrong...");

                    foreach (string instruction in instructions.Split('*'))
                    {
                        if (instruction.Contains("illegal list update-"))
                        {
                            instructions = instruction;
                        }
                    }
                }

                else
                {
                    instructions = instructions.Split('*')[0];
                }
                //probably the problem is that some messages are combined....
                //instructions = instructions.Split('*')[0];


                if (instructions.Contains("illegal list update-"))
                {
                    Console.WriteLine("instructions - illegal list update!!");
                    instructions = instructions.Split('-')[1];
                    illegal_apps.Clear();
                    foreach (string app in instructions.Split(','))
                    {
                        illegal_apps.Add(app.ToLower());
                        Console.WriteLine(app.ToLower());
                    }
                }

                else if (instructions.Contains("ports domain request"))
                {
                    clisock.Send(string.Format("ports domain status-{0}", Make_Port_domain_Status_Msg()));
                    //Console.WriteLine("sent ports message!!!");
                }
            }
        }


        static string Make_Port_domain_Status_Msg()
        {
            string http_str = string.Empty;
            string Telnet_str = string.Empty;
            string https_str = string.Empty;
            string msg_str = string.Empty;

            List<Connection> process_port = Get_Process_port_status();
            
            foreach (Connection con in process_port)
            {
                if (con.image_name != null)
                {
                    if (con.Foreign_address_port == "80")
                    {
                        if (http_str != string.Empty)
                        {
                            if (!http_str.Contains(con.image_name))
                            {
                                http_str += ", ";
                                http_str += con.image_name;
                            }
                        }

                        else
                        {
                            http_str += con.image_name;
                        }
                    }

                    else if (con.Foreign_address_port == "443")
                    {
                        if (https_str != string.Empty)
                        {
                            if (!https_str.Contains(con.image_name))
                            {
                                https_str += ", ";
                                https_str += con.image_name;
                            }
                        }
                        else
                        {
                            https_str += con.image_name;
                        }
                    }

                    else if (con.Foreign_address_port == "23")
                    {
                        if (Telnet_str != string.Empty)
                        {
                            if (!Telnet_str.Contains(con.image_name))
                            {
                                Telnet_str += ", ";
                                Telnet_str += con.image_name;
                            }
                        }
                        else
                        {
                            Telnet_str += con.image_name;
                        }
                    }
                }
                 
            }
            msg_str = string.Format("{0}#{1}#{2}#{3}", http_str, https_str, Telnet_str, Get_current_chrome_domain());
            return msg_str;
        }


        public static string Get_current_chrome_domain()
        {
            string domain = "";
            Process[] procsChrome = Process.GetProcessesByName("chrome");
            foreach (Process chrome in procsChrome)
            {
                // the chrome process must have a window
                if (chrome.MainWindowHandle == IntPtr.Zero)
                {
                    continue;
                }

                // find the automation element
                AutomationElement elm = AutomationElement.FromHandle(chrome.MainWindowHandle);
                AutomationElement elmUrlBar = elm.FindFirst(TreeScope.Descendants,
                  new PropertyCondition(AutomationElement.NameProperty, "Address and search bar"));

                // if it can be found, get the value from the URL bar
                if (elmUrlBar != null)
                {
                    AutomationPattern[] patterns = elmUrlBar.GetSupportedPatterns();
                    if (patterns.Length > 0)
                    {
                        ValuePattern val = (ValuePattern)elmUrlBar.GetCurrentPattern(patterns[0]);
                        string url = val.Current.Value;
                        string[] url_new = url.Split(new[] { "//" }, StringSplitOptions.None);
                        foreach (string x in url_new)
                        {
                            if (x.Contains("www"))
                            {

                                domain = x;
                                if (x.Contains("/"))
                                {
                                    domain = x.Split('/')[0];
                                }
                                domain = domain.Replace("www.", "");
                            }

                            else
                            {
                                if (x.Contains(".com"))
                                {
                                    if (x.Contains("/"))
                                    {
                                        domain = x.Split('/')[0];
                                    }

                                    if (domain.Contains("www."))
                                    {
                                        domain = domain.Replace("www.", "");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return domain;
        }

        static List<Connection> Get_Process_port_status()
        {
            List<Connection> process_port = new List<Connection>();

            string command = "netstat -a -n -o";
            string output = ExecuteCommandAndCaptureOutput(string.Format("{0} {1}", "/c", command));
            string[] new_lst = (output.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));

            foreach (string line in new_lst)
            {
                if (line.Contains("ESTABLISHED") && !(line.Contains("::")))
                {
                    string[] tmp = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    string protocol = tmp[0];
                    string local_address_ip = tmp[1].Split(':')[0];
                    string local_address_port = tmp[1].Split(':')[1];
                    string foreign_address_ip = tmp[2].Split(':')[0];
                    string foreign_address_port = tmp[2].Split(':')[1];
                    string state = tmp[3];
                    string pid = tmp[4];
                    string image_name = "";
                    try
                    {
                        image_name = Process.GetProcessById(Int32.Parse(pid)).ProcessName;
                    }
                    catch { }

                    Connection cnt = new Connection(protocol, local_address_ip, local_address_port, foreign_address_ip, foreign_address_port, state, pid, image_name);
                    process_port.Add(cnt);
                }
            }
            return process_port;
        }

        static List<string> Get_Apps_Name()
        {
            List<string> lst = new List<string>();
            StringBuilder sb = new StringBuilder();
            foreach (Process p in Process.GetProcesses("."))
            {
                try
                {
                    if (p.MainWindowTitle.Length > 0)
                    {
                        lst.Add(p.ProcessName.ToString().ToLower());
                    }
                }
                catch { }
            }
            return lst;
        }

        public static string ExecuteCommandAndCaptureOutput(string arguments)
        {
            string commandOut = string.Empty;
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();

            commandOut = process.StandardOutput.ReadToEnd();
            string errors = process.StandardError.ReadToEnd();
            return commandOut;
        }

     

        static void running_monitor(ClientSock clisock)
        {
            List<string> notified_apps = new List<string>();
            Console.WriteLine("started monitor 2.0.....");
            while(true)
            {
                foreach (string running_app in Get_Apps_Name())
                {
                    //Console.WriteLine(running_app);
                    foreach (string illegal_app in illegal_apps)
                    {
                        if (running_app.Contains(illegal_app))
                        {
                            if (!notified_apps.Contains(running_app))
                            {
                                notified_apps.Add(running_app);
                                clisock.Send(string.Format("illegal notification-{0}", illegal_app));
                                Console.WriteLine("sent to server - notfication!");
                            }
                            
                        }
                    } 
                }


                foreach(string notified_app in notified_apps)
                {
                    if (!Get_Apps_Name().Contains(notified_app))
                    {
                        notified_apps.Remove(notified_app);
                        break;
                    }

                    if (notified_apps.Count == 0)
                        break;
                }
            }
        }

        static void Main(string[] args)
        {
            Thread connect_to_server = new Thread(Program.Connect);
            connect_to_server.Start();
        }
    }
}
