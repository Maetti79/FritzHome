﻿using System;
using System.Windows.Forms;
using System.Globalization;

namespace HorstHome
{
    public partial class AlarmView : UserControl
    {
        String Uri;
        String SID;
        SmartDevice device;
        Boolean Initiated;
        private HorstHome parent;

        public AlarmView()
        {
            InitializeComponent();
        }
        
        public AlarmView(HorstHome mainfrm, String u, String s, SmartDevice d)
        {
            InitializeComponent();
            parent = mainfrm;
            changeCulture();
            Uri = u;
            SID = s;
            device = d;

            if (device.GetType() == typeof(HanFun))
            {
                pictureBox6.Image = DeviceIcons.Images[0];
            }
            if (device.GetType() == typeof(HanFunMotion))
            {
                pictureBox6.Image = DeviceIcons.Images[0];
            }
            if (device.GetType() == typeof(HanFunSmoke))
            {
                pictureBox6.Image = DeviceIcons.Images[1];
            }

            Modell.Text = device.GetType().ToString().Replace("HorstHome.", "");
            DeviceName.Text = device.DeviceName;
            Identifier.Text = device.Identifier;
            Manufacturer.Text = device.Manufacturer;
            Firmware.Text = device.FirmwareVersion;

            updateDynamics();
            refreshTimer.Enabled = true;
        }

        public void changeCulture()
        {
            if (parent != null)
            {
                CultureInfo.DefaultThreadCurrentCulture = parent.culture;
                CultureInfo.DefaultThreadCurrentUICulture = parent.culture;
                if (parent.culture.ToString() == "de-DE")
                {
                    LabelModel.Text = i18n.de.Detail_Model.ToString();
                    LabelName.Text = i18n.de.Detail_Name.ToString();
                    LabelIdentifier.Text = i18n.de.Detail_Indentifier.ToString();
                    LabelManufacturer.Text = i18n.de.Detail_Manufacturer.ToString();
                    LabelBattery.Text = i18n.de.Detail_Battery.ToString();
                }
                else
                {
                    LabelModel.Text = i18n.en.Detail_Model.ToString();
                    LabelName.Text = i18n.en.Detail_Name.ToString();
                    LabelIdentifier.Text = i18n.en.Detail_Indentifier.ToString();
                    LabelManufacturer.Text = i18n.en.Detail_Manufacturer.ToString();
                    LabelBattery.Text = i18n.en.Detail_Battery.ToString();
                }
            }
        }

        public void updateDynamics()
        {
            if (Initiated == true)
            {
 
            }
            else
            {
    
            }
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Value = device.Battery;
            pictureBox5.Image = BatteryIcons.Images[device.BatteryIconIndex()];
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            device.tryUpdate(Uri, SID, 30);
            DateTime Now = DateTime.Now;
            if (Now.Subtract(device.lastUpdate).TotalSeconds > 0)
            {
                Initiated = true;
            }
            updateDynamics();
        }

        private void AlarmView_Load(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}
