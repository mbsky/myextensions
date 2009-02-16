using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Xml;

namespace System
{
    public class CryptoHelper
    {

        protected RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider();


        public CryptoHelper()
        {
            //mojoEncryptionConfiguration config = mojoEncryptionConfiguration.GetConfig();
            //rsaProvider.FromXmlString(config.RSAKey);
        }

        public String RSAPublicKey
        {
            get { return rsaProvider.ToXmlString(false); }
        }

        public String RSAPrivateKey
        {
            get { return rsaProvider.ToXmlString(true); }
        }

        public string Encrypt(string clearTextString)
        {

            byte[] EncryptedStr;
            EncryptedStr = rsaProvider.Encrypt(Encoding.ASCII.GetBytes(clearTextString), false);
            StringBuilder stringBuilder = new System.Text.StringBuilder();
            for (int i = 0; i <= EncryptedStr.Length - 1; i++)
            {
                if (i != EncryptedStr.Length - 1)
                {
                    stringBuilder.Append(EncryptedStr[i] + "~");
                }
                else
                {
                    stringBuilder.Append(EncryptedStr[i]);
                }
            }
            return stringBuilder.ToString();

        }

        public string Decrypt(string encryptedString)
        {

            byte[] DecryptedStr = rsaProvider.Decrypt(StringToByteArray(encryptedString.Trim()), false);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i <= DecryptedStr.Length - 1; i++)
            {
                stringBuilder.Append(Convert.ToChar(DecryptedStr[i]));
            }
            return stringBuilder.ToString();

        }

        public byte[] StringToByteArray(string inputString)
        {
            string[] s;
            s = inputString.Trim().Split('~');
            byte[] b = new byte[s.Length];

            for (int i = 0; i <= s.Length - 1; i++)
            {
                b[i] = Convert.ToByte(s[i]);
            }
            return b;
        }


        public static string Hash(string cleanString)
        {
            if (cleanString != null)
            {
                Byte[] clearBytes = new UnicodeEncoding().GetBytes(cleanString);

                Byte[] hashedBytes
                    = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);

                return BitConverter.ToString(hashedBytes);
            }
            else
            {
                return String.Empty;
            }

        }

        //

        // TODO: move to config, should not be hard coded
        private static byte[] key_192 = new byte[] 
			{10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10,
				10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10};

        private static byte[] iv_128 = new byte[]
			{10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10,
				10, 10, 10, 10};

        public static string EncryptRijndaelManaged(string value)
        {
            if (value == string.Empty) return string.Empty;

            RijndaelManaged crypto = new RijndaelManaged();
            MemoryStream memoryStream = new MemoryStream();

            CryptoStream cryptoStream = new CryptoStream(
                memoryStream,
                crypto.CreateEncryptor(key_192, iv_128),
                CryptoStreamMode.Write);

            StreamWriter streamWriter = new StreamWriter(cryptoStream);

            streamWriter.Write(value);
            streamWriter.Flush();
            cryptoStream.FlushFinalBlock();
            memoryStream.Flush();

            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }

        public static string DecryptRijndaelManaged(string value)
        {
            if (value == string.Empty) return string.Empty;

            RijndaelManaged crypto = new RijndaelManaged();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(value));

            CryptoStream cryptoStream = new CryptoStream(
                memoryStream,
                crypto.CreateDecryptor(key_192, iv_128),
                CryptoStreamMode.Read);

            StreamReader streamReader = new StreamReader(cryptoStream);

            return streamReader.ReadToEnd();
        }

        public string SignAndSecureData(string value)
        {
            return SignAndSecureData(new string[] { value });
        }



        public string SignAndSecureData(string[] values)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<x></x>");

            for (int i = 0; i < values.Length; i++)
            {
                AddNode(xmlDoc, "v" + i.ToString(), values[i]);
            }


            byte[] signature = rsaProvider.SignData(Encoding.ASCII.GetBytes(xmlDoc.InnerXml),
                "SHA1");

            AddNode(xmlDoc, "s", Convert.ToBase64String(signature, 0, signature.Length));
            return EncryptRijndaelManaged(xmlDoc.InnerXml);
        }


        public bool DecryptAndVerifyData(string input, out string[] values)
        {
            string xml = DecryptRijndaelManaged(input);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            values = null;

            XmlNode node = xmlDoc.GetElementsByTagName("s")[0];
            node.ParentNode.RemoveChild(node);

            byte[] signature = Convert.FromBase64String(node.InnerText);

            byte[] data = Encoding.ASCII.GetBytes(xmlDoc.InnerXml);
            if (!rsaProvider.VerifyData(data, "SHA1", signature))
                return false;

            int count;
            for (count = 0; count < 100; count++)
            {
                if (xmlDoc.GetElementsByTagName("v" + count.ToString())[0] == null)
                    break;
            }

            values = new string[count];

            for (int i = 0; i < count; i++)
                values[i] = xmlDoc.GetElementsByTagName("v" + i.ToString())[0].InnerText;

            return true;
        }

        private static void AddNode(XmlDocument xmlDoc, string name, string content)
        {
            XmlElement elem = xmlDoc.CreateElement(name);
            XmlText text = xmlDoc.CreateTextNode(content);
            xmlDoc.DocumentElement.AppendChild(elem);
            xmlDoc.DocumentElement.LastChild.AppendChild(text);
        }

    }

}
