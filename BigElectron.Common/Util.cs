using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace BigElectron.Common
{
	public static class Util
	{
		public static Stream GenerateStreamFromString(string s)
		{
			var stream = new MemoryStream();
			var writer = new StreamWriter(stream);
			writer.Write(s);
			writer.Flush();
			stream.Position = 0;
			return stream;
		}

		public static DataTable StringToDataTable(string v1, string v2, List<string> list)
		{
			throw new NotImplementedException();
		}

		public static Stream GenerateStreamFromXml<T>(T item)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			var stream = new MemoryStream();
			serializer.Serialize(stream, item); stream.Position = 0;
			return stream;
		}

		public static T DeserializeXmlFromFile<T>(string filePath)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			string xmlString = File.ReadAllText(filePath);
			using (StringReader stringReader = new StringReader(xmlString))
			{
				return (T)serializer.Deserialize(stringReader);
			}
		}

		/// <summary>
		/// just a quick test.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="item"></param>
		public static void GenerateXmlFile<T>(T item)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			using (FileStream fs = new FileStream(@"sertest.xml", FileMode.Create))
			{
				serializer.Serialize(fs, item);
			}
		}

		public static string CalculateContentMD5(Stream content)
		{
			MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
			byte[] hash = provider.ComputeHash(content);
			return Convert.ToBase64String(hash);
		}

		public static string ToUpperFirst(this string source)
		{
			string first = source.Substring(0, 1);
			string result = source.Remove(0, 1).Insert(0, first.ToUpper());
			return result;
		}

		public static string GetInnerTextFromXmlDocument(XmlDocument xdoc, string tag)
		{
			if (xdoc.GetElementsByTagName(tag).Count > 0)
				return xdoc.GetElementsByTagName(tag)[0].InnerText;
			return "";
		}

		public static DateTime Truncate(this DateTime dateTime, TimeSpan timeSpan)
		{
			if (timeSpan == TimeSpan.Zero) return dateTime; // Or could throw an ArgumentException
			return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
		}

		public static void SendEmail(string subject, string message, List<string> recipients = null)
		{
			if (recipients == null) recipients = new List<string> { "lulanoski@offess.com" };

			using (SmtpClient smtpClient = new SmtpClient("mail.offess.com", 25))
			{
				smtpClient.Credentials = new System.Net.NetworkCredential("ddelucie@hotmail.com", "xxxxxxxxxx");
				//smtpClient.UseDefaultCredentials = true;
				smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
				smtpClient.EnableSsl = false;
				MailMessage mail = new MailMessage();

				//Setting From , To and CC
				mail.From = new MailAddress("ddelucie@hotmail.com", "Alert");
				foreach (var recipient in recipients)
				{
					mail.To.Add(new MailAddress(recipient));
				}
				mail.Body = message;
				mail.Subject = subject;
				smtpClient.Send(mail);
			}
		}


		public static decimal ParseDecimal(this string value, decimal defaultValue = 0.0m)
		{
			decimal result;
			if (!decimal.TryParse(value, out result))
				return defaultValue;
			return result;
		}
		public static int ParseInt(this string value, int defaultValue = 0)
		{
			int result;
			if (!int.TryParse(value, out result))
				return defaultValue;
			return result;
		}

		public static string Serialize<T>(this T value)
		{
			if (value == null)
			{
				return string.Empty;
			}
			try
			{

				var xmlserializer = new XmlSerializer(typeof(T));
				var stringWriter = new StringWriter();
				using (var writer = XmlWriter.Create(stringWriter))
				{
					xmlserializer.Serialize(writer, value);
					return stringWriter.ToString();
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Serialize error occurred", ex);
			}
		}

		public static string ToJson(this object value)
		{
			if (value == null) return "";

			try
			{
				string json = JsonConvert.SerializeObject(value);
				return json;
			}
			catch
			{
				//log exception but dont throw one
				return "";
			}
		}

		public static string StreamToString(this Stream stream)
		{
			StreamReader reader = new StreamReader(stream);
			string text = reader.ReadToEnd();
			return text;
		}



		public static DataTable StringToDataTable(string data)
		{
			DataTable dataTable = new DataTable();
			if (string.IsNullOrEmpty(data)) return dataTable;

			using (StringReader reader = new StringReader(data))
			{
				string line;
				bool isFirst = true;
				while ((line = reader.ReadLine()) != null)
				{
					if (isFirst)
					{
						foreach (string columnheader in line.Split('\t'))
						{
							dataTable.Columns.Add(columnheader);
						}


						isFirst = false;
						continue;
					}
					DataRow datarow = dataTable.NewRow();
					datarow.ItemArray = line.Split('\t');
					dataTable.Rows.Add(datarow);
				}
			}
			return dataTable;

		}

	}
}
