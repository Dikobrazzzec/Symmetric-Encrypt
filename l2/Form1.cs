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
            textBox3.Text = starter.output(starter.algElect(comboBox1.SelectedIndex, comboBox2.SelectedIndex, Encoding.UTF8.GetBytes(textBox1.Text), Encoding.UTF8.GetBytes(textBox2.Text)));
            textBox1.Text = starter.output(starter.keyB);
            textBox2.Text = starter.output(starter.IVB);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label5.Text = Convert.ToString(textBox1.Text.Length );
            label8.Text = Convert.ToString(textBox1.Text.Length*8/2);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            label6.Text = Convert.ToString(textBox2.Text.Length);
            label7.Text = Convert.ToString(textBox2.Text.Length * 8 / 2);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            label11.Text = Convert.ToString(textBox3.Text.Length);
            label1.Text = Convert.ToString(textBox3.Text.Length * 8 / 2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            symmetricCrypt starter = new symmetricCrypt();
            textBox3.Text = starter.output(starter.testEncr(textBox3.Text, Encoding.UTF8.GetBytes(textBox1.Text), Encoding.UTF8.GetBytes(textBox2.Text)));
            textBox1.Text = starter.output(starter.keyB);
            textBox2.Text = starter.output(starter.IVB);
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

    public string output(byte[] byteArr)                                                    //разобраться с длиной вывода
    {
        StringBuilder outString = new StringBuilder();
        foreach (var i in byteArr)
            outString.Append(i.ToString("x2"));
        return outString.ToString();
     //   return BitConverter.ToString(byteArr);
        // return Encoding.UTF8.GetString(byteArr);
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

    ///////////////////////////DECRYPT///////////////////////////

    private string decryptAES (string dataPath, byte[] key, byte[] IV)
    {
        byte [] data = File.ReadAllBytes(dataPath);
        //byte[] byteData = Encoding.UTF8.GetBytes(data);
        //byte[] encryptData = Array.Empty<byte>();
        string decryptdata = String.Empty;
        using (Aes myAes = Aes.Create())
        {
            if (key.Length == 16)
            {
                myAes.Key = key;
                MessageBox.Show("The custom key will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (key.Length != 16 & key.Length != 0)
                MessageBox.Show("Invalid length of key. Random key will be used .", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (IV.Length == 16)
            {
                myAes.IV = IV;
                MessageBox.Show("The custome IV will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (IV.Length != 16 & IV.Length != 0)
                MessageBox.Show("Invalid length of IV. Standart IV will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            keyB = myAes.Key;
            IVB = myAes.IV;
            ICryptoTransform decryptor = myAes.CreateEncryptor(myAes.Key, myAes.IV);
            using (MemoryStream msDecrypt = new MemoryStream(data))
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
        return decryptdata;
    }

    private string decryptDes(string dataPath, byte[] key, byte[] IV)
    {
        byte[] data = File.ReadAllBytes(dataPath);
        //byte[] byteData = Encoding.UTF8.GetBytes(data);
        //byte[] encryptData = Array.Empty<byte>();
        string decryptdata = String.Empty;
        using (Aes myAes = Aes.Create())
        {
            if (key.Length == 16)
            {
                myAes.Key = key;
                MessageBox.Show("The custom key will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (key.Length != 16 & key.Length != 0)
                MessageBox.Show("Invalid length of key. Random key will be used .", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (IV.Length == 16)
            {
                myAes.IV = IV;
                MessageBox.Show("The custome IV will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (IV.Length != 16 & IV.Length != 0)
                MessageBox.Show("Invalid length of IV. Standart IV will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            keyB = myAes.Key;
            IVB = myAes.IV;
            ICryptoTransform decryptor = myAes.CreateEncryptor(myAes.Key, myAes.IV);
            using (MemoryStream msDecrypt = new MemoryStream(data))
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
        return decryptdata;
    }

    private string decryptRC2(string dataPath, byte[] key, byte[] IV)
    {
        byte[] data = File.ReadAllBytes(dataPath);
        //byte[] byteData = Encoding.UTF8.GetBytes(data);
        //byte[] encryptData = Array.Empty<byte>();
        string decryptdata = String.Empty;
        using (Aes myAes = Aes.Create())
        {
            if (key.Length == 16)
            {
                myAes.Key = key;
                MessageBox.Show("The custom key will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (key.Length != 16 & key.Length != 0)
                MessageBox.Show("Invalid length of key. Random key will be used .", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (IV.Length == 16)
            {
                myAes.IV = IV;
                MessageBox.Show("The custome IV will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (IV.Length != 16 & IV.Length != 0)
                MessageBox.Show("Invalid length of IV. Standart IV will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            keyB = myAes.Key;
            IVB = myAes.IV;
            ICryptoTransform decryptor = myAes.CreateEncryptor(myAes.Key, myAes.IV);
            using (MemoryStream msDecrypt = new MemoryStream(data))
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
        return decryptdata;
    }

    private string decryptRijndael(string dataPath, byte[] key, byte[] IV)
    {
        byte[] data = File.ReadAllBytes(dataPath);
        //byte[] byteData = Encoding.UTF8.GetBytes(data);
        //byte[] encryptData = Array.Empty<byte>();
        string decryptdata = String.Empty;
        using (Aes myAes = Aes.Create())
        {
            if (key.Length == 16)
            {
                myAes.Key = key;
                MessageBox.Show("The custom key will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (key.Length != 16 & key.Length != 0)
                MessageBox.Show("Invalid length of key. Random key will be used .", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (IV.Length == 16)
            {
                myAes.IV = IV;
                MessageBox.Show("The custome IV will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (IV.Length != 16 & IV.Length != 0)
                MessageBox.Show("Invalid length of IV. Standart IV will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            keyB = myAes.Key;
            IVB = myAes.IV;
            ICryptoTransform decryptor = myAes.CreateEncryptor(myAes.Key, myAes.IV);
            using (MemoryStream msDecrypt = new MemoryStream(data))
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
        return decryptdata;
    }

    private string decryptTripleDes(string dataPath, byte[] key, byte[] IV)
    {
        byte[] data = File.ReadAllBytes(dataPath);
        //byte[] byteData = Encoding.UTF8.GetBytes(data);
        //byte[] encryptData = Array.Empty<byte>();
        string decryptdata = String.Empty;
        using (Aes myAes = Aes.Create())
        {
            if (key.Length == 16)
            {
                myAes.Key = key;
                MessageBox.Show("The custom key will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (key.Length != 16 & key.Length != 0)
                MessageBox.Show("Invalid length of key. Random key will be used .", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (IV.Length == 16)
            {
                myAes.IV = IV;
                MessageBox.Show("The custome IV will be used.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (IV.Length != 16 & IV.Length != 0)
                MessageBox.Show("Invalid length of IV. Standart IV will be used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            keyB = myAes.Key;
            IVB = myAes.IV;
            ICryptoTransform decryptor = myAes.CreateEncryptor(myAes.Key, myAes.IV);
            using (MemoryStream msDecrypt = new MemoryStream(data))
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
        return decryptdata;
    }


    public byte[] testEncr (string data, byte[] key, byte[] IV)
    {
        byte [] encryptData ;
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

}
