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
            label1.Text = starter.output(starter.algElect(comboBox1.SelectedIndex, comboBox2.SelectedIndex, Encoding.UTF8.GetBytes(textBox1.Text), Encoding.UTF8.GetBytes(textBox2.Text)));
            textBox1.Text = starter.output(starter.keyB);
            textBox2.Text = starter.output(starter.IVB);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

/* 
 * 32BYTE:
 * a65sd4f6aws4ef32as14d6f85a4s32df
 * a6s5d4f6a5sd1f63a5we4fa3s2df4a65
 * 24BYTE:
 * 65asd4fasdf6a5s4dfa6s5d4
 * as32df16awe4f3a2sdfa65sd
 * 16 BYTE:
 * 5bytgadfgasdfge1
 * 5qewradsasdfasdf
 * 8BYTE:
 * 5qewrads
 * 9asdfasm
*/


class symmetricCrypt
{
    public byte[] keyB;
    public byte[] IVB;

    private string fileElector()
    {
        string filePath = string.Empty;
        string fileContent = string.Empty;
        using (OpenFileDialog openFileDialog = new OpenFileDialog())
        {
            openFileDialog.InitialDirectory = "c:\\ | d:\\ | e:\\";
            openFileDialog.Filter = "TXT files (*.txt)| *txt| All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
                Stream fileStream = openFileDialog.OpenFile();
            }
        }
        return filePath;
    }

    public string output(byte[] byteArr)                                //сюда надо передовать по отдельности data key IV
    {
        StringBuilder outString = new StringBuilder();
        foreach (var i in byteArr)
            outString.Append(i.ToString("X2"));
        return outString.ToString();
    }



    public byte [] algElect(int combBox1Ind, int combBox2Ind, byte[] key, byte[] IV)
    { 
        string dataPath = fileElector();
        byte[] encryptData = Array.Empty<byte>();
        if (combBox1Ind == 0)
        {
            switch (combBox2Ind)
            {
                case 0:
                    encryptData = encryptAES(dataPath, key, IV);    
                    break;
                case 1:
                    encryptData = encryptDES(dataPath, key, IV);
                    break;
                case 2:
                    encryptData = encryptRC2(dataPath, key, IV);
                    break;
                case 3:
                    encryptData = encryptRijndael(dataPath, key, IV);
                    break;
                case 4:
                    encryptData = encryptTripleDes(dataPath, key, IV);
                    break;
            }
        }
        return encryptData;
    }

    private byte[] encryptAES(string dataPath, byte[] key, byte[] IV)
    {
        FileStream data = File.OpenRead(dataPath);
        //  string data = "12312aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa3";
        //  if (data == null || data.Length <= 0)
        //     throw new ArgumentNullException("data2");
        byte[] encryptData = Array.Empty<byte>();
        using (Aes myAes = Aes.Create())
        {
            if (key.Length == 16)
            {
                myAes.Key = key;
                MessageBox.Show("The custom key will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (key.Length != 16 & key.Length != 0)
                MessageBox.Show("Invalid length of key. The standart key will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (IV.Length == 16)
            {
                myAes.IV = IV;
                MessageBox.Show("The custome IV will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (IV.Length != 16 & IV.Length != 0)
                MessageBox.Show("Invalid length of IV. The standart IV will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            keyB = myAes.Key;
            IVB = myAes.IV;
            ICryptoTransform encryptor = myAes.CreateEncryptor(myAes.Key, myAes.IV);
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(data);
                    }
                    encryptData = msEncrypt.ToArray();
                }
            }
        }
        return encryptData;
    }

    private byte[] encryptDES(string dataPath, byte[] key, byte[] IV)
    {
        FileStream data = File.OpenRead(dataPath);
        byte[] encryptData = Array.Empty<byte>();
        using (DES myDes = DES.Create())
        {
            if (key.Length == 8)                                                                                                                    
            {
                myDes.Key = key;
                MessageBox.Show("The custom key will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);                       
            }
            if (key.Length != 8 & key.Length != 0)
                MessageBox.Show("Invalid length of key. The standart key will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (IV.Length == 8)                                                                                                                     
            {
                myDes.IV = IV;
                MessageBox.Show("The custome IV will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (IV.Length != 8 & IV.Length != 0)
                MessageBox.Show("Invalid length of IV. The standart IV will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            keyB = myDes.Key;
            IVB = myDes.IV;
            ICryptoTransform encryptor = myDes.CreateEncryptor(myDes.Key, myDes.IV);
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(data);
                    }
                    encryptData = msEncrypt.ToArray();
                }
            }

        }
        return encryptData;
    }

    private byte[] encryptRC2(string dataPath, byte[] key, byte[] IV)
    {
        FileStream data = File.OpenRead(dataPath);
        byte[] encryptData = Array.Empty<byte>();
        using (RC2 myRC2 = RC2.Create())
        {
            if (key.Length == 16)
            {
                myRC2.Key = key;
                MessageBox.Show("The custom key will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (key.Length != 16 & key.Length != 0)
                MessageBox.Show("Invalid length of key. The standart key will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (IV.Length == 8)
            {
                myRC2.IV = IV;
                MessageBox.Show("The custome IV will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (IV.Length != 8 & IV.Length != 0)
                MessageBox.Show("Invalid length of IV. The standart IV will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            keyB = myRC2.Key;
            IVB = myRC2.IV;
            ICryptoTransform encryptor = myRC2.CreateEncryptor(myRC2.Key, myRC2.IV);
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(data);
                    }
                    encryptData = msEncrypt.ToArray();
                }
            }

        }
        return encryptData;
    }

    private byte[] encryptRijndael(string dataPath, byte[] key, byte[] IV)
    {
        FileStream data = File.OpenRead(dataPath);
        byte[] encryptData = Array.Empty<byte>();
        using (Rijndael myRij = Rijndael.Create())
        {
            if (key.Length == 32)
            {
                myRij.Key = key;
                MessageBox.Show("The custom key will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (key.Length != 32 & key.Length != 0)
                MessageBox.Show("Invalid length of key. The standart key will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (IV.Length == 16)
            {
                myRij.IV = IV;
                MessageBox.Show("The custome IV will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (IV.Length != 16 & IV.Length != 0)
                MessageBox.Show("Invalid length of IV. The standart IV will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            keyB = myRij.Key;
            IVB = myRij.IV;
            ICryptoTransform encryptor = myRij.CreateEncryptor(myRij.Key, myRij.IV);
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(data);
                    }
                    encryptData = msEncrypt.ToArray();
                }
            }

        }
        return encryptData;
    }

    private byte[] encryptTripleDes(string dataPath, byte[] key, byte[] IV)
    {
        FileStream data = File.OpenRead(dataPath);
        byte[] encryptData = Array.Empty<byte>();
        using (TripleDES myTripD = TripleDES.Create())
        {
            if (key.Length == 24)
            {
                myTripD.Key = key;
                MessageBox.Show("The custom key will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (key.Length != 24 & key.Length != 0)
                MessageBox.Show("Invalid length of key. The standart key will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (IV.Length == 8)
            {
                myTripD.IV = IV;
                MessageBox.Show("The custome IV will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (IV.Length != 8 & IV.Length != 0)
                MessageBox.Show("Invalid length of IV. The standart IV will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            keyB = myTripD.Key;
            IVB = myTripD.IV;
            ICryptoTransform encryptor = myTripD.CreateEncryptor(myTripD.Key, myTripD.IV);
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(data);
                    }
                    encryptData = msEncrypt.ToArray();
                }
            }

        }
        return encryptData;
    }
}
