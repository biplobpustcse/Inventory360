using Inventory360Entity;
using DAL.Interface.Update;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.DataAccess.Update
{
    public class DUpdateDatabaseUpdate : IUpdateDatabaseUpdate
    {
        private Inventory360Entities _db;

        public DUpdateDatabaseUpdate()
        {
            _db = new Inventory360Entities();
        }

        public bool UpdateDatabase()
        {
            try
            {
                CreateOrAlterTableWithRelation();
                CreateOrAlterFunction();
                CreateOrAlterStoredProcedure();
                InsertOrUpdateTableDefaultData();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool CheckTableRecord(string tableName, string column1, string column1Value, string column2, string column2Value)
        {
            dynamic items = _db.Database.SqlQuery<dynamic>("SELECT * FROM " + tableName + " WHERE " + column1 + " = '" + column1Value + "'"
                + (string.IsNullOrEmpty(column2) || string.IsNullOrEmpty(column2Value) ? string.Empty : (" AND " + column2 + " = '" + column2Value + "'")))
                .ToList();

            //if row count is 0 that means no record found in database
            //otherwise record found in database
            if (items.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool CheckTable(string tableName, string fieldName = "", string whereClause = "")
        {
            dynamic items;
            if (!string.IsNullOrEmpty(fieldName))
            {
                items = _db.Database.SqlQuery<dynamic>("SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tableName + "' AND COLUMN_NAME = '" + fieldName + "' " + whereClause)
                    .ToList();
            }
            else
            {
                items = _db.Database.SqlQuery<dynamic>("Select * From sysobjects Where name='" + tableName + "'" + whereClause)
                    .ToList();
            }

            //if row count is 0 that means no record found in database
            //otherwise record found in database
            if (items.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool CheckRelationshipBetweenTwoTables(string tableName, string keyName)
        {
            dynamic items = _db.Database.SqlQuery<dynamic>("SELECT * FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE TABLE_NAME = '" + tableName + "' AND CONSTRAINT_NAME = '" + keyName + "'")
                .ToList();

            //if row count is 0 that means no record found in database
            //otherwise record found in database
            if (items.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool DropDefaultConstraintByColumn(string tableName, string columnName)
        {
            try
            {
                List<string> items = _db.Database.SqlQuery<string>("SELECT default_constraints.name FROM sys.all_columns INNER JOIN sys.tables ON all_columns.object_id = tables.object_id INNER JOIN sys.schemas ON tables.schema_id = schemas.schema_id INNER JOIN sys.default_constraints ON all_columns.default_object_id = default_constraints.object_id "
                    + "WHERE schemas.name = 'dbo' AND tables.name = '" + tableName + "' AND all_columns.name = '" + columnName + "'")
                        .ToList();

                foreach (var item in items)
                {
                    _db.Database.ExecuteSqlCommand("ALTER TABLE " + tableName + " DROP CONSTRAINT " + item);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void CreateOrAlterTableWithRelation()
        {
            /*
            //add column if not found
            if (!CheckTable("Setup_Price", "ProductDimensionId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Price ADD ProductDimensionId bigint NULL");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Setup_Price", "FK_Setup_Price_Setup_ProductDimension"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Price ADD CONSTRAINT FK_Setup_Price_Setup_ProductDimension FOREIGN KEY (ProductDimensionId) REFERENCES Setup_ProductDimension (ProductDimensionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Configuration_OperationalEventDetail", "DefaultPriceTypeId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_OperationalEventDetail ADD DefaultPriceTypeId bigint NULL");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Configuration_OperationalEventDetail", "FK_Configuration_OperationalEventDetail_Setup_PriceType"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_OperationalEventDetail ADD CONSTRAINT FK_Configuration_OperationalEventDetail_Setup_PriceType FOREIGN KEY (DefaultPriceTypeId) REFERENCES Setup_PriceType (PriceTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_SalesOrderNos"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_SalesOrderNos(
                 [Id] [uniqueidentifier] DEFAULT newid() NOT NULL,
                 [SalesOrderNo] [varchar](50) NOT NULL,
                 [Year] [bigint] NOT NULL,
                 [CompanyId] [bigint] NOT NULL,
                 CONSTRAINT [PK_Task_SalesOrderNos] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add unique key if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesOrderNos", "IX_Task_SalesOrderNos"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrderNos ADD CONSTRAINT IX_Task_SalesOrderNos UNIQUE NONCLUSTERED (SalesOrderNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesOrderNos", "FK_Task_SalesOrderNos_Setup_Company"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrderNos ADD CONSTRAINT FK_Task_SalesOrderNos_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_SalesOrder"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_SalesOrder(
                    [SalesOrderId] [uniqueidentifier] DEFAULT newid() NOT NULL,
	                [SalesOrderNo] [varchar](50) NOT NULL,
	                [OrderDate] [datetime] NOT NULL,
	                [CustomerId] [bigint] NOT NULL,
	                [SalesPersonId] [bigint] NOT NULL,
	                [SalesType] [varchar](20) NOT NULL,
	                [ReferenceNo] [varchar](50) NULL,
	                [ReferenceDate] [datetime] NULL,
	                [OperationTypeId] [bigint] NOT NULL,
	                [TermsAndConditionsId] [bigint] NULL,
	                [TermsAndConditionsDetail] [varchar](5000) NULL,
	                [Remarks] [varchar](1000) NULL,
	                [ShipmentType] [varchar](2) NOT NULL,
	                [ApxShipmentDate] [datetime] NULL,
	                [ShipmentMode] [varchar](5) NOT NULL,
	                [DeliveryFromId] [bigint] NOT NULL,
	                [WareHouseId] [bigint] NULL,
	                [PaymentModeId] [bigint] NOT NULL,
	                [PromisedDate] [datetime] NULL,
	                [PaymentTermsId] [bigint] NULL,
	                [PaymentTermsDetail] [varchar](5000) NULL,
	                [SelectedCurrency] [varchar](5) NOT NULL,
	                [Currency1Rate] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Currency2Rate] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [OrderAmount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Order1Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Order2Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [OrderDiscount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Order1Discount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Order2Discount] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [IsSettled] [bit] DEFAULT 0 NOT NULL,
	                [Approved] [varchar](1) DEFAULT 'N' NOT NULL,
	                [ApprovedBy] [bigint] NULL,
	                [ApprovedDate] [datetime] NULL,
	                [CancelReason] [varchar](200) NULL,
	                [LocationId] [bigint] NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
	                [EntryBy] [bigint] NOT NULL,
	                [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Task_SalesOrder] PRIMARY KEY CLUSTERED ([SalesOrderId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add unique key if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesOrder", "IX_Task_SalesOrder"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ADD CONSTRAINT IX_Task_SalesOrder UNIQUE NONCLUSTERED (SalesOrderNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesOrder", "FK_Task_SalesOrder_Setup_Customer"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ADD CONSTRAINT FK_Task_SalesOrder_Setup_Customer FOREIGN KEY (CustomerId) REFERENCES Setup_Customer (CustomerId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesOrder", "FK_Task_SalesOrder_Setup_Employee"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ADD CONSTRAINT FK_Task_SalesOrder_Setup_Employee FOREIGN KEY (SalesPersonId) REFERENCES Setup_Employee (EmployeeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesOrder", "FK_Task_SalesOrder_Configuration_OperationType"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ADD CONSTRAINT FK_Task_SalesOrder_Configuration_OperationType FOREIGN KEY (OperationTypeId) REFERENCES Configuration_OperationType (OperationTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesOrder", "FK_Task_SalesOrder_Setup_TermsAndConditions"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ADD CONSTRAINT FK_Task_SalesOrder_Setup_TermsAndConditions FOREIGN KEY (TermsAndConditionsId) REFERENCES Setup_TermsAndConditions (TermsAndConditionsId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesOrder", "FK_Task_SalesOrder_Setup_Location"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ADD CONSTRAINT FK_Task_SalesOrder_Setup_Location FOREIGN KEY (DeliveryFromId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesOrder", "FK_Task_SalesOrder_Setup_Location1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ADD CONSTRAINT FK_Task_SalesOrder_Setup_Location1 FOREIGN KEY (WareHouseId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesOrder", "FK_Task_SalesOrder_Configuration_PaymentMode"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ADD CONSTRAINT FK_Task_SalesOrder_Configuration_PaymentMode FOREIGN KEY (PaymentModeId) REFERENCES Configuration_PaymentMode (PaymentModeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesOrder", "FK_Task_SalesOrder_Setup_TermsAndConditions1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ADD CONSTRAINT FK_Task_SalesOrder_Setup_TermsAndConditions1 FOREIGN KEY (PaymentTermsId) REFERENCES Setup_TermsAndConditions (TermsAndConditionsId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesOrder", "FK_Task_SalesOrder_Security_User"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ADD CONSTRAINT FK_Task_SalesOrder_Security_User FOREIGN KEY (ApprovedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesOrder", "FK_Task_SalesOrder_Setup_Location2"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ADD CONSTRAINT FK_Task_SalesOrder_Setup_Location2 FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesOrder", "FK_Task_SalesOrder_Setup_Company"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ADD CONSTRAINT FK_Task_SalesOrder_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesOrder", "FK_Task_SalesOrder_Security_User1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ADD CONSTRAINT FK_Task_SalesOrder_Security_User1 FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_SalesOrderDetail"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_SalesOrderDetail(
                    [SalesOrderDetailId] [uniqueidentifier] DEFAULT newid() NOT NULL,
	                [SalesOrderId] [uniqueidentifier] NOT NULL,
	                [ProductId] [bigint] NOT NULL,
	                [ProductDimensionId] [bigint] NULL,
	                [UnitTypeId] [bigint] NOT NULL,
	                [Quantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [DeliveredQuantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Price] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Price1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Price2] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Discount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Discount1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Discount2] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Cost] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Cost1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Cost2] [numeric](18, 4) DEFAULT 0 NOT NULL,
                 CONSTRAINT [PK_Task_SalesOrderDetail] PRIMARY KEY CLUSTERED ([SalesOrderDetailId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesOrderDetail", "FK_Task_SalesOrderDetail_Task_SalesOrder"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrderDetail ADD CONSTRAINT FK_Task_SalesOrderDetail_Task_SalesOrder FOREIGN KEY (SalesOrderId) REFERENCES Task_SalesOrder (SalesOrderId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesOrderDetail", "FK_Task_SalesOrderDetail_Setup_Product"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrderDetail ADD CONSTRAINT FK_Task_SalesOrderDetail_Setup_Product FOREIGN KEY (ProductId) REFERENCES Setup_Product (ProductId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesOrderDetail", "FK_Task_SalesOrderDetail_Setup_ProductDimension"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrderDetail ADD CONSTRAINT FK_Task_SalesOrderDetail_Setup_ProductDimension FOREIGN KEY (ProductDimensionId) REFERENCES Setup_ProductDimension (ProductDimensionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesOrderDetail", "FK_Task_SalesOrderDetail_Setup_UnitType"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrderDetail ADD CONSTRAINT FK_Task_SalesOrderDetail_Setup_UnitType FOREIGN KEY (UnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_SalesOrderDeliveryInfo"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_SalesOrderDeliveryInfo(
	                [DeliveryInfoId] [uniqueidentifier] DEFAULT newid() NOT NULL,
	                [SalesOrderId] [uniqueidentifier] NOT NULL,
	                [DeliveryPlace] [varchar](300) NULL,
	                [ContactPerson] [varchar](100) NULL,
	                [ContactPersonNo] [varchar](30) NULL,
	                [TransportId] [bigint] NULL,
	                [TransportTypeId] [bigint] NULL,
	                [VehicleNo] [varchar](30) NULL,
	                [DriverName] [varchar](100) NULL,
	                [DriverContactNo] [varchar](30) NULL,
                 CONSTRAINT [PK_Task_SalesOrderDeliveryInfo] PRIMARY KEY CLUSTERED ([DeliveryInfoId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesOrderDeliveryInfo", "FK_Task_SalesOrderDeliveryInfo_Task_SalesOrder"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrderDeliveryInfo ADD CONSTRAINT FK_Task_SalesOrderDeliveryInfo_Task_SalesOrder FOREIGN KEY (SalesOrderId) REFERENCES Task_SalesOrder (SalesOrderId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesOrderDeliveryInfo", "FK_Task_SalesOrderDeliveryInfo_Setup_Transport"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrderDeliveryInfo ADD CONSTRAINT FK_Task_SalesOrderDeliveryInfo_Setup_Transport FOREIGN KEY (TransportId) REFERENCES Setup_Transport (TransportId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesOrderDeliveryInfo", "FK_Task_SalesOrderDeliveryInfo_Setup_TransportType"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrderDeliveryInfo ADD CONSTRAINT FK_Task_SalesOrderDeliveryInfo_Setup_TransportType FOREIGN KEY (TransportTypeId) REFERENCES Setup_TransportType (TransportTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Setup_Price", "UnitTypeId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Price ADD UnitTypeId bigint NOT NULL");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Setup_Price", "FK_Setup_Price_Setup_UnitType"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Price ADD CONSTRAINT FK_Setup_Price_Setup_UnitType FOREIGN KEY (UnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_DeliveryChallanNos"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_DeliveryChallanNos(
                 [Id] [uniqueidentifier] DEFAULT newid() NOT NULL,
                 [ChallanNo] [varchar](50) NOT NULL,
                 [Year] [bigint] NOT NULL,
                 [CompanyId] [bigint] NOT NULL,
                 CONSTRAINT [PK_Task_DeliveryChallanNos] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add unique key if not found
            if (!CheckRelationshipBetweenTwoTables("Task_DeliveryChallanNos", "IX_Task_DeliveryChallanNos"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanNos ADD CONSTRAINT IX_Task_DeliveryChallanNos UNIQUE NONCLUSTERED (ChallanNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_DeliveryChallanNos", "FK_Task_DeliveryChallanNos_Setup_Company"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanNos ADD CONSTRAINT FK_Task_DeliveryChallanNos_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_DeliveryChallan"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_DeliveryChallan(
	                [ChallanId] [uniqueidentifier] DEFAULT newid() NOT NULL,
                    [ChallanNo] [varchar](50) NOT NULL,
	                [ChallanDate] [datetime] NOT NULL,
	                [CustomerId] [bigint] NOT NULL,
	                [SalesOrderId] [uniqueidentifier] NOT NULL,
                    [DeliveryFromId] [bigint] NOT NULL,
	                [WareHouseId] [bigint] NULL,
	                [DeliveryPlace] [varchar](300) NULL,
	                [ContactPerson] [varchar](100) NULL,
	                [ContactPersonNo] [varchar](30) NULL,
	                [TransportId] [bigint] NULL,
	                [TransportTypeId] [bigint] NULL,
	                [VehicleNo] [varchar](30) NULL,
	                [DriverName] [varchar](100) NULL,
	                [DriverContactNo] [varchar](30) NULL,
                    [IsSettled] [bit] DEFAULT 0 NOT NULL,
	                [Approved] [varchar](1) DEFAULT 'N' NOT NULL,
	                [ApprovedBy] [bigint] NULL,
	                [ApprovedDate] [datetime] NULL,
	                [CancelReason] [varchar](200) NULL,
	                [LocationId] [bigint] NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
	                [EntryBy] [bigint] NOT NULL,
	                [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Task_DeliveryChallan] PRIMARY KEY CLUSTERED ([ChallanId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add unique key if not found
            if (!CheckRelationshipBetweenTwoTables("Task_DeliveryChallan", "IX_Task_DeliveryChallan"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallan ADD CONSTRAINT IX_Task_DeliveryChallan UNIQUE NONCLUSTERED (ChallanNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_DeliveryChallan", "FK_Task_DeliveryChallan_Setup_Customer"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallan ADD CONSTRAINT FK_Task_DeliveryChallan_Setup_Customer FOREIGN KEY (CustomerId) REFERENCES Setup_Customer (CustomerId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_DeliveryChallan", "FK_Task_DeliveryChallan_Task_SalesOrder"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallan ADD CONSTRAINT FK_Task_DeliveryChallan_Task_SalesOrder FOREIGN KEY (SalesOrderId) REFERENCES Task_SalesOrder (SalesOrderId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_DeliveryChallan", "FK_Task_DeliveryChallan_Setup_Transport"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallan ADD CONSTRAINT FK_Task_DeliveryChallan_Setup_Transport FOREIGN KEY (TransportId) REFERENCES Setup_Transport (TransportId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_DeliveryChallan", "FK_Task_DeliveryChallan_Setup_TransportType"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallan ADD CONSTRAINT FK_Task_DeliveryChallan_Setup_TransportType FOREIGN KEY (TransportTypeId) REFERENCES Setup_TransportType (TransportTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_DeliveryChallan", "FK_Task_DeliveryChallan_Setup_Location"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallan ADD CONSTRAINT FK_Task_DeliveryChallan_Setup_Location FOREIGN KEY (DeliveryFromId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_DeliveryChallan", "FK_Task_DeliveryChallan_Setup_Location1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallan ADD CONSTRAINT FK_Task_DeliveryChallan_Setup_Location1 FOREIGN KEY (WareHouseId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_DeliveryChallan", "FK_Task_DeliveryChallan_Security_User"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallan ADD CONSTRAINT FK_Task_DeliveryChallan_Security_User FOREIGN KEY (ApprovedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_DeliveryChallan", "FK_Task_DeliveryChallan_Setup_Location2"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallan ADD CONSTRAINT FK_Task_DeliveryChallan_Setup_Location2 FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_DeliveryChallan", "FK_Task_DeliveryChallan_Setup_Company"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallan ADD CONSTRAINT FK_Task_DeliveryChallan_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_DeliveryChallan", "FK_Task_DeliveryChallan_Security_User1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallan ADD CONSTRAINT FK_Task_DeliveryChallan_Security_User1 FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_DeliveryChallanDetail"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_DeliveryChallanDetail(
                    [ChallanDetailId] [uniqueidentifier] DEFAULT newid() NOT NULL,
	                [ChallanId] [uniqueidentifier] NOT NULL,
	                [ProductId] [bigint] NOT NULL,
	                [ProductDimensionId] [bigint] NULL,
	                [UnitTypeId] [bigint] NOT NULL,
	                [Quantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [InvoicedQuantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [Price] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Price1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Price2] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Discount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Discount1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Discount2] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Cost] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Cost1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Cost2] [numeric](18, 4) DEFAULT 0 NOT NULL,
                 CONSTRAINT [PK_Task_DeliveryChallanDetail] PRIMARY KEY CLUSTERED ([ChallanDetailId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_DeliveryChallanDetail", "FK_Task_DeliveryChallanDetail_Task_DeliveryChallan"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD CONSTRAINT FK_Task_DeliveryChallanDetail_Task_DeliveryChallan FOREIGN KEY (ChallanId) REFERENCES Task_DeliveryChallan (ChallanId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_DeliveryChallanDetail", "FK_Task_DeliveryChallanDetail_Setup_Product"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD CONSTRAINT FK_Task_DeliveryChallanDetail_Setup_Product FOREIGN KEY (ProductId) REFERENCES Setup_Product (ProductId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_DeliveryChallanDetail", "FK_Task_DeliveryChallanDetail_Setup_ProductDimension"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD CONSTRAINT FK_Task_DeliveryChallanDetail_Setup_ProductDimension FOREIGN KEY (ProductDimensionId) REFERENCES Setup_ProductDimension (ProductDimensionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_DeliveryChallanDetail", "FK_Task_DeliveryChallanDetail_Setup_UnitType"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD CONSTRAINT FK_Task_DeliveryChallanDetail_Setup_UnitType FOREIGN KEY (UnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Stock_CurrentStock"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Stock_CurrentStock(
                    [CurrentStockId] [uniqueidentifier] DEFAULT newid() NOT NULL,
	                [ProductId] [bigint] NOT NULL,
	                [ProductDimensionId] [bigint] NULL,
	                [UnitTypeId] [bigint] NOT NULL,
	                [Quantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [Cost] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Cost1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Cost2] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [LocationId] [bigint] NOT NULL,
	                [WareHouseId] [bigint] NULL,
                    [CompanyId] [bigint] NOT NULL,
	                [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Stock_CurrentStock] PRIMARY KEY CLUSTERED ([CurrentStockId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Stock_CurrentStock", "FK_Stock_CurrentStock_Setup_Product"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_CurrentStock ADD CONSTRAINT FK_Stock_CurrentStock_Setup_Product FOREIGN KEY (ProductId) REFERENCES Setup_Product (ProductId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Stock_CurrentStock", "FK_Stock_CurrentStock_Setup_ProductDimension"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_CurrentStock ADD CONSTRAINT FK_Stock_CurrentStock_Setup_ProductDimension FOREIGN KEY (ProductDimensionId) REFERENCES Setup_ProductDimension (ProductDimensionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Stock_CurrentStock", "FK_Stock_CurrentStock_Setup_UnitType"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_CurrentStock ADD CONSTRAINT FK_Stock_CurrentStock_Setup_UnitType FOREIGN KEY (UnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Stock_CurrentStock", "FK_Stock_CurrentStock_Setup_Location"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_CurrentStock ADD CONSTRAINT FK_Stock_CurrentStock_Setup_Location FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Stock_CurrentStock", "FK_Stock_CurrentStock_Setup_Location1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_CurrentStock ADD CONSTRAINT FK_Stock_CurrentStock_Setup_Location1 FOREIGN KEY (WareHouseId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Stock_CurrentStock", "FK_Stock_CurrentStock_Setup_Company"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_CurrentStock ADD CONSTRAINT FK_Stock_CurrentStock_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_SalesInvoiceNos"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_SalesInvoiceNos(
                 [Id] [uniqueidentifier] DEFAULT newid() NOT NULL,
                 [InvoiceNo] [varchar](50) NOT NULL,
                 [Year] [bigint] NOT NULL,
                 [CompanyId] [bigint] NOT NULL,
                 CONSTRAINT [PK_Task_SalesInvoiceNos] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add unique key if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesInvoiceNos", "IX_Task_SalesInvoiceNos"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceNos ADD CONSTRAINT IX_Task_SalesInvoiceNos UNIQUE NONCLUSTERED (InvoiceNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesInvoiceNos", "FK_Task_SalesInvoiceNos_Setup_Company"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceNos ADD CONSTRAINT FK_Task_SalesInvoiceNos_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_SalesInvoice"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_SalesInvoice(
                    [InvoiceId] [uniqueidentifier] DEFAULT newid() NOT NULL,
	                [InvoiceNo] [varchar](50) NOT NULL,
                    [InvoiceDate] [datetime] NOT NULL,
	                [CustomerId] [bigint] NOT NULL,
	                [SelectedCurrency] [varchar](5) NOT NULL,
	                [InvoiceAmount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Invoice1Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Invoice2Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [InvoiceDiscount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Invoice1Discount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Invoice2Discount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Approved] [varchar](1) DEFAULT 'N' NOT NULL,
	                [ApprovedBy] [bigint] NULL,
	                [ApprovedDate] [datetime] NULL,
	                [CancelReason] [varchar](200) NULL,
                    [VoucherId] [uniqueidentifier] NULL,
                    [IsSettled] [bit] DEFAULT 0 NOT NULL,
                    [CollectedAmount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Collected1Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Collected2Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [LocationId] [bigint] NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
	                [EntryBy] [bigint] NOT NULL,
	                [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Task_SalesInvoice] PRIMARY KEY CLUSTERED ([InvoiceId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add unique key if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesInvoice", "IX_Task_SalesInvoice"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD CONSTRAINT IX_Task_SalesInvoice UNIQUE NONCLUSTERED (InvoiceNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesInvoice", "FK_Task_SalesInvoice_Setup_Customer"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD CONSTRAINT FK_Task_SalesInvoice_Setup_Customer FOREIGN KEY (CustomerId) REFERENCES Setup_Customer (CustomerId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesInvoice", "FK_Task_SalesInvoice_Security_User"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD CONSTRAINT FK_Task_SalesInvoice_Security_User FOREIGN KEY (ApprovedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesInvoice", "FK_Task_SalesInvoice_Setup_Location"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD CONSTRAINT FK_Task_SalesInvoice_Setup_Location FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesInvoice", "FK_Task_SalesInvoice_Setup_Company"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD CONSTRAINT FK_Task_SalesInvoice_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesInvoice", "FK_Task_SalesInvoice_Security_User1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD CONSTRAINT FK_Task_SalesInvoice_Security_User1 FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesInvoice", "FK_Task_SalesInvoice_Task_Voucher"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD CONSTRAINT FK_Task_SalesInvoice_Task_Voucher FOREIGN KEY (VoucherId) REFERENCES Task_Voucher (VoucherId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_SalesInvoiceDetail"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_SalesInvoiceDetail(
                    [InvoiceDetailId] [uniqueidentifier] DEFAULT newid() NOT NULL,
	                [InvoiceId] [uniqueidentifier] NOT NULL,
                    [ChallanId] [uniqueidentifier] NOT NULL,
	                [ProductId] [bigint] NOT NULL,
	                [ProductDimensionId] [bigint] NULL,
	                [UnitTypeId] [bigint] NOT NULL,
	                [Quantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Price] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Price1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Price2] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Discount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Discount1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Discount2] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Cost] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Cost1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Cost2] [numeric](18, 4) DEFAULT 0 NOT NULL,
                 CONSTRAINT [PK_Task_SalesInvoiceDetail] PRIMARY KEY CLUSTERED ([InvoiceDetailId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesInvoiceDetail", "FK_Task_SalesInvoiceDetail_Task_SalesInvoice"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD CONSTRAINT FK_Task_SalesInvoiceDetail_Task_SalesInvoice FOREIGN KEY (InvoiceId) REFERENCES Task_SalesInvoice (InvoiceId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesInvoiceDetail", "FK_Task_SalesInvoiceDetail_Task_DeliveryChallan"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD CONSTRAINT FK_Task_SalesInvoiceDetail_Task_DeliveryChallan FOREIGN KEY (ChallanId) REFERENCES Task_DeliveryChallan (ChallanId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesInvoiceDetail", "FK_Task_SalesInvoiceDetail_Setup_Product"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD CONSTRAINT FK_Task_SalesInvoiceDetail_Setup_Product FOREIGN KEY (ProductId) REFERENCES Setup_Product (ProductId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesInvoiceDetail", "FK_Task_SalesInvoiceDetail_Setup_ProductDimension"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD CONSTRAINT FK_Task_SalesInvoiceDetail_Setup_ProductDimension FOREIGN KEY (ProductDimensionId) REFERENCES Setup_ProductDimension (ProductDimensionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesInvoiceDetail", "FK_Task_SalesInvoiceDetail_Setup_UnitType"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD CONSTRAINT FK_Task_SalesInvoiceDetail_Setup_UnitType FOREIGN KEY (UnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Configuration_FormattingTag"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Configuration_FormattingTag(
                    [Id] [bigint] IDENTITY(1,1) NOT NULL,
	                [TagName] [varchar](50) NOT NULL,
                    [Type] [varchar](20) NOT NULL,
	                [DataSource] [Varchar](500) NULL,
                 CONSTRAINT [PK_Configuration_FormattingTag] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //if table not found then create table
            if (!CheckTable("Configuration_Voucher"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Configuration_Voucher(
                    [VoucherConfigurationId] [bigint] IDENTITY(1,1) NOT NULL,
	                [Prefix] [varchar](5) NOT NULL,
                    [NumberFormat] [varchar](100) NOT NULL,
	                [Description] [Varchar](1500) NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
                 CONSTRAINT [PK_Configuration_Voucher] PRIMARY KEY CLUSTERED ([VoucherConfigurationId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Configuration_Voucher", "FK_Configuration_Voucher_Setup_Company"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_Voucher ADD CONSTRAINT FK_Configuration_Voucher_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Configuration_VoucherDetail"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Configuration_VoucherDetail(
                    [VoucherConfigurationDetailId] [bigint] IDENTITY(1,1) NOT NULL,
                    [VoucherConfigurationId] [bigint] NOT NULL,
	                [Particulars] [Varchar](1500) NOT NULL,
	                [AccountsId] [bigint] NULL,
                    [DrOrCr] [Varchar](2) NOT NULL,
                    [DataSource] [Varchar](500) NULL,
                 CONSTRAINT [PK_Configuration_VoucherDetail] PRIMARY KEY CLUSTERED ([VoucherConfigurationDetailId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Configuration_VoucherDetail", "FK_Configuration_VoucherDetail_Configuration_Voucher"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_VoucherDetail ADD CONSTRAINT FK_Configuration_VoucherDetail_Configuration_Voucher FOREIGN KEY (VoucherConfigurationId) REFERENCES Configuration_Voucher (VoucherConfigurationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Configuration_VoucherDetail", "FK_Configuration_VoucherDetail_Setup_Accounts"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_VoucherDetail ADD CONSTRAINT FK_Configuration_VoucherDetail_Setup_Accounts FOREIGN KEY (AccountsId) REFERENCES Setup_Accounts (AccountsId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Configuration_OperationalEventDetail"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Configuration_OperationalEventDetail(
                    [OperationalEventDetailId] [bigint] IDENTITY(1,1) NOT NULL,
                    [OperationalEventId] [bigint] NOT NULL,
                    [OperationTypeId] [bigint] NOT NULL,
                    [PaymentModeId] [bigint] NULL,
                    [Prefix] [Varchar](5) NOT NULL,
                    [NumberFormat] [varchar](100) NOT NULL,
                    [VoucherConfigurationId] [bigint] NULL,
                    [LocationId] [bigint] NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
                 CONSTRAINT [PK_Configuration_OperationalEventDetail] PRIMARY KEY CLUSTERED ([OperationalEventDetailId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Configuration_OperationalEventDetail", "FK_Configuration_OperationalEventDetail_Configuration_OperationalEvent"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_OperationalEventDetail ADD CONSTRAINT FK_Configuration_OperationalEventDetail_Configuration_OperationalEvent FOREIGN KEY (OperationalEventId) REFERENCES Configuration_OperationalEvent (OperationalEventId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Configuration_OperationalEventDetail", "FK_Configuration_OperationalEventDetail_Configuration_OperationType"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_OperationalEventDetail ADD CONSTRAINT FK_Configuration_OperationalEventDetail_Configuration_OperationType FOREIGN KEY (OperationTypeId) REFERENCES Configuration_OperationType (OperationTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Configuration_OperationalEventDetail", "FK_Configuration_OperationalEventDetail_Configuration_PaymentMode"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_OperationalEventDetail ADD CONSTRAINT FK_Configuration_OperationalEventDetail_Configuration_PaymentMode FOREIGN KEY (PaymentModeId) REFERENCES Configuration_PaymentMode (PaymentModeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Configuration_OperationalEventDetail", "FK_Configuration_OperationalEventDetail_Configuration_Voucher"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_OperationalEventDetail ADD CONSTRAINT FK_Configuration_OperationalEventDetail_Configuration_Voucher FOREIGN KEY (VoucherConfigurationId) REFERENCES Configuration_Voucher (VoucherConfigurationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Configuration_OperationalEventDetail", "FK_Configuration_OperationalEventDetail_Setup_Location"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_OperationalEventDetail ADD CONSTRAINT FK_Configuration_OperationalEventDetail_Setup_Location FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Configuration_OperationalEventDetail", "FK_Configuration_OperationalEventDetail_Setup_Company"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_OperationalEventDetail ADD CONSTRAINT FK_Configuration_OperationalEventDetail_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Stock_AdjustmentNos"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Stock_AdjustmentNos(
                 [Id] [uniqueidentifier] DEFAULT newid() NOT NULL,
                 [AdjustmentNo] [varchar](50) NOT NULL,
                 [Year] [bigint] NOT NULL,
                 [CompanyId] [bigint] NOT NULL,
                 CONSTRAINT [PK_Stock_AdjustmentNos] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add unique key if not found
            if (!CheckRelationshipBetweenTwoTables("Stock_AdjustmentNos", "IX_Stock_AdjustmentNos"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_AdjustmentNos ADD CONSTRAINT IX_Stock_AdjustmentNos UNIQUE NONCLUSTERED (AdjustmentNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Stock_AdjustmentNos", "FK_Stock_AdjustmentNos_Setup_Company"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_AdjustmentNos ADD CONSTRAINT FK_Stock_AdjustmentNos_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Stock_Adjustment"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Stock_Adjustment(
                    [AdjustmentId] [uniqueidentifier] DEFAULT newid() NOT NULL,
	                [AdjustmentNo] [varchar](50) NOT NULL,
	                [AdjustmentDate] [datetime] NOT NULL,
	                [SelectedCurrency] [varchar](5) NOT NULL,
	                [Currency1Rate] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Currency2Rate] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Approved] [varchar](1) DEFAULT 'N' NOT NULL,
	                [ApprovedBy] [bigint] NULL,
	                [ApprovedDate] [datetime] NULL,
	                [CancelReason] [varchar](200) NULL,
	                [LocationId] [bigint] NOT NULL,
                    [WareHouseId] [bigint] NULL,
	                [CompanyId] [bigint] NOT NULL,
	                [EntryBy] [bigint] NOT NULL,
	                [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Stock_Adjustment] PRIMARY KEY CLUSTERED ([AdjustmentId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add unique key if not found
            if (!CheckRelationshipBetweenTwoTables("Stock_Adjustment", "IX_Stock_Adjustment"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_Adjustment ADD CONSTRAINT IX_Stock_Adjustment UNIQUE NONCLUSTERED (AdjustmentNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Stock_Adjustment", "FK_Stock_Adjustment_Security_User"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_Adjustment ADD CONSTRAINT FK_Stock_Adjustment_Security_User FOREIGN KEY (ApprovedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Stock_Adjustment", "FK_Stock_Adjustment_Setup_Location"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_Adjustment ADD CONSTRAINT FK_Stock_Adjustment_Setup_Location FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Stock_Adjustment", "FK_Stock_Adjustment_Setup_Location1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_Adjustment ADD CONSTRAINT FK_Stock_Adjustment_Setup_Location1 FOREIGN KEY (WareHouseId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Stock_Adjustment", "FK_Stock_Adjustment_Setup_Company"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_Adjustment ADD CONSTRAINT FK_Stock_Adjustment_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Stock_Adjustment", "FK_Stock_Adjustment_Security_User1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_Adjustment ADD CONSTRAINT FK_Stock_Adjustment_Security_User1 FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Stock_AdjustmentDetail"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Stock_AdjustmentDetail(
                    [AdjustmentDetailId] [uniqueidentifier] DEFAULT newid() NOT NULL,
	                [AdjustmentId] [uniqueidentifier] NOT NULL,
	                [ProductId] [bigint] NOT NULL,
	                [ProductDimensionId] [bigint] NULL,
	                [UnitTypeId] [bigint] NOT NULL,
	                [Quantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [IncreaseDecrease] [varchar](1) NOT NULL,
	                [Cost] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Cost1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Cost2] [numeric](18, 4) DEFAULT 0 NOT NULL,
                 CONSTRAINT [PK_Stock_AdjustmentDetail] PRIMARY KEY CLUSTERED ([AdjustmentDetailId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Stock_AdjustmentDetail", "FK_Stock_AdjustmentDetail_Stock_Adjustment"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_AdjustmentDetail ADD CONSTRAINT FK_Stock_AdjustmentDetail_Stock_Adjustment FOREIGN KEY (AdjustmentId) REFERENCES Stock_Adjustment (AdjustmentId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Stock_AdjustmentDetail", "FK_Stock_AdjustmentDetail_Setup_Product"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_AdjustmentDetail ADD CONSTRAINT FK_Stock_AdjustmentDetail_Setup_Product FOREIGN KEY (ProductId) REFERENCES Setup_Product (ProductId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Stock_AdjustmentDetail", "FK_Stock_AdjustmentDetail_Setup_ProductDimension"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_AdjustmentDetail ADD CONSTRAINT FK_Stock_AdjustmentDetail_Setup_ProductDimension FOREIGN KEY (ProductDimensionId) REFERENCES Setup_ProductDimension (ProductDimensionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Stock_AdjustmentDetail", "FK_Stock_AdjustmentDetail_Setup_UnitType"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_AdjustmentDetail ADD CONSTRAINT FK_Stock_AdjustmentDetail_Setup_UnitType FOREIGN KEY (UnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Setup_Customer", "SelectedCurrency"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Customer ADD SelectedCurrency [varchar](5) NULL");
            }

            //add column if not found
            if (!CheckTable("Setup_Customer", "Currency1Rate"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Customer ADD Currency1Rate [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Setup_Customer", "Currency2Rate"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Customer ADD Currency2Rate [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Setup_Customer", "OpeningBalance1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Customer ADD OpeningBalance1 [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Setup_Customer", "OpeningBalance2"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Customer ADD OpeningBalance2 [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Setup_Customer", "ChequeDishonourBalance1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Customer ADD ChequeDishonourBalance1 [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Setup_Customer", "ChequeDishonourBalance2"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Customer ADD ChequeDishonourBalance2 [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Setup_Customer", "CreditLimit1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Customer ADD CreditLimit1 [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Setup_Customer", "CreditLimit2"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Customer ADD CreditLimit2 [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Setup_Customer", "LedgerLimit1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Customer ADD LedgerLimit1 [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Setup_Customer", "LedgerLimit2"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Customer ADD LedgerLimit2 [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Setup_Supplier", "SelectedCurrency"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Supplier ADD SelectedCurrency [varchar](5) NULL");
            }

            //add column if not found
            if (!CheckTable("Setup_Supplier", "Currency1Rate"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Supplier ADD Currency1Rate [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Setup_Supplier", "Currency2Rate"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Supplier ADD Currency2Rate [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Setup_Supplier", "OpeningBalance1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Supplier ADD OpeningBalance1 [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Setup_Supplier", "OpeningBalance2"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Supplier ADD OpeningBalance2 [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //if table not found then create table
            if (!CheckTable("Temp_PartyLedger"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Temp_PartyLedger(
                    [TempId] [uniqueidentifier] NOT NULL,
	                [Date] [datetime] NULL,
	                [Type] [varchar](50) NULL,
	                [Particular] [varchar](50) NULL,
	                [DrAmount] [numeric](18, 4) NULL,
	                [CrAmount] [numeric](18, 4) NULL,
	                [CustomerId] [bigint] NULL,
	                [SupplierId] [bigint] NULL,
	                [CompanyId] [bigint] NULL,
	                [EntryBy] [bigint] NULL,
                 CONSTRAINT [PK_Temp_PartyLedger] PRIMARY KEY CLUSTERED ([TempId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Temp_PartyLedger", "FK_Temp_PartyLedger_Setup_Customer"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_PartyLedger ADD CONSTRAINT FK_Temp_PartyLedger_Setup_Customer FOREIGN KEY (CustomerId) REFERENCES Setup_Customer (CustomerId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Temp_PartyLedger", "FK_Temp_PartyLedger_Setup_Supplier"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_PartyLedger ADD CONSTRAINT FK_Temp_PartyLedger_Setup_Supplier FOREIGN KEY (SupplierId) REFERENCES Setup_Supplier (SupplierId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Temp_PartyLedger", "FK_Temp_PartyLedger_Setup_Company"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_PartyLedger ADD CONSTRAINT FK_Temp_PartyLedger_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Temp_PartyLedger", "FK_Temp_PartyLedger_Security_User"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_PartyLedger ADD CONSTRAINT FK_Temp_PartyLedger_Security_User FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_CollectionNos"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_CollectionNos(
                 [Id] [uniqueidentifier] DEFAULT newid() NOT NULL,
                 [CollectionNo] [varchar](50) NOT NULL,
                 [Year] [bigint] NOT NULL,
                 [CompanyId] [bigint] NOT NULL,
                 CONSTRAINT [PK_Task_CollectionNos] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add unique key if not found
            if (!CheckRelationshipBetweenTwoTables("Task_CollectionNos", "IX_Task_CollectionNos"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CollectionNos ADD CONSTRAINT IX_Task_CollectionNos UNIQUE NONCLUSTERED (CollectionNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_CollectionNos", "FK_Task_CollectionNos_Setup_Company"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CollectionNos ADD CONSTRAINT FK_Task_CollectionNos_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_Collection"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_Collection(
                    [CollectionId] [uniqueidentifier] DEFAULT newid() NOT NULL,
	                [CollectionNo] [varchar](50) NOT NULL,
	                [CollectionDate] [datetime] NOT NULL,
	                [SelectedCurrency] [varchar](5) NOT NULL,
	                [Currency1Rate] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Currency2Rate] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [CollectedAmount] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [CollectedAmount1] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [CollectedAmount2] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [CustomerId] [bigint] NOT NULL,
                    [SalesPersonId] [bigint] NOT NULL,
                    [CollectedBy] [bigint] NOT NULL,
                    [MRNo] [varchar](50) NULL,
                    [Remarks] [varchar](1000) NULL,
                    [OperationTypeId] [bigint] NOT NULL,
                    [OperationalEventId] [bigint] NOT NULL,
	                [Approved] [varchar](1) DEFAULT 'N' NOT NULL,
	                [ApprovedBy] [bigint] NULL,
	                [ApprovedDate] [datetime] NULL,
	                [CancelReason] [varchar](200) NULL,
	                [LocationId] [bigint] NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
	                [EntryBy] [bigint] NOT NULL,
	                [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Task_Collection] PRIMARY KEY CLUSTERED ([CollectionId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_Collection", "FK_Task_Collection_Setup_Customer"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Collection ADD CONSTRAINT FK_Task_Collection_Setup_Customer FOREIGN KEY (CustomerId) REFERENCES Setup_Customer (CustomerId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_Collection", "FK_Task_Collection_Setup_Employee"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Collection ADD CONSTRAINT FK_Task_Collection_Setup_Employee FOREIGN KEY (SalesPersonId) REFERENCES Setup_Employee (EmployeeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_Collection", "FK_Task_Collection_Security_User"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Collection ADD CONSTRAINT FK_Task_Collection_Security_User FOREIGN KEY (CollectedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_Collection", "FK_Task_Collection_Configuration_OperationType"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Collection ADD CONSTRAINT FK_Task_Collection_Configuration_OperationType FOREIGN KEY (OperationTypeId) REFERENCES Configuration_OperationType (OperationTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_Collection", "FK_Task_Collection_Configuration_OperationalEvent"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Collection ADD CONSTRAINT FK_Task_Collection_Configuration_OperationalEvent FOREIGN KEY (OperationalEventId) REFERENCES Configuration_OperationalEvent (OperationalEventId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_Collection", "FK_Task_Collection_Security_User1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Collection ADD CONSTRAINT FK_Task_Collection_Security_User1 FOREIGN KEY (ApprovedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_Collection", "FK_Task_Collection_Setup_Location"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Collection ADD CONSTRAINT FK_Task_Collection_Setup_Location FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_Collection", "FK_Task_Collection_Setup_Company"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Collection ADD CONSTRAINT FK_Task_Collection_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_Collection", "FK_Task_Collection_Security_User2"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Collection ADD CONSTRAINT FK_Task_Collection_Security_User2 FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_CollectionDetail"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_CollectionDetail(
                    [CollectionDetailId] [uniqueidentifier] DEFAULT newid() NOT NULL,
                    [CollectionId] [uniqueidentifier] NOT NULL,
                    [PaymentModeId] [bigint] NOT NULL,
                    [Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [Amount1] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [Amount2] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [VoucherId] [uniqueidentifier] NULL,
                CONSTRAINT [PK_Task_CollectionDetail] PRIMARY KEY CLUSTERED ([CollectionDetailId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_CollectionDetail", "FK_Task_CollectionDetail_Task_Collection"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CollectionDetail ADD CONSTRAINT FK_Task_CollectionDetail_Task_Collection FOREIGN KEY (CollectionId) REFERENCES Task_Collection (CollectionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_CollectionDetail", "FK_Task_CollectionDetail_Configuration_PaymentMode"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CollectionDetail ADD CONSTRAINT FK_Task_CollectionDetail_Configuration_PaymentMode FOREIGN KEY (PaymentModeId) REFERENCES Configuration_PaymentMode (PaymentModeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_CollectionDetail", "FK_Task_CollectionDetail_Task_Voucher"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CollectionDetail ADD CONSTRAINT FK_Task_CollectionDetail_Task_Voucher FOREIGN KEY (VoucherId) REFERENCES Task_Voucher (VoucherId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_ChequeInfo"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_ChequeInfo(
                    [ChequeInfoId] [uniqueidentifier] DEFAULT newid() NOT NULL,
                    [CollectionDetailId] [uniqueidentifier] NULL,
                    [EntryVoucherId] [uniqueidentifier] NULL,
                    [BankId] [bigint] NOT NULL,
                    [ChequeNo] [varchar](50) NOT NULL,
                    [ChequeDate] [datetime] NOT NULL,
                    [Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [Amount1] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [Amount2] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [Status] [varchar](5) NULL,
                    [StatusDate] [datetime] NULL,
                    [LocationId] [bigint] NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
	                [EntryBy] [bigint] NOT NULL,
	                [EntryDate] [datetime] NOT NULL,
                CONSTRAINT [PK_Task_ChequeInfo] PRIMARY KEY CLUSTERED ([ChequeInfoId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ChequeInfo", "FK_Task_ChequeInfo_Task_CollectionDetail"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ChequeInfo ADD CONSTRAINT FK_Task_ChequeInfo_Task_CollectionDetail FOREIGN KEY (CollectionDetailId) REFERENCES Task_CollectionDetail (CollectionDetailId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ChequeInfo", "FK_Task_ChequeInfo_Task_Voucher"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ChequeInfo ADD CONSTRAINT FK_Task_ChequeInfo_Task_Voucher FOREIGN KEY (EntryVoucherId) REFERENCES Task_Voucher (VoucherId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ChequeInfo", "FK_Task_ChequeInfo_Setup_Bank"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ChequeInfo ADD CONSTRAINT FK_Task_ChequeInfo_Setup_Bank FOREIGN KEY (BankId) REFERENCES Setup_Bank (BankId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ChequeInfo", "FK_Task_ChequeInfo_Setup_Location"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ChequeInfo ADD CONSTRAINT FK_Task_ChequeInfo_Setup_Location FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ChequeInfo", "FK_Task_ChequeInfo_Setup_Company"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ChequeInfo ADD CONSTRAINT FK_Task_ChequeInfo_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ChequeInfo", "FK_Task_ChequeInfo_Security_User"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ChequeInfo ADD CONSTRAINT FK_Task_ChequeInfo_Security_User FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_ChequeTreatment"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_ChequeTreatment(
                    [TreatmentId] [uniqueidentifier] DEFAULT newid() NOT NULL,
                    [ChequeInfoId] [uniqueidentifier] NOT NULL,
                    [Status] [varchar](5) NULL,
                    [StatusDate] [datetime] NULL,
                    [TreatmentBankId] [bigint] NOT NULL,
                    [VoucherId] [uniqueidentifier] NULL,
	                [EntryBy] [bigint] NOT NULL,
	                [EntryDate] [datetime] NOT NULL,
                CONSTRAINT [PK_Task_ChequeTreatment] PRIMARY KEY CLUSTERED ([TreatmentId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ChequeTreatment", "FK_Task_ChequeTreatment_Task_ChequeInfo"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ChequeTreatment ADD CONSTRAINT FK_Task_ChequeTreatment_Task_ChequeInfo FOREIGN KEY (ChequeInfoId) REFERENCES Task_ChequeInfo (ChequeInfoId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ChequeTreatment", "FK_Task_ChequeTreatment_Setup_Bank"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ChequeTreatment ADD CONSTRAINT FK_Task_ChequeTreatment_Setup_Bank FOREIGN KEY (TreatmentBankId) REFERENCES Setup_Bank (BankId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ChequeTreatment", "FK_Task_ChequeTreatment_Task_Voucher"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ChequeTreatment ADD CONSTRAINT FK_Task_ChequeTreatment_Task_Voucher FOREIGN KEY (VoucherId) REFERENCES Task_Voucher (VoucherId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ChequeTreatment", "FK_Task_ChequeTreatment_Security_User"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ChequeTreatment ADD CONSTRAINT FK_Task_ChequeTreatment_Security_User FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Configuration_PaymentMode", "NeedDetail"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_PaymentMode ADD NeedDetail bit DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Setup_Customer", "CollectedAmount"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Customer ADD CollectedAmount [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Setup_Customer", "Collected1Amount"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Customer ADD Collected1Amount [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Setup_Customer", "Collected2Amount"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Customer ADD Collected2Amount [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //if table not found then create table
            if (!CheckTable("Task_CollectionMapping"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_CollectionMapping(
                    [MappingId] [uniqueidentifier] DEFAULT newid() NOT NULL,
                    [CollectionId] [uniqueidentifier] NOT NULL,
                    [SalesOrderId] [uniqueidentifier] NULL,
                    [InvoiceId] [uniqueidentifier] NULL,
                    [Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [Amount1] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [Amount2] [numeric](18, 4) DEFAULT 0 NOT NULL,
                CONSTRAINT [PK_Task_CollectionMapping] PRIMARY KEY CLUSTERED ([MappingId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_CollectionMapping", "FK_Task_CollectionMapping_Task_Collection"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CollectionMapping ADD CONSTRAINT FK_Task_CollectionMapping_Task_Collection FOREIGN KEY (CollectionId) REFERENCES Task_Collection (CollectionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_CollectionMapping", "FK_Task_CollectionMapping_Task_SalesOrder"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CollectionMapping ADD CONSTRAINT FK_Task_CollectionMapping_Task_SalesOrder FOREIGN KEY (SalesOrderId) REFERENCES Task_SalesOrder (SalesOrderId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_CollectionMapping", "FK_Task_CollectionMapping_Task_SalesInvoice"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CollectionMapping ADD CONSTRAINT FK_Task_CollectionMapping_Task_SalesInvoice FOREIGN KEY (InvoiceId) REFERENCES Task_SalesInvoice (InvoiceId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_SalesOrder", "BuyerId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ADD BuyerId bigint NULL");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesOrder", "FK_Task_SalesOrder_Setup_Customer1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ADD CONSTRAINT FK_Task_SalesOrder_Setup_Customer1 FOREIGN KEY (BuyerId) REFERENCES Setup_Customer (CustomerId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_DeliveryChallan", "BuyerId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallan ADD BuyerId bigint NULL");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_DeliveryChallan", "FK_Task_DeliveryChallan_Setup_Customer1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallan ADD CONSTRAINT FK_Task_DeliveryChallan_Setup_Customer1 FOREIGN KEY (BuyerId) REFERENCES Setup_Customer (CustomerId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_SalesInvoice", "BuyerId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD BuyerId bigint NULL");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesInvoice", "FK_Task_SalesInvoice_Setup_Customer1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD CONSTRAINT FK_Task_SalesInvoice_Setup_Customer1 FOREIGN KEY (BuyerId) REFERENCES Setup_Customer (CustomerId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_SalesOrder", "CollectedAmount"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ADD CollectedAmount [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_SalesOrder", "Collected1Amount"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ADD Collected1Amount [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_SalesOrder", "Collected2Amount"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ADD Collected2Amount [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //if table not found then create table
            if (!CheckTable("Task_ItemRequisitionNos"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_ItemRequisitionNos(
                 [Id] [uniqueidentifier] DEFAULT newid() NOT NULL,
                 [RequisitionNo] [varchar](50) NOT NULL,
                 [Year] [bigint] NOT NULL,
                 [CompanyId] [bigint] NOT NULL,
                 CONSTRAINT [PK_Task_ItemRequisitionNos] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add unique key if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ItemRequisitionNos", "IX_Task_ItemRequisitionNos"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ItemRequisitionNos ADD CONSTRAINT IX_Task_ItemRequisitionNos UNIQUE NONCLUSTERED (RequisitionNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ItemRequisitionNos", "FK_Task_ItemRequisitionNos_Setup_Company"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ItemRequisitionNos ADD CONSTRAINT FK_Task_ItemRequisitionNos_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_ItemRequisition"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_ItemRequisition(
                    [RequisitionId] [uniqueidentifier] DEFAULT newid() NOT NULL,
	                [RequisitionNo] [varchar](50) NOT NULL,
	                [RequisitionDate] [datetime] NOT NULL,
                    [RequestedBy] [bigint] NOT NULL,
                    [Remarks] [varchar](1000) NULL,
                    [IsSettled] [bit] DEFAULT 0 NOT NULL,
	                [Approved] [varchar](1) DEFAULT 'N' NOT NULL,
	                [ApprovedBy] [bigint] NULL,
	                [ApprovedDate] [datetime] NULL,
	                [CancelReason] [varchar](200) NULL,
	                [LocationId] [bigint] NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
	                [EntryBy] [bigint] NOT NULL,
	                [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Task_ItemRequisition] PRIMARY KEY CLUSTERED ([RequisitionId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add unique key if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ItemRequisition", "IX_Task_ItemRequisition"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ItemRequisition ADD CONSTRAINT IX_Task_ItemRequisition UNIQUE NONCLUSTERED (RequisitionNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ItemRequisition", "FK_Task_ItemRequisition_Setup_Employee"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ItemRequisition ADD CONSTRAINT FK_Task_ItemRequisition_Setup_Employee FOREIGN KEY (RequestedBy) REFERENCES Setup_Employee (EmployeeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ItemRequisition", "FK_Task_ItemRequisition_Security_User"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ItemRequisition ADD CONSTRAINT FK_Task_ItemRequisition_Security_User FOREIGN KEY (ApprovedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ItemRequisition", "FK_Task_ItemRequisition_Setup_Location"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ItemRequisition ADD CONSTRAINT FK_Task_ItemRequisition_Setup_Location FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ItemRequisition", "FK_Task_ItemRequisition_Setup_Company"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ItemRequisition ADD CONSTRAINT FK_Task_ItemRequisition_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ItemRequisition", "FK_Task_ItemRequisition_Security_User1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ItemRequisition ADD CONSTRAINT FK_Task_ItemRequisition_Security_User1 FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_ItemRequisitionDetail"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_ItemRequisitionDetail(
                    [RequisitionDetailId] [uniqueidentifier] DEFAULT newid() NOT NULL,
	                [RequisitionId] [uniqueidentifier] NOT NULL,
	                [ProductId] [bigint] NOT NULL,
	                [ProductDimensionId] [bigint] NULL,
	                [UnitTypeId] [bigint] NOT NULL,
	                [Quantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [FinalizedQuantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [RequiredDate] [datetime] NULL,
                    [Reason] [varchar](500) NULL,
                 CONSTRAINT [PK_Task_ItemRequisitionDetail] PRIMARY KEY CLUSTERED ([RequisitionDetailId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ItemRequisitionDetail", "FK_Task_ItemRequisitionDetail_Task_ItemRequisition"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ItemRequisitionDetail ADD CONSTRAINT FK_Task_ItemRequisitionDetail_Task_ItemRequisition FOREIGN KEY (RequisitionId) REFERENCES Task_ItemRequisition (RequisitionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ItemRequisitionDetail", "FK_Task_ItemRequisitionDetail_Setup_Product"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ItemRequisitionDetail ADD CONSTRAINT FK_Task_ItemRequisitionDetail_Setup_Product FOREIGN KEY (ProductId) REFERENCES Setup_Product (ProductId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ItemRequisitionDetail", "FK_Task_ItemRequisitionDetail_Setup_ProductDimension"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ItemRequisitionDetail ADD CONSTRAINT FK_Task_ItemRequisitionDetail_Setup_ProductDimension FOREIGN KEY (ProductDimensionId) REFERENCES Setup_ProductDimension (ProductDimensionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ItemRequisitionDetail", "FK_Task_ItemRequisitionDetail_Setup_UnitType"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ItemRequisitionDetail ADD CONSTRAINT FK_Task_ItemRequisitionDetail_Setup_UnitType FOREIGN KEY (UnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_RequisitionFinalizeNos"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_RequisitionFinalizeNos(
                 [Id] [uniqueidentifier] DEFAULT newid() NOT NULL,
                 [RequisitionNo] [varchar](50) NOT NULL,
                 [Year] [bigint] NOT NULL,
                 [CompanyId] [bigint] NOT NULL,
                 CONSTRAINT [PK_Task_RequisitionFinalizeNos] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add unique key if not found
            if (!CheckRelationshipBetweenTwoTables("Task_RequisitionFinalizeNos", "IX_Task_RequisitionFinalizeNos"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_RequisitionFinalizeNos ADD CONSTRAINT IX_Task_RequisitionFinalizeNos UNIQUE NONCLUSTERED (RequisitionNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_RequisitionFinalizeNos", "FK_Task_RequisitionFinalizeNos_Setup_Company"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_RequisitionFinalizeNos ADD CONSTRAINT FK_Task_RequisitionFinalizeNos_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_RequisitionFinalize"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_RequisitionFinalize(
                 [RequisitionId] [uniqueidentifier] DEFAULT newid() NOT NULL,
                 [RequisitionNo] [varchar](50) NOT NULL,
                 [RequisitionDate] [datetime] NOT NULL,
                 [RequisitionBy] [bigint] NOT NULL,
                 [Remarks] [varchar](1000) NULL,
                 [IsSettled] [bit] DEFAULT 0 NOT NULL,
                 [Approved] [varchar](1) DEFAULT 'N' NOT NULL,
                 [ApprovedBy] [bigint] NULL,
                 [ApprovedDate] [datetime] NULL,
                 [CancelReason] [varchar](200) NULL,
                 [LocationId] [bigint] NOT NULL,
                 [CompanyId] [bigint] NOT NULL,
                 [EntryBy] [bigint] NOT NULL,
                 [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Task_RequisitionFinalize] PRIMARY KEY CLUSTERED ([RequisitionId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add unique key if not found
            if (!CheckRelationshipBetweenTwoTables("Task_RequisitionFinalize", "IX_Task_RequisitionFinalize"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_RequisitionFinalize ADD CONSTRAINT IX_Task_RequisitionFinalize UNIQUE NONCLUSTERED (RequisitionNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_RequisitionFinalize", "FK_Task_RequisitionFinalize_Setup_Employee"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_RequisitionFinalize ADD CONSTRAINT FK_Task_RequisitionFinalize_Setup_Employee FOREIGN KEY (RequisitionBy) REFERENCES Setup_Employee (EmployeeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_RequisitionFinalize", "FK_Task_RequisitionFinalize_Security_User"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_RequisitionFinalize ADD CONSTRAINT FK_Task_RequisitionFinalize_Security_User FOREIGN KEY (ApprovedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_RequisitionFinalize", "FK_Task_RequisitionFinalize_Setup_Location"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_RequisitionFinalize ADD CONSTRAINT FK_Task_RequisitionFinalize_Setup_Location FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_RequisitionFinalize", "FK_Task_RequisitionFinalize_Setup_Company"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_RequisitionFinalize ADD CONSTRAINT FK_Task_RequisitionFinalize_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_RequisitionFinalize", "FK_Task_RequisitionFinalize_Security_User1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_RequisitionFinalize ADD CONSTRAINT FK_Task_RequisitionFinalize_Security_User1 FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_RequisitionFinalizeDetail"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_RequisitionFinalizeDetail(
                 [RequisitionDetailId] [uniqueidentifier] DEFAULT newid() NOT NULL,
                 [RequisitionId] [uniqueidentifier] NOT NULL,
                 [ItemRequisitionId] [uniqueidentifier] NULL,
                 [ProductId] [bigint] NOT NULL,
                 [ProductDimensionId] [bigint] NULL,
                 [UnitTypeId] [bigint] NOT NULL,
                 [Quantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
	             [OrderedQuantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
                 CONSTRAINT [PK_Task_RequisitionFinalizeDetail] PRIMARY KEY CLUSTERED ([RequisitionDetailId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_RequisitionFinalizeDetail", "FK_Task_RequisitionFinalizeDetail_Task_RequisitionFinalize"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_RequisitionFinalizeDetail ADD CONSTRAINT FK_Task_RequisitionFinalizeDetail_Task_RequisitionFinalize FOREIGN KEY (RequisitionId) REFERENCES Task_RequisitionFinalize (RequisitionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_RequisitionFinalizeDetail", "FK_Task_RequisitionFinalizeDetail_Task_ItemRequisition"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_RequisitionFinalizeDetail ADD CONSTRAINT FK_Task_RequisitionFinalizeDetail_Task_ItemRequisition FOREIGN KEY (ItemRequisitionId) REFERENCES Task_ItemRequisition (RequisitionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_RequisitionFinalizeDetail", "FK_Task_RequisitionFinalizeDetail_Setup_Product"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_RequisitionFinalizeDetail ADD CONSTRAINT FK_Task_RequisitionFinalizeDetail_Setup_Product FOREIGN KEY (ProductId) REFERENCES Setup_Product (ProductId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_RequisitionFinalizeDetail", "FK_Task_RequisitionFinalizeDetail_Setup_ProductDimension"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_RequisitionFinalizeDetail ADD CONSTRAINT FK_Task_RequisitionFinalizeDetail_Setup_ProductDimension FOREIGN KEY (ProductDimensionId) REFERENCES Setup_ProductDimension (ProductDimensionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_RequisitionFinalizeDetail", "FK_Task_RequisitionFinalizeDetail_Setup_UnitType"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_RequisitionFinalizeDetail ADD CONSTRAINT FK_Task_RequisitionFinalizeDetail_Setup_UnitType FOREIGN KEY (UnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_PurchaseOrderNos"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_PurchaseOrderNos(
                 [Id] [uniqueidentifier] DEFAULT newid() NOT NULL,
                 [OrderNo] [varchar](50) NOT NULL,
                 [Year] [bigint] NOT NULL,
                 [CompanyId] [bigint] NOT NULL,
                 CONSTRAINT [PK_Task_PurchaseOrderNos] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add unique key if not found
            if (!CheckRelationshipBetweenTwoTables("Task_PurchaseOrderNos", "IX_Task_PurchaseOrderNos"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrderNos ADD CONSTRAINT IX_Task_PurchaseOrderNos UNIQUE NONCLUSTERED (OrderNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_PurchaseOrderNos", "FK_Task_PurchaseOrderNos_Setup_Company"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrderNos ADD CONSTRAINT FK_Task_PurchaseOrderNos_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_PurchaseOrder"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_PurchaseOrder(
                 [OrderId] [uniqueidentifier] DEFAULT newid() NOT NULL,
                 [OrderNo] [varchar](50) NOT NULL,
                 [OrderDate] [datetime] NOT NULL,
                 [PurchaseType] [varchar](10) NOT NULL,
                 [SelectedCurrency] [varchar](5) NOT NULL,
	             [Currency1Rate] [numeric](18, 4) DEFAULT 0 NOT NULL,
	             [Currency2Rate] [numeric](18, 4) DEFAULT 0 NOT NULL,
                 [SupplierId] [bigint] NOT NULL,
                 [ReferenceNo] [varchar](50) NULL,
	             [ReferenceDate] [datetime] NULL,
                 [PaymentModeId] [bigint] NOT NULL,
                 [Remarks] [varchar](1000) NULL,
                 [TermsAndConditionsId] [bigint] NULL,
	             [TermsAndConditionsDetail] [varchar](5000) NULL,
                 [PaymentTermsId] [bigint] NULL,
	             [PaymentTermsDetail] [varchar](5000) NULL,
                 [ShipmentType] [varchar](2) NOT NULL,
                 [DeliveryTo] [varchar](100) NULL,
	             [DeliveryContactNo] [varchar](30) NULL,
                 [DeliveryDate] [datetime] NULL,
                 [IsSettled] [bit] DEFAULT 0 NOT NULL,
                 [Approved] [varchar](1) DEFAULT 'N' NOT NULL,
                 [ApprovedBy] [bigint] NULL,
                 [ApprovedDate] [datetime] NULL,
                 [CancelReason] [varchar](200) NULL,
                 [LocationId] [bigint] NOT NULL,
                 [CompanyId] [bigint] NOT NULL,
                 [EntryBy] [bigint] NOT NULL,
                 [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Task_PurchaseOrder] PRIMARY KEY CLUSTERED ([OrderId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add unique key if not found
            if (!CheckRelationshipBetweenTwoTables("Task_PurchaseOrder", "IX_Task_PurchaseOrder"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrder ADD CONSTRAINT IX_Task_PurchaseOrder UNIQUE NONCLUSTERED (OrderNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_PurchaseOrder", "FK_Task_PurchaseOrder_Setup_Supplier"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrder ADD CONSTRAINT FK_Task_PurchaseOrder_Setup_Supplier FOREIGN KEY (SupplierId) REFERENCES Setup_Supplier (SupplierId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_PurchaseOrder", "FK_Task_PurchaseOrder_Configuration_PaymentMode"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrder ADD CONSTRAINT FK_Task_PurchaseOrder_Configuration_PaymentMode FOREIGN KEY (PaymentModeId) REFERENCES Configuration_PaymentMode (PaymentModeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_PurchaseOrder", "FK_Task_PurchaseOrder_Setup_TermsAndConditions"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrder ADD CONSTRAINT FK_Task_PurchaseOrder_Setup_TermsAndConditions FOREIGN KEY (TermsAndConditionsId) REFERENCES Setup_TermsAndConditions (TermsAndConditionsId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_PurchaseOrder", "FK_Task_PurchaseOrder_Setup_TermsAndConditions1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrder ADD CONSTRAINT FK_Task_PurchaseOrder_Setup_TermsAndConditions1 FOREIGN KEY (PaymentTermsId) REFERENCES Setup_TermsAndConditions (TermsAndConditionsId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_PurchaseOrder", "FK_Task_PurchaseOrder_Security_User"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrder ADD CONSTRAINT FK_Task_PurchaseOrder_Security_User FOREIGN KEY (ApprovedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_PurchaseOrder", "FK_Task_PurchaseOrder_Setup_Location"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrder ADD CONSTRAINT FK_Task_PurchaseOrder_Setup_Location FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_PurchaseOrder", "FK_Task_PurchaseOrder_Setup_Company"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrder ADD CONSTRAINT FK_Task_PurchaseOrder_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_PurchaseOrder", "FK_Task_PurchaseOrder_Security_User1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrder ADD CONSTRAINT FK_Task_PurchaseOrder_Security_User1 FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_PurchaseOrderDetail"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_PurchaseOrderDetail(
                 [OrderDetailId] [uniqueidentifier] DEFAULT newid() NOT NULL,
                 [OrderId] [uniqueidentifier] NOT NULL,
                 [RequisitionId] [uniqueidentifier] NULL,
                 [ProductId] [bigint] NOT NULL,
                 [ProductDimensionId] [bigint] NULL,
                 [UnitTypeId] [bigint] NOT NULL,
                 [Quantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
	             [ReceivedQuantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
                 [Price] [numeric](18, 4) DEFAULT 0 NOT NULL,
	             [Price1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	             [Price2] [numeric](18, 4) DEFAULT 0 NOT NULL,
                 CONSTRAINT [PK_Task_PurchaseOrderDetail] PRIMARY KEY CLUSTERED ([OrderDetailId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_PurchaseOrderDetail", "FK_Task_PurchaseOrderDetail_Task_PurchaseOrder"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrderDetail ADD CONSTRAINT FK_Task_PurchaseOrderDetail_Task_PurchaseOrder FOREIGN KEY (OrderId) REFERENCES Task_PurchaseOrder (OrderId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_PurchaseOrderDetail", "FK_Task_PurchaseOrderDetail_Task_RequisitionFinalize"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrderDetail ADD CONSTRAINT FK_Task_PurchaseOrderDetail_Task_RequisitionFinalize FOREIGN KEY (RequisitionId) REFERENCES Task_RequisitionFinalize (RequisitionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_PurchaseOrderDetail", "FK_Task_PurchaseOrderDetail_Setup_Product"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrderDetail ADD CONSTRAINT FK_Task_PurchaseOrderDetail_Setup_Product FOREIGN KEY (ProductId) REFERENCES Setup_Product (ProductId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_PurchaseOrderDetail", "FK_Task_PurchaseOrderDetail_Setup_ProductDimension"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrderDetail ADD CONSTRAINT FK_Task_PurchaseOrderDetail_Setup_ProductDimension FOREIGN KEY (ProductDimensionId) REFERENCES Setup_ProductDimension (ProductDimensionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_PurchaseOrderDetail", "FK_Task_PurchaseOrderDetail_Setup_UnitType"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrderDetail ADD CONSTRAINT FK_Task_PurchaseOrderDetail_Setup_UnitType FOREIGN KEY (UnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Setup_Measurement", "LengthValue"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Measurement ADD LengthValue [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Setup_Measurement", "WidthValue"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Measurement ADD WidthValue [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Setup_Measurement", "HeightValue"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Measurement ADD HeightValue [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //if table not found then create table
            if (!CheckTable("Task_GoodsReceiveNos"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_GoodsReceiveNos(
                 [Id] [uniqueidentifier] DEFAULT newid() NOT NULL,
                 [ReceiveNo] [varchar](50) NOT NULL,
                 [Year] [bigint] NOT NULL,
                 [CompanyId] [bigint] NOT NULL,
                 CONSTRAINT [PK_Task_GoodsReceiveNos] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add unique key if not found
            if (!CheckRelationshipBetweenTwoTables("Task_GoodsReceiveNos", "IX_Task_GoodsReceiveNos"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceiveNos ADD CONSTRAINT IX_Task_GoodsReceiveNos UNIQUE NONCLUSTERED (ReceiveNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_GoodsReceiveNos", "FK_Task_GoodsReceiveNos_Setup_Company"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceiveNos ADD CONSTRAINT FK_Task_GoodsReceiveNos_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_GoodsReceive"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_GoodsReceive(
                 [ReceiveId] [uniqueidentifier] DEFAULT newid() NOT NULL,
                 [ReceiveNo] [varchar](50) NOT NULL,
                 [ReceiveDate] [datetime] NOT NULL,
                 [ReferenceNo] [varchar](50) NULL,
	             [ReferenceDate] [datetime] NULL,
                 [SupplierId] [bigint] NOT NULL,
                 [OrderId] [uniqueidentifier] NOT NULL,
                 [Remarks] [varchar](1000) NULL,
                 [SelectedCurrency] [varchar](5) NOT NULL,
	             [Currency1Rate] [numeric](18, 4) DEFAULT 0 NOT NULL,
	             [Currency2Rate] [numeric](18, 4) DEFAULT 0 NOT NULL,
                 [IsSettled] [bit] DEFAULT 0 NOT NULL,
                 [Approved] [varchar](1) DEFAULT 'N' NOT NULL,
                 [ApprovedBy] [bigint] NULL,
                 [ApprovedDate] [datetime] NULL,
                 [CancelReason] [varchar](200) NULL,
                 [VoucherId] [uniqueidentifier] NULL,
                 [LocationId] [bigint] NOT NULL,
                 [CompanyId] [bigint] NOT NULL,
                 [EntryBy] [bigint] NOT NULL,
                 [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Task_GoodsReceive] PRIMARY KEY CLUSTERED ([ReceiveId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add unique key if not found
            if (!CheckRelationshipBetweenTwoTables("Task_GoodsReceive", "IX_Task_GoodsReceive"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceive ADD CONSTRAINT IX_Task_GoodsReceive UNIQUE NONCLUSTERED (ReceiveNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_GoodsReceive", "FK_Task_GoodsReceive_Setup_Supplier"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceive ADD CONSTRAINT FK_Task_GoodsReceive_Setup_Supplier FOREIGN KEY (SupplierId) REFERENCES Setup_Supplier (SupplierId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_GoodsReceive", "FK_Task_GoodsReceive_Task_PurchaseOrder"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceive ADD CONSTRAINT FK_Task_GoodsReceive_Task_PurchaseOrder FOREIGN KEY (OrderId) REFERENCES Task_PurchaseOrder (OrderId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_GoodsReceive", "FK_Task_GoodsReceive_Security_User"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceive ADD CONSTRAINT FK_Task_GoodsReceive_Security_User FOREIGN KEY (ApprovedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_GoodsReceive", "FK_Task_GoodsReceive_Task_Voucher"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceive ADD CONSTRAINT FK_Task_GoodsReceive_Task_Voucher FOREIGN KEY (VoucherId) REFERENCES Task_Voucher (VoucherId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_GoodsReceive", "FK_Task_GoodsReceive_Setup_Location"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceive ADD CONSTRAINT FK_Task_GoodsReceive_Setup_Location FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_GoodsReceive", "FK_Task_GoodsReceive_Setup_Company"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceive ADD CONSTRAINT FK_Task_GoodsReceive_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_GoodsReceive", "FK_Task_GoodsReceive_Security_User1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceive ADD CONSTRAINT FK_Task_GoodsReceive_Security_User1 FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_GoodsReceiveDetail"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_GoodsReceiveDetail(
                 [ReceiveDetailId] [uniqueidentifier] DEFAULT newid() NOT NULL,
                 [ReceiveId] [uniqueidentifier] NOT NULL,
                 [ProductId] [bigint] NOT NULL,
                 [ProductDimensionId] [bigint] NULL,
                 [UnitTypeId] [bigint] NOT NULL,
                 [WarehouseId] [bigint] NULL,
                 [Quantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
	             [FinalizedQuantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
                 [Price] [numeric](18, 4) DEFAULT 0 NOT NULL,
	             [Price1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	             [Price2] [numeric](18, 4) DEFAULT 0 NOT NULL,
                 CONSTRAINT [PK_Task_GoodsReceiveDetail] PRIMARY KEY CLUSTERED ([ReceiveDetailId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_GoodsReceiveDetail", "FK_Task_GoodsReceiveDetail_Task_GoodsReceive"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceiveDetail ADD CONSTRAINT FK_Task_GoodsReceiveDetail_Task_GoodsReceive FOREIGN KEY (ReceiveId) REFERENCES Task_GoodsReceive (ReceiveId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_GoodsReceiveDetail", "FK_Task_GoodsReceiveDetail_Setup_Product"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceiveDetail ADD CONSTRAINT FK_Task_GoodsReceiveDetail_Setup_Product FOREIGN KEY (ProductId) REFERENCES Setup_Product (ProductId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_GoodsReceiveDetail", "FK_Task_GoodsReceiveDetail_Setup_ProductDimension"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceiveDetail ADD CONSTRAINT FK_Task_GoodsReceiveDetail_Setup_ProductDimension FOREIGN KEY (ProductDimensionId) REFERENCES Setup_ProductDimension (ProductDimensionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_GoodsReceiveDetail", "FK_Task_GoodsReceiveDetail_Setup_UnitType"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceiveDetail ADD CONSTRAINT FK_Task_GoodsReceiveDetail_Setup_UnitType FOREIGN KEY (UnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_GoodsReceiveDetail", "FK_Task_GoodsReceiveDetail_Setup_Location"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceiveDetail ADD CONSTRAINT FK_Task_GoodsReceiveDetail_Setup_Location FOREIGN KEY (WarehouseId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_ReceiveFinalizeNos"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_ReceiveFinalizeNos(
                 [Id] [uniqueidentifier] DEFAULT newid() NOT NULL,
                 [FinalizeNo] [varchar](50) NOT NULL,
                 [Year] [bigint] NOT NULL,
                 [CompanyId] [bigint] NOT NULL,
                 CONSTRAINT [PK_Task_ReceiveFinalizeNos] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add unique key if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ReceiveFinalizeNos", "IX_Task_ReceiveFinalizeNos"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalizeNos ADD CONSTRAINT IX_Task_ReceiveFinalizeNos UNIQUE NONCLUSTERED (FinalizeNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ReceiveFinalizeNos", "FK_Task_ReceiveFinalizeNos_Setup_Company"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalizeNos ADD CONSTRAINT FK_Task_ReceiveFinalizeNos_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_ReceiveFinalize"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_ReceiveFinalize(
                    [FinalizeId] [uniqueidentifier] DEFAULT newid() NOT NULL,
	                [FinalizeNo] [varchar](50) NOT NULL,
                    [FinalizeDate] [datetime] NOT NULL,
	                [SupplierId] [bigint] NOT NULL,
	                [SelectedCurrency] [varchar](5) NOT NULL,
	                [FinalizeAmount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Finalize1Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Finalize2Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Approved] [varchar](1) DEFAULT 'N' NOT NULL,
	                [ApprovedBy] [bigint] NULL,
	                [ApprovedDate] [datetime] NULL,
	                [CancelReason] [varchar](200) NULL,
                    [VoucherId] [uniqueidentifier] NULL,
                    [IsSettled] [bit] DEFAULT 0 NOT NULL,
                    [PaidAmount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Paid1Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Paid2Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [LocationId] [bigint] NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
	                [EntryBy] [bigint] NOT NULL,
	                [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Task_ReceiveFinalize] PRIMARY KEY CLUSTERED ([FinalizeId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add unique key if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ReceiveFinalize", "IX_Task_ReceiveFinalize"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalize ADD CONSTRAINT IX_Task_ReceiveFinalize UNIQUE NONCLUSTERED (FinalizeNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ReceiveFinalize", "FK_Task_ReceiveFinalize_Setup_Supplier"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalize ADD CONSTRAINT FK_Task_ReceiveFinalize_Setup_Supplier FOREIGN KEY (SupplierId) REFERENCES Setup_Supplier (SupplierId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ReceiveFinalize", "FK_Task_ReceiveFinalize_Security_User"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalize ADD CONSTRAINT FK_Task_ReceiveFinalize_Security_User FOREIGN KEY (ApprovedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ReceiveFinalize", "FK_Task_ReceiveFinalize_Task_Voucher"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalize ADD CONSTRAINT FK_Task_ReceiveFinalize_Task_Voucher FOREIGN KEY (VoucherId) REFERENCES Task_Voucher (VoucherId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ReceiveFinalize", "FK_Task_ReceiveFinalize_Setup_Location"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalize ADD CONSTRAINT FK_Task_ReceiveFinalize_Setup_Location FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ReceiveFinalize", "FK_Task_ReceiveFinalize_Setup_Company"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalize ADD CONSTRAINT FK_Task_ReceiveFinalize_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ReceiveFinalize", "FK_Task_ReceiveFinalize_Security_User1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalize ADD CONSTRAINT FK_Task_ReceiveFinalize_Security_User1 FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_ReceiveFinalizeDetail"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_ReceiveFinalizeDetail(
                 [FinalizeDetailId] [uniqueidentifier] DEFAULT newid() NOT NULL,
                 [FinalizeId] [uniqueidentifier] NOT NULL,
                 [ReceiveId] [uniqueidentifier] NOT NULL,
                 [ProductId] [bigint] NOT NULL,
                 [ProductDimensionId] [bigint] NULL,
                 [UnitTypeId] [bigint] NOT NULL,
                 [Quantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
                 [Price] [numeric](18, 4) DEFAULT 0 NOT NULL,
	             [Price1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	             [Price2] [numeric](18, 4) DEFAULT 0 NOT NULL,
                 CONSTRAINT [PK_Task_ReceiveFinalizeDetail] PRIMARY KEY CLUSTERED ([FinalizeDetailId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ReceiveFinalizeDetail", "FK_Task_ReceiveFinalizeDetail_Task_ReceiveFinalize"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalizeDetail ADD CONSTRAINT FK_Task_ReceiveFinalizeDetail_Task_ReceiveFinalize FOREIGN KEY (FinalizeId) REFERENCES Task_ReceiveFinalize (FinalizeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ReceiveFinalizeDetail", "FK_Task_ReceiveFinalizeDetail_Task_GoodsReceive"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalizeDetail ADD CONSTRAINT FK_Task_ReceiveFinalizeDetail_Task_GoodsReceive FOREIGN KEY (ReceiveId) REFERENCES Task_GoodsReceive (ReceiveId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ReceiveFinalizeDetail", "FK_Task_ReceiveFinalizeDetail_Setup_Product"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalizeDetail ADD CONSTRAINT FK_Task_ReceiveFinalizeDetail_Setup_Product FOREIGN KEY (ProductId) REFERENCES Setup_Product (ProductId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ReceiveFinalizeDetail", "FK_Task_ReceiveFinalizeDetail_Setup_ProductDimension"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalizeDetail ADD CONSTRAINT FK_Task_ReceiveFinalizeDetail_Setup_ProductDimension FOREIGN KEY (ProductDimensionId) REFERENCES Setup_ProductDimension (ProductDimensionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ReceiveFinalizeDetail", "FK_Task_ReceiveFinalizeDetail_Setup_UnitType"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalizeDetail ADD CONSTRAINT FK_Task_ReceiveFinalizeDetail_Setup_UnitType FOREIGN KEY (UnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_PurchaseOrder", "OrderAmount"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrder ADD OrderAmount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrder ADD Order1Amount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrder ADD Order2Amount [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_PurchaseOrder", "PaidAmount"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrder ADD PaidAmount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrder ADD Paid1Amount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrder ADD Paid2Amount [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Setup_Supplier", "PaidAmount"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Supplier ADD PaidAmount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Supplier ADD Paid1Amount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Supplier ADD Paid2Amount [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //if table not found then create table
            if (!CheckTable("Task_PaymentNos"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_PaymentNos(
                 [Id] [uniqueidentifier] DEFAULT newid() NOT NULL,
                 [PaymentNo] [varchar](50) NOT NULL,
                 [Year] [bigint] NOT NULL,
                 [CompanyId] [bigint] NOT NULL,
                 CONSTRAINT [PK_Task_PaymentNos] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add unique key if not found
            if (!CheckRelationshipBetweenTwoTables("Task_PaymentNos", "IX_Task_PaymentNos"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PaymentNos ADD CONSTRAINT IX_Task_PaymentNos UNIQUE NONCLUSTERED (PaymentNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_PaymentNos", "FK_Task_PaymentNos_Setup_Company"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PaymentNos ADD CONSTRAINT FK_Task_PaymentNos_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_Payment"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_Payment(
                    [PaymentId] [uniqueidentifier] DEFAULT newid() NOT NULL,
	                [PaymentNo] [varchar](50) NOT NULL,
	                [PaymentDate] [datetime] NOT NULL,
	                [SelectedCurrency] [varchar](5) NOT NULL,
	                [Currency1Rate] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Currency2Rate] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [PaidAmount] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [PaidAmount1] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [PaidAmount2] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [SupplierId] [bigint] NOT NULL,
                    [PaidBy] [bigint] NOT NULL,
                    [ReferenceNo] [varchar](50) NULL,
                    [Remarks] [varchar](1000) NULL,
                    [OperationTypeId] [bigint] NOT NULL,
                    [OperationalEventId] [bigint] NOT NULL,
	                [Approved] [varchar](1) DEFAULT 'N' NOT NULL,
	                [ApprovedBy] [bigint] NULL,
	                [ApprovedDate] [datetime] NULL,
	                [CancelReason] [varchar](200) NULL,
	                [LocationId] [bigint] NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
	                [EntryBy] [bigint] NOT NULL,
	                [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Task_Payment] PRIMARY KEY CLUSTERED ([PaymentId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_Payment", "FK_Task_Payment_Setup_Supplier"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Payment ADD CONSTRAINT FK_Task_Payment_Setup_Supplier FOREIGN KEY (SupplierId) REFERENCES Setup_Supplier (SupplierId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_Payment", "FK_Task_Payment_Security_User"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Payment ADD CONSTRAINT FK_Task_Payment_Security_User FOREIGN KEY (PaidBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_Payment", "FK_Task_Payment_Configuration_OperationType"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Payment ADD CONSTRAINT FK_Task_Payment_Configuration_OperationType FOREIGN KEY (OperationTypeId) REFERENCES Configuration_OperationType (OperationTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_Payment", "FK_Task_Payment_Configuration_OperationalEvent"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Payment ADD CONSTRAINT FK_Task_Payment_Configuration_OperationalEvent FOREIGN KEY (OperationalEventId) REFERENCES Configuration_OperationalEvent (OperationalEventId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_Payment", "FK_Task_Payment_Security_User1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Payment ADD CONSTRAINT FK_Task_Payment_Security_User1 FOREIGN KEY (ApprovedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_Payment", "FK_Task_Payment_Setup_Location"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Payment ADD CONSTRAINT FK_Task_Payment_Setup_Location FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_Payment", "FK_Task_Payment_Setup_Company"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Payment ADD CONSTRAINT FK_Task_Payment_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_Payment", "FK_Task_Payment_Security_User2"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Payment ADD CONSTRAINT FK_Task_Payment_Security_User2 FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_PaymentDetail"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_PaymentDetail(
                    [PaymentDetailId] [uniqueidentifier] DEFAULT newid() NOT NULL,
                    [PaymentId] [uniqueidentifier] NOT NULL,
                    [PaymentModeId] [bigint] NOT NULL,
                    [Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [Amount1] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [Amount2] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [VoucherId] [uniqueidentifier] NULL,
                CONSTRAINT [PK_Task_PaymentDetail] PRIMARY KEY CLUSTERED ([PaymentDetailId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_PaymentDetail", "FK_Task_PaymentDetail_Task_Payment"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PaymentDetail ADD CONSTRAINT FK_Task_PaymentDetail_Task_Payment FOREIGN KEY (PaymentId) REFERENCES Task_Payment (PaymentId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_PaymentDetail", "FK_Task_PaymentDetail_Configuration_PaymentMode"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PaymentDetail ADD CONSTRAINT FK_Task_PaymentDetail_Configuration_PaymentMode FOREIGN KEY (PaymentModeId) REFERENCES Configuration_PaymentMode (PaymentModeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_PaymentDetail", "FK_Task_PaymentDetail_Task_Voucher"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PaymentDetail ADD CONSTRAINT FK_Task_PaymentDetail_Task_Voucher FOREIGN KEY (VoucherId) REFERENCES Task_Voucher (VoucherId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_PaymentMapping"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_PaymentMapping(
                    [MappingId] [uniqueidentifier] DEFAULT newid() NOT NULL,
                    [PaymentId] [uniqueidentifier] NOT NULL,
                    [OrderId] [uniqueidentifier] NULL,
                    [FinalizeId] [uniqueidentifier] NULL,
                    [Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [Amount1] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [Amount2] [numeric](18, 4) DEFAULT 0 NOT NULL,
                CONSTRAINT [PK_Task_PaymentMapping] PRIMARY KEY CLUSTERED ([MappingId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_PaymentMapping", "FK_Task_PaymentMapping_Task_Payment"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PaymentMapping ADD CONSTRAINT FK_Task_PaymentMapping_Task_Payment FOREIGN KEY (PaymentId) REFERENCES Task_Payment (PaymentId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_PaymentMapping", "FK_Task_PaymentMapping_Task_PurchaseOrder"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PaymentMapping ADD CONSTRAINT FK_Task_PaymentMapping_Task_PurchaseOrder FOREIGN KEY (OrderId) REFERENCES Task_PurchaseOrder (OrderId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_PaymentMapping", "FK_Task_PaymentMapping_Task_ReceiveFinalize"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PaymentMapping ADD CONSTRAINT FK_Task_PaymentMapping_Task_ReceiveFinalize FOREIGN KEY (FinalizeId) REFERENCES Task_ReceiveFinalize (FinalizeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_ChequeInfo", "PaymentDetailId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ChequeInfo ADD PaymentDetailId [uniqueidentifier] NULL");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ChequeInfo", "FK_Task_ChequeInfo_Task_PaymentDetail"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ChequeInfo ADD CONSTRAINT FK_Task_ChequeInfo_Task_PaymentDetail FOREIGN KEY (PaymentDetailId) REFERENCES Task_PaymentDetail (PaymentDetailId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Stock_CurrentStockSerial"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Stock_CurrentStockSerial(
                    [CurrentStockSerialId] [uniqueidentifier] NOT NULL,
	                [CurrentStockId] [uniqueidentifier] NOT NULL,
	                [Serial] [varchar](100) NOT NULL,
	                [AdditionalSerial] [varchar](100) NULL,
                CONSTRAINT [PK_Stock_CurrentStockSerial] PRIMARY KEY CLUSTERED ([CurrentStockSerialId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Stock_CurrentStockSerial", "FK_Stock_CurrentStockSerial_Stock_CurrentStock"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_CurrentStockSerial ADD CONSTRAINT FK_Stock_CurrentStockSerial_Stock_CurrentStock FOREIGN KEY (CurrentStockId) REFERENCES Stock_CurrentStock (CurrentStockId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add unique key if not found
            if (!CheckRelationshipBetweenTwoTables("Stock_CurrentStockSerial", "IX_Stock_CurrentStockSerial"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_CurrentStockSerial ADD CONSTRAINT IX_Stock_CurrentStockSerial UNIQUE NONCLUSTERED (CurrentStockId ASC, Serial ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //add column if not found
            if (!CheckTable("Stock_CurrentStock", "BatchNo"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_CurrentStock ADD BatchNo [varchar](50) NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_CurrentStock ADD ManufactureDate [datetime] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_CurrentStock ADD ExpireDate [datetime] NULL");
            }

            //if table not found then create table
            if (!CheckTable("Task_GoodsReceiveDetailSerial"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_GoodsReceiveDetailSerial(
                    [ReceiveDetailSerialId] [uniqueidentifier] NOT NULL,
	                [ReceiveDetailId] [uniqueidentifier] NOT NULL,
	                [Serial] [varchar](100) NOT NULL,
	                [AdditionalSerial] [varchar](100) NULL,
                CONSTRAINT [PK_Task_GoodsReceiveDetailSerial] PRIMARY KEY CLUSTERED ([ReceiveDetailSerialId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_GoodsReceiveDetailSerial", "FK_Task_GoodsReceiveDetailSerial_Task_GoodsReceiveDetail"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceiveDetailSerial ADD CONSTRAINT FK_Task_GoodsReceiveDetailSerial_Task_GoodsReceiveDetail FOREIGN KEY (ReceiveDetailId) REFERENCES Task_GoodsReceiveDetail (ReceiveDetailId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_GoodsReceiveDetail", "BatchNo"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceiveDetail ADD BatchNo [varchar](50) NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceiveDetail ADD ManufactureDate [datetime] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceiveDetail ADD ExpireDate [datetime] NULL");
            }

            //if table not found then create table
            if (!CheckTable("Task_DeliveryChallanDetailSerial"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_DeliveryChallanDetailSerial(
                    [ChallanDetailSerialId] [uniqueidentifier] NOT NULL,
	                [ChallanDetailId] [uniqueidentifier] NOT NULL,
	                [Serial] [varchar](100) NOT NULL,
	                [AdditionalSerial] [varchar](100) NULL,
                CONSTRAINT [PK_Task_DeliveryChallanDetailSerial] PRIMARY KEY CLUSTERED ([ChallanDetailSerialId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_DeliveryChallanDetailSerial", "FK_Task_DeliveryChallanDetailSerial_Task_DeliveryChallanDetail"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetailSerial ADD CONSTRAINT FK_Task_DeliveryChallanDetailSerial_Task_DeliveryChallanDetail FOREIGN KEY (ChallanDetailId) REFERENCES Task_DeliveryChallanDetail (ChallanDetailId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_DeliveryChallanDetail", "BatchNo"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD BatchNo [varchar](50) NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD ManufactureDate [datetime] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD ExpireDate [datetime] NULL");
            }

            //add column if not found
            if (!CheckTable("Setup_Accounts", "CategorizationId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Accounts ADD CategorizationId [tinyint] NULL");
            }            

            //add column if not found
            if (!CheckTable("Task_Voucher", "OperationType"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Voucher ADD OperationType [varchar](5) NULL");
            }

            //add column if not found
            if (!CheckTable("Setup_Bank", "Branch"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Bank ADD Branch [varchar](100) NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Bank ADD BankAccountNo [varchar](50) NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Bank ADD IsOwnBank [bit] DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Bank ADD AccountsId [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Bank ADD EditedBy [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Bank ADD EditedDate [datetime] NULL");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Setup_Bank", "FK_Setup_Bank_Setup_Accounts"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Bank ADD CONSTRAINT FK_Setup_Bank_Setup_Accounts FOREIGN KEY (AccountsId) REFERENCES Setup_Accounts (AccountsId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Setup_Bank", "FK_Setup_Bank_Security_User1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Bank ADD CONSTRAINT FK_Setup_Bank_Security_User1 FOREIGN KEY (EditedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Configuration_Code"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Configuration_Code(
	                [Id] [bigint] NOT NULL,
	                [FormName] [varchar](50) NOT NULL,
	                [IsAutoCode] [bit] DEFAULT ((0)) NOT NULL,
	                [Prefix] [varchar](10) NULL,
	                [FullCodeLength] [int] NOT NULL,
	                [IsCodeVisible] [bit] DEFAULT ((0)) NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
                 CONSTRAINT [PK_Configuration_Code] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Length should not more than specific table code length' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Configuration_Code', @level2type=N'COLUMN',@level2name=N'FullCodeLength'");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Configuration_Code", "FK_Configuration_Code_Setup_Company"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_Code ADD CONSTRAINT FK_Configuration_Code_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add unique key if not found
            if (!CheckRelationshipBetweenTwoTables("Configuration_Code", "IX_Configuration_Code"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_Code ADD CONSTRAINT IX_Configuration_Code UNIQUE NONCLUSTERED (FormName ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //add column if not found
            if (!CheckTable("Setup_ProductGroup", "EditedBy"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_ProductGroup ADD EditedBy [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_ProductGroup ADD EditedDate [datetime] NULL");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Setup_ProductGroup", "FK_Setup_ProductGroup_Security_User1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_ProductGroup ADD CONSTRAINT FK_Setup_ProductGroup_Security_User1 FOREIGN KEY (EditedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Setup_ProductSubGroup", "EditedBy"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_ProductSubGroup ADD EditedBy [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_ProductSubGroup ADD EditedDate [datetime] NULL");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Setup_ProductSubGroup", "FK_Setup_ProductSubGroup_Security_User1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_ProductSubGroup ADD CONSTRAINT FK_Setup_ProductSubGroup_Security_User1 FOREIGN KEY (EditedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Setup_Brand", "EditedBy"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Brand ADD EditedBy [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Brand ADD EditedDate [datetime] NULL");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Setup_Brand", "FK_Setup_Brand_Security_User1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Brand ADD CONSTRAINT FK_Setup_Brand_Security_User1 FOREIGN KEY (EditedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Setup_ProductCategory", "EditedBy"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_ProductCategory ADD EditedBy [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_ProductCategory ADD EditedDate [datetime] NULL");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Setup_ProductCategory", "FK_Setup_ProductCategory_Security_User1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_ProductCategory ADD CONSTRAINT FK_Setup_ProductCategory_Security_User1 FOREIGN KEY (EditedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Setup_Product", "EditedBy"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Product ADD EditedBy [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Product ADD EditedDate [datetime] NULL");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Setup_Product", "FK_Setup_Product_Security_User1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Product ADD CONSTRAINT FK_Setup_Product_Security_User1 FOREIGN KEY (EditedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Setup_Customer", "EditedBy"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Customer ADD EditedBy [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Customer ADD EditedDate [datetime] NULL");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Setup_Customer", "FK_Setup_Customer_Security_User1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Customer ADD CONSTRAINT FK_Setup_Customer_Security_User1 FOREIGN KEY (EditedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Setup_Supplier", "EditedBy"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Supplier ADD EditedBy [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Supplier ADD EditedDate [datetime] NULL");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Setup_Supplier", "FK_Setup_Supplier_Security_User1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Supplier ADD CONSTRAINT FK_Setup_Supplier_Security_User1 FOREIGN KEY (EditedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_StockAdjustmentDetailSerial"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_StockAdjustmentDetailSerial(
                    [AdjustmentDetailSerialId] [uniqueidentifier] NOT NULL,
	                [AdjustmentDetailId] [uniqueidentifier] NOT NULL,
	                [Serial] [varchar](100) NOT NULL,
	                [AdditionalSerial] [varchar](100) NULL,
                CONSTRAINT [PK_Task_StockAdjustmentDetailSerial] PRIMARY KEY CLUSTERED ([AdjustmentDetailSerialId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_StockAdjustmentDetailSerial", "FK_Task_StockAdjustmentDetailSerial_Task_StockAdjustmentDetail"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_StockAdjustmentDetailSerial ADD CONSTRAINT FK_Task_StockAdjustmentDetailSerial_Task_StockAdjustmentDetail FOREIGN KEY (AdjustmentDetailId) REFERENCES Task_StockAdjustmentDetail (AdjustmentDetailId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //drop relationship if found
            if (CheckRelationshipBetweenTwoTables("Task_StockAdjustment", "FK_Task_StockAdjustment_Setup_Location1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_StockAdjustment DROP CONSTRAINT FK_Task_StockAdjustment_Setup_Location1");
            }

            //drop column if found
            if (CheckTable("Task_StockAdjustment", "WareHouseId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_StockAdjustment DROP COLUMN WareHouseId");
            }

            //add column if not found
            if (!CheckTable("Task_StockAdjustment", "RequestedBy"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_StockAdjustment ADD RequestedBy [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_StockAdjustment ADD Remarks [varchar](1000) NULL");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_StockAdjustment", "FK_Task_StockAdjustment_Setup_Employee"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_StockAdjustment ADD CONSTRAINT FK_Task_StockAdjustment_Setup_Employee FOREIGN KEY (RequestedBy) REFERENCES Setup_Employee (EmployeeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_StockAdjustmentDetail", "WareHouseId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_StockAdjustmentDetail ADD WareHouseId [bigint] NULL");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_StockAdjustmentDetail", "FK_Task_StockAdjustmentDetail_Setup_Location"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_StockAdjustmentDetail ADD CONSTRAINT FK_Task_StockAdjustmentDetail_Setup_Location FOREIGN KEY (WareHouseId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_StockAdjustmentDetail", "PrimaryUnitTypeId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_StockAdjustmentDetail ADD PrimaryUnitTypeId [bigint] NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_StockAdjustmentDetail ADD SecondaryUnitTypeId [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_StockAdjustmentDetail ADD TertiaryUnitTypeId [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_StockAdjustmentDetail ADD SecondaryConversionRatio [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_StockAdjustmentDetail ADD TertiaryConversionRatio [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_StockAdjustmentDetail", "FK_Task_StockAdjustmentDetail_Setup_UnitType1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_StockAdjustmentDetail ADD CONSTRAINT FK_Task_StockAdjustmentDetail_Setup_UnitType1 FOREIGN KEY (PrimaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_StockAdjustmentDetail ADD CONSTRAINT FK_Task_StockAdjustmentDetail_Setup_UnitType2 FOREIGN KEY (SecondaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_StockAdjustmentDetail ADD CONSTRAINT FK_Task_StockAdjustmentDetail_Setup_UnitType3 FOREIGN KEY (TertiaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Setup_Customer", "TransactionType"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Customer ADD TransactionType [varchar](10) DEFAULT 'Both' NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Setup_Customer", "IsWalkIn"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Customer ADD IsWalkIn [bit] DEFAULT ((0)) NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Configuration_PaymentMode", "AutoHonor"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_PaymentMode ADD AutoHonor [bit] DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_SalesOrderDetail", "PrimaryUnitTypeId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrderDetail ADD PrimaryUnitTypeId [bigint] NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrderDetail ADD SecondaryUnitTypeId [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrderDetail ADD TertiaryUnitTypeId [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrderDetail ADD SecondaryConversionRatio [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrderDetail ADD TertiaryConversionRatio [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesOrderDetail", "FK_Task_SalesOrderDetail_Setup_UnitType1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrderDetail ADD CONSTRAINT FK_Task_SalesOrderDetail_Setup_UnitType1 FOREIGN KEY (PrimaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrderDetail ADD CONSTRAINT FK_Task_SalesOrderDetail_Setup_UnitType2 FOREIGN KEY (SecondaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrderDetail ADD CONSTRAINT FK_Task_SalesOrderDetail_Setup_UnitType3 FOREIGN KEY (TertiaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_DeliveryChallanDetail", "PrimaryUnitTypeId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD PrimaryUnitTypeId [bigint] NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD SecondaryUnitTypeId [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD TertiaryUnitTypeId [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD SecondaryConversionRatio [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD TertiaryConversionRatio [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_DeliveryChallanDetail", "FK_Task_DeliveryChallanDetail_Setup_UnitType1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD CONSTRAINT FK_Task_DeliveryChallanDetail_Setup_UnitType1 FOREIGN KEY (PrimaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD CONSTRAINT FK_Task_DeliveryChallanDetail_Setup_UnitType2 FOREIGN KEY (SecondaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD CONSTRAINT FK_Task_DeliveryChallanDetail_Setup_UnitType3 FOREIGN KEY (TertiaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_SalesInvoiceDetail", "PrimaryUnitTypeId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD PrimaryUnitTypeId [bigint] NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD SecondaryUnitTypeId [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD TertiaryUnitTypeId [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD SecondaryConversionRatio [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD TertiaryConversionRatio [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesInvoiceDetail", "FK_Task_SalesInvoiceDetail_Setup_UnitType1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD CONSTRAINT FK_Task_SalesInvoiceDetail_Setup_UnitType1 FOREIGN KEY (PrimaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD CONSTRAINT FK_Task_SalesInvoiceDetail_Setup_UnitType2 FOREIGN KEY (SecondaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD CONSTRAINT FK_Task_SalesInvoiceDetail_Setup_UnitType3 FOREIGN KEY (TertiaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_SalesInvoice", "InvoiceOperationType"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD InvoiceOperationType [varchar](1) DEFAULT 'R' NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD TermsAndConditionsId [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD TermsAndConditionsDetail [varchar](5000) NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD CommissionType [varchar](1) DEFAULT 'A' NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD CommissionValue [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD CommissionAmount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD Commission1Amount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD Commission2Amount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD InvoiceDiscountType [varchar](1) DEFAULT 'A' NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD InvoiceDiscountValue [numeric](18, 4) DEFAULT 0 NOT NULL");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'R = Regular Sales, D = Direct Sales' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_SalesInvoice', @level2type=N'COLUMN',@level2name=N'InvoiceOperationType'");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A = Amount, P = Percentage' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_SalesInvoice', @level2type=N'COLUMN',@level2name=N'CommissionType'");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A = Amount, P = Percentage' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_SalesInvoice', @level2type=N'COLUMN',@level2name=N'InvoiceDiscountType'");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesInvoice", "FK_Task_SalesInvoice_Setup_TermsAndConditions"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD CONSTRAINT FK_Task_SalesInvoice_Setup_TermsAndConditions FOREIGN KEY (TermsAndConditionsId) REFERENCES Setup_TermsAndConditions (TermsAndConditionsId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //drop relationship if found
            if (CheckRelationshipBetweenTwoTables("Task_DeliveryChallan", "FK_Task_DeliveryChallan_Setup_Location1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallan DROP CONSTRAINT FK_Task_DeliveryChallan_Setup_Location1");
            }

            //drop column if found
            if (CheckTable("Task_DeliveryChallan", "WareHouseId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallan DROP COLUMN WareHouseId");
            }

            //add column if not found
            if (!CheckTable("Task_DeliveryChallanDetail", "WareHouseId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD WareHouseId [bigint] NULL");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_DeliveryChallanDetail", "FK_Task_DeliveryChallanDetail_Setup_Location"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD CONSTRAINT FK_Task_DeliveryChallanDetail_Setup_Location FOREIGN KEY (WareHouseId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_SalesInvoiceDetail", "WareHouseId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD WareHouseId [bigint] NULL");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesInvoiceDetail", "FK_Task_SalesInvoiceDetail_Setup_Location"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD CONSTRAINT FK_Task_SalesInvoiceDetail_Setup_Location FOREIGN KEY (WareHouseId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_PurchaseOrderDetail", "Discount"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrderDetail ADD Discount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrderDetail ADD Discount1 [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrderDetail ADD Discount2 [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrderDetail ADD PrimaryUnitTypeId [bigint] NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrderDetail ADD SecondaryUnitTypeId [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrderDetail ADD TertiaryUnitTypeId [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrderDetail ADD SecondaryConversionRatio [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrderDetail ADD TertiaryConversionRatio [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_PurchaseOrderDetail", "FK_Task_PurchaseOrderDetail_Setup_UnitType1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrderDetail ADD CONSTRAINT FK_Task_PurchaseOrderDetail_Setup_UnitType1 FOREIGN KEY (PrimaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrderDetail ADD CONSTRAINT FK_Task_PurchaseOrderDetail_Setup_UnitType2 FOREIGN KEY (SecondaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrderDetail ADD CONSTRAINT FK_Task_PurchaseOrderDetail_Setup_UnitType3 FOREIGN KEY (TertiaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_GoodsReceiveDetail", "Discount"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceiveDetail ADD Discount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceiveDetail ADD Discount1 [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceiveDetail ADD Discount2 [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceiveDetail ADD PrimaryUnitTypeId [bigint] NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceiveDetail ADD SecondaryUnitTypeId [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceiveDetail ADD TertiaryUnitTypeId [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceiveDetail ADD SecondaryConversionRatio [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceiveDetail ADD TertiaryConversionRatio [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_GoodsReceiveDetail", "FK_Task_GoodsReceiveDetail_Setup_UnitType1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceiveDetail ADD CONSTRAINT FK_Task_GoodsReceiveDetail_Setup_UnitType1 FOREIGN KEY (PrimaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceiveDetail ADD CONSTRAINT FK_Task_GoodsReceiveDetail_Setup_UnitType2 FOREIGN KEY (SecondaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceiveDetail ADD CONSTRAINT FK_Task_GoodsReceiveDetail_Setup_UnitType3 FOREIGN KEY (TertiaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_ReceiveFinalizeDetail", "Discount"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalizeDetail ADD Discount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalizeDetail ADD Discount1 [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalizeDetail ADD Discount2 [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalizeDetail ADD PrimaryUnitTypeId [bigint] NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalizeDetail ADD SecondaryUnitTypeId [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalizeDetail ADD TertiaryUnitTypeId [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalizeDetail ADD SecondaryConversionRatio [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalizeDetail ADD TertiaryConversionRatio [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ReceiveFinalizeDetail", "FK_Task_ReceiveFinalizeDetail_Setup_UnitType1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalizeDetail ADD CONSTRAINT FK_Task_ReceiveFinalizeDetail_Setup_UnitType1 FOREIGN KEY (PrimaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalizeDetail ADD CONSTRAINT FK_Task_ReceiveFinalizeDetail_Setup_UnitType2 FOREIGN KEY (SecondaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalizeDetail ADD CONSTRAINT FK_Task_ReceiveFinalizeDetail_Setup_UnitType3 FOREIGN KEY (TertiaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_ReceiveFinalize", "PurchaseOperationType"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalize ADD PurchaseOperationType [varchar](1) DEFAULT 'R' NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalize ADD TermsAndConditionsId [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalize ADD TermsAndConditionsDetail [varchar](5000) NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalize ADD PaymentTermsId [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalize ADD PaymentTermsDetail [varchar](5000) NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalize ADD DiscountType [varchar](1) DEFAULT 'A' NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalize ADD DiscountValue [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalize ADD Discount1Value [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalize ADD Discount2Value [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalize ADD DiscountAmount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalize ADD Discount1Amount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalize ADD Discount2Amount [numeric](18, 4) DEFAULT 0 NOT NULL");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'R = Regular Purchase, D = Direct Purchase' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_ReceiveFinalize', @level2type=N'COLUMN',@level2name=N'PurchaseOperationType'");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A = Amount, P = Percentage' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_ReceiveFinalize', @level2type=N'COLUMN',@level2name=N'DiscountType'");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ReceiveFinalize", "FK_Task_ReceiveFinalize_Setup_TermsAndConditions"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalize ADD CONSTRAINT FK_Task_ReceiveFinalize_Setup_TermsAndConditions FOREIGN KEY (TermsAndConditionsId) REFERENCES Setup_TermsAndConditions (TermsAndConditionsId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ReceiveFinalize", "FK_Task_ReceiveFinalize_Setup_TermsAndConditions1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalize ADD CONSTRAINT FK_Task_ReceiveFinalize_Setup_TermsAndConditions1 FOREIGN KEY (PaymentTermsId) REFERENCES Setup_TermsAndConditions (TermsAndConditionsId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Stock_CurrentStock", "ReferenceNo"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_CurrentStock ADD ReferenceNo [varchar](50) NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_CurrentStock ADD ReferenceDate [datetime] NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_CurrentStock ADD MainReferenceNo [varchar](50) NULL");
            }

            //if table not found then create table
            if (!CheckTable("Stock_TransitStock"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Stock_TransitStock(
	                [TransitStockId] [uniqueidentifier] NOT NULL,
	                [TransitType] [varchar](20) NOT NULL,
	                [ProductId] [bigint] NOT NULL,
	                [ProductDimensionId] [bigint] NULL,
	                [UnitTypeId] [bigint] NOT NULL,
	                [Quantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Cost] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Cost1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Cost2] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [LocationId] [bigint] NOT NULL,
	                [WareHouseId] [bigint] NULL,
	                [CompanyId] [bigint] NOT NULL,
	                [EntryDate] [datetime] NOT NULL,
	                [BatchNo] [varchar](50) NULL,
	                [ManufactureDate] [datetime] NULL,
	                [ExpireDate] [datetime] NULL,
                    [ReferenceNo] [varchar](50) NOT NULL,
                    [ReferenceDate] [datetime] NOT NULL,
                    [MainReferenceNo] [varchar](50) NULL,
                 CONSTRAINT [PK_Stock_TransitStock] PRIMARY KEY CLUSTERED ([TransitStockId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_TransitStock ADD CONSTRAINT FK_Stock_TransitStock_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_TransitStock ADD CONSTRAINT FK_Stock_TransitStock_Setup_Location FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_TransitStock ADD CONSTRAINT FK_Stock_TransitStock_Setup_Location1 FOREIGN KEY (WareHouseId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_TransitStock ADD CONSTRAINT FK_Stock_TransitStock_Setup_Product FOREIGN KEY (ProductId) REFERENCES Setup_Product (ProductId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_TransitStock ADD CONSTRAINT FK_Stock_TransitStock_Setup_ProductDimension FOREIGN KEY (ProductDimensionId) REFERENCES Setup_ProductDimension (ProductDimensionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_TransitStock ADD CONSTRAINT FK_Stock_TransitStock_Setup_UnitType FOREIGN KEY (UnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Stock_TransitStockSerial"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Stock_TransitStockSerial(
	                [TransitStockSerialId] [uniqueidentifier] NOT NULL,
	                [TransitStockId] [uniqueidentifier] NOT NULL,
	                [Serial] [varchar](100) NOT NULL,
	                [AdditionalSerial] [varchar](100) NULL,
                 CONSTRAINT [PK_Stock_TransitStockSerial] PRIMARY KEY CLUSTERED ([TransitStockSerialId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_TransitStockSerial ADD CONSTRAINT IX_Stock_TransitStockSerial UNIQUE NONCLUSTERED (TransitStockId ASC, Serial ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_TransitStockSerial ADD CONSTRAINT FK_Stock_TransitStockSerial_Stock_TransitStock FOREIGN KEY (TransitStockId) REFERENCES Stock_TransitStock (TransitStockId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_DeliveryChallanDetail", "ReferenceNo"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD ReferenceNo [varchar](50) NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_SalesInvoice", "IsSettledByReturn"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD IsSettledByReturn [bit] DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_SalesInvoiceDetail", "ReturnedQuantity"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD ReturnedQuantity [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //if table not found then create table
            if (!CheckTable("Task_SalesReturnNos"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_SalesReturnNos(
                 [Id] [uniqueidentifier] DEFAULT newid() NOT NULL,
                 [ReturnNo] [varchar](50) NOT NULL,
                 [Year] [bigint] NOT NULL,
                 [CompanyId] [bigint] NOT NULL,
                 CONSTRAINT [PK_Task_SalesReturnNos] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesReturnNos ADD CONSTRAINT IX_Task_SalesReturnNos UNIQUE NONCLUSTERED (ReturnNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesReturnNos ADD CONSTRAINT FK_Task_SalesReturnNos_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_SalesReturn"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_SalesReturn(
                    [ReturnId] [uniqueidentifier] NOT NULL,
	                [ReturnNo] [varchar](50) NOT NULL,
	                [ReturnDate] [datetime] NOT NULL,
	                [SelectedCurrency] [varchar](5) NOT NULL,
	                [Currency1Rate] [numeric](18, 4) NOT NULL,
	                [Currency2Rate] [numeric](18, 4) NOT NULL,
	                [ReturnAmount] [numeric](18, 4) NOT NULL,
	                [Return1Amount] [numeric](18, 4) NOT NULL,
	                [Return2Amount] [numeric](18, 4) NOT NULL,
	                [Approved] [varchar](1) NOT NULL,
	                [ApprovedBy] [bigint] NULL,
	                [ApprovedDate] [datetime] NULL,
	                [CancelReason] [varchar](200) NULL,
	                [LocationId] [bigint] NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
	                [EntryBy] [bigint] NOT NULL,
	                [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Task_SalesReturn] PRIMARY KEY CLUSTERED ([ReturnId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesReturn ADD CONSTRAINT IX_Task_SalesReturn UNIQUE NONCLUSTERED (ReturnNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesReturn ADD CONSTRAINT FK_Task_SalesReturn_Security_User FOREIGN KEY (ApprovedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesReturn ADD CONSTRAINT FK_Task_SalesReturn_Security_User1 FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesReturn ADD CONSTRAINT FK_Task_SalesReturn_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesReturn ADD CONSTRAINT FK_Task_SalesReturn_Setup_Location FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_SalesReturnDetail"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_SalesReturnDetail(
                    [ReturnDetailId] [uniqueidentifier] NOT NULL,
	                [ReturnId] [uniqueidentifier] NOT NULL,
	                [InvoiceId] [uniqueidentifier] NULL,
	                [ProductId] [bigint] NOT NULL,
	                [ProductDimensionId] [bigint] NULL,
	                [UnitTypeId] [bigint] NOT NULL,
	                [Quantity] [numeric](18, 4) NOT NULL,
	                [Price] [numeric](18, 4) NOT NULL,
	                [Price1] [numeric](18, 4) NOT NULL,
	                [Price2] [numeric](18, 4) NOT NULL,
	                [SoldPrice] [numeric](18, 4) NOT NULL,
	                [SoldPrice1] [numeric](18, 4) NOT NULL,
	                [SoldPrice2] [numeric](18, 4) NOT NULL,
	                [DiscountAtSold] [numeric](18, 4) NOT NULL,
	                [Discount1AtSold] [numeric](18, 4) NOT NULL,
	                [Discount2AtSold] [numeric](18, 4) NOT NULL,
	                [InvoiceDiscountAtSold] [numeric](18, 4) NOT NULL,
	                [InvoiceDiscount1AtSold] [numeric](18, 4) NOT NULL,
	                [InvoiceDiscount2AtSold] [numeric](18, 4) NOT NULL,
	                [Cost] [numeric](18, 4) NOT NULL,
	                [Cost1] [numeric](18, 4) NOT NULL,
	                [Cost2] [numeric](18, 4) NOT NULL,
	                [LossOrGainAmount] [numeric](18, 4) NOT NULL,
	                [LossOrGainAmount1] [numeric](18, 4) NOT NULL,
	                [LossOrGainAmount2] [numeric](18, 4) NOT NULL,
	                [PrimaryUnitTypeId] [bigint] NOT NULL,
	                [SecondaryUnitTypeId] [bigint] NULL,
	                [TertiaryUnitTypeId] [bigint] NULL,
	                [SecondaryConversionRatio] [numeric](18, 4) NOT NULL,
	                [TertiaryConversionRatio] [numeric](18, 4) NOT NULL,
                 CONSTRAINT [PK_Task_SalesReturnDetail] PRIMARY KEY CLUSTERED ([ReturnDetailId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesReturnDetail ADD CONSTRAINT FK_Task_SalesReturnDetail_Setup_Product FOREIGN KEY (ProductId) REFERENCES Setup_Product (ProductId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesReturnDetail ADD CONSTRAINT FK_Task_SalesReturnDetail_Setup_ProductDimension FOREIGN KEY (ProductDimensionId) REFERENCES Setup_ProductDimension (ProductDimensionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesReturnDetail ADD CONSTRAINT FK_Task_SalesReturnDetail_Setup_UnitType FOREIGN KEY (UnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesReturnDetail ADD CONSTRAINT FK_Task_SalesReturnDetail_Setup_UnitType1 FOREIGN KEY (PrimaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesReturnDetail ADD CONSTRAINT FK_Task_SalesReturnDetail_Setup_UnitType2 FOREIGN KEY (SecondaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesReturnDetail ADD CONSTRAINT FK_Task_SalesReturnDetail_Setup_UnitType3 FOREIGN KEY (TertiaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesReturnDetail ADD CONSTRAINT FK_Task_SalesReturnDetail_Task_SalesInvoice FOREIGN KEY (InvoiceId) REFERENCES Task_SalesInvoice (InvoiceId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesReturnDetail ADD CONSTRAINT FK_Task_SalesReturnDetail_Task_SalesReturn FOREIGN KEY (ReturnId) REFERENCES Task_SalesReturn (ReturnId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_SalesReturnDetailSerial"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_SalesReturnDetailSerial(
                    [ReturnDetailSerialId] [uniqueidentifier] NOT NULL,
	                [ReturnDetailId] [uniqueidentifier] NOT NULL,
	                [Serial] [varchar](100) NOT NULL,
	                [AdditionalSerial] [varchar](100) NULL,
                CONSTRAINT [PK_Task_SalesReturnDetailSerial] PRIMARY KEY CLUSTERED ([ReturnDetailSerialId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesReturnDetailSerial ADD CONSTRAINT FK_Task_SalesReturnDetailSerial_Task_SalesReturnDetail FOREIGN KEY (ReturnDetailId) REFERENCES Task_SalesReturnDetail (ReturnDetailId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_GoodsReceive", "PurchaseOperationType"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceive ADD PurchaseOperationType [varchar](1) DEFAULT 'R' NOT NULL");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'R = Regular Purchase, D = Direct Purchase' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_GoodsReceive', @level2type=N'COLUMN',@level2name=N'PurchaseOperationType'");
            }

            //add column if not found
            if (!CheckTable("Task_PurchaseOrder", "DiscountType"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrder ADD DiscountType [varchar](1) DEFAULT 'A' NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrder ADD DiscountValue [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrder ADD Discount1Value [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrder ADD Discount2Value [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrder ADD DiscountAmount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrder ADD Discount1Amount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseOrder ADD Discount2Amount [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_SalesInvoice", "Currency1Rate"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD Currency1Rate [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD Currency2Rate [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_ReceiveFinalize", "Currency1Rate"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalize ADD Currency1Rate [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalize ADD Currency2Rate [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //if table not found then create table
            if (!CheckTable("Task_SalesInvoiceDetailSerial"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_SalesInvoiceDetailSerial(
                    [InvoiceDetailSerialId] [uniqueidentifier] NOT NULL,
	                [InvoiceDetailId] [uniqueidentifier] NOT NULL,
	                [Serial] [varchar](100) NOT NULL,
	                [AdditionalSerial] [varchar](100) NULL,
                CONSTRAINT [PK_Task_SalesInvoiceDetailSerial] PRIMARY KEY CLUSTERED ([InvoiceDetailSerialId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetailSerial ADD CONSTRAINT FK_Task_SalesInvoiceDetailSerial_Task_SalesInvoiceDetail FOREIGN KEY (InvoiceDetailId) REFERENCES Task_SalesInvoiceDetail (InvoiceDetailId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_ReceiveFinalizeDetailSerial"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_ReceiveFinalizeDetailSerial(
                    [FinalizeDetailSerialId] [uniqueidentifier] NOT NULL,
	                [FinalizeDetailId] [uniqueidentifier] NOT NULL,
	                [Serial] [varchar](100) NOT NULL,
	                [AdditionalSerial] [varchar](100) NULL,
                CONSTRAINT [PK_Task_ReceiveFinalizeDetailSerial] PRIMARY KEY CLUSTERED ([FinalizeDetailSerialId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReceiveFinalizeDetailSerial ADD CONSTRAINT FK_Task_ReceiveFinalizeDetailSerial_Task_ReceiveFinalizeDetail FOREIGN KEY (FinalizeDetailId) REFERENCES Task_ReceiveFinalizeDetail (FinalizeDetailId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_SalesInvoiceDetailSerial", "IsReturned"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetailSerial ADD IsReturned [bit] DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_SalesReturn", "AgainstPreviousSales"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesReturn ADD AgainstPreviousSales [bit] DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesReturn ADD CustomerId [bigint] NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesReturn ADD VoucherId [uniqueidentifier] NULL");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'True = Previous Sales, False = Invoice/Bill' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_SalesReturn', @level2type=N'COLUMN',@level2name=N'AgainstPreviousSales'");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesReturn ADD CONSTRAINT FK_Task_SalesReturn_Setup_Customer FOREIGN KEY (CustomerId) REFERENCES Setup_Customer (CustomerId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesReturn ADD CONSTRAINT FK_Task_SalesReturn_Task_Voucher FOREIGN KEY (VoucherId) REFERENCES Task_Voucher (VoucherId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_GoodsReceive", "ReceiveAmount"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceive ADD ReceiveAmount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceive ADD Receive1Amount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceive ADD Receive2Amount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceive ADD DiscountType [varchar](1) DEFAULT 'A' NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceive ADD DiscountValue [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceive ADD Discount1Value [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceive ADD Discount2Value [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceive ADD DiscountAmount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceive ADD Discount1Amount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceive ADD Discount2Amount [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //if table not found then create table
            if (!CheckTable("Task_TransferRequisitionFinalizeNos"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_TransferRequisitionFinalizeNos(
                 [Id] [uniqueidentifier] DEFAULT newid() NOT NULL,
                 [RequisitionNo] [varchar](50) NOT NULL,
                 [Year] [bigint] NOT NULL,
                 [CompanyId] [bigint] NOT NULL,
                 CONSTRAINT [PK_Task_TransferRequisitionFinalizeNos] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add unique key if not found
            if (!CheckRelationshipBetweenTwoTables("Task_TransferRequisitionFinalizeNos", "IX_Task_TransferRequisitionFinalizeNos"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferRequisitionFinalizeNos ADD CONSTRAINT IX_Task_TransferRequisitionFinalizeNos UNIQUE NONCLUSTERED (RequisitionNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_TransferRequisitionFinalizeNos", "FK_Task_TransferRequisitionFinalizeNos_Setup_Company"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferRequisitionFinalizeNos ADD CONSTRAINT FK_Task_TransferRequisitionFinalizeNos_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_TransferRequisitionFinalize"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_TransferRequisitionFinalize(
                 [RequisitionId] [uniqueidentifier] DEFAULT newid() NOT NULL,
                 [RequisitionNo] [varchar](50) NOT NULL,
                 [RequisitionDate] [datetime] NOT NULL,
                 [RequisitionBy] [bigint] NOT NULL,
                 [ReqToLocationId] [bigint] NOT NULL,
                 [StockType] [varchar](10) NOT NULL,
                 [Remarks] [varchar](1000) NULL,
                 [IsSettled] [bit] DEFAULT 0 NOT NULL,
                 [Approved] [varchar](1) DEFAULT 'N' NOT NULL,
                 [ApprovedBy] [bigint] NULL,
                 [ApprovedDate] [datetime] NULL,
                 [CancelReason] [varchar](200) NULL,
                 [LocationId] [bigint] NOT NULL,
                 [CompanyId] [bigint] NOT NULL,
                 [EntryBy] [bigint] NOT NULL,
                 [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Task_TransferRequisitionFinalize] PRIMARY KEY CLUSTERED ([RequisitionId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Current/RMA/LIM/Bad' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_TransferRequisitionFinalize', @level2type=N'COLUMN',@level2name=N'StockType'");
            }

            //add unique key if not found
            if (!CheckRelationshipBetweenTwoTables("Task_TransferRequisitionFinalize", "IX_Task_TransferRequisitionFinalize"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferRequisitionFinalize ADD CONSTRAINT IX_Task_TransferRequisitionFinalize UNIQUE NONCLUSTERED (RequisitionNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_TransferRequisitionFinalize", "FK_Task_TransferRequisitionFinalize_Setup_Employee"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferRequisitionFinalize ADD CONSTRAINT FK_Task_TransferRequisitionFinalize_Setup_Employee FOREIGN KEY (RequisitionBy) REFERENCES Setup_Employee (EmployeeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_TransferRequisitionFinalize", "FK_Task_TransferRequisitionFinalize_Security_User"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferRequisitionFinalize ADD CONSTRAINT FK_Task_TransferRequisitionFinalize_Security_User FOREIGN KEY (ApprovedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_TransferRequisitionFinalize", "FK_Task_TransferRequisitionFinalize_Setup_Location"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferRequisitionFinalize ADD CONSTRAINT FK_Task_TransferRequisitionFinalize_Setup_Location FOREIGN KEY (ReqToLocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_TransferRequisitionFinalize", "FK_Task_TransferRequisitionFinalize_Setup_Location1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferRequisitionFinalize ADD CONSTRAINT FK_Task_TransferRequisitionFinalize_Setup_Location1 FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_TransferRequisitionFinalize", "FK_Task_TransferRequisitionFinalize_Setup_Company"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferRequisitionFinalize ADD CONSTRAINT FK_Task_TransferRequisitionFinalize_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_TransferRequisitionFinalize", "FK_Task_TransferRequisitionFinalize_Security_User1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferRequisitionFinalize ADD CONSTRAINT FK_Task_TransferRequisitionFinalize_Security_User1 FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_TransferRequisitionFinalizeDetail"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_TransferRequisitionFinalizeDetail(
                 [RequisitionDetailId] [uniqueidentifier] DEFAULT newid() NOT NULL,
                 [RequisitionId] [uniqueidentifier] NOT NULL,
                 [ItemRequisitionId] [uniqueidentifier] NULL,
                 [ProductId] [bigint] NOT NULL,
                 [ProductDimensionId] [bigint] NULL,
                 [UnitTypeId] [bigint] NOT NULL,
                 [Quantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
	             [OrderedQuantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
                 CONSTRAINT [PK_Task_TransferRequisitionFinalizeDetail] PRIMARY KEY CLUSTERED ([RequisitionDetailId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_TransferRequisitionFinalizeDetail", "FK_Task_TransferRequisitionFinalizeDetail_Task_TransferRequisitionFinalize"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferRequisitionFinalizeDetail ADD CONSTRAINT FK_Task_TransferRequisitionFinalizeDetail_Task_TransferRequisitionFinalize FOREIGN KEY (RequisitionId) REFERENCES Task_TransferRequisitionFinalize (RequisitionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_TransferRequisitionFinalizeDetail", "FK_Task_TransferRequisitionFinalizeDetail_Task_ItemRequisition"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferRequisitionFinalizeDetail ADD CONSTRAINT FK_Task_TransferRequisitionFinalizeDetail_Task_ItemRequisition FOREIGN KEY (ItemRequisitionId) REFERENCES Task_ItemRequisition (RequisitionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_TransferRequisitionFinalizeDetail", "FK_Task_TransferRequisitionFinalizeDetail_Setup_Product"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferRequisitionFinalizeDetail ADD CONSTRAINT FK_Task_TransferRequisitionFinalizeDetail_Setup_Product FOREIGN KEY (ProductId) REFERENCES Setup_Product (ProductId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_TransferRequisitionFinalizeDetail", "FK_Task_TransferRequisitionFinalizeDetail_Setup_ProductDimension"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferRequisitionFinalizeDetail ADD CONSTRAINT FK_Task_TransferRequisitionFinalizeDetail_Setup_ProductDimension FOREIGN KEY (ProductDimensionId) REFERENCES Setup_ProductDimension (ProductDimensionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_TransferRequisitionFinalizeDetail", "FK_Task_TransferRequisitionFinalizeDetail_Setup_UnitType"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferRequisitionFinalizeDetail ADD CONSTRAINT FK_Task_TransferRequisitionFinalizeDetail_Setup_UnitType FOREIGN KEY (UnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_PurchaseReturnNos"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_PurchaseReturnNos(
                 [Id] [uniqueidentifier] DEFAULT newid() NOT NULL,
                 [ReturnNo] [varchar](50) NOT NULL,
                 [Year] [bigint] NOT NULL,
                 [CompanyId] [bigint] NOT NULL,
                 CONSTRAINT [PK_Task_PurchaseReturnNos] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseReturnNos ADD CONSTRAINT IX_Task_PurchaseReturnNos UNIQUE NONCLUSTERED (ReturnNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseReturnNos ADD CONSTRAINT FK_Task_PurchaseReturnNos_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_PurchaseReturn"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_PurchaseReturn(
                    [ReturnId] [uniqueidentifier] NOT NULL,
	                [ReturnNo] [varchar](50) NOT NULL,
	                [ReturnDate] [datetime] NOT NULL,
                    [AgainstPreviousPurchase] [bit] DEFAULT 0 NOT NULL,
                    [SupplierId] [bigint] NOT NULL,
	                [SelectedCurrency] [varchar](5) NOT NULL,
	                [Currency1Rate] [numeric](18, 4) NOT NULL,
	                [Currency2Rate] [numeric](18, 4) NOT NULL,
	                [ReturnAmount] [numeric](18, 4) NOT NULL,
	                [Return1Amount] [numeric](18, 4) NOT NULL,
	                [Return2Amount] [numeric](18, 4) NOT NULL,
	                [Approved] [varchar](1) NOT NULL,
	                [ApprovedBy] [bigint] NULL,
	                [ApprovedDate] [datetime] NULL,
	                [CancelReason] [varchar](200) NULL,
                    [VoucherId] [uniqueidentifier] NULL,
	                [LocationId] [bigint] NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
	                [EntryBy] [bigint] NOT NULL,
	                [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Task_PurchaseReturn] PRIMARY KEY CLUSTERED ([ReturnId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'True = Previous Purchase, False = Goods Receive' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_PurchaseReturn', @level2type=N'COLUMN',@level2name=N'AgainstPreviousPurchase'");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseReturn ADD CONSTRAINT IX_Task_PurchaseReturn UNIQUE NONCLUSTERED (ReturnNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseReturn ADD CONSTRAINT FK_Task_PurchaseReturn_Setup_Supplier FOREIGN KEY (SupplierId) REFERENCES Setup_Supplier (SupplierId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseReturn ADD CONSTRAINT FK_Task_PurchaseReturn_Security_User FOREIGN KEY (ApprovedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseReturn ADD CONSTRAINT FK_Task_PurchaseReturn_Task_Voucher FOREIGN KEY (VoucherId) REFERENCES Task_Voucher (VoucherId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseReturn ADD CONSTRAINT FK_Task_PurchaseReturn_Security_User1 FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseReturn ADD CONSTRAINT FK_Task_PurchaseReturn_Setup_Location FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseReturn ADD CONSTRAINT FK_Task_PurchaseReturn_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_PurchaseReturnDetail"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_PurchaseReturnDetail(
                    [ReturnDetailId] [uniqueidentifier] NOT NULL,
	                [ReturnId] [uniqueidentifier] NOT NULL,
	                [ReceiveId] [uniqueidentifier] NULL,
	                [ProductId] [bigint] NOT NULL,
	                [ProductDimensionId] [bigint] NULL,
	                [UnitTypeId] [bigint] NOT NULL,
	                [Quantity] [numeric](18, 4) NOT NULL,
	                [Price] [numeric](18, 4) NOT NULL,
	                [Price1] [numeric](18, 4) NOT NULL,
	                [Price2] [numeric](18, 4) NOT NULL,
	                [PurchasedPrice] [numeric](18, 4) NOT NULL,
	                [PurchasedPrice1] [numeric](18, 4) NOT NULL,
	                [PurchasedPrice2] [numeric](18, 4) NOT NULL,
	                [DiscountAtPurchase] [numeric](18, 4) NOT NULL,
	                [Discount1AtPurchase] [numeric](18, 4) NOT NULL,
	                [Discount2AtPurchase] [numeric](18, 4) NOT NULL,
	                [GoodsReceiveDiscountAtPurchase] [numeric](18, 4) NOT NULL,
	                [GoodsReceiveDiscount1AtPurchase] [numeric](18, 4) NOT NULL,
	                [GoodsReceiveDiscount2AtPurchase] [numeric](18, 4) NOT NULL,
	                [Cost] [numeric](18, 4) NOT NULL,
	                [Cost1] [numeric](18, 4) NOT NULL,
	                [Cost2] [numeric](18, 4) NOT NULL,
	                [LossOrGainAmount] [numeric](18, 4) NOT NULL,
	                [LossOrGainAmount1] [numeric](18, 4) NOT NULL,
	                [LossOrGainAmount2] [numeric](18, 4) NOT NULL,
	                [PrimaryUnitTypeId] [bigint] NOT NULL,
	                [SecondaryUnitTypeId] [bigint] NULL,
	                [TertiaryUnitTypeId] [bigint] NULL,
	                [SecondaryConversionRatio] [numeric](18, 4) NOT NULL,
	                [TertiaryConversionRatio] [numeric](18, 4) NOT NULL,
                 CONSTRAINT [PK_Task_PurchaseReturnDetail] PRIMARY KEY CLUSTERED ([ReturnDetailId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseReturnDetail ADD CONSTRAINT FK_Task_PurchaseReturnDetail_Setup_Product FOREIGN KEY (ProductId) REFERENCES Setup_Product (ProductId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseReturnDetail ADD CONSTRAINT FK_Task_PurchaseReturnDetail_Setup_ProductDimension FOREIGN KEY (ProductDimensionId) REFERENCES Setup_ProductDimension (ProductDimensionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseReturnDetail ADD CONSTRAINT FK_Task_PurchaseReturnDetail_Setup_UnitType FOREIGN KEY (UnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseReturnDetail ADD CONSTRAINT FK_Task_PurchaseReturnDetail_Setup_UnitType1 FOREIGN KEY (PrimaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseReturnDetail ADD CONSTRAINT FK_Task_PurchaseReturnDetail_Setup_UnitType2 FOREIGN KEY (SecondaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseReturnDetail ADD CONSTRAINT FK_Task_PurchaseReturnDetail_Setup_UnitType3 FOREIGN KEY (TertiaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseReturnDetail ADD CONSTRAINT FK_Task_PurchaseReturnDetail_Task_GoodsReceive FOREIGN KEY (ReceiveId) REFERENCES Task_GoodsReceive (ReceiveId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseReturnDetail ADD CONSTRAINT FK_Task_PurchaseReturnDetail_Task_PurchaseReturn FOREIGN KEY (ReturnId) REFERENCES Task_PurchaseReturn (ReturnId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_PurchaseReturnDetailSerial"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_PurchaseReturnDetailSerial(
                    [ReturnDetailSerialId] [uniqueidentifier] NOT NULL,
	                [ReturnDetailId] [uniqueidentifier] NOT NULL,
	                [Serial] [varchar](100) NOT NULL,
	                [AdditionalSerial] [varchar](100) NULL,
                CONSTRAINT [PK_Task_PurchaseReturnDetailSerial] PRIMARY KEY CLUSTERED ([ReturnDetailSerialId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseReturnDetailSerial ADD CONSTRAINT FK_Task_PurchaseReturnDetailSerial_Task_PurchaseReturnDetail FOREIGN KEY (ReturnDetailId) REFERENCES Task_PurchaseReturnDetail (ReturnDetailId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_GoodsReceive", "IsSettledByReturn"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceive ADD IsSettledByReturn [bit] DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_GoodsReceiveDetail", "ReturnedQuantity"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceiveDetail ADD ReturnedQuantity [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_GoodsReceiveDetailSerial", "IsReturned"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceiveDetailSerial ADD IsReturned [bit] DEFAULT 0 NOT NULL");
            }

            //if table not found then create table
            if (!CheckTable("Task_TransferOrderNos"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_TransferOrderNos(
                 [Id] [uniqueidentifier] DEFAULT newid() NOT NULL,
                 [OrderNo] [varchar](50) NOT NULL,
                 [Year] [bigint] NOT NULL,
                 [CompanyId] [bigint] NOT NULL,
                 CONSTRAINT [PK_Task_TransferOrderNos] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferOrderNos ADD CONSTRAINT IX_Task_TransferOrderNos UNIQUE NONCLUSTERED (OrderNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferOrderNos ADD CONSTRAINT FK_Task_TransferOrderNos_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_TransferOrder"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_TransferOrder(
                 [OrderId] [uniqueidentifier] DEFAULT newid() NOT NULL,
                 [OrderNo] [varchar](50) NOT NULL,
                 [OrderDate] [datetime] NOT NULL,
                 [OrderBy] [bigint] NULL,
                 [TransferToId] [bigint] NOT NULL,
                 [TransferToStockType] [varchar](10) NOT NULL,
                 [TransferFromId] [bigint] NOT NULL,
                 [TransferFromStockType] [varchar](10) NOT NULL,
                 [Remarks] [varchar](1000) NULL,
                 [IsSettled] [bit] DEFAULT 0 NOT NULL,
                 [Approved] [varchar](1) DEFAULT 'N' NOT NULL,
                 [ApprovedBy] [bigint] NULL,
                 [ApprovedDate] [datetime] NULL,
                 [CancelReason] [varchar](200) NULL,
                 [LocationId] [bigint] NOT NULL,
                 [CompanyId] [bigint] NOT NULL,
                 [EntryBy] [bigint] NOT NULL,
                 [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Task_TransferOrder] PRIMARY KEY CLUSTERED ([OrderId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Current/RMA/LIM/Bad' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_TransferOrder', @level2type=N'COLUMN',@level2name=N'TransferToStockType'");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Current/RMA/LIM/Bad' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_TransferOrder', @level2type=N'COLUMN',@level2name=N'TransferFromStockType'");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferOrder ADD CONSTRAINT IX_Task_TransferOrder UNIQUE NONCLUSTERED (OrderNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferOrder ADD CONSTRAINT FK_Task_TransferOrder_Setup_Employee FOREIGN KEY (OrderBy) REFERENCES Setup_Employee (EmployeeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferOrder ADD CONSTRAINT FK_Task_TransferOrder_Setup_Location FOREIGN KEY (TransferToId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferOrder ADD CONSTRAINT FK_Task_TransferOrder_Setup_Location1 FOREIGN KEY (TransferFromId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferOrder ADD CONSTRAINT FK_Task_TransferOrder_Security_User FOREIGN KEY (ApprovedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferOrder ADD CONSTRAINT FK_Task_TransferOrder_Setup_Location2 FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferOrder ADD CONSTRAINT FK_Task_TransferOrder_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferOrder ADD CONSTRAINT FK_Task_TransferOrder_Security_User1 FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_TransferOrderDetail"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_TransferOrderDetail(
                    [OrderDetailId] [uniqueidentifier] DEFAULT newid() NOT NULL,
                    [OrderId] [uniqueidentifier] NOT NULL,
                    [RequisitionId] [uniqueidentifier] NULL,
                    [ProductId] [bigint] NOT NULL,
                    [ProductDimensionId] [bigint] NULL,
                    [UnitTypeId] [bigint] NOT NULL,
                    [Quantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [ChallanQuantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
                 CONSTRAINT [PK_Task_TransferOrderDetail] PRIMARY KEY CLUSTERED ([OrderDetailId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferOrderDetail ADD CONSTRAINT FK_Task_TransferOrderDetail_Task_TransferOrder FOREIGN KEY (OrderId) REFERENCES Task_TransferOrder (OrderId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferOrderDetail ADD CONSTRAINT FK_Task_TransferOrderDetail_Task_TransferRequisitionFinalize FOREIGN KEY (RequisitionId) REFERENCES Task_TransferRequisitionFinalize (RequisitionId) ON UPDATE NO ACTION ON DELETE NO ACTION");                
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferOrderDetail ADD CONSTRAINT FK_Task_TransferOrderDetail_Setup_Product FOREIGN KEY (ProductId) REFERENCES Setup_Product (ProductId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferOrderDetail ADD CONSTRAINT FK_Task_TransferOrderDetail_Setup_ProductDimension FOREIGN KEY (ProductDimensionId) REFERENCES Setup_ProductDimension (ProductDimensionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferOrderDetail ADD CONSTRAINT FK_Task_TransferOrderDetail_Setup_UnitType FOREIGN KEY (UnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Setup_Port"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Setup_Port(
                    [PortId] [bigint] NOT NULL,
	                [PortCode] [varchar](10) NOT NULL,
	                [PortName] [varchar](50) NOT NULL,
	                [Address] [varchar](500) NOT NULL,
                    [ShipmentMode] [varchar](10) NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
	                [EntryBy] [bigint] NOT NULL,
	                [EntryDate] [datetime] NOT NULL,
                    [EditedBy] [bigint] NULL,
                    [EditedDate] [datetime] NULL,
                 CONSTRAINT [PK_Setup_Port] PRIMARY KEY CLUSTERED ([PortId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Port ADD CONSTRAINT IX_Setup_Port UNIQUE NONCLUSTERED (PortCode ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Port ADD CONSTRAINT FK_Setup_Port_Security_User FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Port ADD CONSTRAINT FK_Setup_Port_Security_User1 FOREIGN KEY (EditedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Port ADD CONSTRAINT FK_Setup_Port_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_ProformaInvoiceNos"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_ProformaInvoiceNos(
                 [Id] [uniqueidentifier] DEFAULT newid() NOT NULL,
                 [InvoiceNo] [varchar](50) NOT NULL,
                 [Year] [bigint] NOT NULL,
                 [CompanyId] [bigint] NOT NULL,
                 CONSTRAINT [PK_Task_ProformaInvoiceNos] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ProformaInvoiceNos ADD CONSTRAINT IX_Task_ProformaInvoiceNos UNIQUE NONCLUSTERED (InvoiceNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ProformaInvoiceNos ADD CONSTRAINT FK_Task_ProformaInvoiceNos_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_ProformaInvoice"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_ProformaInvoice(
                    [InvoiceId] [uniqueidentifier] NOT NULL,
	                [InvoiceNo] [varchar](50) NOT NULL,
	                [InvoiceDate] [datetime] NOT NULL,
                    [SupplierId] [bigint] NOT NULL,
	                [SelectedCurrency] [varchar](5) NOT NULL,
	                [Currency1Rate] [numeric](18, 4) NOT NULL,
	                [Currency2Rate] [numeric](18, 4) NOT NULL,
                    [AmendmentNo] [tinyint] DEFAULT 0 NOT NULL,
                    [ShipmentMode] [varchar](10) NOT NULL,
                    [Incoterms] [varchar](20) NOT NULL,
	                [ReferenceNo] [varchar](50) NULL,
                    [OperationTypeId] [bigint] NOT NULL,
                    [PortOfLoadingId] [bigint] NOT NULL,
                    [PortOfDischargeId] [bigint] NOT NULL,
	                [InvoiceAmount] [numeric](18, 4) NOT NULL,
	                [Invoice1Amount] [numeric](18, 4) NOT NULL,
	                [Invoice2Amount] [numeric](18, 4) NOT NULL,
	                [TermsAndConditionsId] [bigint] NULL,
	                [TermsAndConditionsDetail] [varchar](5000) NULL,
                    [IsSettledByLIM] [bit] DEFAULT 0 NOT NULL,
	                [Approved] [varchar](1) NOT NULL,
	                [ApprovedBy] [bigint] NULL,
	                [ApprovedDate] [datetime] NULL,
	                [CancelReason] [varchar](200) NULL,
	                [LocationId] [bigint] NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
	                [EntryBy] [bigint] NOT NULL,
	                [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Task_ProformaInvoice] PRIMARY KEY CLUSTERED ([InvoiceId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ProformaInvoice ADD CONSTRAINT IX_Task_ProformaInvoice UNIQUE NONCLUSTERED (InvoiceNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ProformaInvoice ADD CONSTRAINT FK_Task_ProformaInvoice_Setup_Supplier FOREIGN KEY (SupplierId) REFERENCES Setup_Supplier (SupplierId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ProformaInvoice ADD CONSTRAINT FK_Task_ProformaInvoice_Configuration_OperationType FOREIGN KEY (OperationTypeId) REFERENCES Configuration_OperationType (OperationTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ProformaInvoice ADD CONSTRAINT FK_Task_ProformaInvoice_Setup_TermsAndConditions FOREIGN KEY (TermsAndConditionsId) REFERENCES Setup_TermsAndConditions (TermsAndConditionsId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ProformaInvoice ADD CONSTRAINT FK_Task_ProformaInvoice_Security_User FOREIGN KEY (ApprovedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ProformaInvoice ADD CONSTRAINT FK_Task_ProformaInvoice_Security_User1 FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ProformaInvoice ADD CONSTRAINT FK_Task_ProformaInvoice_Setup_Location FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ProformaInvoice ADD CONSTRAINT FK_Task_ProformaInvoice_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ProformaInvoice ADD CONSTRAINT FK_Task_ProformaInvoice_Setup_Port FOREIGN KEY (PortOfLoadingId) REFERENCES Setup_Port (PortId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ProformaInvoice ADD CONSTRAINT FK_Task_ProformaInvoice_Setup_Port1 FOREIGN KEY (PortOfDischargeId) REFERENCES Setup_Port (PortId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_ProformaInvoiceDetail"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_ProformaInvoiceDetail(
                    [InvoiceDetailId] [uniqueidentifier] DEFAULT newid() NOT NULL,
                    [InvoiceId] [uniqueidentifier] NOT NULL,
                    [PurchaseOrderId] [uniqueidentifier] NULL,
                    [ProductId] [bigint] NOT NULL,
                    [ProductDimensionId] [bigint] NULL,
                    [UnitTypeId] [bigint] NOT NULL,
                    [Quantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [LIMStockInQuantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [Price] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Price1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Price2] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [WeightOrCBM] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [PackingUnitTypeId] [bigint] NOT NULL,
                    [PackingQuantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
                 CONSTRAINT [PK_Task_ProformaInvoiceDetail] PRIMARY KEY CLUSTERED ([InvoiceDetailId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ProformaInvoiceDetail ADD CONSTRAINT FK_Task_ProformaInvoiceDetail_Task_ProformaInvoice FOREIGN KEY (InvoiceId) REFERENCES Task_ProformaInvoice (InvoiceId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ProformaInvoiceDetail ADD CONSTRAINT FK_Task_ProformaInvoiceDetail_Task_PurchaseOrder FOREIGN KEY (PurchaseOrderId) REFERENCES Task_PurchaseOrder (OrderId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ProformaInvoiceDetail ADD CONSTRAINT FK_Task_ProformaInvoiceDetail_Setup_Product FOREIGN KEY (ProductId) REFERENCES Setup_Product (ProductId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ProformaInvoiceDetail ADD CONSTRAINT FK_Task_ProformaInvoiceDetail_Setup_ProductDimension FOREIGN KEY (ProductDimensionId) REFERENCES Setup_ProductDimension (ProductDimensionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ProformaInvoiceDetail ADD CONSTRAINT FK_Task_ProformaInvoiceDetail_Setup_UnitType FOREIGN KEY (UnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ProformaInvoiceDetail ADD CONSTRAINT FK_Task_ProformaInvoiceDetail_Setup_UnitType1 FOREIGN KEY (PackingUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_LIMStockInNos"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_LIMStockInNos(
                 [Id] [uniqueidentifier] DEFAULT newid() NOT NULL,
                 [StockInNo] [varchar](50) NOT NULL,
                 [Year] [bigint] NOT NULL,
                 [CompanyId] [bigint] NOT NULL,
                 CONSTRAINT [PK_Task_LIMStockInNos] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LIMStockInNos ADD CONSTRAINT IX_Task_LIMStockInNos UNIQUE NONCLUSTERED (StockInNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LIMStockInNos ADD CONSTRAINT FK_Task_LIMStockInNos_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_LIMStockIn"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_LIMStockIn(
                    [StockInId] [uniqueidentifier] NOT NULL,
	                [StockInNo] [varchar](50) NOT NULL,
	                [StockInDate] [datetime] NOT NULL,
	                [LIMAgainst] [varchar](10) NOT NULL,
                    [ProformaInvoiceId] [uniqueidentifier] NOT NULL,
	                [PartShipmentNo] [varchar](20) NOT NULL,
	                [SelectedCurrency] [varchar](5) NOT NULL,
	                [Currency1Rate] [numeric](18, 4) NOT NULL,
	                [Currency2Rate] [numeric](18, 4) NOT NULL,
	                [ReferenceNo] [varchar](50) NULL,
                    [ReferenceDate] [datetime] NULL,
                    [Remarks] [varchar](1000) NULL,
	                [StockInAmount] [numeric](18, 4) NOT NULL,
	                [StockIn1Amount] [numeric](18, 4) NOT NULL,
	                [StockIn2Amount] [numeric](18, 4) NOT NULL,
                    [IsSettledByImportIn] [bit] DEFAULT 0 NOT NULL,
	                [Approved] [varchar](1) NOT NULL,
	                [ApprovedBy] [bigint] NULL,
	                [ApprovedDate] [datetime] NULL,
	                [CancelReason] [varchar](200) NULL,
	                [LocationId] [bigint] NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
	                [EntryBy] [bigint] NOT NULL,
	                [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Task_LIMStockIn] PRIMARY KEY CLUSTERED ([StockInId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PI = Proforma Invoice, PO = Purchase Order (Import), Direct = Direct LIM Stock' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_LIMStockIn', @level2type=N'COLUMN',@level2name=N'LIMAgainst'");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LIMStockIn ADD CONSTRAINT IX_Task_LIMStockIn UNIQUE NONCLUSTERED (StockInNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LIMStockIn ADD CONSTRAINT IX_Task_LIMStockIn1 UNIQUE NONCLUSTERED (ProformaInvoiceId ASC, PartShipmentNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LIMStockIn ADD CONSTRAINT FK_Task_LIMStockIn_Task_ProformaInvoice FOREIGN KEY (ProformaInvoiceId) REFERENCES Task_ProformaInvoice (InvoiceId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LIMStockIn ADD CONSTRAINT FK_Task_LIMStockIn_Security_User FOREIGN KEY (ApprovedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LIMStockIn ADD CONSTRAINT FK_Task_LIMStockIn_Security_User1 FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LIMStockIn ADD CONSTRAINT FK_Task_LIMStockIn_Setup_Location FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LIMStockIn ADD CONSTRAINT FK_Task_LIMStockIn_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_LIMStockInDetail"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_LIMStockInDetail(
                    [StockInDetailId] [uniqueidentifier] DEFAULT newid() NOT NULL,
                    [StockInId] [uniqueidentifier] NOT NULL,
                    [PurchaseOrderNo] [varchar](50) NULL,
                    [ProductId] [bigint] NOT NULL,
                    [ProductDimensionId] [bigint] NULL,
                    [UnitTypeId] [bigint] NOT NULL,
                    [Quantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [NoCostQuantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [ImportInQuantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [Price] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Price1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Price2] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [PrimaryUnitTypeId] [bigint] NOT NULL,
	                [SecondaryUnitTypeId] [bigint] NULL,
	                [TertiaryUnitTypeId] [bigint] NULL,
	                [SecondaryConversionRatio] [numeric](18, 4) NOT NULL,
	                [TertiaryConversionRatio] [numeric](18, 4) NOT NULL,
                 CONSTRAINT [PK_Task_LIMStockInDetail] PRIMARY KEY CLUSTERED ([StockInDetailId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LIMStockInDetail ADD CONSTRAINT FK_Task_LIMStockInDetail_Task_LIMStockIn FOREIGN KEY (StockInId) REFERENCES Task_LIMStockIn (StockInId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LIMStockInDetail ADD CONSTRAINT FK_Task_LIMStockInDetail_Setup_Product FOREIGN KEY (ProductId) REFERENCES Setup_Product (ProductId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LIMStockInDetail ADD CONSTRAINT FK_Task_LIMStockInDetail_Setup_ProductDimension FOREIGN KEY (ProductDimensionId) REFERENCES Setup_ProductDimension (ProductDimensionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LIMStockInDetail ADD CONSTRAINT FK_Task_LIMStockInDetail_Setup_UnitType FOREIGN KEY (UnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LIMStockInDetail ADD CONSTRAINT FK_Task_LIMStockInDetail_Setup_UnitType1 FOREIGN KEY (PrimaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LIMStockInDetail ADD CONSTRAINT FK_Task_LIMStockInDetail_Setup_UnitType2 FOREIGN KEY (SecondaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LIMStockInDetail ADD CONSTRAINT FK_Task_LIMStockInDetail_Setup_UnitType3 FOREIGN KEY (TertiaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Stock_LIMStock"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Stock_LIMStock(
                    [LIMStockId] [uniqueidentifier] NOT NULL,
	                [ProductId] [bigint] NOT NULL,
	                [ProductDimensionId] [bigint] NULL,
	                [UnitTypeId] [bigint] NOT NULL,
	                [Quantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [Cost] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Cost1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Cost2] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [ReferenceNo] [varchar](50) NOT NULL,
	                [ReferenceDate] [datetime] NOT NULL,
                    [LocationId] [bigint] NOT NULL,
                    [CompanyId] [bigint] NOT NULL,
	                [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Stock_LIMStock] PRIMARY KEY CLUSTERED ([LIMStockId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_LIMStock ADD CONSTRAINT FK_Stock_LIMStock_Setup_Product FOREIGN KEY (ProductId) REFERENCES Setup_Product (ProductId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_LIMStock ADD CONSTRAINT FK_Stock_LIMStock_Setup_ProductDimension FOREIGN KEY (ProductDimensionId) REFERENCES Setup_ProductDimension (ProductDimensionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_LIMStock ADD CONSTRAINT FK_Stock_LIMStock_Setup_UnitType FOREIGN KEY (UnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_LIMStock ADD CONSTRAINT FK_Stock_LIMStock_Setup_Location FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_LIMStock ADD CONSTRAINT FK_Stock_LIMStock_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_LC"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_LC(
                    [LCId] [uniqueidentifier] NOT NULL,
                    [LCNo] [varchar](50) NOT NULL,
	                [LCDate] [datetime] NOT NULL,
                    [ProformaInvoiceId] [uniqueidentifier] NOT NULL,
	                [ImporterBankId] [bigint] NOT NULL,
	                [SupplierBankId] [bigint] NOT NULL,
	                [SupplierId] [bigint] NULL,
                    [LocationId] [bigint] NOT NULL,
                    [CompanyId] [bigint] NOT NULL,
	                [EntryBy] [bigint] NOT NULL,
	                [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Task_LC] PRIMARY KEY CLUSTERED ([LCId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LC ADD CONSTRAINT IX_Task_LC UNIQUE NONCLUSTERED (LCNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LC ADD CONSTRAINT FK_Task_LC_Task_ProformaInvoice FOREIGN KEY (ProformaInvoiceId) REFERENCES Task_ProformaInvoice (InvoiceId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LC ADD CONSTRAINT FK_Task_LC_Setup_Bank FOREIGN KEY (ImporterBankId) REFERENCES Setup_Bank (BankId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LC ADD CONSTRAINT FK_Task_LC_Setup_Bank1 FOREIGN KEY (SupplierBankId) REFERENCES Setup_Bank (BankId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LC ADD CONSTRAINT FK_Task_LC_Setup_Supplier FOREIGN KEY (SupplierId) REFERENCES Setup_Supplier (SupplierId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LC ADD CONSTRAINT FK_Task_LC_Setup_Location FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LC ADD CONSTRAINT FK_Task_LC_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LC ADD CONSTRAINT FK_Task_LC_Security_User FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }
            */

            //rename column if found
            if (CheckTable("Task_SalesOrder", "IsSettled"))
            {
                _db.Database.ExecuteSqlCommand("sp_rename 'Task_SalesOrder.IsSettled', 'IsSettledByChallan', 'COLUMN'");
            }

            //rename column if found
            if (CheckTable("Task_DeliveryChallan", "IsSettled"))
            {
                _db.Database.ExecuteSqlCommand("sp_rename 'Task_DeliveryChallan.IsSettled', 'IsSettledByInvoice', 'COLUMN'");
            }

            //rename column if found
            if (CheckTable("Task_SalesInvoice", "IsSettled"))
            {
                _db.Database.ExecuteSqlCommand("sp_rename 'Task_SalesInvoice.IsSettled', 'IsSettledByCollection', 'COLUMN'");
            }

            //add column if not found
            if (!CheckTable("Task_PurchaseReturnDetail", "WarehouseId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseReturnDetail ADD WarehouseId [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseReturnDetail ADD CONSTRAINT FK_Task_PurchaseReturnDetail_Setup_Location FOREIGN KEY (WarehouseId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //rename column if found
            if (CheckTable("Task_GoodsReceive", "IsSettled"))
            {
                _db.Database.ExecuteSqlCommand("sp_rename 'Task_GoodsReceive.IsSettled', 'IsSettledByRecFinalize', 'COLUMN'");
            }

            //add column if not found
            if (!CheckTable("Task_PurchaseReturn", "Remarks"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseReturn ADD Remarks [varchar](500) NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PurchaseReturn ADD Reason [varchar](500) NULL");
            }

            //drop table if found
            if (CheckTable("Task_ComplainReceiveDetail_Charge", string.Empty))
            {
                _db.Database.ExecuteSqlCommand("DROP TABLE Task_ComplainReceiveDetail_Charge");
            }

            //if table not found then create table
            if (!CheckTable("Task_ComplainReceive_Charge"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_ComplainReceive_Charge(
	                [ReceiveChargeId] [uniqueidentifier] NOT NULL,
	                [ReceiveId] [uniqueidentifier] NOT NULL,
	                [ChargeEventId] [bigint] NOT NULL,
	                [ChargeAmount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Charge1Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Charge2Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
                 CONSTRAINT [PK_Task_ComplainReceive_Charge] PRIMARY KEY CLUSTERED ([ReceiveChargeId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ComplainReceive_Charge ADD CONSTRAINT FK_Task_ComplainReceive_Charge_Task_ComplainReceive FOREIGN KEY (ReceiveId) REFERENCES Task_ComplainReceive (ReceiveId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ComplainReceive_Charge ADD CONSTRAINT FK_Task_ComplainReceive_Charge_Configuration_EventWiseCharge FOREIGN KEY (ChargeEventId) REFERENCES Configuration_EventWiseCharge (ChargeEventId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //drop column if found
            if (CheckTable("Task_ComplainReceive", "TotalSparePartsDiscount"))
            {
                DropDefaultConstraintByColumn("Task_ComplainReceive", "TotalSparePartsDiscount");
                DropDefaultConstraintByColumn("Task_ComplainReceive", "TotalSpareParts1Discount");
                DropDefaultConstraintByColumn("Task_ComplainReceive", "TotalSpareParts2Discount");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ComplainReceive DROP COLUMN TotalSparePartsDiscount");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ComplainReceive DROP COLUMN TotalSpareParts1Discount");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ComplainReceive DROP COLUMN TotalSpareParts2Discount");
            }

            //drop column if found
            if (CheckTable("Task_ComplainReceive", "TotalSparePartsAmount"))
            {
                DropDefaultConstraintByColumn("Task_ComplainReceive", "TotalSparePartsAmount");
                DropDefaultConstraintByColumn("Task_ComplainReceive", "TotalSpareParts1Amount");
                DropDefaultConstraintByColumn("Task_ComplainReceive", "TotalSpareParts2Amount");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ComplainReceive DROP COLUMN TotalSparePartsAmount");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ComplainReceive DROP COLUMN TotalSpareParts1Amount");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ComplainReceive DROP COLUMN TotalSpareParts2Amount");
            }

            //add column if not found
            if (!CheckTable("Task_ComplainReceiveDetail", "TotalSpareAmount"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ComplainReceiveDetail ADD TotalSpareAmount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ComplainReceiveDetail ADD TotalSpareAmount1 [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ComplainReceiveDetail ADD TotalSpareAmount2 [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_ComplainReceiveDetail", "TotalSpareDiscount"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ComplainReceiveDetail ADD TotalSpareDiscount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ComplainReceiveDetail ADD TotalSpareDiscount1 [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ComplainReceiveDetail ADD TotalSpareDiscount2 [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //if table not found then create table
            if (!CheckTable("Task_CustomerDeliveryNos"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_CustomerDeliveryNos(
	                [Id] [uniqueidentifier] NOT NULL,
	                [DeliveryNo] [varchar](50) NOT NULL,
	                [Year] [bigint] NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
                 CONSTRAINT [PK_Task_CustomerDeliveryNos] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
                 CONSTRAINT [IX_Task_CustomerDeliveryNos] UNIQUE NONCLUSTERED (
	                [DeliveryNo] ASC,
	                [CompanyId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDeliveryNos ADD CONSTRAINT FK_Task_CustomerDeliveryNos_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_CustomerDelivery"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_CustomerDelivery(
	                [DeliveryId] [uniqueidentifier] NOT NULL,
	                [DeliveryNo] [varchar](50) NOT NULL,
	                [DeliveryDate] [datetime] NOT NULL,
	                [SelectedCurrency] [varchar](5) NOT NULL,
	                [Currency1Rate] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Currency2Rate] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [RequestedBy] [bigint] NOT NULL,
	                [CustomerId] [bigint] NOT NULL,
	                [TotalAmount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [TotalAmount1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [TotalAmount2] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [TotalChargeAmount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [TotalChargeAmount1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [TotalChargeAmount2] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Discount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Discount1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Discount2] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [Remarks] [varchar](500) NULL,
	                [Approved] [varchar](1) NOT NULL,
	                [ApprovedBy] [bigint] NULL,
	                [ApprovedDate] [datetime] NULL,
	                [CancelReason] [varchar](200) NULL,
	                [LocationId] [bigint] NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
	                [EntryBy] [bigint] NOT NULL,
	                [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Task_CustomerDelivery] PRIMARY KEY CLUSTERED ([DeliveryId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
                 CONSTRAINT [IX_Task_CustomerDelivery] UNIQUE NONCLUSTERED (
	                [DeliveryNo] ASC,
	                [CompanyId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDelivery ADD CONSTRAINT FK_Task_CustomerDelivery_Setup_Employee FOREIGN KEY (RequestedBy) REFERENCES Setup_Employee (EmployeeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDelivery ADD CONSTRAINT FK_Task_CustomerDelivery_Security_User FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDelivery ADD CONSTRAINT FK_Task_CustomerDelivery_Security_User1 FOREIGN KEY (ApprovedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDelivery ADD CONSTRAINT FK_Task_CustomerDelivery_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDelivery ADD CONSTRAINT FK_Task_CustomerDelivery_Setup_Customer FOREIGN KEY (CustomerId) REFERENCES Setup_Customer (CustomerId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDelivery ADD CONSTRAINT FK_Task_CustomerDelivery_Setup_Location FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Treated as sum of all product(s) amout.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_CustomerDelivery', @level2type=N'COLUMN',@level2name=N'TotalAmount'");
            }

            //add column if not found
            if (!CheckTable("Task_CustomerDelivery", "Approved"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDelivery ADD RequestedBy [bigint] NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDelivery ADD Approved [varchar](1) DEFAULT 'N' NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDelivery ADD ApprovedBy [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDelivery ADD ApprovedDate [datetime] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDelivery ADD CancelReason [varchar](200) NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDelivery ADD Remarks [varchar](500) NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDelivery ADD CONSTRAINT FK_Task_CustomerDelivery_Setup_Employee FOREIGN KEY (RequestedBy) REFERENCES Setup_Employee (EmployeeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDelivery ADD CONSTRAINT FK_Task_CustomerDelivery_Security_User1 FOREIGN KEY (ApprovedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_CustomerDelivery_Charge"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_CustomerDelivery_Charge(
	                [DeliveryChargeId] [uniqueidentifier] NOT NULL,
	                [DeliveryId] [uniqueidentifier] NOT NULL,
	                [ChargeEventId] [bigint] NOT NULL,
	                [ChargeAmount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Charge1Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Charge2Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
                 CONSTRAINT [PK_Task_CustomerDelivery_Charge] PRIMARY KEY CLUSTERED ([DeliveryChargeId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDelivery_Charge ADD CONSTRAINT FK_Task_CustomerDelivery_Charge_Configuration_EventWiseCharge FOREIGN KEY (ChargeEventId) REFERENCES Configuration_EventWiseCharge (ChargeEventId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDelivery_Charge ADD CONSTRAINT FK_Task_CustomerDelivery_Charge_Task_CustomerDelivery FOREIGN KEY (DeliveryId) REFERENCES Task_CustomerDelivery (DeliveryId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_CustomerDeliveryDetail"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_CustomerDeliveryDetail(
	                [DeliveryDetailId] [uniqueidentifier] NOT NULL,
	                [DeliveryId] [uniqueidentifier] NOT NULL,
	                [ComplainReceiveId] [uniqueidentifier] NOT NULL,
	                [PreviousProductId] [bigint] NOT NULL,
	                [PreviousProductDimensionId] [bigint] NULL,
	                [PreviousUnitTypeId] [bigint] NOT NULL,
	                [PreviousSerial] [varchar](100) NOT NULL,
	                [NewProductId] [bigint] NOT NULL,
	                [NewProductDimensionId] [bigint] NULL,
	                [NewUnitTypeId] [bigint] NOT NULL,
	                [NewSerial] [varchar](100) NOT NULL,
	                [Cost] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Cost1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Cost2] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [IsAdjustmentRequired] [bit] DEFAULT 0 NOT NULL,
	                [AdjustmentType] [varchar](1) NULL,
	                [AdjustedAmount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [AdjustedAmount1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [AdjustedAmount2] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [DeliveryType] [bigint] NOT NULL,
	                [TotalSpareAmount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [TotalSpareAmount1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [TotalSpareAmount2] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [TotalSpareDiscount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [TotalSpareDiscount1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [TotalSpareDiscount2] [numeric](18, 4) DEFAULT 0 NOT NULL,
                 CONSTRAINT [PK_Task_CustomerDeliveryDetail] PRIMARY KEY CLUSTERED ([DeliveryDetailId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDeliveryDetail ADD CONSTRAINT FK_Task_CustomerDeliveryDetail_Setup_Product FOREIGN KEY (PreviousProductId) REFERENCES Setup_Product (ProductId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDeliveryDetail ADD CONSTRAINT FK_Task_CustomerDeliveryDetail_Setup_Product1 FOREIGN KEY (NewProductId) REFERENCES Setup_Product (ProductId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDeliveryDetail ADD CONSTRAINT FK_Task_CustomerDeliveryDetail_Setup_ProductDimension FOREIGN KEY (PreviousProductDimensionId) REFERENCES Setup_ProductDimension (ProductDimensionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDeliveryDetail ADD CONSTRAINT FK_Task_CustomerDeliveryDetail_Setup_ProductDimension1 FOREIGN KEY (NewProductDimensionId) REFERENCES Setup_ProductDimension (ProductDimensionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDeliveryDetail ADD CONSTRAINT FK_Task_CustomerDeliveryDetail_Setup_UnitType FOREIGN KEY (PreviousUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDeliveryDetail ADD CONSTRAINT FK_Task_CustomerDeliveryDetail_Setup_UnitType1 FOREIGN KEY (NewUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDeliveryDetail ADD CONSTRAINT FK_Task_CustomerDeliveryDetail_Task_ComplainReceive FOREIGN KEY (ComplainReceiveId) REFERENCES Task_ComplainReceive (ReceiveId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDeliveryDetail ADD CONSTRAINT FK_Task_CustomerDeliveryDetail_Task_CustomerDelivery FOREIGN KEY (DeliveryId) REFERENCES Task_CustomerDelivery (DeliveryId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If need any adjustment required against every product then it will true' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_CustomerDeliveryDetail', @level2type=N'COLUMN',@level2name=N'IsAdjustmentRequired'");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If adjustment required true then this field will be either D or A. D = Deduction, A = Addition' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_CustomerDeliveryDetail', @level2type=N'COLUMN',@level2name=N'AdjustmentType'");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1 = Same Product Same Serial, 2 = Same Product Different Serial, 3 = Different Product Different Serial, 4 = Cash Back' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_CustomerDeliveryDetail', @level2type=N'COLUMN',@level2name=N'DeliveryType'");
            }

            //if table not found then create table
            if (!CheckTable("Task_CustomerDeliveryDetail_Problem"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_CustomerDeliveryDetail_Problem(
	                [DeliveryDetailProblemId] [uniqueidentifier] NOT NULL,
	                [DeliveryDetailId] [uniqueidentifier] NOT NULL,
	                [ProblemId] [bigint] NOT NULL,
	                [Note] [varchar](200) NULL,
                 CONSTRAINT [PK_Task_CustomerDeliveryDetail_Problem] PRIMARY KEY CLUSTERED ([DeliveryDetailProblemId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDeliveryDetail_Problem ADD CONSTRAINT FK_Task_CustomerDeliveryDetail_Problem_Setup_Problem FOREIGN KEY (ProblemId) REFERENCES Setup_Problem (ProblemId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDeliveryDetail_Problem ADD CONSTRAINT FK_Task_CustomerDeliveryDetail_Problem_Task_CustomerDeliveryDetail FOREIGN KEY (DeliveryDetailId) REFERENCES Task_CustomerDeliveryDetail (DeliveryDetailId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_CustomerDeliveryDetail_SpareProduct"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_CustomerDeliveryDetail_SpareProduct(
	                [DeliveryDetailSpareId] [uniqueidentifier] NOT NULL,
	                [DeliveryDetailId] [uniqueidentifier] NOT NULL,
	                [ProductId] [bigint] NOT NULL,
	                [ProductDimensionId] [bigint] NULL,
	                [UnitTypeId] [bigint] NOT NULL,
	                [Quantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Price] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Price1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Price2] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Discount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Discount1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Discount2] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [SpareType] [varchar](1) DEFAULT ('R') NOT NULL,
	                [Remarks] [varchar](100) NULL,
                 CONSTRAINT [PK_Task_CustomerDeliveryDetail_SpareProduct] PRIMARY KEY CLUSTERED ([DeliveryDetailSpareId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDeliveryDetail_SpareProduct ADD CONSTRAINT FK_Task_CustomerDeliveryDetail_SpareProduct_Setup_Product FOREIGN KEY (ProductId) REFERENCES Setup_Product (ProductId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDeliveryDetail_SpareProduct ADD CONSTRAINT FK_Task_CustomerDeliveryDetail_SpareProduct_Setup_ProductDimension FOREIGN KEY (ProductDimensionId) REFERENCES Setup_ProductDimension (ProductDimensionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDeliveryDetail_SpareProduct ADD CONSTRAINT FK_Task_CustomerDeliveryDetail_SpareProduct_Setup_UnitType FOREIGN KEY (UnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDeliveryDetail_SpareProduct ADD CONSTRAINT FK_Task_CustomerDeliveryDetail_SpareProduct_Task_CustomerDeliveryDetail FOREIGN KEY (DeliveryDetailId) REFERENCES Task_CustomerDeliveryDetail (DeliveryDetailId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Default R. R = Replaced, N = New' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_CustomerDeliveryDetail_SpareProduct', @level2type=N'COLUMN',@level2name=N'SpareType'");
            }

            //add column if not found
            if (!CheckTable("Task_SalesInvoice", "ReferenceNo"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD ReferenceNo [varchar](50) NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD ReferenceDate [datetime] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD Remarks [varchar](500) NULL");
            }

            //add column if not found
            if (!CheckTable("Configuration_PaymentMode", "IsCashType"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_PaymentMode ADD IsCashType [bit] DEFAULT 1 NOT NULL");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Default True. True = Cash Type, False = Credit Type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Configuration_PaymentMode', @level2type=N'COLUMN',@level2name=N'IsCashType'");
            }

            //add column if not found
            if (!CheckTable("Task_SalesReturn", "Remarks"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesReturn ADD Remarks [varchar](500) NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesReturn ADD Reason [varchar](500) NULL");
            }

            //add column if not found
            if (!CheckTable("Task_SalesInvoice", "IsSalesModeCash"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD IsSalesModeCash [bit] DEFAULT 1 NOT NULL");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Default True. True = Cash Mode, False = Credit Mode' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_SalesInvoice', @level2type=N'COLUMN',@level2name=N'IsSalesModeCash'");
            }

            //add column if not found
            if (!CheckTable("Task_SalesOrder", "IsSalesModeCash"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ADD IsSalesModeCash [bit] DEFAULT 1 NOT NULL");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Default True. True = Cash Mode, False = Credit Mode' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_SalesOrder', @level2type=N'COLUMN',@level2name=N'IsSalesModeCash'");
            }

            //rename column if found
            if (CheckTable("Task_TransferOrder", "IsSettled"))
            {
                _db.Database.ExecuteSqlCommand("sp_rename 'Task_TransferOrder.IsSettled', 'IsSettledByChallan', 'COLUMN'");
            }

            //add column if not found
            if (!CheckTable("Task_TransferChallanDetail", "PrimaryUnitTypeId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferChallanDetail ADD PrimaryUnitTypeId [bigint] NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferChallanDetail ADD SecondaryUnitTypeId [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferChallanDetail ADD TertiaryUnitTypeId [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferChallanDetail ADD SecondaryConversionRatio [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferChallanDetail ADD TertiaryConversionRatio [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_TransferChallanDetail", "FK_Task_TransferChallanDetail_Setup_UnitType1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferChallanDetail ADD CONSTRAINT FK_Task_TransferChallanDetail_Setup_UnitType1 FOREIGN KEY (PrimaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferChallanDetail ADD CONSTRAINT FK_Task_TransferChallanDetail_Setup_UnitType2 FOREIGN KEY (SecondaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferChallanDetail ADD CONSTRAINT FK_Task_TransferChallanDetail_Setup_UnitType3 FOREIGN KEY (TertiaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //rename column if found
            if (CheckTable("Task_TransferChallan", "IsSettled"))
            {
                _db.Database.ExecuteSqlCommand("sp_rename 'Task_TransferChallan.IsSettled', 'IsSettledByReceive', 'COLUMN'");
            }

            //add column if not found
            if (!CheckTable("Setup_Company", "VATRegNo"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Company ADD VATRegNo [varchar](20) NULL");
            }

            //add column if not found
            if (!CheckTable("Task_Voucher", "OperationalEventId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Voucher ADD OperationalEventId [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Voucher ADD CONSTRAINT FK_Task_Voucher_Configuration_OperationalEvent FOREIGN KEY (OperationalEventId) REFERENCES Configuration_OperationalEvent (OperationalEventId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_PostedVoucher", "OperationalEventId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PostedVoucher ADD OperationalEventId [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PostedVoucher ADD CONSTRAINT FK_Task_PostedVoucher_Configuration_OperationalEvent FOREIGN KEY (OperationalEventId) REFERENCES Configuration_OperationalEvent (OperationalEventId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_TransferReceiveDetail", "PrimaryUnitTypeId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferReceiveDetail ADD PrimaryUnitTypeId [bigint] NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferReceiveDetail ADD SecondaryUnitTypeId [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferReceiveDetail ADD TertiaryUnitTypeId [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferReceiveDetail ADD SecondaryConversionRatio [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferReceiveDetail ADD TertiaryConversionRatio [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_TransferReceiveDetail", "FK_Task_TransferReceiveDetail_Setup_UnitType1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferReceiveDetail ADD CONSTRAINT FK_Task_TransferReceiveDetail_Setup_UnitType1 FOREIGN KEY (PrimaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferReceiveDetail ADD CONSTRAINT FK_Task_TransferReceiveDetail_Setup_UnitType2 FOREIGN KEY (SecondaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferReceiveDetail ADD CONSTRAINT FK_Task_TransferReceiveDetail_Setup_UnitType3 FOREIGN KEY (TertiaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //rename column if found
            if (CheckTable("Task_TransferReceiveDetail", "WareHouseId"))
            {
                _db.Database.ExecuteSqlCommand("sp_rename 'Task_TransferReceiveDetail.WareHouseId', 'ReceivedWareHouseId', 'COLUMN'");
            }

            //add column if not found
            if (!CheckTable("Task_TransferReceiveDetail", "TranFromWareHouseId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferReceiveDetail ADD TranFromWareHouseId [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferReceiveDetail ADD CONSTRAINT FK_Task_TransferReceiveDetail_Setup_Location1 FOREIGN KEY (TranFromWareHouseId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_TransferChallan", "TransferOperationType"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferChallan ADD TransferOperationType [varchar](1) DEFAULT 'R' NOT NULL");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'R = Regular Transfer, D = Direct Transfer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_TransferChallan', @level2type=N'COLUMN',@level2name=N'TransferOperationType'");
            }

            //add column if not found
            if (!CheckTable("Task_TransferChallanDetail", "ToWareHouseId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferChallanDetail ADD ToWareHouseId [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferChallanDetail ADD CONSTRAINT FK_Task_TransferChallanDetail_Setup_Location1 FOREIGN KEY (ToWareHouseId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_ReplacementClaimNos"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_ReplacementClaimNos(
	                [Id] [uniqueidentifier] NOT NULL,
	                [ClaimNo] [varchar](50) NOT NULL,
	                [Year] [bigint] NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
                 CONSTRAINT [PK_Task_ReplacementClaimNos] PRIMARY KEY CLUSTERED([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
                 CONSTRAINT [IX_Task_ReplacementClaimNos] UNIQUE NONCLUSTERED(
	                [ClaimNo] ASC,
	                [CompanyId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementClaimNos ADD CONSTRAINT FK_Task_ReplacementClaimNos_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_ReplacementClaim"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_ReplacementClaim(
	                [ClaimId] [uniqueidentifier] NOT NULL,
	                [ClaimNo] [varchar](50) NOT NULL,
	                [ClaimDate] [datetime] NOT NULL,
	                [SelectedCurrency] [varchar](5) NOT NULL,
	                [Currency1Rate] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Currency2Rate] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [RequestedBy] [bigint] NOT NULL,
	                [SupplierId] [bigint] NOT NULL,
	                [Remarks] [varchar](500) NULL,
	                [IsSettledByReplacementReceive] [bit] DEFAULT 0 NOT NULL,
	                [Approved] [varchar](1) NOT NULL,
	                [ApprovedBy] [bigint] NULL,
	                [ApprovedDate] [datetime] NULL,
	                [CancelReason] [varchar](200) NULL,
	                [LocationId] [bigint] NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
	                [EntryBy] [bigint] NOT NULL,
	                [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Task_ReplacementClaim] PRIMARY KEY CLUSTERED ([ClaimId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementClaim ADD CONSTRAINT FK_Task_ReplacementClaim_Security_User FOREIGN KEY (ApprovedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementClaim ADD CONSTRAINT FK_Task_ReplacementClaim_Security_User1 FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementClaim ADD CONSTRAINT FK_Task_ReplacementClaim_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementClaim ADD CONSTRAINT FK_Task_ReplacementClaim_Setup_Employee FOREIGN KEY (RequestedBy) REFERENCES Setup_Employee (EmployeeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementClaim ADD CONSTRAINT FK_Task_ReplacementClaim_Setup_Location FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementClaim ADD CONSTRAINT FK_Task_ReplacementClaim_Setup_Supplier FOREIGN KEY (SupplierId) REFERENCES Setup_Supplier (SupplierId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_ReplacementClaimDetail"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_ReplacementClaimDetail(
	                ClaimDetailId uniqueidentifier NOT NULL,
	                ClaimId uniqueidentifier NOT NULL,
	                ComplainReceiveId uniqueidentifier NULL,
	                ProductId bigint NOT NULL,
	                ProductDimensionId bigint NULL,
	                UnitTypeId bigint NOT NULL,
	                Serial varchar(100) NOT NULL,
	                AdditionalSerial varchar(100) NULL,
	                ReceivedSerialNo varchar(100) NULL,
	                ReceivedAdditionalSerial varchar(100) NULL,
	                IsSettled bit DEFAULT 0 NOT NULL,
	                StockInBy varchar(20) NULL,
	                StockInRefNo varchar(50) NULL,
	                StockInRefDate datetime NULL,
	                ReferenceNo varchar(50) NULL,
	                ReferenceDate datetime NULL,
                CONSTRAINT[PK_Task_ReplacementClaimDetail] PRIMARY KEY CLUSTERED([ClaimDetailId] ASC) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]) ON[PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementClaimDetail ADD CONSTRAINT FK_Task_ReplacementClaimDetail_Setup_Product FOREIGN KEY (ProductId) REFERENCES Setup_Product (ProductId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementClaimDetail ADD CONSTRAINT FK_Task_ReplacementClaimDetail_Setup_ProductDimension FOREIGN KEY (ProductDimensionId) REFERENCES Setup_ProductDimension (ProductDimensionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementClaimDetail ADD CONSTRAINT FK_Task_ReplacementClaimDetail_Setup_UnitType FOREIGN KEY (UnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementClaimDetail ADD CONSTRAINT FK_Task_ReplacementClaimDetail_Task_ComplainReceive FOREIGN KEY (ComplainReceiveId) REFERENCES Task_ComplainReceive (ReceiveId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementClaimDetail ADD CONSTRAINT FK_Task_ReplacementClaimDetail_Task_ReplacementClaim FOREIGN KEY (ClaimId) REFERENCES Task_ReplacementClaim (ClaimId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Import / Purchase / Otherwise remain Null' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_ReplacementClaimDetail', @level2type=N'COLUMN',@level2name=N'StockInBy'");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Import / Purchase No.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_ReplacementClaimDetail', @level2type=N'COLUMN',@level2name=N'StockInRefNo'");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Import / Purchase Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_ReplacementClaimDetail', @level2type=N'COLUMN',@level2name=N'StockInRefDate'");
            }

            //if table not found then create table
            if (!CheckTable("Task_ReplacementClaimDetail_Problem"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_ReplacementClaimDetail_Problem(
	                ClaimDetailProblemId uniqueidentifier NOT NULL,
	                ClaimDetailId uniqueidentifier NOT NULL,
	                ProblemId bigint NOT NULL,
	                Note varchar(200) NULL,
                CONSTRAINT[PK_Task_ReplacementClaimDetail_Problem] PRIMARY KEY CLUSTERED([ClaimDetailProblemId] ASC) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]) ON[PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementClaimDetail_Problem ADD CONSTRAINT FK_Task_ReplacementClaimDetail_Problem_Setup_Problem FOREIGN KEY (ProblemId) REFERENCES Setup_Problem (ProblemId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementClaimDetail_Problem ADD CONSTRAINT FK_Task_ReplacementClaimDetail_Problem_Task_ReplacementClaimDetail FOREIGN KEY (ClaimDetailId) REFERENCES Task_ReplacementClaimDetail (ClaimDetailId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Temp_CustomerSupplierOutstanding"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Temp_CustomerSupplierOutstanding(
	                TempId uniqueidentifier NOT NULL,
	                ReportFor varchar(1) NOT NULL,
	                CustomerId bigint NULL,
	                SupplierId bigint NULL,
	                LedgerEndingBalance numeric(18, 4) DEFAULT 0 NOT NULL,
	                ReceivedChequeNotTreatedAmount numeric(18, 4) DEFAULT 0 NOT NULL,
	                ReceivedDishonourChequeAmount numeric(18, 4) DEFAULT 0 NOT NULL,
	                IssuedChequeNotTreatedAmount numeric(18, 4) DEFAULT 0 NOT NULL,
	                IssuedDishonourChequeAmount numeric(18, 4) DEFAULT 0 NOT NULL,
	                CompanyId bigint NOT NULL,
	                EntryBy bigint NOT NULL,
                CONSTRAINT[PK_Temp_CustomerSupplierOutstanding] PRIMARY KEY CLUSTERED([TempId] ASC) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]) ON[PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_CustomerSupplierOutstanding ADD CONSTRAINT FK_Temp_CustomerSupplierOutstanding_Security_User FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_CustomerSupplierOutstanding ADD CONSTRAINT FK_Temp_CustomerSupplierOutstanding_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_CustomerSupplierOutstanding ADD CONSTRAINT FK_Temp_CustomerSupplierOutstanding_Setup_Customer FOREIGN KEY (CustomerId) REFERENCES Setup_Customer (CustomerId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_CustomerSupplierOutstanding ADD CONSTRAINT FK_Temp_CustomerSupplierOutstanding_Setup_Supplier FOREIGN KEY (SupplierId) REFERENCES Setup_Supplier (SupplierId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'C = Customer, S = Supplier' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Temp_CustomerSupplierOutstanding', @level2type=N'COLUMN',@level2name=N'ReportFor'");
            }

            //rename column if found
            if (CheckTable("Task_ReplacementClaimDetail", "ReferenceNo"))
            {
                _db.Database.ExecuteSqlCommand("sp_rename 'Task_ReplacementClaimDetail.ReferenceNo', 'LCOrReferenceNo', 'COLUMN'");
                _db.Database.ExecuteSqlCommand("sp_rename 'Task_ReplacementClaimDetail.ReferenceDate', 'LCOrReferenceDate', 'COLUMN'");
            }

            //add column if not found
            if (!CheckTable("Task_ReplacementClaimDetail", "ProformaInvoiceNo"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementClaimDetail ADD ProformaInvoiceNo [varchar](50) NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementClaimDetail ADD ProformaInvoiceDate [datetime] NULL");
            }

            //add column if not found
            if (!CheckTable("Task_CustomerDeliveryDetail", "TotalServiceAmount"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDeliveryDetail ADD TotalServiceAmount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDeliveryDetail ADD TotalServiceAmount1 [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDeliveryDetail ADD TotalServiceAmount2 [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDeliveryDetail ADD TotalServiceDiscount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDeliveryDetail ADD TotalServiceDiscount1 [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDeliveryDetail ADD TotalServiceDiscount2 [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_ComplainReceiveDetail", "TotalServiceAmount"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ComplainReceiveDetail ADD TotalServiceAmount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ComplainReceiveDetail ADD TotalServiceAmount1 [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ComplainReceiveDetail ADD TotalServiceAmount2 [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ComplainReceiveDetail ADD TotalServiceDiscount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ComplainReceiveDetail ADD TotalServiceDiscount1 [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ComplainReceiveDetail ADD TotalServiceDiscount2 [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_GoodsReceive", "EditedBy"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceive ADD EditedBy [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceive ADD EditedDate [datetime] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceive ADD CONSTRAINT FK_Task_GoodsReceive_Security_User2 FOREIGN KEY (EditedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_SalesInvoice", "CustomerDetail"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD CustomerDetail [varchar](50) NULL");
            }

            //if table not found then create table
            if (!CheckTable("Task_ReplacementReceiveNos"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_ReplacementReceiveNos(
	                [Id] [uniqueidentifier] NOT NULL,
	                [ReceiveNo] [varchar](50) NOT NULL,
	                [Year] [bigint] NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
                 CONSTRAINT [PK_Task_ReplacementReceiveNos] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
                 CONSTRAINT [IX_Task_ReplacementReceiveNos] UNIQUE NONCLUSTERED ([ReceiveNo] ASC, [CompanyId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementReceiveNos ADD CONSTRAINT FK_Task_ReplacementReceiveNos_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_ReplacementReceive"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_ReplacementReceive(
	                [ReceiveId] [uniqueidentifier] NOT NULL,
	                [ReceiveNo] [varchar](50) NOT NULL,
	                [ReceiveDate] [datetime] NOT NULL,
	                [SelectedCurrency] [varchar](5) NOT NULL,
	                [Currency1Rate] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Currency2Rate] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [RequestedBy] [bigint] NOT NULL,
	                [Remarks] [varchar](500) NULL,
	                [Approved] [varchar](1) DEFAULT ('N') NOT NULL,
	                [ApprovedBy] [bigint] NULL,
	                [ApprovedDate] [datetime] NULL,
	                [CancelReason] [varchar](200) NULL,
	                [LocationId] [bigint] NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
	                [EntryBy] [bigint] NOT NULL,
	                [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Task_ReplacementReceive] PRIMARY KEY CLUSTERED ([ReceiveId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
                 CONSTRAINT [IX_Task_ReplacementReceive] UNIQUE NONCLUSTERED ([ReceiveNo] ASC, [CompanyId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementReceive ADD CONSTRAINT FK_Task_ReplacementReceive_Setup_Employee FOREIGN KEY (RequestedBy) REFERENCES Setup_Employee (EmployeeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementReceive ADD CONSTRAINT FK_Task_ReplacementReceive_Security_User FOREIGN KEY (ApprovedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementReceive ADD CONSTRAINT FK_Task_ReplacementReceive_Setup_Location FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementReceive ADD CONSTRAINT FK_Task_ReplacementReceive_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementReceive ADD CONSTRAINT FK_Task_ReplacementReceive_Security_User1 FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_ReplacementReceiveDetail"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_ReplacementReceiveDetail(
	                [ReceiveDetailId] [uniqueidentifier] NOT NULL,
	                [ReceiveId] [uniqueidentifier] NOT NULL,
	                [ReplacementClaimId] [uniqueidentifier] NOT NULL,
	                [PreviousProductId] [bigint] NOT NULL,
	                [PreviousProductDimensionId] [bigint] NULL,
	                [PreviousUnitTypeId] [bigint] NOT NULL,
	                [PreviousSerial] [varchar](100) NOT NULL,
	                [NewProductId] [bigint] NOT NULL,
	                [NewProductDimensionId] [bigint] NULL,
	                [NewUnitTypeId] [bigint] NOT NULL,
	                [NewSerial] [varchar](100) NOT NULL,
	                [Cost] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Cost1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Cost2] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [ReplacementType] [bigint] NOT NULL,
	                [AdjustmentType] [varchar](1) NULL,
	                [AdjustedAmount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [AdjustedAmount1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [AdjustedAmount2] [numeric](18, 4) DEFAULT 0 NOT NULL,
                 CONSTRAINT [PK_Task_ReplacementReceiveDetail] PRIMARY KEY CLUSTERED ([ReceiveDetailId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementReceiveDetail ADD CONSTRAINT FK_Task_ReplacementReceiveDetail_Setup_Product FOREIGN KEY (PreviousProductId) REFERENCES Setup_Product (ProductId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementReceiveDetail ADD CONSTRAINT FK_Task_ReplacementReceiveDetail_Setup_Product1 FOREIGN KEY (NewProductId) REFERENCES Setup_Product (ProductId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementReceiveDetail ADD CONSTRAINT FK_Task_ReplacementReceiveDetail_Setup_ProductDimension FOREIGN KEY (PreviousProductDimensionId) REFERENCES Setup_ProductDimension (ProductDimensionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementReceiveDetail ADD CONSTRAINT FK_Task_ReplacementReceiveDetail_Setup_ProductDimension1 FOREIGN KEY (NewProductDimensionId) REFERENCES Setup_ProductDimension (ProductDimensionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementReceiveDetail ADD CONSTRAINT FK_Task_ReplacementReceiveDetail_Setup_UnitType FOREIGN KEY (PreviousUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementReceiveDetail ADD CONSTRAINT FK_Task_ReplacementReceiveDetail_Setup_UnitType1 FOREIGN KEY (NewUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementReceiveDetail ADD CONSTRAINT FK_Task_ReplacementReceiveDetail_Task_ReplacementClaim FOREIGN KEY (ReplacementClaimId) REFERENCES Task_ReplacementClaim (ClaimId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementReceiveDetail ADD CONSTRAINT FK_Task_ReplacementReceiveDetail_Task_ReplacementReceive FOREIGN KEY (ReceiveId) REFERENCES Task_ReplacementReceive (ReceiveId) ON UPDATE NO ACTION ON DELETE NO ACTION");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1 = Same Product Same Serial, 2 = Same Product Different Serial, 3 = Different Product Different Serial, 4 = Cash Back' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_ReplacementReceiveDetail', @level2type=N'COLUMN',@level2name=N'ReplacementType'");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If adjustment required true then this field will be either D or A. D = Deduction, A = Addition' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_ReplacementReceiveDetail', @level2type=N'COLUMN',@level2name=N'AdjustmentType'");
            }

            //if table not found then create table
            if (!CheckTable("Task_ReplacementReceive_Charge"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_ReplacementReceive_Charge(
	                [ReceiveChargeId] [uniqueidentifier] NOT NULL,
	                [ReceiveId] [uniqueidentifier] NOT NULL,
	                [ChargeEventId] [bigint] NOT NULL,
	                [ChargeAmount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Charge1Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Charge2Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
                 CONSTRAINT [PK_Task_ReplacementReceive_Charge] PRIMARY KEY CLUSTERED ([ReceiveChargeId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementReceive_Charge ADD CONSTRAINT FK_Task_ReplacementReceive_Charge_Task_ReplacementReceive FOREIGN KEY (ReceiveId) REFERENCES Task_ReplacementReceive (ReceiveId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementReceive_Charge ADD CONSTRAINT FK_Task_ReplacementReceive_Charge_Configuration_EventWiseCharge FOREIGN KEY (ChargeEventId) REFERENCES Configuration_EventWiseCharge (ChargeEventId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_ReplacementReceive", "TotalChargeAmount"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementReceive ADD TotalChargeAmount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementReceive ADD TotalChargeAmount1 [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementReceive ADD TotalChargeAmount2 [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementReceive ADD TotalDiscount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementReceive ADD TotalDiscount1 [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementReceive ADD TotalDiscount2 [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Setup_Location", "POSPrinterWidth"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Location ADD POSPrinterWidth [bigint] DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Width must set in Mili Meter (mm)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Setup_Location', @level2type=N'COLUMN',@level2name=N'POSPrinterWidth'");
            }

            //add column if not found
            if (!CheckTable("Configuration_OperationalEventDetail", "DefaultCustomerId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_OperationalEventDetail ADD DefaultCustomerId [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_OperationalEventDetail ADD DefaultPaymentModeId [bigint] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_OperationalEventDetail ADD CONSTRAINT FK_Configuration_OperationalEventDetail_Setup_Customer FOREIGN KEY (DefaultCustomerId) REFERENCES Setup_Customer (CustomerId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_OperationalEventDetail ADD CONSTRAINT FK_Configuration_OperationalEventDetail_Configuration_PaymentMode1 FOREIGN KEY (DefaultPaymentModeId) REFERENCES Configuration_PaymentMode (PaymentModeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            ////if table not found then create table
            //if (!CheckTable("Others_ReportHeaderConfig"))
            //{
            //    _db.Database.ExecuteSqlCommand(@"CREATE TABLE Others_ReportHeaderConfig(
            //     [ReportHeadingId] [bigint] NOT NULL,
            //     [OthersReportId] [bigint] NOT NULL,
            //     [ShowHeader] [bit] DEFAULT ((1)) NOT NULL,
            //     [HeaderHeight] [bigint] DEFAULT ((0)) NOT NULL,
            //     [CompanyId] [bigint] NOT NULL,
            //     CONSTRAINT [PK_Others_ReportHeaderConfig] PRIMARY KEY CLUSTERED ([ReportHeadingId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

            //    _db.Database.ExecuteSqlCommand("ALTER TABLE Others_ReportHeaderConfig ADD CONSTRAINT FK_Others_ReportHeaderConfig_Others_Report FOREIGN KEY (OthersReportId) REFERENCES Others_Report (Id) ON UPDATE NO ACTION ON DELETE NO ACTION");
            //    _db.Database.ExecuteSqlCommand("ALTER TABLE Others_ReportHeaderConfig ADD CONSTRAINT FK_Others_ReportHeaderConfig_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");

            //    _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'True = Show Report Header, False = Hide Report Header' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Others_ReportHeaderConfig', @level2type=N'COLUMN',@level2name=N'OthersReportId'");
            //    _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Pixel Value' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Others_ReportHeaderConfig', @level2type=N'COLUMN',@level2name=N'HeaderHeight'");
            //}

            //add column if not found
            if (!CheckTable("Task_ReplacementClaim", "VoucherId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementClaim ADD VoucherId [uniqueidentifier] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementClaim ADD CONSTRAINT FK_Task_ReplacementClaim_Task_Voucher FOREIGN KEY (VoucherId) REFERENCES Task_Voucher (VoucherId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_ReplacementClaimDetail", "Cost"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementClaimDetail ADD Cost [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementClaimDetail ADD Cost1 [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementClaimDetail ADD Cost2 [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ComplainReceiveDetail", "FK_Task_ComplainReceiveDetail_Task_SalesInvoice"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ComplainReceiveDetail ADD CONSTRAINT FK_Task_ComplainReceiveDetail_Task_SalesInvoice FOREIGN KEY (InvoiceId) REFERENCES Task_SalesInvoice (InvoiceId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_ReplacementReceive", "VoucherId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementReceive ADD VoucherId [uniqueidentifier] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementReceive ADD CONSTRAINT FK_Task_ReplacementReceive_Task_Voucher FOREIGN KEY (VoucherId) REFERENCES Task_Voucher (VoucherId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_CustomerDelivery", "VoucherId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDelivery ADD VoucherId [uniqueidentifier] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDelivery ADD CONSTRAINT FK_Task_CustomerDelivery_Task_Voucher FOREIGN KEY (VoucherId) REFERENCES Task_Voucher (VoucherId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_SalesOrder", "TransactionFrom"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ADD TransactionFrom [varchar](10) DEFAULT 'Current' NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_DeliveryChallan", "TransactionFrom"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallan ADD TransactionFrom [varchar](10) DEFAULT 'Current' NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_SalesInvoice", "TransactionFrom"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD TransactionFrom [varchar](10) DEFAULT 'Current' NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_DeliveryChallan", "CustomerDetail"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallan ADD CustomerDetail [varchar](50) NULL");
            }

            //add column if not found
            if (!CheckTable("Task_PartyAdjustment", "TransactionFrom"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PartyAdjustment ADD TransactionFrom [varchar](10) DEFAULT 'Direct' NOT NULL");
            }

            //drop column if found
            if (CheckTable("Task_SalesOrder", "StockType"))
            {
                DropDefaultConstraintByColumn("Task_SalesOrder", "StockType");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder DROP COLUMN StockType");
            }

            //drop column if found
            if (CheckTable("Task_DeliveryChallan", "StockType"))
            {
                DropDefaultConstraintByColumn("Task_DeliveryChallan", "StockType");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallan DROP COLUMN StockType");
            }

            //drop column if found
            if (CheckTable("Task_SalesInvoice", "StockType"))
            {
                DropDefaultConstraintByColumn("Task_SalesInvoice", "StockType");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice DROP COLUMN StockType");
            }

            //add column if not found
            if (!CheckTable("Task_SalesOrder", "CustomerDetail"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ADD CustomerDetail [varchar](50) NULL");
            }

            //add column if not found
            if (!CheckTable("Task_SalesInvoice", "InvoiceDiscount1Value"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD InvoiceDiscount1Value [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD InvoiceDiscount2Value [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD Commission1Value [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD Commission2Value [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_SalesOrder", "EditedBy"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ADD EditedBy [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ADD EditedDate [datetime] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ADD CONSTRAINT FK_Task_SalesOrder_Security_User2 FOREIGN KEY (EditedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_DeliveryChallan", "EditedBy"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallan ADD EditedBy [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallan ADD EditedDate [datetime] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallan ADD CONSTRAINT FK_Task_DeliveryChallan_Security_User2 FOREIGN KEY (EditedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_SalesInvoice", "EditedBy"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD EditedBy [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD EditedDate [datetime] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD CONSTRAINT FK_Task_SalesInvoice_Security_User2 FOREIGN KEY (EditedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add unique key if not found
            if (!CheckRelationshipBetweenTwoTables("Task_Collection", "IX_Task_Collection"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Collection ADD CONSTRAINT IX_Task_Collection UNIQUE NONCLUSTERED (CollectionNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //add unique key if not found
            if (!CheckRelationshipBetweenTwoTables("Task_Payment", "IX_Task_Payment"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Payment ADD CONSTRAINT IX_Task_Payment UNIQUE NONCLUSTERED (PaymentNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //add unique key if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ReplacementClaim", "IX_Task_ReplacementClaim"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementClaim ADD CONSTRAINT IX_Task_ReplacementClaim UNIQUE NONCLUSTERED (ClaimNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //add unique key if not found
            if (!CheckRelationshipBetweenTwoTables("Task_TransferReceive", "IX_Task_TransferReceive"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferReceive ADD CONSTRAINT IX_Task_TransferReceive UNIQUE NONCLUSTERED (ReceiveNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            ////add column if not found
            //if (!CheckTable("Others_ReportHeaderConfig", "ShowLocationAddress"))
            //{
            //    _db.Database.ExecuteSqlCommand("ALTER TABLE Others_ReportHeaderConfig ADD ShowLocationAddress [bit] DEFAULT 0 NOT NULL");
            //}

            //if table not found then create table
            if (!CheckTable("Task_CustomerDeliveryDetail_SpareSerial"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_CustomerDeliveryDetail_SpareSerial(
                    [DeliveryDetailSpareSerialId] [uniqueidentifier] NOT NULL,
	                [DeliveryDetailSpareId] [uniqueidentifier] NOT NULL,
	                [Serial] [varchar](100) NOT NULL,
	                [AdditionalSerial] [varchar](100) NULL,
                CONSTRAINT [PK_Task_CustomerDeliveryDetail_SpareSerial] PRIMARY KEY CLUSTERED ([DeliveryDetailSpareSerialId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDeliveryDetail_SpareSerial ADD CONSTRAINT FK_Task_CustomerDeliveryDetail_SpareSerial_Task_CustomerDeliveryDetail_SpareProduct FOREIGN KEY (DeliveryDetailSpareId) REFERENCES Task_CustomerDeliveryDetail_SpareProduct (DeliveryDetailSpareId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add unique key if not found
            if (!CheckRelationshipBetweenTwoTables("Setup_Company", "IX_Setup_Company"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Company ADD CONSTRAINT IX_Setup_Company UNIQUE NONCLUSTERED (Code ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //add column if not found
            if (!CheckTable("Task_CustomerDeliveryDetail_SpareProduct", "Cost"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDeliveryDetail_SpareProduct ADD Cost [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDeliveryDetail_SpareProduct ADD Cost1 [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDeliveryDetail_SpareProduct ADD Cost2 [numeric](18, 4) DEFAULT 0 NOT NULL");

                _db.Database.ExecuteSqlCommand("UPDATE Task_CustomerDeliveryDetail_SpareProduct SET Cost = Price - Discount, Cost1 = Price1 - Discount1, Cost2 = Price2 - Discount2");
            }

            //add column if not found
            if (!CheckTable("Setup_Company", "FinStartingMonth"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Company ADD FinStartingMonth [tinyint] DEFAULT 1 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Security_User", "PasswordResetDays"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Security_User ADD PasswordResetDays [smallint] DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Security_User ADD LastResetDate [date] DEFAULT GETDATE() NOT NULL");
            }

            //alter column if found
            if (CheckTable("Setup_Customer", "Address"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Customer ALTER COLUMN Address [varchar](500) NULL");
            }

            //alter column if found
            if (CheckTable("Setup_Supplier", "Address"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Supplier ALTER COLUMN Address [varchar](500) NULL");
            }

            //add column if not found
            if (!CheckTable("Setup_CustomerGroup", "IsDefaultSelected"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_CustomerGroup ADD IsDefaultSelected [bit] DEFAULT 0 NOT NULL");
            }

            //if table not found then create table
            if (!CheckTable("Others_Menu"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Others_Menu(
                    [MenuId] [bigint] NOT NULL,
	                [MenuLevel] [tinyint] NOT NULL,
	                [MenuTitle] [varchar](50) NOT NULL,
	                [MenuOrder] [smallint] NOT NULL,
	                [ParentId] [bigint] NULL,
	                [MenuCode] [varchar](5) NULL,
                 CONSTRAINT [PK_Others_Menu] PRIMARY KEY CLUSTERED ([MenuId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //if table not found then create table
            if (!CheckTable("Security_RoleWiseMenuPermission"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Security_RoleWiseMenuPermission(
                    [Id] [uniqueidentifier] NOT NULL,
	                [LevelId] [bigint] NOT NULL,
	                [RoleId] [bigint] NOT NULL,
	                [MenuId] [bigint] NOT NULL,
	                [Status] [bit] DEFAULT 0 NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
	                [EntryBy] [bigint] NOT NULL,
	                [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Security_RoleWiseMenuPermission] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Security_RoleWiseMenuPermission ADD CONSTRAINT FK_Security_RoleWiseMenuPermission_Security_Level FOREIGN KEY (LevelId) REFERENCES Security_Level (LevelId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Security_RoleWiseMenuPermission ADD CONSTRAINT FK_Security_RoleWiseMenuPermission_Security_Role FOREIGN KEY (RoleId) REFERENCES Security_Role (RoleId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Security_RoleWiseMenuPermission ADD CONSTRAINT FK_Security_RoleWiseMenuPermission_Others_Menu FOREIGN KEY (MenuId) REFERENCES Others_Menu (MenuId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Security_RoleWiseMenuPermission ADD CONSTRAINT FK_Security_RoleWiseMenuPermission_Security_User FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Security_RoleWiseMenuPermission ADD CONSTRAINT FK_Security_RoleWiseMenuPermission_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Security_UserWiseMenuPermission"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Security_UserWiseMenuPermission(
                    [Id] [uniqueidentifier] NOT NULL,
	                [SecurityUserId] [bigint] NOT NULL,
	                [MenuId] [bigint] NOT NULL,
	                [Status] [bit] DEFAULT 0 NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
	                [EntryBy] [bigint] NOT NULL,
	                [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Security_UserWiseMenuPermission] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Security_UserWiseMenuPermission ADD CONSTRAINT FK_Security_UserWiseMenuPermission_Security_User FOREIGN KEY (SecurityUserId) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Security_UserWiseMenuPermission ADD CONSTRAINT FK_Security_UserWiseMenuPermission_Others_Menu FOREIGN KEY (MenuId) REFERENCES Others_Menu (MenuId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Security_UserWiseMenuPermission ADD CONSTRAINT FK_Security_UserWiseMenuPermission_Security_User1 FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Security_UserWiseMenuPermission ADD CONSTRAINT FK_Security_UserWiseMenuPermission_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_Voucher", "EditedBy"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Voucher ADD EditedBy [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Voucher ADD EditedDate [datetime] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Voucher ADD CONSTRAINT FK_Task_Voucher_Security_User3 FOREIGN KEY (EditedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Setup_HSCode"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Setup_HSCode(
                    [HSCodeId] [bigint] NOT NULL,
	                [HSCode] [varchar](20) NOT NULL,
	                [Description] [varchar](200) NOT NULL,
	                [DetailDescription] [varchar](1500) NULL,
                 CONSTRAINT [PK_Setup_HSCode] PRIMARY KEY CLUSTERED ([HSCodeId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");
            }

            //if table not found then create table
            if (!CheckTable("Setup_GovtDuty"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Setup_GovtDuty(
                    [GovtDutyId] [bigint] NOT NULL,
	                [DutyName] [varchar](10) NOT NULL,
	                [Description] [varchar](200) NULL,
                    [DutyOrder] [smallint] NOT NULL,
	                [DefaultValue] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [AccountsId] [bigint] NULL,
                 CONSTRAINT [PK_Setup_GovtDuty] PRIMARY KEY CLUSTERED ([GovtDutyId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_GovtDuty ADD CONSTRAINT FK_Setup_GovtDuty_Setup_Accounts FOREIGN KEY (AccountsId) REFERENCES Setup_Accounts (AccountsId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //rename table name if found
            if (CheckTable("Configuration_GovtDutyRate"))
            {
                _db.Database.ExecuteSqlCommand("sp_rename N'Configuration_GovtDutyRate', N'Configuration_GovtDutyRate_HSCode'");
            }

            //if table not found then create table
            if (!CheckTable("Configuration_GovtDutyRate_HSCode"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Configuration_GovtDutyRate_HSCode(
                    [RateId] [uniqueidentifier] NOT NULL,
	                [OperationalEventId] [bigint] NOT NULL,
	                [HSCodeId] [bigint] NOT NULL,
	                [GovtDutyId] [bigint] NOT NULL,
	                [SelectedCurrency] [varchar](5) NOT NULL,
	                [Currency1Rate] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Currency2Rate] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [RateType] [varchar](1) DEFAULT 'P' NOT NULL,
	                [RateValue] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [RateValue1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [RateValue2] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [ExemptedType] [varchar](1) DEFAULT 'P' NOT NULL,
	                [ExemptedValue] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [ExemptedValue1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [ExemptedValue2] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Remarks] [varchar](100) NULL,
	                [EntryBy] [bigint] NOT NULL,
                 CONSTRAINT [PK_Configuration_GovtDutyRate] PRIMARY KEY CLUSTERED ([RateId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A = Amount, P = Percentage' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Configuration_GovtDutyRate_HSCode', @level2type=N'COLUMN',@level2name=N'RateType'");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A = Amount, P = Percentage' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Configuration_GovtDutyRate_HSCode', @level2type=N'COLUMN',@level2name=N'ExemptedType'");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_GovtDutyRate_HSCode ADD CONSTRAINT FK_Configuration_GovtDutyRate_Configuration_OperationalEvent FOREIGN KEY (OperationalEventId) REFERENCES Configuration_OperationalEvent (OperationalEventId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_GovtDutyRate_HSCode ADD CONSTRAINT FK_Configuration_GovtDutyRate_Setup_HSCode FOREIGN KEY (HSCodeId) REFERENCES Setup_HSCode (HSCodeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_GovtDutyRate_HSCode ADD CONSTRAINT FK_Configuration_GovtDutyRate_Setup_GovtDuty FOREIGN KEY (GovtDutyId) REFERENCES Setup_GovtDuty (GovtDutyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_GovtDutyRate_HSCode ADD CONSTRAINT FK_Configuration_GovtDutyRate_Security_User FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Setup_Product", "Height"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Product ADD Height [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Product ADD Width [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Product ADD Thickness [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Product ADD CapacityId [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Product ADD Capacity [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Product ADD OriginCountryId [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Product ADD HSCodeId [bigint] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Product ADD CONSTRAINT FK_Setup_Product_Setup_Capacity FOREIGN KEY (CapacityId) REFERENCES Setup_Capacity (CapacityId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Product ADD CONSTRAINT FK_Setup_Product_Setup_Country FOREIGN KEY (OriginCountryId) REFERENCES Setup_Country (CountryId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Product ADD CONSTRAINT FK_Setup_Product_Setup_HSCode FOREIGN KEY (HSCodeId) REFERENCES Setup_HSCode (HSCodeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table found then drop table
            if (CheckTable("Setup_ProductAdditionalInfo"))
            {
                _db.Database.ExecuteSqlCommand(@"DROP TABLE Setup_ProductAdditionalInfo");
            }

            //add unique key if not found
            if (!CheckRelationshipBetweenTwoTables("Configuration_GovtDutyRate_HSCode", "IX_Configuration_GovtDutyRate"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_GovtDutyRate_HSCode ADD CONSTRAINT IX_Configuration_GovtDutyRate UNIQUE NONCLUSTERED (OperationalEventId ASC, HSCodeId ASC, GovtDutyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //add column if not found
            if (!CheckTable("Setup_Company", "GovtFinStartingMonth"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Company ADD GovtFinStartingMonth [tinyint] DEFAULT 7 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_SalesInvoice", "GovtDutyChallanNo"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD GovtDutyChallanNo [bigint] DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_SalesInvoiceDetail", "IsIncludingGovtDuty"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD IsIncludingGovtDuty [bit] DEFAULT 0 NOT NULL");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'True = Including, False = Excluding' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_SalesInvoiceDetail', @level2type=N'COLUMN',@level2name=N'IsIncludingGovtDuty'");
            }

            //add column if not found
            if (!CheckTable("Task_SalesInvoice", "GovtDutyAmount"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD GovtDutyAmount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD GovtDuty1Amount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD GovtDuty2Amount [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_DeliveryChallan", "GovtDutyChallanNo"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallan ADD GovtDutyChallanNo [bigint] DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_DeliveryChallanDetail", "IsIncludingGovtDuty"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD IsIncludingGovtDuty [bit] DEFAULT 0 NOT NULL");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'True = Including, False = Excluding' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_DeliveryChallanDetail', @level2type=N'COLUMN',@level2name=N'IsIncludingGovtDuty'");
            }

            //add column if not found
            if (!CheckTable("Task_SalesOrder", "GovtDutyChallanNo"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ADD GovtDutyChallanNo [bigint] DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_SalesOrderDetail", "IsIncludingGovtDuty"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrderDetail ADD IsIncludingGovtDuty [bit] DEFAULT 0 NOT NULL");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'True = Including, False = Excluding' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_SalesOrderDetail', @level2type=N'COLUMN',@level2name=N'IsIncludingGovtDuty'");
            }

            //add column if not found
            if (!CheckTable("Task_SalesOrder", "GovtDutyAmount"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ADD GovtDutyAmount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ADD GovtDuty1Amount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ADD GovtDuty2Amount [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //if table not found then create table
            if (!CheckTable("Task_SalesInvoiceDetail_GovtDuty"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_SalesInvoiceDetail_GovtDuty(
                    [Id] [uniqueidentifier] NOT NULL,
                    [InvoiceDetailId] [uniqueidentifier] NOT NULL,
	                [GovtDutyId] [bigint] NOT NULL,
	                [RateType] [varchar](1) DEFAULT 'P' NOT NULL,
	                [RateValue] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [RateValue1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [RateValue2] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [RateAmount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [RateAmount1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [RateAmount2] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [ExemptedType] [varchar](1) DEFAULT 'P' NOT NULL,
	                [ExemptedValue] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [ExemptedValue1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [ExemptedValue2] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [ExemptedAmount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [ExemptedAmount1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [ExemptedAmount2] [numeric](18, 4) DEFAULT 0 NOT NULL,
                 CONSTRAINT [PK_Task_SalesInvoiceDetail_GovtDuty] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A = Amount, P = Percentage' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_SalesInvoiceDetail_GovtDuty', @level2type=N'COLUMN',@level2name=N'RateType'");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A = Amount, P = Percentage' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_SalesInvoiceDetail_GovtDuty', @level2type=N'COLUMN',@level2name=N'ExemptedType'");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail_GovtDuty ADD CONSTRAINT FK_Task_SalesInvoiceDetail_GovtDuty_Task_SalesInvoiceDetail FOREIGN KEY (InvoiceDetailId) REFERENCES Task_SalesInvoiceDetail (InvoiceDetailId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail_GovtDuty ADD CONSTRAINT FK_Task_SalesInvoiceDetail_GovtDuty_Setup_GovtDuty FOREIGN KEY (GovtDutyId) REFERENCES Setup_GovtDuty (GovtDutyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_DeliveryChallanDetail_GovtDuty"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_DeliveryChallanDetail_GovtDuty(
                    [Id] [uniqueidentifier] NOT NULL,
                    [ChallanDetailId] [uniqueidentifier] NOT NULL,
	                [GovtDutyId] [bigint] NOT NULL,
	                [RateType] [varchar](1) DEFAULT 'P' NOT NULL,
	                [RateValue] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [RateValue1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [RateValue2] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [RateAmount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [RateAmount1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [RateAmount2] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [ExemptedType] [varchar](1) DEFAULT 'P' NOT NULL,
	                [ExemptedValue] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [ExemptedValue1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [ExemptedValue2] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [ExemptedAmount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [ExemptedAmount1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [ExemptedAmount2] [numeric](18, 4) DEFAULT 0 NOT NULL,
                 CONSTRAINT [PK_Task_DeliveryChallanDetail_GovtDuty] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A = Amount, P = Percentage' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_DeliveryChallanDetail_GovtDuty', @level2type=N'COLUMN',@level2name=N'RateType'");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A = Amount, P = Percentage' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_DeliveryChallanDetail_GovtDuty', @level2type=N'COLUMN',@level2name=N'ExemptedType'");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail_GovtDuty ADD CONSTRAINT FK_Task_DeliveryChallanDetail_GovtDuty_Task_DeliveryChallanDetail FOREIGN KEY (ChallanDetailId) REFERENCES Task_DeliveryChallanDetail (ChallanDetailId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail_GovtDuty ADD CONSTRAINT FK_Task_DeliveryChallanDetail_GovtDuty_Setup_GovtDuty FOREIGN KEY (GovtDutyId) REFERENCES Setup_GovtDuty (GovtDutyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_SalesOrderDetail_GovtDuty"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_SalesOrderDetail_GovtDuty(
                    [Id] [uniqueidentifier] NOT NULL,
                    [SalesOrderDetailId] [uniqueidentifier] NOT NULL,
	                [GovtDutyId] [bigint] NOT NULL,
	                [RateType] [varchar](1) DEFAULT 'P' NOT NULL,
	                [RateValue] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [RateValue1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [RateValue2] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [RateAmount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [RateAmount1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [RateAmount2] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [ExemptedType] [varchar](1) DEFAULT 'P' NOT NULL,
	                [ExemptedValue] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [ExemptedValue1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [ExemptedValue2] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [ExemptedAmount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [ExemptedAmount1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [ExemptedAmount2] [numeric](18, 4) DEFAULT 0 NOT NULL,
                 CONSTRAINT [PK_Task_SalesOrderDetail_GovtDuty] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A = Amount, P = Percentage' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_SalesOrderDetail_GovtDuty', @level2type=N'COLUMN',@level2name=N'RateType'");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A = Amount, P = Percentage' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_SalesOrderDetail_GovtDuty', @level2type=N'COLUMN',@level2name=N'ExemptedType'");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrderDetail_GovtDuty ADD CONSTRAINT FK_Task_SalesOrderDetail_GovtDuty_Task_SalesOrderDetail FOREIGN KEY (SalesOrderDetailId) REFERENCES Task_SalesOrderDetail (SalesOrderDetailId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrderDetail_GovtDuty ADD CONSTRAINT FK_Task_SalesOrderDetail_GovtDuty_Setup_GovtDuty FOREIGN KEY (GovtDutyId) REFERENCES Setup_GovtDuty (GovtDutyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Temp_PartyLedger", "OperationType"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_PartyLedger ADD OperationType [varchar](1) NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_PartyLedger ADD Particular1 [varchar](250) NULL");
            }

            //add unique key if not found
            if (!CheckRelationshipBetweenTwoTables("Configuration_FormattingTag", "IX_Configuration_FormattingTag"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_FormattingTag ADD CONSTRAINT IX_Configuration_FormattingTag UNIQUE NONCLUSTERED (TagName ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //alter column if found
            if (CheckTable("Temp_PartyLedger", "Particular"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_PartyLedger ALTER COLUMN Particular [varchar](500) NULL");
            }

            //alter column if found
            if (CheckTable("Task_SalesInvoice", "GovtDutyChallanNo"))
            {
                DropDefaultConstraintByColumn("Task_SalesInvoice", "GovtDutyChallanNo");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ALTER COLUMN GovtDutyChallanNo [varchar](15) NULL");
            }

            //alter column if found
            if (CheckTable("Task_DeliveryChallan", "GovtDutyChallanNo"))
            {
                DropDefaultConstraintByColumn("Task_DeliveryChallan", "GovtDutyChallanNo");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallan ALTER COLUMN GovtDutyChallanNo [varchar](15) NULL");
            }

            //alter column if found
            if (CheckTable("Task_SalesOrder", "GovtDutyChallanNo"))
            {
                DropDefaultConstraintByColumn("Task_SalesOrder", "GovtDutyChallanNo");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ALTER COLUMN GovtDutyChallanNo [varchar](15) NULL");
            }

            //add column if not found
            if (!CheckTable("Setup_Company", "MaxWidthInPixel"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Company ADD MaxWidthInPixel [smallint] DEFAULT 110 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Company ADD MaxHeightInPixel [smallint] DEFAULT 110 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_SalesInvoice", "DeliveryPlace"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD DeliveryPlace [varchar](300) NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD TransportTypeId [bigint] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD CONSTRAINT FK_Task_SalesInvoice_Setup_TransportType FOREIGN KEY (TransportTypeId) REFERENCES Setup_TransportType (TransportTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_Collection", "EditedBy"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Collection ADD EditedBy [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Collection ADD EditedDate [datetime] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Collection ADD CONSTRAINT FK_Task_Collection_Security_User3 FOREIGN KEY (EditedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_Payment", "EditedBy"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Payment ADD EditedBy [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Payment ADD EditedDate [datetime] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Payment ADD CONSTRAINT FK_Task_Payment_Security_User3 FOREIGN KEY (EditedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Setup_Accounts", "SelectedCurrency"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Accounts ADD SelectedCurrency [varchar](5) DEFAULT 'BDT' NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Setup_Accounts", "Currency1Rate"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Accounts ADD Currency1Rate [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Setup_Accounts", "Currency2Rate"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Accounts ADD Currency2Rate [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Setup_Accounts", "IsOpeningFinalized"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Accounts ADD IsOpeningFinalized [bit] DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Setup_Accounts", "OpeningEditedBy"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Accounts ADD OpeningEditedBy [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Accounts ADD OpeningEditedDate [datetime] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Accounts ADD CONSTRAINT FK_Setup_Accounts_Security_User2 FOREIGN KEY (OpeningEditedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add unique key if not found
            if (!CheckRelationshipBetweenTwoTables("Setup_GovtDuty", "IX_Setup_GovtDuty"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_GovtDuty ADD CONSTRAINT IX_Setup_GovtDuty UNIQUE NONCLUSTERED (DutyName ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //if table not found then create table
            if (!CheckTable("Setup_GovtDutyAdjustment"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Setup_GovtDutyAdjustment(
                    [AdjustmentId] [bigint] NOT NULL,
	                [AdjustmentName] [varchar](10) NOT NULL,
	                [Description] [varchar](200) NULL,
                    [AccountsId] [bigint] NULL,
                 CONSTRAINT [PK_Setup_GovtDutyAdjustment] PRIMARY KEY CLUSTERED ([AdjustmentId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_GovtDutyAdjustment ADD CONSTRAINT IX_Setup_GovtDutyAdjustment UNIQUE NONCLUSTERED (AdjustmentName ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_GovtDutyAdjustment ADD CONSTRAINT FK_Setup_GovtDutyAdjustment_Setup_Accounts FOREIGN KEY (AccountsId) REFERENCES Setup_Accounts (AccountsId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_Collection_GovtDutyAdjustment"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_Collection_GovtDutyAdjustment(
                    [CollectionAdjustmentId] [uniqueidentifier] DEFAULT newid() NOT NULL,
                    [CollectionId] [uniqueidentifier] NOT NULL,
                    [GovtDutyAdjustmentId] [bigint] NOT NULL,
                    [Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [Amount1] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [Amount2] [numeric](18, 4) DEFAULT 0 NOT NULL,
                CONSTRAINT [PK_Task_Collection_GovtDutyAdjustment] PRIMARY KEY CLUSTERED ([CollectionAdjustmentId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Collection_GovtDutyAdjustment ADD CONSTRAINT FK_Task_Collection_GovtDutyAdjustment_Task_Collection FOREIGN KEY (CollectionId) REFERENCES Task_Collection (CollectionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Collection_GovtDutyAdjustment ADD CONSTRAINT FK_Task_Collection_GovtDutyAdjustment_Setup_GovtDutyAdjustment FOREIGN KEY (GovtDutyAdjustmentId) REFERENCES Setup_GovtDutyAdjustment (AdjustmentId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_Collection", "SecurityDeposit"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Collection ADD SecurityDeposit [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Collection ADD SecurityDeposit1 [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Collection ADD SecurityDeposit2 [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Collection ADD RecoveryDate [datetime] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Collection ADD OthersAdjustment [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Collection ADD OthersAdjustment1 [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Collection ADD OthersAdjustment2 [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Collection ADD VoucherId [uniqueidentifier] NULL");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'For Security Deposit, Others Adjustment and Govt. Duty Adjustment voucher' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_Collection', @level2type=N'COLUMN',@level2name=N'VoucherId'");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Collection ADD CONSTRAINT FK_Task_Collection_Task_Voucher FOREIGN KEY (VoucherId) REFERENCES Task_Voucher (VoucherId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_Payment_GovtDutyAdjustment"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_Payment_GovtDutyAdjustment(
                    [PaymentAdjustmentId] [uniqueidentifier] DEFAULT newid() NOT NULL,
                    [PaymentId] [uniqueidentifier] NOT NULL,
                    [GovtDutyAdjustmentId] [bigint] NOT NULL,
                    [Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [Amount1] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [Amount2] [numeric](18, 4) DEFAULT 0 NOT NULL,
                CONSTRAINT [PK_Task_Payment_GovtDutyAdjustment] PRIMARY KEY CLUSTERED ([PaymentAdjustmentId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Payment_GovtDutyAdjustment ADD CONSTRAINT FK_Task_Payment_GovtDutyAdjustment_Task_Payment FOREIGN KEY (PaymentId) REFERENCES Task_Payment (PaymentId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Payment_GovtDutyAdjustment ADD CONSTRAINT FK_Task_Payment_GovtDutyAdjustment_Setup_GovtDutyAdjustment FOREIGN KEY (GovtDutyAdjustmentId) REFERENCES Setup_GovtDutyAdjustment (AdjustmentId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_Payment", "SecurityDeposit"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Payment ADD SecurityDeposit [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Payment ADD SecurityDeposit1 [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Payment ADD SecurityDeposit2 [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Payment ADD RecoveryDate [datetime] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Payment ADD OthersAdjustment [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Payment ADD OthersAdjustment1 [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Payment ADD OthersAdjustment2 [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Payment ADD VoucherId [uniqueidentifier] NULL");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'For Security Deposit, Others Adjustment and Govt. Duty Adjustment voucher' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_Payment', @level2type=N'COLUMN',@level2name=N'VoucherId'");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Payment ADD CONSTRAINT FK_Task_Payment_Task_Voucher FOREIGN KEY (VoucherId) REFERENCES Task_Voucher (VoucherId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Others_ReportDesignConfig"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Others_ReportDesignConfig(
                 [ReportDesignId] [bigint] NOT NULL,
                 [OperationalEventId] [bigint] NOT NULL,
                 [ShowHeader] [bit] DEFAULT ((1)) NOT NULL,
                 [HeaderHeight] [bigint] DEFAULT ((0)) NOT NULL,
                 [ShowLocationAddress] [bit] DEFAULT 0 NOT NULL,
                 [CompanyId] [bigint] NOT NULL,
                 CONSTRAINT [PK_Others_ReportDesignConfig] PRIMARY KEY CLUSTERED ([ReportDesignId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Others_ReportDesignConfig ADD CONSTRAINT FK_Others_ReportDesignConfig_Configuration_OperationalEvent FOREIGN KEY (OperationalEventId) REFERENCES Configuration_OperationalEvent (OperationalEventId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Others_ReportDesignConfig ADD CONSTRAINT FK_Others_ReportDesignConfig_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'True = Show Report Header, False = Hide Report Header' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Others_ReportDesignConfig', @level2type=N'COLUMN',@level2name=N'ShowHeader'");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Pixel Value' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Others_ReportDesignConfig', @level2type=N'COLUMN',@level2name=N'HeaderHeight'");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'True = Show Location Address, False = Hide Location Address' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Others_ReportDesignConfig', @level2type=N'COLUMN',@level2name=N'ShowLocationAddress'");
            }

            _db.Database.ExecuteSqlCommand("EXEC sys.sp_UpdateExtendedProperty @name=N'MS_Description', @value=N'Milimeter (mm) Value' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Others_ReportDesignConfig', @level2type=N'COLUMN',@level2name=N'HeaderHeight'");

            //drop table if found
            if (CheckTable("Others_ReportHeaderConfig", string.Empty))
            {
                _db.Database.ExecuteSqlCommand("DECLARE @eventId bigint; SELECT @eventId = OperationalEventId FROM Configuration_OperationalEvent WHERE EventName = 'Sales' AND SubEventName = 'Invoice'; INSERT INTO Others_ReportDesignConfig (ReportDesignId, OperationalEventId, ShowHeader, HeaderHeight, ShowLocationAddress, CompanyId) SELECT ReportHeadingId, @eventId, ShowHeader, HeaderHeight, ShowLocationAddress, CompanyId FROM Others_ReportHeaderConfig;");
                _db.Database.ExecuteSqlCommand("DROP TABLE Others_ReportHeaderConfig");
            }

            //add column if not found
            if (!CheckTable("Setup_ProductDimension", "Code"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_ProductDimension ADD Code [varchar](3) DEFAULT '000' NOT NULL");
                _db.Database.ExecuteSqlCommand(@"
                    DECLARE @ProductId bigint;
                    DECLARE @FETCH_STATUS_1 INT;

                    DECLARE Cursor_1 CURSOR FOR
                    SELECT DISTINCT ProductId FROM Setup_ProductDimension

                    OPEN Cursor_1
                    FETCH NEXT FROM Cursor_1 INTO @ProductId
                    SET @FETCH_STATUS_1 = @@FETCH_STATUS
                    WHILE @FETCH_STATUS_1 = 0
                    BEGIN
                        DECLARE @Code INT;
                        DECLARE @DimensionId bigint;
                        DECLARE @FETCH_STATUS_2 INT;

                        SET @Code = 1

                        DECLARE Cursor_2 CURSOR FOR
                        SELECT ProductDimensionId FROM Setup_ProductDimension WHERE ProductId = @ProductId

                        OPEN Cursor_2
                        FETCH NEXT FROM Cursor_2 INTO @DimensionId
                        SET @FETCH_STATUS_2 = @@FETCH_STATUS
                        WHILE @FETCH_STATUS_2 = 0
                        BEGIN
                            UPDATE Setup_ProductDimension SET Code = RIGHT(CONCAT('000', @Code), 3) WHERE ProductDimensionId = @DimensionId

                            SET @Code = @Code + 1

                            FETCH NEXT FROM Cursor_2 INTO @DimensionId
                            SET @FETCH_STATUS_2 = @@FETCH_STATUS
                        END

                        CLOSE Cursor_2
                        DEALLOCATE Cursor_2

                        FETCH NEXT FROM Cursor_1 INTO @ProductId
                        SET @FETCH_STATUS_1 = @@FETCH_STATUS
                    END
                    CLOSE Cursor_1
                    DEALLOCATE Cursor_1");
            }

            _db.Database.ExecuteSqlCommand("UPDATE Setup_Product SET Code = REPLACE(Code, '$', '')");

            //if table not found then create table
            if (!CheckTable("Setup_Features"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Setup_Features(
                 [FeatureId] [bigint] NOT NULL,
                 [FeatureName] [varchar](50) NOT NULL,
                 [DefaultValue] [bit] NOT NULL,
                 [Description] [varchar](200) NOT NULL,
                 CONSTRAINT [PK_Setup_Features] PRIMARY KEY CLUSTERED ([FeatureId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Features ADD CONSTRAINT IX_Setup_Features UNIQUE NONCLUSTERED (FeatureName ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //drop table if found
            if (CheckTable("Others_Features", string.Empty))
            {
                _db.Database.ExecuteSqlCommand("DROP TABLE Others_Features");
            }

            //if table not found then create table
            if (!CheckTable("Setup_SubFeatures"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Setup_SubFeatures(
                 [SubFeatureId] [bigint] NOT NULL,
                 [FeatureId] [bigint] NOT NULL,
                 [SubFeatureName] [varchar](50) NOT NULL,
                 [DefaultValue] [bit] NOT NULL,
                 [Group] [varchar](15) NOT NULL,
                 [Description] [varchar](200) NOT NULL,
                 CONSTRAINT [PK_Setup_SubFeatures] PRIMARY KEY CLUSTERED ([SubFeatureId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_SubFeatures ADD CONSTRAINT IX_Setup_SubFeatures UNIQUE NONCLUSTERED (FeatureId ASC, SubFeatureName ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_SubFeatures ADD CONSTRAINT FK_Setup_SubFeatures_Setup_Features FOREIGN KEY (FeatureId) REFERENCES Setup_Features (FeatureId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Configuration_Features"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Configuration_Features(
                    [Id] [bigint] NOT NULL,
                    [FeatureId] [bigint] NOT NULL,
                    [SubFeatureId] [bigint] NULL,
                    [Value] [bit] NOT NULL,
                    [OperationEventId] [bigint] NULL,
                    [LocationId] [bigint] NULL,
                    [CompanyId] [bigint] NOT NULL,
                    [EntryBy] [bigint] NOT NULL,
                    [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Configuration_Features] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_Features ADD CONSTRAINT FK_Configuration_Features_Security_User FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_Features ADD CONSTRAINT FK_Configuration_Features_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_Features ADD CONSTRAINT FK_Configuration_Features_Setup_Location FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_Features ADD CONSTRAINT FK_Configuration_Features_Setup_Features FOREIGN KEY (FeatureId) REFERENCES Setup_Features (FeatureId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_Features ADD CONSTRAINT FK_Configuration_Features_Setup_SubFeatures FOREIGN KEY (SubFeatureId) REFERENCES Setup_SubFeatures (SubFeatureId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_SalesInvoiceDetail", "InvoiceDiscount"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD InvoiceDiscount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD InvoiceDiscount1 [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD InvoiceDiscount2 [numeric](18, 4) DEFAULT 0 NOT NULL");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Value ratio and per unit wise invoice discount.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_SalesInvoiceDetail', @level2type=N'COLUMN',@level2name=N'InvoiceDiscount'");
            }

            //add column if not found
            if (!CheckTable("Task_DeliveryChallanDetail", "InvoiceDiscount"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD InvoiceDiscount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD InvoiceDiscount1 [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD InvoiceDiscount2 [numeric](18, 4) DEFAULT 0 NOT NULL");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Value ratio and per unit wise invoice discount.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_DeliveryChallanDetail', @level2type=N'COLUMN',@level2name=N'InvoiceDiscount'");
            }

            //add column if not found
            if (!CheckTable("Task_SalesOrderDetail", "InvoiceDiscount"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrderDetail ADD InvoiceDiscount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrderDetail ADD InvoiceDiscount1 [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrderDetail ADD InvoiceDiscount2 [numeric](18, 4) DEFAULT 0 NOT NULL");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Value ratio and per unit wise invoice discount.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_SalesOrderDetail', @level2type=N'COLUMN',@level2name=N'InvoiceDiscount'");
            }

            //if table not found then create table
            if (!CheckTable("Configuration_GovtDutyRate_Location"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Configuration_GovtDutyRate_Location(
                    [RateId] [uniqueidentifier] NOT NULL,
	                [OperationalEventId] [bigint] NOT NULL,
	                [LocationId] [bigint] NOT NULL,
	                [GovtDutyId] [bigint] NOT NULL,
	                [SelectedCurrency] [varchar](5) NOT NULL,
	                [Currency1Rate] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Currency2Rate] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [RateType] [varchar](1) DEFAULT 'P' NOT NULL,
	                [RateValue] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [RateValue1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [RateValue2] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [ExemptedType] [varchar](1) DEFAULT 'P' NOT NULL,
	                [ExemptedValue] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [ExemptedValue1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [ExemptedValue2] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Remarks] [varchar](100) NULL,
	                [EntryBy] [bigint] NOT NULL,
                 CONSTRAINT [PK_Configuration_GovtDutyRate_Location] PRIMARY KEY CLUSTERED ([RateId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A = Amount, P = Percentage' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Configuration_GovtDutyRate_Location', @level2type=N'COLUMN',@level2name=N'RateType'");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A = Amount, P = Percentage' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Configuration_GovtDutyRate_Location', @level2type=N'COLUMN',@level2name=N'ExemptedType'");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_GovtDutyRate_Location ADD CONSTRAINT IX_Configuration_GovtDutyRate_Location UNIQUE NONCLUSTERED (OperationalEventId ASC, LocationId ASC, GovtDutyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_GovtDutyRate_Location ADD CONSTRAINT FK_Configuration_GovtDutyRate_Location_Configuration_OperationalEvent FOREIGN KEY (OperationalEventId) REFERENCES Configuration_OperationalEvent (OperationalEventId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_GovtDutyRate_Location ADD CONSTRAINT FK_Configuration_GovtDutyRate_Location_Setup_Location FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_GovtDutyRate_Location ADD CONSTRAINT FK_Configuration_GovtDutyRate_Location_Setup_GovtDuty FOREIGN KEY (GovtDutyId) REFERENCES Setup_GovtDuty (GovtDutyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_GovtDutyRate_Location ADD CONSTRAINT FK_Configuration_GovtDutyRate_Location_Security_User FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_SalesOrderDetail", "DiscountType"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrderDetail ADD DiscountType [varchar](1) DEFAULT 'A' NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrderDetail ADD DiscountValue [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrderDetail ADD Discount1Value [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrderDetail ADD Discount2Value [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_DeliveryChallanDetail", "DiscountType"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD DiscountType [varchar](1) DEFAULT 'A' NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD DiscountValue [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD Discount1Value [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD Discount2Value [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_SalesInvoiceDetail", "DiscountType"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD DiscountType [varchar](1) DEFAULT 'A' NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD DiscountValue [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD Discount1Value [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD Discount2Value [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            // UPDATE INVOICE AMOUNT BY SUM(PRODUCT QUANTITY * (PRODUCT PRICE - PRODUCT WISE DISCOUNT))
            _db.Database.ExecuteSqlCommand("UPDATE Task_SalesInvoice SET InvoiceAmount = COALESCE((SELECT SUM(Quantity * (Price - Discount)) AS Total FROM Task_SalesInvoiceDetail GROUP BY InvoiceId HAVING (Task_SalesInvoiceDetail.InvoiceId = Task_SalesInvoice.InvoiceId)), 0)");

            _db.Database.ExecuteSqlCommand(@"DECLARE @FETCH_STATUS_1 INT;
                DECLARE @FETCH_STATUS_2 INT;
                DECLARE @InvoiceId uniqueidentifier;
                DECLARE @InvoiceDetailId uniqueidentifier;
                DECLARE @InvoiceAmount numeric(18, 4);
                DECLARE @InvoiceDiscount numeric(18, 4);
                DECLARE @PdtQty numeric(18, 4);
                DECLARE @PdtTotal numeric(18, 4);
                DECLARE @InvoicePdtDiscount numeric(18, 4);

                DECLARE Cursor1 CURSOR FOR
                SELECT InvoiceId, InvoiceAmount, InvoiceDiscount FROM Task_SalesInvoice WHERE InvoiceDiscount > 0

                OPEN Cursor1
                FETCH NEXT FROM Cursor1 INTO @InvoiceId, @InvoiceAmount, @InvoiceDiscount
                SET @FETCH_STATUS_1 = @@FETCH_STATUS
                WHILE @FETCH_STATUS_1 = 0
                BEGIN
                    DECLARE Cursor2 CURSOR FOR
                    SELECT InvoiceDetailId , TotalAmount = Quantity * (Price - Discount), Quantity FROM Task_SalesInvoiceDetail WHERE InvoiceId = @InvoiceId

                    OPEN Cursor2
                    FETCH NEXT FROM Cursor2 INTO @InvoiceDetailId, @PdtTotal, @PdtQty
                    SET @FETCH_STATUS_2 = @@FETCH_STATUS
                    WHILE @FETCH_STATUS_2 = 0
                    BEGIN
                        SET @InvoicePdtDiscount = ROUND(((@InvoiceDiscount * @PdtTotal) / @InvoiceAmount) / @PdtQty, 4)

                        UPDATE Task_SalesInvoiceDetail SET InvoiceDiscount = @InvoicePdtDiscount WHERE InvoiceDetailId = @InvoiceDetailId

                        FETCH NEXT FROM Cursor2 INTO @InvoiceDetailId, @PdtTotal, @PdtQty
                        SET @FETCH_STATUS_2 = @@FETCH_STATUS
                    END

                    CLOSE Cursor2
                    DEALLOCATE Cursor2

                    FETCH NEXT FROM Cursor1 INTO @InvoiceId, @InvoiceAmount, @InvoiceDiscount
                    SET @FETCH_STATUS_1 = @@FETCH_STATUS
                END

                CLOSE Cursor1
                DEALLOCATE Cursor1");

            //if table not found then create table
            if (!CheckTable("Setup_CostingGroup"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Setup_CostingGroup(
                    [CostingGroupId] [bigint] NOT NULL,
                    [Code] [varchar](3) NOT NULL,
                    [Name] [varchar](100) NOT NULL,
                 CONSTRAINT [PK_Setup_CostingGroup] PRIMARY KEY CLUSTERED ([CostingGroupId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_CostingGroup ADD CONSTRAINT IX_Setup_CostingGroup UNIQUE NONCLUSTERED (Name ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //add column if not found
            if (!CheckTable("Setup_CostingGroup", "Code"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_CostingGroup ADD Code [varchar](3) NULL");
            }

            //if table not found then create table
            if (!CheckTable("Setup_CostingControl"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Setup_CostingControl(
                    [CostingControlId] [bigint] NOT NULL,
                    [CostingGroupId] [bigint] NOT NULL,
                    [Name] [varchar](100) NOT NULL,
                    [CostAllocationMethod] [varchar](5) NOT NULL,
                    [CompanyId] [bigint] NOT NULL,
                    [EntryBy] [bigint] NOT NULL,
                    [EntryDate] [datetime] NOT NULL,
                    [EditedBy] [bigint] NULL,
                    [EditedDate] [datetime] NULL,
                 CONSTRAINT [PK_Setup_CostingControl] PRIMARY KEY CLUSTERED ([CostingControlId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'VR = Value Ratio, WR = Weight Ratio, TR = Tax Ratio' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Setup_CostingControl', @level2type=N'COLUMN',@level2name=N'CostAllocationMethod'");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_CostingControl ADD CONSTRAINT IX_Setup_CostingControl UNIQUE NONCLUSTERED (CostingGroupId ASC, Name ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_CostingControl ADD CONSTRAINT FK_Setup_CostingControl_Setup_CostingGroup FOREIGN KEY (CostingGroupId) REFERENCES Setup_CostingGroup (CostingGroupId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_CostingControl ADD CONSTRAINT FK_Setup_CostingControl_Security_User FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_CostingControl ADD CONSTRAINT FK_Setup_CostingControl_Security_User1 FOREIGN KEY (EditedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_CostingControl ADD CONSTRAINT FK_Setup_CostingControl_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Setup_CostingHead"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Setup_CostingHead(
                    [CostingHeadId] [bigint] NOT NULL,
                    [CostingControlId] [bigint] NOT NULL,
                    [Name] [varchar](100) NOT NULL,
                    [AccountsId] [bigint] NULL,
                    [CompanyId] [bigint] NOT NULL,
                    [EntryBy] [bigint] NOT NULL,
                    [EntryDate] [datetime] NOT NULL,
                    [EditedBy] [bigint] NULL,
                    [EditedDate] [datetime] NULL,
                 CONSTRAINT [PK_Setup_CostingHead] PRIMARY KEY CLUSTERED ([CostingHeadId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_CostingHead ADD CONSTRAINT IX_Setup_CostingHead UNIQUE NONCLUSTERED (CostingControlId ASC, Name ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_CostingHead ADD CONSTRAINT FK_Setup_CostingHead_Setup_CostingControl FOREIGN KEY (CostingControlId) REFERENCES Setup_CostingControl (CostingControlId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_CostingHead ADD CONSTRAINT FK_Setup_CostingHead_Setup_Accounts FOREIGN KEY (AccountsId) REFERENCES Setup_Accounts (AccountsId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_CostingHead ADD CONSTRAINT FK_Setup_CostingHead_Security_User FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_CostingHead ADD CONSTRAINT FK_Setup_CostingHead_Security_User1 FOREIGN KEY (EditedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_CostingHead ADD CONSTRAINT FK_Setup_CostingHead_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_LCOpeningNos"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_LCOpeningNos(
                 [Id] [uniqueidentifier] DEFAULT newid() NOT NULL,
                 [LCOpeningNo] [varchar](50) NOT NULL,
                 [Year] [bigint] NOT NULL,
                 [CompanyId] [bigint] NOT NULL,
                 CONSTRAINT [PK_Task_LCOpeningNos] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LCOpeningNos ADD CONSTRAINT FK_Task_LCOpeningNos_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //rename table name if found
            if (CheckTable("Task_LC"))
            {
                _db.Database.ExecuteSqlCommand("sp_rename N'Task_LC', N'Task_LCOpening'");
                _db.Database.ExecuteSqlCommand("sp_rename N'PK_Task_LC', N'PK_Task_LCOpening'");
                _db.Database.ExecuteSqlCommand("sp_rename N'FK_Task_LC_Security_User', N'FK_Task_LCOpening_Security_User'");
                _db.Database.ExecuteSqlCommand("sp_rename N'FK_Task_LC_Setup_Bank', N'FK_Task_LCOpening_Setup_Bank'");
                _db.Database.ExecuteSqlCommand("sp_rename N'FK_Task_LC_Setup_Bank1', N'FK_Task_LCOpening_Setup_Bank1'");
                _db.Database.ExecuteSqlCommand("sp_rename N'FK_Task_LC_Setup_Company', N'FK_Task_LCOpening_Setup_Company'");
                _db.Database.ExecuteSqlCommand("sp_rename N'FK_Task_LC_Setup_Location', N'FK_Task_LCOpening_Setup_Location'");
                _db.Database.ExecuteSqlCommand("sp_rename N'FK_Task_LC_Setup_Supplier', N'FK_Task_LCOpening_Setup_Supplier'");
                _db.Database.ExecuteSqlCommand("sp_rename N'FK_Task_LC_Task_ProformaInvoice', N'FK_Task_LCOpening_Task_ProformaInvoice'");
                _db.Database.ExecuteSqlCommand("sp_rename N'IX_Task_LC', N'IX_Task_LCOpening'");
            }

            //add column if not found
            if (!CheckTable("Task_LCOpening", "SelectedCurrency"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LCOpening ADD SelectedCurrency [varchar](5) NULL");
            }

            //add column if not found
            if (!CheckTable("Task_LCOpening", "Currency1Rate"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LCOpening ADD Currency1Rate [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_LCOpening", "Currency2Rate"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LCOpening ADD Currency2Rate [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_LCOpening", "LCAmount"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LCOpening ADD LCAmount [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_LCOpening", "LCAmount1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LCOpening ADD LCAmount1 [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_LCOpening", "LCAmount2"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LCOpening ADD LCAmount2 [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_LCOpening", "LCMargin"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LCOpening ADD LCMargin [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Always in percentage (%)', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_LCOpening', @level2type=N'COLUMN',@level2name=N'LCMargin'");
            }

            //add column if not found
            if (!CheckTable("Task_LCOpening", "LCMarginAmount"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LCOpening ADD LCMarginAmount [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_LCOpening", "LCMarginAmount1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LCOpening ADD LCMarginAmount1 [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_LCOpening", "LCMarginAmount2"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LCOpening ADD LCMarginAmount2 [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_LCOpening", "LTRAmount"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LCOpening ADD LTRAmount [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_LCOpening", "LTRAmount1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LCOpening ADD LTRAmount1 [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_LCOpening", "LTRAmount2"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LCOpening ADD LTRAmount2 [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_LCOpening", "LTRNo"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LCOpening ADD LTRNo [varchar](50) NULL");
            }

            //add column if not found
            if (!CheckTable("Task_LCOpening", "ReferenceNo"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LCOpening ADD ReferenceNo [varchar](50) NULL");
            }

            //add column if not found
            if (!CheckTable("Task_LCOpening", "ReferenceDate"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LCOpening ADD ReferenceDate [datetime] NULL");
            }

            //add column if not found
            if (!CheckTable("Task_LCOpening", "Approved"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LCOpening ADD Approved [varchar](1) DEFAULT ('N') NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LCOpening ADD [ApprovedBy] [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LCOpening ADD [ApprovedDate] [datetime] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LCOpening ADD [CancelReason] [varchar](200) NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LCOpening ADD CONSTRAINT FK_Task_LCOpening_Security_User1 FOREIGN KEY (ApprovedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_LCOpening", "LCSupplierId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LCOpening ADD [LCSupplierId] [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LCOpening ADD CONSTRAINT FK_Task_LCOpening_Setup_Supplier1 FOREIGN KEY (LCSupplierId) REFERENCES Setup_Supplier (SupplierId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //alter column if found
            if (CheckTable("Task_LCOpening", "SupplierId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LCOpening ALTER COLUMN SupplierId [bigint] NOT NULL");
            }

            //if table not found then create table
            if (!CheckTable("Setup_DocumentsGroup"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Setup_DocumentsGroup(
                    [GroupId] [bigint] NOT NULL,
                    [Name] [varchar](100) NOT NULL,
                    [CompanyId] [bigint] NOT NULL,
                    [EntryBy] [bigint] NOT NULL,
                    [EntryDate] [datetime] NOT NULL,
                    [EditedBy] [bigint] NULL,
                    [EditedDate] [datetime] NULL,
                 CONSTRAINT [PK_Setup_DocumentsGroup] PRIMARY KEY CLUSTERED ([GroupId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_DocumentsGroup ADD CONSTRAINT IX_Setup_DocumentsGroup UNIQUE NONCLUSTERED (Name ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_DocumentsGroup ADD CONSTRAINT FK_Setup_DocumentsGroup_Security_User FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_DocumentsGroup ADD CONSTRAINT FK_Setup_DocumentsGroup_Security_User1 FOREIGN KEY (EditedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_DocumentsGroup ADD CONSTRAINT FK_Setup_DocumentsGroup_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Setup_DocumentsTitle"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Setup_DocumentsTitle(
                    [TitleId] [bigint] NOT NULL,
                    [GroupId] [bigint] NOT NULL,
                    [Name] [varchar](100) NOT NULL,
                    [CompanyId] [bigint] NOT NULL,
                    [EntryBy] [bigint] NOT NULL,
                    [EntryDate] [datetime] NOT NULL,
                    [EditedBy] [bigint] NULL,
                    [EditedDate] [datetime] NULL,
                 CONSTRAINT [PK_Setup_DocumentsTitle] PRIMARY KEY CLUSTERED ([TitleId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_DocumentsTitle ADD CONSTRAINT IX_Setup_DocumentsTitle UNIQUE NONCLUSTERED (GroupId ASC, Name ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_DocumentsTitle ADD CONSTRAINT FK_Setup_DocumentsTitle_Setup_DocumentsGroup FOREIGN KEY (GroupId) REFERENCES Setup_DocumentsGroup (GroupId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_DocumentsTitle ADD CONSTRAINT FK_Setup_DocumentsTitle_Security_User FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_DocumentsTitle ADD CONSTRAINT FK_Setup_DocumentsTitle_Security_User1 FOREIGN KEY (EditedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_DocumentsTitle ADD CONSTRAINT FK_Setup_DocumentsTitle_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Others_Documents"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Others_Documents(
                    [DocumentId] [uniqueidentifier] NOT NULL,
	                [DocumentNo] [varchar](20) NOT NULL,
	                [OperationalEventId] [bigint] NOT NULL,
	                [OperationNo] [varchar](50) NOT NULL,
                    [TitleId] [bigint] NOT NULL,
	                [Description] [varchar](250) NULL,
	                [DocumentSPath] [varchar](250) NOT NULL,
                    [CompanyId] [bigint] NOT NULL,
	                [EntryBy] [bigint] NOT NULL,
                    [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Others_Documents] PRIMARY KEY CLUSTERED ([DocumentId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Others_Documents ADD CONSTRAINT IX_Others_Documents UNIQUE NONCLUSTERED (DocumentNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Others_Documents ADD CONSTRAINT FK_Others_Documents_Configuration_OperationalEvent FOREIGN KEY (OperationalEventId) REFERENCES Configuration_OperationalEvent (OperationalEventId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Others_Documents ADD CONSTRAINT FK_Others_Documents_Setup_DocumentsTitle FOREIGN KEY (TitleId) REFERENCES Setup_DocumentsTitle (TitleId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Others_Documents ADD CONSTRAINT FK_Others_Documents_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Others_Documents ADD CONSTRAINT FK_Others_Documents_Security_User FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_ImportDocuments"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_ImportDocuments(
                    [ImportDocumentId] [uniqueidentifier] NOT NULL,
                    [LCId] [uniqueidentifier] NOT NULL,
                    [CostingGroupId] [bigint] NOT NULL,
                    [DocumentId] [uniqueidentifier] NOT NULL,
	                [EntryBy] [bigint] NOT NULL,
                    [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Task_ImportDocuments] PRIMARY KEY CLUSTERED ([ImportDocumentId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ImportDocuments ADD CONSTRAINT FK_Task_ImportDocuments_Task_LCOpening FOREIGN KEY (LCId) REFERENCES Task_LCOpening (LCId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ImportDocuments ADD CONSTRAINT FK_Task_ImportDocuments_Setup_CostingGroup FOREIGN KEY (CostingGroupId) REFERENCES Setup_CostingGroup (CostingGroupId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ImportDocuments ADD CONSTRAINT FK_Task_ImportDocuments_Others_Documents FOREIGN KEY (DocumentId) REFERENCES Others_Documents (DocumentId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ImportDocuments ADD CONSTRAINT FK_Task_ImportDocuments_Security_User FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_ImportCostingNos"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_ImportCostingNos(
                 [Id] [uniqueidentifier] DEFAULT newid() NOT NULL,
                 [ImportCostingNos] [varchar](50) NOT NULL,
                 [Year] [bigint] NOT NULL,
                 [CompanyId] [bigint] NOT NULL,
                 CONSTRAINT [PK_Task_ImportCostingNos] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ImportCostingNos ADD CONSTRAINT FK_Task_ImportCostingNos_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_ImportCosting"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_ImportCosting(
                    [ImportCostingId] [uniqueidentifier] NOT NULL,
                    [ImportCostingNo] [varchar](50) NOT NULL,
                    [LCId] [uniqueidentifier] NOT NULL,
	                [SelectedCurrency] [varchar](5) NOT NULL,
	                [Currency1Rate] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Currency2Rate] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Approved] [varchar](1) DEFAULT ('N') NOT NULL,
	                [ApprovedBy] [bigint] NULL,
	                [ApprovedDate] [datetime] NULL,
	                [CancelReason] [varchar](200) NULL,
	                [LocationId] [bigint] NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
	                [EntryBy] [bigint] NOT NULL,
                    [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Task_ImportCosting] PRIMARY KEY CLUSTERED ([ImportCostingId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ImportCosting ADD CONSTRAINT FK_Task_ImportCosting_Task_LCOpening FOREIGN KEY (LCId) REFERENCES Task_LCOpening (LCId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ImportCosting ADD CONSTRAINT FK_Task_ImportCosting_Security_User FOREIGN KEY (ApprovedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ImportCosting ADD CONSTRAINT FK_Task_ImportCosting_Setup_Location FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ImportCosting ADD CONSTRAINT FK_Task_ImportCosting_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ImportCosting ADD CONSTRAINT FK_Task_ImportCosting_Security_User1 FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_ImportCostingDetail"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_ImportCostingDetail(
                    [ImportCostingDetailId] [uniqueidentifier] NOT NULL,
                    [ImportCostingId] [uniqueidentifier] NOT NULL,
                    [CostingHeadId] [bigint] NOT NULL,
                    [CostingType] [varchar](1) DEFAULT 'A' NOT NULL,
                    [CostingValue] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [CostingValue1] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [CostingValue2] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [CostingAmount] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [CostingAmount1] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [CostingAmount2] [numeric](18, 4) DEFAULT 0 NOT NULL,
                 CONSTRAINT [PK_Task_ImportCostingDetail] PRIMARY KEY CLUSTERED ([ImportCostingDetailId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A = Amount, P = Percentage' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_ImportCostingDetail', @level2type=N'COLUMN',@level2name=N'CostingType'");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ImportCostingDetail ADD CONSTRAINT FK_Task_ImportCostingDetail_Task_ImportCosting FOREIGN KEY (ImportCostingId) REFERENCES Task_ImportCosting (ImportCostingId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ImportCostingDetail ADD CONSTRAINT FK_Task_ImportCostingDetail_Setup_CostingHead FOREIGN KEY (CostingHeadId) REFERENCES Setup_CostingHead (CostingHeadId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //alter column if found
            if (CheckTable("Others_Documents", "DocumentSPath"))
            {
                _db.Database.ExecuteSqlCommand("sp_rename 'Others_Documents.DocumentSPath', 'FileRecord', 'COLUMN'");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Others_Documents ALTER COLUMN FileRecord image NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Others_Documents ADD FileType [varchar](50) NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Others_Documents ADD DocumentName [nvarchar](200) NOT NULL");
            }

            //rename column if found
            if (CheckTable("Task_ProformaInvoice", "IsSettledByLIM"))
            {
                _db.Database.ExecuteSqlCommand("sp_rename 'Task_ProformaInvoice.IsSettledByLIM', 'IsSettledByLCOpening', 'COLUMN'");
            }

            //rename column if found
            if (CheckTable("Task_ImportCosting", "ImportCostingNos"))
            {
                _db.Database.ExecuteSqlCommand("sp_rename 'Task_ImportCosting.ImportCostingNos', 'ImportCostingNo', 'COLUMN'");
            }

            //add unique key if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ImportCosting", "IX_Task_ImportCosting"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ImportCosting ADD CONSTRAINT IX_Task_ImportCosting UNIQUE NONCLUSTERED (ImportCostingNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //if table not found then create table
            if (!CheckTable("Setup_ConvertionRatio"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Setup_ConvertionRatio(
                    [ConvertionRatioId] [uniqueidentifier] NOT NULL,
	                [RatioNo] [varchar](50) NOT NULL,
	                [RatioTitle] [varchar](100) NOT NULL,
	                [RatioDate] [datetime] NOT NULL,
	                [Description] [varchar](200) NULL,
	                [Approved] [varchar](1) DEFAULT ('N') NOT NULL,
	                [ApprovedBy] [bigint] NULL,
	                [ApprovedDate] [datetime] NULL,
	                [CancelReason] [varchar](200) NULL,
	                [CompanyId] [bigint] NOT NULL,
	                [EntryBy] [bigint] NOT NULL,
	                [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Setup_ConvertionRatio] PRIMARY KEY CLUSTERED ([ConvertionRatioId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_ConvertionRatio ADD CONSTRAINT IX_Setup_ConvertionRatio UNIQUE NONCLUSTERED (RatioNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_ConvertionRatio ADD CONSTRAINT FK_Setup_ConvertionRatio_Security_User FOREIGN KEY (ApprovedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_ConvertionRatio ADD CONSTRAINT FK_Setup_ConvertionRatio_Security_User1 FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_ConvertionRatio ADD CONSTRAINT FK_Setup_ConvertionRatio_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Setup_ConvertionRatioDetail"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Setup_ConvertionRatioDetail(
                    [ConvertionRatioDetailId] [uniqueidentifier] NOT NULL,
	                [ConvertionRatioId] [uniqueidentifier] NOT NULL,
	                [ProductFor] [varchar](1) NOT NULL,
	                [ProductId] [bigint] NOT NULL,
	                [ProductDimensionId] [bigint] NULL,
	                [UnitTypeId] [bigint] NOT NULL,
	                [Quantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Remarks] [varchar](200) NULL,
                 CONSTRAINT [PK_Setup_ConvertionRatioDetail] PRIMARY KEY CLUSTERED ([ConvertionRatioDetailId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'M = Main Product, C = Component Product' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Setup_ConvertionRatioDetail', @level2type=N'COLUMN',@level2name=N'ProductFor'");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Always primary unit type ID save.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Setup_ConvertionRatioDetail', @level2type=N'COLUMN',@level2name=N'UnitTypeId'");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_ConvertionRatioDetail ADD CONSTRAINT FK_Setup_ConvertionRatioDetail_Setup_ConvertionRatio FOREIGN KEY (ConvertionRatioId) REFERENCES Setup_ConvertionRatio (ConvertionRatioId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_ConvertionRatioDetail ADD CONSTRAINT FK_Setup_ConvertionRatioDetail_Setup_Product FOREIGN KEY (ProductId) REFERENCES Setup_Product (ProductId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_ConvertionRatioDetail ADD CONSTRAINT FK_Setup_ConvertionRatioDetail_Setup_ProductDimension FOREIGN KEY (ProductDimensionId) REFERENCES Setup_ProductDimension (ProductDimensionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_ConvertionRatioDetail ADD CONSTRAINT FK_Setup_ConvertionRatioDetail_Setup_UnitType FOREIGN KEY (UnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_ConvertionNos"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_ConvertionNos(
                    [Id] [uniqueidentifier] NOT NULL,
	                [ConvertionNo] [varchar](50) NOT NULL,
	                [Year] [bigint] NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
                 CONSTRAINT [PK_Task_ConvertionNos] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ConvertionNos ADD CONSTRAINT IX_Task_ConvertionNos UNIQUE NONCLUSTERED (ConvertionNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ConvertionNos ADD CONSTRAINT FK_Task_ConvertionNos_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_Convertion"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_Convertion(
                    [ConvertionId] [uniqueidentifier] NOT NULL,
	                [ConvertionNo] [varchar](50) NOT NULL,
	                [ConvertionDate] [datetime] NOT NULL,
	                [ConvertionType] [varchar](1) NOT NULL,
	                [ConvertionFor] [varchar](1) NOT NULL,
	                [ConvertionRatioId] [uniqueidentifier] NULL,
	                [Remarks] [varchar](200) NULL,
	                [Approved] [varchar](1) NOT NULL,
	                [ApprovedBy] [bigint] NULL,
	                [ApprovedDate] [datetime] NULL,
	                [CancelReason] [varchar](200) NULL,
	                [LocationId] [bigint] NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
	                [EntryBy] [bigint] NOT NULL,
	                [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Task_Convertion] PRIMARY KEY CLUSTERED ([ConvertionId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'D = Direct, R = Ratio' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_Convertion', @level2type=N'COLUMN',@level2name=N'ConvertionType'");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A = Assemble, D = Disassemble' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_Convertion', @level2type=N'COLUMN',@level2name=N'ConvertionFor'");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Convertion ADD CONSTRAINT IX_Task_Convertion UNIQUE NONCLUSTERED (ConvertionNo ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Convertion ADD CONSTRAINT FK_Task_Convertion_Security_User FOREIGN KEY (ApprovedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Convertion ADD CONSTRAINT FK_Task_Convertion_Security_User1 FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Convertion ADD CONSTRAINT FK_Task_Convertion_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Convertion ADD CONSTRAINT FK_Task_Convertion_Setup_ConvertionRatio FOREIGN KEY (ConvertionRatioId) REFERENCES Setup_ConvertionRatio (ConvertionRatioId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_Convertion ADD CONSTRAINT FK_Task_Convertion_Setup_Location FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_ConvertionDetail"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_ConvertionDetail(
                    [ConvertionDetailId] [uniqueidentifier] NOT NULL,
	                [ConvertionId] [uniqueidentifier] NOT NULL,
	                [ProductFor] [varchar](1) NOT NULL,
	                [ProductId] [bigint] NOT NULL,
	                [ProductDimensionId] [bigint] NULL,
	                [UnitTypeId] [bigint] NOT NULL,
	                [WareHouseId] [bigint] NULL,
	                [Quantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Cost] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Cost1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Cost2] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [ReferenceNo] [varchar](50) NOT NULL,
                 CONSTRAINT [PK_Task_ConvertionDetail] PRIMARY KEY CLUSTERED ([ConvertionDetailId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'M = Main Product, C = Component Product' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_ConvertionDetail', @level2type=N'COLUMN',@level2name=N'ProductFor'");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Always primary unit type ID save.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_ConvertionDetail', @level2type=N'COLUMN',@level2name=N'UnitTypeId'");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ConvertionDetail ADD CONSTRAINT FK_Task_ConvertionDetail_Setup_Location FOREIGN KEY (WareHouseId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ConvertionDetail ADD CONSTRAINT FK_Task_ConvertionDetail_Setup_Product FOREIGN KEY (ProductId) REFERENCES Setup_Product (ProductId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ConvertionDetail ADD CONSTRAINT FK_Task_ConvertionDetail_Setup_ProductDimension FOREIGN KEY (ProductDimensionId) REFERENCES Setup_ProductDimension (ProductDimensionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ConvertionDetail ADD CONSTRAINT FK_Task_ConvertionDetail_Setup_UnitType FOREIGN KEY (UnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ConvertionDetail ADD CONSTRAINT FK_Task_ConvertionDetail_Task_Convertion FOREIGN KEY (ConvertionId) REFERENCES Task_Convertion (ConvertionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_ConvertionDetailSerial"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_ConvertionDetailSerial(
                    [ConvertionDetailSerialId] [uniqueidentifier] NOT NULL,
	                [ConvertionDetailId] [uniqueidentifier] NOT NULL,
	                [Serial] [varchar](100) NOT NULL,
	                [AdditionalSerial] [varchar](100) NULL,
                CONSTRAINT [PK_Task_ConvertionDetailSerial] PRIMARY KEY CLUSTERED ([ConvertionDetailSerialId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ConvertionDetailSerial ADD CONSTRAINT FK_Task_ConvertionDetailSerial_Task_ConvertionDetail FOREIGN KEY (ConvertionDetailId) REFERENCES Task_ConvertionDetail (ConvertionDetailId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Others_ReportDesignConfig", "DefaultReportName"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Others_ReportDesignConfig ADD [DefaultReportName] [varchar](100) NULL");
            }

            //add column if not found
            if (!CheckTable("Others_ReportDesignConfig", "PaperLength"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Others_ReportDesignConfig ADD [PaperLength] [bigint] DEFAULT 297 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Others_ReportDesignConfig ADD [PaperWidth] [bigint] DEFAULT 210 NOT NULL");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Milimeter (mm) Value' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Others_ReportDesignConfig', @level2type=N'COLUMN',@level2name=N'PaperLength'");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Milimeter (mm) Value' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Others_ReportDesignConfig', @level2type=N'COLUMN',@level2name=N'PaperWidth'");
            }

            //add column if not found
            if (!CheckTable("Task_LCOpening", "IsSettledByImportCosting"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_LCOpening ADD [IsSettledByImportCosting] [bit] DEFAULT 0 NOT NULL");
            }

            //if table not found then create table
            if (!CheckTable("Task_ImportCostingProduct"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_ImportCostingProduct(
                    [ImportCostingProductId] [uniqueidentifier] DEFAULT newid() NOT NULL,
                    [ImportCostingId] [uniqueidentifier] NOT NULL,
                    [ProductId] [bigint] NOT NULL,
                    [ProductDimensionId] [bigint] NULL,
                    [UnitTypeId] [bigint] NOT NULL,
                    [Quantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [LIMStockInQuantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [Price] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Price1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Price2] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [WeightOrCBM] [numeric](18, 4) DEFAULT 0 NOT NULL,
                    [PackingUnitTypeId] [bigint] NOT NULL,
                    [PackingQuantity] [numeric](18, 4) DEFAULT 0 NOT NULL,
                 CONSTRAINT [PK_Task_ImportCostingProduct] PRIMARY KEY CLUSTERED ([ImportCostingProductId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ImportCostingProduct ADD CONSTRAINT FK_Task_ImportCostingProduct_Task_ImportCosting FOREIGN KEY (ImportCostingId) REFERENCES Task_ImportCosting (ImportCostingId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ImportCostingProduct ADD CONSTRAINT FK_Task_ImportCostingProduct_Setup_Product FOREIGN KEY (ProductId) REFERENCES Setup_Product (ProductId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ImportCostingProduct ADD CONSTRAINT FK_Task_ImportCostingProduct_Setup_ProductDimension FOREIGN KEY (ProductDimensionId) REFERENCES Setup_ProductDimension (ProductDimensionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ImportCostingProduct ADD CONSTRAINT FK_Task_ImportCostingProduct_Setup_UnitType FOREIGN KEY (UnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ImportCostingProduct ADD CONSTRAINT FK_Task_ImportCostingProduct_Setup_UnitType1 FOREIGN KEY (PackingUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //alter column if found
            if (CheckTable("Task_ComplainReceive", "RequestedBy"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ComplainReceive ALTER COLUMN RequestedBy [bigint] NULL");
            }

            //alter column if found
            if (CheckTable("Task_CustomerDelivery", "RequestedBy"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_CustomerDelivery ALTER COLUMN RequestedBy [bigint] NULL");
            }

            //alter column if found
            if (CheckTable("Task_ReplacementClaim", "RequestedBy"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementClaim ALTER COLUMN RequestedBy [bigint] NULL");
            }

            //alter column if found
            if (CheckTable("Task_ReplacementReceive", "RequestedBy"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ReplacementReceive ALTER COLUMN RequestedBy [bigint] NULL");
            }

            //if table not found then create table
            if (!CheckTable("Setup_AccountsDetail_Location"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Setup_AccountsDetail_Location(
                    [AccountsDetailId] [uniqueidentifier] NOT NULL,
                    [AccountsId] [bigint] NOT NULL,
                    [LocationId] [bigint] NULL,
                    [OpeningBalance] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [OpeningBalance1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [OpeningBalance2] [numeric](18, 4) DEFAULT 0 NOT NULL,
                 CONSTRAINT [PK_Setup_AccountsDetail_Location] PRIMARY KEY CLUSTERED ([AccountsDetailId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_AccountsDetail_Location ADD CONSTRAINT FK_Setup_AccountsDetail_Location_Setup_Accounts FOREIGN KEY (AccountsId) REFERENCES Setup_Accounts (AccountsId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_AccountsDetail_Location ADD CONSTRAINT FK_Setup_AccountsDetail_Location_Setup_Location FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //alter column if found
            if (CheckTable("Task_SalesInvoice", "CustomerDetail"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ALTER COLUMN CustomerDetail [varchar](250) NULL");
            }

            //alter column if found
            if (CheckTable("Task_DeliveryChallan", "CustomerDetail"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallan ALTER COLUMN CustomerDetail [varchar](250) NULL");
            }

            //alter column if found
            if (CheckTable("Task_SalesOrder", "CustomerDetail"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ALTER COLUMN CustomerDetail [varchar](250) NULL");
            }

            //add column if not found
            if (!CheckTable("Task_PaymentMapping", "ReceiveId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PaymentMapping ADD [ReceiveId] [uniqueidentifier] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_PaymentMapping ADD CONSTRAINT FK_Task_PaymentMapping_Task_GoodsReceive FOREIGN KEY (ReceiveId) REFERENCES Task_GoodsReceive (ReceiveId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Stock_CurrentStock", "GoodsReceiveId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_CurrentStock ADD [GoodsReceiveId] [uniqueidentifier] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_CurrentStock ADD CONSTRAINT FK_Stock_CurrentStock_Task_GoodsReceive FOREIGN KEY (GoodsReceiveId) REFERENCES Task_GoodsReceive (ReceiveId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Stock_CurrentStock", "ImportedStockInId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_CurrentStock ADD [ImportedStockInId] [uniqueidentifier] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_CurrentStock ADD CONSTRAINT FK_Stock_CurrentStock_Task_ImportedStockIn FOREIGN KEY (ImportedStockInId) REFERENCES Task_ImportedStockIn (StockInId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Stock_CurrentStock", "SupplierId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_CurrentStock ADD [SupplierId] [bigint] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_CurrentStock ADD CONSTRAINT FK_Stock_CurrentStock_Setup_Supplier FOREIGN KEY (SupplierId) REFERENCES Setup_Supplier (SupplierId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //drop column if found
            if (CheckTable("Stock_CurrentStock", "MainReferenceNo"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_CurrentStock DROP COLUMN MainReferenceNo");
            }

            //add column if not found
            if (!CheckTable("Stock_TransitStock", "GoodsReceiveId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_TransitStock ADD [GoodsReceiveId] [uniqueidentifier] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_TransitStock ADD CONSTRAINT FK_Stock_TransitStock_Task_GoodsReceive FOREIGN KEY (GoodsReceiveId) REFERENCES Task_GoodsReceive (ReceiveId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Stock_TransitStock", "ImportedStockInId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_TransitStock ADD [ImportedStockInId] [uniqueidentifier] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_TransitStock ADD CONSTRAINT FK_Stock_TransitStock_Task_ImportedStockIn FOREIGN KEY (ImportedStockInId) REFERENCES Task_ImportedStockIn (StockInId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Stock_TransitStock", "SupplierId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_TransitStock ADD [SupplierId] [bigint] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_TransitStock ADD CONSTRAINT FK_Stock_TransitStock_Setup_Supplier FOREIGN KEY (SupplierId) REFERENCES Setup_Supplier (SupplierId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //drop column if found
            if (CheckTable("Stock_TransitStock", "MainReferenceNo"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_TransitStock DROP COLUMN MainReferenceNo");
            }

            //rename column if found
            if (CheckTable("Task_ReceiveFinalize", "IsSettled"))
            {
                _db.Database.ExecuteSqlCommand("sp_rename 'Task_ReceiveFinalize.IsSettled', 'IsSettledByPayment', 'COLUMN'");
            }

            //rename column if found
            if (CheckTable("Task_PurchaseOrder", "IsSettled"))
            {
                _db.Database.ExecuteSqlCommand("sp_rename 'Task_PurchaseOrder.IsSettled', 'IsSettledByGoodsReceive', 'COLUMN'");
            }

            //add column if not found
            if (!CheckTable("Task_DeliveryChallanDetail", "GoodsReceiveId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD [GoodsReceiveId] [uniqueidentifier] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD CONSTRAINT FK_Task_DeliveryChallanDetail_Task_GoodsReceive FOREIGN KEY (GoodsReceiveId) REFERENCES Task_GoodsReceive (ReceiveId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_DeliveryChallanDetail", "ImportedStockInId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD [ImportedStockInId] [uniqueidentifier] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD CONSTRAINT FK_Task_DeliveryChallanDetail_Task_ImportedStockIn FOREIGN KEY (ImportedStockInId) REFERENCES Task_ImportedStockIn (StockInId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_DeliveryChallanDetail", "SupplierId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD [SupplierId] [bigint] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD CONSTRAINT FK_Task_DeliveryChallanDetail_Setup_Supplier FOREIGN KEY (SupplierId) REFERENCES Setup_Supplier (SupplierId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_DeliveryChallanDetail", "ReferenceDate"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD ReferenceDate [datetime] NULL");
            }

            //add column if not found
            if (!CheckTable("Task_SalesInvoiceDetail", "GoodsReceiveId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD [GoodsReceiveId] [uniqueidentifier] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD CONSTRAINT FK_Task_SalesInvoiceDetail_Task_GoodsReceive FOREIGN KEY (GoodsReceiveId) REFERENCES Task_GoodsReceive (ReceiveId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_SalesInvoiceDetail", "ImportedStockInId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD [ImportedStockInId] [uniqueidentifier] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD CONSTRAINT FK_Task_SalesInvoiceDetail_Task_ImportedStockIn FOREIGN KEY (ImportedStockInId) REFERENCES Task_ImportedStockIn (StockInId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_SalesInvoiceDetail", "SupplierId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD [SupplierId] [bigint] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD CONSTRAINT FK_Task_SalesInvoiceDetail_Setup_Supplier FOREIGN KEY (SupplierId) REFERENCES Setup_Supplier (SupplierId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_SalesReturnDetail", "GoodsReceiveId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesReturnDetail ADD [GoodsReceiveId] [uniqueidentifier] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesReturnDetail ADD CONSTRAINT FK_Task_SalesReturnDetail_Task_GoodsReceive FOREIGN KEY (GoodsReceiveId) REFERENCES Task_GoodsReceive (ReceiveId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_SalesReturnDetail", "ImportedStockInId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesReturnDetail ADD [ImportedStockInId] [uniqueidentifier] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesReturnDetail ADD CONSTRAINT FK_Task_SalesReturnDetail_Task_ImportedStockIn FOREIGN KEY (ImportedStockInId) REFERENCES Task_ImportedStockIn (StockInId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_SalesReturnDetail", "SupplierId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesReturnDetail ADD [SupplierId] [bigint] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesReturnDetail ADD CONSTRAINT FK_Task_SalesReturnDetail_Setup_Supplier FOREIGN KEY (SupplierId) REFERENCES Setup_Supplier (SupplierId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Stock_RMAStock", "GoodsReceiveId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_RMAStock ADD [GoodsReceiveId] [uniqueidentifier] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_RMAStock ADD CONSTRAINT FK_Stock_RMAStock_Task_GoodsReceive FOREIGN KEY (GoodsReceiveId) REFERENCES Task_GoodsReceive (ReceiveId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Stock_RMAStock", "ImportedStockInId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_RMAStock ADD [ImportedStockInId] [uniqueidentifier] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_RMAStock ADD CONSTRAINT FK_Stock_RMAStock_Task_ImportedStockIn FOREIGN KEY (ImportedStockInId) REFERENCES Task_ImportedStockIn (StockInId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Stock_RMAStock", "SupplierId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_RMAStock ADD [SupplierId] [bigint] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_RMAStock ADD CONSTRAINT FK_Stock_RMAStock_Setup_Supplier FOREIGN KEY (SupplierId) REFERENCES Setup_Supplier (SupplierId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Stock_BadStock", "GoodsReceiveId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_BadStock ADD [GoodsReceiveId] [uniqueidentifier] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_BadStock ADD CONSTRAINT FK_Stock_BadStock_Task_GoodsReceive FOREIGN KEY (GoodsReceiveId) REFERENCES Task_GoodsReceive (ReceiveId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Stock_BadStock", "ImportedStockInId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_BadStock ADD [ImportedStockInId] [uniqueidentifier] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_BadStock ADD CONSTRAINT FK_Stock_BadStock_Task_ImportedStockIn FOREIGN KEY (ImportedStockInId) REFERENCES Task_ImportedStockIn (StockInId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Stock_BadStock", "SupplierId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_BadStock ADD [SupplierId] [bigint] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_BadStock ADD CONSTRAINT FK_Stock_BadStock_Setup_Supplier FOREIGN KEY (SupplierId) REFERENCES Setup_Supplier (SupplierId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_TransferChallanDetail", "GoodsReceiveId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferChallanDetail ADD [GoodsReceiveId] [uniqueidentifier] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferChallanDetail ADD CONSTRAINT FK_Task_TransferChallanDetail_Task_GoodsReceive FOREIGN KEY (GoodsReceiveId) REFERENCES Task_GoodsReceive (ReceiveId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_TransferChallanDetail", "ImportedStockInId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferChallanDetail ADD [ImportedStockInId] [uniqueidentifier] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferChallanDetail ADD CONSTRAINT FK_Task_TransferChallanDetail_Task_ImportedStockIn FOREIGN KEY (ImportedStockInId) REFERENCES Task_ImportedStockIn (StockInId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_TransferChallanDetail", "SupplierId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferChallanDetail ADD [SupplierId] [bigint] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferChallanDetail ADD CONSTRAINT FK_Task_TransferChallanDetail_Setup_Supplier FOREIGN KEY (SupplierId) REFERENCES Setup_Supplier (SupplierId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_TransferChallanDetail", "CurrentBadRMAStockId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferChallanDetail ADD [CurrentBadRMAStockId] [uniqueidentifier] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferChallanDetail ADD [PrimaryQuantity] [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_TransferReceiveDetail", "GoodsReceiveId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferReceiveDetail ADD [GoodsReceiveId] [uniqueidentifier] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferReceiveDetail ADD CONSTRAINT FK_Task_TransferReceiveDetail_Task_GoodsReceive FOREIGN KEY (GoodsReceiveId) REFERENCES Task_GoodsReceive (ReceiveId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_TransferReceiveDetail", "ImportedStockInId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferReceiveDetail ADD [ImportedStockInId] [uniqueidentifier] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferReceiveDetail ADD CONSTRAINT FK_Task_TransferReceiveDetail_Task_ImportedStockIn FOREIGN KEY (ImportedStockInId) REFERENCES Task_ImportedStockIn (StockInId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_TransferReceiveDetail", "SupplierId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferReceiveDetail ADD [SupplierId] [bigint] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_TransferReceiveDetail ADD CONSTRAINT FK_Task_TransferReceiveDetail_Setup_Supplier FOREIGN KEY (SupplierId) REFERENCES Setup_Supplier (SupplierId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Stock_LIMStock", "SupplierId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_LIMStock ADD [SupplierId] [bigint] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Stock_LIMStock ADD CONSTRAINT FK_Stock_LIMStock_Setup_Supplier FOREIGN KEY (SupplierId) REFERENCES Setup_Supplier (SupplierId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_SalesInvoiceDetail", "IsWarrantyAvailable"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD [ShortSpecification] [varchar](500) NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD [IsWarrantyAvailable] [bit] DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD [WarrantyDays] [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD [IsServiceWarranty] [bit] DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD [ServiceWarrantyDays] [numeric](18, 4) DEFAULT 0 NOT NULL");

                _db.Database.ExecuteSqlCommand("UPDATE Task_SalesInvoiceDetail SET IsWarrantyAvailable = Setup_Product.IsWarrantyAvailable, WarrantyDays = Setup_Product.WarrantyDays, IsServiceWarranty = Setup_Product.IsServiceWarranty, ServiceWarrantyDays = Setup_Product.ServiceWarrantyDays "
                    + "FROM Setup_Product INNER JOIN Task_SalesInvoiceDetail ON Setup_Product.ProductId = Task_SalesInvoiceDetail.ProductId");
            }

            //add column if not found
            if (!CheckTable("Setup_Company", "CompanyOpeningDate"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Company ADD [CompanyOpeningDate] [datetime] NULL");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Actual Company Formation or Registration Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Setup_Company', @level2type=N'COLUMN',@level2name=N'CompanyOpeningDate'");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Software Opening Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Setup_Company', @level2type=N'COLUMN',@level2name=N'OpeningDate'");
            }

            //add column if not found
            if (!CheckTable("Task_SalesOrderDetail", "IsWarrantyAvailable"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrderDetail ADD [ShortSpecification] [varchar](500) NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrderDetail ADD [IsWarrantyAvailable] [bit] DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrderDetail ADD [WarrantyDays] [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrderDetail ADD [IsServiceWarranty] [bit] DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrderDetail ADD [ServiceWarrantyDays] [numeric](18, 4) DEFAULT 0 NOT NULL");

                _db.Database.ExecuteSqlCommand("UPDATE Task_SalesOrderDetail SET IsWarrantyAvailable = Setup_Product.IsWarrantyAvailable, WarrantyDays = Setup_Product.WarrantyDays, IsServiceWarranty = Setup_Product.IsServiceWarranty, ServiceWarrantyDays = Setup_Product.ServiceWarrantyDays "
                    + "FROM Setup_Product INNER JOIN Task_SalesOrderDetail ON Setup_Product.ProductId = Task_SalesOrderDetail.ProductId");
            }

            //add column if not found
            if (!CheckTable("Task_DeliveryChallanDetail", "IsWarrantyAvailable"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD [ShortSpecification] [varchar](500) NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD [IsWarrantyAvailable] [bit] DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD [WarrantyDays] [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD [IsServiceWarranty] [bit] DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallanDetail ADD [ServiceWarrantyDays] [numeric](18, 4) DEFAULT 0 NOT NULL");

                _db.Database.ExecuteSqlCommand("UPDATE Task_DeliveryChallanDetail SET IsWarrantyAvailable = Setup_Product.IsWarrantyAvailable, WarrantyDays = Setup_Product.WarrantyDays, IsServiceWarranty = Setup_Product.IsServiceWarranty, ServiceWarrantyDays = Setup_Product.ServiceWarrantyDays "
                    + "FROM Setup_Product INNER JOIN Task_DeliveryChallanDetail ON Setup_Product.ProductId = Task_DeliveryChallanDetail.ProductId");
            }

            //add column if not found
            if (!CheckTable("Task_ConvertionDetail", "PrimaryUnitTypeId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ConvertionDetail ADD PrimaryUnitTypeId [bigint] NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ConvertionDetail ADD SecondaryUnitTypeId [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ConvertionDetail ADD TertiaryUnitTypeId [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ConvertionDetail ADD SecondaryConversionRatio [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ConvertionDetail ADD TertiaryConversionRatio [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_ConvertionDetail", "FK_Task_ConvertionDetail_Setup_UnitType1"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ConvertionDetail ADD CONSTRAINT FK_Task_ConvertionDetail_Setup_UnitType1 FOREIGN KEY (PrimaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ConvertionDetail ADD CONSTRAINT FK_Task_ConvertionDetail_Setup_UnitType2 FOREIGN KEY (SecondaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ConvertionDetail ADD CONSTRAINT FK_Task_ConvertionDetail_Setup_UnitType3 FOREIGN KEY (TertiaryUnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //alter column if found
            if (CheckTable("Setup_Customer", "Name"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Customer ALTER COLUMN Name [nvarchar](250) NOT NULL");
            }

            //alter column if found
            if (CheckTable("Setup_Customer", "Address"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Customer ALTER COLUMN Address [nvarchar](500) NULL");
            }

            //alter column if found
            if (CheckTable("Task_SalesOrderDeliveryInfo", "DeliveryPlace"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrderDeliveryInfo ALTER COLUMN DeliveryPlace [nvarchar](500) NULL");
            }

            //alter column if found
            if (CheckTable("Task_SalesOrder", "TermsAndConditionsDetail"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ALTER COLUMN TermsAndConditionsDetail [nvarchar](4000) NULL");
            }

            //alter column if found
            if (CheckTable("Task_DeliveryChallan", "CustomerDetail"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallan ALTER COLUMN CustomerDetail [nvarchar](250) NULL");
            }

            //alter column if found
            if (CheckTable("Task_DeliveryChallan", "DeliveryPlace"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallan ALTER COLUMN DeliveryPlace [nvarchar](500) NULL");
            }

            //alter column if found
            if (CheckTable("Task_SalesInvoice", "CustomerDetail"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ALTER COLUMN CustomerDetail [nvarchar](250) NULL");
            }

            //alter column if found
            if (CheckTable("Task_SalesInvoice", "TermsAndConditionsDetail"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ALTER COLUMN TermsAndConditionsDetail [nvarchar](4000) NULL");
            }

            //alter column if found
            if (CheckTable("Task_SalesInvoice", "DeliveryPlace"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ALTER COLUMN DeliveryPlace [nvarchar](500) NULL");
            }

            //add column if not found
            if (!CheckTable("Setup_ProductDimension", "SKUCode"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_ProductDimension ADD SKUCode [varchar](50) NULL");
            }

            //drop table if found
            if (CheckTable("Task_DeliveryChallan_DeliveryInfo", string.Empty))
            {
                _db.Database.ExecuteSqlCommand("DROP TABLE Task_DeliveryChallan_DeliveryInfo");
            }

            //add column if not found
            if (!CheckTable("Setup_Charge", "Code"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Charge ADD Code [varchar](50) NULL");
            }

            //add unique key if not found
            if (CheckRelationshipBetweenTwoTables("Setup_Charge", "IX_Setup_Charge"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Charge DROP CONSTRAINT IX_Setup_Charge");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Charge ADD CONSTRAINT IX_Setup_Charge UNIQUE NONCLUSTERED (Code ASC, CompanyId ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            //add column if not found
            if (!CheckTable("Configuration_EventWiseCharge", "AccountsId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_EventWiseCharge ADD AccountsId [bigint] NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Configuration_EventWiseCharge ADD CONSTRAINT FK_Configuration_EventWiseCharge_Setup_Accounts FOREIGN KEY (AccountsId) REFERENCES Setup_Accounts (AccountsId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_SalesOrder_Charge"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_SalesOrder_Charge(
	                [SalesOrderChargeId] [uniqueidentifier] NOT NULL,
	                [SalesOrderId] [uniqueidentifier] NOT NULL,
	                [ChargeEventId] [bigint] NOT NULL,
	                [ChargeAmount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Charge1Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Charge2Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
                 CONSTRAINT [PK_Task_SalesOrder_Charge] PRIMARY KEY CLUSTERED ([SalesOrderChargeId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder_Charge ADD CONSTRAINT FK_Task_SalesOrder_Charge_Configuration_EventWiseCharge FOREIGN KEY (ChargeEventId) REFERENCES Configuration_EventWiseCharge (ChargeEventId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder_Charge ADD CONSTRAINT FK_Task_SalesOrder_Charge_Task_SalesOrder FOREIGN KEY (SalesOrderId) REFERENCES Task_SalesOrder (SalesOrderId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_DeliveryChallan_Charge"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_DeliveryChallan_Charge(
	                [DeliveryChallanChargeId] [uniqueidentifier] NOT NULL,
	                [ChallanId] [uniqueidentifier] NOT NULL,
	                [ChargeEventId] [bigint] NOT NULL,
	                [ChargeAmount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Charge1Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Charge2Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
                 CONSTRAINT [PK_Task_DeliveryChallan_Charge] PRIMARY KEY CLUSTERED ([DeliveryChallanChargeId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallan_Charge ADD CONSTRAINT FK_Task_DeliveryChallan_Charge_Configuration_EventWiseCharge FOREIGN KEY (ChargeEventId) REFERENCES Configuration_EventWiseCharge (ChargeEventId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_DeliveryChallan_Charge ADD CONSTRAINT FK_Task_DeliveryChallan_Charge_Task_DeliveryChallan FOREIGN KEY (ChallanId) REFERENCES Task_DeliveryChallan (ChallanId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_SalesInvoice_Charge"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_SalesInvoice_Charge(
	                [SalesInvoiceChargeId] [uniqueidentifier] NOT NULL,
	                [InvoiceId] [uniqueidentifier] NOT NULL,
	                [ChargeEventId] [bigint] NOT NULL,
	                [ChargeAmount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Charge1Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Charge2Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
                 CONSTRAINT [PK_Task_SalesInvoice_Charge] PRIMARY KEY CLUSTERED ([SalesInvoiceChargeId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice_Charge ADD CONSTRAINT FK_Task_SalesInvoice_Charge_Configuration_EventWiseCharge FOREIGN KEY (ChargeEventId) REFERENCES Configuration_EventWiseCharge (ChargeEventId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice_Charge ADD CONSTRAINT FK_Task_SalesInvoice_Charge_Task_SalesInvoice FOREIGN KEY (InvoiceId) REFERENCES Task_SalesInvoice (InvoiceId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_SalesOrder", "TotalChargeAmount"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ADD TotalChargeAmount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ADD TotalChargeAmount1 [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesOrder ADD TotalChargeAmount2 [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_SalesInvoice", "TotalChargeAmount"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD TotalChargeAmount [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD TotalChargeAmount1 [numeric](18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD TotalChargeAmount2 [numeric](18, 4) DEFAULT 0 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_SalesInvoiceDetail", "ProductEntrySequence"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD ProductEntrySequence [bigint] DEFAULT 1 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_GoodsReceiveDetail", "ProductEntrySequence"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_GoodsReceiveDetail ADD ProductEntrySequence [bigint] DEFAULT 1 NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Temp_CustomerSupplierOutstanding", "Group"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_CustomerSupplierOutstanding ADD [Group] varchar(50) NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_CustomerSupplierOutstanding ADD [Name] varchar(400) NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_CustomerSupplierOutstanding ADD [SalesPerson] varchar(400) NULL");
            }

            //add column if not found
            if (!CheckTable("Others_ReportDesignConfig", "LeftMargin"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Others_ReportDesignConfig ADD [LeftMargin] [bigint] DEFAULT 10 NOT NULL");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Milimeter (mm) Value' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Others_ReportDesignConfig', @level2type=N'COLUMN',@level2name=N'LeftMargin'");
            }

            //if table not found then create table
            if (!CheckTable("Temp_SalesInvoice"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Temp_SalesInvoice(
	                [InvoiceId] [uniqueidentifier] NOT NULL,
	                [InvoiceNo] [varchar](50) NOT NULL,
	                [InvoiceDate] [datetime] NOT NULL,
	                [CustomerId] [bigint] NOT NULL,
	                [CustomerDetail] [nvarchar](250) NULL,
	                [BuyerId] [bigint] NULL,
	                [SelectedCurrency] [varchar](5) NOT NULL,
	                [Currency1Rate] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Currency2Rate] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [InvoiceDiscountType] [varchar](1) NOT NULL,
	                [InvoiceDiscountValue] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [InvoiceDiscount1Value] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [InvoiceDiscount2Value] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [InvoiceDiscount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Invoice1Discount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Invoice2Discount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [InvoiceAmount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Invoice1Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Invoice2Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [CommissionType] [varchar](1) NOT NULL,
	                [CommissionValue] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Commission1Value] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Commission2Value] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [CommissionAmount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Commission1Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Commission2Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [GovtDutyAmount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [GovtDuty1Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [GovtDuty2Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [TotalChargeAmount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [TotalChargeAmount1] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [TotalChargeAmount2] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [CollectedAmount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Collected1Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Collected2Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Approved] [varchar](1) DEFAULT 'N' NOT NULL,
	                [ApprovedBy] [bigint] NULL,
	                [ApprovedDate] [datetime] NULL,
	                [CancelReason] [varchar](200) NULL,
	                [InvoiceOperationType] [varchar](1) DEFAULT 'R' NOT NULL,
	                [TermsAndConditionsId] [bigint] NULL,
	                [TermsAndConditionsDetail] [nvarchar](4000) NULL,
	                [ReferenceNo] [varchar](50) NULL,
	                [ReferenceDate] [datetime] NULL,
	                [Remarks] [varchar](500) NULL,
	                [TransactionFrom] [varchar](10) DEFAULT 'Current' NOT NULL,
	                [IsSalesModeCash] [bit] DEFAULT ((1)) NOT NULL,
	                [IsSettledByCollection] [bit] DEFAULT ((0)) NOT NULL,
	                [GovtDutyChallanNo] [varchar](15) NULL,
	                [DeliveryPlace] [nvarchar](500) NULL,
	                [ContactPerson] [varchar](100) NULL,
	                [ContactPersonNo] [varchar](30) NULL,
	                [TransportId] [bigint] NULL,
	                [TransportTypeId] [bigint] NULL,
	                [VehicleNo] [varchar](30) NULL,
	                [DriverName] [varchar](100) NULL,
	                [DriverContactNo] [varchar](30) NULL,
	                [LocationId] [bigint] NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
	                [EntryBy] [bigint] NOT NULL,
	                [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Temp_SalesInvoice] PRIMARY KEY CLUSTERED ([InvoiceId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY], 
                 CONSTRAINT [IX_Temp_SalesInvoice] UNIQUE NONCLUSTERED ([InvoiceNo] ASC, [CompanyId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_SalesInvoice ADD CONSTRAINT FK_Temp_SalesInvoice_Security_User FOREIGN KEY (ApprovedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_SalesInvoice ADD CONSTRAINT FK_Temp_SalesInvoice_Security_User1 FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_SalesInvoice ADD CONSTRAINT FK_Temp_SalesInvoice_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_SalesInvoice ADD CONSTRAINT FK_Temp_SalesInvoice_Setup_Customer FOREIGN KEY (CustomerId) REFERENCES Setup_Customer (CustomerId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_SalesInvoice ADD CONSTRAINT FK_Temp_SalesInvoice_Setup_Customer1 FOREIGN KEY (BuyerId) REFERENCES Setup_Customer (CustomerId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_SalesInvoice ADD CONSTRAINT FK_Temp_SalesInvoice_Setup_Location FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_SalesInvoice ADD CONSTRAINT FK_Temp_SalesInvoice_Setup_TermsAndConditions FOREIGN KEY (TermsAndConditionsId) REFERENCES Setup_TermsAndConditions (TermsAndConditionsId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_SalesInvoice ADD CONSTRAINT FK_Temp_SalesInvoice_Setup_Transport FOREIGN KEY (TransportId) REFERENCES Setup_Transport (TransportId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_SalesInvoice ADD CONSTRAINT FK_Temp_SalesInvoice_Setup_TransportType FOREIGN KEY (TransportTypeId) REFERENCES Setup_TransportType (TransportTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A = Amount, P = Percentage' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Temp_SalesInvoice', @level2type=N'COLUMN',@level2name=N'InvoiceDiscountType'");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A = Amount, P = Percentage' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Temp_SalesInvoice', @level2type=N'COLUMN',@level2name=N'CommissionType'");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'R = Regular Sales, D = Direct Sales' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Temp_SalesInvoice', @level2type=N'COLUMN',@level2name=N'InvoiceOperationType'");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Default True. True = Cash Mode, False = Credit Mode' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Temp_SalesInvoice', @level2type=N'COLUMN',@level2name=N'IsSalesModeCash'");
            }

            //if table not found then create table
            if (!CheckTable("Temp_SalesInvoice_Charge"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Temp_SalesInvoice_Charge(
	                [SalesInvoiceChargeId] [uniqueidentifier] NOT NULL,
	                [InvoiceId] [uniqueidentifier] NOT NULL,
	                [ChargeEventId] [bigint] NOT NULL,
	                [ChargeAmount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Charge1Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
	                [Charge2Amount] [numeric](18, 4) DEFAULT 0 NOT NULL,
                 CONSTRAINT [PK_Temp_SalesInvoice_Charge] PRIMARY KEY CLUSTERED ([SalesInvoiceChargeId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_SalesInvoice_Charge ADD CONSTRAINT FK_Temp_SalesInvoice_Charge_Configuration_EventWiseCharge FOREIGN KEY (ChargeEventId) REFERENCES Configuration_EventWiseCharge (ChargeEventId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_SalesInvoice_Charge ADD CONSTRAINT FK_Temp_SalesInvoice_Charge_Temp_SalesInvoice FOREIGN KEY (InvoiceId) REFERENCES Temp_SalesInvoice (InvoiceId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Temp_SalesInvoiceDetail"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Temp_SalesInvoiceDetail(
	                [InvoiceDetailId] [uniqueidentifier] DEFAULT (newid()) NOT NULL,
	                [InvoiceId] [uniqueidentifier] NOT NULL,
	                [ChallanNo] [varchar](50) NOT NULL,
	                [ProductEntrySequence] [bigint] DEFAULT ((1)) NOT NULL,
	                [ProductId] [bigint] NOT NULL,
	                [ProductDimensionId] [bigint] NULL,
	                [UnitTypeId] [bigint] NOT NULL,
	                [Quantity] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Price] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Price1] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Price2] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [DiscountType] [varchar](1) NOT NULL,
	                [DiscountValue] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Discount1Value] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Discount2Value] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Discount] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Discount1] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Discount2] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [InvoiceDiscount] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [InvoiceDiscount1] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [InvoiceDiscount2] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Cost] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Cost1] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Cost2] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [WareHouseId] [bigint] NULL,
	                [IsIncludingGovtDuty] [bit] NOT NULL,
	                [ShortSpecification] [varchar](500) NULL,
	                [IsWarrantyAvailable] [bit] NOT NULL,
	                [WarrantyDays] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [IsServiceWarranty] [bit] NOT NULL,
	                [ServiceWarrantyDays] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
                 CONSTRAINT [PK_Temp_SalesInvoiceDetail] PRIMARY KEY CLUSTERED ([InvoiceDetailId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_SalesInvoiceDetail ADD CONSTRAINT FK_Temp_SalesInvoiceDetail_Setup_Location FOREIGN KEY (WareHouseId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_SalesInvoiceDetail ADD CONSTRAINT FK_Temp_SalesInvoiceDetail_Setup_Product FOREIGN KEY (ProductId) REFERENCES Setup_Product (ProductId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_SalesInvoiceDetail ADD CONSTRAINT FK_Temp_SalesInvoiceDetail_Setup_ProductDimension FOREIGN KEY (ProductDimensionId) REFERENCES Setup_ProductDimension (ProductDimensionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_SalesInvoiceDetail ADD CONSTRAINT FK_Temp_SalesInvoiceDetail_Setup_UnitType FOREIGN KEY (UnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_SalesInvoiceDetail ADD CONSTRAINT FK_Temp_SalesInvoiceDetail_Temp_SalesInvoice FOREIGN KEY (InvoiceId) REFERENCES Temp_SalesInvoice (InvoiceId) ON UPDATE NO ACTION ON DELETE NO ACTION");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Value ratio and per unit wise invoice discount.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Temp_SalesInvoiceDetail', @level2type=N'COLUMN',@level2name=N'InvoiceDiscount'");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'True = Including, False = Excluding' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Temp_SalesInvoiceDetail', @level2type=N'COLUMN',@level2name=N'IsIncludingGovtDuty'");
            }

            //if table not found then create table
            if (!CheckTable("Temp_SalesInvoiceDetail_GovtDuty"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Temp_SalesInvoiceDetail_GovtDuty(
	                [Id] [uniqueidentifier] NOT NULL,
	                [InvoiceDetailId] [uniqueidentifier] NOT NULL,
	                [GovtDutyId] [bigint] NOT NULL,
	                [RateType] [varchar](1) DEFAULT ('P') NOT NULL,
	                [RateValue] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [RateValue1] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [RateValue2] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [RateAmount] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [RateAmount1] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [RateAmount2] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [ExemptedType] [varchar](1) DEFAULT ('P') NOT NULL,
	                [ExemptedValue] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [ExemptedValue1] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [ExemptedValue2] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [ExemptedAmount] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [ExemptedAmount1] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [ExemptedAmount2] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
                 CONSTRAINT [PK_Temp_SalesInvoiceDetail_GovtDuty] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_SalesInvoiceDetail_GovtDuty ADD CONSTRAINT FK_Temp_SalesInvoiceDetail_GovtDuty_Setup_GovtDuty FOREIGN KEY (GovtDutyId) REFERENCES Setup_GovtDuty (GovtDutyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_SalesInvoiceDetail_GovtDuty ADD CONSTRAINT FK_Temp_SalesInvoiceDetail_GovtDuty_Temp_SalesInvoiceDetail FOREIGN KEY (InvoiceDetailId) REFERENCES Temp_SalesInvoiceDetail (InvoiceDetailId) ON UPDATE NO ACTION ON DELETE NO ACTION");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A = Amount, P = Percentage' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Temp_SalesInvoiceDetail_GovtDuty', @level2type=N'COLUMN',@level2name=N'RateType'");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A = Amount, P = Percentage' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Temp_SalesInvoiceDetail_GovtDuty', @level2type=N'COLUMN',@level2name=N'ExemptedType'");
            }

            //if table not found then create table
            if (!CheckTable("Temp_SalesInvoiceDetailSerial"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Temp_SalesInvoiceDetailSerial(
	                [InvoiceDetailSerialId] [uniqueidentifier] NOT NULL,
	                [InvoiceDetailId] [uniqueidentifier] NOT NULL,
	                [Serial] [varchar](100) NOT NULL,
	                [AdditionalSerial] [varchar](100) NULL,
                 CONSTRAINT [PK_Temp_SalesInvoiceDetailSerial] PRIMARY KEY CLUSTERED ([InvoiceDetailSerialId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_SalesInvoiceDetailSerial ADD CONSTRAINT FK_Temp_SalesInvoiceDetailSerial_Temp_SalesInvoiceDetail FOREIGN KEY (InvoiceDetailId) REFERENCES Temp_SalesInvoiceDetail (InvoiceDetailId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Setup_PriceType", "LowerLimit"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_PriceType ADD [SelectedCurrency] [varchar] (5) NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_PriceType ADD [Currency1Rate] [numeric] (18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_PriceType ADD [Currency2Rate] [numeric] (18, 4) DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_PriceType ADD [RangeType] [varchar] (1) NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_PriceType ADD [LowerLimit] [numeric](18, 4) DEFAULT ((0)) NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_PriceType ADD [LowerLimit1] [numeric](18, 4) DEFAULT ((0)) NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_PriceType ADD [LowerLimit2] [numeric](18, 4) DEFAULT ((0)) NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_PriceType ADD [UpperLimit] [numeric](18, 4) DEFAULT ((0)) NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_PriceType ADD [UpperLimit1] [numeric](18, 4) DEFAULT ((0)) NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_PriceType ADD [UpperLimit2] [numeric](18, 4) DEFAULT ((0)) NOT NULL");
            }

            //if table not found then create table
            if (!CheckTable("Temp_Collection"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Temp_Collection(
	                [CollectionId] [uniqueidentifier] NOT NULL,
	                [CollectionNo] [varchar](50) NOT NULL,
	                [CollectionDate] [datetime] NOT NULL,
	                [SelectedCurrency] [varchar](5) NOT NULL,
	                [Currency1Rate] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Currency2Rate] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [CollectedAmount] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [CollectedAmount1] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [CollectedAmount2] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [CustomerId] [bigint] NOT NULL,
	                [SalesPersonId] [bigint] NOT NULL,
	                [CollectedBy] [bigint] NOT NULL,
	                [MRNo] [varchar](50) NULL,
	                [Remarks] [varchar](1000) NULL,
	                [OperationTypeId] [bigint] NOT NULL,
	                [OperationalEventId] [bigint] NOT NULL,
	                [SecurityDeposit] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [SecurityDeposit1] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [SecurityDeposit2] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [RecoveryDate] [datetime] NULL,
	                [OthersAdjustment] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [OthersAdjustment1] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [OthersAdjustment2] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Approved] [varchar](1) DEFAULT ('N') NOT NULL,
	                [ApprovedBy] [bigint] NULL,
	                [ApprovedDate] [datetime] NULL,
	                [CancelReason] [varchar](200) NULL,
	                [LocationId] [bigint] NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
	                [EntryBy] [bigint] NOT NULL,
	                [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Temp_Collection] PRIMARY KEY CLUSTERED ([CollectionId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
                 CONSTRAINT [IX_Temp_Collection] UNIQUE NONCLUSTERED ([CollectionNo] ASC, [CompanyId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_Collection ADD CONSTRAINT FK_Temp_Collection_Configuration_OperationalEvent FOREIGN KEY (OperationalEventId) REFERENCES Configuration_OperationalEvent (OperationalEventId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_Collection ADD CONSTRAINT FK_Temp_Collection_Configuration_OperationType FOREIGN KEY (OperationTypeId) REFERENCES Configuration_OperationType (OperationTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_Collection ADD CONSTRAINT FK_Temp_Collection_Security_User FOREIGN KEY (CollectedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_Collection ADD CONSTRAINT FK_Temp_Collection_Security_User1 FOREIGN KEY (ApprovedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_Collection ADD CONSTRAINT FK_Temp_Collection_Security_User2 FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_Collection ADD CONSTRAINT FK_Temp_Collection_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_Collection ADD CONSTRAINT FK_Temp_Collection_Setup_Customer FOREIGN KEY (CustomerId) REFERENCES Setup_Customer (CustomerId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_Collection ADD CONSTRAINT FK_Temp_Collection_Setup_Employee FOREIGN KEY (SalesPersonId) REFERENCES Setup_Employee (EmployeeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_Collection ADD CONSTRAINT FK_Temp_Collection_Setup_Location FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Temp_CollectionMapping"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Temp_CollectionMapping(
	                [MappingId] [uniqueidentifier] NOT NULL,
	                [CollectionId] [uniqueidentifier] NOT NULL,
	                [InvoiceId] [uniqueidentifier] NOT NULL,
	                [Amount] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Amount1] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Amount2] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
                 CONSTRAINT [PK_Temp_CollectionMapping] PRIMARY KEY CLUSTERED ([MappingId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_CollectionMapping ADD CONSTRAINT FK_Temp_CollectionMapping_Temp_Collection FOREIGN KEY (CollectionId) REFERENCES Temp_Collection (CollectionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_CollectionMapping ADD CONSTRAINT FK_Temp_CollectionMapping_Temp_SalesInvoice FOREIGN KEY (InvoiceId) REFERENCES Temp_SalesInvoice (InvoiceId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Temp_CollectionDetail"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Temp_CollectionDetail(
	                [CollectionDetailId] [uniqueidentifier] NOT NULL,
	                [CollectionId] [uniqueidentifier] NOT NULL,
	                [PaymentModeId] [bigint] NOT NULL,
	                [Amount] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Amount1] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Amount2] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
                 CONSTRAINT [PK_Temp_CollectionDetail] PRIMARY KEY CLUSTERED ([CollectionDetailId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_CollectionDetail ADD CONSTRAINT FK_Temp_CollectionDetail_Configuration_PaymentMode FOREIGN KEY (PaymentModeId) REFERENCES Configuration_PaymentMode (PaymentModeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_CollectionDetail ADD CONSTRAINT FK_Temp_CollectionDetail_Temp_Collection FOREIGN KEY (CollectionId) REFERENCES Temp_Collection (CollectionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Temp_ChequeInfo"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Temp_ChequeInfo(
	                [ChequeInfoId] [uniqueidentifier] NOT NULL,
	                [CollectionDetailId] [uniqueidentifier] NOT NULL,
	                [BankId] [bigint] NOT NULL,
	                [ChequeNo] [varchar](50) NOT NULL,
	                [ChequeDate] [datetime] NOT NULL,
	                [Amount] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Amount1] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Amount2] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
                 CONSTRAINT [PK_Temp_ChequeInfo] PRIMARY KEY CLUSTERED ([ChequeInfoId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_ChequeInfo ADD CONSTRAINT FK_Temp_ChequeInfo_Setup_Bank FOREIGN KEY (BankId) REFERENCES Setup_Bank (BankId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_ChequeInfo ADD CONSTRAINT FK_Temp_ChequeInfo_Temp_CollectionDetail FOREIGN KEY (CollectionDetailId) REFERENCES Temp_CollectionDetail (CollectionDetailId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_SalesInvoice", "IsFullPaid"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD IsFullPaid [bit] DEFAULT 0 NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD ApprovedCollectedAmount [numeric](18, 4) DEFAULT ((0)) NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD ApprovedCollected1Amount [numeric](18, 4) DEFAULT ((0)) NOT NULL");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice ADD ApprovedCollected2Amount [numeric](18, 4) DEFAULT ((0)) NOT NULL");
            }

            //if table not found then create table
            if (!CheckTable("Task_SalesQuotationNos"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_SalesQuotationNos(
	                [Id] [uniqueidentifier] NOT NULL,
	                [QuotationNo] [varchar](50) NOT NULL,
	                [Year] [bigint] NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
                 CONSTRAINT [PK_Task_SalesQuotationNos] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
                 CONSTRAINT [IX_Task_SalesQuotationNos] UNIQUE NONCLUSTERED ([QuotationNo] ASC,	[CompanyId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesQuotationNos ADD CONSTRAINT FK_Task_SalesQuotationNos_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_SalesQuotation"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_SalesQuotation(
	                [QuotationId] [uniqueidentifier] NOT NULL,
	                [QuotationNo] [varchar](50) NOT NULL,
	                [QuotationDate] [datetime] NOT NULL,
	                [CustomerId] [bigint] NOT NULL,
	                [CustomerDetail] [nvarchar](250) NULL,
	                [OriginalBuyerId] [bigint] NULL,
	                [SalesPersonId] [bigint] NOT NULL,
	                [SelectedCurrency] [varchar](5) NOT NULL,
	                [Currency1Rate] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Currency2Rate] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [ReferenceNo] [varchar](50) NULL,
	                [ReferenceDate] [datetime] NULL,
	                [OperationTypeId] [bigint] NOT NULL,
	                [RequestedBy] [varchar](50) NULL,
	                [TermsAndConditionsId] [bigint] NULL,
	                [TermsAndConditionsDetail] [nvarchar](4000) NULL,
	                [PaymentModeId] [bigint] NOT NULL,
	                [PromisedDate] [datetime] NULL,
	                [PaymentTermsId] [bigint] NULL,
	                [PaymentTermsDetail] [nvarchar](4000) NULL,
	                [BankGuaranteeRequired] [bit] DEFAULT ((1)) NOT NULL,
	                [WorkOrderRequired] [bit] DEFAULT ((1)) NOT NULL,
	                [CommissionType] [varchar](1) NOT NULL,
	                [CommissionValue] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Commission1Value] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Commission2Value] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [CommissionAmount] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Commission1Amount] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Commission2Amount] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [CommissionTakenBy] [varchar](50) NULL,
	                [CommissionTakerAddress] [nvarchar](250) NULL,
	                [CommissionContactPerson] [varchar](50) NULL,
	                [CommissionContactNo] [varchar](20) NULL,
	                [Approved] [varchar](1) DEFAULT ('N') NOT NULL,
	                [ApprovedBy] [bigint] NULL,
	                [ApprovedDate] [datetime] NULL,
	                [CancelReason] [varchar](200) NULL,
	                [IsSalesDone] [bit] DEFAULT ((0)) NOT NULL,
	                [LocationId] [bigint] NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
	                [EntryBy] [bigint] NOT NULL,
	                [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Task_SalesQuotation] PRIMARY KEY CLUSTERED ([QuotationId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesQuotation ADD CONSTRAINT FK_Task_SalesQuotation_Configuration_PaymentMode FOREIGN KEY (PaymentModeId) REFERENCES Configuration_PaymentMode (PaymentModeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesQuotation ADD CONSTRAINT FK_Task_SalesQuotation_Security_User FOREIGN KEY (ApprovedBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesQuotation ADD CONSTRAINT FK_Task_SalesQuotation_Security_User1 FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesQuotation ADD CONSTRAINT FK_Task_SalesQuotation_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesQuotation ADD CONSTRAINT FK_Task_SalesQuotation_Setup_Customer FOREIGN KEY (CustomerId) REFERENCES Setup_Customer (CustomerId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesQuotation ADD CONSTRAINT FK_Task_SalesQuotation_Setup_Customer1 FOREIGN KEY (OriginalBuyerId) REFERENCES Setup_Customer (CustomerId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesQuotation ADD CONSTRAINT FK_Task_SalesQuotation_Setup_Employee FOREIGN KEY (SalesPersonId) REFERENCES Setup_Employee (EmployeeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesQuotation ADD CONSTRAINT FK_Task_SalesQuotation_Setup_Location FOREIGN KEY (LocationId) REFERENCES Setup_Location (LocationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesQuotation ADD CONSTRAINT FK_Task_SalesQuotation_Setup_TermsAndConditions FOREIGN KEY (TermsAndConditionsId) REFERENCES Setup_TermsAndConditions (TermsAndConditionsId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesQuotation ADD CONSTRAINT FK_Task_SalesQuotation_Setup_TermsAndConditions1 FOREIGN KEY (PaymentTermsId) REFERENCES Setup_TermsAndConditions (TermsAndConditionsId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_SalesQuotation_Charge"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_SalesQuotation_Charge(
	                [QuotationChargeId] [uniqueidentifier] NOT NULL,
	                [QuotationId] [uniqueidentifier] NOT NULL,
	                [ChargeEventId] [bigint] NOT NULL,
	                [ChargeAmount] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Charge1Amount] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Charge2Amount] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
                 CONSTRAINT [PK_Task_SalesQuotation_Charge] PRIMARY KEY CLUSTERED ([QuotationChargeId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesQuotation_Charge ADD CONSTRAINT FK_Task_SalesQuotation_Charge_Configuration_EventWiseCharge FOREIGN KEY (ChargeEventId) REFERENCES Configuration_EventWiseCharge (ChargeEventId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesQuotation_Charge ADD CONSTRAINT FK_Task_SalesQuotation_Charge_Task_SalesQuotation FOREIGN KEY (QuotationId) REFERENCES Task_SalesQuotation (QuotationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_SalesQuotation_DeliveryInfo"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_SalesQuotation_DeliveryInfo(
	                [DeliveryInfoId] [uniqueidentifier] NOT NULL,
	                [QuotationId] [uniqueidentifier] NOT NULL,
	                [DeliveryPlace] [nvarchar](500) NULL,
	                [ContactPerson] [varchar](100) NULL,
	                [ContactPersonNo] [varchar](30) NULL,
	                [TransportId] [bigint] NULL,
	                [TransportTypeId] [bigint] NULL,
	                [VehicleNo] [varchar](30) NULL,
	                [DriverName] [varchar](100) NULL,
	                [DriverContactNo] [varchar](30) NULL,
                 CONSTRAINT [PK_Task_SalesQuotationDeliveryInfo] PRIMARY KEY CLUSTERED ([DeliveryInfoId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesQuotation_DeliveryInfo ADD CONSTRAINT FK_Task_SalesQuotationDeliveryInfo_Setup_Transport FOREIGN KEY (TransportId) REFERENCES Setup_Transport (TransportId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesQuotation_DeliveryInfo ADD CONSTRAINT FK_Task_SalesQuotationDeliveryInfo_Setup_TransportType FOREIGN KEY (TransportTypeId) REFERENCES Setup_TransportType (TransportTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesQuotation_DeliveryInfo ADD CONSTRAINT FK_Task_SalesQuotationDeliveryInfo_Task_SalesQuotation FOREIGN KEY (QuotationId) REFERENCES Task_SalesQuotation (QuotationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_SalesQuotation_GovtDutyAdjustment"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_SalesQuotation_GovtDutyAdjustment(
	                [QuotationDutyAdjustmentId] [uniqueidentifier] NOT NULL,
	                [QuotationId] [uniqueidentifier] NOT NULL,
	                [GovtDutyAdjustmentId] [bigint] NOT NULL,
	                [Amount] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Amount1] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Amount2] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
                 CONSTRAINT [PK_Task_SalesQuotation_GovtDutyAdjustment] PRIMARY KEY CLUSTERED ([QuotationDutyAdjustmentId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesQuotation_GovtDutyAdjustment ADD CONSTRAINT FK_Task_SalesQuotation_GovtDutyAdjustment_Setup_GovtDutyAdjustment FOREIGN KEY (GovtDutyAdjustmentId) REFERENCES Setup_GovtDutyAdjustment (AdjustmentId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesQuotation_GovtDutyAdjustment ADD CONSTRAINT FK_Task_SalesQuotation_GovtDutyAdjustment_Task_SalesQuotation FOREIGN KEY (QuotationId) REFERENCES Task_SalesQuotation (QuotationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_SalesQuotation_Header"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_SalesQuotation_Header(
	                [HeaderId] [uniqueidentifier] NOT NULL,
	                [QuotationId] [uniqueidentifier] NOT NULL,
	                [Salutation] [varchar](10) NOT NULL,
	                [AttentionToName] [varchar](50) NOT NULL,
	                [Designation] [varchar](50) NULL,
	                [CompanyName] [varchar](50) NOT NULL,
	                [Address] [nvarchar](250) NULL,
	                [HeaderSubject] [nvarchar](250) NOT NULL,
	                [HeaderDetail] [nvarchar](4000) NULL,
                 CONSTRAINT [PK_Task_SalesQuotation_Header] PRIMARY KEY CLUSTERED ([HeaderId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesQuotation_Header ADD CONSTRAINT FK_Task_SalesQuotation_Header_Task_SalesQuotation FOREIGN KEY (QuotationId) REFERENCES Task_SalesQuotation (QuotationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_SalesQuotation_SecurityAndBanking"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_SalesQuotation_SecurityAndBanking(
	                [SecurityAndBankingId] [uniqueidentifier] NOT NULL,
	                [QuotationId] [uniqueidentifier] NOT NULL,
	                [EMPercentValue] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [EMAmount] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [EMAmount1] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [EMAmount2] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [EMReferenceNo] [varchar](50) NULL,
	                [EMRefundDate] [datetime] NULL,
	                [SDPercentValue] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [SDAmount] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [SDAmount1] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [SDAmount2] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [SDReferenceNo] [varchar](50) NULL,
	                [SDRefundDate] [datetime] NULL,
	                [BGPercentValue] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [BGAmount] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [BGAmount1] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [BGAmount2] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [BGMarginAmount] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [BGMarginAmount1] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [BGMarginAmount2] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [BankExpenseAmount] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [BankExpenseAmount1] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [BankExpenseAmount2] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [BGNo] [varchar](50) NULL,
	                [BankId] [bigint] NULL,
	                [BGRefundDate] [datetime] NULL,
                 CONSTRAINT [PK_Task_SalesQuotation_SecurityAndBanking] PRIMARY KEY CLUSTERED ([SecurityAndBankingId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesQuotation_SecurityAndBanking ADD CONSTRAINT FK_Task_SalesQuotation_SecurityAndBanking_Setup_Bank FOREIGN KEY (BankId) REFERENCES Setup_Bank (BankId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesQuotation_SecurityAndBanking ADD CONSTRAINT FK_Task_SalesQuotation_SecurityAndBanking_Task_SalesQuotation FOREIGN KEY (QuotationId) REFERENCES Task_SalesQuotation (QuotationId) ON UPDATE NO ACTION ON DELETE NO ACTION");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Earnest Money Percentage Value' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_SalesQuotation_SecurityAndBanking', @level2type=N'COLUMN',@level2name=N'EMPercentValue'");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Security Deposite Percentage Value' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_SalesQuotation_SecurityAndBanking', @level2type=N'COLUMN',@level2name=N'SDPercentValue'");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bank Guarantee Percentage Value' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_SalesQuotation_SecurityAndBanking', @level2type=N'COLUMN',@level2name=N'BGPercentValue'");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bank Guarantee Number' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_SalesQuotation_SecurityAndBanking', @level2type=N'COLUMN',@level2name=N'BGNo'");
            }

            //if table not found then create table
            if (!CheckTable("Task_SalesQuotation_Header"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_SalesQuotation_Header(
	                [HeaderId] [uniqueidentifier] NOT NULL,
	                [QuotationId] [uniqueidentifier] NOT NULL,
	                [Salutation] [varchar](10) NOT NULL,
	                [AttentionToName] [varchar](50) NOT NULL,
	                [Designation] [varchar](50) NULL,
	                [CompanyName] [varchar](50) NOT NULL,
	                [Address] [nvarchar](250) NULL,
	                [HeaderSubject] [nvarchar](250) NOT NULL,
	                [HeaderDetail] [nvarchar](4000) NULL,
                 CONSTRAINT [PK_Task_SalesQuotation_Header] PRIMARY KEY CLUSTERED ([HeaderId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesQuotation_Header ADD CONSTRAINT FK_Task_SalesQuotation_Header_Task_SalesQuotation FOREIGN KEY (QuotationId) REFERENCES Task_SalesQuotation (QuotationId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Task_SalesQuotationDetail"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_SalesQuotationDetail(
	                [QuotaionDetailId] [uniqueidentifier] NOT NULL,
	                [QuotationId] [uniqueidentifier] NOT NULL,
	                [ProductId] [bigint] NOT NULL,
	                [ProductDimensionId] [bigint] NULL,
	                [UnitTypeId] [bigint] NOT NULL,
	                [Quantity] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [OrderedQuantity] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Price] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Price1] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Price2] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [DiscountType] [varchar](1) NOT NULL,
	                [DiscountValue] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Discount1Value] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Discount2Value] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Discount] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Discount1] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [Discount2] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [AdditionalDescription] [varchar](500) NULL,
	                [IsSampleRequired] [bit] NOT NULL,
	                [SampleProvidedBy] [varchar](50) NULL,
	                [SampleProvidedDate] [datetime] NULL,
	                [RequestedBy] [bigint] NULL,
                 CONSTRAINT [PK_Task_SalesQuotationDetail] PRIMARY KEY CLUSTERED ([QuotaionDetailId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesQuotationDetail ADD CONSTRAINT FK_Task_SalesQuotationDetail_Setup_Employee FOREIGN KEY (RequestedBy) REFERENCES Setup_Employee (EmployeeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesQuotationDetail ADD CONSTRAINT FK_Task_SalesQuotationDetail_Setup_Product FOREIGN KEY (ProductId) REFERENCES Setup_Product (ProductId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesQuotationDetail ADD CONSTRAINT FK_Task_SalesQuotationDetail_Setup_ProductDimension FOREIGN KEY (ProductDimensionId) REFERENCES Setup_ProductDimension (ProductDimensionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesQuotationDetail ADD CONSTRAINT FK_Task_SalesQuotationDetail_Setup_UnitType FOREIGN KEY (UnitTypeId) REFERENCES Setup_UnitType (UnitTypeId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesQuotationDetail ADD CONSTRAINT FK_Task_SalesQuotationDetail_Task_SalesQuotation FOREIGN KEY (QuotationId) REFERENCES Task_SalesQuotation (QuotationId) ON UPDATE NO ACTION ON DELETE NO ACTION");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'True = Including, False = Excluding' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_SalesQuotationDetail', @level2type=N'COLUMN',@level2name=N'IsSampleRequired'");
            }

            //if table not found then create table
            if (!CheckTable("Task_SalesQuotationDetail_GovtDuty"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Task_SalesQuotationDetail_GovtDuty(
	                [Id] [uniqueidentifier] NOT NULL,
	                [QuotaionDetailId] [uniqueidentifier] NOT NULL,
	                [GovtDutyId] [bigint] NOT NULL,
	                [RateType] [varchar](1) DEFAULT ('P') NOT NULL,
	                [RateValue] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [RateValue1] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [RateValue2] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [RateAmount] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [RateAmount1] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [RateAmount2] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [ExemptedType] [varchar](1) DEFAULT ('P') NOT NULL,
	                [ExemptedValue] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [ExemptedValue1] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [ExemptedValue2] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [ExemptedAmount] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [ExemptedAmount1] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
	                [ExemptedAmount2] [numeric](18, 4) DEFAULT ((0)) NOT NULL,
                 CONSTRAINT [PK_Task_SalesQuotationDetail_GovtDuty] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesQuotationDetail_GovtDuty ADD CONSTRAINT FK_Task_SalesQuotationDetail_GovtDuty_Setup_GovtDuty FOREIGN KEY (GovtDutyId) REFERENCES Setup_GovtDuty (GovtDutyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesQuotationDetail_GovtDuty ADD CONSTRAINT FK_Task_SalesQuotationDetail_GovtDuty_Task_SalesQuotationDetail FOREIGN KEY (QuotaionDetailId) REFERENCES Task_SalesQuotationDetail (QuotaionDetailId) ON UPDATE NO ACTION ON DELETE NO ACTION");

                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A = Amount, P = Percentage' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_SalesQuotationDetail_GovtDuty', @level2type=N'COLUMN',@level2name=N'RateType'");
                _db.Database.ExecuteSqlCommand("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A = Amount, P = Percentage' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Task_SalesQuotationDetail_GovtDuty', @level2type=N'COLUMN',@level2name=N'ExemptedType'");
            }

            //add column if not found
            if (!CheckTable("Temp_CustomerSupplierOutstanding", "Code"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Temp_CustomerSupplierOutstanding ADD [Code] [varchar] (50) NULL");
            }

            //add column if not found
            if (!CheckTable("Task_SalesInvoiceDetail", "ChallanDetailId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD [ChallanDetailId] [uniqueidentifier] NULL");
                _db.Database.ExecuteSqlCommand("UPDATE Task_SalesInvoiceDetail SET ChallanDetailId =Task_DeliveryChallanDetail.ChallanDetailId FROM Task_DeliveryChallanDetail INNER JOIN Task_SalesInvoiceDetail "
                    + "ON Task_DeliveryChallanDetail.ChallanId = Task_SalesInvoiceDetail.ChallanId AND Task_DeliveryChallanDetail.ProductId = Task_SalesInvoiceDetail.ProductId "
                    + "AND COALESCE(Task_DeliveryChallanDetail.ProductDimensionId, 0) = COALESCE(Task_SalesInvoiceDetail.ProductDimensionId, 0) AND Task_DeliveryChallanDetail.UnitTypeId = Task_SalesInvoiceDetail.UnitTypeId "
                    + "AND COALESCE(Task_DeliveryChallanDetail.GoodsReceiveId, 0x0) = COALESCE(Task_SalesInvoiceDetail.GoodsReceiveId, 0x0) AND COALESCE(Task_DeliveryChallanDetail.ImportedStockInId, 0x0) = COALESCE(Task_SalesInvoiceDetail.ImportedStockInId, 0x0) "
                    + "AND COALESCE(Task_DeliveryChallanDetail.SupplierId, 0) = COALESCE(Task_SalesInvoiceDetail.SupplierId, 0) AND COALESCE (Task_DeliveryChallanDetail.WareHouseId, 0) = COALESCE (Task_SalesInvoiceDetail.WareHouseId, 0)");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ALTER COLUMN ChallanDetailId [uniqueidentifier] NOT NULL");
            }

            //add new relationship if not found
            if (!CheckRelationshipBetweenTwoTables("Task_SalesInvoiceDetail", "FK_Task_SalesInvoiceDetail_Task_DeliveryChallanDetail"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail ADD CONSTRAINT FK_Task_SalesInvoiceDetail_Task_DeliveryChallanDetail FOREIGN KEY (ChallanDetailId) REFERENCES Task_DeliveryChallanDetail (ChallanDetailId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            ////drop relationship if found
            //if (CheckRelationshipBetweenTwoTables("Task_SalesInvoiceDetail", "FK_Task_SalesInvoiceDetail_Task_DeliveryChallan"))
            //{
            //    _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail DROP CONSTRAINT FK_Task_SalesInvoiceDetail_Task_DeliveryChallan");
            //    _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoiceDetail DROP COLUMN ChallanId");
            //}

            if (CheckTable("Setup_Company", "IsCompanyNameShow"))
            {
                DropDefaultConstraintByColumn("Setup_Company", "IsCompanyNameShow");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Company DROP COLUMN IsCompanyNameShow");
            }

            if (CheckTable("Setup_Company", "IsGovtDutyApplicable"))
            {
                DropDefaultConstraintByColumn("Setup_Company", "IsGovtDutyApplicable");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Company DROP COLUMN IsGovtDutyApplicable");
            }

            if (CheckTable("Setup_Company", "IsGovtDutyAdjApplicable"))
            {
                DropDefaultConstraintByColumn("Setup_Company", "IsGovtDutyAdjApplicable");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_Company DROP COLUMN IsGovtDutyAdjApplicable");
            }

            if (CheckTable("Task_SalesInvoice", "InvoiceType"))
            {
                DropDefaultConstraintByColumn("Task_SalesInvoice", "InvoiceType");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_SalesInvoice DROP COLUMN InvoiceType");
            }

            //if table not found then create table
            if (!CheckTable("Setup_GeoRegion"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Setup_GeoRegion(
	                [RegionId] [bigint] NOT NULL,
	                [RegionName] [varchar](100) NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
	                [EntryBy] [bigint] NOT NULL,
	                [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Setup_GeoRegion] PRIMARY KEY CLUSTERED ([RegionId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
                 CONSTRAINT [IX_Setup_GeoRegion] UNIQUE NONCLUSTERED ([RegionName] ASC, [CompanyId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_GeoRegion ADD CONSTRAINT FK_Setup_GeoRegion_Security_User FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_GeoRegion ADD CONSTRAINT FK_Setup_GeoRegion_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Setup_GeoDivision"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Setup_GeoDivision(
	                [DivisionId] [bigint] NOT NULL,
	                [RegionId] [bigint] NOT NULL,
	                [DivisionName] [varchar](100) NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
	                [EntryBy] [bigint] NOT NULL,
	                [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Setup_GeoDivision] PRIMARY KEY CLUSTERED ([DivisionId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
                 CONSTRAINT [IX_Setup_GeoDivision] UNIQUE NONCLUSTERED ([RegionId] ASC, [DivisionName] ASC, [CompanyId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_GeoDivision ADD CONSTRAINT FK_Setup_GeoDivision_Setup_GeoRegion FOREIGN KEY (RegionId) REFERENCES Setup_GeoRegion (RegionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_GeoDivision ADD CONSTRAINT FK_Setup_GeoDivision_Security_User FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_GeoDivision ADD CONSTRAINT FK_Setup_GeoDivision_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //if table not found then create table
            if (!CheckTable("Setup_GeoDistrict"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Setup_GeoDistrict(
	                [DistrictId] [bigint] NOT NULL,
	                [DivisionId] [bigint] NOT NULL,
	                [DistrictName] [varchar](100) NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
	                [EntryBy] [bigint] NOT NULL,
	                [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Setup_GeoDistrict] PRIMARY KEY CLUSTERED ([DistrictId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
                 CONSTRAINT [IX_Setup_GeoDistrict] UNIQUE NONCLUSTERED ([DivisionId] ASC, [DistrictName] ASC, [CompanyId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_GeoDistrict ADD CONSTRAINT FK_Setup_GeoDistrict_Setup_GeoDivision FOREIGN KEY (DivisionId) REFERENCES Setup_GeoDivision (DivisionId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_GeoDistrict ADD CONSTRAINT FK_Setup_GeoDistrict_Security_User FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_GeoDistrict ADD CONSTRAINT FK_Setup_GeoDistrict_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_ConvertionDetail", "GoodsReceiveId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ConvertionDetail ADD [GoodsReceiveId] [uniqueidentifier] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ConvertionDetail ADD CONSTRAINT FK_Task_ConvertionDetail_Task_GoodsReceive FOREIGN KEY (GoodsReceiveId) REFERENCES Task_GoodsReceive (ReceiveId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_ConvertionDetail", "ImportedStockInId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ConvertionDetail ADD [ImportedStockInId] [uniqueidentifier] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ConvertionDetail ADD CONSTRAINT FK_Task_ConvertionDetail_Task_ImportedStockIn FOREIGN KEY (ImportedStockInId) REFERENCES Task_ImportedStockIn (StockInId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_ConvertionDetail", "SupplierId"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ConvertionDetail ADD [SupplierId] [bigint] NULL");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ConvertionDetail ADD CONSTRAINT FK_Task_ConvertionDetail_Setup_Supplier FOREIGN KEY (SupplierId) REFERENCES Setup_Supplier (SupplierId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

            //add column if not found
            if (!CheckTable("Task_ConvertionDetail", "ReferenceNo"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ConvertionDetail ADD [ReferenceNo] [varchar](50) DEFAULT ' ' NOT NULL");
            }

            //add column if not found
            if (!CheckTable("Task_ConvertionDetail", "ReferenceDate"))
            {
                _db.Database.ExecuteSqlCommand("ALTER TABLE Task_ConvertionDetail ADD [ReferenceDate] [datetime] NULL");
            }

            //if table not found then create table
            if (!CheckTable("Setup_GeoPoliceStation"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE TABLE Setup_GeoPoliceStation(
	                [PoliceStationId] [bigint] NOT NULL,
	                [DistrictId] [bigint] NOT NULL,
	                [PoliceStationName] [varchar](100) NOT NULL,
	                [CompanyId] [bigint] NOT NULL,
	                [EntryBy] [bigint] NOT NULL,
	                [EntryDate] [datetime] NOT NULL,
                 CONSTRAINT [PK_Setup_GeoPoliceStation] PRIMARY KEY CLUSTERED ([PoliceStationId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
                 CONSTRAINT [IX_Setup_GeoPoliceStation] UNIQUE NONCLUSTERED ([DistrictId] ASC, [PoliceStationName] ASC, [CompanyId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_GeoPoliceStation ADD CONSTRAINT FK_Setup_GeoPoliceStation_Setup_GeoDistrict FOREIGN KEY (DistrictId) REFERENCES Setup_GeoDistrict (DistrictId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_GeoPoliceStation ADD CONSTRAINT FK_Setup_GeoPoliceStation_Security_User FOREIGN KEY (EntryBy) REFERENCES Security_User (SecurityUserId) ON UPDATE NO ACTION ON DELETE NO ACTION");
                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_GeoPoliceStation ADD CONSTRAINT FK_Setup_GeoPoliceStation_Setup_Company FOREIGN KEY (CompanyId) REFERENCES Setup_Company (CompanyId) ON UPDATE NO ACTION ON DELETE NO ACTION");
            }

        }

        private void CreateOrAlterFunction()
        {
            //if function not found then create function
            if (!CheckTable("fn_CalculateOpeningBalanceForAccounts"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE FUNCTION fn_CalculateOpeningBalanceForAccounts
                (
                    @AccountsId bigint,
	                @CompanyId bigint,
	                @DateFrom date
                )
                RETURNS numeric(18, 4)
                AS
                BEGIN
	                DECLARE @ClosingBalance numeric(18, 4);
	                DECLARE @Acc_OpeningDate date;
	                DECLARE @Acc_OpeningBalance numeric(18, 4);

	                SET @ClosingBalance = 0;

	                SELECT @Acc_OpeningDate = OpeningDate, @Acc_OpeningBalance = OpeningBalance FROM Setup_Accounts WHERE CompanyId = @CompanyId AND AccountsId = @AccountsId

	                IF (@Acc_OpeningDate < @DateFrom)
	                BEGIN
						SELECT @ClosingBalance = COALESCE(SUM(Amount), 0) FROM Task_PostedVoucher WHERE AccountsId = @AccountsId AND CAST(Date AS date) > @Acc_OpeningDate AND CAST(Date AS date) < @DateFrom
	                END

					SET @ClosingBalance = @ClosingBalance + @Acc_OpeningBalance

	                RETURN @ClosingBalance
                END");
            }

            //       //if function found then alter function
            //       if (CheckTable("fn_CalculateOpeningBalanceForAccounts"))
            //       {
            //           _db.Database.ExecuteSqlCommand(@"ALTER FUNCTION fn_CalculateOpeningBalanceForAccounts
            //           (
            //@CurrencyLevel int,
            //               @AccountsId bigint,
            //            @CompanyId bigint,
            //            @DateFrom date
            //           )
            //           RETURNS numeric(18, 4)
            //           AS
            //           BEGIN
            //            DECLARE @ClosingBalance numeric(18, 4);
            //            DECLARE @Acc_OpeningDate date;
            //            DECLARE @Acc_OpeningBalance numeric(18, 4);

            //            SET @ClosingBalance = 0;

            //            SELECT @Acc_OpeningDate = OpeningDate, @Acc_OpeningBalance = CASE @CurrencyLevel WHEN 1 THEN OpeningBalance WHEN 2 THEN OpeningBalance1 WHEN 3 THEN OpeningBalance2 END FROM Setup_Accounts WHERE CompanyId = @CompanyId AND AccountsId = @AccountsId

            //            IF (@Acc_OpeningDate < @DateFrom)
            //            BEGIN
            //	SELECT @ClosingBalance = COALESCE(SUM(CASE @CurrencyLevel WHEN 1 THEN Amount WHEN 2 THEN Currency1Amount WHEN 3 THEN Currency2Amount END), 0) FROM Task_PostedVoucher WHERE CompanyId = @CompanyId AND AccountsId = @AccountsId AND CAST(Date AS date) > @Acc_OpeningDate AND CAST(Date AS date) < @DateFrom
            //            END

            //SET @ClosingBalance = @ClosingBalance + @Acc_OpeningBalance

            //            RETURN @ClosingBalance
            //           END");
            //       }

            //if function found then alter function
            if (CheckTable("fn_CalculateOpeningBalanceForAccounts"))
            {
                _db.Database.ExecuteSqlCommand(@"ALTER FUNCTION fn_CalculateOpeningBalanceForAccounts
                (
					@CurrencyLevel int,
                    @AccountsId bigint,
					@LocationId bigint,
	                @CompanyId bigint,
	                @DateFrom date
                )
                RETURNS numeric(18, 4)
                AS
                BEGIN
	                DECLARE @ClosingBalance numeric(18, 4);
	                DECLARE @Acc_OpeningDate date;
	                DECLARE @Acc_OpeningBalance numeric(18, 4);
					DECLARE @FeatureLocationWiseAccountsBalance bit;
					DECLARE @QueryDataCount int;

	                SET @ClosingBalance = 0;

					SELECT @FeatureLocationWiseAccountsBalance = [dbo].[fn_CompanyFeatureValue] ('LocationWiseAccountsBalance', @CompanyId)

					IF(@FeatureLocationWiseAccountsBalance = 0)
					BEGIN
						-- LocationWiseAccountsBalance = False
						SELECT @Acc_OpeningDate = Setup_Company.OpeningDate, @Acc_OpeningBalance = CASE @CurrencyLevel WHEN 1 THEN OpeningBalance WHEN 2 THEN OpeningBalance1 WHEN 3 THEN OpeningBalance2 END 
						FROM Setup_Accounts INNER JOIN Setup_Company ON Setup_Accounts.CompanyId = Setup_Company.CompanyId
						WHERE Setup_Accounts.CompanyId = @CompanyId AND AccountsId = @AccountsId
					END
	                ELSE
					BEGIN
						-- LocationWiseAccountsBalance = True
						SELECT @QueryDataCount = COUNT(Setup_Accounts.AccountsId) FROM Setup_Accounts INNER JOIN Setup_AccountsDetail_Location ON Setup_Accounts.AccountsId = Setup_AccountsDetail_Location.AccountsId
						WHERE  (Setup_Accounts.AccountsId = @AccountsId) AND (Setup_AccountsDetail_Location.LocationId = @LocationId) AND (Setup_Accounts.CompanyId = @CompanyId)

						IF (@QueryDataCount = 0)
						BEGIN
							SELECT @Acc_OpeningDate = Setup_Company.OpeningDate, @Acc_OpeningBalance = COALESCE((CASE @CurrencyLevel WHEN 1 THEN Setup_Accounts.OpeningBalance WHEN 2 THEN Setup_Accounts.OpeningBalance1 WHEN 3 THEN Setup_Accounts.OpeningBalance2 END), 0) FROM Setup_Company INNER JOIN Setup_Accounts ON Setup_Company.CompanyId = Setup_Accounts.CompanyId
							WHERE Setup_Accounts.AccountsId = @AccountsId AND Setup_Company.CompanyId = @CompanyId
						END
						ELSE
						BEGIN
							SELECT @Acc_OpeningDate = Setup_Company.OpeningDate, @Acc_OpeningBalance = COALESCE(SUM(CASE @CurrencyLevel WHEN 1 THEN Setup_AccountsDetail_Location.OpeningBalance WHEN 2 THEN Setup_AccountsDetail_Location.OpeningBalance1 WHEN 3 THEN Setup_AccountsDetail_Location.OpeningBalance2 END), 0)
							FROM Setup_Accounts INNER JOIN Setup_AccountsDetail_Location ON Setup_Accounts.AccountsId = Setup_AccountsDetail_Location.AccountsId
							INNER JOIN Setup_Company ON Setup_Accounts.CompanyId = Setup_Company.CompanyId
							WHERE  (Setup_Accounts.AccountsId = @AccountsId) AND (Setup_AccountsDetail_Location.LocationId = @LocationId) AND (Setup_Accounts.CompanyId = @CompanyId)
							GROUP BY Setup_Company.OpeningDate
						END
					END

	                IF (@Acc_OpeningDate < @DateFrom)
	                BEGIN
						SELECT @ClosingBalance = COALESCE(SUM(CASE @CurrencyLevel WHEN 1 THEN Amount WHEN 2 THEN Currency1Amount WHEN 3 THEN Currency2Amount END), 0) FROM Task_PostedVoucher 
						WHERE CompanyId = @CompanyId AND AccountsId = @AccountsId AND CAST(Date AS date) > @Acc_OpeningDate AND CAST(Date AS date) < @DateFrom
						AND (0 = (CASE WHEN @FeatureLocationWiseAccountsBalance = 1 AND @LocationId > 0 THEN 1 ELSE 0 END) OR LocationId = @LocationId)
	                END

					SET @ClosingBalance = @ClosingBalance + COALESCE(@Acc_OpeningBalance, 0)

	                RETURN @ClosingBalance
                END");
            }

            //if function not found then create function
            if (!CheckTable("fn_CurrencyLevel"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE FUNCTION fn_CurrencyLevel
                (
	                @CompanyId bigint,
	                @Currency varchar(5)
                )
                RETURNS int
                AS
                BEGIN
					DECLARE @BaseCurrency varchar(5);
					DECLARE @Currency1 varchar(5);
					DECLARE @Currency2 varchar(5);
					DECLARE @CurrencyLevel int;

					-- Get company currency
					SELECT @BaseCurrency = BaseCurrency, @Currency1 = Currency1, @Currency2 = Currency2 FROM Setup_Company WHERE CompanyId = @CompanyId

					IF(@BaseCurrency = @Currency)
					BEGIN
						SET @CurrencyLevel = 1
					END

					IF(@Currency1 = @Currency)
					BEGIN
						SET @CurrencyLevel = 2
					END

					IF(@Currency2 = @Currency)
					BEGIN
						SET @CurrencyLevel = 3
					END

	                RETURN @CurrencyLevel
                END");
            }

            //if function not found then create function
            if (!CheckTable("fn_CalculateOpeningBalanceForSupplierLedger"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE FUNCTION fn_CalculateOpeningBalanceForSupplierLedger
                (
					@CurrencyLevel int,
                    @SupplierId bigint,
	                @CompanyId bigint,
	                @DateFrom date
                )
                RETURNS numeric(18, 4)
                AS
                BEGIN
	                DECLARE @ClosingBalance numeric(18, 4);
					DECLARE @Amount numeric(18, 4);
					DECLARE @Amount1 numeric(18, 4);
					DECLARE @Amount2 numeric(18, 4);
					DECLARE @OpeningBalance numeric(18, 4);
					DECLARE @OpeningBalance1 numeric(18, 4);
					DECLARE @OpeningBalance2 numeric(18, 4);
	                DECLARE @Supplier_OpeningDate date;

	                SET @ClosingBalance = 0;

					-- START SUPPLIER
					IF @SupplierId > 0
					BEGIN
						SELECT @Supplier_OpeningDate = OpeningDate, @OpeningBalance = OpeningBalance, @OpeningBalance1 = OpeningBalance1, @OpeningBalance2 = OpeningBalance2 FROM Setup_Supplier WHERE CompanyId = @CompanyId AND SupplierId = @SupplierId
						IF @CurrencyLevel = 1
						BEGIN
							SET @ClosingBalance = @ClosingBalance + @OpeningBalance
						END
						ELSE IF @CurrencyLevel = 2
						BEGIN
							SET @ClosingBalance = @ClosingBalance + @OpeningBalance1
						END
						ELSE
						BEGIN
							SET @ClosingBalance = @ClosingBalance + @OpeningBalance2
						END

						IF (@Supplier_OpeningDate IS NULL)
						BEGIN
							SELECT @Supplier_OpeningDate = OpeningDate FROM Setup_Company WHERE CompanyId = @CompanyId
						END

						IF (@Supplier_OpeningDate < @DateFrom)
						BEGIN
							-- FOR RECEIVE FINALIZE (+)
							SET @Amount = 0 
							SET @Amount1 = 0
							SET @Amount2 = 0

							SELECT @Amount = COALESCE(SUM(FinalizeAmount), 0), @Amount1 = COALESCE(SUM(Finalize1Amount), 0) ,@Amount2 = COALESCE(SUM(Finalize2Amount), 0)
							FROM Task_ReceiveFinalize WHERE (SupplierId = @SupplierId) AND (CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(FinalizeDate AS date) > @Supplier_OpeningDate AND CAST(FinalizeDate AS date) < @DateFrom

							IF @CurrencyLevel = 1
							BEGIN
								SET @ClosingBalance = @ClosingBalance + @Amount
							END
							ELSE IF @CurrencyLevel = 2
							BEGIN
								SET @ClosingBalance = @ClosingBalance + @Amount1
							END
							ELSE
							BEGIN
								SET @ClosingBalance = @ClosingBalance + @Amount2
							END

							-- FOR PAYMENT (-)
							SET @Amount = 0 
							SET @Amount1 = 0
							SET @Amount2 = 0

							SELECT @Amount = COALESCE(SUM(PaidAmount), 0), @Amount1 = COALESCE(SUM(PaidAmount1), 0) ,@Amount2 = COALESCE(SUM(PaidAmount2), 0)
							FROM Task_Payment WHERE (SupplierId = @SupplierId) AND (CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(PaymentDate AS date) > @Supplier_OpeningDate AND CAST(PaymentDate AS date) < @DateFrom

							IF @CurrencyLevel = 1
							BEGIN
								SET @ClosingBalance = @ClosingBalance - @Amount
							END
							ELSE IF @CurrencyLevel = 2
							BEGIN
								SET @ClosingBalance = @ClosingBalance - @Amount1
							END
							ELSE
							BEGIN
								SET @ClosingBalance = @ClosingBalance - @Amount2
							END
						END
					END
					-- END SUPPLIER

	                RETURN @ClosingBalance
                END");
            }

            //if function found then alter function
            if (CheckTable("fn_CalculateOpeningBalanceForSupplierLedger"))
            {
                _db.Database.ExecuteSqlCommand(@"ALTER FUNCTION fn_CalculateOpeningBalanceForSupplierLedger
                (
					@CurrencyLevel int,
                    @SupplierId bigint,
	                @CompanyId bigint,
	                @DateFrom datetime
                )
                RETURNS numeric(18, 4)
                AS
                BEGIN
	                DECLARE @ClosingBalance numeric(18, 4);
					DECLARE @Amount numeric(18, 4);
					DECLARE @Amount1 numeric(18, 4);
					DECLARE @Amount2 numeric(18, 4);
					DECLARE @OpeningBalance numeric(18, 4);
					DECLARE @OpeningBalance1 numeric(18, 4);
					DECLARE @OpeningBalance2 numeric(18, 4);
	                DECLARE @Supplier_OpeningDate date;

	                SET @ClosingBalance = 0;

					-- START SUPPLIER
					IF @SupplierId > 0
					BEGIN
						SELECT @Supplier_OpeningDate = Setup_Company.OpeningDate, @OpeningBalance = OpeningBalance, @OpeningBalance1 = OpeningBalance1, @OpeningBalance2 = OpeningBalance2 
						FROM Setup_Supplier INNER JOIN Setup_Company ON Setup_Supplier.CompanyId = Setup_Company.CompanyId
						WHERE Setup_Supplier.CompanyId = @CompanyId AND SupplierId = @SupplierId

						IF @CurrencyLevel = 1
						BEGIN
							SET @ClosingBalance = @ClosingBalance + @OpeningBalance
						END
						ELSE IF @CurrencyLevel = 2
						BEGIN
							SET @ClosingBalance = @ClosingBalance + @OpeningBalance1
						END
						ELSE
						BEGIN
							SET @ClosingBalance = @ClosingBalance + @OpeningBalance2
						END

						IF (@Supplier_OpeningDate IS NULL)
						BEGIN
							SELECT @Supplier_OpeningDate = OpeningDate FROM Setup_Company WHERE CompanyId = @CompanyId
						END

						IF (@Supplier_OpeningDate < @DateFrom)
						BEGIN
							-- FOR GOODS RECEIVE (+)
							SET @Amount = 0 
							SET @Amount1 = 0
							SET @Amount2 = 0

							SELECT @Amount = COALESCE(SUM(ReceiveAmount - DiscountAmount), 0), @Amount1 = COALESCE(SUM(Receive1Amount - Discount1Amount), 0) ,@Amount2 = COALESCE(SUM(Receive2Amount - Discount2Amount), 0)
							FROM Task_GoodsReceive WHERE (SupplierId = @SupplierId) AND (CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(ReceiveDate AS date) > @Supplier_OpeningDate AND CAST(ReceiveDate AS datetime) < @DateFrom

							IF @CurrencyLevel = 1
							BEGIN
								SET @ClosingBalance = @ClosingBalance + @Amount
							END
							ELSE IF @CurrencyLevel = 2
							BEGIN
								SET @ClosingBalance = @ClosingBalance + @Amount1
							END
							ELSE
							BEGIN
								SET @ClosingBalance = @ClosingBalance + @Amount2
							END

							-- FOR PAYMENT (-)
							SET @Amount = 0 
							SET @Amount1 = 0
							SET @Amount2 = 0

							SELECT @Amount = COALESCE(SUM(PaidAmount + SecurityDeposit + OthersAdjustment), 0), @Amount1 = COALESCE(SUM(PaidAmount1 + SecurityDeposit1 + OthersAdjustment1), 0) ,@Amount2 = COALESCE(SUM(PaidAmount2 + SecurityDeposit2 + OthersAdjustment2), 0)
							FROM Task_Payment WHERE (SupplierId = @SupplierId) AND (CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(PaymentDate AS date) > @Supplier_OpeningDate AND CAST(PaymentDate AS datetime) < @DateFrom

							IF @CurrencyLevel = 1
							BEGIN
								SET @ClosingBalance = @ClosingBalance - @Amount
							END
							ELSE IF @CurrencyLevel = 2
							BEGIN
								SET @ClosingBalance = @ClosingBalance - @Amount1
							END
							ELSE
							BEGIN
								SET @ClosingBalance = @ClosingBalance - @Amount2
							END

							-- FOR PAYMENT DUTY ADJUSTMENT (-)
							SET @Amount = 0 
							SET @Amount1 = 0
							SET @Amount2 = 0

							SELECT @Amount = COALESCE(SUM(Task_Payment_GovtDutyAdjustment.Amount), 0), @Amount1 = COALESCE(SUM(Task_Payment_GovtDutyAdjustment.Amount1), 0), @Amount2 = COALESCE(SUM(Task_Payment_GovtDutyAdjustment.Amount2), 0)
							FROM Task_Payment INNER JOIN Task_Payment_GovtDutyAdjustment ON Task_Payment.PaymentId = Task_Payment_GovtDutyAdjustment.PaymentId							
							WHERE (Task_Payment.SupplierId = @SupplierId) AND (Task_Payment.CompanyId = @CompanyId) AND (Task_Payment.Approved = 'A') AND CAST(Task_Payment.PaymentDate AS date) > @Supplier_OpeningDate AND CAST(Task_Payment.PaymentDate AS datetime) < @DateFrom

							IF @CurrencyLevel = 1
							BEGIN
								SET @ClosingBalance = @ClosingBalance - @Amount
							END
							ELSE IF @CurrencyLevel = 2
							BEGIN
								SET @ClosingBalance = @ClosingBalance - @Amount1
							END
							ELSE
							BEGIN
								SET @ClosingBalance = @ClosingBalance - @Amount2
							END

							-- FOR PURCHASE RETURN (-)
							SET @Amount = 0 
							SET @Amount1 = 0
							SET @Amount2 = 0

							SELECT @Amount = COALESCE(SUM(ReturnAmount), 0), @Amount1 = COALESCE(SUM(Return1Amount), 0) ,@Amount2 = COALESCE(SUM(Return2Amount), 0)
							FROM Task_PurchaseReturn WHERE (SupplierId = @SupplierId) AND (CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(ReturnDate AS date) > @Supplier_OpeningDate AND CAST(ReturnDate AS datetime) < @DateFrom

							IF @CurrencyLevel = 1
							BEGIN
								SET @ClosingBalance = @ClosingBalance - @Amount
							END
							ELSE IF @CurrencyLevel = 2
							BEGIN
								SET @ClosingBalance = @ClosingBalance - @Amount1
							END
							ELSE
							BEGIN
								SET @ClosingBalance = @ClosingBalance - @Amount2
							END

							-- FOR ADJUSTMENT (+)
							SET @Amount = 0 
							SET @Amount1 = 0
							SET @Amount2 = 0

							SELECT @Amount = COALESCE(SUM(D.AdjustedAmount), 0), @Amount1 = COALESCE(SUM(D.AdjustedAmount1), 0) ,@Amount2 = COALESCE(SUM(D.AdjustedAmount2), 0)
							FROM Task_PartyAdjustmentDetail D INNER JOIN Task_PartyAdjustment A ON D.AdjustmentId = A.AdjustmentId 
							WHERE (A.AdjustmentFor = 'S') AND (A.AdjustmentNature = 'A') AND (D.SupplierId = @SupplierId) AND (A.CompanyId = @CompanyId) AND (A.Approved = 'A') AND CAST(A.AdjustmentDate AS date) > @Supplier_OpeningDate AND CAST(A.AdjustmentDate AS datetime) < @DateFrom

							IF @CurrencyLevel = 1
							BEGIN
								SET @ClosingBalance = @ClosingBalance + @Amount
							END
							ELSE IF @CurrencyLevel = 2
							BEGIN
								SET @ClosingBalance = @ClosingBalance + @Amount1
							END
							ELSE
							BEGIN
								SET @ClosingBalance = @ClosingBalance + @Amount2
							END

							-- FOR ADJUSTMENT (-)
							SET @Amount = 0 
							SET @Amount1 = 0
							SET @Amount2 = 0

							SELECT @Amount = COALESCE(SUM(D.AdjustedAmount), 0), @Amount1 = COALESCE(SUM(D.AdjustedAmount1), 0) ,@Amount2 = COALESCE(SUM(D.AdjustedAmount2), 0)
							FROM Task_PartyAdjustmentDetail D INNER JOIN Task_PartyAdjustment A ON D.AdjustmentId = A.AdjustmentId 
							WHERE (A.AdjustmentFor = 'S') AND (A.AdjustmentNature = 'D') AND (D.SupplierId = @SupplierId) AND (A.CompanyId = @CompanyId) AND (A.Approved = 'A') AND CAST(A.AdjustmentDate AS date) > @Supplier_OpeningDate AND CAST(A.AdjustmentDate AS datetime) < @DateFrom

							IF @CurrencyLevel = 1
							BEGIN
								SET @ClosingBalance = @ClosingBalance - @Amount
							END
							ELSE IF @CurrencyLevel = 2
							BEGIN
								SET @ClosingBalance = @ClosingBalance - @Amount1
							END
							ELSE
							BEGIN
								SET @ClosingBalance = @ClosingBalance - @Amount2
							END

                            -- For Balance Adjusted (+) From Cheque Treatment
							SET @Amount = 0 
							SET @Amount1 = 0
							SET @Amount2 = 0

							SELECT @Amount = COALESCE(SUM(Task_ChequeInfo.Amount), 0), @Amount1 = COALESCE(SUM(Task_ChequeInfo.Amount1), 0) ,@Amount2 = COALESCE(SUM(Task_ChequeInfo.Amount2), 0)
							FROM Task_ChequeInfo INNER JOIN Task_PaymentDetail ON Task_ChequeInfo.PaymentDetailId = Task_PaymentDetail.PaymentDetailId 
							INNER JOIN Task_Payment ON Task_PaymentDetail.PaymentId = Task_Payment.PaymentId 
							INNER JOIN Task_ChequeTreatment ON Task_ChequeInfo.ChequeInfoId = Task_ChequeTreatment.ChequeInfoId AND Task_ChequeInfo.Status = Task_ChequeTreatment.Status 
							INNER JOIN Task_Voucher ON Task_ChequeTreatment.VoucherId = Task_Voucher.VoucherId
							WHERE (Task_ChequeInfo.Status = 'B') AND (Task_Payment.SupplierId = @SupplierId) AND (Task_ChequeInfo.CompanyId = @CompanyId) AND (Task_Payment.Approved = 'A') AND CAST(Task_ChequeInfo.StatusDate AS date) > @Supplier_OpeningDate AND CAST(Task_ChequeInfo.StatusDate AS datetime) < @DateFrom

							IF @CurrencyLevel = 1
							BEGIN
								SET @ClosingBalance = @ClosingBalance + @Amount
							END
							ELSE IF @CurrencyLevel = 2
							BEGIN
								SET @ClosingBalance = @ClosingBalance + @Amount1
							END
							ELSE
							BEGIN
								SET @ClosingBalance = @ClosingBalance + @Amount2
							END

						END
					END
					-- END SUPPLIER

	                RETURN @ClosingBalance
                END");
            }

            //if function not found then create function
            if (!CheckTable("fn_CalculateOpeningBalanceForSupplierLedger_V2"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE FUNCTION fn_CalculateOpeningBalanceForSupplierLedger_V2
                (
	                @For varchar(1),
	                @CompanyId bigint,
	                @DateFrom datetime
                )
                RETURNS @T1 TABLE(
	                [CustomerId] [bigint] NULL,
	                [SupplierId] [bigint] NOT NULL,
	                [For] [varchar](1) NOT NULL,
	                [TransactionFrom] [varchar](50) NOT NULL,
	                [Amount] [numeric](18, 4) NOT NULL,
	                [Amount1] [numeric](18, 4) NOT NULL,
	                [Amount2] [numeric](18, 4) NOT NULL)
                AS
                BEGIN
	                DECLARE @Supplier_OpeningDate date;

	                -- START SUPPLIER
	                IF @For = 'S' OR @For = 'A'
	                BEGIN
		                SELECT @Supplier_OpeningDate = OpeningDate FROM Setup_Company WHERE CompanyId = @CompanyId

		                -- FOR OPENING BALANCE (AS USUAL)
		                INSERT INTO @T1 (CustomerId, SupplierId, [For], TransactionFrom, Amount, Amount1, Amount2)
		                SELECT Setup_Customer.CustomerId, Setup_Supplier.SupplierId, @For, 'SOB', Setup_Supplier.OpeningBalance, Setup_Supplier.OpeningBalance1, Setup_Supplier.OpeningBalance2
		                FROM Setup_Supplier INNER JOIN Setup_Company ON Setup_Supplier.CompanyId = Setup_Company.CompanyId 
		                LEFT OUTER JOIN Setup_Customer ON Setup_Supplier.SupplierId = Setup_Customer.SupplierId
		                WHERE  (Setup_Supplier.CompanyId = @CompanyId)

		                IF (@Supplier_OpeningDate < @DateFrom)
		                BEGIN
			                -- FOR GOODS RECEIVE (+)
			                INSERT INTO @T1 (CustomerId, SupplierId, [For], TransactionFrom, Amount, Amount1, Amount2)
			                SELECT Setup_Customer.CustomerId, Task_GoodsReceive.SupplierId, @For, 'GoodsReceive', COALESCE(SUM(ReceiveAmount - DiscountAmount), 0), COALESCE(SUM(Receive1Amount - Discount1Amount), 0), COALESCE(SUM(Receive2Amount - Discount2Amount), 0)
			                FROM Task_GoodsReceive INNER JOIN Setup_Supplier ON Task_GoodsReceive.SupplierId = Setup_Supplier.SupplierId
			                LEFT OUTER JOIN Setup_Customer ON Setup_Supplier.SupplierId = Setup_Customer.SupplierId
			                WHERE (Task_GoodsReceive.CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(ReceiveDate AS date) > @Supplier_OpeningDate AND CAST(ReceiveDate AS datetime) < @DateFrom			
			                GROUP BY Setup_Customer.CustomerId, Task_GoodsReceive.SupplierId
			                HAVING (Task_GoodsReceive.SupplierId IN (SELECT SupplierId FROM Setup_Supplier WHERE CompanyId = @CompanyId AND IsActive = 1
							--AND CASE WHEN @For = 'A' THEN (CASE WHEN @SupplierId = 0 THEN 1 WHEN @SupplierId = CustomerId THEN 1 ELSE 0 END) ELSE (CASE WHEN @SupplierId = 0 THEN 1 WHEN @SupplierId = SupplierId THEN 1 ELSE 0 END) END = 1))
							--AND CASE WHEN @SupplierId = 0 THEN 1 WHEN @SupplierId = SupplierId THEN 1 ELSE 0 END = 1
							))

			                -- FOR PAYMENT (-)
			                INSERT INTO @T1 (CustomerId, SupplierId, [For], TransactionFrom, Amount, Amount1, Amount2)
			                SELECT Setup_Customer.CustomerId, Task_Payment.SupplierId, @For, 'Payment', COALESCE(SUM(Task_Payment.PaidAmount + SecurityDeposit + OthersAdjustment), 0) * -1, COALESCE(SUM(Task_Payment.PaidAmount1 + SecurityDeposit1 + OthersAdjustment1), 0) * -1, COALESCE(SUM(Task_Payment.PaidAmount2 + SecurityDeposit2 + OthersAdjustment2), 0) * -1
			                FROM Task_Payment INNER JOIN Setup_Supplier ON Task_Payment.SupplierId = Setup_Supplier.SupplierId
			                LEFT OUTER JOIN Setup_Customer ON Setup_Supplier.SupplierId = Setup_Customer.SupplierId
			                WHERE (Task_Payment.CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(PaymentDate AS date) > @Supplier_OpeningDate AND CAST(PaymentDate AS datetime) < @DateFrom
			                GROUP BY Setup_Customer.CustomerId, Task_Payment.SupplierId
			                HAVING (Task_Payment.SupplierId IN (SELECT SupplierId FROM Setup_Supplier WHERE CompanyId = @CompanyId AND IsActive = 1))

			                -- FOR PAYMENT DUTY ADJUSTMENT (-)
			                INSERT INTO @T1 (CustomerId, SupplierId, [For], TransactionFrom, Amount, Amount1, Amount2)
			                SELECT Setup_Customer.CustomerId, Task_Payment.SupplierId, @For, 'PaymentDutyAdjNegative', COALESCE(SUM(Task_Payment_GovtDutyAdjustment.Amount), 0) * -1, COALESCE(SUM(Task_Payment_GovtDutyAdjustment.Amount1), 0) * -1, COALESCE(SUM(Task_Payment_GovtDutyAdjustment.Amount2), 0) * -1
			                FROM Task_Payment INNER JOIN Task_Payment_GovtDutyAdjustment ON Task_Payment.PaymentId = Task_Payment_GovtDutyAdjustment.PaymentId	
			                INNER JOIN Setup_Supplier ON Task_Payment.SupplierId = Setup_Supplier.SupplierId
			                LEFT OUTER JOIN Setup_Customer ON Setup_Supplier.SupplierId = Setup_Customer.SupplierId						
			                WHERE (Task_Payment.CompanyId = @CompanyId) AND (Task_Payment.Approved = 'A') AND CAST(Task_Payment.PaymentDate AS date) > @Supplier_OpeningDate AND CAST(Task_Payment.PaymentDate AS datetime) < @DateFrom
			                GROUP BY Setup_Customer.CustomerId, Task_Payment.SupplierId
			                HAVING (Task_Payment.SupplierId IN (SELECT SupplierId FROM Setup_Supplier WHERE CompanyId = @CompanyId AND IsActive = 1))

			                -- FOR PURCHASE RETURN (-)
			                INSERT INTO @T1 (CustomerId, SupplierId, [For], TransactionFrom, Amount, Amount1, Amount2)
			                SELECT Setup_Customer.CustomerId, Task_PurchaseReturn.SupplierId, @For, 'PurchaseReturn', COALESCE(SUM(ReturnAmount), 0) * -1, COALESCE(SUM(Return1Amount), 0) * -1, COALESCE(SUM(Return2Amount), 0) * -1
			                FROM Task_PurchaseReturn INNER JOIN Setup_Supplier ON Task_PurchaseReturn.SupplierId = Setup_Supplier.SupplierId
			                LEFT OUTER JOIN Setup_Customer ON Setup_Supplier.SupplierId = Setup_Customer.SupplierId
			                WHERE (Task_PurchaseReturn.CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(ReturnDate AS date) > @Supplier_OpeningDate AND CAST(ReturnDate AS datetime) < @DateFrom
			                GROUP BY Setup_Customer.CustomerId, Task_PurchaseReturn.SupplierId
			                HAVING (Task_PurchaseReturn.SupplierId IN (SELECT SupplierId FROM Setup_Supplier WHERE CompanyId = @CompanyId AND IsActive = 1))

			                -- FOR ADJUSTMENT (+)
			                INSERT INTO @T1 (CustomerId, SupplierId, [For], TransactionFrom, Amount, Amount1, Amount2)
			                SELECT Setup_Customer.CustomerId, D.SupplierId, @For, 'SupplierAdjPositive', COALESCE(SUM(D.AdjustedAmount), 0), COALESCE(SUM(D.AdjustedAmount1), 0), COALESCE(SUM(D.AdjustedAmount2), 0)
			                FROM Task_PartyAdjustmentDetail D INNER JOIN Task_PartyAdjustment A ON D.AdjustmentId = A.AdjustmentId 
			                INNER JOIN Setup_Supplier ON D.SupplierId = Setup_Supplier.SupplierId
			                LEFT OUTER JOIN Setup_Customer ON Setup_Supplier.SupplierId = Setup_Customer.SupplierId
			                WHERE (A.AdjustmentFor = 'S') AND (A.AdjustmentNature = 'A') AND (A.CompanyId = @CompanyId) AND (A.Approved = 'A') AND CAST(A.AdjustmentDate AS date) > @Supplier_OpeningDate AND CAST(A.AdjustmentDate AS datetime) < @DateFrom
			                GROUP BY Setup_Customer.CustomerId, D.SupplierId
			                HAVING (D.SupplierId IN (SELECT SupplierId FROM Setup_Supplier WHERE CompanyId = @CompanyId AND IsActive = 1))

			                -- FOR ADJUSTMENT (-)
			                INSERT INTO @T1 (CustomerId, SupplierId, [For], TransactionFrom, Amount, Amount1, Amount2)
			                SELECT Setup_Customer.CustomerId, D.SupplierId, @For, 'SupplierAdjPositive', COALESCE(SUM(D.AdjustedAmount), 0) * -1, COALESCE(SUM(D.AdjustedAmount1), 0) * -1, COALESCE(SUM(D.AdjustedAmount2), 0) * -1
			                FROM Task_PartyAdjustmentDetail D INNER JOIN Task_PartyAdjustment A ON D.AdjustmentId = A.AdjustmentId 
			                INNER JOIN Setup_Supplier ON D.SupplierId = Setup_Supplier.SupplierId
			                LEFT OUTER JOIN Setup_Customer ON Setup_Supplier.SupplierId = Setup_Customer.SupplierId
			                WHERE (A.AdjustmentFor = 'S') AND (A.AdjustmentNature = 'D') AND (A.CompanyId = @CompanyId) AND (A.Approved = 'A') AND CAST(A.AdjustmentDate AS date) > @Supplier_OpeningDate AND CAST(A.AdjustmentDate AS datetime) < @DateFrom
			                GROUP BY Setup_Customer.CustomerId, D.SupplierId
			                HAVING (D.SupplierId IN (SELECT SupplierId FROM Setup_Supplier WHERE CompanyId = @CompanyId AND IsActive = 1))

			                -- For Balance Adjusted (+) From Cheque Treatment
			                INSERT INTO @T1 (CustomerId, SupplierId, [For], TransactionFrom, Amount, Amount1, Amount2)
			                SELECT Setup_Customer.CustomerId, Task_Payment.SupplierId, @For, 'SupplierAdjPositive', COALESCE(SUM(Task_ChequeInfo.Amount), 0), COALESCE(SUM(Task_ChequeInfo.Amount1), 0), COALESCE(SUM(Task_ChequeInfo.Amount2), 0)
			                FROM Task_ChequeInfo INNER JOIN Task_PaymentDetail ON Task_ChequeInfo.PaymentDetailId = Task_PaymentDetail.PaymentDetailId 
			                INNER JOIN Task_Payment ON Task_PaymentDetail.PaymentId = Task_Payment.PaymentId 
			                INNER JOIN Task_ChequeTreatment ON Task_ChequeInfo.ChequeInfoId = Task_ChequeTreatment.ChequeInfoId AND Task_ChequeInfo.Status = Task_ChequeTreatment.Status 
			                INNER JOIN Task_Voucher ON Task_ChequeTreatment.VoucherId = Task_Voucher.VoucherId
			                INNER JOIN Setup_Supplier ON Task_Payment.SupplierId = Setup_Supplier.SupplierId
			                LEFT OUTER JOIN Setup_Customer ON Setup_Supplier.SupplierId = Setup_Customer.SupplierId
			                WHERE (Task_ChequeInfo.Status = 'B') AND (Task_ChequeInfo.CompanyId = @CompanyId) AND (Task_Payment.Approved = 'A') AND CAST(Task_ChequeInfo.StatusDate AS date) > @Supplier_OpeningDate AND CAST(Task_ChequeInfo.StatusDate AS datetime) < @DateFrom
			                GROUP BY Setup_Customer.CustomerId, Task_Payment.SupplierId
			                HAVING (Task_Payment.SupplierId IN (SELECT SupplierId FROM Setup_Supplier WHERE CompanyId = @CompanyId AND IsActive = 1))

		                END
	                END
	                -- END SUPPLIER

                  RETURN
                END");
            }

            //if function found then alter function
            if (CheckTable("fn_CalculateOpeningBalanceForSupplierLedger_V2"))
            {
                _db.Database.ExecuteSqlCommand(@"ALTER FUNCTION fn_CalculateOpeningBalanceForSupplierLedger_V2
                (
	                @For varchar(1),
	                @CompanyId bigint,
	                @DateFrom datetime
                )
                RETURNS @T1 TABLE(
	                [CustomerId] [bigint] NULL,
	                [SupplierId] [bigint] NOT NULL,
	                [For] [varchar](1) NOT NULL,
	                [TransactionFrom] [varchar](50) NOT NULL,
	                [Amount] [numeric](18, 4) NOT NULL,
	                [Amount1] [numeric](18, 4) NOT NULL,
	                [Amount2] [numeric](18, 4) NOT NULL)
                AS
                BEGIN
	                DECLARE @Supplier_OpeningDate date;

	                -- START SUPPLIER
	                IF @For = 'S' OR @For = 'A'
	                BEGIN
		                SELECT @Supplier_OpeningDate = OpeningDate FROM Setup_Company WHERE CompanyId = @CompanyId

		                -- FOR OPENING BALANCE (AS USUAL)
		                INSERT INTO @T1 (CustomerId, SupplierId, [For], TransactionFrom, Amount, Amount1, Amount2)
		                SELECT Setup_Customer.CustomerId, Setup_Supplier.SupplierId, @For, 'SOB', Setup_Supplier.OpeningBalance, Setup_Supplier.OpeningBalance1, Setup_Supplier.OpeningBalance2
		                FROM Setup_Supplier INNER JOIN Setup_Company ON Setup_Supplier.CompanyId = Setup_Company.CompanyId 
		                LEFT OUTER JOIN Setup_Customer ON Setup_Supplier.SupplierId = Setup_Customer.SupplierId
		                WHERE  (Setup_Supplier.CompanyId = @CompanyId)

		                IF (@Supplier_OpeningDate < @DateFrom)
		                BEGIN
			                -- FOR GOODS RECEIVE (+)
			                INSERT INTO @T1 (CustomerId, SupplierId, [For], TransactionFrom, Amount, Amount1, Amount2)
			                SELECT Setup_Customer.CustomerId, Task_GoodsReceive.SupplierId, @For, 'GoodsReceive', COALESCE(SUM(ReceiveAmount - DiscountAmount), 0), COALESCE(SUM(Receive1Amount - Discount1Amount), 0), COALESCE(SUM(Receive2Amount - Discount2Amount), 0)
			                FROM Task_GoodsReceive INNER JOIN Setup_Supplier ON Task_GoodsReceive.SupplierId = Setup_Supplier.SupplierId
			                LEFT OUTER JOIN Setup_Customer ON Setup_Supplier.SupplierId = Setup_Customer.SupplierId
			                WHERE (Task_GoodsReceive.CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(ReceiveDate AS date) > @Supplier_OpeningDate AND CAST(ReceiveDate AS datetime) < @DateFrom			
			                GROUP BY Setup_Customer.CustomerId, Task_GoodsReceive.SupplierId
			                HAVING (Task_GoodsReceive.SupplierId IN (SELECT SupplierId FROM Setup_Supplier WHERE CompanyId = @CompanyId AND IsActive = 1
							--AND CASE WHEN @For = 'A' THEN (CASE WHEN @SupplierId = 0 THEN 1 WHEN @SupplierId = CustomerId THEN 1 ELSE 0 END) ELSE (CASE WHEN @SupplierId = 0 THEN 1 WHEN @SupplierId = SupplierId THEN 1 ELSE 0 END) END = 1))
							--AND CASE WHEN @SupplierId = 0 THEN 1 WHEN @SupplierId = SupplierId THEN 1 ELSE 0 END = 1
							))

			                -- FOR PAYMENT (-)
			                INSERT INTO @T1 (CustomerId, SupplierId, [For], TransactionFrom, Amount, Amount1, Amount2)
			                SELECT Setup_Customer.CustomerId, Task_Payment.SupplierId, @For, 'Payment', COALESCE(SUM(Task_Payment.PaidAmount + SecurityDeposit + OthersAdjustment), 0) * -1, COALESCE(SUM(Task_Payment.PaidAmount1 + SecurityDeposit1 + OthersAdjustment1), 0) * -1, COALESCE(SUM(Task_Payment.PaidAmount2 + SecurityDeposit2 + OthersAdjustment2), 0) * -1
			                FROM Task_Payment INNER JOIN Setup_Supplier ON Task_Payment.SupplierId = Setup_Supplier.SupplierId
			                LEFT OUTER JOIN Setup_Customer ON Setup_Supplier.SupplierId = Setup_Customer.SupplierId
			                WHERE (Task_Payment.CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(PaymentDate AS date) > @Supplier_OpeningDate AND CAST(PaymentDate AS datetime) < @DateFrom
			                GROUP BY Setup_Customer.CustomerId, Task_Payment.SupplierId
			                HAVING (Task_Payment.SupplierId IN (SELECT SupplierId FROM Setup_Supplier WHERE CompanyId = @CompanyId AND IsActive = 1))

			                -- FOR PAYMENT DUTY ADJUSTMENT (-)
			                INSERT INTO @T1 (CustomerId, SupplierId, [For], TransactionFrom, Amount, Amount1, Amount2)
			                SELECT Setup_Customer.CustomerId, Task_Payment.SupplierId, @For, 'PaymentDutyAdjNegative', COALESCE(SUM(Task_Payment_GovtDutyAdjustment.Amount), 0) * -1, COALESCE(SUM(Task_Payment_GovtDutyAdjustment.Amount1), 0) * -1, COALESCE(SUM(Task_Payment_GovtDutyAdjustment.Amount2), 0) * -1
			                FROM Task_Payment INNER JOIN Task_Payment_GovtDutyAdjustment ON Task_Payment.PaymentId = Task_Payment_GovtDutyAdjustment.PaymentId	
			                INNER JOIN Setup_Supplier ON Task_Payment.SupplierId = Setup_Supplier.SupplierId
			                LEFT OUTER JOIN Setup_Customer ON Setup_Supplier.SupplierId = Setup_Customer.SupplierId						
			                WHERE (Task_Payment.CompanyId = @CompanyId) AND (Task_Payment.Approved = 'A') AND CAST(Task_Payment.PaymentDate AS date) > @Supplier_OpeningDate AND CAST(Task_Payment.PaymentDate AS datetime) < @DateFrom
			                GROUP BY Setup_Customer.CustomerId, Task_Payment.SupplierId
			                HAVING (Task_Payment.SupplierId IN (SELECT SupplierId FROM Setup_Supplier WHERE CompanyId = @CompanyId AND IsActive = 1))

			                -- FOR PURCHASE RETURN (-)
			                INSERT INTO @T1 (CustomerId, SupplierId, [For], TransactionFrom, Amount, Amount1, Amount2)
			                SELECT Setup_Customer.CustomerId, Task_PurchaseReturn.SupplierId, @For, 'PurchaseReturn', COALESCE(SUM(ReturnAmount), 0) * -1, COALESCE(SUM(Return1Amount), 0) * -1, COALESCE(SUM(Return2Amount), 0) * -1
			                FROM Task_PurchaseReturn INNER JOIN Setup_Supplier ON Task_PurchaseReturn.SupplierId = Setup_Supplier.SupplierId
			                LEFT OUTER JOIN Setup_Customer ON Setup_Supplier.SupplierId = Setup_Customer.SupplierId
			                WHERE (Task_PurchaseReturn.CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(ReturnDate AS date) > @Supplier_OpeningDate AND CAST(ReturnDate AS datetime) < @DateFrom
			                GROUP BY Setup_Customer.CustomerId, Task_PurchaseReturn.SupplierId
			                HAVING (Task_PurchaseReturn.SupplierId IN (SELECT SupplierId FROM Setup_Supplier WHERE CompanyId = @CompanyId AND IsActive = 1))

			                -- FOR ADJUSTMENT (+)
			                INSERT INTO @T1 (CustomerId, SupplierId, [For], TransactionFrom, Amount, Amount1, Amount2)
			                SELECT Setup_Customer.CustomerId, D.SupplierId, @For, 'SupplierAdjPositive', COALESCE(SUM(D.AdjustedAmount), 0), COALESCE(SUM(D.AdjustedAmount1), 0), COALESCE(SUM(D.AdjustedAmount2), 0)
			                FROM Task_PartyAdjustmentDetail D INNER JOIN Task_PartyAdjustment A ON D.AdjustmentId = A.AdjustmentId 
			                INNER JOIN Setup_Supplier ON D.SupplierId = Setup_Supplier.SupplierId
			                LEFT OUTER JOIN Setup_Customer ON Setup_Supplier.SupplierId = Setup_Customer.SupplierId
			                WHERE (A.AdjustmentFor = 'S') AND (A.AdjustmentNature = 'A') AND (A.CompanyId = @CompanyId) AND (A.Approved = 'A') AND CAST(A.AdjustmentDate AS date) > @Supplier_OpeningDate AND CAST(A.AdjustmentDate AS datetime) < @DateFrom
			                GROUP BY Setup_Customer.CustomerId, D.SupplierId
			                HAVING (D.SupplierId IN (SELECT SupplierId FROM Setup_Supplier WHERE CompanyId = @CompanyId AND IsActive = 1))

			                -- FOR ADJUSTMENT (-)
			                INSERT INTO @T1 (CustomerId, SupplierId, [For], TransactionFrom, Amount, Amount1, Amount2)
			                SELECT Setup_Customer.CustomerId, D.SupplierId, @For, 'SupplierAdjPositive', COALESCE(SUM(D.AdjustedAmount), 0) * -1, COALESCE(SUM(D.AdjustedAmount1), 0) * -1, COALESCE(SUM(D.AdjustedAmount2), 0) * -1
			                FROM Task_PartyAdjustmentDetail D INNER JOIN Task_PartyAdjustment A ON D.AdjustmentId = A.AdjustmentId 
			                INNER JOIN Setup_Supplier ON D.SupplierId = Setup_Supplier.SupplierId
			                LEFT OUTER JOIN Setup_Customer ON Setup_Supplier.SupplierId = Setup_Customer.SupplierId
			                WHERE (A.AdjustmentFor = 'S') AND (A.AdjustmentNature = 'D') AND (A.CompanyId = @CompanyId) AND (A.Approved = 'A') AND CAST(A.AdjustmentDate AS date) > @Supplier_OpeningDate AND CAST(A.AdjustmentDate AS datetime) < @DateFrom
			                GROUP BY Setup_Customer.CustomerId, D.SupplierId
			                HAVING (D.SupplierId IN (SELECT SupplierId FROM Setup_Supplier WHERE CompanyId = @CompanyId AND IsActive = 1))

			                -- For Balance Adjusted (+) From Cheque Treatment
			                INSERT INTO @T1 (CustomerId, SupplierId, [For], TransactionFrom, Amount, Amount1, Amount2)
			                SELECT Setup_Customer.CustomerId, Task_Payment.SupplierId, @For, 'SupplierAdjPositive', COALESCE(SUM(Task_ChequeInfo.Amount), 0), COALESCE(SUM(Task_ChequeInfo.Amount1), 0), COALESCE(SUM(Task_ChequeInfo.Amount2), 0)
			                FROM Task_ChequeInfo INNER JOIN Task_PaymentDetail ON Task_ChequeInfo.PaymentDetailId = Task_PaymentDetail.PaymentDetailId 
			                INNER JOIN Task_Payment ON Task_PaymentDetail.PaymentId = Task_Payment.PaymentId 
			                INNER JOIN Task_ChequeTreatment ON Task_ChequeInfo.ChequeInfoId = Task_ChequeTreatment.ChequeInfoId AND Task_ChequeInfo.Status = Task_ChequeTreatment.Status 
			                INNER JOIN Task_Voucher ON Task_ChequeTreatment.VoucherId = Task_Voucher.VoucherId
			                INNER JOIN Setup_Supplier ON Task_Payment.SupplierId = Setup_Supplier.SupplierId
			                LEFT OUTER JOIN Setup_Customer ON Setup_Supplier.SupplierId = Setup_Customer.SupplierId
			                WHERE (Task_ChequeInfo.Status = 'B') AND (Task_ChequeInfo.CompanyId = @CompanyId) AND (Task_Payment.Approved = 'A') AND CAST(Task_ChequeInfo.StatusDate AS date) > @Supplier_OpeningDate AND CAST(Task_ChequeInfo.StatusDate AS datetime) < @DateFrom
			                GROUP BY Setup_Customer.CustomerId, Task_Payment.SupplierId
			                HAVING (Task_Payment.SupplierId IN (SELECT SupplierId FROM Setup_Supplier WHERE CompanyId = @CompanyId AND IsActive = 1))

		                END
	                END
	                -- END SUPPLIER

                  RETURN
                END");
            }

            //if function not found then create function
            if (!CheckTable("fn_CalculateOpeningBalanceForPartyLedger"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE FUNCTION fn_CalculateOpeningBalanceForPartyLedger
                (
					@CurrencyLevel int,
                    @CustomerId bigint,
					@SupplierId bigint,
	                @CompanyId bigint,
	                @DateFrom date
                )
                RETURNS numeric(18, 4)
                AS
                BEGIN
	                DECLARE @ClosingBalance numeric(18, 4);
					DECLARE @Amount numeric(18, 4);
					DECLARE @Amount1 numeric(18, 4);
					DECLARE @Amount2 numeric(18, 4);
					DECLARE @OpeningBalance numeric(18, 4);
					DECLARE @OpeningBalance1 numeric(18, 4);
					DECLARE @OpeningBalance2 numeric(18, 4);
	                DECLARE @Customer_OpeningDate date;

	                SET @ClosingBalance = 0;

					-- START CUSTOMER
					IF @CustomerId > 0
					BEGIN
						SELECT @Customer_OpeningDate = OpeningDate, @OpeningBalance = OpeningBalance, @OpeningBalance1 = OpeningBalance1, @OpeningBalance2 = OpeningBalance2 FROM Setup_Customer WHERE CompanyId = @CompanyId AND CustomerId = @CustomerId
						IF @CurrencyLevel = 1
						BEGIN
							SET @ClosingBalance = @ClosingBalance + @OpeningBalance
						END
						ELSE IF @CurrencyLevel = 2
						BEGIN
							SET @ClosingBalance = @ClosingBalance + @OpeningBalance1
						END
						ELSE
						BEGIN
							SET @ClosingBalance = @ClosingBalance + @OpeningBalance2
						END

						IF (@Customer_OpeningDate IS NULL)
						BEGIN
							SELECT @Customer_OpeningDate = OpeningDate FROM Setup_Company WHERE CompanyId = @CompanyId
						END

						IF (@Customer_OpeningDate < @DateFrom)
						BEGIN
							-- FOR SALES (+)
							SET @Amount = 0 
							SET @Amount1 = 0
							SET @Amount2 = 0

							SELECT @Amount = COALESCE(SUM(InvoiceAmount - InvoiceDiscount), 0), @Amount1 = COALESCE(SUM(Invoice1Amount - Invoice1Discount), 0) ,@Amount2 = COALESCE(SUM(Invoice2Amount - Invoice2Discount), 0)
							FROM Task_SalesInvoice WHERE (CustomerId = @CustomerId) AND (CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(InvoiceDate AS date) > @Customer_OpeningDate AND CAST(InvoiceDate AS date) < @DateFrom

							IF @CurrencyLevel = 1
							BEGIN
								SET @ClosingBalance = @ClosingBalance + @Amount
							END
							ELSE IF @CurrencyLevel = 2
							BEGIN
								SET @ClosingBalance = @ClosingBalance + @Amount1
							END
							ELSE
							BEGIN
								SET @ClosingBalance = @ClosingBalance + @Amount2
							END
						END
					END
					-- END CUSTOMER

	                RETURN @ClosingBalance
                END");
            }

            //if function found then alter function
            if (CheckTable("fn_CalculateOpeningBalanceForPartyLedger"))
            {
                _db.Database.ExecuteSqlCommand(@"ALTER FUNCTION fn_CalculateOpeningBalanceForPartyLedger
                (
					@CurrencyLevel int,
                    @CustomerId bigint,
					@SupplierId bigint,
	                @CompanyId bigint,
	                @DateFrom datetime
                )
                RETURNS numeric(18, 4)
                AS
                BEGIN
	                DECLARE @ClosingBalance numeric(18, 4);
					DECLARE @Amount numeric(18, 4);
					DECLARE @Amount1 numeric(18, 4);
					DECLARE @Amount2 numeric(18, 4);
					DECLARE @OpeningBalance numeric(18, 4);
					DECLARE @OpeningBalance1 numeric(18, 4);
					DECLARE @OpeningBalance2 numeric(18, 4);
	                DECLARE @Customer_OpeningDate date;

	                SET @ClosingBalance = 0;

					-- START CUSTOMER
					IF @CustomerId > 0
					BEGIN
						SELECT @Customer_OpeningDate = Setup_Company.OpeningDate, @OpeningBalance = OpeningBalance, @OpeningBalance1 = OpeningBalance1, @OpeningBalance2 = OpeningBalance2 
						FROM Setup_Customer INNER JOIN Setup_Company ON Setup_Customer.CompanyId = Setup_Company.CompanyId
						WHERE Setup_Customer.CompanyId = @CompanyId AND CustomerId = @CustomerId

						IF @CurrencyLevel = 1
						BEGIN
							SET @ClosingBalance = @ClosingBalance + @OpeningBalance
						END
						ELSE IF @CurrencyLevel = 2
						BEGIN
							SET @ClosingBalance = @ClosingBalance + @OpeningBalance1
						END
						ELSE
						BEGIN
							SET @ClosingBalance = @ClosingBalance + @OpeningBalance2
						END

						IF (@Customer_OpeningDate IS NULL)
						BEGIN
							SELECT @Customer_OpeningDate = OpeningDate FROM Setup_Company WHERE CompanyId = @CompanyId
						END

						IF (@Customer_OpeningDate < @DateFrom)
						BEGIN
							-- FOR SALES (+)
							SET @Amount = 0 
							SET @Amount1 = 0
							SET @Amount2 = 0

							SELECT @Amount = COALESCE(SUM(InvoiceAmount - InvoiceDiscount + GovtDutyAmount + TotalChargeAmount), 0), @Amount1 = COALESCE(SUM(Invoice1Amount - Invoice1Discount + GovtDuty1Amount + TotalChargeAmount1), 0) ,@Amount2 = COALESCE(SUM(Invoice2Amount - Invoice2Discount + GovtDuty2Amount + TotalChargeAmount2), 0)
							FROM Task_SalesInvoice WHERE (CustomerId = @CustomerId) AND (CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(InvoiceDate AS date) > @Customer_OpeningDate AND CAST(InvoiceDate AS datetime) < @DateFrom

							IF @CurrencyLevel = 1
							BEGIN
								SET @ClosingBalance = @ClosingBalance + @Amount
							END
							ELSE IF @CurrencyLevel = 2
							BEGIN
								SET @ClosingBalance = @ClosingBalance + @Amount1
							END
							ELSE
							BEGIN
								SET @ClosingBalance = @ClosingBalance + @Amount2
							END

							-- FOR COLLECTION (-)
							SET @Amount = 0 
							SET @Amount1 = 0
							SET @Amount2 = 0

							SELECT @Amount = COALESCE(SUM(CollectedAmount + SecurityDeposit + OthersAdjustment), 0), @Amount1 = COALESCE(SUM(CollectedAmount1 + SecurityDeposit1 + OthersAdjustment1), 0) ,@Amount2 = COALESCE(SUM(CollectedAmount2 + SecurityDeposit2 + OthersAdjustment2), 0)
							FROM Task_Collection WHERE (CustomerId = @CustomerId) AND (CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(CollectionDate AS date) > @Customer_OpeningDate AND CAST(CollectionDate AS datetime) < @DateFrom

							IF @CurrencyLevel = 1
							BEGIN
								SET @ClosingBalance = @ClosingBalance - @Amount
							END
							ELSE IF @CurrencyLevel = 2
							BEGIN
								SET @ClosingBalance = @ClosingBalance - @Amount1
							END
							ELSE
							BEGIN
								SET @ClosingBalance = @ClosingBalance - @Amount2
							END

							-- FOR COLLECTION DUTY ADJUSTMENT (-)
							SET @Amount = 0 
							SET @Amount1 = 0
							SET @Amount2 = 0

							SELECT @Amount = COALESCE(SUM(Task_Collection_GovtDutyAdjustment.Amount), 0), @Amount1 = COALESCE(SUM(Task_Collection_GovtDutyAdjustment.Amount1), 0), @Amount2 = COALESCE(SUM(Task_Collection_GovtDutyAdjustment.Amount2), 0)
							FROM Task_Collection INNER JOIN Task_Collection_GovtDutyAdjustment ON Task_Collection.CollectionId = Task_Collection_GovtDutyAdjustment.CollectionId							
							WHERE (Task_Collection.CustomerId = @CustomerId) AND (Task_Collection.CompanyId = @CompanyId) AND (Task_Collection.Approved = 'A') AND CAST(Task_Collection.CollectionDate AS date) > @Customer_OpeningDate AND CAST(Task_Collection.CollectionDate AS datetime) < @DateFrom

							IF @CurrencyLevel = 1
							BEGIN
								SET @ClosingBalance = @ClosingBalance - @Amount
							END
							ELSE IF @CurrencyLevel = 2
							BEGIN
								SET @ClosingBalance = @ClosingBalance - @Amount1
							END
							ELSE
							BEGIN
								SET @ClosingBalance = @ClosingBalance - @Amount2
							END

							-- FOR SALES RETURN (-)
							SET @Amount = 0 
							SET @Amount1 = 0
							SET @Amount2 = 0

							SELECT @Amount = COALESCE(SUM(ReturnAmount), 0), @Amount1 = COALESCE(SUM(Return1Amount), 0) ,@Amount2 = COALESCE(SUM(Return2Amount), 0)
							FROM Task_SalesReturn WHERE (CustomerId = @CustomerId) AND (CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(ReturnDate AS date) > @Customer_OpeningDate AND CAST(ReturnDate AS datetime) < @DateFrom

							IF @CurrencyLevel = 1
							BEGIN
								SET @ClosingBalance = @ClosingBalance - @Amount
							END
							ELSE IF @CurrencyLevel = 2
							BEGIN
								SET @ClosingBalance = @ClosingBalance - @Amount1
							END
							ELSE
							BEGIN
								SET @ClosingBalance = @ClosingBalance - @Amount2
							END

							-- FOR ADJUSTMENT (+)
							SET @Amount = 0 
							SET @Amount1 = 0
							SET @Amount2 = 0

							SELECT @Amount = COALESCE(SUM(D.AdjustedAmount), 0), @Amount1 = COALESCE(SUM(D.AdjustedAmount1), 0) ,@Amount2 = COALESCE(SUM(D.AdjustedAmount2), 0)
							FROM Task_PartyAdjustmentDetail D INNER JOIN Task_PartyAdjustment A ON D.AdjustmentId = A.AdjustmentId 
							WHERE (A.AdjustmentFor = 'C') AND (A.AdjustmentNature = 'A') AND(CustomerId = @CustomerId) AND (CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(A.AdjustmentDate AS date) > @Customer_OpeningDate AND CAST(A.AdjustmentDate AS datetime) < @DateFrom

							IF @CurrencyLevel = 1
							BEGIN
								SET @ClosingBalance = @ClosingBalance + @Amount
							END
							ELSE IF @CurrencyLevel = 2
							BEGIN
								SET @ClosingBalance = @ClosingBalance + @Amount1
							END
							ELSE
							BEGIN
								SET @ClosingBalance = @ClosingBalance + @Amount2
							END

							-- FOR ADJUSTMENT (-)
							SET @Amount = 0 
							SET @Amount1 = 0
							SET @Amount2 = 0

							SELECT @Amount = COALESCE(SUM(D.AdjustedAmount), 0), @Amount1 = COALESCE(SUM(D.AdjustedAmount1), 0) ,@Amount2 = COALESCE(SUM(D.AdjustedAmount2), 0)
							FROM Task_PartyAdjustmentDetail D INNER JOIN Task_PartyAdjustment A ON D.AdjustmentId = A.AdjustmentId 
							WHERE (A.AdjustmentFor = 'C') AND (A.AdjustmentNature = 'D') AND(CustomerId = @CustomerId) AND (CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(A.AdjustmentDate AS date) > @Customer_OpeningDate AND CAST(A.AdjustmentDate AS datetime) < @DateFrom

							IF @CurrencyLevel = 1
							BEGIN
								SET @ClosingBalance = @ClosingBalance - @Amount
							END
							ELSE IF @CurrencyLevel = 2
							BEGIN
								SET @ClosingBalance = @ClosingBalance - @Amount1
							END
							ELSE
							BEGIN
								SET @ClosingBalance = @ClosingBalance - @Amount2
							END

                            -- Balance Adjusted (+) From Cheque Treatment
							SET @Amount = 0 
							SET @Amount1 = 0
							SET @Amount2 = 0

							SELECT @Amount = COALESCE(SUM(Task_ChequeInfo.Amount), 0), @Amount1 = COALESCE(SUM(Task_ChequeInfo.Amount1), 0) ,@Amount2 = COALESCE(SUM(Task_ChequeInfo.Amount2), 0)
							FROM Task_ChequeInfo INNER JOIN Task_CollectionDetail ON Task_ChequeInfo.CollectionDetailId = Task_CollectionDetail.CollectionDetailId 
							INNER JOIN Task_Collection ON Task_CollectionDetail.CollectionId = Task_Collection.CollectionId 
							INNER JOIN Task_ChequeTreatment ON Task_ChequeInfo.ChequeInfoId = Task_ChequeTreatment.ChequeInfoId AND Task_ChequeInfo.Status = Task_ChequeTreatment.Status 
							INNER JOIN Task_Voucher ON Task_ChequeTreatment.VoucherId = Task_Voucher.VoucherId
							WHERE (Task_ChequeInfo.Status = 'B') AND(Task_Collection.CustomerId = @CustomerId) AND (Task_ChequeInfo.CompanyId = @CompanyId) AND (Task_Collection.Approved = 'A') AND CAST(Task_ChequeInfo.StatusDate AS date) > @Customer_OpeningDate AND CAST(Task_ChequeInfo.StatusDate AS datetime) < @DateFrom

							IF @CurrencyLevel = 1
							BEGIN
								SET @ClosingBalance = @ClosingBalance + @Amount
							END
							ELSE IF @CurrencyLevel = 2
							BEGIN
								SET @ClosingBalance = @ClosingBalance + @Amount1
							END
							ELSE
							BEGIN
								SET @ClosingBalance = @ClosingBalance + @Amount2
							END

						END
					END
					-- END CUSTOMER

					-- START SUPPLIER
					IF @SupplierId > 0
					BEGIN
						DECLARE @SupplierOpening numeric(18, 4);

						SET @SupplierOpening = 0;

						SELECT @SupplierOpening = dbo.fn_CalculateOpeningBalanceForSupplierLedger(@CurrencyLevel, @SupplierId, @CompanyId, @DateFrom)

						SET @ClosingBalance = @ClosingBalance - @SupplierOpening
					END
					-- END SUPPLIER

	                RETURN @ClosingBalance
                END");
            }

            //if function not found then create function
            if (!CheckTable("fn_CalculateOpeningBalanceForPartyLedger_V2"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE FUNCTION fn_CalculateOpeningBalanceForPartyLedger_V2
                (
	                @For varchar(1),
                    @CustomerId bigint,
	                @CompanyId bigint,
	                @DateFrom datetime
                )
                RETURNS @T TABLE(
	                [CustomerId] [bigint] NOT NULL,
	                [For] [varchar](1) NOT NULL,
	                [TransactionFrom] [varchar](50) NOT NULL,
	                [Amount] [numeric](18, 4) NOT NULL,
	                [Amount1] [numeric](18, 4) NOT NULL,
	                [Amount2] [numeric](18, 4) NOT NULL)
                AS
                BEGIN
	                DECLARE @Customer_OpeningDate date;

	                -- START CUSTOMER
	                SELECT @Customer_OpeningDate = OpeningDate FROM Setup_Company WHERE CompanyId = @CompanyId

		            -- FOR OPENING BALANCE (AS USUAL)
		            INSERT INTO @T (CustomerId, [For], TransactionFrom, Amount, Amount1, Amount2)
		            SELECT Setup_Customer.CustomerId, @For, 'COB', OpeningBalance, OpeningBalance1, OpeningBalance2
		            FROM Setup_Customer INNER JOIN Setup_Company ON Setup_Customer.CompanyId = Setup_Company.CompanyId
		            WHERE Setup_Customer.CompanyId = @CompanyId 
		            AND CustomerId IN (SELECT CustomerId FROM Setup_Customer WHERE CompanyId = @CompanyId AND IsActive = 1 AND CASE WHEN @CustomerId = 0 THEN 1 WHEN CustomerId = @CustomerId THEN 1 ELSE 0 END = 1)

		            IF (@Customer_OpeningDate < @DateFrom)
		            BEGIN
			            -- FOR SALES (+)
			            INSERT INTO @T (CustomerId, [For], TransactionFrom, Amount, Amount1, Amount2)
			            SELECT CustomerId, @For, 'Sales', COALESCE(SUM(InvoiceAmount - InvoiceDiscount + GovtDutyAmount), 0), COALESCE(SUM(Invoice1Amount - Invoice1Discount + GovtDuty1Amount), 0), COALESCE(SUM(Invoice2Amount - Invoice2Discount + GovtDuty2Amount), 0) 
			            FROM Task_SalesInvoice 
			            WHERE (CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(InvoiceDate AS date) > @Customer_OpeningDate AND CAST(InvoiceDate AS datetime) < @DateFrom
			            GROUP BY CustomerId
			            HAVING (CustomerId IN (SELECT CustomerId FROM Setup_Customer WHERE CompanyId = @CompanyId AND IsActive = 1 AND  CASE WHEN @CustomerId = 0 THEN 1 WHEN CustomerId = @CustomerId THEN 1 ELSE 0 END = 1))

			            -- FOR COLLECTION (-)
			            INSERT INTO @T (CustomerId, [For], TransactionFrom, Amount, Amount1, Amount2)
			            SELECT CustomerId, @For, 'Collection', COALESCE(SUM(CollectedAmount + SecurityDeposit + OthersAdjustment), 0) * -1, COALESCE(SUM(CollectedAmount1 + SecurityDeposit1 + OthersAdjustment1), 0) * -1, COALESCE(SUM(CollectedAmount2 + SecurityDeposit2 + OthersAdjustment2), 0) * -1 
			            FROM Task_Collection 
			            WHERE (CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(CollectionDate AS date) > @Customer_OpeningDate AND CAST(CollectionDate AS datetime) < @DateFrom
			            GROUP BY CustomerId
			            HAVING (CustomerId IN (SELECT CustomerId FROM Setup_Customer WHERE CompanyId = @CompanyId AND IsActive = 1 AND  CASE WHEN @CustomerId = 0 THEN 1 WHEN CustomerId = @CustomerId THEN 1 ELSE 0 END = 1))

			            -- FOR COLLECTION DUTY ADJUSTMENT (-)
			            INSERT INTO @T (CustomerId, [For], TransactionFrom, Amount, Amount1, Amount2)
			            SELECT Task_Collection.CustomerId, @For, 'CollectionDutyAdjNegative', COALESCE(SUM(Task_Collection_GovtDutyAdjustment.Amount), 0) * -1,  COALESCE(SUM(Task_Collection_GovtDutyAdjustment.Amount1), 0) * -1, COALESCE(SUM(Task_Collection_GovtDutyAdjustment.Amount2), 0) * -1
			            FROM Task_Collection INNER JOIN Task_Collection_GovtDutyAdjustment ON Task_Collection.CollectionId = Task_Collection_GovtDutyAdjustment.CollectionId							
			            WHERE (Task_Collection.CompanyId = @CompanyId) AND (Task_Collection.Approved = 'A') AND CAST(Task_Collection.CollectionDate AS date) > @Customer_OpeningDate AND CAST(Task_Collection.CollectionDate AS datetime) < @DateFrom
			            GROUP BY CustomerId
			            HAVING ((Task_Collection.CustomerId IN (SELECT CustomerId FROM Setup_Customer WHERE CompanyId = @CompanyId AND IsActive = 1 AND  CASE WHEN @CustomerId = 0 THEN 1 WHEN CustomerId = @CustomerId THEN 1 ELSE 0 END = 1)))

			            -- FOR SALES RETURN (-)
			            INSERT INTO @T (CustomerId, [For], TransactionFrom, Amount, Amount1, Amount2)
			            SELECT CustomerId, @For, 'SalesReturn', COALESCE(SUM(ReturnAmount), 0) * -1, COALESCE(SUM(Return1Amount), 0) * -1, COALESCE(SUM(Return2Amount), 0) * -1
			            FROM Task_SalesReturn 
			            WHERE (CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(ReturnDate AS date) > @Customer_OpeningDate AND CAST(ReturnDate AS datetime) < @DateFrom
			            GROUP BY CustomerId
			            HAVING (CustomerId IN (SELECT CustomerId FROM Setup_Customer WHERE CompanyId = @CompanyId AND IsActive = 1 AND  CASE WHEN @CustomerId = 0 THEN 1 WHEN CustomerId = @CustomerId THEN 1 ELSE 0 END = 1))

			            -- FOR ADJUSTMENT (+)
			            INSERT INTO @T (CustomerId, [For], TransactionFrom, Amount, Amount1, Amount2)
			            SELECT CustomerId, @For, 'PartyAdj', COALESCE(SUM(D.AdjustedAmount), 0), COALESCE(SUM(D.AdjustedAmount1), 0), COALESCE(SUM(D.AdjustedAmount2), 0)
			            FROM Task_PartyAdjustmentDetail D INNER JOIN Task_PartyAdjustment A ON D.AdjustmentId = A.AdjustmentId 
			            WHERE (A.AdjustmentFor = 'C') AND (A.AdjustmentNature = 'A') AND (CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(A.AdjustmentDate AS date) > @Customer_OpeningDate AND CAST(A.AdjustmentDate AS datetime) < @DateFrom
			            GROUP BY CustomerId
			            HAVING (CustomerId IN (SELECT CustomerId FROM Setup_Customer WHERE CompanyId = @CompanyId AND IsActive = 1 AND  CASE WHEN @CustomerId = 0 THEN 1 WHEN CustomerId = @CustomerId THEN 1 ELSE 0 END = 1))

			            -- FOR ADJUSTMENT (-)
			            INSERT INTO @T (CustomerId, [For], TransactionFrom, Amount, Amount1, Amount2)
			            SELECT CustomerId, @For, 'PartyAdjNegative', COALESCE(SUM(D.AdjustedAmount), 0) * -1, COALESCE(SUM(D.AdjustedAmount1), 0) * -1, COALESCE(SUM(D.AdjustedAmount2), 0) * -1
			            FROM Task_PartyAdjustmentDetail D INNER JOIN Task_PartyAdjustment A ON D.AdjustmentId = A.AdjustmentId 
			            WHERE (A.AdjustmentFor = 'C') AND (A.AdjustmentNature = 'D') AND (CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(A.AdjustmentDate AS date) > @Customer_OpeningDate AND CAST(A.AdjustmentDate AS datetime) < @DateFrom
			            GROUP BY CustomerId
			            HAVING ((CustomerId IN (SELECT CustomerId FROM Setup_Customer WHERE CompanyId = @CompanyId AND IsActive = 1 AND  CASE WHEN @CustomerId = 0 THEN 1 WHEN CustomerId = @CustomerId THEN 1 ELSE 0 END = 1)))

                        -- Balance Adjusted (+) From Cheque Treatment
			            INSERT INTO @T (CustomerId, [For], TransactionFrom, Amount, Amount1, Amount2)
			            SELECT Task_Collection.CustomerId, @For, 'AdjFromChequeTreat', COALESCE(SUM(Task_ChequeInfo.Amount), 0), COALESCE(SUM(Task_ChequeInfo.Amount1), 0), COALESCE(SUM(Task_ChequeInfo.Amount2), 0)
			            FROM Task_ChequeInfo INNER JOIN Task_CollectionDetail ON Task_ChequeInfo.CollectionDetailId = Task_CollectionDetail.CollectionDetailId 
			            INNER JOIN Task_Collection ON Task_CollectionDetail.CollectionId = Task_Collection.CollectionId 
			            INNER JOIN Task_ChequeTreatment ON Task_ChequeInfo.ChequeInfoId = Task_ChequeTreatment.ChequeInfoId AND Task_ChequeInfo.Status = Task_ChequeTreatment.Status 
			            INNER JOIN Task_Voucher ON Task_ChequeTreatment.VoucherId = Task_Voucher.VoucherId
			            WHERE (Task_ChequeInfo.Status = 'B') AND (Task_ChequeInfo.CompanyId = @CompanyId) AND (Task_Collection.Approved = 'A') AND CAST(Task_ChequeInfo.StatusDate AS date) > @Customer_OpeningDate AND CAST(Task_ChequeInfo.StatusDate AS datetime) < @DateFrom
			            GROUP BY CustomerId
			            HAVING ((Task_Collection.CustomerId IN (SELECT CustomerId FROM Setup_Customer WHERE CompanyId = @CompanyId AND IsActive = 1 AND  CASE WHEN @CustomerId = 0 THEN 1 WHEN CustomerId = @CustomerId THEN 1 ELSE 0 END = 1)))

		            END
	                -- END CUSTOMER

	                -- START SUPPLIER
					INSERT INTO @T (CustomerId, [For], TransactionFrom, Amount, Amount1, Amount2)
		            SELECT CustomerId, @For, 'SOB', COALESCE(SUM(Amount), 0) * -1, COALESCE(SUM(Amount1), 0) * -1, COALESCE(SUM(Amount2), 0) * -1 
					FROM [dbo].[fn_CalculateOpeningBalanceForSupplierLedger_V2] ('A', @CompanyId, @DateFrom) AS fn_SupplierLedger
		            WHERE CustomerId IS NOT NULL
		            GROUP BY CustomerId
		            HAVING ((fn_SupplierLedger.CustomerId IN (SELECT CustomerId FROM Setup_Customer WHERE CompanyId = @CompanyId AND IsActive = 1 AND  CASE WHEN @CustomerId = 0 THEN 1 WHEN CustomerId = @CustomerId THEN 1 ELSE 0 END = 1)))
	                -- END SUPPLIER

                  RETURN
                END");
            }

            //if function found then alter function
            if (CheckTable("fn_CalculateOpeningBalanceForPartyLedger_V2"))
            {
                _db.Database.ExecuteSqlCommand(@"ALTER FUNCTION fn_CalculateOpeningBalanceForPartyLedger_V2
                (
	                @For varchar(1),
                    @CustomerId bigint,
	                @CompanyId bigint,
	                @DateFrom datetime
                )
                RETURNS @T TABLE(
	                [CustomerId] [bigint] NOT NULL,
	                [For] [varchar](1) NOT NULL,
	                [TransactionFrom] [varchar](50) NOT NULL,
	                [Amount] [numeric](18, 4) NOT NULL,
	                [Amount1] [numeric](18, 4) NOT NULL,
	                [Amount2] [numeric](18, 4) NOT NULL)
                AS
                BEGIN
	                DECLARE @Customer_OpeningDate date;

	                -- START CUSTOMER
	                SELECT @Customer_OpeningDate = OpeningDate FROM Setup_Company WHERE CompanyId = @CompanyId

		            -- FOR OPENING BALANCE (AS USUAL)
		            INSERT INTO @T (CustomerId, [For], TransactionFrom, Amount, Amount1, Amount2)
		            SELECT Setup_Customer.CustomerId, @For, 'COB', OpeningBalance, OpeningBalance1, OpeningBalance2
		            FROM Setup_Customer INNER JOIN Setup_Company ON Setup_Customer.CompanyId = Setup_Company.CompanyId
		            WHERE Setup_Customer.CompanyId = @CompanyId 
		            AND CustomerId IN (SELECT CustomerId FROM Setup_Customer WHERE CompanyId = @CompanyId AND IsActive = 1 AND CASE WHEN @CustomerId = 0 THEN 1 WHEN CustomerId = @CustomerId THEN 1 ELSE 0 END = 1)

		            IF (@Customer_OpeningDate < @DateFrom)
		            BEGIN
			            -- FOR SALES (+)
			            INSERT INTO @T (CustomerId, [For], TransactionFrom, Amount, Amount1, Amount2)
			            SELECT CustomerId, @For, 'Sales', COALESCE(SUM(InvoiceAmount - InvoiceDiscount + GovtDutyAmount), 0), COALESCE(SUM(Invoice1Amount - Invoice1Discount + GovtDuty1Amount), 0), COALESCE(SUM(Invoice2Amount - Invoice2Discount + GovtDuty2Amount), 0) 
			            FROM Task_SalesInvoice 
			            WHERE (CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(InvoiceDate AS date) > @Customer_OpeningDate AND CAST(InvoiceDate AS datetime) < @DateFrom
			            GROUP BY CustomerId
			            HAVING (CustomerId IN (SELECT CustomerId FROM Setup_Customer WHERE CompanyId = @CompanyId AND IsActive = 1 AND  CASE WHEN @CustomerId = 0 THEN 1 WHEN CustomerId = @CustomerId THEN 1 ELSE 0 END = 1))

			            -- FOR COLLECTION (-)
			            INSERT INTO @T (CustomerId, [For], TransactionFrom, Amount, Amount1, Amount2)
			            SELECT CustomerId, @For, 'Collection', COALESCE(SUM(CollectedAmount + SecurityDeposit + OthersAdjustment), 0) * -1, COALESCE(SUM(CollectedAmount1 + SecurityDeposit1 + OthersAdjustment1), 0) * -1, COALESCE(SUM(CollectedAmount2 + SecurityDeposit2 + OthersAdjustment2), 0) * -1 
			            FROM Task_Collection 
			            WHERE (CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(CollectionDate AS date) > @Customer_OpeningDate AND CAST(CollectionDate AS datetime) < @DateFrom
			            GROUP BY CustomerId
			            HAVING (CustomerId IN (SELECT CustomerId FROM Setup_Customer WHERE CompanyId = @CompanyId AND IsActive = 1 AND  CASE WHEN @CustomerId = 0 THEN 1 WHEN CustomerId = @CustomerId THEN 1 ELSE 0 END = 1))

			            -- FOR COLLECTION DUTY ADJUSTMENT (-)
			            INSERT INTO @T (CustomerId, [For], TransactionFrom, Amount, Amount1, Amount2)
			            SELECT Task_Collection.CustomerId, @For, 'CollectionDutyAdjNegative', COALESCE(SUM(Task_Collection_GovtDutyAdjustment.Amount), 0) * -1,  COALESCE(SUM(Task_Collection_GovtDutyAdjustment.Amount1), 0) * -1, COALESCE(SUM(Task_Collection_GovtDutyAdjustment.Amount2), 0) * -1
			            FROM Task_Collection INNER JOIN Task_Collection_GovtDutyAdjustment ON Task_Collection.CollectionId = Task_Collection_GovtDutyAdjustment.CollectionId							
			            WHERE (Task_Collection.CompanyId = @CompanyId) AND (Task_Collection.Approved = 'A') AND CAST(Task_Collection.CollectionDate AS date) > @Customer_OpeningDate AND CAST(Task_Collection.CollectionDate AS datetime) < @DateFrom
			            GROUP BY CustomerId
			            HAVING ((Task_Collection.CustomerId IN (SELECT CustomerId FROM Setup_Customer WHERE CompanyId = @CompanyId AND IsActive = 1 AND  CASE WHEN @CustomerId = 0 THEN 1 WHEN CustomerId = @CustomerId THEN 1 ELSE 0 END = 1)))

			            -- FOR SALES RETURN (-)
			            INSERT INTO @T (CustomerId, [For], TransactionFrom, Amount, Amount1, Amount2)
			            SELECT CustomerId, @For, 'SalesReturn', COALESCE(SUM(ReturnAmount), 0) * -1, COALESCE(SUM(Return1Amount), 0) * -1, COALESCE(SUM(Return2Amount), 0) * -1
			            FROM Task_SalesReturn 
			            WHERE (CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(ReturnDate AS date) > @Customer_OpeningDate AND CAST(ReturnDate AS datetime) < @DateFrom
			            GROUP BY CustomerId
			            HAVING (CustomerId IN (SELECT CustomerId FROM Setup_Customer WHERE CompanyId = @CompanyId AND IsActive = 1 AND  CASE WHEN @CustomerId = 0 THEN 1 WHEN CustomerId = @CustomerId THEN 1 ELSE 0 END = 1))

			            -- FOR ADJUSTMENT (+)
			            INSERT INTO @T (CustomerId, [For], TransactionFrom, Amount, Amount1, Amount2)
			            SELECT CustomerId, @For, 'PartyAdj', COALESCE(SUM(D.AdjustedAmount), 0), COALESCE(SUM(D.AdjustedAmount1), 0), COALESCE(SUM(D.AdjustedAmount2), 0)
			            FROM Task_PartyAdjustmentDetail D INNER JOIN Task_PartyAdjustment A ON D.AdjustmentId = A.AdjustmentId 
			            WHERE (A.AdjustmentFor = 'C') AND (A.AdjustmentNature = 'A') AND (CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(A.AdjustmentDate AS date) > @Customer_OpeningDate AND CAST(A.AdjustmentDate AS datetime) < @DateFrom
			            GROUP BY CustomerId
			            HAVING (CustomerId IN (SELECT CustomerId FROM Setup_Customer WHERE CompanyId = @CompanyId AND IsActive = 1 AND  CASE WHEN @CustomerId = 0 THEN 1 WHEN CustomerId = @CustomerId THEN 1 ELSE 0 END = 1))

			            -- FOR ADJUSTMENT (-)
			            INSERT INTO @T (CustomerId, [For], TransactionFrom, Amount, Amount1, Amount2)
			            SELECT CustomerId, @For, 'PartyAdjNegative', COALESCE(SUM(D.AdjustedAmount), 0) * -1, COALESCE(SUM(D.AdjustedAmount1), 0) * -1, COALESCE(SUM(D.AdjustedAmount2), 0) * -1
			            FROM Task_PartyAdjustmentDetail D INNER JOIN Task_PartyAdjustment A ON D.AdjustmentId = A.AdjustmentId 
			            WHERE (A.AdjustmentFor = 'C') AND (A.AdjustmentNature = 'D') AND (CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(A.AdjustmentDate AS date) > @Customer_OpeningDate AND CAST(A.AdjustmentDate AS datetime) < @DateFrom
			            GROUP BY CustomerId
			            HAVING ((CustomerId IN (SELECT CustomerId FROM Setup_Customer WHERE CompanyId = @CompanyId AND IsActive = 1 AND  CASE WHEN @CustomerId = 0 THEN 1 WHEN CustomerId = @CustomerId THEN 1 ELSE 0 END = 1)))

                        -- Balance Adjusted (+) From Cheque Treatment
			            INSERT INTO @T (CustomerId, [For], TransactionFrom, Amount, Amount1, Amount2)
			            SELECT Task_Collection.CustomerId, @For, 'AdjFromChequeTreat', COALESCE(SUM(Task_ChequeInfo.Amount), 0), COALESCE(SUM(Task_ChequeInfo.Amount1), 0), COALESCE(SUM(Task_ChequeInfo.Amount2), 0)
			            FROM Task_ChequeInfo INNER JOIN Task_CollectionDetail ON Task_ChequeInfo.CollectionDetailId = Task_CollectionDetail.CollectionDetailId 
			            INNER JOIN Task_Collection ON Task_CollectionDetail.CollectionId = Task_Collection.CollectionId 
			            INNER JOIN Task_ChequeTreatment ON Task_ChequeInfo.ChequeInfoId = Task_ChequeTreatment.ChequeInfoId AND Task_ChequeInfo.Status = Task_ChequeTreatment.Status 
			            INNER JOIN Task_Voucher ON Task_ChequeTreatment.VoucherId = Task_Voucher.VoucherId
			            WHERE (Task_ChequeInfo.Status = 'B') AND (Task_ChequeInfo.CompanyId = @CompanyId) AND (Task_Collection.Approved = 'A') AND CAST(Task_ChequeInfo.StatusDate AS date) > @Customer_OpeningDate AND CAST(Task_ChequeInfo.StatusDate AS datetime) < @DateFrom
			            GROUP BY CustomerId
			            HAVING ((Task_Collection.CustomerId IN (SELECT CustomerId FROM Setup_Customer WHERE CompanyId = @CompanyId AND IsActive = 1 AND  CASE WHEN @CustomerId = 0 THEN 1 WHEN CustomerId = @CustomerId THEN 1 ELSE 0 END = 1)))

		            END
	                -- END CUSTOMER

	                -- START SUPPLIER
					INSERT INTO @T (CustomerId, [For], TransactionFrom, Amount, Amount1, Amount2)
		            SELECT CustomerId, @For, 'SOB', COALESCE(SUM(Amount), 0) * -1, COALESCE(SUM(Amount1), 0) * -1, COALESCE(SUM(Amount2), 0) * -1 
					FROM [dbo].[fn_CalculateOpeningBalanceForSupplierLedger_V2] ('A', @CompanyId, @DateFrom) AS fn_SupplierLedger
		            WHERE CustomerId IS NOT NULL
		            GROUP BY CustomerId
		            HAVING ((fn_SupplierLedger.CustomerId IN (SELECT CustomerId FROM Setup_Customer WHERE CompanyId = @CompanyId AND IsActive = 1 AND  CASE WHEN @CustomerId = 0 THEN 1 WHEN CustomerId = @CustomerId THEN 1 ELSE 0 END = 1)))
	                -- END SUPPLIER

                  RETURN
                END");
            }

            //if function not found then create function
            if (!CheckTable("fn_CompanyFeatureValue"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE FUNCTION fn_CompanyFeatureValue
                (
	                @FeatureName varchar(50),
                    @CompanyId bigint
                )
                RETURNS bit
                AS
                BEGIN
	                DECLARE @DefaultValue bit
	                DECLARE @ConfiguredValue bit

	                SELECT @DefaultValue = DefaultValue FROM Setup_Features WHERE FeatureName = @FeatureName

	                DECLARE @RowCount int
	                SELECT @RowCount = COUNT(Configuration_Features.Value) FROM Configuration_Features INNER JOIN Setup_Features ON Configuration_Features.FeatureId = Setup_Features.FeatureId
	                WHERE (Configuration_Features.CompanyId = @CompanyId) AND (Configuration_Features.SubFeatureId IS NULL) AND (Setup_Features.FeatureName = @FeatureName)

	                IF @RowCount > 0
	                BEGIN
		                SELECT @ConfiguredValue = Configuration_Features.Value FROM Configuration_Features INNER JOIN Setup_Features ON Configuration_Features.FeatureId = Setup_Features.FeatureId
		                WHERE (Configuration_Features.CompanyId = @CompanyId) AND (Configuration_Features.SubFeatureId IS NULL) AND (Setup_Features.FeatureName = @FeatureName)

		                SET @DefaultValue = @ConfiguredValue
	                END

	                RETURN @DefaultValue
                END");
            }

            //end CreateOrAlterFunction method
        }

        private void CreateOrAlterStoredProcedure()
        {
            //if stored procedure not found then create stored procedure
            if (!CheckTable("SP_AccountsLedger"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE PROCEDURE SP_AccountsLedger
                @CompanyId bigint,
                @DateFrom date,
                @DateTo date,
                @AccGroupId bigint,
                @AccSubGroupId bigint,
                @AccControlId bigint,
                @AccSubsidiaryId bigint,
                @AccId bigint,
                @EntryBy bigint,
                @ReportType varchar
                AS
                BEGIN
	                --@ReportType will be (L/P)
	                --L = Ledger Report 
	                --P = Provisional Ledger Report
	                DECLARE @FETCH_STATUS_Accounts int;
	                DECLARE @FETCH_STATUS_PostedVoucher int;
	                DECLARE @Accounts_AccId bigint;
					DECLARE @Accounts_AccName varchar(200);
					DECLARE @Acc_BalanceType varchar(2);
					DECLARE @Acc_OpeningDate datetime;
					DECLARE @Acc_Opening numeric(18, 4);
	                DECLARE @OpeningBalanceAmount numeric(18, 4);
	                DECLARE @PostedVoucher_Amount numeric(18, 4);	                        
	                DECLARE @Dr_Amount numeric(18, 4);
	                DECLARE @Cr_Amount numeric(18, 4);
	                DECLARE @PostedVoucher_Date datetime;
	                DECLARE @PostedVoucher_VoucherNo varchar(50);
	                DECLARE @PostedVoucher_Description varchar(1500);

					SET @OpeningBalanceAmount = 0
							
					DELETE FROM Temp_AccountsLedgerOrProvisionalLedger WHERE LedgerOrProvisional = @ReportType AND CompanyId = @CompanyId AND EntryBy = @EntryBy

	                SELECT Setup_Accounts.*, Setup_AccountsControl.AccountsControlId, Setup_AccountsSubGroup.AccountsSubGroupId, Setup_AccountsGroup.AccountsGroupId INTO #TempAccountsTable FROM Setup_Accounts
					INNER JOIN Setup_AccountsSubsidiary ON Setup_Accounts.AccountsSubsidiaryId = Setup_AccountsSubsidiary.AccountsSubsidiaryId
					INNER JOIN Setup_AccountsControl ON Setup_AccountsSubsidiary.AccountsControlId = Setup_AccountsControl.AccountsControlId
					INNER JOIN Setup_AccountsSubGroup ON Setup_AccountsControl.AccountsSubGroupId = Setup_AccountsSubGroup.AccountsSubGroupId
					INNER JOIN Setup_AccountsGroup ON Setup_AccountsSubGroup.AccountsGroupId = Setup_AccountsGroup.AccountsGroupId 
					WHERE Setup_Accounts.CompanyId = @CompanyId

	                IF (@AccGroupId > 0)
	                BEGIN
		                DELETE FROM #TempAccountsTable WHERE AccountsGroupId <> @AccGroupId
	                END

	                IF (@AccSubGroupId > 0)
	                BEGIN
		                DELETE FROM #TempAccountsTable WHERE AccountsSubGroupId <> @AccSubGroupId
	                END

	                IF (@AccControlId > 0)
	                BEGIN
		                DELETE FROM #TempAccountsTable WHERE AccountsControlId <> @AccControlId
	                END

	                IF (@AccSubsidiaryId > 0)
	                BEGIN
		                DELETE FROM #TempAccountsTable WHERE AccountsSubsidiaryId <> @AccSubsidiaryId
	                END

	                IF (@AccId > 0)
	                BEGIN
		                DELETE FROM #TempAccountsTable WHERE AccountsId <> @AccId
	                END

		            --START CURSOR FOR Accounts
		            DECLARE CursorAccounts CURSOR FOR
		            SELECT AccountsId, Name, OpeningDate, BalanceType FROM #TempAccountsTable Order By AccountsId

		            OPEN CursorAccounts
		            FETCH NEXT FROM CursorAccounts INTO @Accounts_AccId, @Accounts_AccName, @Acc_OpeningDate, @Acc_BalanceType
		            SET @FETCH_STATUS_Accounts = @@FETCH_STATUS
		            WHILE @FETCH_STATUS_Accounts = 0
		            BEGIN
						SET @Acc_Opening = 0
			            SELECT @Acc_Opening = dbo.fn_CalculateOpeningBalanceForAccounts(@Accounts_AccId, @CompanyId, @DateFrom)
						SET @OpeningBalanceAmount = @OpeningBalanceAmount + @Acc_Opening

			            IF(@ReportType = 'L')
			            BEGIN
				            --insert voucher data to temp table
				            INSERT INTO Temp_AccountsLedgerOrProvisionalLedger (LedgerOrProvisional, AccountsId, AccountsName, Date, VoucherNo, VoucherDescription, DrAmount, CrAmount, CompanyId, EntryBy)
				            SELECT @ReportType, @Accounts_AccId, @Accounts_AccName, V.Date, V.VoucherNo, VD.Particulars, VD.Debit, VD.Credit, @CompanyId, @EntryBy
							FROM Task_Voucher V INNER JOIN Task_VoucherDetail VD ON V.VoucherId = VD.VoucherId
				            WHERE V.Approved = 'A' AND VD.AccountsId = @Accounts_AccId AND V.CompanyId = @CompanyId AND CAST(Date AS date) >= @DateFrom AND CAST(Date AS date) <= @DateTo Order By Date
			            END
			            ELSE IF (@ReportType = 'P')
			            BEGIN
				            --start cursor for CursorPostedVoucher
				            DECLARE CursorPostedVoucher CURSOR FOR
				            SELECT P.Amount, P.Date, V.VoucherNo, COALESCE(VD.Particulars, '') AS Particulars FROM Task_PostedVoucher P 
							INNER JOIN Task_VoucherDetail AS VD ON P.VoucherDetailId = VD.VoucherDetailId
				            INNER JOIN Task_Voucher V ON VD.VoucherId = V.VoucherId									
				            WHERE P.AccountsId = @Accounts_AccId AND V.CompanyId = @CompanyId AND V.Approved = 'A' AND V.Posted = 1 AND CAST(P.Date AS date) >= @DateFrom AND CAST(P.Date AS date) <= @DateTo 
							Order By P.Date

				            OPEN CursorPostedVoucher
				            FETCH NEXT FROM CursorPostedVoucher INTO @PostedVoucher_Amount, @PostedVoucher_Date, @PostedVoucher_VoucherNo, @PostedVoucher_Description
				            SET @FETCH_STATUS_PostedVoucher = @@FETCH_STATUS
				            WHILE @FETCH_STATUS_PostedVoucher = 0
				            BEGIN

					            IF(@Acc_BalanceType = 'Dr' and @PostedVoucher_Amount > 0)
					            BEGIN
						            SET @Dr_Amount = ABS(@PostedVoucher_Amount)
						            SET @Cr_Amount = 0
					            END
					            ELSE IF(@Acc_BalanceType = 'Dr' and @PostedVoucher_Amount < 0)
					            BEGIN
						            SET @Dr_Amount = 0
						            SET @Cr_Amount = ABS(@PostedVoucher_Amount)
					            END
					            ELSE IF(@Acc_BalanceType = 'Cr' and @PostedVoucher_Amount > 0)
					            BEGIN
						            SET @Dr_Amount = 0
						            SET @Cr_Amount = ABS(@PostedVoucher_Amount)
					            END
					            ELSE IF(@Acc_BalanceType = 'Cr' and @PostedVoucher_Amount < 0)
					            BEGIN
						            SET @Dr_Amount = ABS(@PostedVoucher_Amount)
						            SET @Cr_Amount = 0
					            END

					            INSERT INTO Temp_AccountsLedgerOrProvisionalLedger (LedgerOrProvisional, AccountsId, AccountsName, Date, VoucherNo, VoucherDescription, DrAmount, CrAmount, CompanyId, EntryBy)
					            VALUES (@ReportType, @Accounts_AccId, @Accounts_AccName, @PostedVoucher_Date, @PostedVoucher_VoucherNo, @PostedVoucher_Description, @Dr_Amount, @Cr_Amount, @CompanyId, @EntryBy)

				            FETCH NEXT FROM CursorPostedVoucher INTO @PostedVoucher_Amount, @PostedVoucher_Date, @PostedVoucher_VoucherNo, @PostedVoucher_Description
				            SET @FETCH_STATUS_PostedVoucher = @@FETCH_STATUS
				            END

				            CLOSE CursorPostedVoucher
				            DEALLOCATE CursorPostedVoucher
				            --end cursor for CursorPostedVoucher
			            END

		            FETCH NEXT FROM CursorAccounts INTO @Accounts_AccId, @Accounts_AccName, @Acc_OpeningDate, @Acc_BalanceType
		            SET @FETCH_STATUS_Accounts = @@FETCH_STATUS
		            END

		            CLOSE CursorAccounts
		            DEALLOCATE CursorAccounts
		            --END CURSOR FOR Accounts

					--insert opening balance info to temp table
			        INSERT INTO Temp_AccountsLedgerOrProvisionalLedger (LedgerOrProvisional, Date, VoucherNo, DrAmount, CrAmount, CompanyId, EntryBy) VALUES
			        (@ReportType, @DateFrom, 'Opening Balance', @OpeningBalanceAmount, 0, @CompanyId, @EntryBy)

                END");
            }

            //       //if stored procedure found then alter stored procedure
            //       if (CheckTable("SP_AccountsLedger"))
            //       {
            //           _db.Database.ExecuteSqlCommand(@"ALTER PROCEDURE SP_AccountsLedger
            //           @Currency varchar(5),
            //           @CompanyId bigint,
            //           @DateFrom date,
            //           @DateTo date,
            //           @AccGroupId bigint,
            //           @AccSubGroupId bigint,
            //           @AccControlId bigint,
            //           @AccSubsidiaryId bigint,
            //           @AccId bigint,
            //           @EntryBy bigint,
            //           @ReportType varchar
            //           AS
            //           BEGIN
            //            --@ReportType will be (L/P)
            //            --L = Ledger Report 
            //            --P = Provisional Ledger Report
            //DECLARE @CurrencyLevel int;
            //            DECLARE @FETCH_STATUS_Accounts int;
            //            DECLARE @FETCH_STATUS_PostedVoucher int;
            //            DECLARE @Accounts_AccId bigint;
            //DECLARE @Accounts_AccName varchar(200);
            //DECLARE @Group_BalanceType varchar(2);
            //DECLARE @Acc_BalanceType varchar(2);
            //DECLARE @Acc_Opening numeric(18, 4);
            //            DECLARE @OpeningBalanceAmount numeric(18, 4);
            //DECLARE @Dr_OpeningBalanceAmount numeric(18, 4);
            //            DECLARE @Cr_OpeningBalanceAmount numeric(18, 4);
            //DECLARE @LedgerVoucher_Id uniqueidentifier;
            //DECLARE @Temp_Id uniqueidentifier;
            //DECLARE @LedgerVoucher_No varchar(50);
            //DECLARE @LedgerVoucher_Date datetime;
            //DECLARE @VoucherDetailParticular varchar(1000);
            //DECLARE @VoucherDetail_Dr numeric(18, 4);
            //DECLARE @VoucherDetail_Cr numeric(18, 4);

            //SET @OpeningBalanceAmount = 0

            //               DELETE FROM Temp_AccountsLedgerDetail WHERE TempId IN (SELECT TempId FROM Temp_AccountsLedgerOrProvisionalLedger WHERE LedgerOrProvisional = @ReportType AND CompanyId = @CompanyId AND EntryBy = @EntryBy)
            //DELETE FROM Temp_AccountsLedgerOrProvisionalLedger WHERE LedgerOrProvisional = @ReportType AND CompanyId = @CompanyId AND EntryBy = @EntryBy

            //-- Get company currency
            //SELECT @CurrencyLevel = dbo.fn_CurrencyLevel(@CompanyId, @Currency)

            //            SELECT Setup_Accounts.*, Setup_AccountsControl.AccountsControlId, Setup_AccountsSubGroup.AccountsSubGroupId, Setup_AccountsGroup.AccountsGroupId INTO #TempAccountsTable FROM Setup_Accounts
            //INNER JOIN Setup_AccountsSubsidiary ON Setup_Accounts.AccountsSubsidiaryId = Setup_AccountsSubsidiary.AccountsSubsidiaryId
            //INNER JOIN Setup_AccountsControl ON Setup_AccountsSubsidiary.AccountsControlId = Setup_AccountsControl.AccountsControlId
            //INNER JOIN Setup_AccountsSubGroup ON Setup_AccountsControl.AccountsSubGroupId = Setup_AccountsSubGroup.AccountsSubGroupId
            //INNER JOIN Setup_AccountsGroup ON Setup_AccountsSubGroup.AccountsGroupId = Setup_AccountsGroup.AccountsGroupId 
            //WHERE Setup_Accounts.CompanyId = @CompanyId

            //            IF (@AccGroupId > 0)
            //            BEGIN
            //             DELETE FROM #TempAccountsTable WHERE AccountsGroupId <> @AccGroupId
            //            END

            //            IF (@AccSubGroupId > 0)
            //            BEGIN
            //             DELETE FROM #TempAccountsTable WHERE AccountsSubGroupId <> @AccSubGroupId
            //            END

            //            IF (@AccControlId > 0)
            //            BEGIN
            //             DELETE FROM #TempAccountsTable WHERE AccountsControlId <> @AccControlId
            //            END

            //            IF (@AccSubsidiaryId > 0)
            //            BEGIN
            //             DELETE FROM #TempAccountsTable WHERE AccountsSubsidiaryId <> @AccSubsidiaryId
            //            END

            //            IF (@AccId > 0)
            //            BEGIN
            //             DELETE FROM #TempAccountsTable WHERE AccountsId <> @AccId
            //            END

            //         --START CURSOR FOR Accounts
            //         DECLARE CursorAccounts CURSOR FOR
            //         SELECT AccountsId, Name, BalanceType FROM #TempAccountsTable Order By AccountsId

            //         OPEN CursorAccounts
            //         FETCH NEXT FROM CursorAccounts INTO @Accounts_AccId, @Accounts_AccName, @Acc_BalanceType
            //         SET @FETCH_STATUS_Accounts = @@FETCH_STATUS
            //         WHILE @FETCH_STATUS_Accounts = 0
            //         BEGIN
            //	SET @Group_BalanceType = @Acc_BalanceType
            //	SET @Acc_Opening = 0
            //          SELECT @Acc_Opening = dbo.fn_CalculateOpeningBalanceForAccounts(@CurrencyLevel, @Accounts_AccId, @CompanyId, @DateFrom)
            //	SET @OpeningBalanceAmount = @OpeningBalanceAmount + @Acc_Opening

            //          IF(@ReportType = 'P')
            //          BEGIN
            //		--start cursor for CursorLedgerVoucher
            //           DECLARE CursorLedgerVoucher CURSOR FOR
            //		SELECT DISTINCT V.VoucherId, V.VoucherNo, V.Date
            //		FROM Task_Voucher V INNER JOIN Task_VoucherDetail VD ON V.VoucherId = VD.VoucherId
            //           WHERE V.Approved = 'A' AND VD.AccountsId = @Accounts_AccId AND V.CompanyId = @CompanyId AND CAST(Date AS date) >= @DateFrom AND CAST(Date AS date) <= @DateTo Order By Date

            //		OPEN CursorLedgerVoucher
            //           FETCH NEXT FROM CursorLedgerVoucher INTO @LedgerVoucher_Id, @LedgerVoucher_No, @LedgerVoucher_Date
            //           SET @FETCH_STATUS_PostedVoucher = @@FETCH_STATUS
            //           WHILE @FETCH_STATUS_PostedVoucher = 0
            //           BEGIN
            //			SELECT @VoucherDetailParticular = STUFF((SELECT  ', ' + EE.Particulars FROM Task_VoucherDetail EE WHERE  EE.VoucherId = @LedgerVoucher_Id AND EE.AccountsId = @Accounts_AccId ORDER BY EE.Particulars FOR XML PATH('')), 1, 1, '')

            //			SELECT @VoucherDetail_Dr = SUM(CASE @CurrencyLevel WHEN 1 THEN VD.Debit WHEN 2 THEN VD.Currency1Debit WHEN 3 THEN VD.Currency2Debit END), @VoucherDetail_Cr = SUM(CASE @CurrencyLevel WHEN 1 THEN VD.Credit WHEN 2 THEN VD.Currency1Credit WHEN 3 THEN VD.Currency2Credit END) FROM Task_VoucherDetail VD WHERE  VD.VoucherId = @LedgerVoucher_Id AND VD.AccountsId = @Accounts_AccId

            //			--insert voucher data to temp table
            //			SET @Temp_Id = NEWID()
            //			INSERT INTO Temp_AccountsLedgerOrProvisionalLedger (TempId, LedgerOrProvisional, Date, VoucherNo, VoucherDescription, DrAmount, CrAmount, CompanyId, EntryBy)
            //			VALUES (@Temp_Id, @ReportType, @LedgerVoucher_Date, @LedgerVoucher_No, @VoucherDetailParticular, @VoucherDetail_Dr, @VoucherDetail_Cr, @CompanyId, @EntryBy)

            //			-- Insert other accounts head to temp detail table
            //			INSERT INTO Temp_AccountsLedgerDetail (TempId, AccountsName, DrAmount, CrAmount)
            //			SELECT @Temp_Id, ACC.Name, (CASE @CurrencyLevel WHEN 1 THEN VD.Debit WHEN 2 THEN VD.Currency1Debit WHEN 3 THEN VD.Currency2Debit END), (CASE @CurrencyLevel WHEN 1 THEN VD.Credit WHEN 2 THEN VD.Currency1Credit WHEN 3 THEN VD.Currency2Credit END)
            //			FROM Task_VoucherDetail VD INNER JOIN Setup_Accounts ACC ON VD.AccountsId = ACC.AccountsId
            //			WHERE VoucherId = @LedgerVoucher_Id AND VD.AccountsId <> @Accounts_AccId

            //			FETCH NEXT FROM CursorLedgerVoucher INTO @LedgerVoucher_Id, @LedgerVoucher_No, @LedgerVoucher_Date
            //			SET @FETCH_STATUS_PostedVoucher = @@FETCH_STATUS
            //		END

            //		CLOSE CursorLedgerVoucher
            //           DEALLOCATE CursorLedgerVoucher
            //           --end cursor for CursorLedgerVoucher
            //          END
            //          ELSE IF (@ReportType = 'L')
            //          BEGIN
            //           --start cursor for CursorPostedVoucher
            //           DECLARE CursorPostedVoucher CURSOR FOR
            //		SELECT DISTINCT V.VoucherId, V.VoucherNo, V.Date FROM Task_Voucher AS V 
            //		INNER JOIN Task_VoucherDetail AS VD ON V.VoucherId = VD.VoucherId 
            //		INNER JOIN Task_PostedVoucher AS PV ON VD.VoucherDetailId = PV.VoucherDetailId AND V.CompanyId = PV.CompanyId
            //		WHERE (V.Approved = 'A') AND (V.Posted = 1) AND (VD.AccountsId = @Accounts_AccId) AND (V.CompanyId = @CompanyId) AND (CAST(V.Date AS date) >= @DateFrom) AND (CAST(V.Date AS date) <= @DateTo)
            //		ORDER BY V.Date

            //		OPEN CursorPostedVoucher
            //           FETCH NEXT FROM CursorPostedVoucher INTO @LedgerVoucher_Id, @LedgerVoucher_No, @LedgerVoucher_Date
            //           SET @FETCH_STATUS_PostedVoucher = @@FETCH_STATUS
            //           WHILE @FETCH_STATUS_PostedVoucher = 0
            //           BEGIN
            //			SELECT @VoucherDetailParticular = STUFF((SELECT  ', ' + EE.Particulars FROM Task_VoucherDetail EE WHERE  EE.VoucherId = @LedgerVoucher_Id AND EE.AccountsId = @Accounts_AccId ORDER BY EE.Particulars FOR XML PATH('')), 1, 1, '')

            //			SELECT @VoucherDetail_Dr = SUM(CASE @CurrencyLevel WHEN 1 THEN VD.Debit WHEN 2 THEN VD.Currency1Debit WHEN 3 THEN VD.Currency2Debit END), @VoucherDetail_Cr = SUM(CASE @CurrencyLevel WHEN 1 THEN VD.Credit WHEN 2 THEN VD.Currency1Credit WHEN 3 THEN VD.Currency2Credit END) FROM Task_VoucherDetail VD WHERE  VD.VoucherId = @LedgerVoucher_Id AND VD.AccountsId = @Accounts_AccId

            //			--insert voucher data to temp table
            //			SET @Temp_Id = NEWID()
            //			INSERT INTO Temp_AccountsLedgerOrProvisionalLedger (TempId, LedgerOrProvisional, Date, VoucherNo, VoucherDescription, DrAmount, CrAmount, CompanyId, EntryBy)
            //			VALUES (@Temp_Id, @ReportType, @LedgerVoucher_Date, @LedgerVoucher_No, @VoucherDetailParticular, @VoucherDetail_Dr, @VoucherDetail_Cr, @CompanyId, @EntryBy)

            //			-- Insert other accounts head to temp detail table
            //			INSERT INTO Temp_AccountsLedgerDetail (TempId, AccountsName, DrAmount, CrAmount)
            //			SELECT @Temp_Id, ACC.Name, (CASE @CurrencyLevel WHEN 1 THEN VD.Debit WHEN 2 THEN VD.Currency1Debit WHEN 3 THEN VD.Currency2Debit END), (CASE @CurrencyLevel WHEN 1 THEN VD.Credit WHEN 2 THEN VD.Currency1Credit WHEN 3 THEN VD.Currency2Credit END)
            //			FROM Task_VoucherDetail VD INNER JOIN Setup_Accounts ACC ON VD.AccountsId = ACC.AccountsId
            //			WHERE VoucherId = @LedgerVoucher_Id AND VD.AccountsId <> @Accounts_AccId

            //			FETCH NEXT FROM CursorPostedVoucher INTO @LedgerVoucher_Id, @LedgerVoucher_No, @LedgerVoucher_Date
            //			SET @FETCH_STATUS_PostedVoucher = @@FETCH_STATUS
            //		END

            //		CLOSE CursorPostedVoucher
            //           DEALLOCATE CursorPostedVoucher
            //           --end cursor for CursorPostedVoucher
            //          END

            //	FETCH NEXT FROM CursorAccounts INTO @Accounts_AccId, @Accounts_AccName, @Acc_BalanceType
            //	SET @FETCH_STATUS_Accounts = @@FETCH_STATUS
            //         END

            //         CLOSE CursorAccounts
            //         DEALLOCATE CursorAccounts
            //         --END CURSOR FOR Accounts

            //--insert opening balance info to temp table
            //IF @Group_BalanceType = 'Dr'
            //BEGIN
            //	IF @OpeningBalanceAmount >= 0
            //	BEGIN
            //		SET @Dr_OpeningBalanceAmount = @OpeningBalanceAmount
            //		SET @Cr_OpeningBalanceAmount = 0
            //	END
            //	ELSE
            //	BEGIN
            //		SET @Dr_OpeningBalanceAmount = 0
            //		SET @Cr_OpeningBalanceAmount = ABS(@OpeningBalanceAmount)
            //	END
            //END
            //ELSE
            //BEGIN
            //	IF @OpeningBalanceAmount >= 0
            //	BEGIN
            //		SET @Dr_OpeningBalanceAmount = 0
            //		SET @Cr_OpeningBalanceAmount = @OpeningBalanceAmount
            //	END
            //	ELSE
            //	BEGIN
            //		SET @Dr_OpeningBalanceAmount = ABS(@OpeningBalanceAmount)
            //		SET @Cr_OpeningBalanceAmount = 0
            //	END
            //END

            //      INSERT INTO Temp_AccountsLedgerOrProvisionalLedger (LedgerOrProvisional, Date, VoucherNo, DrAmount, CrAmount, CompanyId, EntryBy) VALUES
            //      (@ReportType, @DateFrom, 'Opening Balance', @Dr_OpeningBalanceAmount, @Cr_OpeningBalanceAmount, @CompanyId, @EntryBy)

            //           END");
            //       }

            //if stored procedure found then alter stored procedure
            if (CheckTable("SP_AccountsLedger"))
            {
                _db.Database.ExecuteSqlCommand(@"ALTER PROCEDURE SP_AccountsLedger
                @Currency varchar(5),
                @LocationId bigint,
                @CompanyId bigint,
                @DateFrom date,
                @DateTo date,
                @AccGroupId bigint,
                @AccSubGroupId bigint,
                @AccControlId bigint,
                @AccSubsidiaryId bigint,
                @AccId bigint,
                @EntryBy bigint,
                @ReportType varchar
                AS
                BEGIN
	                --@ReportType will be (L/P)
	                --L = Ledger Report 
	                --P = Provisional Ledger Report
					DECLARE @CurrencyLevel int;
	                DECLARE @FETCH_STATUS_Accounts int;
	                DECLARE @FETCH_STATUS_PostedVoucher int;
	                DECLARE @Accounts_AccId bigint;
					DECLARE @Accounts_AccName varchar(200);
					DECLARE @Group_BalanceType varchar(2);
					DECLARE @Acc_BalanceType varchar(2);
					DECLARE @Acc_Opening numeric(18, 4);
	                DECLARE @OpeningBalanceAmount numeric(18, 4);
					DECLARE @Dr_OpeningBalanceAmount numeric(18, 4);
	                DECLARE @Cr_OpeningBalanceAmount numeric(18, 4);
					DECLARE @LedgerVoucher_Id uniqueidentifier;
					DECLARE @Temp_Id uniqueidentifier;
					DECLARE @LedgerVoucher_No varchar(50);
					DECLARE @LedgerVoucher_Date datetime;
					DECLARE @VoucherDetailParticular varchar(1000);
					DECLARE @VoucherDetail_Dr numeric(18, 4);
					DECLARE @VoucherDetail_Cr numeric(18, 4);
					DECLARE @FeatureLocationWiseAccountsBalance bit;

					SET @OpeningBalanceAmount = 0
							
                    DELETE FROM Temp_AccountsLedgerDetail WHERE TempId IN (SELECT TempId FROM Temp_AccountsLedgerOrProvisionalLedger WHERE LedgerOrProvisional = @ReportType AND CompanyId = @CompanyId AND EntryBy = @EntryBy)
					DELETE FROM Temp_AccountsLedgerOrProvisionalLedger WHERE LedgerOrProvisional = @ReportType AND CompanyId = @CompanyId AND EntryBy = @EntryBy

					-- Get company currency
					SELECT @CurrencyLevel = dbo.fn_CurrencyLevel(@CompanyId, @Currency)

					-- Get LocationWiseAccountsBalance feature
					SELECT @FeatureLocationWiseAccountsBalance = [dbo].[fn_CompanyFeatureValue] ('LocationWiseAccountsBalance', @CompanyId)

	                SELECT Setup_Accounts.*, Setup_AccountsControl.AccountsControlId, Setup_AccountsSubGroup.AccountsSubGroupId, Setup_AccountsGroup.AccountsGroupId INTO #TempAccountsTable FROM Setup_Accounts
					INNER JOIN Setup_AccountsSubsidiary ON Setup_Accounts.AccountsSubsidiaryId = Setup_AccountsSubsidiary.AccountsSubsidiaryId
					INNER JOIN Setup_AccountsControl ON Setup_AccountsSubsidiary.AccountsControlId = Setup_AccountsControl.AccountsControlId
					INNER JOIN Setup_AccountsSubGroup ON Setup_AccountsControl.AccountsSubGroupId = Setup_AccountsSubGroup.AccountsSubGroupId
					INNER JOIN Setup_AccountsGroup ON Setup_AccountsSubGroup.AccountsGroupId = Setup_AccountsGroup.AccountsGroupId 
					WHERE Setup_Accounts.CompanyId = @CompanyId

	                IF (@AccGroupId > 0)
	                BEGIN
		                DELETE FROM #TempAccountsTable WHERE AccountsGroupId <> @AccGroupId
	                END

	                IF (@AccSubGroupId > 0)
	                BEGIN
		                DELETE FROM #TempAccountsTable WHERE AccountsSubGroupId <> @AccSubGroupId
	                END

	                IF (@AccControlId > 0)
	                BEGIN
		                DELETE FROM #TempAccountsTable WHERE AccountsControlId <> @AccControlId
	                END

	                IF (@AccSubsidiaryId > 0)
	                BEGIN
		                DELETE FROM #TempAccountsTable WHERE AccountsSubsidiaryId <> @AccSubsidiaryId
	                END

	                IF (@AccId > 0)
	                BEGIN
		                DELETE FROM #TempAccountsTable WHERE AccountsId <> @AccId
	                END

		            --START CURSOR FOR Accounts
		            DECLARE CursorAccounts CURSOR FOR
		            SELECT AccountsId, Name, BalanceType FROM #TempAccountsTable Order By AccountsId

		            OPEN CursorAccounts
		            FETCH NEXT FROM CursorAccounts INTO @Accounts_AccId, @Accounts_AccName, @Acc_BalanceType
		            SET @FETCH_STATUS_Accounts = @@FETCH_STATUS
		            WHILE @FETCH_STATUS_Accounts = 0
		            BEGIN
						SET @Group_BalanceType = @Acc_BalanceType
						SET @Acc_Opening = 0
			            SELECT @Acc_Opening = dbo.fn_CalculateOpeningBalanceForAccounts(@CurrencyLevel, @Accounts_AccId, @LocationId, @CompanyId, @DateFrom)
						SET @OpeningBalanceAmount = @OpeningBalanceAmount + @Acc_Opening

			            IF(@ReportType = 'P')
			            BEGIN
							--start cursor for CursorLedgerVoucher
				            DECLARE CursorLedgerVoucher CURSOR FOR
							SELECT DISTINCT V.VoucherId, V.VoucherNo, V.Date
							FROM Task_Voucher V INNER JOIN Task_VoucherDetail VD ON V.VoucherId = VD.VoucherId
				            WHERE V.Approved = 'A' AND VD.AccountsId = @Accounts_AccId AND V.CompanyId = @CompanyId AND CAST(Date AS date) >= @DateFrom AND CAST(Date AS date) <= @DateTo 
							AND (0 = (CASE WHEN @FeatureLocationWiseAccountsBalance = 1 AND @LocationId > 0 THEN 1 ELSE 0 END) OR V.LocationId = @LocationId)
							Order By Date

							OPEN CursorLedgerVoucher
				            FETCH NEXT FROM CursorLedgerVoucher INTO @LedgerVoucher_Id, @LedgerVoucher_No, @LedgerVoucher_Date
				            SET @FETCH_STATUS_PostedVoucher = @@FETCH_STATUS
				            WHILE @FETCH_STATUS_PostedVoucher = 0
				            BEGIN
								SELECT @VoucherDetailParticular = STUFF((SELECT  ', ' + EE.Particulars FROM Task_VoucherDetail EE WHERE  EE.VoucherId = @LedgerVoucher_Id AND EE.AccountsId = @Accounts_AccId ORDER BY EE.Particulars FOR XML PATH('')), 1, 1, '')

								SELECT @VoucherDetail_Dr = SUM(CASE @CurrencyLevel WHEN 1 THEN VD.Debit WHEN 2 THEN VD.Currency1Debit WHEN 3 THEN VD.Currency2Debit END), @VoucherDetail_Cr = SUM(CASE @CurrencyLevel WHEN 1 THEN VD.Credit WHEN 2 THEN VD.Currency1Credit WHEN 3 THEN VD.Currency2Credit END) FROM Task_VoucherDetail VD WHERE  VD.VoucherId = @LedgerVoucher_Id AND VD.AccountsId = @Accounts_AccId

								--insert voucher data to temp table
								SET @Temp_Id = NEWID()
								INSERT INTO Temp_AccountsLedgerOrProvisionalLedger (TempId, LedgerOrProvisional, Date, VoucherNo, VoucherDescription, DrAmount, CrAmount, CompanyId, EntryBy)
								VALUES (@Temp_Id, @ReportType, @LedgerVoucher_Date, @LedgerVoucher_No, @VoucherDetailParticular, @VoucherDetail_Dr, @VoucherDetail_Cr, @CompanyId, @EntryBy)

								-- Insert other accounts head to temp detail table
								INSERT INTO Temp_AccountsLedgerDetail (TempId, AccountsName, DrAmount, CrAmount)
								SELECT @Temp_Id, ACC.Name, (CASE @CurrencyLevel WHEN 1 THEN VD.Debit WHEN 2 THEN VD.Currency1Debit WHEN 3 THEN VD.Currency2Debit END), (CASE @CurrencyLevel WHEN 1 THEN VD.Credit WHEN 2 THEN VD.Currency1Credit WHEN 3 THEN VD.Currency2Credit END)
								FROM Task_VoucherDetail VD INNER JOIN Setup_Accounts ACC ON VD.AccountsId = ACC.AccountsId
								WHERE VoucherId = @LedgerVoucher_Id AND VD.AccountsId <> @Accounts_AccId

								FETCH NEXT FROM CursorLedgerVoucher INTO @LedgerVoucher_Id, @LedgerVoucher_No, @LedgerVoucher_Date
								SET @FETCH_STATUS_PostedVoucher = @@FETCH_STATUS
							END

							CLOSE CursorLedgerVoucher
				            DEALLOCATE CursorLedgerVoucher
				            --end cursor for CursorLedgerVoucher
			            END
			            ELSE IF (@ReportType = 'L')
			            BEGIN
				            --start cursor for CursorPostedVoucher
				            DECLARE CursorPostedVoucher CURSOR FOR
							SELECT DISTINCT V.VoucherId, V.VoucherNo, V.Date FROM Task_Voucher AS V 
							INNER JOIN Task_VoucherDetail AS VD ON V.VoucherId = VD.VoucherId 
							INNER JOIN Task_PostedVoucher AS PV ON VD.VoucherDetailId = PV.VoucherDetailId AND V.CompanyId = PV.CompanyId
							WHERE (V.Approved = 'A') AND (V.Posted = 1) AND (VD.AccountsId = @Accounts_AccId) AND (V.CompanyId = @CompanyId) AND (CAST(V.Date AS date) >= @DateFrom) AND (CAST(V.Date AS date) <= @DateTo)
							AND (0 = (CASE WHEN @FeatureLocationWiseAccountsBalance = 1 AND @LocationId > 0 THEN 1 ELSE 0 END) OR V.LocationId = @LocationId)
							ORDER BY V.Date

							OPEN CursorPostedVoucher
				            FETCH NEXT FROM CursorPostedVoucher INTO @LedgerVoucher_Id, @LedgerVoucher_No, @LedgerVoucher_Date
				            SET @FETCH_STATUS_PostedVoucher = @@FETCH_STATUS
				            WHILE @FETCH_STATUS_PostedVoucher = 0
				            BEGIN
								SELECT @VoucherDetailParticular = STUFF((SELECT  ', ' + EE.Particulars FROM Task_VoucherDetail EE WHERE  EE.VoucherId = @LedgerVoucher_Id AND EE.AccountsId = @Accounts_AccId ORDER BY EE.Particulars FOR XML PATH('')), 1, 1, '')

								SELECT @VoucherDetail_Dr = SUM(CASE @CurrencyLevel WHEN 1 THEN VD.Debit WHEN 2 THEN VD.Currency1Debit WHEN 3 THEN VD.Currency2Debit END), @VoucherDetail_Cr = SUM(CASE @CurrencyLevel WHEN 1 THEN VD.Credit WHEN 2 THEN VD.Currency1Credit WHEN 3 THEN VD.Currency2Credit END) FROM Task_VoucherDetail VD WHERE  VD.VoucherId = @LedgerVoucher_Id AND VD.AccountsId = @Accounts_AccId

								--insert voucher data to temp table
								SET @Temp_Id = NEWID()
								INSERT INTO Temp_AccountsLedgerOrProvisionalLedger (TempId, LedgerOrProvisional, Date, VoucherNo, VoucherDescription, DrAmount, CrAmount, CompanyId, EntryBy)
								VALUES (@Temp_Id, @ReportType, @LedgerVoucher_Date, @LedgerVoucher_No, @VoucherDetailParticular, @VoucherDetail_Dr, @VoucherDetail_Cr, @CompanyId, @EntryBy)

								-- Insert other accounts head to temp detail table
								INSERT INTO Temp_AccountsLedgerDetail (TempId, AccountsName, DrAmount, CrAmount)
								SELECT @Temp_Id, ACC.Name, (CASE @CurrencyLevel WHEN 1 THEN VD.Debit WHEN 2 THEN VD.Currency1Debit WHEN 3 THEN VD.Currency2Debit END), (CASE @CurrencyLevel WHEN 1 THEN VD.Credit WHEN 2 THEN VD.Currency1Credit WHEN 3 THEN VD.Currency2Credit END)
								FROM Task_VoucherDetail VD INNER JOIN Setup_Accounts ACC ON VD.AccountsId = ACC.AccountsId
								WHERE VoucherId = @LedgerVoucher_Id AND VD.AccountsId <> @Accounts_AccId

								FETCH NEXT FROM CursorPostedVoucher INTO @LedgerVoucher_Id, @LedgerVoucher_No, @LedgerVoucher_Date
								SET @FETCH_STATUS_PostedVoucher = @@FETCH_STATUS
							END

							CLOSE CursorPostedVoucher
				            DEALLOCATE CursorPostedVoucher
				            --end cursor for CursorPostedVoucher
			            END

						FETCH NEXT FROM CursorAccounts INTO @Accounts_AccId, @Accounts_AccName, @Acc_BalanceType
						SET @FETCH_STATUS_Accounts = @@FETCH_STATUS
		            END

		            CLOSE CursorAccounts
		            DEALLOCATE CursorAccounts
		            --END CURSOR FOR Accounts

					--insert opening balance info to temp table
					IF @Group_BalanceType = 'Dr'
					BEGIN
						IF @OpeningBalanceAmount >= 0
						BEGIN
							SET @Dr_OpeningBalanceAmount = @OpeningBalanceAmount
							SET @Cr_OpeningBalanceAmount = 0
						END
						ELSE
						BEGIN
							SET @Dr_OpeningBalanceAmount = 0
							SET @Cr_OpeningBalanceAmount = ABS(@OpeningBalanceAmount)
						END
					END
					ELSE
					BEGIN
						IF @OpeningBalanceAmount >= 0
						BEGIN
							SET @Dr_OpeningBalanceAmount = 0
							SET @Cr_OpeningBalanceAmount = @OpeningBalanceAmount
						END
						ELSE
						BEGIN
							SET @Dr_OpeningBalanceAmount = ABS(@OpeningBalanceAmount)
							SET @Cr_OpeningBalanceAmount = 0
						END
					END

			        INSERT INTO Temp_AccountsLedgerOrProvisionalLedger (LedgerOrProvisional, Date, VoucherNo, DrAmount, CrAmount, CompanyId, EntryBy) VALUES
			        (@ReportType, @DateFrom, 'Opening Balance', @Dr_OpeningBalanceAmount, @Cr_OpeningBalanceAmount, @CompanyId, @EntryBy)

                END");
            }

            //if stored procedure not found then create stored procedure
            if (!CheckTable("SP_TrialBalance"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE PROCEDURE SP_TrialBalance
                @CompanyId bigint,
                @EntryBy bigint,
                @ReportType bit,
                @Currency varchar(5),
                @AccountsLevel varchar(20),
                @SelectedId bigint,
                @ReportFor varchar(5),
                @DateFrom date,
                @DateTo date
                AS
                BEGIN
	                --@ReportType will be (0/1)
	                --0 = without hierarchy
	                --1 = with hierarchy
	                --@ReportFor will be (TB/TTB)
	                --TB = Trial Balance
	                --TTB = Transactional Trial Balance

	                DECLARE @Acc_BalanceType varchar(2);
	                DECLARE @Accounts_AccId bigint;
	                DECLARE @CurrencyLevel int;
	                DECLARE @FETCH_STATUS_Accounts int;
	                DECLARE @Acc_Opening numeric(18, 4);
	                DECLARE @Dr_OpeningBalanceAmount numeric(18, 4);
	                DECLARE @Cr_OpeningBalanceAmount numeric(18, 4);
	                DECLARE @Dr_TranAmount numeric(18, 4);
	                DECLARE @Cr_TranAmount numeric(18, 4);

	                SET @CurrencyLevel = 1;

	                DELETE FROM Temp_TrialBalance WHERE ReportType = @ReportType AND CompanyId = @CompanyId AND EntryBy = @EntryBy

	                -- For only trial balance, add 1 day with DateFrom
	                IF(@ReportFor = 'TB')
	                BEGIN
		                SET @DateFrom = DATEADD(d, 1, @DateFrom)
	                END

	                -- Get company currency
	                SELECT @CurrencyLevel = dbo.fn_CurrencyLevel(@CompanyId, @Currency)

	                SELECT Setup_AccountsGroup.BalanceType, Setup_Accounts.AccountsId, Setup_Accounts.AccountsSubsidiaryId, Setup_AccountsControl.AccountsControlId, Setup_AccountsSubGroup.AccountsSubGroupId, Setup_AccountsGroup.AccountsGroupId INTO #TempAccountsTableForTrialBalance FROM Setup_Accounts
	                INNER JOIN Setup_AccountsSubsidiary ON Setup_Accounts.AccountsSubsidiaryId = Setup_AccountsSubsidiary.AccountsSubsidiaryId
	                INNER JOIN Setup_AccountsControl ON Setup_AccountsSubsidiary.AccountsControlId = Setup_AccountsControl.AccountsControlId
	                INNER JOIN Setup_AccountsSubGroup ON Setup_AccountsControl.AccountsSubGroupId = Setup_AccountsSubGroup.AccountsSubGroupId
	                INNER JOIN Setup_AccountsGroup ON Setup_AccountsSubGroup.AccountsGroupId = Setup_AccountsGroup.AccountsGroupId 
	                WHERE Setup_Accounts.CompanyId = @CompanyId

	                IF (@AccountsLevel = 'SubGroup')
	                BEGIN
		                DELETE FROM #TempAccountsTableForTrialBalance WHERE AccountsGroupId <> @SelectedId
	                END

	                IF (@AccountsLevel = 'Control')
	                BEGIN
		                DELETE FROM #TempAccountsTableForTrialBalance WHERE AccountsSubGroupId <> @SelectedId
	                END

	                IF (@AccountsLevel = 'Subsidiary')
	                BEGIN
		                DELETE FROM #TempAccountsTableForTrialBalance WHERE AccountsControlId <> @SelectedId
	                END

	                IF (@AccountsLevel = 'Accounts')
	                BEGIN
		                DELETE FROM #TempAccountsTableForTrialBalance WHERE AccountsSubsidiaryId <> @SelectedId
	                END

	                --START CURSOR FOR Accounts
	                DECLARE CursorAccounts CURSOR FOR
	                SELECT AccountsId, BalanceType FROM #TempAccountsTableForTrialBalance Order By AccountsId

	                OPEN CursorAccounts
	                FETCH NEXT FROM CursorAccounts INTO @Accounts_AccId, @Acc_BalanceType
	                SET @FETCH_STATUS_Accounts = @@FETCH_STATUS
	                WHILE @FETCH_STATUS_Accounts = 0
	                BEGIN
		                SET @Acc_Opening = 0
		                SET @Dr_TranAmount = 0
		                SET @Cr_TranAmount = 0
		                SELECT @Acc_Opening = dbo.fn_CalculateOpeningBalanceForAccounts(@CurrencyLevel, @Accounts_AccId, @CompanyId, @DateFrom)

		                --insert opening balance info to temp table
		                IF @Acc_BalanceType = 'Dr'
		                BEGIN
			                IF @Acc_Opening >= 0
			                BEGIN
				                SET @Dr_OpeningBalanceAmount = @Acc_Opening
				                SET @Cr_OpeningBalanceAmount = 0
			                END
			                ELSE
			                BEGIN
				                SET @Dr_OpeningBalanceAmount = 0
				                SET @Cr_OpeningBalanceAmount = ABS(@Acc_Opening)
			                END
		                END
		                ELSE
		                BEGIN
			                IF @Acc_Opening >= 0
			                BEGIN
				                SET @Dr_OpeningBalanceAmount = 0
				                SET @Cr_OpeningBalanceAmount = @Acc_Opening
			                END
			                ELSE
			                BEGIN
				                SET @Dr_OpeningBalanceAmount = ABS(@Acc_Opening)
				                SET @Cr_OpeningBalanceAmount = 0
			                END
		                END

		                IF(@ReportType = 1 OR @ReportFor = 'TTB')
		                BEGIN
			                DECLARE @Debit numeric(18, 4);
			                DECLARE @Debit1 numeric(18, 4);
			                DECLARE @Debit2 numeric(18, 4);
			                DECLARE @Credit numeric(18, 4);
			                DECLARE @Credit1 numeric(18, 4);
			                DECLARE @Credit2 numeric(18, 4);

			                SELECT @Debit = COALESCE(SUM(VD.Debit), 0), @Credit = COALESCE(SUM(VD.Credit), 0), @Debit1 = COALESCE(SUM(VD.Currency1Debit), 0), @Credit1 = COALESCE(SUM(VD.Currency1Credit), 0), @Debit2 = COALESCE(SUM(VD.Currency2Debit), 0), @Credit2 = COALESCE(SUM(VD.Currency2Credit), 0)
			                FROM Task_Voucher AS V INNER JOIN Task_VoucherDetail AS VD ON V.VoucherId = VD.VoucherId 
			                INNER JOIN Task_PostedVoucher AS PV ON VD.VoucherDetailId = PV.VoucherDetailId AND V.CompanyId = PV.CompanyId
			                WHERE (V.Approved = 'A') AND (V.Posted = 1) AND (VD.AccountsId = @Accounts_AccId) AND (V.CompanyId = @CompanyId) AND (CAST(V.Date AS date) >= @DateFrom) AND (CAST(V.Date AS date) <= @DateTo)

			                IF(@CurrencyLevel = 1)
			                BEGIN
				                SET @Dr_TranAmount = @Debit
				                SET @Cr_TranAmount = @Credit
			                END

			                IF(@CurrencyLevel = 2)
			                BEGIN
				                SET @Dr_TranAmount = @Debit1
				                SET @Cr_TranAmount = @Credit1
			                END

			                IF(@CurrencyLevel = 3)
			                BEGIN
				                SET @Dr_TranAmount = @Debit2
				                SET @Cr_TranAmount = @Credit2
			                END
		                END

		                INSERT INTO Temp_TrialBalance(ReportType, AccountsId, OpeningDr, OpeningCr, TransactionDr, TransactionCr, CompanyId, EntryBy) VALUES
		                (@ReportType, @Accounts_AccId, @Dr_OpeningBalanceAmount, @Cr_OpeningBalanceAmount, @Dr_TranAmount, @Cr_TranAmount, @CompanyId, @EntryBy)

		                FETCH NEXT FROM CursorAccounts INTO @Accounts_AccId, @Acc_BalanceType
		                SET @FETCH_STATUS_Accounts = @@FETCH_STATUS
	                END

	                CLOSE CursorAccounts
	                DEALLOCATE CursorAccounts
	                --END CURSOR FOR Accounts
                END");
            }

            //        //if stored procedure found then alter stored procedure
            //        if (CheckTable("SP_TrialBalance"))
            //        {
            //            _db.Database.ExecuteSqlCommand(@"ALTER PROCEDURE SP_TrialBalance
            //            @CompanyId bigint,
            //            @EntryBy bigint,
            //            @ReportType bit,
            //            @Currency varchar(5),
            //            @AccountsLevel varchar(20),
            //            @GroupId bigint,
            //@SubGroupId bigint,
            //@ControlId bigint,
            //@SubsidiaryId bigint,
            //            @ReportFor varchar(5),
            //            @DateFrom date,
            //            @DateTo date
            //            AS
            //            BEGIN
            //             --@ReportType will be (0/1)
            //             --0 = without hierarchy
            //             --1 = with hierarchy
            //             --@ReportFor will be (TB/TTB)
            //             --TB = Trial Balance
            //             --TTB = Transactional Trial Balance

            //             DECLARE @Acc_BalanceType varchar(2);
            //             DECLARE @Accounts_AccId bigint;
            //             DECLARE @CurrencyLevel int;
            //             DECLARE @FETCH_STATUS_Accounts int;
            //             DECLARE @Acc_Opening numeric(18, 4);
            //             DECLARE @Dr_OpeningBalanceAmount numeric(18, 4);
            //             DECLARE @Cr_OpeningBalanceAmount numeric(18, 4);
            //             DECLARE @Dr_TranAmount numeric(18, 4);
            //             DECLARE @Cr_TranAmount numeric(18, 4);

            //             SET @CurrencyLevel = 1;

            //             DELETE FROM Temp_TrialBalance WHERE ReportType = @ReportType AND CompanyId = @CompanyId AND EntryBy = @EntryBy

            //             -- For only trial balance, add 1 day with DateFrom
            //             IF(@ReportFor = 'TB')
            //             BEGIN
            //              SET @DateFrom = DATEADD(d, 1, @DateFrom)
            //             END

            //             -- Get company currency
            //             SELECT @CurrencyLevel = dbo.fn_CurrencyLevel(@CompanyId, @Currency)

            //             SELECT Setup_AccountsGroup.BalanceType, Setup_Accounts.AccountsId, Setup_Accounts.AccountsSubsidiaryId, Setup_AccountsControl.AccountsControlId, Setup_AccountsSubGroup.AccountsSubGroupId, Setup_AccountsGroup.AccountsGroupId INTO #TempAccountsTableForTrialBalance FROM Setup_Accounts
            //             INNER JOIN Setup_AccountsSubsidiary ON Setup_Accounts.AccountsSubsidiaryId = Setup_AccountsSubsidiary.AccountsSubsidiaryId
            //             INNER JOIN Setup_AccountsControl ON Setup_AccountsSubsidiary.AccountsControlId = Setup_AccountsControl.AccountsControlId
            //             INNER JOIN Setup_AccountsSubGroup ON Setup_AccountsControl.AccountsSubGroupId = Setup_AccountsSubGroup.AccountsSubGroupId
            //             INNER JOIN Setup_AccountsGroup ON Setup_AccountsSubGroup.AccountsGroupId = Setup_AccountsGroup.AccountsGroupId 
            //             WHERE Setup_Accounts.CategorizationId <> 13 AND Setup_Accounts.CompanyId = @CompanyId

            //             IF (@GroupId > 0)
            //             BEGIN
            //              DELETE FROM #TempAccountsTableForTrialBalance WHERE AccountsGroupId <> @GroupId
            //             END

            //             IF (@SubGroupId > 0)
            //             BEGIN
            //              DELETE FROM #TempAccountsTableForTrialBalance WHERE AccountsSubGroupId <> @SubGroupId
            //             END

            //             IF (@ControlId > 0)
            //             BEGIN
            //              DELETE FROM #TempAccountsTableForTrialBalance WHERE AccountsControlId <> @ControlId
            //             END

            //             IF (@SubsidiaryId > 0)
            //             BEGIN
            //              DELETE FROM #TempAccountsTableForTrialBalance WHERE AccountsSubsidiaryId <> @SubsidiaryId
            //             END

            //             --START CURSOR FOR Accounts
            //             DECLARE CursorAccounts CURSOR FOR
            //             SELECT AccountsId, BalanceType FROM #TempAccountsTableForTrialBalance Order By AccountsId

            //             OPEN CursorAccounts
            //             FETCH NEXT FROM CursorAccounts INTO @Accounts_AccId, @Acc_BalanceType
            //             SET @FETCH_STATUS_Accounts = @@FETCH_STATUS
            //             WHILE @FETCH_STATUS_Accounts = 0
            //             BEGIN
            //              SET @Acc_Opening = 0
            //              SET @Dr_TranAmount = 0
            //              SET @Cr_TranAmount = 0
            //              SELECT @Acc_Opening = dbo.fn_CalculateOpeningBalanceForAccounts(@CurrencyLevel, @Accounts_AccId, 0, @CompanyId, @DateFrom)

            //              --insert opening balance info to temp table
            //              IF @Acc_BalanceType = 'Dr'
            //              BEGIN
            //               IF @Acc_Opening >= 0
            //               BEGIN
            //                SET @Dr_OpeningBalanceAmount = @Acc_Opening
            //                SET @Cr_OpeningBalanceAmount = 0
            //               END
            //               ELSE
            //               BEGIN
            //                SET @Dr_OpeningBalanceAmount = 0
            //                SET @Cr_OpeningBalanceAmount = ABS(@Acc_Opening)
            //               END
            //              END
            //              ELSE
            //              BEGIN
            //               IF @Acc_Opening >= 0
            //               BEGIN
            //                SET @Dr_OpeningBalanceAmount = 0
            //                SET @Cr_OpeningBalanceAmount = @Acc_Opening
            //               END
            //               ELSE
            //               BEGIN
            //                SET @Dr_OpeningBalanceAmount = ABS(@Acc_Opening)
            //                SET @Cr_OpeningBalanceAmount = 0
            //               END
            //              END

            //              IF(@ReportType = 1 OR @ReportFor = 'TTB')
            //              BEGIN
            //               DECLARE @Debit numeric(18, 4);
            //               DECLARE @Debit1 numeric(18, 4);
            //               DECLARE @Debit2 numeric(18, 4);
            //               DECLARE @Credit numeric(18, 4);
            //               DECLARE @Credit1 numeric(18, 4);
            //               DECLARE @Credit2 numeric(18, 4);

            //               SELECT @Debit = COALESCE(SUM(VD.Debit), 0), @Credit = COALESCE(SUM(VD.Credit), 0), @Debit1 = COALESCE(SUM(VD.Currency1Debit), 0), @Credit1 = COALESCE(SUM(VD.Currency1Credit), 0), @Debit2 = COALESCE(SUM(VD.Currency2Debit), 0), @Credit2 = COALESCE(SUM(VD.Currency2Credit), 0)
            //               FROM Task_Voucher AS V INNER JOIN Task_VoucherDetail AS VD ON V.VoucherId = VD.VoucherId 
            //               INNER JOIN Task_PostedVoucher AS PV ON VD.VoucherDetailId = PV.VoucherDetailId AND V.CompanyId = PV.CompanyId
            //               WHERE (V.Approved = 'A') AND (V.Posted = 1) AND (VD.AccountsId = @Accounts_AccId) AND (V.CompanyId = @CompanyId) AND (CAST(V.Date AS date) >= @DateFrom) AND (CAST(V.Date AS date) <= @DateTo)

            //               IF(@CurrencyLevel = 1)
            //               BEGIN
            //                SET @Dr_TranAmount = @Debit
            //                SET @Cr_TranAmount = @Credit
            //               END

            //               IF(@CurrencyLevel = 2)
            //               BEGIN
            //                SET @Dr_TranAmount = @Debit1
            //                SET @Cr_TranAmount = @Credit1
            //               END

            //               IF(@CurrencyLevel = 3)
            //               BEGIN
            //                SET @Dr_TranAmount = @Debit2
            //                SET @Cr_TranAmount = @Credit2
            //               END
            //              END

            //              INSERT INTO Temp_TrialBalance(ReportType, AccountsId, OpeningDr, OpeningCr, TransactionDr, TransactionCr, CompanyId, EntryBy) VALUES
            //              (@ReportType, @Accounts_AccId, @Dr_OpeningBalanceAmount, @Cr_OpeningBalanceAmount, @Dr_TranAmount, @Cr_TranAmount, @CompanyId, @EntryBy)

            //              FETCH NEXT FROM CursorAccounts INTO @Accounts_AccId, @Acc_BalanceType
            //              SET @FETCH_STATUS_Accounts = @@FETCH_STATUS
            //             END

            //             CLOSE CursorAccounts
            //             DEALLOCATE CursorAccounts
            //             --END CURSOR FOR Accounts
            //            END");
            //        }

            //if stored procedure found then alter stored procedure
            if (CheckTable("SP_TrialBalance"))
            {
                _db.Database.ExecuteSqlCommand(@"ALTER PROCEDURE SP_TrialBalance
                @LocationId bigint,
                @CompanyId bigint,
                @EntryBy bigint,
                @ReportType bit,
                @Currency varchar(5),
                @AccountsLevel varchar(20),
                @GroupId bigint,
				@SubGroupId bigint,
				@ControlId bigint,
				@SubsidiaryId bigint,
                @ReportFor varchar(5),
                @DateFrom date,
                @DateTo date
                AS
                BEGIN
	                --@ReportType will be (0/1)
	                --0 = without hierarchy
	                --1 = with hierarchy
	                --@ReportFor will be (TB/TTB)
	                --TB = Trial Balance
	                --TTB = Transactional Trial Balance

	                DECLARE @Acc_BalanceType varchar(2);
	                DECLARE @Accounts_AccId bigint;
	                DECLARE @CurrencyLevel int;
	                DECLARE @FETCH_STATUS_Accounts int;
					DECLARE @FeatureLocationWiseAccountsBalance bit;
	                DECLARE @Acc_Opening numeric(18, 4);
	                DECLARE @Dr_OpeningBalanceAmount numeric(18, 4);
	                DECLARE @Cr_OpeningBalanceAmount numeric(18, 4);
	                DECLARE @Dr_TranAmount numeric(18, 4);
	                DECLARE @Cr_TranAmount numeric(18, 4);

	                SET @CurrencyLevel = 1;

	                DELETE FROM Temp_TrialBalance WHERE ReportType = @ReportType AND CompanyId = @CompanyId AND EntryBy = @EntryBy

	                -- For only trial balance, add 1 day with DateFrom
	                IF(@ReportFor = 'TB')
	                BEGIN
		                SET @DateFrom = DATEADD(d, 1, @DateFrom)
	                END

	                -- Get company currency
	                SELECT @CurrencyLevel = dbo.fn_CurrencyLevel(@CompanyId, @Currency)

					-- Get LocationWiseAccountsBalance feature
					SELECT @FeatureLocationWiseAccountsBalance = [dbo].[fn_CompanyFeatureValue] ('LocationWiseAccountsBalance', @CompanyId)

	                SELECT Setup_AccountsGroup.BalanceType, Setup_Accounts.AccountsId, Setup_Accounts.AccountsSubsidiaryId, Setup_AccountsControl.AccountsControlId, Setup_AccountsSubGroup.AccountsSubGroupId, Setup_AccountsGroup.AccountsGroupId INTO #TempAccountsTableForTrialBalance FROM Setup_Accounts
	                INNER JOIN Setup_AccountsSubsidiary ON Setup_Accounts.AccountsSubsidiaryId = Setup_AccountsSubsidiary.AccountsSubsidiaryId
	                INNER JOIN Setup_AccountsControl ON Setup_AccountsSubsidiary.AccountsControlId = Setup_AccountsControl.AccountsControlId
	                INNER JOIN Setup_AccountsSubGroup ON Setup_AccountsControl.AccountsSubGroupId = Setup_AccountsSubGroup.AccountsSubGroupId
	                INNER JOIN Setup_AccountsGroup ON Setup_AccountsSubGroup.AccountsGroupId = Setup_AccountsGroup.AccountsGroupId 
	                WHERE Setup_Accounts.CategorizationId <> 13 AND Setup_Accounts.CompanyId = @CompanyId

	                IF (@GroupId > 0)
	                BEGIN
		                DELETE FROM #TempAccountsTableForTrialBalance WHERE AccountsGroupId <> @GroupId
	                END

	                IF (@SubGroupId > 0)
	                BEGIN
		                DELETE FROM #TempAccountsTableForTrialBalance WHERE AccountsSubGroupId <> @SubGroupId
	                END

	                IF (@ControlId > 0)
	                BEGIN
		                DELETE FROM #TempAccountsTableForTrialBalance WHERE AccountsControlId <> @ControlId
	                END

	                IF (@SubsidiaryId > 0)
	                BEGIN
		                DELETE FROM #TempAccountsTableForTrialBalance WHERE AccountsSubsidiaryId <> @SubsidiaryId
	                END

	                --START CURSOR FOR Accounts
	                DECLARE CursorAccounts CURSOR FOR
	                SELECT AccountsId, BalanceType FROM #TempAccountsTableForTrialBalance Order By AccountsId

	                OPEN CursorAccounts
	                FETCH NEXT FROM CursorAccounts INTO @Accounts_AccId, @Acc_BalanceType
	                SET @FETCH_STATUS_Accounts = @@FETCH_STATUS
	                WHILE @FETCH_STATUS_Accounts = 0
	                BEGIN
		                SET @Acc_Opening = 0
		                SET @Dr_TranAmount = 0
		                SET @Cr_TranAmount = 0
		                SELECT @Acc_Opening = dbo.fn_CalculateOpeningBalanceForAccounts(@CurrencyLevel, @Accounts_AccId, @LocationId, @CompanyId, @DateFrom)

		                --insert opening balance info to temp table
		                IF @Acc_BalanceType = 'Dr'
		                BEGIN
			                IF @Acc_Opening >= 0
			                BEGIN
				                SET @Dr_OpeningBalanceAmount = @Acc_Opening
				                SET @Cr_OpeningBalanceAmount = 0
			                END
			                ELSE
			                BEGIN
				                SET @Dr_OpeningBalanceAmount = 0
				                SET @Cr_OpeningBalanceAmount = ABS(@Acc_Opening)
			                END
		                END
		                ELSE
		                BEGIN
			                IF @Acc_Opening >= 0
			                BEGIN
				                SET @Dr_OpeningBalanceAmount = 0
				                SET @Cr_OpeningBalanceAmount = @Acc_Opening
			                END
			                ELSE
			                BEGIN
				                SET @Dr_OpeningBalanceAmount = ABS(@Acc_Opening)
				                SET @Cr_OpeningBalanceAmount = 0
			                END
		                END

		                IF(@ReportType = 1 OR @ReportFor = 'TTB')
		                BEGIN
			                DECLARE @Debit numeric(18, 4);
			                DECLARE @Debit1 numeric(18, 4);
			                DECLARE @Debit2 numeric(18, 4);
			                DECLARE @Credit numeric(18, 4);
			                DECLARE @Credit1 numeric(18, 4);
			                DECLARE @Credit2 numeric(18, 4);

			                SELECT @Debit = COALESCE(SUM(VD.Debit), 0), @Credit = COALESCE(SUM(VD.Credit), 0), @Debit1 = COALESCE(SUM(VD.Currency1Debit), 0), @Credit1 = COALESCE(SUM(VD.Currency1Credit), 0), @Debit2 = COALESCE(SUM(VD.Currency2Debit), 0), @Credit2 = COALESCE(SUM(VD.Currency2Credit), 0)
			                FROM Task_Voucher AS V INNER JOIN Task_VoucherDetail AS VD ON V.VoucherId = VD.VoucherId 
			                INNER JOIN Task_PostedVoucher AS PV ON VD.VoucherDetailId = PV.VoucherDetailId AND V.CompanyId = PV.CompanyId
			                WHERE (V.Approved = 'A') AND (V.Posted = 1) AND (VD.AccountsId = @Accounts_AccId) AND (V.CompanyId = @CompanyId) AND (CAST(V.Date AS date) >= @DateFrom) AND (CAST(V.Date AS date) <= @DateTo)
							AND (0 = (CASE WHEN @FeatureLocationWiseAccountsBalance = 1 AND @LocationId > 0 THEN 1 ELSE 0 END) OR V.LocationId = @LocationId)

			                IF(@CurrencyLevel = 1)
			                BEGIN
				                SET @Dr_TranAmount = @Debit
				                SET @Cr_TranAmount = @Credit
			                END

			                IF(@CurrencyLevel = 2)
			                BEGIN
				                SET @Dr_TranAmount = @Debit1
				                SET @Cr_TranAmount = @Credit1
			                END

			                IF(@CurrencyLevel = 3)
			                BEGIN
				                SET @Dr_TranAmount = @Debit2
				                SET @Cr_TranAmount = @Credit2
			                END
		                END

		                INSERT INTO Temp_TrialBalance(ReportType, AccountsId, OpeningDr, OpeningCr, TransactionDr, TransactionCr, CompanyId, EntryBy) VALUES
		                (@ReportType, @Accounts_AccId, @Dr_OpeningBalanceAmount, @Cr_OpeningBalanceAmount, @Dr_TranAmount, @Cr_TranAmount, @CompanyId, @EntryBy)

		                FETCH NEXT FROM CursorAccounts INTO @Accounts_AccId, @Acc_BalanceType
		                SET @FETCH_STATUS_Accounts = @@FETCH_STATUS
	                END

	                CLOSE CursorAccounts
	                DEALLOCATE CursorAccounts
	                --END CURSOR FOR Accounts
                END");
            }

            //if stored procedure not found then create stored procedure
            if (!CheckTable("SP_PartyLedger"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE PROCEDURE SP_PartyLedger
                @Currency varchar(5),
                @CompanyId bigint,
                @DateFrom date,
                @DateTo date,
                @CustomerId bigint,
                @EntryBy bigint
                AS
                BEGIN
					DECLARE @CurrencyLevel int;
					DECLARE @OpeningBalanceAmount numeric(18, 4);
					DECLARE @Dr_Opening numeric(18, 4);
					DECLARE @Cr_Opening numeric(18, 4);

					-- Get company currency
					SELECT @CurrencyLevel = dbo.fn_CurrencyLevel(@CompanyId, @Currency)

					-- Clear temp table
					DELETE FROM Temp_PartyLedger WHERE CompanyId = @CompanyId AND EntryBy = @EntryBy

					SELECT @OpeningBalanceAmount = dbo.fn_CalculateOpeningBalanceForPartyLedger(@CurrencyLevel, @CustomerId, 0, @CompanyId, @DateFrom)
					IF @OpeningBalanceAmount >= 0
					BEGIN
						SET @Dr_Opening = @OpeningBalanceAmount
						SET @Cr_Opening = 0
					END
					ELSE
					BEGIN
						SET @Dr_Opening = 0
						SET @Cr_Opening = ABS(@OpeningBalanceAmount)
					END
					-- INSERT INTO TEMP TABLE AS OPENING BALANCE
					INSERT INTO Temp_PartyLedger (TempId, Date, Type, DrAmount, CrAmount, CustomerId, CompanyId, EntryBy)
					VALUES (NEWID(), @DateFrom, 'OB', @Dr_Opening, @Cr_Opening, @CustomerId, @CompanyId, @EntryBy)

					-- SALES
					INSERT INTO Temp_PartyLedger (TempId, Date, Type, Particular, DrAmount, CrAmount, CustomerId, CompanyId, EntryBy)
					SELECT NEWID(), InvoiceDate, 'Sales', InvoiceNo, (CASE WHEN @CurrencyLevel = 1 THEN (InvoiceAmount - InvoiceDiscount) WHEN @CurrencyLevel = 2 THEN (Invoice1Amount - Invoice1Discount) WHEN @CurrencyLevel = 3 THEN (Invoice2Amount - Invoice2Discount) END) AS DrAmount, 0 AS CrAmount, @CustomerId, @CompanyId, @EntryBy
					FROM Task_SalesInvoice WHERE (CustomerId = @CustomerId) AND (CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(InvoiceDate AS date) >= @DateFrom AND CAST(InvoiceDate AS date) <= @DateTo
				END");
            }

            //if stored procedure found then alter stored procedure
            if (CheckTable("SP_PartyLedger"))
            {
                _db.Database.ExecuteSqlCommand(@"ALTER PROCEDURE SP_PartyLedger
                @Currency varchar(5),
                @CompanyId bigint,
                @FromDate varchar(10),
                @ToDate varchar(10),
                @CustomerId bigint,
                @EntryBy bigint
                AS
                BEGIN
					DECLARE @CurrencyLevel int;
					DECLARE @SupplierId bigint;
					DECLARE @DateFrom date;
					DECLARE @DateTo date;
					DECLARE @OpeningBalanceAmount numeric(18, 4);
					DECLARE @Dr_Opening numeric(18, 4);
					DECLARE @Cr_Opening numeric(18, 4);

					SELECT @DateFrom = CONVERT(date, @FromDate, 101);
					SELECT @DateTo = CONVERT(date, @ToDate, 101);

					-- Get company currency
					SELECT @CurrencyLevel = dbo.fn_CurrencyLevel(@CompanyId, @Currency)

					-- Clear temp table
					DELETE FROM Temp_PartyLedger WHERE CompanyId = @CompanyId AND EntryBy = @EntryBy

					SELECT @SupplierId = COALESCE(SupplierId, 0) FROM Setup_Customer WHERE CustomerId = @CustomerId AND CompanyId = @CompanyId

					SELECT @OpeningBalanceAmount = dbo.fn_CalculateOpeningBalanceForPartyLedger(@CurrencyLevel, @CustomerId, @SupplierId, @CompanyId, @DateFrom)
					IF @OpeningBalanceAmount >= 0
					BEGIN
						SET @Dr_Opening = @OpeningBalanceAmount
						SET @Cr_Opening = 0
					END
					ELSE
					BEGIN
						SET @Dr_Opening = 0
						SET @Cr_Opening = ABS(@OpeningBalanceAmount)
					END
					-- INSERT INTO TEMP TABLE AS OPENING BALANCE
					INSERT INTO Temp_PartyLedger (TempId, Date, Type, DrAmount, CrAmount, CustomerId, CompanyId, EntryBy)
					VALUES (NEWID(), @DateFrom, 'OB', @Dr_Opening, @Cr_Opening, @CustomerId, @CompanyId, @EntryBy)

					-- CUSTOMER SECTION START
					-- SALES (+)
					INSERT INTO Temp_PartyLedger (TempId, Date, Type, OperationType, Particular, Particular1, DrAmount, CrAmount, CustomerId, CompanyId, EntryBy)
					SELECT NEWID(), InvoiceDate, 'Sales', InvoiceOperationType, InvoiceNo, (CASE WHEN IsSalesModeCash = 1 THEN 'Cash' WHEN IsSalesModeCash = 0 THEN 'Credit' END) + ' Sales Ref. No. # ' + ISNULL(ReferenceNo, ''), (CASE WHEN @CurrencyLevel = 1 THEN (InvoiceAmount - InvoiceDiscount + GovtDutyAmount + TotalChargeAmount) WHEN @CurrencyLevel = 2 THEN (Invoice1Amount - Invoice1Discount + GovtDuty1Amount + TotalChargeAmount1) WHEN @CurrencyLevel = 3 THEN (Invoice2Amount - Invoice2Discount + GovtDuty2Amount + TotalChargeAmount2) END) AS DrAmount, 0 AS CrAmount, @CustomerId, @CompanyId, @EntryBy
					FROM Task_SalesInvoice WHERE (CustomerId = @CustomerId) AND (CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(InvoiceDate AS date) >= @DateFrom AND CAST(InvoiceDate AS date) <= @DateTo

					-- COLLECTION (-)
					DECLARE @CollectionId UNIQUEIDENTIFIER;
					DECLARE @CollectionDate DATETIME;
					DECLARE @CollectionNo VARCHAR(50);
					DECLARE @MRNo VARCHAR(100);
					DECLARE @CrAmount NUMERIC(18, 4);
					DECLARE @FETCH_STATUS_ALL INT;

					DECLARE CursorCollection CURSOR FOR
					SELECT CollectionId, CollectionDate, CollectionNo, MRNo, (CASE WHEN @CurrencyLevel = 1 THEN CollectedAmount + SecurityDeposit + OthersAdjustment WHEN @CurrencyLevel = 2 THEN CollectedAmount1 + SecurityDeposit1 + OthersAdjustment1 WHEN @CurrencyLevel = 3 THEN CollectedAmount2 + SecurityDeposit2 + OthersAdjustment2 END) AS CrAmount
					FROM Task_Collection WHERE (CustomerId = @CustomerId) AND (CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(CollectionDate AS date) >= @DateFrom AND CAST(CollectionDate AS date) <= @DateTo
					
					OPEN CursorCollection
					FETCH NEXT FROM CursorCollection INTO @CollectionId, @CollectionDate, @CollectionNo, @MRNo, @CrAmount
					SET @FETCH_STATUS_ALL = @@FETCH_STATUS
					WHILE @FETCH_STATUS_ALL = 0
					BEGIN
						DECLARE @colMode VARCHAR(500);						
						DECLARE @CrAmount_Adj NUMERIC(18, 4);
						SET @colMode = CASE WHEN (SELECT COUNT(PaymentModeId) FROM Task_CollectionDetail WHERE (CollectionId = @CollectionId)) > 1 THEN 'Multiple Mode' ELSE (SELECT A.Name FROM Configuration_PaymentMode AS A INNER JOIN Task_CollectionDetail AS B ON A.PaymentModeId = B.PaymentModeId WHERE (B.CollectionId = @CollectionId)) END

						SELECT @CrAmount_Adj = COALESCE(SUM(CASE WHEN @CurrencyLevel = 1 THEN Amount WHEN @CurrencyLevel = 2 THEN Amount1 WHEN @CurrencyLevel = 3 THEN Amount2 END), 0)
						FROM Task_Collection_GovtDutyAdjustment WHERE (CollectionId = @CollectionId)

						INSERT INTO Temp_PartyLedger (TempId, Date, Type, Particular, Particular1, DrAmount, CrAmount, CustomerId, CompanyId, EntryBy) VALUES
						(NEWID(), @CollectionDate, 'Collection', @CollectionNo, @colMode + ' Collection Money Receipt. No. # ' + ISNULL(@MRNo, ''), 0, @CrAmount + @CrAmount_Adj, @CustomerId, @CompanyId, @EntryBy)

						FETCH NEXT FROM CursorCollection INTO @CollectionId, @CollectionDate, @CollectionNo, @MRNo, @CrAmount
						SET @FETCH_STATUS_ALL = @@FETCH_STATUS
					END
					CLOSE CursorCollection
					DEALLOCATE CursorCollection

					-- SALES RETURN (-)
					INSERT INTO Temp_PartyLedger (TempId, Date, Type, Particular, Particular1, DrAmount, CrAmount, CustomerId, CompanyId, EntryBy)
					SELECT NEWID(), ReturnDate, 'S.Return', ReturnNo, 'Sales Return', 0 AS DrAmount, (CASE WHEN @CurrencyLevel = 1 THEN ReturnAmount WHEN @CurrencyLevel = 2 THEN Return1Amount WHEN @CurrencyLevel = 3 THEN Return2Amount END) AS CrAmount, @CustomerId, @CompanyId, @EntryBy
					FROM Task_SalesReturn WHERE (CustomerId = @CustomerId) AND (CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(ReturnDate AS date) >= @DateFrom AND CAST(ReturnDate AS date) <= @DateTo

					-- Adjustment (+)
					INSERT INTO Temp_PartyLedger (TempId, Date, Type, Particular, Particular1, DrAmount, CrAmount, CustomerId, CompanyId, EntryBy)
					SELECT NEWID(), A.AdjustmentDate, 'C.Adjustment (+)', A.AdjustmentNo, 'C.Adjustment (+)', (CASE WHEN @CurrencyLevel = 1 THEN D.AdjustedAmount WHEN @CurrencyLevel = 2 THEN D.AdjustedAmount1 WHEN @CurrencyLevel = 3 THEN D.AdjustedAmount2 END) AS DrAmount, 0 AS CrAmount, @CustomerId, @CompanyId, @EntryBy
					FROM Task_PartyAdjustmentDetail D INNER JOIN Task_PartyAdjustment A ON D.AdjustmentId = A.AdjustmentId 
					WHERE (A.AdjustmentFor = 'C') AND (A.AdjustmentNature = 'A') AND (D.CustomerId = @CustomerId) AND (A.CompanyId = @CompanyId) AND (A.Approved = 'A') AND CAST(A.AdjustmentDate AS date) >= @DateFrom AND CAST(A.AdjustmentDate AS date) <= @DateTo

					-- Adjustment (-)
					INSERT INTO Temp_PartyLedger (TempId, Date, Type, Particular, Particular1, DrAmount, CrAmount, CustomerId, CompanyId, EntryBy)
					SELECT NEWID(), A.AdjustmentDate, 'C.Adjustment (-)', A.AdjustmentNo, 'C.Adjustment (-)', 0 AS DrAmount, (CASE WHEN @CurrencyLevel = 1 THEN D.AdjustedAmount WHEN @CurrencyLevel = 2 THEN D.AdjustedAmount1 WHEN @CurrencyLevel = 3 THEN D.AdjustedAmount2 END) AS CrAmount, @CustomerId, @CompanyId, @EntryBy
					FROM Task_PartyAdjustmentDetail D INNER JOIN Task_PartyAdjustment A ON D.AdjustmentId = A.AdjustmentId 
					WHERE (A.AdjustmentFor = 'C') AND (A.AdjustmentNature = 'D') AND (D.CustomerId = @CustomerId) AND (A.CompanyId = @CompanyId) AND (A.Approved = 'A') AND CAST(A.AdjustmentDate AS date) >= @DateFrom AND CAST(A.AdjustmentDate AS date) <= @DateTo

                    -- Balance Adjusted (+) From Cheque Treatment
					INSERT INTO Temp_PartyLedger (TempId, Date, Type, Particular, Particular1, DrAmount, CrAmount, CustomerId, CompanyId, EntryBy)
					SELECT NEWID(), Task_ChequeInfo.StatusDate, 'Balance Adjusted', Task_Collection.CollectionNo, 'Balance Adjusted', (CASE WHEN @CurrencyLevel = 1 THEN Task_ChequeInfo.Amount WHEN @CurrencyLevel = 2 THEN Task_ChequeInfo.Amount1 WHEN @CurrencyLevel = 3 THEN Task_ChequeInfo.Amount2 END) AS DrAmount, 0 AS CrAmount, @CustomerId, @CompanyId, @EntryBy
					FROM Task_ChequeInfo INNER JOIN Task_CollectionDetail ON Task_ChequeInfo.CollectionDetailId = Task_CollectionDetail.CollectionDetailId 
					INNER JOIN Task_Collection ON Task_CollectionDetail.CollectionId = Task_Collection.CollectionId 
					INNER JOIN Task_ChequeTreatment ON Task_ChequeInfo.ChequeInfoId = Task_ChequeTreatment.ChequeInfoId AND Task_ChequeInfo.Status = Task_ChequeTreatment.Status 
					INNER JOIN Task_Voucher ON Task_ChequeTreatment.VoucherId = Task_Voucher.VoucherId
					WHERE (Task_ChequeInfo.Status = 'B') AND (Task_Collection.CustomerId = @CustomerId) AND (Task_ChequeInfo.CompanyId = @CompanyId) AND (Task_Collection.Approved = 'A') AND CAST(Task_ChequeInfo.StatusDate AS date) >= @DateFrom AND CAST(Task_ChequeInfo.StatusDate AS date) <= @DateTo
					-- CUSTOMER SECTION END

					-- SUPPLIER SECTION START
					IF @SupplierId > 0
					BEGIN
						-- GOODS RECEIVE (-)
						INSERT INTO Temp_PartyLedger (TempId, Date, Type, OperationType, Particular, Particular1, CrAmount, DrAmount, CustomerId, CompanyId, EntryBy)
						SELECT NEWID(), ReceiveDate, 'Purchase', PurchaseOperationType, ReceiveNo, 'Purchase Ref. No ' + ISNULL(ReferenceNo, ''), (CASE WHEN @CurrencyLevel = 1 THEN (ReceiveAmount - DiscountAmount) WHEN @CurrencyLevel = 2 THEN (Receive1Amount - Discount1Amount) WHEN @CurrencyLevel = 3 THEN (Receive2Amount - Discount2Amount) END) AS CrAmount, 0 AS DrAmount, @CustomerId, @CompanyId, @EntryBy
						FROM Task_GoodsReceive WHERE (SupplierId = @SupplierId) AND (CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(ReceiveDate AS date) >= @DateFrom AND CAST(ReceiveDate AS date) <= @DateTo

						-- PAYMENT (+)
						DECLARE @PaymentId UNIQUEIDENTIFIER;
						DECLARE @PaymentDate DATETIME;
						DECLARE @PaymentNo VARCHAR(50);
						DECLARE @ReferenceNo VARCHAR(100);
						DECLARE @DrAmount NUMERIC(18, 4);

						DECLARE CursorPayment CURSOR FOR
						SELECT PaymentId, PaymentDate, PaymentNo, ReferenceNo, (CASE WHEN @CurrencyLevel = 1 THEN PaidAmount + SecurityDeposit + OthersAdjustment WHEN @CurrencyLevel = 2 THEN PaidAmount1 + SecurityDeposit1 + OthersAdjustment1 WHEN @CurrencyLevel = 3 THEN PaidAmount2 + SecurityDeposit2 + OthersAdjustment2 END) AS DrAmount
						FROM Task_Payment WHERE (SupplierId = @SupplierId) AND (CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(PaymentDate AS date) >= @DateFrom AND CAST(PaymentDate AS date) <= @DateTo
						
						OPEN CursorPayment
						FETCH NEXT FROM CursorPayment INTO @PaymentId, @PaymentDate, @PaymentNo, @ReferenceNo, @DrAmount
						SET @FETCH_STATUS_ALL = @@FETCH_STATUS
						WHILE @FETCH_STATUS_ALL = 0
						BEGIN
							DECLARE @payMode VARCHAR(500);
							DECLARE @DrAmount_Adj NUMERIC(18, 4);
							SET @payMode = CASE WHEN (SELECT COUNT(PaymentModeId) FROM Task_PaymentDetail WHERE (PaymentId = @PaymentId)) > 1 THEN 'Multiple Mode' ELSE (SELECT A.Name FROM Configuration_PaymentMode AS A INNER JOIN Task_PaymentDetail AS B ON A.PaymentModeId = B.PaymentModeId WHERE (B.PaymentId = @PaymentId)) END

							SELECT @DrAmount_Adj = COALESCE(SUM(CASE WHEN @CurrencyLevel = 1 THEN Amount WHEN @CurrencyLevel = 2 THEN Amount1 WHEN @CurrencyLevel = 3 THEN Amount2 END), 0)
							FROM Task_Payment_GovtDutyAdjustment WHERE (PaymentId = @PaymentId)

							INSERT INTO Temp_PartyLedger (TempId, Date, Type, Particular, Particular1, DrAmount, CrAmount, CustomerId, CompanyId, EntryBy) VALUES
							(NEWID(), @PaymentDate, 'Payment', @PaymentNo, @payMode + ' Payment Reference No. # ' + ISNULL(@ReferenceNo, ''), @DrAmount + @DrAmount_Adj, 0, @CustomerId, @CompanyId, @EntryBy)

							FETCH NEXT FROM CursorPayment INTO @PaymentId, @PaymentDate, @PaymentNo, @ReferenceNo, @DrAmount
							SET @FETCH_STATUS_ALL = @@FETCH_STATUS
						END
						CLOSE CursorPayment
						DEALLOCATE CursorPayment

						-- PURCHASE RETURN (+)
						INSERT INTO Temp_PartyLedger (TempId, Date, Type, Particular, Particular1, CrAmount, DrAmount, CustomerId, CompanyId, EntryBy)
						SELECT NEWID(), ReturnDate, 'P.Return', ReturnNo, 'Purchase Return', 0 AS CrAmount, (CASE WHEN @CurrencyLevel = 1 THEN ReturnAmount WHEN @CurrencyLevel = 2 THEN Return1Amount WHEN @CurrencyLevel = 3 THEN Return2Amount END) AS DrAmount, @CustomerId, @CompanyId, @EntryBy
						FROM Task_PurchaseReturn WHERE (SupplierId = @SupplierId) AND (CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(ReturnDate AS date) >= @DateFrom AND CAST(ReturnDate AS date) <= @DateTo

						-- Adjustment (+)
						INSERT INTO Temp_PartyLedger (TempId, Date, Type, Particular, Particular1, CrAmount, DrAmount, CustomerId, CompanyId, EntryBy)
						SELECT NEWID(), A.AdjustmentDate, 'S.Adjustment (+)', A.AdjustmentNo, 'S.Adjustment (+)', (CASE WHEN @CurrencyLevel = 1 THEN D.AdjustedAmount WHEN @CurrencyLevel = 2 THEN D.AdjustedAmount1 WHEN @CurrencyLevel = 3 THEN D.AdjustedAmount2 END) AS CrAmount, 0 AS DrAmount, @CustomerId, @CompanyId, @EntryBy
						FROM Task_PartyAdjustmentDetail D INNER JOIN Task_PartyAdjustment A ON D.AdjustmentId = A.AdjustmentId 
						WHERE (A.AdjustmentFor = 'S') AND (A.AdjustmentNature = 'A') AND (D.SupplierId = @SupplierId) AND (A.CompanyId = @CompanyId) AND (A.Approved = 'A') AND CAST(A.AdjustmentDate AS date) >= @DateFrom AND CAST(A.AdjustmentDate AS date) <= @DateTo

						-- Adjustment (-)
						INSERT INTO Temp_PartyLedger (TempId, Date, Type, Particular, Particular1, CrAmount, DrAmount, CustomerId, CompanyId, EntryBy)
						SELECT NEWID(), A.AdjustmentDate, 'S.Adjustment (-)', A.AdjustmentNo, 'S.Adjustment (-)', 0 AS CrAmount, (CASE WHEN @CurrencyLevel = 1 THEN D.AdjustedAmount WHEN @CurrencyLevel = 2 THEN D.AdjustedAmount1 WHEN @CurrencyLevel = 3 THEN D.AdjustedAmount2 END) AS DrAmount, @CustomerId, @CompanyId, @EntryBy
						FROM Task_PartyAdjustmentDetail D INNER JOIN Task_PartyAdjustment A ON D.AdjustmentId = A.AdjustmentId 
						WHERE (A.AdjustmentFor = 'S') AND (A.AdjustmentNature = 'D') AND (D.SupplierId = @SupplierId) AND (A.CompanyId = @CompanyId) AND (A.Approved = 'A') AND CAST(A.AdjustmentDate AS date) >= @DateFrom AND CAST(A.AdjustmentDate AS date) <= @DateTo

                        -- Balance Adjusted (-) From Cheque Treatment
						INSERT INTO Temp_PartyLedger (TempId, Date, Type, Particular, Particular1, CrAmount, DrAmount, CustomerId, CompanyId, EntryBy)
						SELECT NEWID(), Task_ChequeInfo.StatusDate, 'Balance Adjusted', Task_Payment.PaymentNo, 'Balance Adjusted', (CASE WHEN @CurrencyLevel = 1 THEN Task_ChequeInfo.Amount WHEN @CurrencyLevel = 2 THEN Task_ChequeInfo.Amount1 WHEN @CurrencyLevel = 3 THEN Task_ChequeInfo.Amount2 END) AS CrAmount, 0 AS DrAmount, @CustomerId, @CompanyId, @EntryBy
						FROM Task_ChequeInfo INNER JOIN Task_PaymentDetail ON Task_ChequeInfo.PaymentDetailId = Task_PaymentDetail.PaymentDetailId 
						INNER JOIN Task_Payment ON Task_PaymentDetail.PaymentId = Task_Payment.PaymentId 
						INNER JOIN Task_ChequeTreatment ON Task_ChequeInfo.ChequeInfoId = Task_ChequeTreatment.ChequeInfoId AND Task_ChequeInfo.Status = Task_ChequeTreatment.Status 
						INNER JOIN Task_Voucher ON Task_ChequeTreatment.VoucherId = Task_Voucher.VoucherId
						WHERE (Task_ChequeInfo.Status = 'B') AND (Task_Payment.SupplierId = @SupplierId) AND (Task_ChequeInfo.CompanyId = @CompanyId) AND (Task_Payment.Approved = 'A') AND CAST(Task_ChequeInfo.StatusDate AS date) >= @DateFrom AND CAST(Task_ChequeInfo.StatusDate AS date) <= @DateTo
					END
					-- SUPPLIER SECTION END
				END");
            }

            //if stored procedure not found then create stored procedure
            if (!CheckTable("SP_SupplierLedger"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE PROCEDURE SP_SupplierLedger
                @Currency varchar(5),
                @CompanyId bigint,
                @DateFrom date,
                @DateTo date,
                @SupplierId bigint,
                @EntryBy bigint
                AS
                BEGIN
					DECLARE @CurrencyLevel int;
					DECLARE @OpeningBalanceAmount numeric(18, 4);
					DECLARE @Dr_Opening numeric(18, 4);
					DECLARE @Cr_Opening numeric(18, 4);

					-- Get company currency
					SELECT @CurrencyLevel = dbo.fn_CurrencyLevel(@CompanyId, @Currency)

					-- Clear temp table
					DELETE FROM Temp_PartyLedger WHERE CompanyId = @CompanyId AND EntryBy = @EntryBy

					SELECT @OpeningBalanceAmount = dbo.fn_CalculateOpeningBalanceForSupplierLedger(@CurrencyLevel, @SupplierId, @CompanyId, @DateFrom)
					IF @OpeningBalanceAmount >= 0
					BEGIN
						SET @Dr_Opening = 0
						SET @Cr_Opening = @OpeningBalanceAmount
					END
					ELSE
					BEGIN
						SET @Dr_Opening = ABS(@OpeningBalanceAmount)
						SET @Cr_Opening = 0
					END
					-- INSERT INTO TEMP TABLE AS OPENING BALANCE
					INSERT INTO Temp_PartyLedger (TempId, Date, Type, DrAmount, CrAmount, SupplierId, CompanyId, EntryBy)
					VALUES (NEWID(), @DateFrom, 'OB', @Dr_Opening, @Cr_Opening, @SupplierId, @CompanyId, @EntryBy)

					-- RECEIVE FINALIZE
					INSERT INTO Temp_PartyLedger (TempId, Date, Type, Particular, CrAmount, DrAmount, SupplierId, CompanyId, EntryBy)
					SELECT NEWID(), FinalizeDate, 'Purchase', FinalizeNo, (CASE WHEN @CurrencyLevel = 1 THEN FinalizeAmount WHEN @CurrencyLevel = 2 THEN Finalize1Amount WHEN @CurrencyLevel = 3 THEN Finalize2Amount END) AS CrAmount, 0 AS DrAmount, @SupplierId, @CompanyId, @EntryBy
					FROM Task_ReceiveFinalize WHERE (SupplierId = @SupplierId) AND (CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(FinalizeDate AS date) >= @DateFrom AND CAST(FinalizeDate AS date) <= @DateTo

					-- PAYMENT
					INSERT INTO Temp_PartyLedger (TempId, Date, Type, Particular, CrAmount, DrAmount, SupplierId, CompanyId, EntryBy)
					SELECT NEWID(), PaymentDate, 'Payment', PaymentNo, 0 AS CrAmount, (CASE WHEN @CurrencyLevel = 1 THEN PaidAmount WHEN @CurrencyLevel = 2 THEN PaidAmount1 WHEN @CurrencyLevel = 3 THEN PaidAmount2 END) AS DrAmount, @SupplierId, @CompanyId, @EntryBy
					FROM Task_Payment WHERE (SupplierId = @SupplierId) AND (CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(PaymentDate AS date) >= @DateFrom AND CAST(PaymentDate AS date) <= @DateTo
				END");
            }

            //if stored procedure found then alter stored procedure
            if (CheckTable("SP_SupplierLedger"))
            {
                _db.Database.ExecuteSqlCommand(@"ALTER PROCEDURE SP_SupplierLedger
                @Currency varchar(5),
                @CompanyId bigint,
                @FromDate varchar(10),
                @ToDate varchar(10),
                @SupplierId bigint,
                @EntryBy bigint
                AS
                BEGIN
					DECLARE @CurrencyLevel int;
					DECLARE @DateFrom date;
					DECLARE @DateTo date;
					DECLARE @OpeningBalanceAmount numeric(18, 4);
					DECLARE @Dr_Opening numeric(18, 4);
					DECLARE @Cr_Opening numeric(18, 4);

					SELECT @DateFrom = CONVERT(date, @FromDate, 101);
					SELECT @DateTo = CONVERT(date, @ToDate, 101);

					-- Get company currency
					SELECT @CurrencyLevel = dbo.fn_CurrencyLevel(@CompanyId, @Currency)

					-- Clear temp table
					DELETE FROM Temp_PartyLedger WHERE CompanyId = @CompanyId AND EntryBy = @EntryBy

					SELECT @OpeningBalanceAmount = dbo.fn_CalculateOpeningBalanceForSupplierLedger(@CurrencyLevel, @SupplierId, @CompanyId, @DateFrom)
					IF @OpeningBalanceAmount >= 0
					BEGIN
						SET @Dr_Opening = 0
						SET @Cr_Opening = @OpeningBalanceAmount
					END
					ELSE
					BEGIN
						SET @Dr_Opening = ABS(@OpeningBalanceAmount)
						SET @Cr_Opening = 0
					END

					-- INSERT INTO TEMP TABLE AS OPENING BALANCE
					INSERT INTO Temp_PartyLedger (TempId, Date, Type, DrAmount, CrAmount, SupplierId, CompanyId, EntryBy)
					VALUES (NEWID(), @DateFrom, 'OB', @Dr_Opening, @Cr_Opening, @SupplierId, @CompanyId, @EntryBy)

					-- GOODS RECEIVE (+)
					INSERT INTO Temp_PartyLedger (TempId, Date, Type, OperationType, Particular, Particular1, CrAmount, DrAmount, SupplierId, CompanyId, EntryBy)
					SELECT NEWID(), ReceiveDate, 'Purchase', PurchaseOperationType, ReceiveNo, 'Purchase Ref. No ' + ISNULL(ReferenceNo, ''), (CASE WHEN @CurrencyLevel = 1 THEN (ReceiveAmount - DiscountAmount) WHEN @CurrencyLevel = 2 THEN (Receive1Amount - Discount1Amount) WHEN @CurrencyLevel = 3 THEN (Receive2Amount - Discount2Amount) END) AS CrAmount, 0 AS DrAmount, @SupplierId, @CompanyId, @EntryBy
					FROM Task_GoodsReceive WHERE (SupplierId = @SupplierId) AND (CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(ReceiveDate AS date) >= @DateFrom AND CAST(ReceiveDate AS date) <= @DateTo

					-- PAYMENT (-)
					DECLARE @PaymentId UNIQUEIDENTIFIER;
					DECLARE @PaymentDate DATETIME;
					DECLARE @PaymentNo VARCHAR(50);
					DECLARE @ReferenceNo VARCHAR(100);
					DECLARE @DrAmount NUMERIC(18, 4);
					DECLARE @FETCH_STATUS_ALL INT;

					DECLARE CursorPayment CURSOR FOR
					SELECT PaymentId, PaymentDate, PaymentNo, ReferenceNo, (CASE WHEN @CurrencyLevel = 1 THEN PaidAmount + SecurityDeposit + OthersAdjustment WHEN @CurrencyLevel = 2 THEN PaidAmount1 + SecurityDeposit1 + OthersAdjustment1 WHEN @CurrencyLevel = 3 THEN PaidAmount2 + SecurityDeposit2 + OthersAdjustment2 END) AS DrAmount
					FROM Task_Payment WHERE (SupplierId = @SupplierId) AND (CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(PaymentDate AS date) >= @DateFrom AND CAST(PaymentDate AS date) <= @DateTo

					OPEN CursorPayment
					FETCH NEXT FROM CursorPayment INTO @PaymentId, @PaymentDate, @PaymentNo, @ReferenceNo, @DrAmount
					SET @FETCH_STATUS_ALL = @@FETCH_STATUS
					WHILE @FETCH_STATUS_ALL = 0
					BEGIN
						DECLARE @payMode VARCHAR(500);
						DECLARE @DrAmount_Adj NUMERIC(18, 4);
						SET @payMode = CASE WHEN (SELECT COUNT(PaymentModeId) FROM Task_PaymentDetail WHERE (PaymentId = @PaymentId)) > 1 THEN 'Multiple Mode' ELSE (SELECT A.Name FROM Configuration_PaymentMode AS A INNER JOIN Task_PaymentDetail AS B ON A.PaymentModeId = B.PaymentModeId WHERE (B.PaymentId = @PaymentId)) END

						SELECT @DrAmount_Adj = COALESCE(SUM(CASE WHEN @CurrencyLevel = 1 THEN Amount WHEN @CurrencyLevel = 2 THEN Amount1 WHEN @CurrencyLevel = 3 THEN Amount2 END), 0)
						FROM Task_Payment_GovtDutyAdjustment WHERE (PaymentId = @PaymentId)
						
						INSERT INTO Temp_PartyLedger (TempId, Date, Type, Particular, Particular1, CrAmount, DrAmount, SupplierId, CompanyId, EntryBy) VALUES
						(NEWID(), @PaymentDate, 'Payment', @PaymentNo, @payMode + ' Payment Reference No. # ' + ISNULL(@ReferenceNo, ''), 0, @DrAmount, @SupplierId, @CompanyId, @EntryBy)

						FETCH NEXT FROM CursorPayment INTO @PaymentId, @PaymentDate, @PaymentNo, @ReferenceNo, @DrAmount
						SET @FETCH_STATUS_ALL = @@FETCH_STATUS
					END
					CLOSE CursorPayment
					DEALLOCATE CursorPayment

					-- PURCHASE RETURN (-)
					INSERT INTO Temp_PartyLedger (TempId, Date, Type, Particular, Particular1, CrAmount, DrAmount, SupplierId, CompanyId, EntryBy)
					SELECT NEWID(), ReturnDate, 'P.Return', ReturnNo, 'Purchase Return', 0 AS CrAmount, (CASE WHEN @CurrencyLevel = 1 THEN ReturnAmount WHEN @CurrencyLevel = 2 THEN Return1Amount WHEN @CurrencyLevel = 3 THEN Return2Amount END) AS DrAmount, @SupplierId, @CompanyId, @EntryBy
					FROM Task_PurchaseReturn WHERE (SupplierId = @SupplierId) AND (CompanyId = @CompanyId) AND (Approved = 'A') AND CAST(ReturnDate AS date) >= @DateFrom AND CAST(ReturnDate AS date) <= @DateTo

					-- ADJUSTMENT (+)
					INSERT INTO Temp_PartyLedger (TempId, Date, Type, Particular, Particular1, CrAmount, DrAmount, SupplierId, CompanyId, EntryBy)
					SELECT NEWID(), A.AdjustmentDate, 'S.Adjustment (+)', A.AdjustmentNo, 'S.Adjustment (+)', (CASE WHEN @CurrencyLevel = 1 THEN D.AdjustedAmount WHEN @CurrencyLevel = 2 THEN D.AdjustedAmount1 WHEN @CurrencyLevel = 3 THEN D.AdjustedAmount2 END) AS CrAmount, 0 AS DrAmount, @SupplierId, @CompanyId, @EntryBy
					FROM Task_PartyAdjustmentDetail D INNER JOIN Task_PartyAdjustment A ON D.AdjustmentId = A.AdjustmentId 
					WHERE (A.AdjustmentFor = 'S') AND (A.AdjustmentNature = 'A') AND (D.SupplierId = @SupplierId) AND (A.CompanyId = @CompanyId) AND (A.Approved = 'A') AND CAST(A.AdjustmentDate AS date) >= @DateFrom AND CAST(A.AdjustmentDate AS date) <= @DateTo

					-- ADJUSTMENT (-)
					INSERT INTO Temp_PartyLedger (TempId, Date, Type, Particular, Particular1, CrAmount, DrAmount, SupplierId, CompanyId, EntryBy)
					SELECT NEWID(), A.AdjustmentDate, 'S.Adjustment (-)', A.AdjustmentNo, 'S.Adjustment (-)', 0 AS CrAmount, (CASE WHEN @CurrencyLevel = 1 THEN D.AdjustedAmount WHEN @CurrencyLevel = 2 THEN D.AdjustedAmount1 WHEN @CurrencyLevel = 3 THEN D.AdjustedAmount2 END) AS DrAmount, @SupplierId, @CompanyId, @EntryBy
					FROM Task_PartyAdjustmentDetail D INNER JOIN Task_PartyAdjustment A ON D.AdjustmentId = A.AdjustmentId 
					WHERE (A.AdjustmentFor = 'S') AND (A.AdjustmentNature = 'D') AND (D.SupplierId = @SupplierId) AND (A.CompanyId = @CompanyId) AND (A.Approved = 'A') AND CAST(A.AdjustmentDate AS date) >= @DateFrom AND CAST(A.AdjustmentDate AS date) <= @DateTo

                    -- Balance Adjusted (+) From Cheque Treatment
					INSERT INTO Temp_PartyLedger (TempId, Date, Type, Particular, Particular1, CrAmount, DrAmount, SupplierId, CompanyId, EntryBy)
					SELECT NEWID(), Task_ChequeInfo.StatusDate, 'Balance Adjusted', Task_Payment.PaymentNo, 'Balance Adjusted', (CASE WHEN @CurrencyLevel = 1 THEN Task_ChequeInfo.Amount WHEN @CurrencyLevel = 2 THEN Task_ChequeInfo.Amount1 WHEN @CurrencyLevel = 3 THEN Task_ChequeInfo.Amount2 END) AS CrAmount, 0 AS DrAmount, @SupplierId, @CompanyId, @EntryBy
					FROM Task_ChequeInfo INNER JOIN Task_PaymentDetail ON Task_ChequeInfo.PaymentDetailId = Task_PaymentDetail.PaymentDetailId 
					INNER JOIN Task_Payment ON Task_PaymentDetail.PaymentId = Task_Payment.PaymentId 
					INNER JOIN Task_ChequeTreatment ON Task_ChequeInfo.ChequeInfoId = Task_ChequeTreatment.ChequeInfoId AND Task_ChequeInfo.Status = Task_ChequeTreatment.Status 
					INNER JOIN Task_Voucher ON Task_ChequeTreatment.VoucherId = Task_Voucher.VoucherId
					WHERE (Task_ChequeInfo.Status = 'B') AND (Task_Payment.SupplierId = @SupplierId) AND (Task_ChequeInfo.CompanyId = @CompanyId) AND (Task_Payment.Approved = 'A') AND CAST(Task_ChequeInfo.StatusDate AS date) >= @DateFrom AND CAST(Task_ChequeInfo.StatusDate AS date) <= @DateTo
				END");
            }

            //if stored procedure not found then create stored procedure
            if (!CheckTable("SP_CustomerSupplierOutstanding"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE PROCEDURE SP_CustomerSupplierOutstanding
                @Currency varchar(5),
	            @For varchar(1),
	            @CustomerSupplierGroupId bigint,
	            @CustomerSupplierId bigint,
	            @SalespersonId bigint,
	            @AsOnDate varchar(10),
	            @LocationId bigint,
	            @CompanyId bigint,
	            @EntryBy bigint
	            AS
	            BEGIN
		            -- @For = C (Customer/Combined), S (Supplier)
		            DECLARE @CurrencyLevel int;
		            DECLARE @FETCH_STATUS_ALL int;
		            DECLARE @CustomerId bigint;
		            DECLARE @SupplierId bigint;
		            DECLARE @Amount numeric(18, 4);
		            DECLARE @Amount1 numeric(18, 4);
		            DECLARE @Amount2 numeric(18, 4);
		            DECLARE @ClosingBalance numeric(18, 4);
		            DECLARE @ReceivedChequeNotTreatedAmount numeric(18, 4);
		            DECLARE @ReceivedDishonourChequeAmount numeric(18, 4);
		            DECLARE @IssuedChequeNotTreatedAmount numeric(18, 4);
		            DECLARE @IssuedDishonourChequeAmount numeric(18, 4);
		            DECLARE @AsOn date;
		            DECLARE @OpeningDate date;

		            SELECT @AsOn = DATEADD(d, 1, CONVERT(date, @AsOnDate, 101));

		            -- Get company currency
		            SELECT @CurrencyLevel = dbo.fn_CurrencyLevel(@CompanyId, @Currency)

		            -- Get Company Opening Date
		            SELECT @OpeningDate = OpeningDate FROM Setup_Company WHERE CompanyId = @CompanyId

		            -- Clear temp table
		            DELETE FROM Temp_CustomerSupplierOutstanding WHERE ReportFor = @For AND CompanyId = @CompanyId AND EntryBy = @EntryBy

		            -- START CUSTOMER/COMBINED
		            IF @For = 'C'
		            BEGIN
			            --START CURSOR FOR CustomerOrCombined
		                DECLARE CursorCustomerOrCombined CURSOR FOR
			            SELECT CustomerId, COALESCE(SupplierId, 0) AS SupplierId FROM Setup_Customer
			            WHERE CompanyId = @CompanyId
			            AND CASE WHEN @CustomerSupplierGroupId = 0 THEN 1 WHEN CustomerGroupId = @CustomerSupplierGroupId THEN 1 ELSE 0 END = 1
			            AND CASE WHEN @CustomerSupplierId = 0 THEN 1 WHEN CustomerId = @CustomerSupplierId THEN 1 ELSE 0 END = 1
			            AND CASE WHEN @SalespersonId = 0 THEN 1 WHEN SalesPersonId = @SalespersonId THEN 1 ELSE 0 END = 1

			            OPEN CursorCustomerOrCombined
		                FETCH NEXT FROM CursorCustomerOrCombined INTO @CustomerId, @SupplierId
		                SET @FETCH_STATUS_ALL = @@FETCH_STATUS
		                WHILE @FETCH_STATUS_ALL = 0
			            BEGIN
				            -- Closing Balance
				            SELECT @ClosingBalance = dbo.fn_CalculateOpeningBalanceForPartyLedger(@CurrencyLevel, @CustomerId, @SupplierId, @CompanyId, @AsOn)

				            -- Not Treated From Cheque Treatment For Customer
				            SET @ReceivedChequeNotTreatedAmount = 0
				            SET @Amount = 0 
				            SET @Amount1 = 0
				            SET @Amount2 = 0

				            SELECT @Amount = COALESCE(SUM(Task_ChequeInfo.Amount), 0), @Amount1 = COALESCE(SUM(Task_ChequeInfo.Amount1), 0), @Amount2 = COALESCE(SUM(Task_ChequeInfo.Amount2), 0)
				            FROM Task_ChequeInfo INNER JOIN Task_CollectionDetail ON Task_ChequeInfo.CollectionDetailId = Task_CollectionDetail.CollectionDetailId 
				            INNER JOIN Task_Collection ON Task_CollectionDetail.CollectionId = Task_Collection.CollectionId
				            WHERE (Task_Collection.Approved = 'A') AND CAST(Task_Collection.CollectionDate AS date) >= @OpeningDate AND CAST(Task_Collection.CollectionDate AS date) < @AsOn AND (Task_Collection.CustomerId = @CustomerId) AND (Task_ChequeInfo.Status = 'N') AND (Task_Collection.CompanyId = @CompanyId) --AND (Task_Collection.LocationId = @LocationId)

				            IF @CurrencyLevel = 1
				            BEGIN
					            SET @ReceivedChequeNotTreatedAmount = @ReceivedChequeNotTreatedAmount + @Amount
				            END
				            ELSE IF @CurrencyLevel = 2
				            BEGIN
					            SET @ReceivedChequeNotTreatedAmount = @ReceivedChequeNotTreatedAmount + @Amount1
				            END
				            ELSE
				            BEGIN
					            SET @ReceivedChequeNotTreatedAmount = @ReceivedChequeNotTreatedAmount + @Amount2
				            END
				
				            -- Dishonored From Cheque Treatment For Customer
				            SET @ReceivedDishonourChequeAmount = 0
				            SET @Amount = 0 
				            SET @Amount1 = 0
				            SET @Amount2 = 0

				            SELECT @Amount = COALESCE(SUM(Task_ChequeInfo.Amount), 0), @Amount1 = COALESCE(SUM(Task_ChequeInfo.Amount1), 0), @Amount2 = COALESCE(SUM(Task_ChequeInfo.Amount2), 0)
				            FROM Task_ChequeInfo INNER JOIN Task_CollectionDetail ON Task_ChequeInfo.CollectionDetailId = Task_CollectionDetail.CollectionDetailId 
				            INNER JOIN Task_Collection ON Task_CollectionDetail.CollectionId = Task_Collection.CollectionId
				            WHERE (Task_Collection.Approved = 'A') AND CAST(Task_Collection.CollectionDate AS date) >= @OpeningDate AND CAST(Task_Collection.CollectionDate AS date) < @AsOn AND (Task_Collection.CustomerId = @CustomerId) AND (Task_ChequeInfo.Status = 'D') AND (Task_Collection.CompanyId = @CompanyId) --AND (Task_Collection.LocationId = @LocationId)

				            IF @CurrencyLevel = 1
				            BEGIN
					            SET @ReceivedDishonourChequeAmount = @ReceivedDishonourChequeAmount + @Amount
				            END
				            ELSE IF @CurrencyLevel = 2
				            BEGIN
					            SET @ReceivedDishonourChequeAmount = @ReceivedDishonourChequeAmount + @Amount1
				            END
				            ELSE
				            BEGIN
					            SET @ReceivedDishonourChequeAmount = @ReceivedDishonourChequeAmount + @Amount2
				            END

				            -- Not Treated From Cheque Treatment For Supplier
				            SET @IssuedChequeNotTreatedAmount = 0
				            SET @Amount = 0 
				            SET @Amount1 = 0
				            SET @Amount2 = 0

				            SELECT @Amount = COALESCE(SUM(Task_ChequeInfo.Amount), 0), @Amount1 = COALESCE(SUM(Task_ChequeInfo.Amount1), 0), @Amount2 = COALESCE(SUM(Task_ChequeInfo.Amount2), 0)
				            FROM Task_ChequeInfo INNER JOIN Task_PaymentDetail ON Task_ChequeInfo.PaymentDetailId = Task_PaymentDetail.PaymentDetailId 
				            INNER JOIN Task_Payment ON Task_PaymentDetail.PaymentId = Task_Payment.PaymentId
				            WHERE (Task_Payment.Approved = 'A') AND CAST(Task_Payment.PaymentDate AS date) >= @OpeningDate AND CAST(Task_Payment.PaymentDate AS date) < @AsOn AND (Task_Payment.SupplierId = @SupplierId) AND (Task_ChequeInfo.Status = 'N') AND (Task_Payment.CompanyId = @CompanyId) --AND (Task_Payment.LocationId = @LocationId)

				            IF @CurrencyLevel = 1
				            BEGIN
					            SET @IssuedChequeNotTreatedAmount = @IssuedChequeNotTreatedAmount + @Amount
				            END
				            ELSE IF @CurrencyLevel = 2
				            BEGIN
					            SET @IssuedChequeNotTreatedAmount = @IssuedChequeNotTreatedAmount + @Amount1
				            END
				            ELSE
				            BEGIN
					            SET @IssuedChequeNotTreatedAmount = @IssuedChequeNotTreatedAmount + @Amount2
				            END
				
				            -- Dishonored From Cheque Treatment For Supplier
				            SET @IssuedDishonourChequeAmount = 0
				            SET @Amount = 0 
				            SET @Amount1 = 0
				            SET @Amount2 = 0

				            SELECT @Amount = COALESCE(SUM(Task_ChequeInfo.Amount), 0), @Amount1 = COALESCE(SUM(Task_ChequeInfo.Amount1), 0), @Amount2 = COALESCE(SUM(Task_ChequeInfo.Amount2), 0)
				            FROM Task_ChequeInfo INNER JOIN Task_PaymentDetail ON Task_ChequeInfo.PaymentDetailId = Task_PaymentDetail.PaymentDetailId 
				            INNER JOIN Task_Payment ON Task_PaymentDetail.PaymentId = Task_Payment.PaymentId
				            WHERE (Task_Payment.Approved = 'A') AND CAST(Task_Payment.PaymentDate AS date) >= @OpeningDate AND CAST(Task_Payment.PaymentDate AS date) < @AsOn AND (Task_Payment.SupplierId = @SupplierId) AND (Task_ChequeInfo.Status = 'D') AND (Task_Payment.CompanyId = @CompanyId) --AND (Task_Payment.LocationId = @LocationId)

				            IF @CurrencyLevel = 1
				            BEGIN
					            SET @IssuedDishonourChequeAmount = @IssuedDishonourChequeAmount + @Amount
				            END
				            ELSE IF @CurrencyLevel = 2
				            BEGIN
					            SET @IssuedDishonourChequeAmount = @IssuedDishonourChequeAmount + @Amount1
				            END
				            ELSE
				            BEGIN
					            SET @IssuedDishonourChequeAmount = @IssuedDishonourChequeAmount + @Amount2
				            END

				            -- Insert to Temp_CustomerSupplierOutstanding
				            INSERT INTO Temp_CustomerSupplierOutstanding (TempId, ReportFor, CustomerId, SupplierId, LedgerEndingBalance, ReceivedChequeNotTreatedAmount, ReceivedDishonourChequeAmount, IssuedChequeNotTreatedAmount, IssuedDishonourChequeAmount, CompanyId, EntryBy) VALUES 
				            (NEWID(), @For, @CustomerId, (CASE WHEN @SupplierId = 0 THEN NULL ELSE @SupplierId END), @ClosingBalance, @ReceivedChequeNotTreatedAmount, @ReceivedDishonourChequeAmount, @IssuedChequeNotTreatedAmount, @IssuedDishonourChequeAmount, @CompanyId, @EntryBy)

				            FETCH NEXT FROM CursorCustomerOrCombined INTO @CustomerId, @SupplierId
				            SET @FETCH_STATUS_ALL = @@FETCH_STATUS
			            END

			            CLOSE CursorCustomerOrCombined
		                DEALLOCATE CursorCustomerOrCombined
		                --END CURSOR FOR CustomerOrCombined
		            END
		            -- END CUSTOMER/COMBINED

		            -- START ONLY SUPPLIER
		            ELSE
		            BEGIN
			            --START CURSOR FOR CursorSupplier
		                DECLARE CursorSupplier CURSOR FOR
			            SELECT SupplierId FROM Setup_Supplier
			            WHERE CompanyId = @CompanyId AND SupplierId NOT IN (SELECT SupplierId FROM Setup_Customer WHERE SupplierId IS NOT NULL AND CompanyId = @CompanyId)
			            AND CASE WHEN @CustomerSupplierGroupId = 0 THEN 1 WHEN SupplierGroupId = @CustomerSupplierGroupId THEN 1 ELSE 0 END = 1
			            AND CASE WHEN @CustomerSupplierId = 0 THEN 1 WHEN SupplierId = @CustomerSupplierId THEN 1 ELSE 0 END = 1

			            OPEN CursorSupplier
		                FETCH NEXT FROM CursorSupplier INTO @SupplierId
		                SET @FETCH_STATUS_ALL = @@FETCH_STATUS
		                WHILE @FETCH_STATUS_ALL = 0
			            BEGIN
				            -- Closing Balance
				            SELECT @ClosingBalance = dbo.fn_CalculateOpeningBalanceForSupplierLedger(@CurrencyLevel, @SupplierId, @CompanyId, @AsOn)

				            -- Not Treated From Cheque Treatment For Supplier
				            SET @IssuedChequeNotTreatedAmount = 0
				            SET @Amount = 0 
				            SET @Amount1 = 0
				            SET @Amount2 = 0

				            SELECT @Amount = COALESCE(SUM(Task_ChequeInfo.Amount), 0), @Amount1 = COALESCE(SUM(Task_ChequeInfo.Amount1), 0), @Amount2 = COALESCE(SUM(Task_ChequeInfo.Amount2), 0)
				            FROM Task_ChequeInfo INNER JOIN Task_PaymentDetail ON Task_ChequeInfo.PaymentDetailId = Task_PaymentDetail.PaymentDetailId 
				            INNER JOIN Task_Payment ON Task_PaymentDetail.PaymentId = Task_Payment.PaymentId
				            WHERE (Task_Payment.Approved = 'A') AND CAST(Task_Payment.PaymentDate AS date) >= @OpeningDate AND CAST(Task_Payment.PaymentDate AS date) < @AsOn AND (Task_Payment.SupplierId = @SupplierId) AND (Task_ChequeInfo.Status = 'N') AND (Task_Payment.CompanyId = @CompanyId) --AND (Task_Payment.LocationId = @LocationId)

				            IF @CurrencyLevel = 1
				            BEGIN
					            SET @IssuedChequeNotTreatedAmount = @IssuedChequeNotTreatedAmount + @Amount
				            END
				            ELSE IF @CurrencyLevel = 2
				            BEGIN
					            SET @IssuedChequeNotTreatedAmount = @IssuedChequeNotTreatedAmount + @Amount1
				            END
				            ELSE
				            BEGIN
					            SET @IssuedChequeNotTreatedAmount = @IssuedChequeNotTreatedAmount + @Amount2
				            END
				
				            -- Dishonored From Cheque Treatment For Supplier
				            SET @IssuedDishonourChequeAmount = 0
				            SET @Amount = 0 
				            SET @Amount1 = 0
				            SET @Amount2 = 0

				            SELECT @Amount = COALESCE(SUM(Task_ChequeInfo.Amount), 0), @Amount1 = COALESCE(SUM(Task_ChequeInfo.Amount1), 0), @Amount2 = COALESCE(SUM(Task_ChequeInfo.Amount2), 0)
				            FROM Task_ChequeInfo INNER JOIN Task_PaymentDetail ON Task_ChequeInfo.PaymentDetailId = Task_PaymentDetail.PaymentDetailId 
				            INNER JOIN Task_Payment ON Task_PaymentDetail.PaymentId = Task_Payment.PaymentId
				            WHERE (Task_Payment.Approved = 'A') AND CAST(Task_Payment.PaymentDate AS date) >= @OpeningDate AND CAST(Task_Payment.PaymentDate AS date) < @AsOn AND (Task_Payment.SupplierId = @SupplierId) AND (Task_ChequeInfo.Status = 'D') AND (Task_Payment.CompanyId = @CompanyId) --AND (Task_Payment.LocationId = @LocationId)

				            IF @CurrencyLevel = 1
				            BEGIN
					            SET @IssuedDishonourChequeAmount = @IssuedDishonourChequeAmount + @Amount
				            END
				            ELSE IF @CurrencyLevel = 2
				            BEGIN
					            SET @IssuedDishonourChequeAmount = @IssuedDishonourChequeAmount + @Amount1
				            END
				            ELSE
				            BEGIN
					            SET @IssuedDishonourChequeAmount = @IssuedDishonourChequeAmount + @Amount2
				            END

				            -- Insert to Temp_CustomerSupplierOutstanding
				            INSERT INTO Temp_CustomerSupplierOutstanding (TempId, ReportFor, SupplierId, LedgerEndingBalance, IssuedChequeNotTreatedAmount, IssuedDishonourChequeAmount, CompanyId, EntryBy) VALUES 
				            (NEWID(), @For, @SupplierId, @ClosingBalance, @IssuedChequeNotTreatedAmount, @IssuedDishonourChequeAmount, @CompanyId, @EntryBy)

				            FETCH NEXT FROM CursorSupplier INTO @SupplierId
				            SET @FETCH_STATUS_ALL = @@FETCH_STATUS
			            END

			            CLOSE CursorSupplier
		                DEALLOCATE CursorSupplier
		                --END CURSOR FOR CustomerOrCombined
		            END
		            -- END ONLY SUPPLIER
	            END");
            }

            //if stored procedure found then alter stored procedure
            if (CheckTable("SP_CustomerSupplierOutstanding"))
            {
                _db.Database.ExecuteSqlCommand(@"ALTER PROCEDURE SP_CustomerSupplierOutstanding
                @Currency varchar(5),
	            @For varchar(1),
	            @CustomerSupplierGroupId bigint,
	            @CustomerSupplierId bigint,
	            @SalespersonId bigint,
	            @AsOnDate varchar(10),
	            @LocationId bigint,
	            @CompanyId bigint,
	            @EntryBy bigint
	            AS
	            BEGIN
		            -- @For = C (Customer/Combined), S (Supplier)
		            DECLARE @CurrencyLevel int;
		            DECLARE @FETCH_STATUS_ALL int;
		            DECLARE @CustomerId bigint;
		            DECLARE @SupplierId bigint;
		            DECLARE @Amount numeric(18, 4);
		            DECLARE @Amount1 numeric(18, 4);
		            DECLARE @Amount2 numeric(18, 4);
		            DECLARE @ClosingBalance numeric(18, 4);
		            DECLARE @ReceivedChequeNotTreatedAmount numeric(18, 4);
		            DECLARE @ReceivedDishonourChequeAmount numeric(18, 4);
		            DECLARE @IssuedChequeNotTreatedAmount numeric(18, 4);
		            DECLARE @IssuedDishonourChequeAmount numeric(18, 4);
					DECLARE @GroupName varchar(50);
					DECLARE @CustomerName varchar(400);
					DECLARE @SupplierName varchar(400);
					DECLARE @SalesPerson varchar(400);
		            DECLARE @AsOn date;
		            DECLARE @OpeningDate date;

		            SELECT @AsOn = DATEADD(d, 1, CONVERT(date, @AsOnDate, 101));

		            -- Get company currency
		            SELECT @CurrencyLevel = dbo.fn_CurrencyLevel(@CompanyId, @Currency)

		            -- Get Company Opening Date
		            SELECT @OpeningDate = OpeningDate FROM Setup_Company WHERE CompanyId = @CompanyId

		            -- Clear temp table
		            DELETE FROM Temp_CustomerSupplierOutstanding WHERE CompanyId = @CompanyId 
		            AND EntryBy = @EntryBy
		            AND CASE WHEN @For = 'A' THEN 1 WHEN ReportFor = @For THEN 1 ELSE 0 END = 1

		            -- START CUSTOMER/COMBINED
		            IF @For = 'C' OR @For = 'A'
		            BEGIN
			            --START CURSOR FOR CustomerOrCombined
		                DECLARE CursorCustomerOrCombined CURSOR FOR
			            SELECT Setup_CustomerGroup.Name, Setup_Customer.CustomerId, Setup_Customer.Code + ' # ' + Setup_Customer.Name, COALESCE(Setup_Customer.SupplierId, 0) AS SupplierId, Setup_Employee.Code + ' # ' + Setup_Employee.Name AS SalesPerson 
						FROM Setup_Customer INNER JOIN Setup_CustomerGroup ON Setup_Customer.CustomerGroupId = Setup_CustomerGroup.CustomerGroupId
						INNER JOIN Setup_Employee ON Setup_Customer.SalesPersonId = Setup_Employee.EmployeeId
			            WHERE Setup_Customer.IsActive = 1 AND Setup_Customer.CompanyId = @CompanyId
			            AND CASE WHEN @CustomerSupplierGroupId = 0 THEN 1 WHEN Setup_Customer.CustomerGroupId = @CustomerSupplierGroupId THEN 1 ELSE 0 END = 1
			            AND CASE WHEN @CustomerSupplierId = 0 THEN 1 WHEN Setup_Customer.CustomerId = @CustomerSupplierId THEN 1 ELSE 0 END = 1
			            AND CASE WHEN @SalespersonId = 0 THEN 1 WHEN Setup_Customer.SalesPersonId = @SalespersonId THEN 1 ELSE 0 END = 1

			            OPEN CursorCustomerOrCombined
		                FETCH NEXT FROM CursorCustomerOrCombined INTO @GroupName, @CustomerId, @CustomerName, @SupplierId, @SalesPerson
		                SET @FETCH_STATUS_ALL = @@FETCH_STATUS
		                WHILE @FETCH_STATUS_ALL = 0
			            BEGIN
				            -- Closing Balance
				            SELECT @ClosingBalance = dbo.fn_CalculateOpeningBalanceForPartyLedger(@CurrencyLevel, @CustomerId, @SupplierId, @CompanyId, @AsOn)

				            -- Not Treated From Cheque Treatment For Customer
				            SET @ReceivedChequeNotTreatedAmount = 0
				            SET @Amount = 0 
				            SET @Amount1 = 0
				            SET @Amount2 = 0

				            SELECT @Amount = COALESCE(SUM(Task_ChequeInfo.Amount), 0), @Amount1 = COALESCE(SUM(Task_ChequeInfo.Amount1), 0), @Amount2 = COALESCE(SUM(Task_ChequeInfo.Amount2), 0)
				            FROM Task_ChequeInfo INNER JOIN Task_CollectionDetail ON Task_ChequeInfo.CollectionDetailId = Task_CollectionDetail.CollectionDetailId 
				            INNER JOIN Task_Collection ON Task_CollectionDetail.CollectionId = Task_Collection.CollectionId
				            WHERE (Task_Collection.Approved = 'A') AND CAST(Task_Collection.CollectionDate AS date) >= @OpeningDate AND CAST(Task_Collection.CollectionDate AS date) < @AsOn AND (Task_Collection.CustomerId = @CustomerId) AND (Task_ChequeInfo.Status = 'N') AND (Task_Collection.CompanyId = @CompanyId) --AND (Task_Collection.LocationId = @LocationId)

				            IF @CurrencyLevel = 1
				            BEGIN
					            SET @ReceivedChequeNotTreatedAmount = @ReceivedChequeNotTreatedAmount + @Amount
				            END
				            ELSE IF @CurrencyLevel = 2
				            BEGIN
					            SET @ReceivedChequeNotTreatedAmount = @ReceivedChequeNotTreatedAmount + @Amount1
				            END
				            ELSE
				            BEGIN
					            SET @ReceivedChequeNotTreatedAmount = @ReceivedChequeNotTreatedAmount + @Amount2
				            END
				
				            -- Dishonored From Cheque Treatment For Customer
				            SET @ReceivedDishonourChequeAmount = 0
				            SET @Amount = 0 
				            SET @Amount1 = 0
				            SET @Amount2 = 0

				            SELECT @Amount = COALESCE(SUM(Task_ChequeInfo.Amount), 0), @Amount1 = COALESCE(SUM(Task_ChequeInfo.Amount1), 0), @Amount2 = COALESCE(SUM(Task_ChequeInfo.Amount2), 0)
				            FROM Task_ChequeInfo INNER JOIN Task_CollectionDetail ON Task_ChequeInfo.CollectionDetailId = Task_CollectionDetail.CollectionDetailId 
				            INNER JOIN Task_Collection ON Task_CollectionDetail.CollectionId = Task_Collection.CollectionId
				            WHERE (Task_Collection.Approved = 'A') AND CAST(Task_Collection.CollectionDate AS date) >= @OpeningDate AND CAST(Task_Collection.CollectionDate AS date) < @AsOn AND (Task_Collection.CustomerId = @CustomerId) AND (Task_ChequeInfo.Status = 'D') AND (Task_Collection.CompanyId = @CompanyId) --AND (Task_Collection.LocationId = @LocationId)

				            IF @CurrencyLevel = 1
				            BEGIN
					            SET @ReceivedDishonourChequeAmount = @ReceivedDishonourChequeAmount + @Amount
				            END
				            ELSE IF @CurrencyLevel = 2
				            BEGIN
					            SET @ReceivedDishonourChequeAmount = @ReceivedDishonourChequeAmount + @Amount1
				            END
				            ELSE
				            BEGIN
					            SET @ReceivedDishonourChequeAmount = @ReceivedDishonourChequeAmount + @Amount2
				            END

				            -- Not Treated From Cheque Treatment For Supplier
				            SET @IssuedChequeNotTreatedAmount = 0
				            SET @Amount = 0 
				            SET @Amount1 = 0
				            SET @Amount2 = 0

				            SELECT @Amount = COALESCE(SUM(Task_ChequeInfo.Amount), 0), @Amount1 = COALESCE(SUM(Task_ChequeInfo.Amount1), 0), @Amount2 = COALESCE(SUM(Task_ChequeInfo.Amount2), 0)
				            FROM Task_ChequeInfo INNER JOIN Task_PaymentDetail ON Task_ChequeInfo.PaymentDetailId = Task_PaymentDetail.PaymentDetailId 
				            INNER JOIN Task_Payment ON Task_PaymentDetail.PaymentId = Task_Payment.PaymentId
				            WHERE (Task_Payment.Approved = 'A') AND CAST(Task_Payment.PaymentDate AS date) >= @OpeningDate AND CAST(Task_Payment.PaymentDate AS date) < @AsOn AND (Task_Payment.SupplierId = @SupplierId) AND (Task_ChequeInfo.Status = 'N') AND (Task_Payment.CompanyId = @CompanyId) --AND (Task_Payment.LocationId = @LocationId)

				            IF @CurrencyLevel = 1
				            BEGIN
					            SET @IssuedChequeNotTreatedAmount = @IssuedChequeNotTreatedAmount + @Amount
				            END
				            ELSE IF @CurrencyLevel = 2
				            BEGIN
					            SET @IssuedChequeNotTreatedAmount = @IssuedChequeNotTreatedAmount + @Amount1
				            END
				            ELSE
				            BEGIN
					            SET @IssuedChequeNotTreatedAmount = @IssuedChequeNotTreatedAmount + @Amount2
				            END
				
				            -- Dishonored From Cheque Treatment For Supplier
				            SET @IssuedDishonourChequeAmount = 0
				            SET @Amount = 0 
				            SET @Amount1 = 0
				            SET @Amount2 = 0

				            SELECT @Amount = COALESCE(SUM(Task_ChequeInfo.Amount), 0), @Amount1 = COALESCE(SUM(Task_ChequeInfo.Amount1), 0), @Amount2 = COALESCE(SUM(Task_ChequeInfo.Amount2), 0)
				            FROM Task_ChequeInfo INNER JOIN Task_PaymentDetail ON Task_ChequeInfo.PaymentDetailId = Task_PaymentDetail.PaymentDetailId 
				            INNER JOIN Task_Payment ON Task_PaymentDetail.PaymentId = Task_Payment.PaymentId
				            WHERE (Task_Payment.Approved = 'A') AND CAST(Task_Payment.PaymentDate AS date) >= @OpeningDate AND CAST(Task_Payment.PaymentDate AS date) < @AsOn AND (Task_Payment.SupplierId = @SupplierId) AND (Task_ChequeInfo.Status = 'D') AND (Task_Payment.CompanyId = @CompanyId) --AND (Task_Payment.LocationId = @LocationId)

				            IF @CurrencyLevel = 1
				            BEGIN
					            SET @IssuedDishonourChequeAmount = @IssuedDishonourChequeAmount + @Amount
				            END
				            ELSE IF @CurrencyLevel = 2
				            BEGIN
					            SET @IssuedDishonourChequeAmount = @IssuedDishonourChequeAmount + @Amount1
				            END
				            ELSE
				            BEGIN
					            SET @IssuedDishonourChequeAmount = @IssuedDishonourChequeAmount + @Amount2
				            END

				            -- Insert to Temp_CustomerSupplierOutstanding
				            INSERT INTO Temp_CustomerSupplierOutstanding (TempId, ReportFor, [Group], CustomerId, SupplierId, Name, LedgerEndingBalance, ReceivedChequeNotTreatedAmount, ReceivedDishonourChequeAmount, IssuedChequeNotTreatedAmount, IssuedDishonourChequeAmount, SalesPerson, CompanyId, EntryBy) VALUES 
				            (NEWID(), 'C', @GroupName, @CustomerId, (CASE WHEN @SupplierId = 0 THEN NULL ELSE @SupplierId END), @CustomerName, @ClosingBalance, @ReceivedChequeNotTreatedAmount, @ReceivedDishonourChequeAmount, @IssuedChequeNotTreatedAmount, @IssuedDishonourChequeAmount, @SalesPerson, @CompanyId, @EntryBy)

				            FETCH NEXT FROM CursorCustomerOrCombined INTO @GroupName, @CustomerId, @CustomerName, @SupplierId, @SalesPerson
				            SET @FETCH_STATUS_ALL = @@FETCH_STATUS
			            END

			            CLOSE CursorCustomerOrCombined
		                DEALLOCATE CursorCustomerOrCombined
		                --END CURSOR FOR CustomerOrCombined
		            END
		            -- END CUSTOMER/COMBINED

		            -- START ONLY SUPPLIER
		            IF @For = 'S' OR @For = 'A'
		            BEGIN
			            --START CURSOR FOR CursorSupplier
		                DECLARE CursorSupplier CURSOR FOR
			            SELECT Setup_SupplierGroup.Name, Setup_Supplier.SupplierId, Setup_Supplier.Code + ' # ' + Setup_Supplier.Name 
						FROM Setup_Supplier INNER JOIN Setup_SupplierGroup ON Setup_Supplier.SupplierGroupId = Setup_SupplierGroup.SupplierGroupId
			            WHERE Setup_Supplier.IsActive = 1 AND Setup_Supplier.CompanyId = @CompanyId AND Setup_Supplier.SupplierId NOT IN (SELECT SupplierId FROM Setup_Customer WHERE SupplierId IS NOT NULL AND CompanyId = @CompanyId)
			            AND CASE WHEN @CustomerSupplierGroupId = 0 THEN 1 WHEN Setup_Supplier.SupplierGroupId = @CustomerSupplierGroupId THEN 1 ELSE 0 END = 1
			            AND CASE WHEN @CustomerSupplierId = 0 THEN 1 WHEN Setup_Supplier.SupplierId = @CustomerSupplierId THEN 1 ELSE 0 END = 1

			            OPEN CursorSupplier
		                FETCH NEXT FROM CursorSupplier INTO @GroupName, @SupplierId, @SupplierName
		                SET @FETCH_STATUS_ALL = @@FETCH_STATUS
		                WHILE @FETCH_STATUS_ALL = 0
			            BEGIN
				            -- Closing Balance
				            SELECT @ClosingBalance = dbo.fn_CalculateOpeningBalanceForSupplierLedger(@CurrencyLevel, @SupplierId, @CompanyId, @AsOn)

				            -- Not Treated From Cheque Treatment For Supplier
				            SET @IssuedChequeNotTreatedAmount = 0
				            SET @Amount = 0 
				            SET @Amount1 = 0
				            SET @Amount2 = 0

				            SELECT @Amount = COALESCE(SUM(Task_ChequeInfo.Amount), 0), @Amount1 = COALESCE(SUM(Task_ChequeInfo.Amount1), 0), @Amount2 = COALESCE(SUM(Task_ChequeInfo.Amount2), 0)
				            FROM Task_ChequeInfo INNER JOIN Task_PaymentDetail ON Task_ChequeInfo.PaymentDetailId = Task_PaymentDetail.PaymentDetailId 
				            INNER JOIN Task_Payment ON Task_PaymentDetail.PaymentId = Task_Payment.PaymentId
				            WHERE (Task_Payment.Approved = 'A') AND CAST(Task_Payment.PaymentDate AS date) >= @OpeningDate AND CAST(Task_Payment.PaymentDate AS date) < @AsOn AND (Task_Payment.SupplierId = @SupplierId) AND (Task_ChequeInfo.Status = 'N') AND (Task_Payment.CompanyId = @CompanyId) --AND (Task_Payment.LocationId = @LocationId)

				            IF @CurrencyLevel = 1
				            BEGIN
					            SET @IssuedChequeNotTreatedAmount = @IssuedChequeNotTreatedAmount + @Amount
				            END
				            ELSE IF @CurrencyLevel = 2
				            BEGIN
					            SET @IssuedChequeNotTreatedAmount = @IssuedChequeNotTreatedAmount + @Amount1
				            END
				            ELSE
				            BEGIN
					            SET @IssuedChequeNotTreatedAmount = @IssuedChequeNotTreatedAmount + @Amount2
				            END
				
				            -- Dishonored From Cheque Treatment For Supplier
				            SET @IssuedDishonourChequeAmount = 0
				            SET @Amount = 0 
				            SET @Amount1 = 0
				            SET @Amount2 = 0

				            SELECT @Amount = COALESCE(SUM(Task_ChequeInfo.Amount), 0), @Amount1 = COALESCE(SUM(Task_ChequeInfo.Amount1), 0), @Amount2 = COALESCE(SUM(Task_ChequeInfo.Amount2), 0)
				            FROM Task_ChequeInfo INNER JOIN Task_PaymentDetail ON Task_ChequeInfo.PaymentDetailId = Task_PaymentDetail.PaymentDetailId 
				            INNER JOIN Task_Payment ON Task_PaymentDetail.PaymentId = Task_Payment.PaymentId
				            WHERE (Task_Payment.Approved = 'A') AND CAST(Task_Payment.PaymentDate AS date) >= @OpeningDate AND CAST(Task_Payment.PaymentDate AS date) < @AsOn AND (Task_Payment.SupplierId = @SupplierId) AND (Task_ChequeInfo.Status = 'D') AND (Task_Payment.CompanyId = @CompanyId) --AND (Task_Payment.LocationId = @LocationId)

				            IF @CurrencyLevel = 1
				            BEGIN
					            SET @IssuedDishonourChequeAmount = @IssuedDishonourChequeAmount + @Amount
				            END
				            ELSE IF @CurrencyLevel = 2
				            BEGIN
					            SET @IssuedDishonourChequeAmount = @IssuedDishonourChequeAmount + @Amount1
				            END
				            ELSE
				            BEGIN
					            SET @IssuedDishonourChequeAmount = @IssuedDishonourChequeAmount + @Amount2
				            END

				            -- Insert to Temp_CustomerSupplierOutstanding
				            INSERT INTO Temp_CustomerSupplierOutstanding (TempId, ReportFor, [Group], SupplierId, Name, LedgerEndingBalance, IssuedChequeNotTreatedAmount, IssuedDishonourChequeAmount, CompanyId, EntryBy) VALUES 
				            (NEWID(), 'S', @GroupName, @SupplierId, @SupplierName, @ClosingBalance, @IssuedChequeNotTreatedAmount, @IssuedDishonourChequeAmount, @CompanyId, @EntryBy)

				            FETCH NEXT FROM CursorSupplier INTO @GroupName, @SupplierId, @SupplierName
				            SET @FETCH_STATUS_ALL = @@FETCH_STATUS
			            END

			            CLOSE CursorSupplier
		                DEALLOCATE CursorSupplier
		                --END CURSOR FOR CustomerOrCombined
		            END
		            -- END ONLY SUPPLIER
	            END");
            }

            //if stored procedure not found then create stored procedure
            if (!CheckTable("SP_CustomerSupplierOutstanding_V2"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE PROCEDURE SP_CustomerSupplierOutstanding_V2
                @Currency varchar(5),
	            @For varchar(1),
	            @CustomerSupplierGroupId bigint,
	            @CustomerSupplierId bigint,
	            @SalespersonId bigint,
	            @AsOnDate varchar(10),
	            @CompanyId bigint,
	            @EntryBy bigint
	            AS
	            BEGIN
		            -- @For = C (Customer/Combined), S (Supplier)
		            DECLARE @CurrencyLevel int;
		            DECLARE @FETCH_STATUS_ALL int;
		            DECLARE @Amount numeric(18, 4);
		            DECLARE @Amount1 numeric(18, 4);
		            DECLARE @Amount2 numeric(18, 4);
		            DECLARE @ClosingBalance numeric(18, 4);
		            DECLARE @IssuedChequeNotTreatedAmount numeric(18, 4);
		            DECLARE @IssuedDishonourChequeAmount numeric(18, 4);
					DECLARE @GroupName varchar(50);
					DECLARE @CustomerName varchar(400);
					DECLARE @SupplierName varchar(400);
		            DECLARE @AsOn date;
		            DECLARE @OpeningDate date;

		            SELECT @AsOn = DATEADD(d, 1, CONVERT(date, @AsOnDate, 101));

		            -- Get company currency
		            SELECT @CurrencyLevel = dbo.fn_CurrencyLevel(@CompanyId, @Currency)

		            -- Get Company Opening Date
		            SELECT @OpeningDate = OpeningDate FROM Setup_Company WHERE CompanyId = @CompanyId

		            -- Clear temp table
		            DELETE FROM Temp_CustomerSupplierOutstanding WHERE CompanyId = @CompanyId 
		            AND EntryBy = @EntryBy
		            AND CASE WHEN @For = 'A' THEN 1 WHEN ReportFor = @For THEN 1 ELSE 0 END = 1

		            -- START CUSTOMER/COMBINED
		            IF @For = 'C' OR @For = 'A'
		            BEGIN
						-- Insert Customer Information to Temp_CustomerSupplierOutstanding
				        INSERT INTO Temp_CustomerSupplierOutstanding (TempId, ReportFor, [Group], CustomerId, Name, SupplierId, SalesPerson, CompanyId, EntryBy)  
				        SELECT NEWID(), 'C', Setup_CustomerGroup.Name, Setup_Customer.CustomerId, Setup_Customer.Code + ' # ' + Setup_Customer.Name, Setup_Customer.SupplierId, Setup_Employee.Code + ' # ' + Setup_Employee.Name, @CompanyId, @EntryBy 
						FROM Setup_Customer 
						INNER JOIN Setup_CustomerGroup ON Setup_Customer.CustomerGroupId = Setup_CustomerGroup.CustomerGroupId
						INNER JOIN Setup_Employee ON Setup_Customer.SalesPersonId = Setup_Employee.EmployeeId
			            WHERE Setup_Customer.IsActive = 1 AND Setup_Customer.CompanyId = @CompanyId
			            AND CASE WHEN @CustomerSupplierGroupId = 0 THEN 1 WHEN Setup_Customer.CustomerGroupId = @CustomerSupplierGroupId THEN 1 ELSE 0 END = 1
			            AND CASE WHEN @CustomerSupplierId = 0 THEN 1 WHEN Setup_Customer.CustomerId = @CustomerSupplierId THEN 1 ELSE 0 END = 1
			            AND CASE WHEN @SalespersonId = 0 THEN 1 WHEN Setup_Customer.SalesPersonId = @SalespersonId THEN 1 ELSE 0 END = 1
						
						-- Update Closing Balance to Temp_CustomerSupplierOutstanding
						UPDATE Temp_CustomerSupplierOutstanding SET LedgerEndingBalance = CASE WHEN @CurrencyLevel = 1 THEN TT.Amount WHEN @CurrencyLevel = 2 THEN TT.Amount1 ELSE TT.Amount2 END
						FROM (SELECT SUM(fn_PartyLedger.Amount) AS Amount, SUM(fn_PartyLedger.Amount1) AS Amount1, SUM(fn_PartyLedger.Amount2) AS Amount2, fn_PartyLedger.CustomerId
						FROM dbo.fn_CalculateOpeningBalanceForPartyLedger_V2(@For, @CustomerSupplierId, @CompanyId, @AsOn) AS fn_PartyLedger INNER JOIN
						Temp_CustomerSupplierOutstanding ON fn_PartyLedger.CustomerId = Temp_CustomerSupplierOutstanding.CustomerId
						WHERE (fn_PartyLedger.[For] = @For) AND (Temp_CustomerSupplierOutstanding.CompanyId = @CompanyId) AND (Temp_CustomerSupplierOutstanding.EntryBy = @EntryBy)
						GROUP BY fn_PartyLedger.CustomerId) AS TT INNER JOIN Temp_CustomerSupplierOutstanding ON TT.CustomerId = Temp_CustomerSupplierOutstanding.CustomerId

						-- Not Treated From Cheque Treatment For Customer
						UPDATE Temp_CustomerSupplierOutstanding SET ReceivedChequeNotTreatedAmount = CASE WHEN @CurrencyLevel = 1 THEN TT.Amount WHEN @CurrencyLevel = 2 THEN TT.Amount1 ELSE TT.Amount2 END
						FROM (SELECT COALESCE (SUM(Task_ChequeInfo.Amount), 0) AS Amount, COALESCE (SUM(Task_ChequeInfo.Amount1), 0) AS Amount1, COALESCE (SUM(Task_ChequeInfo.Amount2), 0) AS Amount2, Task_Collection.CustomerId
						FROM Task_ChequeInfo INNER JOIN Task_CollectionDetail ON Task_ChequeInfo.CollectionDetailId = Task_CollectionDetail.CollectionDetailId 
						INNER JOIN Task_Collection ON Task_CollectionDetail.CollectionId = Task_Collection.CollectionId
						WHERE (Task_Collection.Approved = 'A') AND CAST(Task_Collection.CollectionDate AS date) >= @OpeningDate AND CAST(Task_Collection.CollectionDate AS date) < @AsOn AND (Task_ChequeInfo.Status = 'N') AND (Task_Collection.CompanyId = @CompanyId)
						GROUP BY Task_Collection.CustomerId) AS TT INNER JOIN Temp_CustomerSupplierOutstanding ON TT.CustomerId = Temp_CustomerSupplierOutstanding.CustomerId
						
						-- Dishonored From Cheque Treatment For Customer
						UPDATE Temp_CustomerSupplierOutstanding SET ReceivedDishonourChequeAmount = CASE WHEN @CurrencyLevel = 1 THEN TT.Amount WHEN @CurrencyLevel = 2 THEN TT.Amount1 ELSE TT.Amount2 END
						FROM (SELECT COALESCE(SUM(Task_ChequeInfo.Amount), 0) AS Amount, COALESCE(SUM(Task_ChequeInfo.Amount1), 0) AS Amount1, COALESCE(SUM(Task_ChequeInfo.Amount2), 0) AS Amount2, Task_Collection.CustomerId
				        FROM Task_ChequeInfo INNER JOIN Task_CollectionDetail ON Task_ChequeInfo.CollectionDetailId = Task_CollectionDetail.CollectionDetailId 
				        INNER JOIN Task_Collection ON Task_CollectionDetail.CollectionId = Task_Collection.CollectionId
				        WHERE (Task_Collection.Approved = 'A') AND CAST(Task_Collection.CollectionDate AS date) >= @OpeningDate AND CAST(Task_Collection.CollectionDate AS date) < @AsOn AND (Task_ChequeInfo.Status = 'D') AND (Task_Collection.CompanyId = @CompanyId)
						GROUP BY Task_Collection.CustomerId) AS TT INNER JOIN Temp_CustomerSupplierOutstanding ON TT.CustomerId = Temp_CustomerSupplierOutstanding.CustomerId
						
						-- Not Treated From Cheque Treatment For Supplier
						UPDATE Temp_CustomerSupplierOutstanding SET IssuedChequeNotTreatedAmount = CASE WHEN @CurrencyLevel = 1 THEN TT.Amount WHEN @CurrencyLevel = 2 THEN TT.Amount1 ELSE TT.Amount2 END
						FROM (SELECT COALESCE(SUM(Task_ChequeInfo.Amount), 0) AS Amount, COALESCE(SUM(Task_ChequeInfo.Amount1), 0) AS Amount1, COALESCE(SUM(Task_ChequeInfo.Amount2), 0) AS Amount2, Task_Payment.SupplierId
				        FROM Task_ChequeInfo INNER JOIN Task_PaymentDetail ON Task_ChequeInfo.PaymentDetailId = Task_PaymentDetail.PaymentDetailId 
				        INNER JOIN Task_Payment ON Task_PaymentDetail.PaymentId = Task_Payment.PaymentId
				        WHERE (Task_Payment.Approved = 'A') AND CAST(Task_Payment.PaymentDate AS date) >= @OpeningDate AND CAST(Task_Payment.PaymentDate AS date) < @AsOn AND (Task_ChequeInfo.Status = 'N') AND (Task_Payment.CompanyId = @CompanyId)
						GROUP BY Task_Payment.SupplierId) AS TT INNER JOIN Temp_CustomerSupplierOutstanding ON TT.SupplierId = Temp_CustomerSupplierOutstanding.SupplierId
						
						-- Dishonored From Cheque Treatment For Supplier
						UPDATE Temp_CustomerSupplierOutstanding SET IssuedDishonourChequeAmount = CASE WHEN @CurrencyLevel = 1 THEN TT.Amount WHEN @CurrencyLevel = 2 THEN TT.Amount1 ELSE TT.Amount2 END
						FROM (SELECT COALESCE(SUM(Task_ChequeInfo.Amount), 0) AS Amount, COALESCE(SUM(Task_ChequeInfo.Amount1), 0) AS Amount1, COALESCE(SUM(Task_ChequeInfo.Amount2), 0) AS Amount2, Task_Payment.SupplierId
				        FROM Task_ChequeInfo INNER JOIN Task_PaymentDetail ON Task_ChequeInfo.PaymentDetailId = Task_PaymentDetail.PaymentDetailId 
				        INNER JOIN Task_Payment ON Task_PaymentDetail.PaymentId = Task_Payment.PaymentId
				        WHERE (Task_Payment.Approved = 'A') AND CAST(Task_Payment.PaymentDate AS date) >= @OpeningDate AND CAST(Task_Payment.PaymentDate AS date) < @AsOn AND (Task_ChequeInfo.Status = 'D') AND (Task_Payment.CompanyId = @CompanyId)
						GROUP BY Task_Payment.SupplierId) AS TT INNER JOIN Temp_CustomerSupplierOutstanding ON TT.SupplierId = Temp_CustomerSupplierOutstanding.SupplierId

		            END
		            -- END CUSTOMER/COMBINED

		            -- START ONLY SUPPLIER
		            IF @For = 'S' OR @For = 'A'
		            BEGIN
						-- Insert Supplier Information to Temp_CustomerSupplierOutstanding
				        INSERT INTO Temp_CustomerSupplierOutstanding (TempId, ReportFor, [Group], SupplierId, Name, CompanyId, EntryBy)  
						SELECT NEWID(), 'S', Setup_SupplierGroup.Name, Setup_Supplier.SupplierId, Setup_Supplier.Code + ' # ' + Setup_Supplier.Name, @CompanyId, @EntryBy
						FROM Setup_Supplier INNER JOIN Setup_SupplierGroup ON Setup_Supplier.SupplierGroupId = Setup_SupplierGroup.SupplierGroupId
			            WHERE Setup_Supplier.IsActive = 1 AND Setup_Supplier.CompanyId = @CompanyId AND Setup_Supplier.SupplierId NOT IN (SELECT SupplierId FROM Setup_Customer WHERE SupplierId IS NOT NULL AND CompanyId = @CompanyId)
			            AND CASE WHEN @CustomerSupplierGroupId = 0 THEN 1 WHEN Setup_Supplier.SupplierGroupId = @CustomerSupplierGroupId THEN 1 ELSE 0 END = 1
			            AND CASE WHEN @CustomerSupplierId = 0 THEN 1 WHEN Setup_Supplier.SupplierId = @CustomerSupplierId THEN 1 ELSE 0 END = 1

						-- Update Closing Balance to Temp_CustomerSupplierOutstanding
						UPDATE Temp_CustomerSupplierOutstanding SET LedgerEndingBalance = CASE WHEN @CurrencyLevel = 1 THEN TT.Amount WHEN @CurrencyLevel = 2 THEN TT.Amount1 ELSE TT.Amount2 END
						FROM (SELECT SUM(fn_PartyLedger.Amount) AS Amount, SUM(fn_PartyLedger.Amount1) AS Amount1, SUM(fn_PartyLedger.Amount2) AS Amount2, fn_PartyLedger.SupplierId
						FROM dbo.fn_CalculateOpeningBalanceForSupplierLedger_V2(@For, @CompanyId, @AsOn) AS fn_PartyLedger INNER JOIN
						Temp_CustomerSupplierOutstanding ON fn_PartyLedger.SupplierId = Temp_CustomerSupplierOutstanding.SupplierId
						WHERE (fn_PartyLedger.[For] = 'S') AND (Temp_CustomerSupplierOutstanding.CompanyId = @CompanyId) AND (Temp_CustomerSupplierOutstanding.EntryBy = @EntryBy)
						GROUP BY fn_PartyLedger.SupplierId) AS TT INNER JOIN Temp_CustomerSupplierOutstanding ON TT.SupplierId = Temp_CustomerSupplierOutstanding.SupplierId

						-- Not Treated From Cheque Treatment For Supplier
						UPDATE Temp_CustomerSupplierOutstanding SET IssuedChequeNotTreatedAmount = CASE WHEN @CurrencyLevel = 1 THEN TT.Amount WHEN @CurrencyLevel = 2 THEN TT.Amount1 ELSE TT.Amount2 END
						FROM (SELECT COALESCE(SUM(Task_ChequeInfo.Amount), 0) AS Amount, COALESCE(SUM(Task_ChequeInfo.Amount1), 0) AS Amount1, COALESCE(SUM(Task_ChequeInfo.Amount2), 0) AS Amount2, Task_Payment.SupplierId
				        FROM Task_ChequeInfo INNER JOIN Task_PaymentDetail ON Task_ChequeInfo.PaymentDetailId = Task_PaymentDetail.PaymentDetailId 
				        INNER JOIN Task_Payment ON Task_PaymentDetail.PaymentId = Task_Payment.PaymentId
				        WHERE (Task_Payment.Approved = 'A') AND CAST(Task_Payment.PaymentDate AS date) >= @OpeningDate AND CAST(Task_Payment.PaymentDate AS date) < @AsOn AND (Task_ChequeInfo.Status = 'N') AND (Task_Payment.CompanyId = @CompanyId)
						GROUP BY Task_Payment.SupplierId) AS TT INNER JOIN Temp_CustomerSupplierOutstanding ON TT.SupplierId = Temp_CustomerSupplierOutstanding.SupplierId

						-- Dishonored From Cheque Treatment For Supplier
						UPDATE Temp_CustomerSupplierOutstanding SET IssuedDishonourChequeAmount = CASE WHEN @CurrencyLevel = 1 THEN TT.Amount WHEN @CurrencyLevel = 2 THEN TT.Amount1 ELSE TT.Amount2 END
						FROM (SELECT COALESCE(SUM(Task_ChequeInfo.Amount), 0) AS Amount, COALESCE(SUM(Task_ChequeInfo.Amount1), 0) AS Amount1,  COALESCE(SUM(Task_ChequeInfo.Amount2), 0) AS Amount2, Task_Payment.SupplierId
				        FROM Task_ChequeInfo INNER JOIN Task_PaymentDetail ON Task_ChequeInfo.PaymentDetailId = Task_PaymentDetail.PaymentDetailId 
				        INNER JOIN Task_Payment ON Task_PaymentDetail.PaymentId = Task_Payment.PaymentId
				        WHERE (Task_Payment.Approved = 'A') AND CAST(Task_Payment.PaymentDate AS date) >= @OpeningDate AND CAST(Task_Payment.PaymentDate AS date) < @AsOn AND (Task_ChequeInfo.Status = 'D') AND (Task_Payment.CompanyId = @CompanyId)
						GROUP BY Task_Payment.SupplierId) AS TT INNER JOIN Temp_CustomerSupplierOutstanding ON TT.SupplierId = Temp_CustomerSupplierOutstanding.SupplierId

		            END
		            -- END ONLY SUPPLIER
	            END");
            }

            //if stored procedure found then alter stored procedure
            if (CheckTable("SP_CustomerSupplierOutstanding_V2"))
            {
                _db.Database.ExecuteSqlCommand(@"ALTER PROCEDURE SP_CustomerSupplierOutstanding_V2
                @Currency varchar(5),
	            @For varchar(1),
	            @CustomerSupplierGroupId bigint,
	            @CustomerSupplierId bigint,
	            @SalespersonId bigint,
	            @AsOnDate varchar(10),
	            @CompanyId bigint,
	            @EntryBy bigint
	            AS
	            BEGIN
		            -- @For = C (Customer/Combined), S (Supplier)
		            DECLARE @CurrencyLevel int;
		            DECLARE @FETCH_STATUS_ALL int;
		            DECLARE @Amount numeric(18, 4);
		            DECLARE @Amount1 numeric(18, 4);
		            DECLARE @Amount2 numeric(18, 4);
		            DECLARE @ClosingBalance numeric(18, 4);
		            DECLARE @IssuedChequeNotTreatedAmount numeric(18, 4);
		            DECLARE @IssuedDishonourChequeAmount numeric(18, 4);
					DECLARE @GroupName varchar(50);
					DECLARE @CustomerName varchar(400);
					DECLARE @SupplierName varchar(400);
		            DECLARE @AsOn date;
		            DECLARE @OpeningDate date;

		            SELECT @AsOn = DATEADD(d, 1, CONVERT(date, @AsOnDate, 101));

		            -- Get company currency
		            SELECT @CurrencyLevel = dbo.fn_CurrencyLevel(@CompanyId, @Currency)

		            -- Get Company Opening Date
		            SELECT @OpeningDate = OpeningDate FROM Setup_Company WHERE CompanyId = @CompanyId

		            -- Clear temp table
		            DELETE FROM Temp_CustomerSupplierOutstanding WHERE CompanyId = @CompanyId 
		            AND EntryBy = @EntryBy
		            AND CASE WHEN @For = 'A' THEN 1 WHEN ReportFor = @For THEN 1 ELSE 0 END = 1

		            -- START CUSTOMER/COMBINED
		            IF @For = 'C' OR @For = 'A'
		            BEGIN
						-- Insert Customer Information to Temp_CustomerSupplierOutstanding
				        INSERT INTO Temp_CustomerSupplierOutstanding (TempId, ReportFor, [Group], CustomerId, Code, Name, SupplierId, SalesPerson, CompanyId, EntryBy)  
				        SELECT NEWID(), 'C', Setup_CustomerGroup.Name, Setup_Customer.CustomerId, Setup_Customer.Code, Setup_Customer.Name, Setup_Customer.SupplierId, Setup_Employee.Code + ' # ' + Setup_Employee.Name, @CompanyId, @EntryBy 
						FROM Setup_Customer 
						INNER JOIN Setup_CustomerGroup ON Setup_Customer.CustomerGroupId = Setup_CustomerGroup.CustomerGroupId
						INNER JOIN Setup_Employee ON Setup_Customer.SalesPersonId = Setup_Employee.EmployeeId
			            WHERE Setup_Customer.IsActive = 1 AND Setup_Customer.CompanyId = @CompanyId
			            AND CASE WHEN @CustomerSupplierGroupId = 0 THEN 1 WHEN Setup_Customer.CustomerGroupId = @CustomerSupplierGroupId THEN 1 ELSE 0 END = 1
			            AND CASE WHEN @CustomerSupplierId = 0 THEN 1 WHEN Setup_Customer.CustomerId = @CustomerSupplierId THEN 1 ELSE 0 END = 1
			            AND CASE WHEN @SalespersonId = 0 THEN 1 WHEN Setup_Customer.SalesPersonId = @SalespersonId THEN 1 ELSE 0 END = 1
						
						-- Update Closing Balance to Temp_CustomerSupplierOutstanding
						UPDATE Temp_CustomerSupplierOutstanding SET LedgerEndingBalance = CASE WHEN @CurrencyLevel = 1 THEN TT.Amount WHEN @CurrencyLevel = 2 THEN TT.Amount1 ELSE TT.Amount2 END
						FROM (SELECT SUM(fn_PartyLedger.Amount) AS Amount, SUM(fn_PartyLedger.Amount1) AS Amount1, SUM(fn_PartyLedger.Amount2) AS Amount2, fn_PartyLedger.CustomerId
						FROM dbo.fn_CalculateOpeningBalanceForPartyLedger_V2(@For, @CustomerSupplierId, @CompanyId, @AsOn) AS fn_PartyLedger INNER JOIN
						Temp_CustomerSupplierOutstanding ON fn_PartyLedger.CustomerId = Temp_CustomerSupplierOutstanding.CustomerId
						WHERE (fn_PartyLedger.[For] = @For) AND (Temp_CustomerSupplierOutstanding.CompanyId = @CompanyId) AND (Temp_CustomerSupplierOutstanding.EntryBy = @EntryBy)
						GROUP BY fn_PartyLedger.CustomerId) AS TT INNER JOIN Temp_CustomerSupplierOutstanding ON TT.CustomerId = Temp_CustomerSupplierOutstanding.CustomerId

						-- Not Treated From Cheque Treatment For Customer
						UPDATE Temp_CustomerSupplierOutstanding SET ReceivedChequeNotTreatedAmount = CASE WHEN @CurrencyLevel = 1 THEN TT.Amount WHEN @CurrencyLevel = 2 THEN TT.Amount1 ELSE TT.Amount2 END
						FROM (SELECT COALESCE (SUM(Task_ChequeInfo.Amount), 0) AS Amount, COALESCE (SUM(Task_ChequeInfo.Amount1), 0) AS Amount1, COALESCE (SUM(Task_ChequeInfo.Amount2), 0) AS Amount2, Task_Collection.CustomerId
						FROM Task_ChequeInfo INNER JOIN Task_CollectionDetail ON Task_ChequeInfo.CollectionDetailId = Task_CollectionDetail.CollectionDetailId 
						INNER JOIN Task_Collection ON Task_CollectionDetail.CollectionId = Task_Collection.CollectionId
						WHERE (Task_Collection.Approved = 'A') AND CAST(Task_Collection.CollectionDate AS date) >= @OpeningDate AND CAST(Task_Collection.CollectionDate AS date) < @AsOn AND (Task_ChequeInfo.Status = 'N') AND (Task_Collection.CompanyId = @CompanyId)
						GROUP BY Task_Collection.CustomerId) AS TT INNER JOIN Temp_CustomerSupplierOutstanding ON TT.CustomerId = Temp_CustomerSupplierOutstanding.CustomerId
						
						-- Dishonored From Cheque Treatment For Customer
						UPDATE Temp_CustomerSupplierOutstanding SET ReceivedDishonourChequeAmount = CASE WHEN @CurrencyLevel = 1 THEN TT.Amount WHEN @CurrencyLevel = 2 THEN TT.Amount1 ELSE TT.Amount2 END
						FROM (SELECT COALESCE(SUM(Task_ChequeInfo.Amount), 0) AS Amount, COALESCE(SUM(Task_ChequeInfo.Amount1), 0) AS Amount1, COALESCE(SUM(Task_ChequeInfo.Amount2), 0) AS Amount2, Task_Collection.CustomerId
				        FROM Task_ChequeInfo INNER JOIN Task_CollectionDetail ON Task_ChequeInfo.CollectionDetailId = Task_CollectionDetail.CollectionDetailId 
				        INNER JOIN Task_Collection ON Task_CollectionDetail.CollectionId = Task_Collection.CollectionId
				        WHERE (Task_Collection.Approved = 'A') AND CAST(Task_Collection.CollectionDate AS date) >= @OpeningDate AND CAST(Task_Collection.CollectionDate AS date) < @AsOn AND (Task_ChequeInfo.Status = 'D') AND (Task_Collection.CompanyId = @CompanyId)
						GROUP BY Task_Collection.CustomerId) AS TT INNER JOIN Temp_CustomerSupplierOutstanding ON TT.CustomerId = Temp_CustomerSupplierOutstanding.CustomerId
						
						-- Not Treated From Cheque Treatment For Supplier
						UPDATE Temp_CustomerSupplierOutstanding SET IssuedChequeNotTreatedAmount = CASE WHEN @CurrencyLevel = 1 THEN TT.Amount WHEN @CurrencyLevel = 2 THEN TT.Amount1 ELSE TT.Amount2 END
						FROM (SELECT COALESCE(SUM(Task_ChequeInfo.Amount), 0) AS Amount, COALESCE(SUM(Task_ChequeInfo.Amount1), 0) AS Amount1, COALESCE(SUM(Task_ChequeInfo.Amount2), 0) AS Amount2, Task_Payment.SupplierId
				        FROM Task_ChequeInfo INNER JOIN Task_PaymentDetail ON Task_ChequeInfo.PaymentDetailId = Task_PaymentDetail.PaymentDetailId 
				        INNER JOIN Task_Payment ON Task_PaymentDetail.PaymentId = Task_Payment.PaymentId
				        WHERE (Task_Payment.Approved = 'A') AND CAST(Task_Payment.PaymentDate AS date) >= @OpeningDate AND CAST(Task_Payment.PaymentDate AS date) < @AsOn AND (Task_ChequeInfo.Status = 'N') AND (Task_Payment.CompanyId = @CompanyId)
						GROUP BY Task_Payment.SupplierId) AS TT INNER JOIN Temp_CustomerSupplierOutstanding ON TT.SupplierId = Temp_CustomerSupplierOutstanding.SupplierId
						
						-- Dishonored From Cheque Treatment For Supplier
						UPDATE Temp_CustomerSupplierOutstanding SET IssuedDishonourChequeAmount = CASE WHEN @CurrencyLevel = 1 THEN TT.Amount WHEN @CurrencyLevel = 2 THEN TT.Amount1 ELSE TT.Amount2 END
						FROM (SELECT COALESCE(SUM(Task_ChequeInfo.Amount), 0) AS Amount, COALESCE(SUM(Task_ChequeInfo.Amount1), 0) AS Amount1, COALESCE(SUM(Task_ChequeInfo.Amount2), 0) AS Amount2, Task_Payment.SupplierId
				        FROM Task_ChequeInfo INNER JOIN Task_PaymentDetail ON Task_ChequeInfo.PaymentDetailId = Task_PaymentDetail.PaymentDetailId 
				        INNER JOIN Task_Payment ON Task_PaymentDetail.PaymentId = Task_Payment.PaymentId
				        WHERE (Task_Payment.Approved = 'A') AND CAST(Task_Payment.PaymentDate AS date) >= @OpeningDate AND CAST(Task_Payment.PaymentDate AS date) < @AsOn AND (Task_ChequeInfo.Status = 'D') AND (Task_Payment.CompanyId = @CompanyId)
						GROUP BY Task_Payment.SupplierId) AS TT INNER JOIN Temp_CustomerSupplierOutstanding ON TT.SupplierId = Temp_CustomerSupplierOutstanding.SupplierId

		            END
		            -- END CUSTOMER/COMBINED

		            -- START ONLY SUPPLIER
		            IF @For = 'S' OR @For = 'A'
		            BEGIN
						-- Insert Supplier Information to Temp_CustomerSupplierOutstanding
				        INSERT INTO Temp_CustomerSupplierOutstanding (TempId, ReportFor, [Group], SupplierId, Code, Name, CompanyId, EntryBy)  
						SELECT NEWID(), 'S', Setup_SupplierGroup.Name, Setup_Supplier.SupplierId, Setup_Supplier.Code, Setup_Supplier.Name, @CompanyId, @EntryBy
						FROM Setup_Supplier INNER JOIN Setup_SupplierGroup ON Setup_Supplier.SupplierGroupId = Setup_SupplierGroup.SupplierGroupId
			            WHERE Setup_Supplier.IsActive = 1 AND Setup_Supplier.CompanyId = @CompanyId AND Setup_Supplier.SupplierId NOT IN (SELECT SupplierId FROM Setup_Customer WHERE SupplierId IS NOT NULL AND CompanyId = @CompanyId)
			            AND CASE WHEN @CustomerSupplierGroupId = 0 THEN 1 WHEN Setup_Supplier.SupplierGroupId = @CustomerSupplierGroupId THEN 1 ELSE 0 END = 1
			            AND CASE WHEN @CustomerSupplierId = 0 THEN 1 WHEN Setup_Supplier.SupplierId = @CustomerSupplierId THEN 1 ELSE 0 END = 1

						-- Update Closing Balance to Temp_CustomerSupplierOutstanding
						UPDATE Temp_CustomerSupplierOutstanding SET LedgerEndingBalance = CASE WHEN @CurrencyLevel = 1 THEN TT.Amount WHEN @CurrencyLevel = 2 THEN TT.Amount1 ELSE TT.Amount2 END
						FROM (SELECT SUM(fn_PartyLedger.Amount) AS Amount, SUM(fn_PartyLedger.Amount1) AS Amount1, SUM(fn_PartyLedger.Amount2) AS Amount2, fn_PartyLedger.SupplierId
						FROM dbo.fn_CalculateOpeningBalanceForSupplierLedger_V2(@For, @CompanyId, @AsOn) AS fn_PartyLedger INNER JOIN
						Temp_CustomerSupplierOutstanding ON fn_PartyLedger.SupplierId = Temp_CustomerSupplierOutstanding.SupplierId
						WHERE (fn_PartyLedger.[For] = 'S') AND (Temp_CustomerSupplierOutstanding.CompanyId = @CompanyId) AND (Temp_CustomerSupplierOutstanding.EntryBy = @EntryBy)
						GROUP BY fn_PartyLedger.SupplierId) AS TT INNER JOIN Temp_CustomerSupplierOutstanding ON TT.SupplierId = Temp_CustomerSupplierOutstanding.SupplierId

						-- Not Treated From Cheque Treatment For Supplier
						UPDATE Temp_CustomerSupplierOutstanding SET IssuedChequeNotTreatedAmount = CASE WHEN @CurrencyLevel = 1 THEN TT.Amount WHEN @CurrencyLevel = 2 THEN TT.Amount1 ELSE TT.Amount2 END
						FROM (SELECT COALESCE(SUM(Task_ChequeInfo.Amount), 0) AS Amount, COALESCE(SUM(Task_ChequeInfo.Amount1), 0) AS Amount1, COALESCE(SUM(Task_ChequeInfo.Amount2), 0) AS Amount2, Task_Payment.SupplierId
				        FROM Task_ChequeInfo INNER JOIN Task_PaymentDetail ON Task_ChequeInfo.PaymentDetailId = Task_PaymentDetail.PaymentDetailId 
				        INNER JOIN Task_Payment ON Task_PaymentDetail.PaymentId = Task_Payment.PaymentId
				        WHERE (Task_Payment.Approved = 'A') AND CAST(Task_Payment.PaymentDate AS date) >= @OpeningDate AND CAST(Task_Payment.PaymentDate AS date) < @AsOn AND (Task_ChequeInfo.Status = 'N') AND (Task_Payment.CompanyId = @CompanyId)
						GROUP BY Task_Payment.SupplierId) AS TT INNER JOIN Temp_CustomerSupplierOutstanding ON TT.SupplierId = Temp_CustomerSupplierOutstanding.SupplierId

						-- Dishonored From Cheque Treatment For Supplier
						UPDATE Temp_CustomerSupplierOutstanding SET IssuedDishonourChequeAmount = CASE WHEN @CurrencyLevel = 1 THEN TT.Amount WHEN @CurrencyLevel = 2 THEN TT.Amount1 ELSE TT.Amount2 END
						FROM (SELECT COALESCE(SUM(Task_ChequeInfo.Amount), 0) AS Amount, COALESCE(SUM(Task_ChequeInfo.Amount1), 0) AS Amount1,  COALESCE(SUM(Task_ChequeInfo.Amount2), 0) AS Amount2, Task_Payment.SupplierId
				        FROM Task_ChequeInfo INNER JOIN Task_PaymentDetail ON Task_ChequeInfo.PaymentDetailId = Task_PaymentDetail.PaymentDetailId 
				        INNER JOIN Task_Payment ON Task_PaymentDetail.PaymentId = Task_Payment.PaymentId
				        WHERE (Task_Payment.Approved = 'A') AND CAST(Task_Payment.PaymentDate AS date) >= @OpeningDate AND CAST(Task_Payment.PaymentDate AS date) < @AsOn AND (Task_ChequeInfo.Status = 'D') AND (Task_Payment.CompanyId = @CompanyId)
						GROUP BY Task_Payment.SupplierId) AS TT INNER JOIN Temp_CustomerSupplierOutstanding ON TT.SupplierId = Temp_CustomerSupplierOutstanding.SupplierId

		            END
		            -- END ONLY SUPPLIER
	            END");
            }

            //if stored procedure not found then create stored procedure
            if (!CheckTable("SP_CustomerOrSupplierWiseChequePerformance"))
            {
                _db.Database.ExecuteSqlCommand(@"CREATE PROCEDURE SP_CustomerOrSupplierWiseChequePerformance
                    @CompanyId bigint= 0,
                    @Currency varchar(5)= null,
                    @ChequeType varchar(50) = null,
                    @LocationId bigint= 0,
                    @BankId bigint= 0,
                    @CustomerOrSupplierId bigint= 0,    
                    @DateFrom date,
                    @DateTo date,
                    @ChequeOrTreatementBankOptionValue varchar(50)= null,
                    @ChequeCollectionOrPaymentDateOptionValue varchar(50)= null,
	                @EntryBy bigint = null
                AS
                BEGIN
	                --DROP TABLE TempCustomerOrSupplierWiseChequePerformance

	                DELETE TempCustomerOrSupplierWiseChequePerformance WHERE CompanyId = @CompanyId AND EntryBy = @EntryBy

	                DECLARE @LocationName VARCHAR(500)= NULL;
	                if(@LocationId <>0)
	                BEGIN
		                SET @LocationName = (SELECT Name FROM Setup_Location WHERE LocationId = @LocationId AND CompanyId = @CompanyId)
	                END
	                if(@ChequeType = 'receivedCheque')
	                BEGIN
		                ------------All cheque------------------
		                SELECT 

			                Task_Collection.CustomerId AS CustomerOrSupplierId
			                ,COUNT(Task_ChequeInfo.ChequeNo) AS NoOfCheque
			                ,SUM(ISNULL(Task_ChequeInfo.Amount,0)) AS ChequeAmount

		                INTO #CustomerWiseAllCheque
		                FROM Task_ChequeInfo
		                INNER JOIN Task_CollectionDetail ON Task_ChequeInfo.CollectionDetailId = Task_CollectionDetail.CollectionDetailId
		                INNER JOIN Task_Collection ON Task_CollectionDetail.CollectionId = Task_Collection.CollectionId
		                LEFT JOIN Task_ChequeTreatment ON Task_ChequeInfo.ChequeInfoId = Task_ChequeTreatment.ChequeInfoId

		                WHERE 
		                Task_Collection.Approved = 'A'
		                AND CASE WHEN @CompanyId = 0 THEN 1 WHEN Task_ChequeInfo.CompanyId = @CompanyId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @LocationId = 0 THEN 1 WHEN Task_ChequeInfo.LocationId = @LocationId THEN 1 ELSE 0 END = 1
		                AND CASE WHEN  @BankId = 0 THEN 1 WHEN @ChequeOrTreatementBankOptionValue = 'Cheque Bank' AND Task_ChequeInfo.BankId = @BankId THEN 1 ELSE 0 END = 1
		                AND CASE WHEN  @BankId = 0 THEN 1 WHEN @ChequeOrTreatementBankOptionValue = 'Treatement Bank' AND Task_ChequeTreatment.TreatmentBankId = @BankId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @CustomerOrSupplierId = 0 THEN 1 WHEN Task_Collection.CustomerId = @CustomerOrSupplierId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateFrom IS  NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'ChequeDate' AND CAST(Task_ChequeInfo.ChequeDate AS DATE) >= @DateFrom THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateTo IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'ChequeDate' AND CAST(Task_ChequeInfo.ChequeDate AS DATE) <= @DateTo THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateFrom IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'CollectionDate' AND CAST(Task_Collection.CollectionDate AS DATE) >= @DateFrom THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateTo IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'CollectionDate' AND CAST(Task_Collection.CollectionDate AS DATE) <= @DateTo THEN 1 ELSE 0 END = 1
		
		                GROUP BY
		                Task_Collection.CustomerId

		                -------------- Dis honor cheque -----------------

		                SELECT 

			                Task_Collection.CustomerId AS CustomerOrSupplierId
			                ,AllCheque.ChequeAmount
			                ,COUNT(Task_ChequeInfo.ChequeNo) AS NoOfDisHonerCheque
			                ,SUM(ISNULL(Task_ChequeInfo.Amount,0)) AS DisHonerChequeAmount
			                ,(SUM(ISNULL(Task_ChequeInfo.Amount,0))/AllCheque.ChequeAmount)*100 AS DisHonerChequePercentageAmount

		                INTO #CustomerWiseDisHonerCheque
		                FROM #CustomerWiseAllCheque AllCheque
		                INNER JOIN Task_Collection ON AllCheque.CustomerOrSupplierId = Task_Collection.CustomerId
		                INNER JOIN Task_CollectionDetail ON Task_Collection.CollectionId = Task_CollectionDetail.CollectionId	
		                INNER JOIN Task_ChequeInfo ON Task_ChequeInfo.CollectionDetailId = Task_CollectionDetail.CollectionDetailId AND Task_ChequeInfo.Status ='D'
		                LEFT JOIN Task_ChequeTreatment ON Task_ChequeInfo.ChequeInfoId = Task_ChequeTreatment.ChequeInfoId
		
		                WHERE  
		                Task_Collection.Approved = 'A'
		                AND CASE WHEN @CompanyId = 0 THEN 1 WHEN Task_ChequeInfo.CompanyId = @CompanyId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @LocationId = 0 THEN 1 WHEN Task_ChequeInfo.LocationId = @LocationId THEN 1 ELSE 0 END = 1
		                AND CASE WHEN  @BankId = 0 THEN 1 WHEN @ChequeOrTreatementBankOptionValue = 'Cheque Bank' AND Task_ChequeInfo.BankId = @BankId THEN 1 ELSE 0 END = 1
		                AND CASE WHEN  @BankId = 0 THEN 1 WHEN @ChequeOrTreatementBankOptionValue = 'Treatement Bank' AND Task_ChequeTreatment.TreatmentBankId = @BankId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @CustomerOrSupplierId = 0 THEN 1 WHEN Task_Collection.CustomerId = @CustomerOrSupplierId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateFrom IS  NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'ChequeDate' AND CAST(Task_ChequeInfo.ChequeDate AS DATE) >= @DateFrom THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateTo IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'ChequeDate' AND CAST(Task_ChequeInfo.ChequeDate AS DATE) <= @DateTo THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateFrom IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'CollectionDate' AND CAST(Task_Collection.CollectionDate AS DATE) >= @DateFrom THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateTo IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'CollectionDate' AND CAST(Task_Collection.CollectionDate AS DATE) <= @DateTo THEN 1 ELSE 0 END = 1
	
		                GROUP BY
		                Task_Collection.CustomerId 
		                ,AllCheque.ChequeAmount

		                -------------- BalanceAdjusted cheque -----------------

		                SELECT 
		                    Task_Collection.CustomerId AS CustomerOrSupplierId
			                ,AllCheque.ChequeAmount
			                ,COUNT(Task_ChequeInfo.ChequeNo) AS NoOfBalanceAdjustedCheque
			                ,SUM(ISNULL(Task_ChequeInfo.Amount,0)) AS BalanceAdjustedChequeAmount
			                ,(SUM(ISNULL(Task_ChequeInfo.Amount,0))/AllCheque.ChequeAmount)*100 AS BalanceAdjustedChequePercentageAmount

		                INTO #CustomerWiseBalanceAdjustedCheque
		                FROM #CustomerWiseAllCheque AllCheque
		                INNER JOIN Task_Collection ON AllCheque.CustomerOrSupplierId = Task_Collection.CustomerId
		                INNER JOIN Task_CollectionDetail ON Task_Collection.CollectionId = Task_CollectionDetail.CollectionId	
		                INNER JOIN Task_ChequeInfo ON Task_ChequeInfo.CollectionDetailId =Task_CollectionDetail.CollectionDetailId AND Task_ChequeInfo.Status ='B'
		                LEFT JOIN Task_ChequeTreatment ON Task_ChequeInfo.ChequeInfoId = Task_ChequeTreatment.ChequeInfoId
		                WHERE 
		                Task_Collection.Approved = 'A'
		                AND CASE WHEN @CompanyId = 0 THEN 1 WHEN Task_ChequeInfo.CompanyId = @CompanyId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @LocationId = 0 THEN 1 WHEN Task_ChequeInfo.LocationId = @LocationId THEN 1 ELSE 0 END = 1
		                AND CASE WHEN  @BankId = 0 THEN 1 WHEN @ChequeOrTreatementBankOptionValue = 'Cheque Bank' AND Task_ChequeInfo.BankId = @BankId THEN 1 ELSE 0 END = 1
		                AND CASE WHEN  @BankId = 0 THEN 1 WHEN @ChequeOrTreatementBankOptionValue = 'Treatement Bank' AND Task_ChequeTreatment.TreatmentBankId = @BankId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @CustomerOrSupplierId = 0 THEN 1 WHEN Task_Collection.CustomerId = @CustomerOrSupplierId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateFrom IS  NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'ChequeDate' AND CAST(Task_ChequeInfo.ChequeDate AS DATE) >= @DateFrom THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateTo IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'ChequeDate' AND CAST(Task_ChequeInfo.ChequeDate AS DATE) <= @DateTo THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateFrom IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'CollectionDate' AND CAST(Task_Collection.CollectionDate AS DATE) >= @DateFrom THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateTo IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'CollectionDate' AND CAST(Task_Collection.CollectionDate AS DATE) <= @DateTo THEN 1 ELSE 0 END = 1
	
		                GROUP BY
		                Task_Collection.CustomerId
		                ,AllCheque.ChequeAmount

		                -------------- Purely Honored -----------------

		                SELECT 
			                Task_Collection.CustomerId AS CustomerOrSupplierId
			                ,COUNT(Task_ChequeInfo.ChequeNo) AS NoOfPurelyHonoredCheque
			                ,SUM(ISNULL(Task_ChequeInfo.Amount,0)) AS PurelyHonoredChequeAmount
			                ,(SUM(ISNULL(Task_ChequeInfo.Amount,0))/AllCheque.ChequeAmount)*100 AS PurelyHonoredChequePercentageAmount

		                INTO #CustomerWisePurelyHonored
		                FROM #CustomerWiseAllCheque AllCheque		
		                LEFT JOIN Task_Collection ON AllCheque.CustomerOrSupplierId = Task_Collection.CustomerId
		                LEFT JOIN Task_CollectionDetail ON Task_Collection.CollectionId = Task_CollectionDetail.CollectionId	
		                LEFT JOIN Task_ChequeInfo ON Task_ChequeInfo.CollectionDetailId =Task_CollectionDetail.CollectionDetailId AND Task_ChequeInfo.Status ='H'
		                LEFT JOIN Task_ChequeTreatment ON Task_ChequeInfo.ChequeInfoId =Task_ChequeTreatment.ChequeInfoId AND Task_ChequeTreatment.Status <> 'D'
		                WHERE 
		                Task_Collection.Approved = 'A'
		                AND CASE WHEN @CompanyId = 0 THEN 1 WHEN Task_ChequeInfo.CompanyId = @CompanyId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @LocationId = 0 THEN 1 WHEN Task_ChequeInfo.LocationId = @LocationId THEN 1 ELSE 0 END = 1
		                AND CASE WHEN  @BankId = 0 THEN 1 WHEN @ChequeOrTreatementBankOptionValue = 'Cheque Bank' AND Task_ChequeInfo.BankId = @BankId THEN 1 ELSE 0 END = 1
		                AND CASE WHEN  @BankId = 0 THEN 1 WHEN @ChequeOrTreatementBankOptionValue = 'Treatement Bank' AND Task_ChequeTreatment.TreatmentBankId = @BankId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @CustomerOrSupplierId = 0 THEN 1 WHEN Task_Collection.CustomerId = @CustomerOrSupplierId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateFrom IS  NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'ChequeDate' AND CAST(Task_ChequeInfo.ChequeDate AS DATE) >= @DateFrom THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateTo IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'ChequeDate' AND CAST(Task_ChequeInfo.ChequeDate AS DATE) <= @DateTo THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateFrom IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'CollectionDate' AND CAST(Task_Collection.CollectionDate AS DATE) >= @DateFrom THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateTo IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'CollectionDate' AND CAST(Task_Collection.CollectionDate AS DATE) <= @DateTo THEN 1 ELSE 0 END = 1
	
		                GROUP BY
		                Task_Collection.CustomerId
		                ,AllCheque.ChequeAmount


		                ---------- final data ------------
		                INSERT INTO TempCustomerOrSupplierWiseChequePerformance (CustomerOrSupplierId,CustomerOrSupplierName,CustomerOrSupplierCode,	CustomerOrSupplierAddress,	CustomerOrSupplierPhoneNo,	NoOfCheque,	ChequeAmount,	NoOfDisHonerCheque,	DisHonerChequeAmount,	DisHonerChequePercentageAmount,	NoOfBalanceAdjustedCheque,	BalanceAdjustedChequeAmount,	BalanceAdjustedChequePercentageAmount,NoOfPurelyHonoredCheque,PurelyHonoredChequeAmount,PurelyHonoredChequePercentageAmount,DateFrom,DateTo,CompanyId,EntryBy,CompanyName,CompanyAddress,CompanyContact,LocationName)
		                SELECT #CustomerWiseAllCheque.CustomerOrSupplierId,Setup_Customer.Name AS CustomerOrSupplierName,Setup_Customer.Code AS CustomerOrSupplierCode,Setup_Customer.Address AS	CustomerOrSupplierAddress,Setup_Customer.PhoneNo AS	CustomerOrSupplierPhoneNo,	NoOfCheque,	#CustomerWiseAllCheque.ChequeAmount,ISNULL( NoOfDisHonerCheque,0),	ISNULL(DisHonerChequeAmount,0),	ISNULL(DisHonerChequePercentageAmount,0),	ISNULL(NoOfBalanceAdjustedCheque,0),	ISNULL(BalanceAdjustedChequeAmount,0),	ISNULL(BalanceAdjustedChequePercentageAmount,0),ISNULL(NoOfPurelyHonoredCheque,0),ISNULL(PurelyHonoredChequeAmount,0),ISNULL(PurelyHonoredChequePercentageAmount,0),@DateFrom,@DateTo,@CompanyId,@EntryBy,Setup_Company.Name AS CompanyName,Setup_Company.Address AS CompanyAddress,Setup_Company.PhoneNo AS CompanyContact,@LocationName
		                FROM #CustomerWiseAllCheque 
		                LEFT JOIN #CustomerWiseDisHonerCheque ON #CustomerWiseAllCheque.CustomerOrSupplierId = #CustomerWiseDisHonerCheque.CustomerOrSupplierId
		                LEFT JOIN #CustomerWiseBalanceAdjustedCheque ON #CustomerWiseAllCheque.CustomerOrSupplierId = #CustomerWiseBalanceAdjustedCheque.CustomerOrSupplierId 
		                LEFT JOIN #CustomerWisePurelyHonored ON #CustomerWiseAllCheque.CustomerOrSupplierId = #CustomerWisePurelyHonored.CustomerOrSupplierId
		                LEFT JOIN Setup_Customer ON #CustomerWiseAllCheque.CustomerOrSupplierId = Setup_Customer.CustomerId
		                LEFT JOIN Setup_Company ON Setup_Company.CompanyId= @CompanyId

		                DROP TABLE #CustomerWiseAllCheque
		                DROP TABLE #CustomerWiseDisHonerCheque
		                DROP TABLE #CustomerWiseBalanceAdjustedCheque
		                DROP TABLE #CustomerWisePurelyHonored
	                END
	                ELSE
	                BEGIN
		                ------------All cheque------------------

		                SELECT 

			                Task_Payment.SupplierId AS CustomerOrSupplierId
			                ,COUNT(Task_ChequeInfo.ChequeNo) AS NoOfCheque
			                ,SUM(ISNULL(Task_ChequeInfo.Amount,0)) AS ChequeAmount

		                INTO #SupplierWiseAllCheque
		                FROM Task_ChequeInfo
		                INNER JOIN Task_PaymentDetail ON Task_ChequeInfo.PaymentDetailId = Task_PaymentDetail.PaymentDetailId
		                INNER JOIN Task_Payment ON Task_PaymentDetail.PaymentId = Task_Payment.PaymentId
		                LEFT JOIN Task_ChequeTreatment ON Task_ChequeInfo.ChequeInfoId = Task_ChequeTreatment.ChequeInfoId

		                WHERE 
		                Task_Payment.Approved = 'A'
		                AND CASE WHEN @CompanyId = 0 THEN 1 WHEN Task_ChequeInfo.CompanyId = @CompanyId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @LocationId = 0 THEN 1 WHEN Task_ChequeInfo.LocationId = @LocationId THEN 1 ELSE 0 END = 1
		                AND CASE WHEN  @BankId = 0 THEN 1 WHEN @ChequeOrTreatementBankOptionValue = 'Cheque Bank' AND Task_ChequeInfo.BankId = @BankId THEN 1 ELSE 0 END = 1
		                AND CASE WHEN  @BankId = 0 THEN 1 WHEN @ChequeOrTreatementBankOptionValue = 'Treatement Bank' AND Task_ChequeTreatment.TreatmentBankId = @BankId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @CustomerOrSupplierId = 0 THEN 1 WHEN Task_Payment.SupplierId = @CustomerOrSupplierId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateFrom IS  NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'ChequeDate' AND CAST(Task_ChequeInfo.ChequeDate AS DATE) >= @DateFrom THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateTo IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'ChequeDate' AND CAST(Task_ChequeInfo.ChequeDate AS DATE) <= @DateTo THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateFrom IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'PaymentDate' AND CAST(Task_Payment.PaymentDate AS DATE) >= @DateFrom THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateTo IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'PaymentDate' AND CAST(Task_Payment.PaymentDate AS DATE) <= @DateTo THEN 1 ELSE 0 END = 1
		
		                GROUP BY
		                Task_Payment.SupplierId

		                -------------- Dis honor cheque -----------------
		                SELECT 
		                Task_Payment.SupplierId AS CustomerOrSupplierId
		                ,AllCheque.ChequeAmount
		                ,COUNT(Task_ChequeInfo.ChequeNo) AS NoOfDisHonerCheque
		                ,SUM(ISNULL(Task_ChequeInfo.Amount,0)) AS DisHonerChequeAmount
		                ,(SUM(ISNULL(Task_ChequeInfo.Amount,0))/AllCheque.ChequeAmount)*100 AS DisHonerChequePercentageAmount

		                INTO #SupplierWiseDisHonerCheque
		                FROM #SupplierWiseAllCheque AllCheque
		                INNER JOIN Task_Payment ON AllCheque.CustomerOrSupplierId = Task_Payment.SupplierId
		                INNER JOIN Task_PaymentDetail ON Task_Payment.PaymentId = Task_PaymentDetail.PaymentId	
		                INNER JOIN Task_ChequeInfo ON Task_ChequeInfo.CollectionDetailId = Task_PaymentDetail.PaymentDetailId AND Task_ChequeInfo.Status ='D'
		                LEFT JOIN Task_ChequeTreatment ON Task_ChequeInfo.ChequeInfoId = Task_ChequeTreatment.ChequeInfoId
		
		                WHERE 
		                Task_Payment.Approved = 'A'
		                AND CASE WHEN @CompanyId = 0 THEN 1 WHEN Task_ChequeInfo.CompanyId = @CompanyId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @LocationId = 0 THEN 1 WHEN Task_ChequeInfo.LocationId = @LocationId THEN 1 ELSE 0 END = 1
		                AND CASE WHEN  @BankId = 0 THEN 1 WHEN @ChequeOrTreatementBankOptionValue = 'Cheque Bank' AND Task_ChequeInfo.BankId = @BankId THEN 1 ELSE 0 END = 1
		                AND CASE WHEN  @BankId = 0 THEN 1 WHEN @ChequeOrTreatementBankOptionValue = 'Treatement Bank' AND Task_ChequeTreatment.TreatmentBankId = @BankId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @CustomerOrSupplierId = 0 THEN 1 WHEN Task_Payment.SupplierId = @CustomerOrSupplierId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateFrom IS  NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'ChequeDate' AND CAST(Task_ChequeInfo.ChequeDate AS DATE) >= @DateFrom THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateTo IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'ChequeDate' AND CAST(Task_ChequeInfo.ChequeDate AS DATE) <= @DateTo THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateFrom IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'PaymentDate' AND CAST(Task_Payment.PaymentDate AS DATE) >= @DateFrom THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateTo IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'PaymentDate' AND CAST(Task_Payment.PaymentDate AS DATE) <= @DateTo THEN 1 ELSE 0 END = 1
	
		                GROUP BY
		                Task_Payment.SupplierId
		                ,AllCheque.ChequeAmount

			                -------------- BalanceAdjusted cheque -----------------

			                SELECT 
		                    Task_Payment.SupplierId AS CustomerOrSupplierId
			                ,AllCheque.ChequeAmount
			                ,COUNT(Task_ChequeInfo.ChequeNo) AS NoOfBalanceAdjustedCheque
			                ,SUM(ISNULL(Task_ChequeInfo.Amount,0)) AS BalanceAdjustedChequeAmount
			                ,(SUM(ISNULL(Task_ChequeInfo.Amount,0))/AllCheque.ChequeAmount)*100 AS BalanceAdjustedChequePercentageAmount

		                INTO #SupplierWiseBalanceAdjustedCheque
		                FROM #SupplierWiseAllCheque AllCheque
		                INNER JOIN Task_Payment ON AllCheque.CustomerOrSupplierId = Task_Payment.SupplierId
		                INNER JOIN Task_PaymentDetail ON Task_Payment.PaymentId = Task_PaymentDetail.PaymentId	
		                INNER JOIN Task_ChequeInfo ON Task_ChequeInfo.CollectionDetailId = Task_PaymentDetail.PaymentDetailId AND Task_ChequeInfo.Status ='B'
		                LEFT JOIN Task_ChequeTreatment ON Task_ChequeInfo.ChequeInfoId = Task_ChequeTreatment.ChequeInfoId
		                WHERE 
		                Task_Payment.Approved = 'A'
		                AND CASE WHEN @CompanyId = 0 THEN 1 WHEN Task_ChequeInfo.CompanyId = @CompanyId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @LocationId = 0 THEN 1 WHEN Task_ChequeInfo.LocationId = @LocationId THEN 1 ELSE 0 END = 1
		                AND CASE WHEN  @BankId = 0 THEN 1 WHEN @ChequeOrTreatementBankOptionValue = 'Cheque Bank' AND Task_ChequeInfo.BankId = @BankId THEN 1 ELSE 0 END = 1
		                AND CASE WHEN  @BankId = 0 THEN 1 WHEN @ChequeOrTreatementBankOptionValue = 'Treatement Bank' AND Task_ChequeTreatment.TreatmentBankId = @BankId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @CustomerOrSupplierId = 0 THEN 1 WHEN Task_Payment.SupplierId = @CustomerOrSupplierId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateFrom IS  NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'ChequeDate' AND CAST(Task_ChequeInfo.ChequeDate AS DATE) >= @DateFrom THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateTo IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'ChequeDate' AND CAST(Task_ChequeInfo.ChequeDate AS DATE) <= @DateTo THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateFrom IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'PaymentDate' AND CAST(Task_Payment.PaymentDate AS DATE) >= @DateFrom THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateTo IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'PaymentDate' AND CAST(Task_Payment.PaymentDate AS DATE) <= @DateTo THEN 1 ELSE 0 END = 1
	
		                GROUP BY
		                Task_Payment.SupplierId
		                ,AllCheque.ChequeAmount

			                -------------- Purely Honored -----------------

		                SELECT 
			                Task_Payment.SupplierId AS CustomerOrSupplierId
			                ,COUNT(Task_ChequeInfo.ChequeNo) AS NoOfPurelyHonoredCheque
			                ,SUM(ISNULL(Task_ChequeInfo.Amount,0)) AS PurelyHonoredChequeAmount
			                ,(SUM(ISNULL(Task_ChequeInfo.Amount,0))/AllCheque.ChequeAmount)*100 AS PurelyHonoredChequePercentageAmount

		                INTO #SupplierWisePurelyHonored
		                FROM #SupplierWiseAllCheque AllCheque		
		                LEFT JOIN Task_Payment ON AllCheque.CustomerOrSupplierId = Task_Payment.SupplierId
		                LEFT JOIN Task_PaymentDetail ON Task_Payment.PaymentId = Task_PaymentDetail.PaymentId	
		                LEFT JOIN Task_ChequeInfo ON Task_ChequeInfo.CollectionDetailId = Task_PaymentDetail.PaymentDetailId AND Task_ChequeInfo.Status ='H'
		                LEFT JOIN Task_ChequeTreatment ON Task_ChequeInfo.ChequeInfoId = Task_ChequeTreatment.ChequeInfoId AND Task_ChequeTreatment.Status <> 'D'
		                WHERE 
		                Task_Payment.Approved = 'A'
		                AND CASE WHEN @CompanyId = 0 THEN 1 WHEN Task_ChequeInfo.CompanyId = @CompanyId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @LocationId = 0 THEN 1 WHEN Task_ChequeInfo.LocationId = @LocationId THEN 1 ELSE 0 END = 1
		                AND CASE WHEN  @BankId = 0 THEN 1 WHEN @ChequeOrTreatementBankOptionValue = 'Cheque Bank' AND Task_ChequeInfo.BankId = @BankId THEN 1 ELSE 0 END = 1
		                AND CASE WHEN  @BankId = 0 THEN 1 WHEN @ChequeOrTreatementBankOptionValue = 'Treatement Bank' AND Task_ChequeTreatment.TreatmentBankId = @BankId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @CustomerOrSupplierId = 0 THEN 1 WHEN Task_Payment.SupplierId = @CustomerOrSupplierId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateFrom IS  NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'ChequeDate' AND CAST(Task_ChequeInfo.ChequeDate AS DATE) >= @DateFrom THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateTo IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'ChequeDate' AND CAST(Task_ChequeInfo.ChequeDate AS DATE) <= @DateTo THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateFrom IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'PaymentDate' AND CAST(Task_Payment.PaymentDate AS DATE) >= @DateFrom THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateTo IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'PaymentDate' AND CAST(Task_Payment.PaymentDate AS DATE) <= @DateTo THEN 1 ELSE 0 END = 1
	
		                GROUP BY
		                Task_Payment.SupplierId
		                ,AllCheque.ChequeAmount


		                INSERT INTO TempCustomerOrSupplierWiseChequePerformance (CustomerOrSupplierId,CustomerOrSupplierName,CustomerOrSupplierCode,	CustomerOrSupplierAddress,	CustomerOrSupplierPhoneNo,	NoOfCheque,	ChequeAmount,	NoOfDisHonerCheque,	DisHonerChequeAmount,	DisHonerChequePercentageAmount,	NoOfBalanceAdjustedCheque,	BalanceAdjustedChequeAmount,	BalanceAdjustedChequePercentageAmount,NoOfPurelyHonoredCheque,PurelyHonoredChequeAmount,PurelyHonoredChequePercentageAmount,DateFrom,DateTo,CompanyId,EntryBy,CompanyName,CompanyAddress,CompanyContact,LocationName)
		                SELECT #SupplierWiseAllCheque.CustomerOrSupplierId,Setup_Customer.Name AS CustomerOrSupplierName,Setup_Customer.Code AS CustomerOrSupplierCode,Setup_Customer.Address AS	CustomerOrSupplierAddress,Setup_Customer.PhoneNo AS	CustomerOrSupplierPhoneNo,	NoOfCheque,	#SupplierWiseAllCheque.ChequeAmount,ISNULL( NoOfDisHonerCheque,0),	ISNULL(DisHonerChequeAmount,0),	ISNULL(DisHonerChequePercentageAmount,0),	ISNULL(NoOfBalanceAdjustedCheque,0),	ISNULL(BalanceAdjustedChequeAmount,0),	ISNULL(BalanceAdjustedChequePercentageAmount,0),ISNULL(NoOfPurelyHonoredCheque,0),ISNULL(PurelyHonoredChequeAmount,0),ISNULL(PurelyHonoredChequePercentageAmount,0),@DateFrom,@DateTo,@CompanyId,@EntryBy,Setup_Company.Name AS CompanyName,Setup_Company.Address AS CompanyAddress,Setup_Company.PhoneNo AS CompanyContact,@LocationName
		                FROM #SupplierWiseAllCheque 
		                LEFT JOIN #SupplierWiseDisHonerCheque ON #SupplierWiseAllCheque.CustomerOrSupplierId = #SupplierWiseDisHonerCheque.CustomerOrSupplierId
		                LEFT JOIN #SupplierWiseBalanceAdjustedCheque ON #SupplierWiseAllCheque.CustomerOrSupplierId = #SupplierWiseBalanceAdjustedCheque.CustomerOrSupplierId 
		                LEFT JOIN #SupplierWisePurelyHonored ON #SupplierWiseAllCheque.CustomerOrSupplierId = #SupplierWisePurelyHonored.CustomerOrSupplierId
		                LEFT JOIN Setup_Customer ON #SupplierWiseAllCheque.CustomerOrSupplierId = Setup_Customer.CustomerId
		                LEFT JOIN Setup_Company ON Setup_Company.CompanyId= @CompanyId

		                DROP TABLE #SupplierWiseAllCheque
		                DROP TABLE #SupplierWiseDisHonerCheque
		                DROP TABLE #SupplierWiseBalanceAdjustedCheque
		                DROP TABLE #SupplierWisePurelyHonored
	                END
                END");
            }

            //if stored procedure found then alter stored procedure
            if (CheckTable("SP_CustomerOrSupplierWiseChequePerformance"))
            {
                _db.Database.ExecuteSqlCommand(@"ALTER PROCEDURE SP_CustomerOrSupplierWiseChequePerformance
                    @CompanyId bigint= 0,
                    @Currency varchar(5)= null,
                    @ChequeType varchar(50) = null,
                    @LocationId bigint= 0,
                    @BankId bigint= 0,
                    @CustomerOrSupplierId bigint= 0,    
                    @DateFrom date,
                    @DateTo date,
                    @ChequeOrTreatementBankOptionValue varchar(50)= null,
                    @ChequeCollectionOrPaymentDateOptionValue varchar(50)= null,
	                @EntryBy bigint = null
                AS
                BEGIN
	                --DROP TABLE TempCustomerOrSupplierWiseChequePerformance

	                DELETE TempCustomerOrSupplierWiseChequePerformance WHERE CompanyId = @CompanyId AND EntryBy = @EntryBy

	                DECLARE @LocationName VARCHAR(500)= NULL;
	                if(@LocationId <>0)
	                BEGIN
		                SET @LocationName = (SELECT Name FROM Setup_Location WHERE LocationId = @LocationId AND CompanyId = @CompanyId)
	                END
	                if(@ChequeType = 'receivedCheque')
	                BEGIN
		                ------------All cheque------------------
		                SELECT 

			                Task_Collection.CustomerId AS CustomerOrSupplierId
			                ,COUNT(Task_ChequeInfo.ChequeNo) AS NoOfCheque
			                ,SUM(ISNULL(Task_ChequeInfo.Amount,0)) AS ChequeAmount

		                INTO #CustomerWiseAllCheque
		                FROM Task_ChequeInfo
		                INNER JOIN Task_CollectionDetail ON Task_ChequeInfo.CollectionDetailId = Task_CollectionDetail.CollectionDetailId
		                INNER JOIN Task_Collection ON Task_CollectionDetail.CollectionId = Task_Collection.CollectionId
		                LEFT JOIN Task_ChequeTreatment ON Task_ChequeInfo.ChequeInfoId = Task_ChequeTreatment.ChequeInfoId

		                WHERE 
		                Task_Collection.Approved = 'A'
		                AND CASE WHEN @CompanyId = 0 THEN 1 WHEN Task_ChequeInfo.CompanyId = @CompanyId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @LocationId = 0 THEN 1 WHEN Task_ChequeInfo.LocationId = @LocationId THEN 1 ELSE 0 END = 1
		                AND CASE WHEN  @BankId = 0 THEN 1 WHEN @ChequeOrTreatementBankOptionValue = 'Cheque Bank' AND Task_ChequeInfo.BankId = @BankId THEN 1 ELSE 0 END = 1
		                AND CASE WHEN  @BankId = 0 THEN 1 WHEN @ChequeOrTreatementBankOptionValue = 'Treatement Bank' AND Task_ChequeTreatment.TreatmentBankId = @BankId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @CustomerOrSupplierId = 0 THEN 1 WHEN Task_Collection.CustomerId = @CustomerOrSupplierId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateFrom IS  NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'ChequeDate' AND CAST(Task_ChequeInfo.ChequeDate AS DATE) >= @DateFrom THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateTo IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'ChequeDate' AND CAST(Task_ChequeInfo.ChequeDate AS DATE) <= @DateTo THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateFrom IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'CollectionDate' AND CAST(Task_Collection.CollectionDate AS DATE) >= @DateFrom THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateTo IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'CollectionDate' AND CAST(Task_Collection.CollectionDate AS DATE) <= @DateTo THEN 1 ELSE 0 END = 1
		
		                GROUP BY
		                Task_Collection.CustomerId

		                -------------- Dis honor cheque -----------------

		                SELECT 

			                Task_Collection.CustomerId AS CustomerOrSupplierId
			                ,AllCheque.ChequeAmount
			                ,COUNT(Task_ChequeInfo.ChequeNo) AS NoOfDisHonerCheque
			                ,SUM(ISNULL(Task_ChequeInfo.Amount,0)) AS DisHonerChequeAmount
			                ,(SUM(ISNULL(Task_ChequeInfo.Amount,0))/AllCheque.ChequeAmount)*100 AS DisHonerChequePercentageAmount

		                INTO #CustomerWiseDisHonerCheque
		                FROM #CustomerWiseAllCheque AllCheque
		                INNER JOIN Task_Collection ON AllCheque.CustomerOrSupplierId = Task_Collection.CustomerId
		                INNER JOIN Task_CollectionDetail ON Task_Collection.CollectionId = Task_CollectionDetail.CollectionId	
		                INNER JOIN Task_ChequeInfo ON Task_ChequeInfo.CollectionDetailId = Task_CollectionDetail.CollectionDetailId AND Task_ChequeInfo.Status ='D'
		                LEFT JOIN Task_ChequeTreatment ON Task_ChequeInfo.ChequeInfoId = Task_ChequeTreatment.ChequeInfoId
		
		                WHERE  
		                Task_Collection.Approved = 'A'
		                AND CASE WHEN @CompanyId = 0 THEN 1 WHEN Task_ChequeInfo.CompanyId = @CompanyId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @LocationId = 0 THEN 1 WHEN Task_ChequeInfo.LocationId = @LocationId THEN 1 ELSE 0 END = 1
		                AND CASE WHEN  @BankId = 0 THEN 1 WHEN @ChequeOrTreatementBankOptionValue = 'Cheque Bank' AND Task_ChequeInfo.BankId = @BankId THEN 1 ELSE 0 END = 1
		                AND CASE WHEN  @BankId = 0 THEN 1 WHEN @ChequeOrTreatementBankOptionValue = 'Treatement Bank' AND Task_ChequeTreatment.TreatmentBankId = @BankId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @CustomerOrSupplierId = 0 THEN 1 WHEN Task_Collection.CustomerId = @CustomerOrSupplierId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateFrom IS  NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'ChequeDate' AND CAST(Task_ChequeInfo.ChequeDate AS DATE) >= @DateFrom THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateTo IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'ChequeDate' AND CAST(Task_ChequeInfo.ChequeDate AS DATE) <= @DateTo THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateFrom IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'CollectionDate' AND CAST(Task_Collection.CollectionDate AS DATE) >= @DateFrom THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateTo IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'CollectionDate' AND CAST(Task_Collection.CollectionDate AS DATE) <= @DateTo THEN 1 ELSE 0 END = 1
	
		                GROUP BY
		                Task_Collection.CustomerId 
		                ,AllCheque.ChequeAmount

		                -------------- BalanceAdjusted cheque -----------------

		                SELECT 
		                    Task_Collection.CustomerId AS CustomerOrSupplierId
			                ,AllCheque.ChequeAmount
			                ,COUNT(Task_ChequeInfo.ChequeNo) AS NoOfBalanceAdjustedCheque
			                ,SUM(ISNULL(Task_ChequeInfo.Amount,0)) AS BalanceAdjustedChequeAmount
			                ,(SUM(ISNULL(Task_ChequeInfo.Amount,0))/AllCheque.ChequeAmount)*100 AS BalanceAdjustedChequePercentageAmount

		                INTO #CustomerWiseBalanceAdjustedCheque
		                FROM #CustomerWiseAllCheque AllCheque
		                INNER JOIN Task_Collection ON AllCheque.CustomerOrSupplierId = Task_Collection.CustomerId
		                INNER JOIN Task_CollectionDetail ON Task_Collection.CollectionId = Task_CollectionDetail.CollectionId	
		                INNER JOIN Task_ChequeInfo ON Task_ChequeInfo.CollectionDetailId =Task_CollectionDetail.CollectionDetailId AND Task_ChequeInfo.Status ='B'
		                LEFT JOIN Task_ChequeTreatment ON Task_ChequeInfo.ChequeInfoId = Task_ChequeTreatment.ChequeInfoId
		                WHERE 
		                Task_Collection.Approved = 'A'
		                AND CASE WHEN @CompanyId = 0 THEN 1 WHEN Task_ChequeInfo.CompanyId = @CompanyId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @LocationId = 0 THEN 1 WHEN Task_ChequeInfo.LocationId = @LocationId THEN 1 ELSE 0 END = 1
		                AND CASE WHEN  @BankId = 0 THEN 1 WHEN @ChequeOrTreatementBankOptionValue = 'Cheque Bank' AND Task_ChequeInfo.BankId = @BankId THEN 1 ELSE 0 END = 1
		                AND CASE WHEN  @BankId = 0 THEN 1 WHEN @ChequeOrTreatementBankOptionValue = 'Treatement Bank' AND Task_ChequeTreatment.TreatmentBankId = @BankId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @CustomerOrSupplierId = 0 THEN 1 WHEN Task_Collection.CustomerId = @CustomerOrSupplierId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateFrom IS  NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'ChequeDate' AND CAST(Task_ChequeInfo.ChequeDate AS DATE) >= @DateFrom THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateTo IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'ChequeDate' AND CAST(Task_ChequeInfo.ChequeDate AS DATE) <= @DateTo THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateFrom IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'CollectionDate' AND CAST(Task_Collection.CollectionDate AS DATE) >= @DateFrom THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateTo IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'CollectionDate' AND CAST(Task_Collection.CollectionDate AS DATE) <= @DateTo THEN 1 ELSE 0 END = 1
	
		                GROUP BY
		                Task_Collection.CustomerId
		                ,AllCheque.ChequeAmount

		                -------------- Purely Honored -----------------

		                SELECT 
			                Task_Collection.CustomerId AS CustomerOrSupplierId
			                ,COUNT(Task_ChequeInfo.ChequeNo) AS NoOfPurelyHonoredCheque
			                ,SUM(ISNULL(Task_ChequeInfo.Amount,0)) AS PurelyHonoredChequeAmount
			                ,(SUM(ISNULL(Task_ChequeInfo.Amount,0))/AllCheque.ChequeAmount)*100 AS PurelyHonoredChequePercentageAmount

		                INTO #CustomerWisePurelyHonored
		                FROM #CustomerWiseAllCheque AllCheque		
		                LEFT JOIN Task_Collection ON AllCheque.CustomerOrSupplierId = Task_Collection.CustomerId
		                LEFT JOIN Task_CollectionDetail ON Task_Collection.CollectionId = Task_CollectionDetail.CollectionId	
		                LEFT JOIN Task_ChequeInfo ON Task_ChequeInfo.CollectionDetailId =Task_CollectionDetail.CollectionDetailId AND Task_ChequeInfo.Status ='H'
		                LEFT JOIN Task_ChequeTreatment ON Task_ChequeInfo.ChequeInfoId =Task_ChequeTreatment.ChequeInfoId AND Task_ChequeTreatment.Status <> 'D'
		                WHERE 
		                Task_Collection.Approved = 'A'
		                AND CASE WHEN @CompanyId = 0 THEN 1 WHEN Task_ChequeInfo.CompanyId = @CompanyId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @LocationId = 0 THEN 1 WHEN Task_ChequeInfo.LocationId = @LocationId THEN 1 ELSE 0 END = 1
		                AND CASE WHEN  @BankId = 0 THEN 1 WHEN @ChequeOrTreatementBankOptionValue = 'Cheque Bank' AND Task_ChequeInfo.BankId = @BankId THEN 1 ELSE 0 END = 1
		                AND CASE WHEN  @BankId = 0 THEN 1 WHEN @ChequeOrTreatementBankOptionValue = 'Treatement Bank' AND Task_ChequeTreatment.TreatmentBankId = @BankId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @CustomerOrSupplierId = 0 THEN 1 WHEN Task_Collection.CustomerId = @CustomerOrSupplierId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateFrom IS  NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'ChequeDate' AND CAST(Task_ChequeInfo.ChequeDate AS DATE) >= @DateFrom THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateTo IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'ChequeDate' AND CAST(Task_ChequeInfo.ChequeDate AS DATE) <= @DateTo THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateFrom IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'CollectionDate' AND CAST(Task_Collection.CollectionDate AS DATE) >= @DateFrom THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateTo IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'CollectionDate' AND CAST(Task_Collection.CollectionDate AS DATE) <= @DateTo THEN 1 ELSE 0 END = 1
	
		                GROUP BY
		                Task_Collection.CustomerId
		                ,AllCheque.ChequeAmount


		                ---------- final data ------------
		                INSERT INTO TempCustomerOrSupplierWiseChequePerformance (CustomerOrSupplierId,CustomerOrSupplierName,CustomerOrSupplierCode,	CustomerOrSupplierAddress,	CustomerOrSupplierPhoneNo,	NoOfCheque,	ChequeAmount,	NoOfDisHonerCheque,	DisHonerChequeAmount,	DisHonerChequePercentageAmount,	NoOfBalanceAdjustedCheque,	BalanceAdjustedChequeAmount,	BalanceAdjustedChequePercentageAmount,NoOfPurelyHonoredCheque,PurelyHonoredChequeAmount,PurelyHonoredChequePercentageAmount,DateFrom,DateTo,CompanyId,EntryBy,CompanyName,CompanyAddress,CompanyContact,LocationName)
		                SELECT #CustomerWiseAllCheque.CustomerOrSupplierId,Setup_Customer.Name AS CustomerOrSupplierName,Setup_Customer.Code AS CustomerOrSupplierCode,Setup_Customer.Address AS	CustomerOrSupplierAddress,Setup_Customer.PhoneNo AS	CustomerOrSupplierPhoneNo,	NoOfCheque,	#CustomerWiseAllCheque.ChequeAmount,ISNULL( NoOfDisHonerCheque,0),	ISNULL(DisHonerChequeAmount,0),	ISNULL(DisHonerChequePercentageAmount,0),	ISNULL(NoOfBalanceAdjustedCheque,0),	ISNULL(BalanceAdjustedChequeAmount,0),	ISNULL(BalanceAdjustedChequePercentageAmount,0),ISNULL(NoOfPurelyHonoredCheque,0),ISNULL(PurelyHonoredChequeAmount,0),ISNULL(PurelyHonoredChequePercentageAmount,0),@DateFrom,@DateTo,@CompanyId,@EntryBy,Setup_Company.Name AS CompanyName,Setup_Company.Address AS CompanyAddress,Setup_Company.PhoneNo AS CompanyContact,@LocationName
		                FROM #CustomerWiseAllCheque 
		                LEFT JOIN #CustomerWiseDisHonerCheque ON #CustomerWiseAllCheque.CustomerOrSupplierId = #CustomerWiseDisHonerCheque.CustomerOrSupplierId
		                LEFT JOIN #CustomerWiseBalanceAdjustedCheque ON #CustomerWiseAllCheque.CustomerOrSupplierId = #CustomerWiseBalanceAdjustedCheque.CustomerOrSupplierId 
		                LEFT JOIN #CustomerWisePurelyHonored ON #CustomerWiseAllCheque.CustomerOrSupplierId = #CustomerWisePurelyHonored.CustomerOrSupplierId
		                LEFT JOIN Setup_Customer ON #CustomerWiseAllCheque.CustomerOrSupplierId = Setup_Customer.CustomerId
		                LEFT JOIN Setup_Company ON Setup_Company.CompanyId= @CompanyId

		                DROP TABLE #CustomerWiseAllCheque
		                DROP TABLE #CustomerWiseDisHonerCheque
		                DROP TABLE #CustomerWiseBalanceAdjustedCheque
		                DROP TABLE #CustomerWisePurelyHonored
	                END
	                ELSE
	                BEGIN
		                ------------All cheque------------------

		                SELECT 

			                Task_Payment.SupplierId AS CustomerOrSupplierId
			                ,COUNT(Task_ChequeInfo.ChequeNo) AS NoOfCheque
			                ,SUM(ISNULL(Task_ChequeInfo.Amount,0)) AS ChequeAmount

		                INTO #SupplierWiseAllCheque
		                FROM Task_ChequeInfo
		                INNER JOIN Task_PaymentDetail ON Task_ChequeInfo.PaymentDetailId = Task_PaymentDetail.PaymentDetailId
		                INNER JOIN Task_Payment ON Task_PaymentDetail.PaymentId = Task_Payment.PaymentId
		                LEFT JOIN Task_ChequeTreatment ON Task_ChequeInfo.ChequeInfoId = Task_ChequeTreatment.ChequeInfoId

		                WHERE 
		                Task_Payment.Approved = 'A'
		                AND CASE WHEN @CompanyId = 0 THEN 1 WHEN Task_ChequeInfo.CompanyId = @CompanyId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @LocationId = 0 THEN 1 WHEN Task_ChequeInfo.LocationId = @LocationId THEN 1 ELSE 0 END = 1
		                AND CASE WHEN  @BankId = 0 THEN 1 WHEN @ChequeOrTreatementBankOptionValue = 'Cheque Bank' AND Task_ChequeInfo.BankId = @BankId THEN 1 ELSE 0 END = 1
		                AND CASE WHEN  @BankId = 0 THEN 1 WHEN @ChequeOrTreatementBankOptionValue = 'Treatement Bank' AND Task_ChequeTreatment.TreatmentBankId = @BankId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @CustomerOrSupplierId = 0 THEN 1 WHEN Task_Payment.SupplierId = @CustomerOrSupplierId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateFrom IS  NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'ChequeDate' AND CAST(Task_ChequeInfo.ChequeDate AS DATE) >= @DateFrom THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateTo IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'ChequeDate' AND CAST(Task_ChequeInfo.ChequeDate AS DATE) <= @DateTo THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateFrom IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'PaymentDate' AND CAST(Task_Payment.PaymentDate AS DATE) >= @DateFrom THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateTo IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'PaymentDate' AND CAST(Task_Payment.PaymentDate AS DATE) <= @DateTo THEN 1 ELSE 0 END = 1
		
		                GROUP BY
		                Task_Payment.SupplierId

		                -------------- Dis honor cheque -----------------
		                SELECT 
		                Task_Payment.SupplierId AS CustomerOrSupplierId
		                ,AllCheque.ChequeAmount
		                ,COUNT(Task_ChequeInfo.ChequeNo) AS NoOfDisHonerCheque
		                ,SUM(ISNULL(Task_ChequeInfo.Amount,0)) AS DisHonerChequeAmount
		                ,(SUM(ISNULL(Task_ChequeInfo.Amount,0))/AllCheque.ChequeAmount)*100 AS DisHonerChequePercentageAmount

		                INTO #SupplierWiseDisHonerCheque
		                FROM #SupplierWiseAllCheque AllCheque
		                INNER JOIN Task_Payment ON AllCheque.CustomerOrSupplierId = Task_Payment.SupplierId
		                INNER JOIN Task_PaymentDetail ON Task_Payment.PaymentId = Task_PaymentDetail.PaymentId	
		                INNER JOIN Task_ChequeInfo ON Task_ChequeInfo.CollectionDetailId = Task_PaymentDetail.PaymentDetailId AND Task_ChequeInfo.Status ='D'
		                LEFT JOIN Task_ChequeTreatment ON Task_ChequeInfo.ChequeInfoId = Task_ChequeTreatment.ChequeInfoId
		
		                WHERE 
		                Task_Payment.Approved = 'A'
		                AND CASE WHEN @CompanyId = 0 THEN 1 WHEN Task_ChequeInfo.CompanyId = @CompanyId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @LocationId = 0 THEN 1 WHEN Task_ChequeInfo.LocationId = @LocationId THEN 1 ELSE 0 END = 1
		                AND CASE WHEN  @BankId = 0 THEN 1 WHEN @ChequeOrTreatementBankOptionValue = 'Cheque Bank' AND Task_ChequeInfo.BankId = @BankId THEN 1 ELSE 0 END = 1
		                AND CASE WHEN  @BankId = 0 THEN 1 WHEN @ChequeOrTreatementBankOptionValue = 'Treatement Bank' AND Task_ChequeTreatment.TreatmentBankId = @BankId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @CustomerOrSupplierId = 0 THEN 1 WHEN Task_Payment.SupplierId = @CustomerOrSupplierId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateFrom IS  NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'ChequeDate' AND CAST(Task_ChequeInfo.ChequeDate AS DATE) >= @DateFrom THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateTo IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'ChequeDate' AND CAST(Task_ChequeInfo.ChequeDate AS DATE) <= @DateTo THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateFrom IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'PaymentDate' AND CAST(Task_Payment.PaymentDate AS DATE) >= @DateFrom THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateTo IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'PaymentDate' AND CAST(Task_Payment.PaymentDate AS DATE) <= @DateTo THEN 1 ELSE 0 END = 1
	
		                GROUP BY
		                Task_Payment.SupplierId
		                ,AllCheque.ChequeAmount

			                -------------- BalanceAdjusted cheque -----------------

			                SELECT 
		                    Task_Payment.SupplierId AS CustomerOrSupplierId
			                ,AllCheque.ChequeAmount
			                ,COUNT(Task_ChequeInfo.ChequeNo) AS NoOfBalanceAdjustedCheque
			                ,SUM(ISNULL(Task_ChequeInfo.Amount,0)) AS BalanceAdjustedChequeAmount
			                ,(SUM(ISNULL(Task_ChequeInfo.Amount,0))/AllCheque.ChequeAmount)*100 AS BalanceAdjustedChequePercentageAmount

		                INTO #SupplierWiseBalanceAdjustedCheque
		                FROM #SupplierWiseAllCheque AllCheque
		                INNER JOIN Task_Payment ON AllCheque.CustomerOrSupplierId = Task_Payment.SupplierId
		                INNER JOIN Task_PaymentDetail ON Task_Payment.PaymentId = Task_PaymentDetail.PaymentId	
		                INNER JOIN Task_ChequeInfo ON Task_ChequeInfo.CollectionDetailId = Task_PaymentDetail.PaymentDetailId AND Task_ChequeInfo.Status ='B'
		                LEFT JOIN Task_ChequeTreatment ON Task_ChequeInfo.ChequeInfoId = Task_ChequeTreatment.ChequeInfoId
		                WHERE 
		                Task_Payment.Approved = 'A'
		                AND CASE WHEN @CompanyId = 0 THEN 1 WHEN Task_ChequeInfo.CompanyId = @CompanyId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @LocationId = 0 THEN 1 WHEN Task_ChequeInfo.LocationId = @LocationId THEN 1 ELSE 0 END = 1
		                AND CASE WHEN  @BankId = 0 THEN 1 WHEN @ChequeOrTreatementBankOptionValue = 'Cheque Bank' AND Task_ChequeInfo.BankId = @BankId THEN 1 ELSE 0 END = 1
		                AND CASE WHEN  @BankId = 0 THEN 1 WHEN @ChequeOrTreatementBankOptionValue = 'Treatement Bank' AND Task_ChequeTreatment.TreatmentBankId = @BankId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @CustomerOrSupplierId = 0 THEN 1 WHEN Task_Payment.SupplierId = @CustomerOrSupplierId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateFrom IS  NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'ChequeDate' AND CAST(Task_ChequeInfo.ChequeDate AS DATE) >= @DateFrom THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateTo IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'ChequeDate' AND CAST(Task_ChequeInfo.ChequeDate AS DATE) <= @DateTo THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateFrom IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'PaymentDate' AND CAST(Task_Payment.PaymentDate AS DATE) >= @DateFrom THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateTo IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'PaymentDate' AND CAST(Task_Payment.PaymentDate AS DATE) <= @DateTo THEN 1 ELSE 0 END = 1
	
		                GROUP BY
		                Task_Payment.SupplierId
		                ,AllCheque.ChequeAmount

			                -------------- Purely Honored -----------------

		                SELECT 
			                Task_Payment.SupplierId AS CustomerOrSupplierId
			                ,COUNT(Task_ChequeInfo.ChequeNo) AS NoOfPurelyHonoredCheque
			                ,SUM(ISNULL(Task_ChequeInfo.Amount,0)) AS PurelyHonoredChequeAmount
			                ,(SUM(ISNULL(Task_ChequeInfo.Amount,0))/AllCheque.ChequeAmount)*100 AS PurelyHonoredChequePercentageAmount

		                INTO #SupplierWisePurelyHonored
		                FROM #SupplierWiseAllCheque AllCheque		
		                LEFT JOIN Task_Payment ON AllCheque.CustomerOrSupplierId = Task_Payment.SupplierId
		                LEFT JOIN Task_PaymentDetail ON Task_Payment.PaymentId = Task_PaymentDetail.PaymentId	
		                LEFT JOIN Task_ChequeInfo ON Task_ChequeInfo.CollectionDetailId = Task_PaymentDetail.PaymentDetailId AND Task_ChequeInfo.Status ='H'
		                LEFT JOIN Task_ChequeTreatment ON Task_ChequeInfo.ChequeInfoId = Task_ChequeTreatment.ChequeInfoId AND Task_ChequeTreatment.Status <> 'D'
		                WHERE 
		                Task_Payment.Approved = 'A'
		                AND CASE WHEN @CompanyId = 0 THEN 1 WHEN Task_ChequeInfo.CompanyId = @CompanyId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @LocationId = 0 THEN 1 WHEN Task_ChequeInfo.LocationId = @LocationId THEN 1 ELSE 0 END = 1
		                AND CASE WHEN  @BankId = 0 THEN 1 WHEN @ChequeOrTreatementBankOptionValue = 'Cheque Bank' AND Task_ChequeInfo.BankId = @BankId THEN 1 ELSE 0 END = 1
		                AND CASE WHEN  @BankId = 0 THEN 1 WHEN @ChequeOrTreatementBankOptionValue = 'Treatement Bank' AND Task_ChequeTreatment.TreatmentBankId = @BankId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @CustomerOrSupplierId = 0 THEN 1 WHEN Task_Payment.SupplierId = @CustomerOrSupplierId THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateFrom IS  NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'ChequeDate' AND CAST(Task_ChequeInfo.ChequeDate AS DATE) >= @DateFrom THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateTo IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'ChequeDate' AND CAST(Task_ChequeInfo.ChequeDate AS DATE) <= @DateTo THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateFrom IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'PaymentDate' AND CAST(Task_Payment.PaymentDate AS DATE) >= @DateFrom THEN 1 ELSE 0 END = 1
		                AND  CASE WHEN @DateTo IS NOT NULL THEN 1 WHEN @ChequeCollectionOrPaymentDateOptionValue = 'PaymentDate' AND CAST(Task_Payment.PaymentDate AS DATE) <= @DateTo THEN 1 ELSE 0 END = 1
	
		                GROUP BY
		                Task_Payment.SupplierId
		                ,AllCheque.ChequeAmount


		                INSERT INTO TempCustomerOrSupplierWiseChequePerformance (CustomerOrSupplierId,CustomerOrSupplierName,CustomerOrSupplierCode,	CustomerOrSupplierAddress,	CustomerOrSupplierPhoneNo,	NoOfCheque,	ChequeAmount,	NoOfDisHonerCheque,	DisHonerChequeAmount,	DisHonerChequePercentageAmount,	NoOfBalanceAdjustedCheque,	BalanceAdjustedChequeAmount,	BalanceAdjustedChequePercentageAmount,NoOfPurelyHonoredCheque,PurelyHonoredChequeAmount,PurelyHonoredChequePercentageAmount,DateFrom,DateTo,CompanyId,EntryBy,CompanyName,CompanyAddress,CompanyContact,LocationName)
		                SELECT #SupplierWiseAllCheque.CustomerOrSupplierId,Setup_Customer.Name AS CustomerOrSupplierName,Setup_Customer.Code AS CustomerOrSupplierCode,Setup_Customer.Address AS	CustomerOrSupplierAddress,Setup_Customer.PhoneNo AS	CustomerOrSupplierPhoneNo,	NoOfCheque,	#SupplierWiseAllCheque.ChequeAmount,ISNULL( NoOfDisHonerCheque,0),	ISNULL(DisHonerChequeAmount,0),	ISNULL(DisHonerChequePercentageAmount,0),	ISNULL(NoOfBalanceAdjustedCheque,0),	ISNULL(BalanceAdjustedChequeAmount,0),	ISNULL(BalanceAdjustedChequePercentageAmount,0),ISNULL(NoOfPurelyHonoredCheque,0),ISNULL(PurelyHonoredChequeAmount,0),ISNULL(PurelyHonoredChequePercentageAmount,0),@DateFrom,@DateTo,@CompanyId,@EntryBy,Setup_Company.Name AS CompanyName,Setup_Company.Address AS CompanyAddress,Setup_Company.PhoneNo AS CompanyContact,@LocationName
		                FROM #SupplierWiseAllCheque 
		                LEFT JOIN #SupplierWiseDisHonerCheque ON #SupplierWiseAllCheque.CustomerOrSupplierId = #SupplierWiseDisHonerCheque.CustomerOrSupplierId
		                LEFT JOIN #SupplierWiseBalanceAdjustedCheque ON #SupplierWiseAllCheque.CustomerOrSupplierId = #SupplierWiseBalanceAdjustedCheque.CustomerOrSupplierId 
		                LEFT JOIN #SupplierWisePurelyHonored ON #SupplierWiseAllCheque.CustomerOrSupplierId = #SupplierWisePurelyHonored.CustomerOrSupplierId
		                LEFT JOIN Setup_Customer ON #SupplierWiseAllCheque.CustomerOrSupplierId = Setup_Customer.CustomerId
		                LEFT JOIN Setup_Company ON Setup_Company.CompanyId= @CompanyId

		                DROP TABLE #SupplierWiseAllCheque
		                DROP TABLE #SupplierWiseDisHonerCheque
		                DROP TABLE #SupplierWiseBalanceAdjustedCheque
		                DROP TABLE #SupplierWisePurelyHonored
	                END
                END");
            }
            //end CreateOrAlterStoredProcedure method
        }

        private void InsertOrUpdateTableDefaultData()
        {
            #region Others_Report
            #region Update
            if (CheckTableRecord("Others_Report", "ReportFor", "Stock", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("UPDATE Others_Report SET ReportFor = 'StockType' WHERE ReportFor = 'Stock'");
            }

            if (CheckTableRecord("Others_Report", "ReportFor", "SalesAnalysis", "ReportName", "Date_Wise_Sales_With_Collection"))
            {
                _db.Database.ExecuteSqlCommand("UPDATE Others_Report SET ReportName = 'Date_Wise_Sales_Invoice_With_Collection' WHERE ReportFor = 'SalesAnalysis' AND ReportName = 'Date_Wise_Sales_With_Collection'");
            }
            #endregion

            #region Insert
            if (!CheckTableRecord("Others_Report", "ReportFor", "SalesAnalysis", "ReportName", "Date_Wise_Sales_Invoice_With_Collection"))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO Others_Report(Id,ReportFor,ReportName) VALUES ('38','SalesAnalysis','Date_Wise_Sales_Invoice_With_Collection')");
            }

            if (!CheckTableRecord("Others_Report", "ReportFor", "StockReport", "ReportName", "Group_And_Category_Wise_Detail"))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO Others_Report(Id,ReportFor,ReportName) VALUES ('65','StockReport','Group_And_Category_Wise_Detail')");
            }

            if (!CheckTableRecord("Others_Report", "ReportFor", "StockReport", "ReportName", "Group_And_Brand_Wise_Detail"))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO Others_Report(Id,ReportFor,ReportName) VALUES ('66','StockReport','Group_And_Brand_Wise_Detail')");
            }

            if (!CheckTableRecord("Others_Report", "ReportFor", "StockReport", "ReportName", "Supplier_Wise"))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO Others_Report(Id,ReportFor,ReportName) VALUES ('67','StockReport','Supplier_Wise')");
            }

            if (!CheckTableRecord("Others_Report", "ReportFor", "PurchaseAnalysis", "ReportName", "Date_Wise_Purchase_Detail"))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO Others_Report(Id,ReportFor,ReportName) VALUES ('68','PurchaseAnalysis','Date_Wise_Purchase_Detail')");
            }

            if (!CheckTableRecord("Others_Report", "ReportFor", "SalesAnalysis", "ReportName", "Date_Wise_Sales_Detail_With_Serial"))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO Others_Report(Id,ReportFor,ReportName) VALUES ('69','SalesAnalysis','Date_Wise_Sales_Detail_With_Serial')");
            }

            if (!CheckTableRecord("Others_Report", "ReportFor", "SalesAnalysis", "ReportName", "Date_Wise_Sales_Order_With_Collection"))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO Others_Report(Id,ReportFor,ReportName) VALUES ('70','SalesAnalysis','Date_Wise_Sales_Order_With_Collection')");
            }

            if (!CheckTableRecord("Others_Report", "ReportFor", "SalesAnalysis", "ReportName", "SalesPerson_Wise_Sales_Detail_With_Serial"))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO Others_Report(Id,ReportFor,ReportName) VALUES ('71','SalesAnalysis','SalesPerson_Wise_Sales_Detail_With_Serial')");
            }

            if (!CheckTableRecord("Others_Report", "ReportFor", "SalesAnalysis", "ReportName", "SalesPerson_Wise_Sales_Detail"))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO Others_Report(Id,ReportFor,ReportName) VALUES ('72','SalesAnalysis','SalesPerson_Wise_Sales_Detail')");
            }
            #endregion
            #endregion

            if (!CheckTableRecord("Setup_GovtDuty", "DutyName", "CD", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_GovtDuty(GovtDutyId,DutyName,DutyOrder) VALUES ('1','CD','1')");
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_GovtDuty(GovtDutyId,DutyName,DutyOrder) VALUES ('2','SD','2')");
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_GovtDuty(GovtDutyId,DutyName,DutyOrder) VALUES ('3','VAT','3')");
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_GovtDuty(GovtDutyId,DutyName,DutyOrder) VALUES ('4','AIT','4')");
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_GovtDuty(GovtDutyId,DutyName,DutyOrder) VALUES ('5','RD','5')");
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_GovtDuty(GovtDutyId,DutyName,DutyOrder) VALUES ('6','ATV','6')");
            }

            if (!CheckTableRecord("Configuration_FormattingTag", "TagName", "CollectionMode", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO Configuration_FormattingTag(Id,TagName,Type) VALUES ('6','CollectionMode','Default')");
                _db.Database.ExecuteSqlCommand("INSERT INTO Configuration_FormattingTag(Id,TagName,Type) VALUES ('7','CollectionNo','Default')");
                _db.Database.ExecuteSqlCommand("INSERT INTO Configuration_FormattingTag(Id,TagName,Type) VALUES ('8','Customer','Default')");
                _db.Database.ExecuteSqlCommand("INSERT INTO Configuration_FormattingTag(Id,TagName,Type) VALUES ('9','Bank','Default')");
                _db.Database.ExecuteSqlCommand("INSERT INTO Configuration_FormattingTag(Id,TagName,Type) VALUES ('10','Reference','Default')");
                _db.Database.ExecuteSqlCommand("INSERT INTO Configuration_FormattingTag(Id,TagName,Type) VALUES ('11','ChequeNo','Default')");
                _db.Database.ExecuteSqlCommand("INSERT INTO Configuration_FormattingTag(Id,TagName,Type) VALUES ('12','PaymentMode','Default')");
                _db.Database.ExecuteSqlCommand("INSERT INTO Configuration_FormattingTag(Id,TagName,Type) VALUES ('13','PaymentNo','Default')");
                _db.Database.ExecuteSqlCommand("INSERT INTO Configuration_FormattingTag(Id,TagName,Type) VALUES ('14','Supplier','Default')");
                _db.Database.ExecuteSqlCommand("INSERT INTO Configuration_FormattingTag(Id,TagName,Type) VALUES ('15','SalesMode','Default')");
                _db.Database.ExecuteSqlCommand("INSERT INTO Configuration_FormattingTag(Id,TagName,Type) VALUES ('16','SalesNo','Default')");
                _db.Database.ExecuteSqlCommand("INSERT INTO Configuration_FormattingTag(Id,TagName,Type) VALUES ('17','PurchaseNo','Default')");
                _db.Database.ExecuteSqlCommand("INSERT INTO Configuration_FormattingTag(Id,TagName,Type) VALUES ('18','PurchaseReturnNo','Default')");
                _db.Database.ExecuteSqlCommand("INSERT INTO Configuration_FormattingTag(Id,TagName,Type) VALUES ('19','Reason','Default')");
                _db.Database.ExecuteSqlCommand("INSERT INTO Configuration_FormattingTag(Id,TagName,Type) VALUES ('20','SalesReturnNo','Default')");
            }

            if (!CheckTableRecord("Configuration_FormattingTag", "TagName", "CustomerDeliveryNo", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO Configuration_FormattingTag(Id,TagName,Type) VALUES ('21','CustomerDeliveryNo','Default')");
                _db.Database.ExecuteSqlCommand("INSERT INTO Configuration_FormattingTag(Id,TagName,Type) VALUES ('22','ReplacementReceiveNo','Default')");
                _db.Database.ExecuteSqlCommand("INSERT INTO Configuration_FormattingTag(Id,TagName,Type) VALUES ('23','AdjustmentNo','Default')");
                _db.Database.ExecuteSqlCommand("INSERT INTO Configuration_FormattingTag(Id,TagName,Type) VALUES ('24','ChequeDate','Default')");
            }

            if (!CheckTableRecord("Setup_GovtDutyAdjustment", "AdjustmentName", "TDS", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_GovtDutyAdjustment(AdjustmentId,AdjustmentName) VALUES ('1','TDS')");
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_GovtDutyAdjustment(AdjustmentId,AdjustmentName) VALUES ('2','VDS')");
            }

            if (!CheckTableRecord("Setup_CostingGroup", "Name", "LC Opening/Bank Charges", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_CostingGroup(CostingGroupId,Code,Name) VALUES ('1','001','LC Opening/Bank Charges')");
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_CostingGroup(CostingGroupId,Code,Name) VALUES ('2','002','Duties & Others')");
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_CostingGroup(CostingGroupId,Code,Name) VALUES ('3','003','Insurance')");
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_CostingGroup(CostingGroupId,Code,Name) VALUES ('4','004','Transport Charges')");
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_CostingGroup(CostingGroupId,Code,Name) VALUES ('5','005','Freight Charges')");
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_CostingGroup(CostingGroupId,Code,Name) VALUES ('6','006','CNF Charges')");
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_CostingGroup(CostingGroupId,Code,Name) VALUES ('7','007','VAT & Taxes')");
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_CostingGroup(CostingGroupId,Code,Name) VALUES ('8','008','Overhead & Others')");
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_CostingGroup(CostingGroupId,Code,Name) VALUES ('9','009','Miscellaneous')");
            }

            if (CheckTableRecord("Setup_CostingGroup", "Name", "LC Opening/Bank Charges", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("UPDATE Setup_CostingGroup SET Code = '001' WHERE CostingGroupId = '1'");
                _db.Database.ExecuteSqlCommand("UPDATE Setup_CostingGroup SET Code = '002' WHERE CostingGroupId = '2'");
                _db.Database.ExecuteSqlCommand("UPDATE Setup_CostingGroup SET Code = '003' WHERE CostingGroupId = '3'");
                _db.Database.ExecuteSqlCommand("UPDATE Setup_CostingGroup SET Code = '004' WHERE CostingGroupId = '4'");
                _db.Database.ExecuteSqlCommand("UPDATE Setup_CostingGroup SET Code = '005' WHERE CostingGroupId = '5'");
                _db.Database.ExecuteSqlCommand("UPDATE Setup_CostingGroup SET Code = '006' WHERE CostingGroupId = '6'");
                _db.Database.ExecuteSqlCommand("UPDATE Setup_CostingGroup SET Code = '007' WHERE CostingGroupId = '7'");
                _db.Database.ExecuteSqlCommand("UPDATE Setup_CostingGroup SET Code = '008' WHERE CostingGroupId = '8'");
                _db.Database.ExecuteSqlCommand("UPDATE Setup_CostingGroup SET Code = '009' WHERE CostingGroupId = '9'");

                _db.Database.ExecuteSqlCommand("ALTER TABLE Setup_CostingGroup ALTER COLUMN Code [varchar](3) NOT NULL");
            }

            #region Operational Event
            if (!CheckTableRecord("Configuration_OperationalEvent", "EventName", "Import", "SubEventName", "LC"))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO Configuration_OperationalEvent(EventName,SubEventName) VALUES ('Import','LC')");
            }
            #endregion

            #region Menu Insert
            if (!CheckTableRecord("Others_Menu", "MenuCode", "M0194", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO [Others_Menu] ([MenuId],[MenuLevel],[MenuTitle],[MenuOrder],[ParentId],[MenuCode]) VALUES ('237','3','Govt. Duty Rate (Location)','8','19','M0194')");
            }

            if (!CheckTableRecord("Others_Menu", "MenuId", "238", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO [Others_Menu] ([MenuId],[MenuLevel],[MenuTitle],[MenuOrder],[ParentId]) VALUES ('238','2','Import','11','1')");
            }

            if (!CheckTableRecord("Others_Menu", "MenuCode", "M0195", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO [Others_Menu] ([MenuId],[MenuLevel],[MenuTitle],[MenuOrder],[ParentId],[MenuCode]) VALUES ('239','3','Costing Control','1','238','M0195')");
            }

            if (!CheckTableRecord("Others_Menu", "MenuCode", "M0196", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO [Others_Menu] ([MenuId],[MenuLevel],[MenuTitle],[MenuOrder],[ParentId],[MenuCode]) VALUES ('240','3','Costing Head','2','238','M0196')");
            }

            if (!CheckTableRecord("Others_Menu", "MenuCode", "M0197", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO [Others_Menu] ([MenuId],[MenuLevel],[MenuTitle],[MenuOrder],[ParentId],[MenuCode]) VALUES ('241','3','LC Opening','2','20','M0197')");
            }

            if (!CheckTableRecord("Others_Menu", "MenuCode", "M0198", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO [Others_Menu] ([MenuId],[MenuLevel],[MenuTitle],[MenuOrder],[ParentId],[MenuCode]) VALUES ('242','3','Debit / Payment','6','231','M0198')");
            }

            if (!CheckTableRecord("Others_Menu", "MenuCode", "M0199", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO [Others_Menu] ([MenuId],[MenuLevel],[MenuTitle],[MenuOrder],[ParentId],[MenuCode]) VALUES ('243','3','Credit / Receive','7','231','M0199')");
            }

            if (!CheckTableRecord("Others_Menu", "MenuCode", "M0200", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO [Others_Menu] ([MenuId],[MenuLevel],[MenuTitle],[MenuOrder],[ParentId],[MenuCode]) VALUES ('244','3','Contra','8','231','M0200')");
            }

            if (!CheckTableRecord("Others_Menu", "MenuCode", "M0201", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO [Others_Menu] ([MenuId],[MenuLevel],[MenuTitle],[MenuOrder],[ParentId],[MenuCode]) VALUES ('245','3','Journal','9','231','M0201')");
            }

            if (!CheckTableRecord("Others_Menu", "MenuCode", "M0202", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO [Others_Menu] ([MenuId],[MenuLevel],[MenuTitle],[MenuOrder],[ParentId],[MenuCode]) VALUES ('246','3','LC Opening','2','30','M0202')");
            }

            if (!CheckTableRecord("Others_Menu", "MenuCode", "M0203", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO [Others_Menu] ([MenuId],[MenuLevel],[MenuTitle],[MenuOrder],[ParentId],[MenuCode]) VALUES ('247','3','LC Opening','2','38','M0203')");
            }

            if (!CheckTableRecord("Others_Menu", "MenuCode", "M0204", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO [Others_Menu] ([MenuId],[MenuLevel],[MenuTitle],[MenuOrder],[ParentId],[MenuCode]) VALUES ('248','3','LC Opening','2','46','M0204')");
            }

            if (!CheckTableRecord("Others_Menu", "MenuCode", "M0205", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO [Others_Menu] ([MenuId],[MenuLevel],[MenuTitle],[MenuOrder],[ParentId],[MenuCode]) VALUES ('249','3','Import Costing','3','20','M0205')");
            }

            if (!CheckTableRecord("Others_Menu", "MenuCode", "M0206", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO [Others_Menu] ([MenuId],[MenuLevel],[MenuTitle],[MenuOrder],[ParentId],[MenuCode]) VALUES ('250','3','Import Costing','3','46','M0206')");
            }

            if (!CheckTableRecord("Others_Menu", "MenuCode", "M0207", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO [Others_Menu] ([MenuId],[MenuLevel],[MenuTitle],[MenuOrder],[ParentId],[MenuCode]) VALUES ('251','3','Import Costing','3','38','M0207')");
            }

            if (!CheckTableRecord("Others_Menu", "MenuCode", "M0208", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO [Others_Menu] ([MenuId],[MenuLevel],[MenuTitle],[MenuOrder],[ParentId],[MenuCode]) VALUES ('252','3','Import Costing','3','30','M0208')");
            }

            if (!CheckTableRecord("Others_Menu", "MenuId", "253", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO [Others_Menu] ([MenuId],[MenuLevel],[MenuTitle],[MenuOrder],[ParentId]) VALUES ('253','2','List Report','11','6')");
            }

            if (!CheckTableRecord("Others_Menu", "MenuCode", "M0209", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO [Others_Menu] ([MenuId],[MenuLevel],[MenuTitle],[MenuOrder],[ParentId],[MenuCode]) VALUES ('254','3','Product List','1','253','M0209')");
            }

            if (!CheckTableRecord("Others_Menu", "MenuCode", "M0210", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO [Others_Menu] ([MenuId],[MenuLevel],[MenuTitle],[MenuOrder],[ParentId],[MenuCode]) VALUES ('255','3','Customer List','2','253','M0210')");
            }

            if (!CheckTableRecord("Others_Menu", "MenuCode", "M0211", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO [Others_Menu] ([MenuId],[MenuLevel],[MenuTitle],[MenuOrder],[ParentId],[MenuCode]) VALUES ('256','3','Supplier List','3','253','M0211')");
            }

            if (!CheckTableRecord("Others_Menu", "MenuLevel", "1", "MenuTitle", "Data Import"))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO [Others_Menu] ([MenuId],[MenuLevel],[MenuTitle],[MenuOrder]) VALUES ('257','1','Data Import','7')");
            }

            if (!CheckTableRecord("Others_Menu", "MenuCode", "M0212", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO [Others_Menu] ([MenuId],[MenuLevel],[MenuTitle],[MenuOrder],[ParentId],[MenuCode]) VALUES ('258','2','Customer','1','257','M0212')");
            }

            if (!CheckTableRecord("Others_Menu", "MenuCode", "M0213", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO [Others_Menu] ([MenuId],[MenuLevel],[MenuTitle],[MenuOrder],[ParentId],[MenuCode]) VALUES ('259','2','Product','2','257','M0213')");
            }

            if (!CheckTableRecord("Others_Menu", "MenuCode", "M0214", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO [Others_Menu] ([MenuId],[MenuLevel],[MenuTitle],[MenuOrder],[ParentId],[MenuCode]) VALUES ('260','2','Sales Order','3','257','M0214')");
            }

            if (!CheckTableRecord("Others_Menu", "MenuCode", "M0215", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO [Others_Menu] ([MenuId],[MenuLevel],[MenuTitle],[MenuOrder],[ParentId],[MenuCode]) VALUES ('261','3','Invoice (False)','2','29','M0215')");
            }

            if (!CheckTableRecord("Others_Menu", "MenuCode", "M0216", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO [Others_Menu] ([MenuId],[MenuLevel],[MenuTitle],[MenuOrder],[ParentId],[MenuCode]) VALUES ('262','3','Order After Approval','2','41','M0216')");
            }

            if (!CheckTableRecord("Others_Menu", "MenuId", "263", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO [Others_Menu] ([MenuId],[MenuLevel],[MenuTitle],[MenuOrder],[ParentId]) VALUES ('263','2','Geo Location','13','1')");
            }

            if (!CheckTableRecord("Others_Menu", "MenuCode", "M0217", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO [Others_Menu] ([MenuId],[MenuLevel],[MenuTitle],[MenuOrder],[ParentId],[MenuCode]) VALUES ('264','3','Region','1','263','M0217')");
            }

            if (!CheckTableRecord("Others_Menu", "MenuCode", "M0218", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO [Others_Menu] ([MenuId],[MenuLevel],[MenuTitle],[MenuOrder],[ParentId],[MenuCode]) VALUES ('265','3','Division','2','263','M0218')");
            }

            if (!CheckTableRecord("Others_Menu", "MenuCode", "M0219", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO [Others_Menu] ([MenuId],[MenuLevel],[MenuTitle],[MenuOrder],[ParentId],[MenuCode]) VALUES ('266','3','District','3','263','M0219')");
            }

            if (!CheckTableRecord("Others_Menu", "MenuCode", "M0220", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO [Others_Menu] ([MenuId],[MenuLevel],[MenuTitle],[MenuOrder],[ParentId],[MenuCode]) VALUES ('267','3','Police Station','4','263','M0220')");
            }
            #endregion

            #region Menu Update
            if (CheckTableRecord("Others_Menu", "MenuId", "253", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("UPDATE Others_Menu SET [MenuTitle] = 'List Report' WHERE MenuId = '253'");
            }

            if (CheckTableRecord("Others_Menu", "MenuCode", "M0175", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("UPDATE Others_Menu SET MenuTitle = 'Receive & Payment Statement' WHERE MenuCode = 'M0175'");
            }

            if (CheckTableRecord("Others_Menu", "MenuCode", "M0188", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("UPDATE Others_Menu SET MenuTitle = 'Govt. Duty Rate (HS Code)' WHERE MenuCode = 'M0188'");
            }

            if (CheckTableRecord("Others_Menu", "MenuId", "28", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("UPDATE Others_Menu SET MenuTitle = 'Purchase Edit' WHERE MenuId = '28'");
            }

            if (CheckTableRecord("Others_Menu", "MenuId", "29", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("UPDATE Others_Menu SET MenuTitle = 'Sales Edit' WHERE MenuId = '29'");
            }

            if (CheckTableRecord("Others_Menu", "MenuId", "231", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("UPDATE Others_Menu SET MenuTitle = 'Accounts & Finance Edit' WHERE MenuId = '231'");
            }

            if (CheckTableRecord("Others_Menu", "MenuCode", "M0081", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("UPDATE Others_Menu SET MenuTitle = 'Edit After Approval' WHERE MenuCode = 'M0081'");
            }

            if (CheckTableRecord("Others_Menu", "MenuCode", "M0186", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("UPDATE Others_Menu SET MenuTitle = 'Edit Before Approval' WHERE MenuCode = 'M0186'");
            }

            if (CheckTableRecord("Others_Menu", "MenuId", "7", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("UPDATE Others_Menu SET MenuOrder = '8' WHERE MenuId = '7'");
            }

            if (CheckTableRecord("Others_Menu", "MenuId", "160", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("UPDATE Others_Menu SET MenuOrder = '3' WHERE MenuId = '160'");
            }

            if (CheckTableRecord("Others_Menu", "MenuId", "161", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("UPDATE Others_Menu SET MenuOrder = '4' WHERE MenuId = '161'");
            }

            if (CheckTableRecord("Others_Menu", "MenuId", "162", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("UPDATE Others_Menu SET MenuOrder = '5' WHERE MenuId = '162'");
            }

            if (CheckTableRecord("Others_Menu", "MenuCode", "M0082", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("UPDATE Others_Menu SET MenuTitle = 'Invoice' WHERE MenuCode = 'M0082'");
            }

            if (CheckTableRecord("Others_Menu", "MenuId", "19", string.Empty, string.Empty))
            {
                // Setup => Others
                _db.Database.ExecuteSqlCommand("UPDATE Others_Menu SET MenuOrder = '14' WHERE MenuId = '19'");
            }
            #endregion

            if (CheckTableRecord("Others_Report", "Id", "48", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("DELETE FROM Others_Report WHERE Id = '48'");
            }

            #region Feature
            if (!CheckTableRecord("Setup_Features", "FeatureName", "CheckCreditLimit", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_Features(FeatureId, FeatureName, DefaultValue, Description) VALUES ('1', 'CheckCreditLimit', '1', 'True = Will check buyer credit limit at time of sales. False = Will not check credit limit at time of sales.')");
            }

            if (!CheckTableRecord("Setup_Features", "FeatureName", "IsCompanyNameShow", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_Features(FeatureId, FeatureName, DefaultValue, Description) VALUES ('2', 'IsCompanyNameShow', '1', 'True = Will show company name in Bill/Invoice. False = Will not show company name in Bill/Invoice.')");
            }

            if (!CheckTableRecord("Setup_Features", "FeatureName", "IsGovtDutyApplicable", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_Features(FeatureId, FeatureName, DefaultValue, Description) VALUES ('3', 'IsGovtDutyApplicable', '0', 'True = Govt. Duty applicable on sales or others, False = Govt. Duty will not applicable.')");
            }

            if (!CheckTableRecord("Setup_Features", "FeatureName", "IsGovtDutyAdjApplicable", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_Features(FeatureId, FeatureName, DefaultValue, Description) VALUES ('4', 'IsGovtDutyAdjApplicable', '0', 'True = Govt. Duty Adjustment applicable on Collection and Payment form, False = Govt. Duty Adjustment will not applicable.')");
            }

            if (!CheckTableRecord("Setup_Features", "FeatureName", "IsShowProductAvgCost", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_Features(FeatureId, FeatureName, DefaultValue, Description) VALUES ('5', 'IsShowProductAvgCost', '0', 'True = Show product average cost at time of sales, False = Product average cost will not show at time of sales.')");
            }

            if (!CheckTableRecord("Setup_Features", "FeatureName", "IsPartialAllowInvoiceFromOrder", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_Features(FeatureId, FeatureName, DefaultValue, Description) VALUES ('6', 'IsPartialAllowInvoiceFromOrder', '1', 'True = Can done partially invoice from order, False = should done fully invoice from order.')");
            }

            if (!CheckTableRecord("Setup_Features", "FeatureName", "IsCheckPasswordAtApproveCancel", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_Features(FeatureId, FeatureName, DefaultValue, Description) VALUES ('7', 'IsCheckPasswordAtApproveCancel', '0', 'True = Check logged user password at time of approve or cancel, False = Do not check logged user password.')");
            }

            if (!CheckTableRecord("Setup_Features", "FeatureName", "IsEntryByCanApproveCancel", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_Features(FeatureId, FeatureName, DefaultValue, Description) VALUES ('8', 'IsEntryByCanApproveCancel', '1', 'True = Event entry by can approve or cancel, False = Entry by can not approve or cancel any event.')");
            }

            if (!CheckTableRecord("Setup_Features", "FeatureName", "IsOperationDateEnable", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_Features(FeatureId, FeatureName, DefaultValue, Description) VALUES ('9', 'IsOperationDateEnable', '1', 'True = Operation date can be changed, False = Operation date will be current date and date selection field will be disabled.')");
            }

            if (!CheckTableRecord("Setup_Features", "FeatureName", "IsAutoLIMCreate", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_Features(FeatureId, FeatureName, DefaultValue, Description) VALUES ('10', 'IsAutoLIMCreate', '1', 'True = At time of Import Costing Approve auto LIM Stock In will create, False = Auto LIM Stock In will not create.')");
            }

            if (!CheckTableRecord("Setup_Features", "FeatureName", "AllowZeroCostProductPurchase", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_Features(FeatureId, FeatureName, DefaultValue, Description) VALUES ('11', 'AllowZeroCostProductPurchase', '0', 'True = Allow 0 cost product at time of purchase, False = Do not allow 0 cost product at time of purchase.')");
            }

            if (!CheckTableRecord("Setup_Features", "FeatureName", "AllowZeroPriceProductSale", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_Features(FeatureId, FeatureName, DefaultValue, Description) VALUES ('12', 'AllowZeroPriceProductSale', '0', 'True = Allow 0 price product at time of sales, False = Do not allow 0 price product at time of sales.')");
            }

            if (!CheckTableRecord("Setup_Features", "FeatureName", "LocationWiseAccountsBalance", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_Features(FeatureId, FeatureName, DefaultValue, Description) VALUES ('13', 'LocationWiseAccountsBalance', '0', 'True = Accounts balance will be considered by selected or logged location, False = Accounts balance will be considered by all location.')");
            }

            if (!CheckTableRecord("Setup_Features", "FeatureName", "IsPeriodicInventory", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_Features(FeatureId, FeatureName, DefaultValue, Description) VALUES ('14', 'IsPeriodicInventory', '1', 'True = In income statement report calculate period wise trading operation for Cost of Goods Sold, False = Calculate Cost of Goods Sold by posted voucher.')");
            }

            if (!CheckTableRecord("Setup_Features", "FeatureName", "IsInvoiceWisePreviousDue", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_Features(FeatureId, FeatureName, DefaultValue, Description) VALUES ('15', 'IsInvoiceWisePreviousDue', '0', 'True = In invoice report will show customer pervious due just before selected invoice, False = do not show any previous due.')");
            }

            if (!CheckTableRecord("Setup_Features", "FeatureName", "PriceChangeAtSales", string.Empty, string.Empty))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_Features(FeatureId, FeatureName, DefaultValue, Description) VALUES ('16', 'PriceChangeAtSales', '1', 'True = Price can change at sales with or without configured limit, False = Price can not change without configured limit.')");
            }
            #endregion

            #region SubFeature
            if (!CheckTableRecord("Setup_SubFeatures", "SubFeatureName", "ProductWiseDuty", "FeatureId", "3"))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_SubFeatures(SubFeatureId, FeatureId, SubFeatureName, DefaultValue, [Group], Description) VALUES ('1', '3', 'ProductWiseDuty', '1', 'option_1', 'True = Govt duty will be calculated on HS code. False = No duty will be calculated on HS code.')");
            }

            if (!CheckTableRecord("Setup_SubFeatures", "SubFeatureName", "LocationWiseDuty", "FeatureId", "3"))
            {
                _db.Database.ExecuteSqlCommand("INSERT INTO Setup_SubFeatures(SubFeatureId, FeatureId, SubFeatureName, DefaultValue, [Group], Description) VALUES ('2', '3', 'LocationWiseDuty', '0', 'option_1', 'True = Govt duty will be calculated on Net total bill to login location. False = No duty will be calculated.')");
            }
            #endregion
        }

    }
}