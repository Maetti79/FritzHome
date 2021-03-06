﻿using System;
using System.Windows.Forms;
using System.Globalization;

namespace HorstHome
{
    public partial class ThermostatView : UserControl
    {
        String Uri;
        String SID;
        SmartDevice device;
        Boolean Initiated;
        private HorstHome parent;

        public ThermostatView()
        {
            InitializeComponent();
        }

        public ThermostatView(HorstHome mainfrm, String u, String s, SmartDevice d)
        {
            InitializeComponent();
            parent = mainfrm;
            changeCulture();
            Uri = u;
            SID = s;
            device = d;

            if (device.GetType() == typeof(DECT300))
            {
                pictureBox6.Image = DeviceIcons.Images[0];
            }
            if (device.GetType() == typeof(DECT301))
            {
                pictureBox6.Image = DeviceIcons.Images[1];
            }
            if (device.GetType() == typeof(CometDECT))
            {
                pictureBox6.Image = DeviceIcons.Images[2];
            }

            Modell.Text = device.GetType().ToString().Replace("HorstHome.", "");
            DeviceName.Text = device.DeviceName;
            Identifier.Text = device.Identifier;
            Manufacturer.Text = device.Manufacturer;
            Firmware.Text = device.FirmwareVersion;

            Initiated = false;

            TempLow.Enabled = false;
            TempLowBar.Enabled = false;
            TempHigh.Enabled = false;
            TempHighBar.Enabled = false;
            TempTarget.Enabled = false;
            TempTargetBar.Enabled = false;
            TempratureSetBtn.Enabled = false;

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
                    LabelFirmware.Text = i18n.de.Detail_Firmware.ToString();
                    LabelBattery.Text = i18n.de.Detail_Battery.ToString();
                    LabelTemperatur.Text = i18n.de.Detail_Themprature.ToString();
                    LabelTemperaturLow.Text = i18n.de.Detail_ThempratureLow.ToString();
                    LabelTemperaturComfort.Text = i18n.de.Detail_ThempratureConfort.ToString();
                    LabelTemperaturTarget.Text = i18n.de.Detail_ThempratureTarget.ToString();
                    OnBtn.Text = i18n.de.Detail_SwitchOn.ToString();
                    OffBtn.Text = i18n.de.Detail_SwitchOff.ToString();
                }
                else
                {
                    LabelModel.Text = i18n.en.Detail_Model.ToString();
                    LabelName.Text = i18n.en.Detail_Name.ToString();
                    LabelIdentifier.Text = i18n.en.Detail_Indentifier.ToString();
                    LabelManufacturer.Text = i18n.en.Detail_Manufacturer.ToString();
                    LabelFirmware.Text = i18n.en.Detail_Firmware.ToString();
                    LabelBattery.Text = i18n.en.Detail_Battery.ToString();
                    LabelTemperatur.Text = i18n.en.Detail_Themprature.ToString();
                    LabelTemperaturLow.Text = i18n.en.Detail_ThempratureLow.ToString();
                    LabelTemperaturComfort.Text = i18n.en.Detail_ThempratureConfort.ToString();
                    LabelTemperaturTarget.Text = i18n.en.Detail_ThempratureTarget.ToString();
                    OnBtn.Text = i18n.en.Detail_SwitchOn.ToString();
                    OffBtn.Text = i18n.en.Detail_SwitchOff.ToString();
                }
            }
        }

        public void updateDynamics()
        {

            if (Initiated == true)
            {
                TempLow.Enabled = true;
                TempLowBar.Enabled = true;
                TempHigh.Enabled = true;
                TempHighBar.Enabled = true;
                TempTarget.Enabled = true;
                TempTargetBar.Enabled = true;
                TempratureSetBtn.Enabled = true;
            } else {
                TempLow.Enabled = false;
                TempLowBar.Enabled = false;
                TempHigh.Enabled = false;
                TempHighBar.Enabled = false;
                TempTarget.Enabled = false;
                TempTargetBar.Enabled = false;
                TempratureSetBtn.Enabled = false;
            }
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Value = device.Battery;
            pictureBox5.Image = BatteryIcons.Images[device.BatteryIconIndex()];

            Tempratur.Text = device.Temperature.ToString() + "°C";

            TempLowBar.Minimum = SmartDevice.Temperature_min * 2;
            TempLowBar.Maximum = SmartDevice.Temperature_max * 2;
            if (device.TemperatureLow == SmartDevice.Temperature_off)
            {
                if (parent.culture.ToString() == "de-DE")
                {
                    TempLow.Text = i18n.de.Detail_SwitchOff.ToString();
                }
                else
                {
                    TempLow.Text = i18n.en.Detail_SwitchOff.ToString();
                }
                TempLowBar.Value = device.TemperatureCensiusToHKR(device.TemperatureLow);
            }
            else
            {
                TempLow.Text = device.TemperatureLow.ToString() + "°C";
                TempLowBar.Value = device.TemperatureCensiusToHKR(device.TemperatureLow);
            }

            TempHigh.Text = device.TemperatureHigh.ToString() + "°C";
            TempHighBar.Minimum = SmartDevice.Temperature_min * 2;
            TempHighBar.Maximum = SmartDevice.Temperature_max * 2;
            if (device.TemperatureHigh == SmartDevice.Temperature_off)
            {
                if (parent.culture.ToString() == "de-DE")
                {
                    TempHigh.Text = i18n.de.Detail_SwitchOff.ToString();
                }
                else
                {
                    TempHigh.Text = i18n.en.Detail_SwitchOff.ToString();
                }
                TempHighBar.Value = device.TemperatureCensiusToHKR(device.TemperatureHigh);
            }
            else
            {
                TempHigh.Text = device.TemperatureHigh.ToString() + "°C";
                TempHighBar.Value = device.TemperatureCensiusToHKR(device.TemperatureHigh);
            }

            TempTargetBar.Minimum = SmartDevice.Temperature_min * 2;
            TempTargetBar.Maximum = SmartDevice.Temperature_max * 2;
            if (device.TemperatureTarget == SmartDevice.Temperature_off)
            {
                device.Switch = false;
                if (parent.culture.ToString() == "de-DE")
                {
                    TempTarget.Text = i18n.de.Detail_SwitchOff.ToString();
                }
                else
                {
                    TempTarget.Text = i18n.en.Detail_SwitchOff.ToString();
                }
                TempTargetBar.Value = device.TemperatureCensiusToHKR(device.TemperatureTarget);
            }
            else
            {
                device.Switch = true;
                TempTarget.Text = device.TemperatureTarget.ToString() + "°C";
                TempTargetBar.Value = device.TemperatureCensiusToHKR(device.TemperatureTarget);
            }

            if (device.Switch == true)
            {
                OnBtn.Enabled = false;
                OffBtn.Enabled = true;
            }
            else
            {
                OnBtn.Enabled = true;
                OffBtn.Enabled = false;
            }

        }

        private void ThermostatView_Load(object sender, EventArgs e)
        {

        }

        private void TempTarget_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void TempratureSetBtn_Click(object sender, EventArgs e)
        {
            try
            {
                double celsius = device.TemperatureHKRToCelsius(TempTargetBar.Value);
                device.TemperatureTarget = celsius;
                device.setTemperatureAsync(Uri, SID, device.TemperatureTarget);
            }
            catch (Exception ex) { }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            double celsius = device.TemperatureHKRToCelsius(TempTargetBar.Value);
            device.TemperatureTarget = celsius;
            TempTarget.Text = device.TemperatureTarget.ToString() + "°C";
        }

        private void Manufacturer_TextChanged(object sender, EventArgs e)
        {

        }

        private void Identifier_TextChanged(object sender, EventArgs e)
        {

        }

        private void Modell_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void DeviceName_TextChanged(object sender, EventArgs e)
        {

        }

        private void TempLow_TextChanged(object sender, EventArgs e)
        {

        }

        private void TempHigh_TextChanged(object sender, EventArgs e)
        {

        }

        private void Tempratur_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {

        }

        private void Firmware_TextChanged(object sender, EventArgs e)
        {

        }

        private void TempHighBar_Scroll(object sender, EventArgs e)
        {
            double celsius = device.TemperatureHKRToCelsius(TempHighBar.Value);
            device.TemperatureHigh = celsius;
            TempHigh.Text = device.TemperatureHigh.ToString() + "°C";
        }

        private void TempLowBar_Scroll(object sender, EventArgs e)
        {
            double celsius = device.TemperatureHKRToCelsius(TempLowBar.Value);
            device.TemperatureLow = celsius;
            TempLow.Text = device.TemperatureLow.ToString() + "°C";
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

        private void OnBtn_Click(object sender, EventArgs e)
        {
            OnBtn.Enabled = false;
            OffBtn.Enabled = false;
            if (device.setSwitchOnAsync(Uri, SID).Result)
            {
                device.Switch = true;
                OnBtn.Enabled = false;
                OffBtn.Enabled = true;
            }
        }

        private void OffBtn_Click(object sender, EventArgs e)
        {
            OnBtn.Enabled = false;
            OffBtn.Enabled = false;
            if (device.setSwitchOffAsync(Uri, SID).Result)
            {
                device.Switch = false;
                OnBtn.Enabled = true;
                OffBtn.Enabled = false;
            }
        }
    }
}
