﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DataObjects;

namespace LogicLayer
{
    public class InvoiceLineManager : IInvoiceLineManager
    {
        private readonly IInvoiceLineAccessor _invoiceLineAccessor;

        public InvoiceLineManager() => _invoiceLineAccessor= new InvoiceLineAccessor();

        public bool AddInvoiceLine(InvoiceLine invoiceLIne)
        {
            bool isAdded;
            try
            {
                isAdded = _invoiceLineAccessor.InsertInvoiceLIne(invoiceLIne);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to add Invoice LIne", ex);
            }

            return isAdded;
        }
    }
}
