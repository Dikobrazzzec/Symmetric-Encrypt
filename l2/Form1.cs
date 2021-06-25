using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace l2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            symmetricCrypt starter = new symmetricCrypt();
            textBox3.Text = starter.algElect(comboBox1.SelectedIndex, comboBox2.SelectedIndex, textBox1.Text, textBox2.Text);
            try
            {
                textBox1.Text = BitConverter.ToString(starter.keyB).Replace("-", "");
            }
            catch (System.ArgumentNullException) {}
            try
            {
                textBox2.Text = BitConverter.ToString(starter.IVB).Replace("-", "");
            }
            catch (System.ArgumentNullException) {}
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label5.Text = Convert.ToString(textBox1.TextLength / 2);
            label8.Text = Convert.ToString(textBox1.TextLength * 4);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            label6.Text = Convert.ToString(textBox2.TextLength /2);
            label7.Text = Convert.ToString(textBox2.TextLength *4);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            label11.Text = Convert.ToString(textBox3.TextLength / 2);
            label1.Text = Convert.ToString(textBox3.TextLength * 4);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}

/* 
 * 64BYTE:
 * a65sd4f6aws4ef32as14d6f85a4s32df
 * a6s5d4f6a5sd1f63a5we4fa3s2df4a65
 * 48BYTE:
 * 65asd4fasdf6a5s4dfa6s5d4
 * as32df16awe4f3a2sdfa65sd
 * 32 BYTE:
 * 5bytgadfgasdfge1
 * 5qewradsasdfasdf
 * 16BYTE:
 * 5qewrads
 * 9asdfasm
 * 8BYTE:
 * q7ds
 * 4ksm
*/

//     AES        |  KEY 32 BYTE  |  256 BIT
//     AES        |   IV 16 BYTE  |  128 BIT
//    ------------|---------------|---------------
//     DES        |   KEY 8 BYTE  |   64 BIT
//     DES        |    IV 8 BYTE  |   64 BIT
//    ------------|---------------|---------------
//     RC2        |  KEY 16 BYTE  |  128 BIT
//     RC2        |   IV  8 BYTE  |   64 BIT
//    ------------|---------------|---------------
//     RIJNDAEL   |  KEY 32 BYTE  |  256 BIT
//     RIJNDAEL   |   IV 16 BYTE  |  128 BIT
//    ------------|---------------|---------------                
//     TRIPLEDES  |  KEY 24 BYTE  |  192 BIT
//     TRIPLEDES  |   IV  8 BYTE  |   64 BIT 


class symmetricCrypt
{
    public byte[] keyB;
    public byte[] IVB;

    private string fileElector()
    {
        string filePath = string.Empty;
        using (OpenFileDialog openFileDialog = new OpenFileDialog())
        {
            openFileDialog.InitialDirectory = "c:\\ | d:\\ | e:\\";
            openFileDialog.Filter = "TXT Files (*.txt)| *txt | All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
                Stream fileStream = openFileDialog.OpenFile();
                //openFileDialog.Dispose();
            }
        }
        return filePath;
    }

    private void fileSafe(string data)
    {
        using (SaveFileDialog saveFileDialog = new SaveFileDialog())
        {
            saveFileDialog.Filter = "TXT Files (*.txt)| *.txt|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog.FileName, data);
                //saveFileDialog.Dispose();
            }
        }
    }

    public string algElect(int combBox1Ind, int combBox2Ind, string key, string IV)
    {
        string dataPath = fileElector();
        string retData = String.Empty;
        if (combBox1Ind == 0)
        {
            switch (combBox2Ind)
            {
                case 0:
                    retData = encryptAES(dataPath, key, IV);
                    break;
                case 1:
                    retData = encryptDES(dataPath, key, IV);
                    break;
                case 2:
                    retData = encryptRC2(dataPath, key, IV);
                    break;
                case 3:
                    retData = encryptRijndael(dataPath, key, IV);
                    break;
                case 4:
                    retData = encryptTripleDes(dataPath, key, IV);
                    break;
            }
        }
        if (combBox1Ind == 1)
        {
            switch (combBox2Ind)
            {
                case 0:
                    retData = decryptAES(dataPath, key, IV);
                    break;
                case 1:
                    retData = decryptDes(dataPath, key, IV);
                    break;
                case 2:
                    retData = decryptRC2(dataPath, key, IV);
                    break;
                case 3:
                    retData = decryptRijndael(dataPath, key, IV);
                    break;
                case 4:
                    retData = decryptTripleDes(dataPath, key, IV);
                    break;
            }
        }
        if (retData.Length != 0)
            fileSafe(retData);
        return retData;
    }

    public static byte[] strToByte(string hex)
    {
        return Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
    }





    ///////////////////////////ENCRYPT///////////////////////////





    private string encryptAES(string dataPath, string key, string IV)
    {
        //CRUSH!!!!
        string strData;
        try
        {
            strData = File.ReadAllText(dataPath);
        }
        catch (System.ArgumentException)
        {
            return "";
        }
        byte[] encryptData = Array.Empty<byte>();
        using (Aes myAes = Aes.Create())
        {
            if (key.Length == 64)
            {
                myAes.Key = Enumerable.Range(0, key.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(key.Substring(x, 2), 16)).ToArray();
                MessageBox.Show("The custom key will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (key.Length != 64 & key.Length != 0)
                MessageBox.Show("Invalid length of key. The random key will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (IV.Length == 32)
            {
                myAes.IV = Enumerable.Range(0, IV.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(IV.Substring(x, 2), 16)).ToArray();
                MessageBox.Show("The custome IV will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (IV.Length != 32 & IV.Length != 0)
                MessageBox.Show("Invalid length of IV. The random IV will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            keyB = myAes.Key;
            IVB = myAes.IV;
            // myAes.Padding = PaddingMode.PKCS7;
            ICryptoTransform encryptor = myAes.CreateEncryptor(myAes.Key, myAes.IV);
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(strData);
                    }

                }
                encryptData = msEncrypt.ToArray();
            }
        }
        return BitConverter.ToString(encryptData).Replace("-", "");
    }

    private string encryptDES(string dataPath, string key, string IV)
    {
        string strData;
        try
        {
            strData = File.ReadAllText(dataPath);
        }
        catch (System.ArgumentException)
        {
            return "";
        }
        byte[] encryptData = Array.Empty<byte>();
        using (DES myDes = DES.Create())
        {
            if (key.Length == 16)
            {
                myDes.Key = strToByte(key);
                MessageBox.Show("The custom key will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (key.Length != 16 & key.Length != 0)
                MessageBox.Show("Invalid length of key. The random key will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (IV.Length == 16)
            {
                myDes.IV = strToByte(IV);
                MessageBox.Show("The custome IV will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (IV.Length != 16 & IV.Length != 0)
                MessageBox.Show("Invalid length of IV. The random IV will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            keyB = myDes.Key;
            IVB = myDes.IV;
            ICryptoTransform encryptor = myDes.CreateEncryptor(myDes.Key, myDes.IV);
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(strData);
                    }
                    encryptData = msEncrypt.ToArray();
                }
            }

        }
        return BitConverter.ToString(encryptData).Replace("-", "");
    }

    private string encryptRC2(string dataPath, string key, string IV)
    {
        string strData;
        try
        {
            strData = File.ReadAllText(dataPath);
        }
        catch (System.ArgumentException)
        {
            return "";
        }
        byte[] encryptData = Array.Empty<byte>();
        using (RC2 myRC2 = RC2.Create())
        {
            if (key.Length == 32)
            {
                myRC2.Key = strToByte(key);
                MessageBox.Show("The custom key will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (key.Length != 32 & key.Length != 0)
                MessageBox.Show("Invalid length of key. The random key will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (IV.Length == 16)
            {
                myRC2.IV = strToByte(IV);
                MessageBox.Show("The custome IV will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (IV.Length != 16 & IV.Length != 0)
                MessageBox.Show("Invalid length of IV. The random IV will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            keyB = myRC2.Key;
            IVB = myRC2.IV;
            ICryptoTransform encryptor = myRC2.CreateEncryptor(myRC2.Key, myRC2.IV);
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(strData);
                    }
                    encryptData = msEncrypt.ToArray();
                }
            }

        }
        return BitConverter.ToString(encryptData).Replace("-", "");
    }

    private string encryptRijndael(string dataPath, string key, string IV)
    {
        string strData;
        try
        {
            strData = File.ReadAllText(dataPath);
        }
        catch (System.ArgumentException)
        {
            return "";
        }
        byte[] encryptData = Array.Empty<byte>();
        using (Rijndael myRij = Rijndael.Create())
        {
            if (key.Length == 64)
            {
                myRij.Key = strToByte(key);
                MessageBox.Show("The custom key will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (key.Length != 64 & key.Length != 0)
                MessageBox.Show("Invalid length of key. The random key will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (IV.Length == 32)
            {
                myRij.IV = strToByte(IV);
                MessageBox.Show("The custome IV will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (IV.Length != 32 & IV.Length != 0)
                MessageBox.Show("Invalid length of IV. The random IV will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            keyB = myRij.Key;
            IVB = myRij.IV;
            ICryptoTransform encryptor = myRij.CreateEncryptor(myRij.Key, myRij.IV);
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(strData);
                    }
                    encryptData = msEncrypt.ToArray();
                }
            }

        }
        return BitConverter.ToString(encryptData).Replace("-", "");
    }

    private string encryptTripleDes(string dataPath, string key, string IV)
    {
        string strData;
        try
        {
            strData = File.ReadAllText(dataPath);
        }
        catch (System.ArgumentException)
        {
            return "";
        }
        byte[] encryptData = Array.Empty<byte>();
        using (TripleDES myTripD = TripleDES.Create())
        {
            if (key.Length == 48)
            {
                myTripD.Key = strToByte(key);
                MessageBox.Show("The custom key will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (key.Length != 48 & key.Length != 0)
                MessageBox.Show("Invalid length of key. The random key will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (IV.Length == 16)
            {
                myTripD.IV = strToByte(IV);
                MessageBox.Show("The custome IV will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (IV.Length != 16 & IV.Length != 0)
                MessageBox.Show("Invalid length of IV. The random IV will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            keyB = myTripD.Key;
            IVB = myTripD.IV;
            ICryptoTransform encryptor = myTripD.CreateEncryptor(myTripD.Key, myTripD.IV);
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(strData);
                    }
                    encryptData = msEncrypt.ToArray();
                }
            }

        }
        return BitConverter.ToString(encryptData).Replace("-", "");
    }

    ///////////////////////////DECRYPT///////////////////////////

    private string decryptAES(string dataPath, string key, string IV)
    {
        byte[] byteData;
        try
        {
            byteData = strToByte(File.ReadAllText(dataPath));
        }
        catch (System.ArgumentException)
        {
            return "";
        }
        string decryptdata = String.Empty;
        using (Aes myAes = Aes.Create())
        {
            if (key.Length == 64)
            {
                myAes.Key = strToByte(key);
                MessageBox.Show("The custom key will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (key.Length != 64 & key.Length != 0)
                MessageBox.Show("Invalid length of key. Random key will be used .", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (IV.Length == 32)
            {
                myAes.IV = strToByte(IV);
                MessageBox.Show("The custome IV will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (IV.Length != 32 & IV.Length != 0)
                MessageBox.Show("Invalid length of IV. Random IV will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            keyB = myAes.Key;
            IVB = myAes.IV;
            ICryptoTransform decryptor = myAes.CreateDecryptor(myAes.Key, myAes.IV);
            try
            {
                using (MemoryStream msDecrypt = new MemoryStream(byteData))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecryptor = new StreamReader(csDecrypt))
                        {
                            decryptdata = srDecryptor.ReadToEnd();
                        }
                    }
                } 
            }
            catch (System.Security.Cryptography.CryptographicException)
            {
                return "Invalid Key or IV";
            }
        }
        return decryptdata;
    }

    private string decryptDes(string dataPath, string key, string IV)
    {
        byte[] byteData;
        try
        {
            byteData = strToByte(File.ReadAllText(dataPath));
        }
        catch (System.ArgumentException)
        {
            return "";
        }
        string decryptdata = String.Empty;
        using (DES myDes = DES.Create())
        {
            if (key.Length == 16)
            {
                myDes.Key = strToByte(key);
                MessageBox.Show("The custom key will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (key.Length != 16 & key.Length != 0)
                MessageBox.Show("Invalid length of key. Random key will be used .", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (IV.Length == 16)
            {
                myDes.IV = strToByte(IV);
                MessageBox.Show("The custome IV will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (IV.Length != 16 & IV.Length != 0)
                MessageBox.Show("Invalid length of IV. Random IV will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            keyB = myDes.Key;
            IVB = myDes.IV;
            ICryptoTransform decryptor = myDes.CreateDecryptor(myDes.Key, myDes.IV);
            try
            {
                using (MemoryStream msDecrypt = new MemoryStream(byteData))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecryptor = new StreamReader(csDecrypt))
                        {
                            decryptdata = srDecryptor.ReadToEnd();
                        }
                    }
                }
            }
            catch (System.Security.Cryptography.CryptographicException)
            {
                return "Invalid Key or IV";
            }
        }
        return decryptdata;
    }

    private string decryptRC2(string dataPath, string key, string IV)
    {
        byte[] byteData;
        try
        {
            byteData = strToByte(File.ReadAllText(dataPath));
        }
        catch (System.ArgumentException)
        {
            return "";
        }
        string decryptdata = String.Empty;
        using (RC2 myRC2 = RC2.Create())
        {
            if (key.Length == 32)
            {
                myRC2.Key = strToByte(key);
                MessageBox.Show("The custom key will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (key.Length != 32 & key.Length != 0)
                MessageBox.Show("Invalid length of key. Random key will be used .", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (IV.Length == 16)
            {
                myRC2.IV = strToByte(IV);
                MessageBox.Show("The custome IV will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (IV.Length != 16 & IV.Length != 0)
                MessageBox.Show("Invalid length of IV. Random IV will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            keyB = myRC2.Key;
            IVB = myRC2.IV;
            ICryptoTransform decryptor = myRC2.CreateDecryptor(myRC2.Key, myRC2.IV);
            try
            {
                using (MemoryStream msDecrypt = new MemoryStream(byteData))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecryptor = new StreamReader(csDecrypt))
                        {
                            decryptdata = srDecryptor.ReadToEnd();
                        }
                    }
                }
            }
            catch (System.Security.Cryptography.CryptographicException)
            {
                return "Invalid Key or IV";
            }
        }
        return decryptdata;
    }

    private string decryptRijndael(string dataPath, string key, string IV)
    {
        byte[] byteData;
        try
        {
            byteData = strToByte(File.ReadAllText(dataPath));
        }
        catch (System.ArgumentException)
        {
            return "";
        }
        string decryptdata = String.Empty;
        using (Rijndael myRij = Rijndael.Create())
        {
            if (key.Length == 64)
            {
                myRij.Key = strToByte(key);
                MessageBox.Show("The custom key will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (key.Length != 64 & key.Length != 0)
                MessageBox.Show("Invalid length of key. Random key will be used .", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (IV.Length == 32)
            {
                myRij.IV = strToByte(IV);
                MessageBox.Show("The custome IV will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (IV.Length != 32 & IV.Length != 0)
                MessageBox.Show("Invalid length of IV. Random IV will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            keyB = myRij.Key;
            IVB = myRij.IV;
            ICryptoTransform decryptor = myRij.CreateDecryptor(myRij.Key, myRij.IV);
            try
            {
                using (MemoryStream msDecrypt = new MemoryStream(byteData))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecryptor = new StreamReader(csDecrypt))
                        {
                            decryptdata = srDecryptor.ReadToEnd();
                        }
                    }
                }
            }
            catch (System.Security.Cryptography.CryptographicException)
            {
                return "Invalid Key or IV";
            }
        }
        return decryptdata;
    }

    private string decryptTripleDes(string dataPath, string key, string IV)
    {
        byte[] byteData;
        try
        {
            byteData = strToByte(File.ReadAllText(dataPath));
        }
        catch (System.ArgumentException)
        {
            return "";
        }
        string decryptdata = String.Empty;
        using (TripleDES myTripD = TripleDES.Create())
        {
            if (key.Length == 48)
            {
                myTripD.Key = strToByte(key);
                MessageBox.Show("The custom key will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (key.Length != 48 & key.Length != 0)
                MessageBox.Show("Invalid length of key. Random key will be used .", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (IV.Length == 16)
            {
                myTripD.IV = strToByte(IV);
                MessageBox.Show("The custome IV will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (IV.Length != 16 & IV.Length != 0)
                MessageBox.Show("Invalid length of IV. Random IV will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            keyB = myTripD.Key;
            IVB = myTripD.IV;
            ICryptoTransform decryptor = myTripD.CreateDecryptor(myTripD.Key, myTripD.IV);
            try
            {
                using (MemoryStream msDecrypt = new MemoryStream(byteData))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecryptor = new StreamReader(csDecrypt))
                        {
                            decryptdata = srDecryptor.ReadToEnd();
                        }
                    }
                }
            }
            catch (System.Security.Cryptography.CryptographicException)
            {
                return "Invalid Key or IV";
            }
        }
        return decryptdata;
    }
}
