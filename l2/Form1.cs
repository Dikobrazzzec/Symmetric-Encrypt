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
            textBox3.Text = starter.
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label5.Text = Convert.ToString(Encoding.Unicode.GetBytes(textBox1.Text).Length);
            label8.Text = Convert.ToString(Convert.ToInt32(label5.Text) * 8);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            label6.Text = Convert.ToString(Encoding.Unicode.GetBytes(textBox2.Text).Length);
            label7.Text = Convert.ToString(Convert.ToInt32(label6.Text) * 8);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            label11.Text = Convert.ToString(Encoding.Unicode.GetBytes(textBox3.Text).Length);
            label1.Text = Convert.ToString(Convert.ToInt32(label11.Text) * 8);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //TEST
            //symmetricCrypt starter = new symmetricCrypt();
            //textBox3.Text = starter.output(starter.testEncr(textBox3.Text, Encoding.Unicode.GetBytes(textBox1.Text), Encoding.Unicode.GetBytes(textBox2.Text)));
            //textBox1.Text = starter.output(starter.keyB);
            //textBox2.Text = starter.output(starter.IVB);
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

    private int fileSafe(string data)
    {
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "txt files (*.txt)| *.txt|All files (*.*)|*.*";
        saveFileDialog.FilterIndex = 2;
        saveFileDialog.RestoreDirectory = true;

        if (saveFileDialog.ShowDialog() == DialogResult.OK)
            File.WriteAllText(saveFileDialog.FileName, data);
        return 0;
    }

    public string algElect(int combBox1Ind, int combBox2Ind, byte[] key, byte[] IV)
    { 
        string dataPath = fileElector();
        string encryptData = String.Empty;
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

    public string decralgElect (int combBox1Ind, int combBox2Ind, byte[] key, byte[] IV)
    {
        string dataPath = fileElector();
        string decryptData = String.Empty;
        if (combBox1Ind == 1)
        {
            switch (combBox2Ind)
            {
                case 0:
                    decryptData = decryptAES(dataPath, key, IV);
                    break;
                case 1:
                    decryptData = decryptDes(dataPath, key, IV);
                    break;
                case 2:
                    decryptData = decryptRC2(dataPath, key, IV);
                    break;
                case 3:
                    decryptData = decryptRijndael(dataPath, key, IV);
                    break;
                case 4:
                    decryptData = decryptTripleDes(dataPath, key, IV);
                    break;
            }
        }
        return decryptData;
    }

    ///////////////////////////ENCRYPT///////////////////////////

    private string encryptAES(string dataPath, byte[] key, byte[] IV)
    {
        byte[] data = Encoding.Unicode.GetBytes(File.ReadAllText(dataPath));
        byte[] encryptData = Array.Empty<byte>();
        using (Aes myAes = Aes.Create())
        {
            if (key.Length == 32)
            {
                myAes.Key = key;
                MessageBox.Show("The custom key will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (key.Length != 32 & key.Length != 0)
                MessageBox.Show("Invalid length of key. The random key will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (IV.Length == 16)
            {
                myAes.IV = IV;
                MessageBox.Show("The custome IV will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (IV.Length != 16 & IV.Length != 0)
                MessageBox.Show("Invalid length of IV. The random IV will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            keyB = myAes.Key;
            IVB = myAes.IV;
            ICryptoTransform encryptor = myAes.CreateEncryptor(myAes.Key, myAes.IV);
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt, Encoding.Unicode))
                    {
                        swEncrypt.Write(data);
                    }
                    encryptData = msEncrypt.ToArray();
                }
            }
        }
        return Encoding.Unicode.GetString(encryptData);
    }

    private string encryptDES(string dataPath, byte[] key, byte[] IV)
    {
        byte[] data = Encoding.Unicode.GetBytes(File.ReadAllText(dataPath));
        byte[] encryptData = Array.Empty<byte>();
        using (DES myDes = DES.Create())
        {
            if (key.Length == 8)                                                                                                                    
            {
                myDes.Key = key;
                MessageBox.Show("The custom key will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);                       
            }
            if (key.Length != 8 & key.Length != 0)
                MessageBox.Show("Invalid length of key. The random key will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (IV.Length == 8)                                                                                                                     
            {
                myDes.IV = IV;
                MessageBox.Show("The custome IV will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (IV.Length != 8 & IV.Length != 0)
                MessageBox.Show("Invalid length of IV. The random IV will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            keyB = myDes.Key;
            IVB = myDes.IV;
            ICryptoTransform encryptor = myDes.CreateEncryptor(myDes.Key, myDes.IV);
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt, Encoding.Unicode))
                    {
                        swEncrypt.Write(data);
                    }
                    encryptData = msEncrypt.ToArray();
                }
            }

        }
        return Encoding.Unicode.GetString(encryptData);
    }

    private string encryptRC2(string dataPath, byte[] key, byte[] IV)
    {
        byte[] data = Encoding.Unicode.GetBytes(File.ReadAllText(dataPath));
        byte[] encryptData = Array.Empty<byte>();
        using (RC2 myRC2 = RC2.Create())
        {
            if (key.Length == 16)
            {
                myRC2.Key = key;
                MessageBox.Show("The custom key will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (key.Length != 16 & key.Length != 0)
                MessageBox.Show("Invalid length of key. The random key will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (IV.Length == 8)
            {
                myRC2.IV = IV;
                MessageBox.Show("The custome IV will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (IV.Length != 8 & IV.Length != 0)
                MessageBox.Show("Invalid length of IV. The random IV will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            keyB = myRC2.Key;
            IVB = myRC2.IV;
            ICryptoTransform encryptor = myRC2.CreateEncryptor(myRC2.Key, myRC2.IV);
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt, Encoding.Unicode))
                    {
                        swEncrypt.Write(data);
                    }
                    encryptData = msEncrypt.ToArray();
                }
            }

        }
        return Encoding.Unicode.GetString(encryptData);
    }

    private string encryptRijndael(string dataPath, byte[] key, byte[] IV)
    {
        byte[] data = Encoding.Unicode.GetBytes(File.ReadAllText(dataPath));
        byte[] encryptData = Array.Empty<byte>();
        using (Rijndael myRij = Rijndael.Create())
        {
            if (key.Length == 32)
            {
                myRij.Key = key;
                MessageBox.Show("The custom key will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (key.Length != 32 & key.Length != 0)
                MessageBox.Show("Invalid length of key. The random key will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (IV.Length == 16)
            {
                myRij.IV = IV;
                MessageBox.Show("The custome IV will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (IV.Length != 16 & IV.Length != 0)
                MessageBox.Show("Invalid length of IV. The random IV will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            keyB = myRij.Key;
            IVB = myRij.IV;
            ICryptoTransform encryptor = myRij.CreateEncryptor(myRij.Key, myRij.IV);
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt, Encoding.Unicode))
                    {
                        swEncrypt.Write(data);
                    }
                    encryptData = msEncrypt.ToArray();
                }
            }

        }
        return Encoding.Unicode.GetString(encryptData);
    }

    private string encryptTripleDes(string dataPath, byte[] key, byte[] IV)
    {
        byte[] data = Encoding.Unicode.GetBytes(File.ReadAllText(dataPath));
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
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt, Encoding.Unicode))
                    {
                        swEncrypt.Write(data);
                    }
                    encryptData = msEncrypt.ToArray();
                }
            }

        }
        return Encoding.Unicode.GetString(encryptData);
    }

    ///////////////////////////DECRYPT///////////////////////////

    private string decryptAES (string dataPath, byte[] key, byte[] IV)
    {
        byte [] data = Encoding.Unicode.GetBytes(File.ReadAllText(dataPath));
        string decryptdata = String.Empty;
        using (Aes myAes = Aes.Create())
        {
            if (key.Length == 32)
            {
                myAes.Key = key;
                MessageBox.Show("The custom key will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (key.Length != 32 & key.Length != 0)
                MessageBox.Show("Invalid length of key. Random key will be used .", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (IV.Length == 16)
            {
                myAes.IV = IV;
                MessageBox.Show("The custome IV will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (IV.Length != 16 & IV.Length != 0)
                MessageBox.Show("Invalid length of IV. Random IV will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            keyB = myAes.Key;
            IVB = myAes.IV;
            ICryptoTransform decryptor = myAes.CreateDecryptor(myAes.Key, myAes.IV);
            using (MemoryStream msDecrypt = new MemoryStream(data))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecryptor = new StreamReader(csDecrypt, Encoding.Unicode))
                    {
                        decryptdata = srDecryptor.ReadToEnd();
                    }
                }
            }
        }
        return decryptdata;
    }

    private string decryptDes(string dataPath, byte[] key, byte[] IV)
    {
        byte[] data = Encoding.Unicode.GetBytes(File.ReadAllText(dataPath));
        string decryptdata = String.Empty;
        using (DES myDes = DES.Create())
        {
            if (key.Length == 8)
            {
                myDes.Key = key;
                MessageBox.Show("The custom key will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (key.Length != 8 & key.Length != 0)
                MessageBox.Show("Invalid length of key. Random key will be used .", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (IV.Length == 8)
            {
                myDes.IV = IV;
                MessageBox.Show("The custome IV will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (IV.Length != 8 & IV.Length != 0)
                MessageBox.Show("Invalid length of IV. Random IV will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            keyB = myDes.Key;
            IVB = myDes.IV;
            ICryptoTransform decryptor = myDes.CreateDecryptor(myDes.Key, myDes.IV);
            using (MemoryStream msDecrypt = new MemoryStream(data))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecryptor = new StreamReader(csDecrypt, Encoding.Unicode))
                    {
                        decryptdata = srDecryptor.ReadToEnd();
                    }
                }
            }
        }
        return decryptdata;
    }

    private string decryptRC2(string dataPath, byte[] key, byte[] IV)
    {
        byte[] data = Encoding.Unicode.GetBytes(File.ReadAllText(dataPath));
        string decryptdata = String.Empty;
        using (RC2 myRC2 = RC2.Create())
        {
            if (key.Length == 16)
            {
                myRC2.Key = key;
                MessageBox.Show("The custom key will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (key.Length != 16 & key.Length != 0)
                MessageBox.Show("Invalid length of key. Random key will be used .", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (IV.Length == 8)
            {
                myRC2.IV = IV;
                MessageBox.Show("The custome IV will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (IV.Length != 8 & IV.Length != 0)
                MessageBox.Show("Invalid length of IV. Random IV will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            keyB = myRC2.Key;
            IVB = myRC2.IV;
            ICryptoTransform decryptor = myRC2.CreateDecryptor(myRC2.Key, myRC2.IV);
            using (MemoryStream msDecrypt = new MemoryStream(data))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecryptor = new StreamReader(csDecrypt, Encoding.Unicode))
                    {
                        decryptdata = srDecryptor.ReadToEnd();
                    }
                }
            }
        }
        return decryptdata;
    }

    private string decryptRijndael(string dataPath, byte[] key, byte[] IV)
    {
        byte[] data = Encoding.Unicode.GetBytes(File.ReadAllText(dataPath));
        string decryptdata = String.Empty;
        using (Rijndael myRij = Rijndael.Create())
        {
            if (key.Length == 32)
            {
                myRij.Key = key;
                MessageBox.Show("The custom key will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (key.Length != 32 & key.Length != 0)
                MessageBox.Show("Invalid length of key. Random key will be used .", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (IV.Length == 16)
            {
                myRij.IV = IV;
                MessageBox.Show("The custome IV will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (IV.Length != 16 & IV.Length != 0)
                MessageBox.Show("Invalid length of IV. Random IV will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            keyB = myRij.Key;
            IVB = myRij.IV;
            ICryptoTransform decryptor = myRij.CreateEncryptor(myRij.Key, myRij.IV);
            using (MemoryStream msDecrypt = new MemoryStream(data))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecryptor = new StreamReader(csDecrypt, Encoding.Unicode))
                    {
                        decryptdata = srDecryptor.ReadToEnd();
                    }
                }
            }
        }
        return decryptdata;
    }

    private string decryptTripleDes(string dataPath, byte[] key, byte[] IV)
    {
        byte[] data = Encoding.Unicode.GetBytes(File.ReadAllText(dataPath));
        string decryptdata = String.Empty;
        using (TripleDES myTripD = TripleDES.Create())
        {
            if (key.Length == 24)
            {
                myTripD.Key = key;
                MessageBox.Show("The custom key will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (key.Length != 24 & key.Length != 0)
                MessageBox.Show("Invalid length of key. Random key will be used .", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (IV.Length == 8)
            {
                myTripD.IV = IV;
                MessageBox.Show("The custome IV will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (IV.Length != 8 & IV.Length != 0)
                MessageBox.Show("Invalid length of IV. Random IV will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            keyB = myTripD.Key;
            IVB = myTripD.IV;
            ICryptoTransform decryptor = myTripD.CreateEncryptor(myTripD.Key, myTripD.IV);
            using (MemoryStream msDecrypt = new MemoryStream(data))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecryptor = new StreamReader(csDecrypt, Encoding.Unicode))
                    {
                        decryptdata = srDecryptor.ReadToEnd();
                    }
                }
            }
        }
        return decryptdata;
    }


    public byte[] testEncr (string data, byte[] key, byte[] IV)
    {
        byte [] encryptData ;
        using (Aes myAes = Aes.Create())
        {
            //if (key.Length == 32)
            //{
            //    myAes.Key = key;
            //    MessageBox.Show("The custom key will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //if (key.Length != 16 & key.Length != 0)
            //    MessageBox.Show("Invalid length of key. The standart key will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //if (IV.Length == 16)
            //{
            //    myAes.IV = IV;
            //    MessageBox.Show("The custome IV will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //if (IV.Length != 16 & IV.Length != 0)
            //    MessageBox.Show("Invalid length of IV. The standart IV will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

}
