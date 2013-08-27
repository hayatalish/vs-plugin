﻿//* Copyright 2010-2011 Research In Motion Limited.
//*
//* Licensed under the Apache License, Version 2.0 (the "License");
//* you may not use this file except in compliance with the License.
//* You may obtain a copy of the License at
//*
//* http://www.apache.org/licenses/LICENSE-2.0
//*
//* Unless required by applicable law or agreed to in writing, software
//* distributed under the License is distributed on an "AS IS" BASIS,
//* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//* See the License for the specific language governing permissions and
//* limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.Win32;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using PkgResources = RIM.VSNDK_Package.Resources;
using RIM.VSNDK_Package.Settings.Models;
using System.Net;

namespace RIM.VSNDK_Package.Settings
{

    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsDialog : DialogWindow
    {
        /// <summary>
        /// Settings Dialog Constructor
        /// </summary>
        public SettingsDialog()
        {
            InitializeComponent();
            SettingsData data = gridMain.DataContext as SettingsData;
            if (data != null)
            {
                data.getSimulatorInfo();
                data.getDeviceInfo();

                tbDevicePassword.Password = data.DevicePassword;
                tbSimulatorPassword.Password = data.SimulatorPassword;
            }
        }

        /// <summary>
        /// Persist Changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            SettingsData data = gridMain.DataContext as SettingsData;
            if (data != null)
            {
                data.DevicePassword = tbDevicePassword.Password;
                data.SimulatorPassword = tbSimulatorPassword.Password;
                data.setDeviceInfo();
                data.setSimulatorInfo();
                data.setNDKPaths(); 
            }


            DialogResult = true; ;

        }

        /// <summary>
        /// Open App Target Dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = System.Windows.Input.Cursors.Wait;

            try
            {
                System.Net.WebRequest.Create("http://downloads.blackberry.com").GetResponse();

                SettingsData data = gridMain.DataContext as SettingsData;
                if (data != null)
                {
                    UpdateManager.UpdateManager win = new UpdateManager.UpdateManager();

                    bool? res = win.ShowDialog();

                    data.RefreshScreen();
                    NDKEntry.ItemsSource = null;
                    NDKEntry.ItemsSource = data.NDKEntries;
                }
            }
            catch (WebException)
            {
                System.Windows.MessageBox.Show("You are currently experiencing internet connection issues and cannot access the Update Manager server.  Please check your connection or try again later.", "Settings", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
            }

            this.Cursor = System.Windows.Input.Cursors.Hand;

        }
    }
}
