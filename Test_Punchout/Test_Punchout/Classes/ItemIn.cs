using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test_Punchout.Classes
{
    public class ItemIn
    {
        public ItemIn(decimal qty, string partId, string auxPartId, decimal unitPrice, string descr, string uom, string classif, string manuPartId, string manuName, string extrinsics)
        {
            Quantity = qty;
            SupplierPartId = partId;
            SupplierPartAuxId = auxPartId;
            UnitPrice = unitPrice;
            Description = descr;
            UnitOfMeasure = uom;
            Classification = classif;
            ManufacturerPartId = manuPartId;
            ManufacturerName = manuName;
            Extrinsics = extrinsics;
        }

        public decimal Quantity { get; }
        public string SupplierPartId { get; }
        public string SupplierPartAuxId { get; }
        public decimal UnitPrice { get; }
        public string Description { get; }
        public string UnitOfMeasure { get; }
        public string Classification { get; }
        public string ManufacturerPartId { get; }
        public string ManufacturerName { get; }
        public string Extrinsics { get; }
    }
}