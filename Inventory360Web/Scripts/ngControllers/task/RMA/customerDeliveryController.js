myApp
    .controller('customerDeliveryController', ['$scope', '$ngConfirm', '$filter', 'dataTableRows', 'applicationBasePath', 'userService',
        'currencyCommonService', 'employeeCommonService', 'productCommonService', 'stockTypeCommonService',
        'customerDeliveryService', 'customerCommonService', 'complainReceiveService',
        function (
            $scope, $ngConfirm, $filter, dataTableRows, applicationBasePath, userService,
            currencyCommonService, employeeCommonService, productCommonService, stockTypeCommonService,
            customerDeliveryService, customerCommonService, complainReceiveService
        ) {
            $scope.OperationalEvent = "RMA";
            $scope.OperationalSubEvent = "CustomerDelivery";
            $scope.operationTypeId = 1;//Regular


            $scope.selectedCurrency = $scope.DefaultCurrency;




            $scope.baseCurrency = $scope.DefaultCurrency;
            $scope.selectedLocationId = 0;
            $scope.wareHouseId = 0;
            $scope.dimensionId = 0;
            $scope.spareProductdimensionId = 0;
            $scope.newPoductDimensionId = 0;
            $scope.isAdjustmentRequired = false;
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
            $scope.isAdded = false;
            $scope.isSaved = false;
            $scope.isDisableCustomer = false;
            $scope.customerDeliveryDate = $filter('date')(new Date(), "M/d/yyyy");
            $scope.productTotal = 0;
            $scope.grandTotal = 0;
            $scope.discount = 0;
            $scope.netTotal = 0;
            $scope.totalServiceProductAmount = 0;

            $scope.loadExchangeRate = function () {
                $scope.selectedCurrency = $scope.currencyId;
                GetCurrencyRate();
            };

            //change Delivery Product Option
            $scope.changeDelvProductOption = function () {
                $scope.selectedNewProductId = 0;
                $scope.newProductName = "";
                $scope.newProductNameJsonData = null;

                $scope.productNewSerial = "";
                $scope.searchProductNewSerial = "";
                $scope.productNewSerialJsonData = null;
            };
            //clculateProductTotal
            $scope.clculateProductTotal = function () {
                $scope.productTotal = 0;
                $.each($scope.addedProductLists, function () {
                    $scope.productTotal += parseFloat(this.TotalSpareAmount);
                });
                $scope.grandTotal = $scope.productTotal + $scope.totalServiceChargeAmount;
                $scope.netTotal = $scope.grandTotal - $scope.discount;
            };
            //calculate Net Total
            $scope.calculateNetTotal = function () {
                $scope.netTotal = $scope.grandTotal - $scope.discount;
            };

            //AdvanchSearch
            $scope.clickAdvanchSearch = function () {
                $scope.isAdvanchSearch = !$scope.isAdvanchSearch;
            };
            //isAdjustmentRequired
            $scope.clickAdjustmentRequired = function () {
                $scope.isAdjustmentRequired = !$scope.isAdjustmentRequired;
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
                $scope.clculateProductTotal();
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
            //get Complain ReceiveNo
            $scope.getComplainReceiveNo = function () {
                var q = ($scope.complainReceiveNo === undefined || $scope.complainReceiveNo == "*") ? '' : $scope.complainReceiveNo;

                if (q.length >= 3 || $scope.complainReceiveNo == "*") {
                    complainReceiveService.FetchAllComplainReceiveNo(q, (selectedCustomerId = $scope.selectedCustomerId === undefined ? 0 : $scope.selectedCustomerId), $scope.dateFrom, $scope.dateTo).then(function (value) {
                        $scope.complainReceiveNoJsonData = value.data;
                    }, function (reason) {
                        console.log(reason);
                    });
                } else {
                    $scope.selectedComplainReceiveId = undefined;
                    $scope.complainReceiveNoJsonData = null;
                }
            };
            $scope.fillComplainReceiveNoTextbox = function (obj) {
                $scope.selectedComplainReceiveId = obj.Value;
                $scope.complainReceiveNo = obj.Item;
                $scope.complainReceiveNoJsonData = null;
                GetAllCharge();
                complainReceiveService.FetchAllComplainReceiveShortInfoById($scope.selectedComplainReceiveId).then(function (value) {
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
                    complainReceiveService.FetchProductNameByComplainReceive(q, ($scope.selectedComplainReceiveId === undefined || $scope.selectedComplainReceiveId == 0 ? '' : $scope.selectedComplainReceiveId)).then(function (value) {
                        $scope.productNameJsonData = value.data;
                    }, function (reason) {
                        console.log(reason);
                    });
                } else {
                    $scope.selectedProductId = 0;
                    $scope.productNameJsonData = null;
                }
            };
            $scope.fillProductTextbox = function (obj) {
                $scope.selectedProductId = Number(obj.Value);
                $scope.productName = obj.Item;
                //GetComplainReceiveInfoByProduct();
                GetProductWiseDimension();
                GetProductWiseUnitType();
                if ($scope.cusDelvProductOptionValue == '1' || $scope.cusDelvProductOptionValue == '2' || $scope.cusDelvProductOptionValue == '4') {
                    //$scope.newProductNameJsonData = angular.copy($scope.productNameJsonData);
                    $scope.fillNawProductTextbox(obj);
                }
                $scope.productNameJsonData = null;
            };
            //product serial search
            $scope.getProductSerial = function () {
                var q = ($scope.searchProductSerial === undefined || $scope.searchProductSerial == "*") ? '' : $scope.searchProductSerial;

                if (q.length >= 2 || $scope.searchProductSerial == "*") {
                    complainReceiveService.FetchProductBySerialFromComplainReceive(q, $scope.selectedProductId, ($scope.selectedComplainReceiveId === undefined || $scope.selectedComplainReceiveId == 0 ? '' : $scope.selectedComplainReceiveId)).then(function (value) {
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
                GetProductWiseDimension();
                GetProductWiseUnitType();
                GetComplainReceiveInfoByProduct();
                //1 = Same Product Same Serial, 2 = Same Product Different Serial, 3 = Different Product Different Serial, 4 = Cash Back
                if ($scope.cusDelvProductOptionValue == '1' || $scope.cusDelvProductOptionValue == '4') {
                    $scope.fillproductNewSerialTextbox(obj);
                }
                else if ($scope.cusDelvProductOptionValue == '2') {
                    $scope.fillproductNewSerialTextbox(obj);
                    $scope.productNewSerial = "";
                    $scope.searchProductNewSerial = "";
                }
                else if ($scope.cusDelvProductOptionValue == '3') {
                    $scope.fillproductNewSerialTextbox(obj);
                    $scope.productNewSerial = "";
                    $scope.searchProductNewSerial = "";
                    $scope.selectedNewProductId = 0;
                    $scope.newProductName = "";
                }
                $scope.productSerialJsonData = null;
            };
            // new product search
            $scope.getNewProductName = function () {
                var q = $scope.newProductName === undefined ? '' : $scope.newProductName;
                if (q.length >= 2) {
                    productCommonService.FetchProductNameFromRMA(q).then(function (value) {
                        $scope.newProductNameJsonData = value.data;
                    }, function (reason) {
                        console.log(reason);
                    });
                } else {
                    $scope.selectedNewProductId = 0;
                    $scope.newProductNameJsonData = null;
                }
            };
            $scope.fillNawProductTextbox = function (obj) {
                $scope.selectedNewProductId = Number(obj.Value);
                $scope.newProductName = obj.Item;
                GetNewProductWiseUnitType();
                GetNewProductCost();
                GetNewProductWiseDimension();
                $scope.newProductNameJsonData = null;
            };
            //product new serial search
            $scope.getProductNewSerial = function () {
                var q = ($scope.searchProductNewSerial === undefined || $scope.searchProductNewSerial == "*") ? '' : $scope.searchProductNewSerial;
                if (q.length >= 2 || ($scope.searchProductNewSerial == "*")) {
                    productCommonService.FetchProductByRMASerial(q, $scope.selectedNewProductId === undefined ? 0 : $scope.selectedNewProductId).then(function (value) {
                        $scope.productNewSerialJsonData = value.data;
                    }, function (reason) {
                        console.log(reason);
                    });
                }
                else {
                    $scope.productNewSerial = "";
                    $scope.productNewSerialJsonData = null;
                }
            };
            $scope.fillproductNewSerialTextbox = function (obj) {

                $scope.productNewSerial = obj.Item;
                $scope.searchProductNewSerial = obj.Item;
                $scope.selectedNewProductId = obj.ProductId;
                $scope.newProductName = obj.ProductName;
                GetNewProductWiseUnitType();
                GetNewProductCost();
                GetNewProductWiseDimension();
                $scope.productNewSerialJsonData = null;
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
                $scope.productIdForSparePart = entity.NewProductId;
                $scope.productNameForSparePart = entity.NewName;
                $scope.productSerialForSparePart = entity.NewSerial;
                ProductWiseSpareProductListprepare();

            }
            var ProductWiseSpareProductListprepare = function () {
                $scope.totalSpareProductAmount = 0;
                $scope.totalServiceProductAmount = 0;
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
                $scope.spareProductDimensionDropDownJsonData = [];
                productCommonService.FetchProductWiseDimension(
                    '', $scope.selectedSpareProductId
                ).then(function (value) {
                    $scope.spareProductDimensionDropDownJsonData = value.data;
                    if ($scope.spareProductDimensionDropDownJsonData.length > 0) {
                        $scope.spareProductdimensionId = $scope.spareProductDimensionDropDownJsonData[0].Value;
                        //$scope.getDimensionName();
                    }
                }, function (reason) {
                    console.log(reason);
                });
            }
            //get stock,price and discount
            var GetProductWiseAvailableStockPriceDiscount = function () {
                $scope.availableStock = 0;
                $scope.spareUnitCost = 0;
                $scope.spareDiscount = 0;
                productCommonService.FetchProductWiseAvailableStockPriceDiscountByOperationalEvent($scope.OperationalEvent, $scope.OperationalSubEvent, $scope.baseCurrency, $scope.operationTypeId, $scope.selectedSpareProductId, $scope.spareProductdimensionId,
                    $scope.spareUnitTypeId, $scope.selectedLocationId, $scope.wareHouseId
                ).then(function (value) {
                    console.log(value.data);
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
                        "Remarks": $scope.spareProductremarks,
                        "isServiceProduct": $scope.isServiceProduct,
                        "SpareType": $scope.spareTypeId,
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
                            this.isServiceProduct = $scope.isServiceProduct;
                            this.SpareType = $scope.spareTypeId;
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
                updateProductListBySpareProduct();
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
                $scope.calculatetotalSpareProductAmount();
                ProductWiseSpareProductListprepare();
                updateProductListBySpareProduct();
                $scope.clculateProductTotal();
            };
            var updateProductListBySpareProduct = function () {
                $.each($scope.addedProductLists, function (index) {
                    if (this.NewProductId == $scope.productIdForSparePart && this.NewSerial == $scope.productSerialForSparePart) {
                        this.SparePartsAmount = $scope.totalSpareProductAmount;
                        this.ServiceChargeAmount = $scope.totalServiceProductAmount;
                        this.TotalSpareAmount = parseFloat(this.SparePartsAmount) + this.ServiceChargeAmount;
                        this.customerDeliveryDetail_SpareProduct = $scope.productWiseSpareProductList.concat($scope.productWiseServiceProductList);
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
                if (($scope.customerName == "" || $scope.customerName == undefined) || $scope.searchProductSerial == "" || $scope.searchProductSerial == undefined || $scope.productNewSerial == "" || $scope.productNewSerial == undefined) {
                    // alert
                    $ngConfirm({
                        title: 'Required',
                        icon: 'glyphicon glyphicon-exclamation-sign',
                        theme: 'supervan',
                        content: 'Customer Name or Product serial/new serial is empty!!!.',
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



                if (previousIentity == 0) {
                    $scope.addedProductLists.push({
                        "identity": ProductIdentity($scope.selectedProductId, $scope.searchProductSerial, ($scope.productDimensionDropDownJsonData.length > 0 ? $scope.dimensionId.toString() : '0'), $scope.unitTypeId.toString()),
                        "InvoiceId": $scope.selectedComplainReceiveId,
                        "ComplainReceiveId": $scope.selectedComplainReceiveId,
                        "PreviousProductId": $scope.selectedProductId.toString(),
                        "PreviousProductDimensionId": $scope.productDimensionDropDownJsonData.length > 0 ? $scope.dimensionId.toString() : '0',
                        "Name": $scope.productName,
                        "Dimension": $scope.dimensionName,
                        "PreviousUnitTypeId": $scope.unitTypeId.toString(),
                        //"UnitTypeName": $scope.unitTypeName,
                        "PreviousSerial": $scope.searchProductSerial,
                        "NewProductId": $scope.selectedNewProductId,
                        "NewName": $scope.newProductName,
                        "NewProductDimensionId": $scope.newPoductDimensionId,
                        "NewUnitTypeId": $scope.newProductIdUnitTypeId.toString(),
                        "UnitTypeName": $scope.newProductUnitTypeName,
                        "NewSerial": $scope.productNewSerial,
                        "Cost": $scope.newProductunitCost,
                        "IsAdjustmentRequired": $scope.isAdjustmentRequired,
                        "AdjustmentType": $scope.adjustmentTypeId,
                        "AdjustedAmount": $scope.adjustmentAmount,
                        "DeliveryType": $scope.cusDelvProductOptionValue,
                        "Problem": getAllSelectedProblem(),
                        "customerDeliveryDetail_Problem": $scope.allSelectedProblems,
                        "SparePartsAmount": 0,
                        "ServiceChargeAmount": 0,
                        "TotalSpareAmount": 0 + $scope.totalServiceProductAmount,
                        "customerDeliveryDetail_SpareProduct": []
                    });

                    $scope.productIdForSparePart = $scope.selectedNewProductId.toString();
                    $scope.productNameForSparePart = $scope.newProductName;
                    $scope.productSerialForSparePart = $scope.productNewSerial;
                    GetSpareProductByParentProduct();

                    $scope.isDisableCustomer = true;
                }
                else {
                    $.each($scope.addedProductLists, function (index) {
                        if (this.identity === previousIentity) {
                            this.ComplainReceiveId = $scope.selectedComplainReceiveId,
                                this.PreviousProductId = $scope.selectedProductId.toString();
                            this.PreviousProductDimensionId = $scope.productDimensionDropDownJsonData.length > 0 ? $scope.dimensionId.toString() : '0';
                            this.Name = $scope.productName;
                            this.Dimension = $scope.dimensionName;
                            this.PreviousUnitTypeId = $scope.unitTypeId.toString();
                            this.UnitTypeName = $scope.unitTypeName;
                            this.PreviousSerial = $scope.searchProductSerial;
                            this.NewProductId = $scope.selectedNewProductId;
                            this.NewName = $scope.newProductName;
                            this.NewProductDimensionId = $scope.newPoductDimensionId;
                            this.NewUnitTypeId = $scope.newProductIdUnitTypeId.toString();
                            this.UnitTypeName = $scope.newProductUnitTypeName;
                            this.NewSerial = $scope.productNewSerial;
                            this.Cost = $scope.unitCost;
                            this.IsAdjustmentRequired = $scope.isAdjustmentRequired;
                            this.AdjustmentType = $scope.adjustmentTypeId;
                            this.AdjustedAmount = $scope.adjustmentAmount;
                            this.DeliveryType = $scope.cusDelvProductOptionValue;
                            this.Problem = getAllSelectedProblem();
                            this.customerDeliveryDetail_Problem = $scope.allSelectedProblems;
                            this.TotalSpareAmount = 0 + $scope.totalServiceProductAmount;
                            return false;
                        }
                    });
                }
                //clear product serial
                $scope.productSerial = "";
                $scope.searchProductSerial = "";
                $scope.productSerialJsonData = null;

                $scope.selectedNewProductId = 0;
                $scope.newProductName = "";
                $scope.productNewSerial = "";
                $scope.searchProductNewSerial = "";
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
            //get spare product by Parent product
            var GetSpareProductByParentProduct = function () {
                customerDeliveryService.FetchSpareProductByParentRcvdIdAndProduct($scope.selectedComplainReceiveId == undefined ? "" : $scope.selectedComplainReceiveId, $scope.productIdForSparePart, $scope.productSerialForSparePart).then(function (value) {

                    $.each(value.data, function () {
                        $scope.selectedSpareProductId = this.ProductId;
                        $scope.spareProductdimensionId = this.ProductDimensionId;
                        $scope.spareProductName = this.ProductName;
                        $scope.spareProductremarks = this.Remarks;
                        $scope.isServiceProduct = this.isServiceProduct;
                        $scope.spareUnitTypeId = this.UnitTypeId;
                        $scope.spareUnitTypeName = this.UnitTypeName;
                        $scope.spareQuantity = this.Quantity;
                        $scope.spareUnitCost = this.Price;
                        $scope.spareDiscount = this.Discount;
                        GetSpareProductWiseDimension();
                        $scope.clickToAddSpareProduct();
                    });
                }, function (reason) {
                    console.log(reason);
                });
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

                if ($scope.customerDeliveryNo != undefined) {
                    // alert
                    $ngConfirm({
                        title: 'Warning',
                        icon: 'glyphicon glyphicon-exclamation-sign',
                        theme: 'supervan',
                        content: 'This Custome rDelivery already saved.',
                        animation: 'scale',
                        buttons: {
                            Ok: {
                                btnClass: "btn-blue"
                            }
                        },
                    });
                    return;
                }
                $scope.totalAmount = 0;
                $scope.totalAmount = $scope.productTotal;
                $scope.totalChargeAmount = 0;
                $scope.totalChargeAmount = $scope.totalServiceChargeAmount;
                $scope.customerDelivery_Charge = $scope.allSelectedCharges;

                $scope.isSaved = true;
                customerDeliveryService.SaveCustomerDelivery(
                    $scope.currencyId, $scope.exchangeRate, $scope.selectedCustomerId, $scope.customerDeliveryDate, $scope.selectedEmployeeId,
                    $scope.remarks, $scope.totalChargeAmount, $scope.totalAmount, $scope.discount, $scope.customerDelivery_Charge,
                    $scope.addedProductLists
                ).then(function (value) {
                    if (Number(value.status) === 200 && value.data.IsSuccess === true) {
                        $scope.customerDeliveryNo = value.data.Message;
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

            $scope.PrintReport = function (deliveryNo) {
                var url = applicationBasePath + "/Inventory360Reports/PrintCustomerDelivery?no=" + deliveryNo + "&currency=" + $scope.baseCurrency + "&user=" + $scope.UserName + "&token=" + userService.GetCurrentUser().access_token;
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
                if ($scope.searchCustomerDelivery !== undefined) {
                    $scope.pageIndex = 1;
                    GetAllComplainReceiveGridListsData();
                }
            };
            //get Spare Type Name
            $scope.getSpareTypeName = function () {
                $.each($scope.spareTypeDropDownJsonData, function () {
                    if (this.Value === $scope.spareTypeId) {
                        $scope.spareTypeName = this.Item;
                    }
                });
            };
            //get Adjustment Type Name
            $scope.getAdjustmentTypeName = function () {
                $.each($scope.adjustmentTypeDropDownJsonData, function () {
                    if (this.Value === $scope.adjustmentTypeId) {
                        $scope.adjustmentTypeName = this.Item;
                    }
                });
            };
            //get dimension
            $scope.getDimensionName = function () {
                $.each($scope.productDimensionDropDownJsonData, function () {
                    if (this.Value === $scope.dimensionId) {
                        $scope.dimensionName = this.Item;
                    }
                });
            };
            // load all CustomerDelivery lists
            var GetAllComplainReceiveGridListsData = function () {
                var q = $scope.searchCustomerDelivery === undefined ? '' : $scope.searchCustomerDelivery;

                customerDeliveryService.FetchCustomerDeliveryLists(
                    q, $scope.pageIndex, dataTableRows
                ).then(function (value) {
                    $scope.allCustomerDeliveryJsonLists = value.data;
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
                        $scope.customerNameJsonData = value.data;
                        $scope.customerNameByPhoneJsonData = angular.copy($scope.customerNameJsonData);
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
            var GetComplainReceiveInfoByProduct = function () {
                complainReceiveService.FetchComplainReceiveInfoByProduct($scope.selectedProductId, $scope.productSerial).then(function (value) {
                    $scope.selectedCustomerId = value.data.CustomerId;
                    $scope.customerName = value.data.CustomerName;
                    $scope.customerMobileNo = value.data.CustomerPhoneNo;
                    $scope.customerAddress = value.data.CustomerAddress;
                    $scope.selectedComplainReceiveId = value.data.ReceiveId;
                    $scope.ReceiveDetailId = value.data.ReceiveDetailId;
                    $scope.complainReceiveNo = value.data.ReceiveNo;
                    GetAllProblem();
                    if ($scope.selectedComplainReceiveId != undefined) {
                        GetAllCharge();
                    }
                }, function (reason) {
                    console.log(reason);
                });
            }
            //get all problem
            var GetAllProblem = function () {
                customerDeliveryService.FetchAllProblemWithComplainReceived(
                    $scope.ReceiveDetailId == undefined ? "" : $scope.ReceiveDetailId
                ).then(function (value) {
                    $scope.allProblemJsonLists = value.data;
                }, function (reason) {
                    console.log(reason);
                });
            }
            //get all charge
            var GetAllCharge = function () {
                customerDeliveryService.FetchAllChargeWithComplainReceived(
                    $scope.selectedComplainReceiveId == undefined ? "" : $scope.selectedComplainReceiveId, $scope.currencyId
                ).then(function (value) {
                    $scope.allChargeJsonLists = value.data;
                    $scope.calculatetotalServiceChargeAmount();
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
            //get new product dimension
            var GetNewProductWiseDimension = function () {
                productCommonService.FetchProductWiseDimension(
                    '', $scope.selectedNewProductId
                ).then(function (value) {
                    $scope.newPoductDimensionDropDownJsonData = value.data;
                    if (value.data.length > 0) {
                        $scope.newPoductDimensionId = value.data[0].Value;
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
            //get new product unit type
            var GetNewProductWiseUnitType = function () {
                productCommonService.FetchProductWiseUnitType(
                    '', $scope.selectedNewProductId
                ).then(function (value) {
                    $scope.newProductIdUnitTypeDropDownJsonData = value.data;
                    $.each(value.data, function () {
                        if (this.IsSelected) {
                            $scope.newProductIdUnitTypeId = this.Value;

                            if (this.Value === $scope.newProductIdUnitTypeId) {
                                $scope.newProductUnitTypeName = this.Item;
                            }
                        }
                    });

                }, function (reason) {
                    console.log(reason);
                });
            };
            //get product cost
            var GetProductCost = function () {
                productCommonService.FetchProductCost($scope.selectedProductId, ($scope.productDimensionDropDownJsonData.length > 0 ? $scope.dimensionId.toString() : '0'),
                    $scope.unitTypeId, $scope.selectedLocationId, $scope.wareHouseId
                ).then(function (value) {
                    console.log(value.data);
                    $scope.unitCost = value.data.Cost;
                }, function (reason) {
                    console.log(reason);
                });
            };
            //get New product cost
            var GetNewProductCost = function () {
                productCommonService.FetchProductCost($scope.selectedNewProductId, '0',
                    $scope.newProductIdUnitTypeId, $scope.selectedLocationId, $scope.wareHouseId
                ).then(function (value) {
                    console.log(value.data);
                    $scope.newProductunitCost = value.data.Cost;
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
            // load ChequeType
            var GetCusDelvProductOption = function () {
                customerDeliveryService.GetCusDelvProductOption().then(function (value) {
                    $scope.CusDelvProductOptionDropDownJsonData = value.data;
                    $.each($scope.CusDelvProductOptionDropDownJsonData, function () {
                        if (this.IsSelected) {
                            $scope.cusDelvProductOptionValue = this.Value;
                            return;
                        }
                    });
                }, function (reason) {
                    console.log(reason);
                });
            }
            // load Adjustment Type
            var GetAdjustmentType = function () {
                customerDeliveryService.GetAdjustmentType().then(function (value) {
                    $scope.adjustmentTypeDropDownJsonData = value.data;
                    $.each($scope.adjustmentTypeDropDownJsonData, function () {
                        if (this.IsSelected) {
                            $scope.adjustmentTypeId = this.Value;
                            return;
                        }
                    });
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
            GetSpareType();
            GetProductStockTypeDropdown();
            GetAllComplainReceiveGridListsData();
            GetCusDelvProductOption();
            GetAdjustmentType();
        }
    ])
    .config(function ($locationProvider) {
        $locationProvider.html5Mode(true);
    });