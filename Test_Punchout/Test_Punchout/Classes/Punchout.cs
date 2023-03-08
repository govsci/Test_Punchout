namespace Test_Punchout.Classes
{
    public class PunchoutSetup
    {
        public PunchoutSetup(int id, string customerId, string matrixName, string itemTable)
        {
            ID = id;
            CustomerID = customerId;
            MatrixName = matrixName;
            ItemTable = itemTable;
        }

        public PunchoutSetup(string deployment, string browserFormPost, string supplierUrl, int id, int parent, string customerId, string duns, string sharedSecret, string matrixName)
        {
            DeploymentMode = deployment;
            ID = id;
            Parent = parent;
            CustomerID = customerId;
            DUNS = duns;
            SharedSecret = sharedSecret;
            MatrixName = matrixName;
            BrowserFormPost = browserFormPost;
            SupplierURL = supplierUrl;
            ItemTable = "";
        }

        public int ID { get; }
        public int Parent { get; }
        public string CustomerID { get; }
        public string DUNS { get; }
        public string SharedSecret { get; }
        public string MatrixName { get; }
        public string DeploymentMode { get; }
        public string BrowserFormPost { get; }
        public string SupplierURL { get; }
        public string ItemTable { get; }
    }

    public class PunchoutResponse
    {
        public PunchoutResponse(string code, string status, string startPageUrl, string statusText, string xml)
        {
            Code = code;
            Status = status;
            StartPageUrl = startPageUrl;
            StatusText = statusText;
            XML = xml;
        }

        public string Code { get; }
        public string Status { get; }
        public string StartPageUrl { get; }
        public string StatusText { get; }
        public string XML { get; set; }
    }
}