using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BigElectron.MWS.Feeds.Model;

namespace BigElectron.MWS.Feeds.Feed
{
	public static class PriceFeedBuilder
	{
		public static AmazonEnvelope Build()
		{

			var amazonEnvelope = new AmazonEnvelope();
			amazonEnvelope.Header = new Header();
			amazonEnvelope.Header.DocumentVersion = "1.01";
			amazonEnvelope.Message = new List<Message>();
			amazonEnvelope.MessageType = "Price";

			return amazonEnvelope;
		}


		public static Message BuildMessage()
		{
			Message message = new Message();
			message.Price = new Price();
			message.Price.StandardPrice = new OverrideCurrencyAmount() { currency = BaseCurrencyCodeWithDefault.USD };
			return message;
		}
	}


}
