myApp
    .controller('complainReceiveController', ['$scope', '$ngConfirm', '$filter', 'dataTableRows', 'applicationBasePath', 'userService',
         'currencyCommonService', 'employeeCommonService', 'productCommonService', 'stockTypeCommonService',
        'complainReceiveService', 'customerCommonService', 'problemSetupService', 'invoiceCommonService', 'chargeSetupService',
        function (
            $scope, $ngConfirm, $filter, dataTableRows, applicationBasePath, userService,
            currencyCommonService, employeeCommonService, productCommonService, stockTypeCommonService,
            complainReceiveService, customerCommonService, problemSetupService, invoiceCommonService, chargeSetupService
        ) {
            $scope.OperationalEvent = "RMA";
            $scope.OperationalSubEvent = "ComplainReceive";
            $scope.operationTypeId = 1;//Regular
            $scope.selectedLocationId = 0;
            $scope.wareHouseId = 0;
            $scope.dimensionId = 0;
            $scope.productSerialOrNot = false;

            $scope.selectedCurrency = $scope.DefaultCurrency;
            $scope.isReceiveAgainstPreviousSales = false;
            $scope.isAdvanchSearch = false;
            $scope.totalServiceChargeAmount = 0;           
            $scope.addedProductLists = [];
            $scope.addedSpareProductLists = [];
            $scope.totalSpareProductAmount = 0;
            $scope.totalSpareProductDiscount = 0;
            $scope.totalChargeAmount = 0;
            $scope.pageIndex = 1;            
            $scope.selectedEmployeeId = 0;
            $scope.selectedProductId = 0;
            $scope.isRequiredCharges = true;
            $scope.isForServiceOnly = false;
            $scope.IsWarrantyAvailable = false;
            $scope.IsServiceWarranty = false;
            $scope.isAdded = false;
            $scope.isSaved = false;
            $scope.isDisableCustomer = false;    
            $scope.complainReceiveDate = $filter('date')(new Date(), "M/d/yyyy");
            $scope.productTotal = 0;
            $scope.grandTotal = 0;
            $scope.discount = 0;
            $scope.netTotal = 0;
            $scope.totalServiceProductAmount = 0;

            $scope.loadExchangeRate = function () {
                $scope.selectedCurrency = $scope.currencyId;
                GetCurrencyRate();
            };

            //generate Product Serial
            $scope.generateProductSerial = function () {
                var productId = $scope.selectedProductId;
                var serialLength = 8;//changable by requirements
                if (productId != 0 && ($scope.productSerialOrNot == false || isReceiveAgainstPreviousSales == true)) {
                    productCommonService.GenerateProductSerial(productId, serialLength).then(function (value) {
                        $scope.productSerial = value.data;
                        $scope.searchProductSerial = value.data;
                    }, function (reason) {
                        console.log(reason);
                    });
                } else {
                    $scope.selectedNewProductId = 0;
                    $scope.newProductNameJsonData = null;
                }
            };
            //clculateProductTotal
            $scope.clculateProductTotal = function () {
                $scope.productTotal = 0;
                $.each($scope.addedProductLists, function () {
                    $scope.productTotal += parseFloat(this.TotalAmount);
                });
                $scope.grandTotal = $scope.productTotal + $scope.totalServiceChargeAmount;
                //$scope.netTotal = $scope.grandTotal - $scope.discount;
                $scope.netTotal = $scope.grandTotal;

            };
            //calculate Net Total
            $scope.calculateNetTotal = function () {
                $scope.netTotal = $scope.grandTotal - $scope.discount;
            };

            //is Previous Sales or not
            $scope.clickReceiveAgainstPreviousSales = function () {
                $scope.isReceiveAgainstPreviousSales = !$scope.isReceiveAgainstPreviousSales;
                if ($scope.isReceiveAgainstPreviousSales) {
                    $scope.isAdvanchSearch = false;
                }
                $scope.selectedInvoiceId = 0;
                $scope.invoiceNo = "";
                $scope.dateFrom = "";
                $scope.dateTo = "";
            };
            $scope.clickAdvanchSearch = function () {
                $scope.isAdvanchSearch = !$scope.isAdvanchSearch;
            };
            //is Required Charges
            $scope.clickForRequiredCharges = function () {
                if ($scope.isRequiredCharges)
                    $scope.isRequiredCharges = false;
                else $scope.isRequiredCharges = true;
            }
            //is service only
            $scope.clickForServiceOnly = function () {
                if ($scope.isForServiceOnly)
                    $scope.isForServiceOnly = false;
                else $scope.isForServiceOnly = true;
            }
            //calculate total Service Charge Amount
            $scope.calculatetotalServiceChargeAmount = function () {
                $scope.totalServiceChargeAmount = 0;
                $scope.allSelectedCharges = [];
                $.each($scope.allChargeJsonLists, function () {
                    if (this.isSelected) {
                        $scope.totalServiceChargeAmount += parseFloat(this.ChargeAmount);
                        $scope.allSelectedCharges.push(this);
                    }
                });
                $scope.grandTotal = 0;
                $scope.grandTotal = $scope.productTotal + $scope.totalServiceChargeAmount;
                $scope.netTotal = 0;
                //$scope.netTotal = $scope.grandTotal - $scope.discount;
                $scope.netTotal = $scope.grandTotal;
            }
            //get employee
            $scope.getEmployeeName = function () {
                var q = $scope.requestedBy === undefined ? '' : $scope.requestedBy;

                if (q.length >= 3) {
                    employeeCommonService.FetchAllEmployee(
                        q
                    ).then(function (value) {
                        $scope.employeeJsonData = value.data;
                    }, function (reason) {
                        console.log(reason);
                    });
                } else {
                    $scope.selectedEmployeeId = 0;
                    $scope.employeeJsonData = null;
                }
            };
            $scope.fillEmployeeTextbox = function (obj) {
                $scope.selectedEmployeeId = Number(obj.Value);
                $scope.requestedBy = obj.Item;
                $scope.employeeJsonData = null;
            };
            //get invoice
            $scope.getInvoiceNo = function () {
                var q = ($scope.invoiceNo === undefined || $scope.invoiceNo == "*") ? '' : $scope.invoiceNo;

                if (q.length >= 3 || $scope.invoiceNo == "*") {
                    invoiceCommonService.FetchAllInvoice(q, (selectedCustomerId = $scope.selectedCustomerId === undefined ? 0 : $scope.selectedCustomerId), $scope.dateFrom, $scope.dateTo).then(function (value) {
                        $scope.invoiceNoJsonData = value.data;
                    }, function (reason) {
                        console.log(reason);
                    });
                } else {
                    $scope.selectedInvoiceId = undefined;
                    $scope.invoiceNoJsonData = null;
                }
            };
            $scope.fillInvoiceNoTextbox = function (obj) {
                $scope.selectedInvoiceId = obj.Value;
                $scope.invoiceNo = obj.Item;
                $scope.invoiceNoJsonData = null;
                invoiceCommonService.FetchAllInvoiceShortInfo($scope.selectedInvoiceId).then(function (value) {
                    $scope.selectedCustomerId = value.data.CustomerId;
                    $scope.customerName = value.data.CustomerName;
                    $scope.customerMobileNo = value.data.CustomerPhoneNo;
                    $scope.customerAddress = value.data.CustomerAddress;
                }, function (reason) {
                    console.log(reason);
                });
            };
            //get customer
            $scope.getCustomerName = function () {
                var q = $scope.customerName === undefined ? '' : $scope.customerName;
                getCustomer(q);
            }
            $scope.getCustomerNameByPhone = function () {
                var q = $scope.customerMobileNo === undefined ? '' : $scope.customerMobileNo;
                getCustomer(q);
            }
            $scope.fillCustomerTextbox = function (obj) {
                $scope.customerOrSupplierName = '';
                $scope.selectedCustomerId = Number(obj.Value);
                $scope.customerName = obj.Item;
                $scope.customerNameJsonData = null;
                $scope.customerNameByPhoneJsonData = null;
                customerCommonService.FetchCustomerShortInfo($scope.selectedCustomerId).then(function (value) {
                    $scope.customerMobileNo = value.data.ContactNo;
                    $scope.customerAddress = value.data.Address;
                }, function (reason) {
                    console.log(reason);
                });
            };
            //product search
            $scope.getProductName = function () {
                var q = ($scope.productName === undefined || $scope.productName == "*") ? '' : $scope.productName;

                if (q.length >= 2 || ($scope.productName == "*")) {

                    if ($scope.isReceiveAgainstPreviousSales) {
                        productCommonService.FetchProductName(q).then(function (value) {
                            $scope.productNameJsonData = value.data;
                        }, function (reason) {
                            console.log(reason);
                        });
                    }
                    else {
                        invoiceCommonService.FetchProductNameByInvoice(q, ($scope.selectedInvoiceId === undefined || $scope.selectedInvoiceId == 0 ? '' : $scope.selectedInvoiceId)).then(function (value) {
                            $scope.productNameJsonData = value.data;
                        }, function (reason) {
                            console.log(reason);
                        });
                    }
                } else {
                    $scope.selectedProductId = 0;
                    $scope.productNameJsonData = null;
                }
            };
            $scope.fillProductTextbox = function (obj) {
                $scope.selectedProductId = Number(obj.Value);
                $scope.productName = obj.Item;
                //GetSalesInvoiceInfoByProduct();
                GetWarrantyInfo();
                GetProductWiseDimension();
                GetProductWiseUnitType();
                ProductSerialOrNot();
                if ($scope.isReceiveAgainstPreviousSales) {
                    GetProductCost();
                }
                else {
                    $scope.unitCost = obj.Cost;
                }
                $scope.productNameJsonData = null;
            };
            //product serial search
            $scope.getProductSerial = function () {
                var q = $scope.searchProductSerial === undefined ? '' : $scope.searchProductSerial;
                if (q.length >= 2) {
                    invoiceCommonService.FetchProductBySerialFromInvoice(q, $scope.isReceiveAgainstPreviousSales, $scope.selectedProductId, ($scope.selectedInvoiceId === undefined || $scope.selectedInvoiceId == 0 ? '' : $scope.selectedInvoiceId)).then(function (value) {
                        $scope.productSerialJsonData = value.data;
                    }, function (reason) {
                        console.log(reason);
                    });
                }
                else {
                    $scope.productSerial = "";
                    $scope.productSerialJsonData = null;
                }
            };
            $scope.fillproductSerialTextbox = function (obj) {

                $scope.productSerial = obj.Item;
                $scope.searchProductSerial = obj.Item;
                $scope.selectedProductId = obj.ProductId;
                $scope.productName = obj.ProductName;
                $scope.unitCost = obj.Cost;
                $scope.productSerialJsonData = null;
                GetWarrantyInfo();
                GetProductWiseDimension();
                GetProductWiseUnitType();
                ProductSerialOrNot();
                if (!$scope.isReceiveAgainstPreviousSales) {
                    GetSalesInvoiceInfoByProduct();
                }
            };
            //get dimension
            $scope.getDimensionName = function () {
                $.each($scope.productDimensionDropDownJsonData, function () {
                    if (this.Value === $scope.dimensionId) {
                        $scope.dimensionName = this.Item;
                    }
                });
            };
            //get unit type
            $scope.getUnitTypeName = function () {
                $.each($scope.unitTypeDropDownJsonData, function () {
                    if (this.Value === $scope.unitTypeId) {
                        $scope.unitTypeName = this.Item;
                    }
                });
            };
            //spare product ---------------------
            $scope.clickAddSpareParts = function (entity) {
                $scope.productIdForSparePart = entity.ProductId;
                $scope.productNameForSparePart = entity.Name;
                $scope.productSerialForSparePart = entity.Serial;
                ProductWiseSpareProductListprepare();
                GetSpareType();

            }
            var ProductWiseSpareProductListprepare = function () {
                $scope.totalSpareProductAmount = 0;
                $scope.totalServiceProductAmount = 0;
                //$scope.totalSpareProductDiscount = 0;
                $scope.productWiseSpareProductList = [];
                $scope.productWiseServiceProductList = [];
                $.each($scope.addedSpareProductLists, function (index) {
                    if (this.parentProductId == $scope.productIdForSparePart && this.parentSerial == $scope.productSerialForSparePart) {
                        if (this.isServiceProduct) {
                            $scope.productWiseServiceProductList.push(this);
                            $scope.totalServiceProductAmount += parseFloat(this.TotalAmount);
                        }
                        else {
                            $scope.productWiseSpareProductList.push(this);
                            $scope.totalSpareProductAmount += parseFloat(this.TotalAmount);
                        }
                    }
                });
            }
            //get spare product
            $scope.getSpareProductName = function () {
                var q = ($scope.spareProductName === undefined || $scope.spareProductName == "*") ? '' : $scope.spareProductName;

                if (q.length >= 2 || $scope.spareProductName == "*") {
                    productCommonService.FetchProductName(q).then(function (value) {
                        $scope.spareProductNameJsonData = value.data;
                    }, function (reason) {
                        console.log(reason);
                    });

                } else {
                    $scope.selectedSpareProductId = 0;
                    $scope.spareProductNameJsonData = null;
                }
            };

            $scope.fillSpareProductTextbox = function (obj) {
                $scope.selectedSpareProductId = Number(obj.Value);
                $scope.spareProductName = obj.Item;
                $scope.spareUnitCost = obj.Cost;
                $scope.isServiceProduct = obj.isServiceProduct;
                $scope.spareProductNameJsonData = null;
                GetSpareProductWiseDimension();
                GetSpareProductWiseUnitType();                
            };
            //get spare product type
            var GetSpareProductWiseUnitType = function () {
                productCommonService.FetchProductWiseUnitType(
                    '', $scope.selectedProductId
                ).then(function (value) {
                    $scope.spareUnitTypeDropDownJsonData = value.data;
                    $.each(value.data, function () {
                        if (this.IsSelected) {
                            $scope.spareUnitTypeId = this.Value;
                            $scope.getSpareUnitTypeName();
                            GetProductWiseAvailableStockPriceDiscount();
                        }
                    });
                }, function (reason) {
                    console.log(reason);
                });
            };
            $scope.getSpareUnitTypeName = function () {
                $.each($scope.unitTypeDropDownJsonData, function () {
                    if (this.Value === $scope.unitTypeId) {
                        $scope.spareUnitTypeName = this.Item;
                    }
                });
            };
            //spare product dimension
            var GetSpareProductWiseDimension = function () {
                productCommonService.FetchProductWiseDimension(
                    '', $scope.selectedSpareProductId
                ).then(function (value) {
                    $scope.spareProductDimensionDropDownJsonData = value.data;
                    if (value.data.length > 0) {
                        $scope.spareProductdimensionId = value.data[0].Value;
                        //$scope.getDimensionName();
                    }
                }, function (reason) {
                    console.log(reason);
                });
            }
            // load Spare Type
            var GetSpareType = function () {
                complainReceiveService.GetSpareType().then(function (value) {
                    $scope.spareTypeDropDownJsonData = value.data;
                    $.each($scope.spareTypeDropDownJsonData, function () {
                        if (this.IsSelected) {
                            $scope.spareTypeId = this.Value;
                            return;
                        }
                    });
                }, function (reason) {
                    console.log(reason);
                });
            }
            //get Spare Type Name
            $scope.getSpareTypeName = function () {
                $.each($scope.spareTypeDropDownJsonData, function () {
                    if (this.Value === $scope.spareTypeId) {
                        $scope.spareTypeName = this.Item;
                    }
                });
            };
            //get stock,price and discount
            var GetProductWiseAvailableStockPriceDiscount = function () {
                $scope.availableStock = 0;
                $scope.spareUnitCost = 0;
                $scope.spareDiscount = 0;               
                productCommonService.FetchProductWiseAvailableStockPriceDiscountByOperationalEvent($scope.OperationalEvent, $scope.OperationalSubEvent, $scope.currencyId, $scope.operationTypeId, $scope.selectedSpareProductId, $scope.spareProductdimensionId,
                    $scope.spareUnitTypeId, $scope.selectedLocationId, $scope.wareHouseId
                ).then(function (value) {
                    $scope.availableStock = value.data.AvailableStock;
                    $scope.spareUnitCost = value.data.Price;
                    $scope.spareDiscount = value.data.Discount;
                }, function (reason) {
                    console.log(reason);
                });
            };
            var SpareProductIdentity = function (parentProductId, parentSerial, productId, productDimensionId, unitTypeId) {
                return parentProductId.toString() + parentSerial.toString() + productId.toString() + (productDimensionId === null ? '0' : productDimensionId.toString()) + unitTypeId.toString();
            };
            //add spare product
            $scope.clickToAddSpareProduct = function () {
                if ($scope.spareUnitCost == 0 || $scope.spareQuantity == 0) {
                    // alert
                    $ngConfirm({
                        title: 'Required',
                        icon: 'glyphicon glyphicon-exclamation-sign',
                        theme: 'supervan',
                        content: 'Cost/Quantity can not be zero.',
                        animation: 'scale',
                        buttons: {
                            Ok: {
                                btnClass: "btn-blue"
                            }
                        },
                    });
                    return;
                }

                var identity = SpareProductIdentity($scope.productIdForSparePart, $scope.productSerialForSparePart, $scope.selectedSpareProductId, ($scope.spareProductDimensionDropDownJsonData.length > 0 ? $scope.spareProductdimensionId.toString() : '0'), $scope.spareUnitTypeId.toString());
                //check If already exist
                var previousIentity = 0;
                $.each($scope.addedSpareProductLists, function (index) {
                    if (this.identity === identity) {
                        previousIentity = identity;
                        return false;
                    }
                });

                if (previousIentity == 0) {
                    $scope.addedSpareProductLists.push({
                        "identity": identity,
                        "parentProductId": $scope.productIdForSparePart,
                        "parentSerial": $scope.productSerialForSparePart,
                        "noOfRows": 0,
                        "ProductId": $scope.selectedSpareProductId.toString(),                        
                        "ProductDimensionId": ($scope.spareProductDimensionDropDownJsonData.length > 0 ? $scope.spareProductdimensionId.toString() : '0'),
                        "Name": $scope.spareProductName,
                        'Remarks': $scope.spareProductremarks,
                        'isServiceProduct': $scope.isServiceProduct,
                        "UnitTypeId": $scope.spareUnitTypeId.toString(),
                        "UnitTypeName": $scope.spareUnitTypeName,
                        "Quantity": $scope.spareQuantity,
                        "Price": $scope.spareUnitCost,
                        "Discount": $scope.spareDiscount,
                        "TotalAmount": (parseFloat($scope.spareUnitCost) - parseFloat($scope.spareDiscount)) * parseFloat($scope.spareQuantity)
                    });
                }
                else {
                    $.each($scope.addedSpareProductLists, function (index) {
                        if (this.identity === previousIentity) {
                            this.noOfRows = 0;
                            this.ProductId = $scope.selectedSpareProductId.toString();
                            this.ProductDimensionId = ($scope.spareProductDimensionDropDownJsonData.length > 0 ? $scope.spareProductdimensionId.toString() : '0');
                            this.Name = $scope.spareProductName;
                            this.Remarks = $scope.spareProductremarks;
                            this.isServiceProduct = $scope.isServiceProduct,
                            this.UnitTypeId = $scope.spareUnitTypeId.toString();
                            this.UnitTypeName = $scope.spareUnitTypeName;
                            this.Quantity = $scope.spareQuantity;
                            this.Price = $scope.spareUnitCost;
                            this.Discount = $scope.spareDiscount;
                            this.TotalAmount = (parseFloat($scope.spareUnitCost) - parseFloat($scope.spareDiscount)) * parseFloat($scope.spareQuantity);
                            return false;
                        }
                    });
                }
                //$scope.calculatetotalSpareProductAmount();
                ProductWiseSpareProductListprepare();

                $scope.updateProductListBySpareProduct();                
                if (previousIentity == 0) {
                    $scope.clearSpareProduct();
                }
                $scope.clculateProductTotal();
            };
            $scope.clearSpareProduct = function () {
                $scope.selectedSpareProductId = 0;
                $scope.spareProductName = "";
                $scope.spareUnitCost = 0;
                $scope.spareProductNameJsonData = null;
                $scope.spareProductdimensionId = 0;
                $scope.spareUnitTypeId = 0;
                $scope.spareUnitTypeName = "";
                $scope.availableStock = 0;
                $scope.spareUnitCost = 0;
                $scope.spareDiscount = 0;              
            }
            $scope.clickRemoveSpareItem = function (identity) {
                $.each($scope.addedSpareProductLists, function (index) {
                    if (this.identity === identity) {
                        $scope.addedSpareProductLists.splice(index, 1);
                        return false;
                    }
                });
                //$scope.calculatetotalSpareProductAmount();
                ProductWiseSpareProductListprepare();
                $scope.updateProductListBySpareProduct();             
                $scope.clculateProductTotal();
            };
            $scope.updateProductListBySpareProduct = function () {
                $.each($scope.addedProductLists, function (index) {
                    if (this.ProductId == $scope.productIdForSparePart && this.Serial == $scope.productSerialForSparePart) {
                        this.SparePartsAmount = $scope.totalSpareProductAmount;
                        this.ServiceChargeAmount = $scope.totalServiceProductAmount;
                        this.TotalAmount = parseFloat(this.SparePartsAmount) + this.ServiceChargeAmount;
                        this.complainReceiveDetail_SpareProduct = $scope.productWiseSpareProductList.concat($scope.productWiseServiceProductList);
                    }
                });
            };
            $scope.calculatetotalSpareProductAmount = function () {
                $scope.totalSpareProductAmount = 0;
                $scope.totalSpareProductDiscount = 0;
                $.each($scope.addedSpareProductLists, function () {
                    $scope.totalSpareProductAmount += parseFloat(this.TotalAmount);
                    $scope.totalSpareProductDiscount += parseFloat(this.Discount);
                });
            }
            //spare product end -------------------
            //add parent product
            $scope.clickToAdd = function () {
                if (($scope.customerName == "" || $scope.customerName == undefined) || $scope.searchProductSerial == "" || $scope.searchProductSerial == undefined) {
                    // alert
                    $ngConfirm({
                        title: 'Required',
                        icon: 'glyphicon glyphicon-exclamation-sign',
                        theme: 'supervan',
                        content: 'Customer Name or Product serial is empty!!!.',
                        animation: 'scale',
                        buttons: {
                            Ok: {
                                btnClass: "btn-blue"
                            }
                        },
                    });
                    return;
                }
                var identity = ProductIdentity($scope.selectedProductId, $scope.searchProductSerial, ($scope.productDimensionDropDownJsonData.length > 0 ? $scope.dimensionId.toString() : '0'), $scope.unitTypeId.toString());
                //check If already exist
                var previousIentity = 0;
                $.each($scope.addedProductLists, function (index) {
                    if (this.identity === identity) {
                        previousIentity = identity;
                        return false;
                    }
                });
                //check warranty Available or left
                if ($scope.warrantyLeft == 0)
                    $scope.IsWarrantyAvailable = true;
                if ($scope.serviceWarrantyDaysAvailable > 0)
                    $scope.IsServiceWarranty = true;

                if (previousIentity == 0) {
                    $scope.addedProductLists.push({
                        "identity": ProductIdentity($scope.selectedProductId, $scope.searchProductSerial, ($scope.productDimensionDropDownJsonData.length > 0 ? $scope.dimensionId.toString() : '0'), $scope.unitTypeId.toString()),
                        "InvoiceId": $scope.selectedInvoiceId,
                        "ProductId": $scope.selectedProductId.toString(),
                        "ProductDimensionId": $scope.productDimensionDropDownJsonData.length > 0 ? $scope.dimensionId.toString() : '0',
                        "Name": $scope.productName,
                        "Dimension": $scope.dimensionName,
                        "UnitTypeId": $scope.unitTypeId.toString(),
                        "UnitTypeName": $scope.unitTypeName,
                        "Serial": $scope.searchProductSerial,
                        "Cost": $scope.unitCost,
                        "IsWarrantyAvailable": $scope.IsWarrantyAvailable,
                        "IsServiceWarranty": $scope.IsServiceWarranty,
                        "IsOnlyService": $scope.isForServiceOnly,
                        "Problem": getAllSelectedProblem(),
                        "complainReceiveDetail_Problem": $scope.allSelectedProblems,
                        "SparePartsAmount": 0,
                        "ServiceChargeAmount": $scope.totalServiceProductAmount,
                        "TotalAmount": 0 + $scope.totalServiceProductAmount,
                        "complainReceiveDetail_SpareProduct": []
                    });
                    $scope.isDisableCustomer = true;
                }
                else {
                    $.each($scope.addedProductLists, function (index) {
                        if (this.identity === previousIentity) {
                            this.ProductId = $scope.selectedProductId.toString();
                            this.ProductDimensionId = $scope.productDimensionDropDownJsonData.length > 0 ? $scope.dimensionId.toString() : '0';
                            this.Name = $scope.productName;
                            this.Dimension = $scope.dimensionName;
                            this.UnitTypeId = $scope.unitTypeId.toString();
                            this.UnitTypeName = $scope.unitTypeName;
                            this.Serial = $scope.searchProductSerial;
                            this.Cost = $scope.unitCost;
                            this.IsWarrantyAvailable = $scope.IsWarrantyAvailable;
                            this.IsServiceWarranty = $scope.IsServiceWarranty;
                            this.IsOnlyService = $scope.isForServiceOnly;
                            this.Problem = getAllSelectedProblem();
                            this.complainReceiveDetail_Problem = $scope.allSelectedProblems;
                            this.ServiceChargeAmount = $scope.totalServiceProductAmount;
                            this.TotalAmount = 0 + $scope.totalServiceProductAmount;
                            return false;
                        }
                    });
                }
                //clear product serial
                $scope.productSerial = "";
                $scope.searchProductSerial = "";
                $scope.productSerialJsonData = null;
                $scope.clculateProductTotal();
            };
            var getAllSelectedProblem = function () {
                var allSelectedProblemName = "";
                $scope.allSelectedProblems = [];
                $.each($scope.allProblemJsonLists, function () {
                    if (this.isSelected) {
                        if (allSelectedProblemName != "") {
                            allSelectedProblemName += ",";
                        }
                        allSelectedProblemName += this.Name;
                        $scope.allSelectedProblems.push(this);
                    }
                })
                return allSelectedProblemName;
            }
            $scope.clickRemoveItem = function (identity) {
                $.each($scope.addedProductLists, function (index) {
                    if (this.identity === identity) {
                        $scope.addedProductLists.splice(index, 1);
                        return false;
                    }
                });
                $scope.clculateProductTotal();
            };

            $scope.clickToReset = function () {
                $scope.productSerial = "";
                $scope.searchProductSerial = "";
                $scope.productSerialJsonData = null;
                $scope.selectedProductId = 0;
                $scope.productName = "";
                $scope.unitCost = 0;
                $scope.productNameJsonData = null;                
                $scope.dimensionId = 0;
                $scope.dimensionName = '';
                $scope.productDimensionDropDownJsonData = null;
                //warranty Available or left
                $scope.warrantyLeft = "";
                $scope.IsWarrantyAvailable = false;
                $scope.serviceWarrantyDaysAvailable = "";
                $scope.IsServiceWarranty = false;
                $scope.productSerialOrNot = false;
            };
            // save data
            $scope.clickToSave = function () {
                if ($scope.addedProductLists.length === 0) {
                    // alert
                    $ngConfirm({
                        title: 'Warning',
                        icon: 'glyphicon glyphicon-exclamation-sign',
                        theme: 'supervan',
                        content: 'Have not added any product.',
                        animation: 'scale',
                        buttons: {
                            Ok: {
                                btnClass: "btn-blue"
                            }
                        },
                    });
                    return;
                }

                if ($scope.complainReceiveNo != undefined) {
                    // alert
                    $ngConfirm({
                        title: 'Warning',
                        icon: 'glyphicon glyphicon-exclamation-sign',
                        theme: 'supervan',
                        content: 'This Complain Receive already saved.',
                        animation: 'scale',
                        buttons: {
                            Ok: {
                                btnClass: "btn-blue"
                            }
                        },
                    });
                    return;
                }
                $scope.totalChargeAmount = 0;
                $scope.totalChargeAmount = $scope.netTotal;
                if ($scope.allSelectedCharges == undefined) {
                    $scope.complainReceive_Charge = [];
                }
                else {
                    $scope.complainReceive_Charge = $scope.allSelectedCharges;
                }


                $scope.isSaved = true;
                complainReceiveService.SaveComplainReceive(                    
                    $scope.selectedCustomerId, $scope.complainReceiveDate, $scope.selectedEmployeeId, $scope.remarks, $scope.currencyId,
                    $scope.isReceiveAgainstPreviousSales, $scope.totalChargeAmount, $scope.complainReceive_Charge, $scope.addedProductLists, $scope.exchangeRate
                ).then(function (value) {
                    if (Number(value.status) === 200 && value.data.IsSuccess === true) {
                        $scope.complainReceiveNo = value.data.Message;
                        // alert
                        $ngConfirm({
                            title: 'Success',
                            icon: 'glyphicon glyphicon-info-sign',
                            theme: 'supervan',
                            content: 'Save successful.',
                            animation: 'scale',
                            buttons: {
                                Ok: {
                                    btnClass: "btn-blue"
                                }
                            },
                        });
                        GetAllComplainReceiveGridListsData();
                    } else {
                        // alert
                        $ngConfirm({
                            title: 'Failed',
                            icon: 'glyphicon glyphicon-info-sign',
                            theme: 'supervan',
                            content: value.data,
                            animation: 'scale',
                            buttons: {
                                Ok: {
                                    btnClass: "btn-blue"
                                }
                            },
                        });
                        $scope.isSaved = false;
                    }
                });
            };

            $scope.PrintReport = function (id) {
                var url = applicationBasePath + "/Inventory360Reports/PrintComplainReceive?id=" + id
                    + "&user=" + $scope.UserName
                    + "&token=" + userService.GetCurrentUser().access_token;
                var win = window.open(url, '_blank');
                win.focus();
            };

            $scope.previous = function (e) {
                e.preventDefault();
                if ($scope.pageIndex > 1) {
                    $scope.pageIndex--;
                    GetAllComplainReceiveGridListsData();
                }
            };

            $scope.next = function (e) {
                e.preventDefault();
                if ($scope.pageIndex < lastPageNo) {
                    $scope.pageIndex++;
                    GetAllComplainReceiveGridListsData();
                }
            };
            $scope.filterComplainReceive = function () {
                if ($scope.searchComplainReceive !== undefined) {
                    $scope.pageIndex = 1;
                    GetAllComplainReceiveGridListsData();
                }
            };
            // load all ComplainReceive lists
            var GetAllComplainReceiveGridListsData = function () {
                var q = $scope.searchComplainReceive === undefined ? '' : $scope.searchComplainReceive;

                complainReceiveService.FetchComplainReceiveLists(
                    q, $scope.pageIndex, dataTableRows
                ).then(function (value) {
                    $scope.allComplainReceiveJsonLists = value.data;
                    lastPageNo = value.data.LastPageNo;
                    if (value.data.TotalNumberOfRecords > 0) {
                        $scope.showGrid = true;
                        $scope.showRecordsInfo = 'Record(s) showing ' + value.data.Start + " to " + value.data.End + " of " + value.data.TotalNumberOfRecords;
                    } else {
                        $scope.showGrid = false;
                        $scope.showRecordsInfo = "No record(s) found...";
                    }
                }, function (reason) {
                    console.log(reason);
                });
            };

            var ProductIdentity = function (productId, serial, productDimensionId, unitTypeId) {
                return productId.toString() + serial.toString() + (productDimensionId === null ? '0' : productDimensionId.toString()) + unitTypeId.toString();
            };
            //get customer
            var getCustomer = function (customer) {
                var q = customer;

                if (q.length >= 3) {
                    customerCommonService.FetchAllCustomer(
                        q
                    ).then(function (value) {                       
                        if ($scope.customerName.length >= 3) {
                            $scope.customerNameJsonData = value.data;
                        }
                        else if ($scope.customerMobileNo.length >= 3) {
                            $scope.customerNameByPhoneJsonData = angular.copy(value.data);
                        }
                    }, function (reason) {
                        console.log(reason);
                    });
                } else {
                    $scope.selectedCustomerId = 0;
                    $scope.customerAddress = "";
                    $scope.customerNameJsonData = null;
                }
            };
            //get invoice info
            var GetSalesInvoiceInfoByProduct = function () {
                invoiceCommonService.FetchSalesInvoiceInfoByProduct($scope.selectedProductId, $scope.productSerial).then(function (value) {
                    $scope.selectedCustomerId = value.data.CustomerId;
                    $scope.customerName = value.data.CustomerName;
                    $scope.customerMobileNo = value.data.CustomerPhoneNo;
                    $scope.customerAddress = value.data.CustomerAddress;
                    $scope.selectedInvoiceId = value.data.InvoiceId;
                    $scope.invoiceNo = value.data.InvoiceNo;
                }, function (reason) {
                    console.log(reason);
                });
            }
            //get all problem
            var GetAllProblem = function () {
                problemSetupService.FetchProblemForComplainReceive().then(function (value) {
                    $scope.allProblemJsonLists = value.data;
                }, function (reason) {
                    console.log(reason);
                });
            }
            //get all charge
            var GetAllCharge = function () {
                chargeSetupService.FetchRMAWiseAllCharge().then(function (value) {
                    $scope.allChargeJsonLists = value.data;
                }, function (reason) {
                    console.log(reason);
                });
            }
            //get warranty info
            var GetWarrantyInfo = function () {
                $scope.warrantyDays = 0;
                $scope.warrantyLeft = 0;
                $scope.serviceWarrantyDays = 0;
                $scope.serviceWarrantyDaysAvailable = 0;

                invoiceCommonService.FetchSalesInvoiceWarrantyInfoByProduct((selectedInvoiceId = $scope.selectedInvoiceId == undefined ? "" : $scope.selectedInvoiceId), $scope.selectedProductId).then(function (value) {
                    $scope.warrantyDays = value.data.WarrantyDays;
                    $scope.warrantyLeft = value.data.WarrantyDaysLeft;
                    $scope.serviceWarrantyDays = value.data.ServiceWarrantyDays;
                    $scope.serviceWarrantyDaysAvailable = value.data.ServiceWarrantyDaysAvailable;
                }, function (reason) {
                    console.log(reason);
                });
            }
            //get product dimension
            var GetProductWiseDimension = function () {
                productCommonService.FetchProductWiseDimension(
                    '', $scope.selectedProductId
                ).then(function (value) {
                    $scope.productDimensionDropDownJsonData = value.data;
                    if (value.data.length > 0) {
                        $scope.dimensionId = value.data[0].Value;
                        $scope.getDimensionName();
                    } else {
                        $scope.dimensionName = '';
                    }
                }, function (reason) {
                    console.log(reason);
                });
            }
            //get product unit type
            var GetProductWiseUnitType = function () {
                productCommonService.FetchProductWiseUnitType(
                    '', $scope.selectedProductId
                ).then(function (value) {
                    $scope.unitTypeDropDownJsonData = value.data;
                    $.each(value.data, function () {
                        if (this.IsSelected) {
                            $scope.unitTypeId = this.Value;
                            $scope.getUnitTypeName();
                        }
                    });
                }, function (reason) {
                    console.log(reason);
                });
            };
            var ProductSerialOrNot = function () {
                productCommonService.FetchProductSerialOrNot($scope.selectedProductId).then(function (value) {
                    $scope.productSerialOrNot = value.data;
                }, function (reason) {
                    console.log(reason);
                });
            };
            //get product cost
            var GetProductCost = function () {
                productCommonService.FetchProductCost($scope.selectedProductId, ($scope.productDimensionDropDownJsonData.length > 0 ? $scope.dimensionId.toString() : '0'),
                    $scope.unitTypeId, $scope.selectedLocationId, $scope.wareHouseId
                ).then(function (value) {
                    $scope.unitCost = value.data.Cost;
                }, function (reason) {
                    console.log(reason);
                });
            };
            //load Product Stock Type Drop Down
            var GetProductStockTypeDropdown = function () {
                stockTypeCommonService.FetchProductStockType().then(function (value) {
                    $scope.productStockTypeDropdownJsonData = value.data;
                }, function (reason) {
                    console.log(reason);
                });
            }

            // load currency
            var GetCurrencyForDropdown = function () {
                currencyCommonService.FetchCurrency().then(function (value) {
                    $scope.currencyDropDownJsonData = value.data;
                    $scope.currencyId = $scope.DefaultCurrency;
                    //$scope.currencyIdForGrid = $scope.DefaultCurrency;
                    GetCurrencyRate();
                }, function (reason) {
                    console.log(reason);
                });
            }

            var GetCurrencyRate = function () {
                currencyCommonService.FetchCurrencyRate(
                    $scope.currencyId
                ).then(function (value) {
                    $scope.exchangeRate = value.data.ExchangeRate;
                    $scope.baseCurrency = value.data.BaseCurrency;
                    $scope.isBaseCurrencySelected = ($scope.baseCurrency === $scope.currencyId);
                }, function (reason) {
                    console.log(reason);
                });
            }

            GetCurrencyForDropdown();
            GetAllProblem();
            GetAllCharge();
            GetProductStockTypeDropdown();
            GetAllComplainReceiveGridListsData();
        }
    ])
    .config(function ($locationProvider) {
        $locationProvider.html5Mode(true);
    });