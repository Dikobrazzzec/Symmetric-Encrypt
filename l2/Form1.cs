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
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

class symmetricCrypt
{
    private string fileElector()                            //no problem
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

    public string output (byte[] byteArr)
    {
        StringBuilder outString = new StringBuilder();
        foreach (var i in byteArr)
            outString.Append(i.ToString("X2"));
        return outString.ToString();
    }

    public byte[] algElect(int combBox1Ind, int combBox2Ind, byte[] key, byte[] IV)
    { 
        string dataPath = fileElector();
        byte[] encryptData = Array.Empty<byte>();
        if (combBox1Ind == 0)
        {
            switch (combBox2Ind)
            {
                case 0:
                    encryptData = encryptAES(dataPath, key, IV);    //16 byte key and IV. CHECK IT!!!!
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
        if (data == null || data.Length <= 0)
            throw new ArgumentNullException("data2");
        byte[] encryptData = Array.Empty<byte>();
        using (Aes myAes = Aes.Create())
        {
            if (key.Length == 16)
            {
                myAes.Key = key;
                MessageBox.Show("The custom key will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);                       //5bytgadfgasdfge1
            }
            if (key.Length != 16 & key.Length != 0)
                MessageBox.Show("Invalid length of key. The standart key will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (IV.Length == 16)
            {
                myAes.IV = IV;
                MessageBox.Show("The custome IV will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if ( IV.Length != 16 & IV.Length != 0)
                MessageBox.Show("invalid length of IV. The standart IV will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

    private byte[] encryptDES(string data, byte[] key, byte[] IV)
    {
        if (data == null || data.Length <= 0)
            throw new ArgumentNullException("data3");
        if (key == null || key.Length <= 0)
            throw new ArgumentNullException("key3");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV3");
        byte[] encrypt = Array.Empty<byte>();
        using (DES myAes = DES.Create())
        {
           
        }
        return encrypt;
    }

    private byte[] encryptRC2(string data, byte[] key, byte[] IV)
    {
        if (data == null || data.Length <= 0)
            throw new ArgumentNullException("data3");
        if (key == null || key.Length <= 0)
            throw new ArgumentNullException("key3");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV3");
        byte[] encrypt = Array.Empty<byte>();
        using (RC2 myRC2 = RC2.Create())
        {

        }
        return encrypt;
    }

    private byte[] encryptRijndael(string data, byte[] key, byte[] IV)
    {
        if (data == null || data.Length <= 0)
            throw new ArgumentNullException("data4");
        if (key == null || key.Length <= 0)
            throw new ArgumentNullException("key4");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV4");
        byte[] encrypt = Array.Empty<byte>();
        using (Rijndael myAes = Rijndael.Create())
        {

        }
        return encrypt;
    }

    private byte[] encryptTripleDes(string data, byte[] key, byte[] IV)
    {
        if (data == null || data.Length <= 0)
            throw new ArgumentNullException("data5");
        if (key == null || key.Length <= 0)
            throw new ArgumentNullException("key5");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV5");
        byte[] encrypt = Array.Empty<byte>();
        using (TripleDES myAes = TripleDES.Create())
        {

        }
        return encrypt;
    }
}
