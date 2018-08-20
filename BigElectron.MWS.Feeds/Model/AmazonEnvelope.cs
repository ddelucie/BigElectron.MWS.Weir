using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using BigElectron.MWS.Feeds.Feed;

namespace BigElectron.MWS.Feeds.Model
{
	[XmlRoot(ElementName = "Header")]
	public class Header
	{
		[XmlElement(ElementName = "DocumentVersion")]
		public string DocumentVersion { get; set; }
		[XmlElement(ElementName = "MerchantIdentifier")]
		public string MerchantIdentifier { get; set; }
	}

	[XmlRoot(ElementName = "ProcessingSummary")]
	public class ProcessingSummary
	{
		[XmlElement(ElementName = "MessagesProcessed")]
		public string MessagesProcessed { get; set; }
		[XmlElement(ElementName = "MessagesSuccessful")]
		public string MessagesSuccessful { get; set; }
		[XmlElement(ElementName = "MessagesWithError")]
		public string MessagesWithError { get; set; }
		[XmlElement(ElementName = "MessagesWithWarning")]
		public string MessagesWithWarning { get; set; }
	}

	[XmlRoot(ElementName = "Result")]
	public class Result
	{
		[XmlElement(ElementName = "MessageID")]
		public string MessageID { get; set; }
		[XmlElement(ElementName = "ResultCode")]
		public string ResultCode { get; set; }
		[XmlElement(ElementName = "ResultMessageCode")]
		public string ResultMessageCode { get; set; }
		[XmlElement(ElementName = "ResultDescription")]
		public string ResultDescription { get; set; }
	}

	[XmlRoot(ElementName = "ProcessingReport")]
	public class ProcessingReport
	{
		[XmlElement(ElementName = "DocumentTransactionID")]
		public string DocumentTransactionID { get; set; }
		[XmlElement(ElementName = "StatusCode")]
		public string StatusCode { get; set; }
		[XmlElement(ElementName = "ProcessingSummary")]
		public ProcessingSummary ProcessingSummary { get; set; }
		[XmlElement(ElementName = "Result")]
		public Result Result { get; set; }
	}

	[XmlRoot(ElementName = "Message")]
	public class Message
	{
		[XmlElement(ElementName = "MessageID")]
		public string MessageID { get; set; }
		[XmlElement(ElementName = "ProcessingReport")]
		public ProcessingReport ProcessingReport { get; set; }
		public Price Price { get; set; }

	}

	[XmlRoot(ElementName = "AmazonEnvelope")]
	public class AmazonEnvelope
	{
		[XmlElement(ElementName = "Header")]
		public Header Header { get; set; }
		[XmlElement(ElementName = "MessageType")]
		public string MessageType { get; set; }
		//[XmlElement(ElementName = "Message")]
		//public Message Message { get; set; }
		[XmlElement(ElementName = "Message")]
		public List<Message> Message { get; set; }


		[XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Xsi { get; set; }
		[XmlAttribute(AttributeName = "noNamespaceSchemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string NoNamespaceSchemaLocation { get; set; }
	}



}
