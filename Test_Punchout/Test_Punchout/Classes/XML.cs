using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using Test_Punchout.Classes;

namespace Test_Punchout.Classes
{
    public static class XML
    {
        public static XmlDocument CreatePosrXml(PunchoutSetup setup, string item)
        {
            XmlDocument XML = new XmlDocument();
            XML.XmlResolver = null;

            XmlDeclaration xmlDeclaration = XML.CreateXmlDeclaration("1.0", "UTF-8", null);
            XML.AppendChild(xmlDeclaration);

            XmlDocumentType documentType = XML.CreateDocumentType("cXML", null, "http://xml.cxml.org/current/cXML.dtd", null);
            XML.AppendChild(documentType);

            XmlElement cxml = XML.CreateElement("cXML");
            cxml.SetAttribute("version", "1.0");
            cxml.SetAttribute("payloadID", Constants.TimeStamp(DateTime.Now).Replace(":", "").Replace("-", "") + "@govsci.com");
            cxml.SetAttribute("xml:lang", "en-US");
            cxml.SetAttribute("timestamp", Constants.cXMLTimeStamp(DateTime.Now));
            XML.AppendChild(cxml);

            XML.SelectSingleNode("cXML").AppendChild(CreateHeader(XML, setup));
            XML.SelectSingleNode("cXML").AppendChild(CreatePOSRBody(XML, setup, item));

            return XML;
        }

        private static XmlElement CreateHeader(XmlDocument xml, PunchoutSetup setup)
        {
            XmlElement header = xml.CreateElement("Header");

            //From
            XmlElement from = xml.CreateElement("From");
            XmlElement fcred = xml.CreateElement("Credential");
            fcred.SetAttribute("domain", "NetworkID");
            XmlElement fid = xml.CreateElement("Identity");
            fid.AppendChild(xml.CreateTextNode(setup.CustomerID));
            fcred.AppendChild(fid);
            from.AppendChild(fcred);
            header.AppendChild(from);

            //To
            XmlElement to = xml.CreateElement("To");
            XmlElement tcred = xml.CreateElement("Credential");
            tcred.SetAttribute("domain", setup.DUNS);
            XmlElement tid = xml.CreateElement("Identity");
            tid.AppendChild(xml.CreateTextNode("785807611"));
            tcred.AppendChild(tid);
            to.AppendChild(tcred);
            header.AppendChild(to);

            //Sender
            XmlElement sender = xml.CreateElement("Sender");
            XmlElement scred = xml.CreateElement("Credential");
            scred.SetAttribute("domain", "NetworkID");
            XmlElement sid = xml.CreateElement("Identity");
            sid.AppendChild(xml.CreateTextNode(setup.CustomerID));
            scred.AppendChild(sid);
            XmlElement ss = xml.CreateElement("SharedSecret");
            ss.AppendChild(xml.CreateTextNode(setup.SharedSecret));
            scred.AppendChild(ss);
            sender.AppendChild(scred);
            XmlElement ua = xml.CreateElement("UserAgent");
            ua.AppendChild(xml.CreateTextNode("GSS Test Punchout"));
            sender.AppendChild(ua);
            header.AppendChild(sender);

            return header;
        }

        private static XmlElement CreatePOSRBody(XmlDocument xml, PunchoutSetup setup, string item)
        {
            XmlElement request = xml.CreateElement("Request");
            request.SetAttribute("deploymentMode", setup.DeploymentMode);

            XmlElement setupRequest = xml.CreateElement("PunchOutSetupRequest");
            setupRequest.SetAttribute("operation", "create");

            XmlElement buyerCookie = xml.CreateElement("BuyerCookie");
            buyerCookie.AppendChild(xml.CreateTextNode(Constants.TimeStamp(DateTime.Now)));
            setupRequest.AppendChild(buyerCookie);

            XmlElement extrinsic1 = xml.CreateElement("Extrinsic");
            extrinsic1.SetAttribute("name", "UserEmail");
            extrinsic1.AppendChild(xml.CreateTextNode("test_" + setup.ID + "@govsci.com"));
            setupRequest.AppendChild(extrinsic1);

            XmlElement extrinsic2 = xml.CreateElement("Extrinsic");
            extrinsic2.SetAttribute("name", "UserId");
            extrinsic2.AppendChild(xml.CreateTextNode("test_" + setup.ID + "@govsci.com"));
            setupRequest.AppendChild(extrinsic2);

            XmlElement extrinsic3 = xml.CreateElement("Extrinsic");
            extrinsic3.SetAttribute("name", "UniqueName");
            extrinsic3.AppendChild(xml.CreateTextNode("test_" + setup.ID));
            setupRequest.AppendChild(extrinsic3);

            XmlElement userExtrinsic = xml.CreateElement("Extrinsic");
            userExtrinsic.SetAttribute("name", "User");
            userExtrinsic.AppendChild(xml.CreateTextNode("test_" + setup.ID));
            setupRequest.AppendChild(userExtrinsic);

            XmlElement browserPost = xml.CreateElement("BrowserFormPost");
            XmlElement browserUrl = xml.CreateElement("URL");
            browserUrl.AppendChild(xml.CreateTextNode(setup.BrowserFormPost));
            browserPost.AppendChild(browserUrl);
            setupRequest.AppendChild(browserPost);

            XmlElement contact = xml.CreateElement("Contact");
            XmlElement name = xml.CreateElement("Name");
            name.AppendChild(xml.CreateTextNode("test_" + setup.ID));
            XmlElement email = xml.CreateElement("Email");
            email.AppendChild(xml.CreateTextNode($"test_{setup.ID}@govsci.com"));
            contact.AppendChild(name);
            contact.AppendChild(email);
            setupRequest.AppendChild(contact);

            XmlElement supplierSetup = xml.CreateElement("SupplierSetup");
            XmlElement supplierUrl = xml.CreateElement("URL");
            supplierUrl.AppendChild(xml.CreateTextNode(setup.SupplierURL));
            supplierSetup.AppendChild(supplierUrl);
            setupRequest.AppendChild(supplierSetup);

            if (item.Length > 0)
            {
                string[] itemsplit = item.Split('|');

                XmlElement selectedItem = xml.CreateElement("SelectedItem");
                XmlElement itemId = xml.CreateElement("ItemID");

                XmlElement partID = xml.CreateElement("SupplierPartID");
                partID.AppendChild(xml.CreateTextNode(itemsplit[1]));

                XmlElement partAuxId = xml.CreateElement("ManufacturerName");
                partAuxId.AppendChild(xml.CreateTextNode(itemsplit[0]));

                itemId.AppendChild(partID);
                itemId.AppendChild(partAuxId);
                selectedItem.AppendChild(itemId);
                setupRequest.AppendChild(selectedItem);
            }

            request.AppendChild(setupRequest);

            return request;
        }

        public static XmlDocument CreatePoXml(PunchoutSetup setup, List<ItemIn> items)
        {
            XmlDocument xml = new XmlDocument();
            xml.XmlResolver = null;

            XmlDeclaration xmlDeclaration = xml.CreateXmlDeclaration("1.0", "UTF-8", null);
            xml.AppendChild(xmlDeclaration);

            XmlDocumentType documentType = xml.CreateDocumentType("cXML", null, "http://xml.cxml.org/current/cXML.dtd", null);
            xml.AppendChild(documentType);

            XmlElement cxml = xml.CreateElement("cXML");
            cxml.SetAttribute("version", "1.0");
            cxml.SetAttribute("payloadID", Constants.TimeStamp(DateTime.Now).Replace(":", "").Replace("-", "") + "@govsci.com");
            cxml.SetAttribute("xml:lang", "en-US");
            cxml.SetAttribute("timestamp", Constants.cXMLTimeStamp(DateTime.Now));
            xml.AppendChild(cxml);

            xml.SelectSingleNode("cXML").AppendChild(CreateHeader(xml, setup));
            xml.SelectSingleNode("cXML").AppendChild(CreatePoBody(xml, setup, items));

            return xml;
        }

        private static XmlElement CreatePoBody(XmlDocument xml, PunchoutSetup setup, List<ItemIn> items)
        {
            XmlElement request = xml.CreateElement("Request");
            request.SetAttribute("deploymentMode", "test");

            XmlElement orderRequest = xml.CreateElement("OrderRequest");

            XmlElement orderRequestHeader = xml.CreateElement("OrderRequestHeader");
            orderRequestHeader.SetAttribute("orderID", DateTime.Now.ToString("yyyyMMddhhmmss"));
            orderRequestHeader.SetAttribute("orderDate", DateTime.Now.ToShortDateString());
            orderRequestHeader.SetAttribute("type", "new");
            orderRequestHeader.SetAttribute("requisitionID", setup.ID.ToString() + "_" + DateTime.Now.ToString("yyyyMMddhhmmss"));

            XmlElement total = xml.CreateElement("Total");
            XmlElement totalMoney = xml.CreateElement("Money");
            totalMoney.SetAttribute("currency", "USD");
            totalMoney.AppendChild(xml.CreateTextNode(items.Sum(s => s.Quantity * s.UnitPrice).ToString("C")));
            total.AppendChild(totalMoney);
            orderRequestHeader.AppendChild(total);

            orderRequestHeader.AppendChild(CreateAddress(xml, "ShipTo"));
            orderRequestHeader.AppendChild(CreateAddress(xml, "BillTo"));

            XmlElement comments = xml.CreateElement("Comments");
            comments.AppendChild(xml.CreateTextNode("This is a test PO from TestPunchout Webform"));
            orderRequestHeader.AppendChild(comments);
            orderRequest.AppendChild(orderRequestHeader);

            for (int i = 0; i < items.Count; i++)
                orderRequest.AppendChild(CreateItemOut(xml, items[i], i + 1));

            request.AppendChild(orderRequest);

            return request;
        }

        private static XmlElement CreateAddress(XmlDocument xml, string addressType)
        {
            XmlElement shipTo = xml.CreateElement(addressType);

            XmlElement address = xml.CreateElement("Address");
            address.SetAttribute("addressID", "GSSWH");

            XmlElement addressName = xml.CreateElement("Name");
            addressName.AppendChild(xml.CreateTextNode("Government Scientific Source, Inc."));
            address.AppendChild(addressName);

            XmlElement postalAddress = xml.CreateElement("PostalAddress");

            XmlElement deliverTo = xml.CreateElement("DeliverTo");
            deliverTo.AppendChild(xml.CreateTextNode("Test Punchout"));
            postalAddress.AppendChild(deliverTo);

            XmlElement street = xml.CreateElement("Street");
            street.AppendChild(xml.CreateTextNode("12351 Sunrise Valley Drive"));
            postalAddress.AppendChild(street);

            XmlElement city = xml.CreateElement("City");
            city.AppendChild(xml.CreateTextNode("Reston"));
            postalAddress.AppendChild(city);

            XmlElement state = xml.CreateElement("State");
            state.AppendChild(xml.CreateTextNode("VA"));
            postalAddress.AppendChild(state);

            XmlElement postalCode = xml.CreateElement("PostalCode");
            postalCode.AppendChild(xml.CreateTextNode("20191"));
            postalAddress.AppendChild(postalCode);

            XmlElement country = xml.CreateElement("Country");
            country.SetAttribute("isoCountryCode", "US");
            country.AppendChild(xml.CreateTextNode("USA"));
            postalAddress.AppendChild(country);

            address.AppendChild(postalAddress);

            XmlElement email = xml.CreateElement("Email");
            email.AppendChild(xml.CreateTextNode("gss-it-development@govsci.com"));
            address.AppendChild(email);

            XmlElement phone = xml.CreateElement("Phone");
            XmlElement telephoneNumber = xml.CreateElement("TelephoneNumber");

            XmlElement countryCode = xml.CreateElement("CountryCode");
            countryCode.SetAttribute("isoCountryCode", "US");
            telephoneNumber.AppendChild(countryCode);

            XmlElement areaOrCityCode = xml.CreateElement("AreaOrCityCode");
            areaOrCityCode.AppendChild(xml.CreateTextNode("703"));
            telephoneNumber.AppendChild(areaOrCityCode);

            XmlElement number = xml.CreateElement("Number");
            number.AppendChild(xml.CreateTextNode("880-n/a"));
            telephoneNumber.AppendChild(number);

            phone.AppendChild(telephoneNumber);
            address.AppendChild(phone);
            shipTo.AppendChild(address);

            return shipTo;
        }
        private static XmlElement CreateItemOut(XmlDocument xml, ItemIn item, int lineNumber)
        {
            XmlElement itemOut = xml.CreateElement("ItemOut");
            itemOut.SetAttribute("quantity", item.Quantity.ToString("G29"));
            itemOut.SetAttribute("lineNumber", lineNumber.ToString());
            itemOut.SetAttribute("requestedDeliveryDate", DateTime.Now.AddDays(14).ToShortDateString());

            XmlElement itemId = xml.CreateElement("ItemID");

            XmlElement supplierPartId = xml.CreateElement("SupplierPartID");
            supplierPartId.AppendChild(xml.CreateTextNode(item.SupplierPartId));
            itemId.AppendChild(supplierPartId);

            XmlElement supplierPartAuxId = xml.CreateElement("SupplierPartAuxiliaryID");
            supplierPartAuxId.AppendChild(xml.CreateTextNode(item.SupplierPartAuxId));
            itemId.AppendChild(supplierPartAuxId);
            itemOut.AppendChild(itemId);

            XmlElement itemDetail = xml.CreateElement("ItemDetail");

            XmlElement unitPrice = xml.CreateElement("UnitPrice");
            XmlElement money = xml.CreateElement("Money");
            money.SetAttribute("currency", "USD");
            money.AppendChild(xml.CreateTextNode(item.UnitPrice.ToString("G29")));
            unitPrice.AppendChild(money);
            itemDetail.AppendChild(unitPrice);

            XmlElement description = xml.CreateElement("Description");
            description.AppendChild(xml.CreateTextNode(item.Description));
            itemDetail.AppendChild(description);

            XmlElement uom = xml.CreateElement("UnitOfMeasure");
            uom.AppendChild(xml.CreateTextNode(item.UnitOfMeasure));
            itemDetail.AppendChild(uom);

            string[] classifs = item.Classification.Split(new string[] { ": " }, StringSplitOptions.None);
            XmlElement classification = xml.CreateElement("Classification");
            classification.SetAttribute("domain", classifs[0]);
            classification.AppendChild(xml.CreateTextNode(classifs[1]));
            itemDetail.AppendChild(classification);

            XmlElement manufacturerPartId = xml.CreateElement("ManufacturerPartID");
            manufacturerPartId.AppendChild(xml.CreateTextNode(item.ManufacturerPartId));
            itemDetail.AppendChild(manufacturerPartId);

            XmlElement maufacturerName = xml.CreateElement("ManufacturerName");
            maufacturerName.AppendChild(xml.CreateTextNode(item.ManufacturerName));
            itemDetail.AppendChild(maufacturerName);

            if (item.Extrinsics.Length > 0)
            {
                string[] extrs = item.Extrinsics.Split(new string[] { "<br>" }, StringSplitOptions.None);
                if (extrs.Length > 0)
                {
                    for (int i = 0; i < extrs.Length; i++)
                    {
                        string[] details = extrs[i].Split(new string[] { ": " }, StringSplitOptions.None);
                        XmlElement extrinsic = xml.CreateElement("Extrinsic");
                        extrinsic.SetAttribute("name", details[0]);
                        extrinsic.AppendChild(xml.CreateTextNode(details[1]));
                        itemDetail.AppendChild(extrinsic);
                    }
                }
            }

            itemOut.AppendChild(itemDetail);

            return itemOut;
        }

        public static PunchoutResponse ReadPosrResponse(XmlDocument xml)
        {
            PunchoutResponse response = null;

            response = new PunchoutResponse(GetString(xml.SelectSingleNode("//Response/Status/@code"))
                , GetString(xml.SelectSingleNode("//Response/Status/@text"))
                , GetString(xml.SelectSingleNode("//Response/PunchOutSetupResponse/StartPage/URL")).Replace("<![CDATA[", "").Replace("]]>", "")
                , GetString(xml.SelectSingleNode("//Response/Status"))
                , xml.InnerXml);

            return response;
        }

        public static PunchoutResponse ReadPoResponse(XmlDocument xml)
        {
            PunchoutResponse response = null;

            response = new PunchoutResponse(
                GetString(xml.SelectSingleNode("//Response/Status/@code"))
                , GetString(xml.SelectSingleNode("//Response/Status/@text"))
                , ""
                , GetString(xml.SelectSingleNode("//Response/Status"))
                , xml.InnerXml);

            return response;
        }

        public static List<ItemIn> ReadPoOrderMessage(XmlDocument xml)
        {
            List<ItemIn> items = new List<ItemIn>();

            if (xml.SelectNodes("//ItemIn") != null)
            {
                foreach(XmlNode itemIn in xml.SelectNodes("//ItemIn"))
                {
                    string extrs = "";
                    XmlNodeList extrinsics = itemIn.SelectNodes("ItemDetail/Extrinsic");
                    if (extrinsics != null)
                        foreach (XmlNode extr in extrinsics)
                            extrs += GetString(extr.SelectSingleNode("@name")) + ": " + GetString(extr) + "<br>";

                    items.Add(new ItemIn(
                        GetDecimal(itemIn.SelectSingleNode("@quantity"))
                        , GetString(itemIn.SelectSingleNode("ItemID/SupplierPartID"))
                        , GetString(itemIn.SelectSingleNode("ItemID/SupplierPartAuxiliaryID"))
                        , GetDecimal(itemIn.SelectSingleNode("ItemDetail/UnitPrice/Money"))
                        , GetString(itemIn.SelectSingleNode("ItemDetail/Description"))
                        , GetString(itemIn.SelectSingleNode("ItemDetail/UnitOfMeasure"))
                        , GetString(itemIn.SelectSingleNode("ItemDetail/Classification/@domain")) + ": " + GetString(itemIn.SelectSingleNode("ItemDetail/Classification"))
                        , GetString(itemIn.SelectSingleNode("ItemDetail/ManufacturerPartID"))
                        , GetString(itemIn.SelectSingleNode("ItemDetail/ManufacturerName "))
                        , (extrs.Length > 0 ? extrs.Remove(extrs.LastIndexOf("<br>")) : "")
                        ));
                }
            }

            return items;
        }

        private static string GetString(XmlNode node)
        {
            try
            {
                return node.InnerXml;
            }
            catch
            {
                return "";
            }
        }

        private static decimal GetDecimal(XmlNode node)
        {
            try
            {
                return decimal.Parse(node.InnerXml);
            }
            catch
            {
                return 0.00M;
            }
        }

    }
}