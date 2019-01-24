using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace LinqToXml
{
    public static class LinqToXml
    {
        /// <summary>
        /// Creates hierarchical data grouped by category
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation (refer to CreateHierarchySourceFile.xml in Resources)</param>
        /// <returns>Xml representation (refer to CreateHierarchyResultFile.xml in Resources)</returns>
        public static string CreateHierarchy(string xmlRepresentation)
        {
            XElement root = XElement.Parse(xmlRepresentation);

            var newData =
                new XElement("Root", root.Elements("Data")
                .GroupBy(data => data.Element("Category").Value)
                .Select(sortData =>
                    new XElement("Group", new XAttribute("ID", sortData.Key),
                    sortData.Select(s =>
                    new XElement("Data", new XElement(s.Element("Quantity")), new XElement(s.Element("Price")))))));

            return newData.ToString();
        }

        /// <summary>
        /// Get list of orders numbers (where shipping state is NY) from xml representation
        /// </summary>
        /// <param name="xmlRepresentation">Orders xml representation (refer to PurchaseOrdersSourceFile.xml in Resources)</param>
        /// <returns>Concatenated orders numbers</returns>
        /// <example>
        /// 99301,99189,99110
        /// </example>
        public static string GetPurchaseOrders(string xmlRepresentation)
        {
            XElement root = XElement.Parse(xmlRepresentation);

            XNamespace aw = "http://www.adventure-works.com";

            var orders = root.Elements(aw + "PurchaseOrder")
                .Where(s => s.Elements(aw + "Address")
                .Any(e => e.Attribute(aw + "Type").Value == "Shipping" && e.Element(aw + "State").Value == "NY"))
                .Select(z => z.Attribute(aw + "PurchaseOrderNumber").Value);

            return String.Join(",", orders);
        }

        /// <summary>
        /// Reads csv representation and creates appropriate xml representation
        /// </summary>
        /// <param name="customers">Csv customers representation (refer to XmlFromCsvSourceFile.csv in Resources)</param>
        /// <returns>Xml customers representation (refer to XmlFromCsvResultFile.xml in Resources)</returns>
        public static string ReadCustomersFromCsv(string customers)
        {
            string[] str = customers.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            var listCustomers =
                new XElement("Root", str.Select(s => s.Split(','))
                .Select(e =>
                new XElement("Customer",
                new XAttribute("CustomerID", e[0]),
                new XElement("CompanyName", e[1]),
                new XElement("ContactName", e[2]),
                new XElement("ContactTitle", e[3]),
                new XElement("Phone", e[4]),
                    new XElement("FullAddress",
                        new XElement("Address", e[5]),
                        new XElement("City", e[6]),
                        new XElement("Region", e[7]),
                        new XElement("PostalCode", e[8]),
                        new XElement("Country", e[9])))));
            return listCustomers.ToString();
        }

        /// <summary>
        /// Gets recursive concatenation of elements
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation of document with Sentence, Word and Punctuation elements. (refer to ConcatenationStringSource.xml in Resources)</param>
        /// <returns>Concatenation of all this element values.</returns>
        public static string GetConcatenationString(string xmlRepresentation)
        {
            XElement data = XElement.Parse(xmlRepresentation);

            return data.Value;
        }

        /// <summary>
        /// Replaces all "customer" elements with "contact" elements with the same childs
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation with customers (refer to ReplaceCustomersWithContactsSource.xml in Resources)</param>
        /// <returns>Xml representation with contacts (refer to ReplaceCustomersWithContactsResult.xml in Resources)</returns>
        public static string ReplaceAllCustomersWithContacts(string xmlRepresentation)
        {
            XElement data = XElement.Parse(xmlRepresentation);
            data.ReplaceAll(
                data.Elements().Select(x => new XElement("contact", x.Descendants()))
            );
            return data.ToString();
        }

        /// <summary>
        /// Finds all ids for channels with 2 or more subscribers and mark the "DELETE" comment
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation with channels (refer to FindAllChannelsIdsSource.xml in Resources)</param>
        /// <returns>Sequence of channels ids</returns>
        public static IEnumerable<int> FindChannelsIds(string xmlRepresentation)
        {
            XElement data = XElement.Parse(xmlRepresentation);

            var channelsIds = data.Elements("channel")
                .Where(x => x.Elements("subscriber").Count() > 1 &&
                        x.Nodes().OfType<XComment>().Any(y => y.Value == "DELETE"))
                .Select(x => (int)x.Attribute("id"));

            return channelsIds;
        }

        /// <summary>
        /// Sort customers in docement by Country and City
        /// </summary>
        /// <param name="xmlRepresentation">Customers xml representation (refer to GeneralCustomersSourceFile.xml in Resources)</param>
        /// <returns>Sorted customers representation (refer to GeneralCustomersResultFile.xml in Resources)</returns>
        public static string SortCustomers(string xmlRepresentation)
        {
            XElement data = XElement.Parse(xmlRepresentation);

            var sort =
                new XElement("Root", data.Elements("Customers")
                .OrderBy(s => s.Element("FullAddress").Element("Country").Value)
                .ThenBy(s => s.Element("FullAddress").Element("City").Value)
                .Select(s => s));

            return sort.ToString();
        }

        /// <summary>
        /// Gets XElement flatten string representation to save memory
        /// </summary>
        /// <param name="xmlRepresentation">XElement object</param>
        /// <returns>Flatten string representation</returns>
        /// <example>
        ///     <root><element>something</element></root>
        /// </example>
        public static string GetFlattenString(XElement xmlRepresentation)
        {
            return xmlRepresentation.ToString(SaveOptions.DisableFormatting);
        }

        /// <summary>
        /// Gets total value of orders by calculating products value
        /// </summary>
        /// <param name="xmlRepresentation">Orders and products xml representation (refer to GeneralOrdersFileSource.xml in Resources)</param>
        /// <returns>Total purchase value</returns>
        public static int GetOrdersValue(string xmlRepresentation)
        {
            XElement data = XElement.Parse(xmlRepresentation);

            var orders = data.Element("Orders").Elements("Order")
                .Select(x => x.Element("product").Value);

            var products = data.Element("products").Elements();

            return
                orders.Join(
                    products,
                    order => order,
                    product => product.Attribute("Id").Value,
                    (order, product) => (int)product.Attribute("Value"))
                .Sum();
        }
    }
}
