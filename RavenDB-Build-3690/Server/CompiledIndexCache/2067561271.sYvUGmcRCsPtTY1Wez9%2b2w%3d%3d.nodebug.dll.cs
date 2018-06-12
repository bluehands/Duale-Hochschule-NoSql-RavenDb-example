using Raven.Abstractions;
using Raven.Database.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System;
using Raven.Database.Linq.PrivateExtensions;
using Lucene.Net.Documents;
using System.Globalization;
using System.Text.RegularExpressions;
using Raven.Database.Indexing;

public class Index_Auto_Orders_ByCustomer_Name : Raven.Database.Linq.AbstractViewGenerator
{
	public Index_Auto_Orders_ByCustomer_Name()
	{
		this.ViewText = @"from doc in docs.Orders
select new {
	Customer_Name = doc.Customer.Name
}";
		this.ForEntityNames.Add("Orders");
		this.AddMapDefinition(docs => 
			from doc in ((IEnumerable<dynamic>)docs)
			where string.Equals(doc["@metadata"]["Raven-Entity-Name"], "Orders", System.StringComparison.InvariantCultureIgnoreCase)
			select new {
				Customer_Name = doc.Customer.Name,
				__document_id = doc.__document_id
			});
		this.AddField("Customer_Name");
		this.AddField("__document_id");
		this.AddQueryParameterForMap("Customer.Name");
		this.AddQueryParameterForMap("__document_id");
		this.AddQueryParameterForReduce("Customer.Name");
		this.AddQueryParameterForReduce("__document_id");
	}
}
