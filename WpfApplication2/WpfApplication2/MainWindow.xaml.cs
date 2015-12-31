using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Cugaze
{

    public partial class MainWindow : Window
    {
        static string json = File.ReadAllText("..\\..\\buttons.json");
        static JavaScriptSerializer ser = new JavaScriptSerializer();
        static JsonMenu currentNode = ser.Deserialize<JsonMenu>(json),
                        rootNode = currentNode;
        static Stack<JsonMenu> navigationStack = new Stack<JsonMenu>();

        enum directions { up, right, down, left, middle }
        directions MouseLastLocation = directions.middle;

        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        static string playerProcessName = "Cubase LE AI Elements 7";

        

        public MainWindow()
        {
            InitializeComponent();

            navigationStack.Push(currentNode);
            LoadCurrentMenuItems();
            ShowNavigationTree();
        }

        
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
           // Application.Current.MainWindow.Height  = System.Windows.SystemParameters.PrimaryScreenHeight;
        //    Application.Current.MainWindow.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            int length = 20;
            String[] args = System.Environment.GetCommandLineArgs();
            if (args.Count() > 1)
            {
                length = Int32.Parse(args[1]);
            }
            GridLength g = new GridLength(length, GridUnitType.Pixel);
            C2.Width = g;
            C1.Width = g;
            R1.Height = g;
            R2.Height = g;


            
        }
        private void LoadCurrentMenuItems()
        {
            menuItemsList.ItemsSource = new List<JsonMenu>(currentNode.children);
            menuItemsList.SelectedIndex = 0;
        }


        private void ShowNavigationTree()
        {
            String navigationTreeText = "",
                   separator = "";

            foreach (JsonMenu menu in navigationStack.Reverse())
            {
                navigationTreeText += separator + menu.title;
                separator = "/";
            }

            navigationTree.Text = navigationTreeText + separator + ((JsonMenu)menuItemsList.SelectedItem).title;
        }


        private void EnterUp(object sender, MouseEventArgs e)
        {
            MouseLastLocation = directions.up;
        }


        private void EnterDown(object sender, MouseEventArgs e)
        {
            MouseLastLocation = directions.down;
        }


        private void EnterLeft(object sender, MouseEventArgs e)
        {
            MouseLastLocation = directions.left;
        }


        private void EnterRight(object sender, MouseEventArgs e)
        {
            MouseLastLocation = directions.right;
        }


        private void EnterMiddle(object sender, MouseEventArgs e)
        {
            switch (MouseLastLocation)
            {
                case directions.up:
                    GoUp(sender, e);
                    break;
                case directions.down:
                    GoDown(sender, e);
                    break;
                case directions.right:
                    GoRight(sender, e);
                    break;
                case directions.left:
                    GoLeft(sender, e);
                    break;
            }
        }


        private void GoRight(object sender, RoutedEventArgs e)
        {
            menuItemsList.SelectedIndex = (menuItemsList.SelectedIndex + 1) % currentNode.children.Length;

            ShowNavigationTree();
        }


        private void GoLeft(object sender, RoutedEventArgs e)
        {
            if (menuItemsList.SelectedIndex == 0)
            {
                menuItemsList.SelectedIndex = currentNode.children.Length - 1;
            }
            else
            {
                menuItemsList.SelectedIndex = (menuItemsList.SelectedIndex - 1) % (currentNode.children.Length);
            }

            ShowNavigationTree();
        }


        private void GoUp(object sender, RoutedEventArgs e)
        {
            if (navigationStack.Count > 1)
            {
                navigationStack.Pop();
                currentNode = navigationStack.Peek();

                LoadCurrentMenuItems();
                ShowNavigationTree();
            }
        }


        private void GoDown(object sender, RoutedEventArgs e)
        {
            currentNode = currentNode.children[menuItemsList.SelectedIndex];
            navigationStack.Push(currentNode);

            if (currentNode.children != null)
            {
                LoadCurrentMenuItems();
                ShowNavigationTree();
            }
            else
            {
                Process process = Process.GetProcessesByName(playerProcessName).FirstOrDefault();
                if (currentNode.action != null && process != null)
                {
                    IntPtr processWindowHandler = process.MainWindowHandle;

                    SetForegroundWindow(processWindowHandler);
                    ShowWindow(processWindowHandler, 3);

                    System.Windows.Forms.SendKeys.SendWait(currentNode.action.qwertyKey);

                }

                navigationStack.Pop();
                currentNode = navigationStack.Peek();
            }
        }


        public string GetCurrentAction()
        {
            return currentNode.children[menuItemsList.SelectedIndex].title;
        }


        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


    }
}