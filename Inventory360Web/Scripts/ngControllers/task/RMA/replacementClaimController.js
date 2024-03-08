myApp
    .controller('replacementClaimController', ['$scope', '$ngConfirm', '$filter', 'dataTableRows', 'applicationBasePath', 'userService',
        'currencyCommonService', 'employeeCommonService', 'productCommonService',
        'replacementClaimService', 'customerCommonService', 'supplierCommonService',
        function (
            $scope, $ngConfirm, $filter, dataTableRows, applicationBasePath, userService,
            currencyCommonService, employeeCommonService, productCommonService,
            replacementClaimService, customerCommonService, supplierCommonService
        ) {
            $scope.dimensionId = 0;
            $scope.selectedSupplierId = 0;
            $scope.baseCurrency = $scope.DefaultCurrency;
            $scope.addedProductLists = [];
            $scope.pageIndex = 1;
            $scope.selectedEmployeeId = 0;
            $scope.selectedProductId = 0;
            $scope.productSerialOrNot = false;
            $scope.isAdvanchSearch = false;        
            $scope.isAdded = false;
            $scope.isSaved = false;
            $scope.isDisableCustomer = false;
            $scope.claimDate = $filter('date')(new Date(), "M/d/yyyy");

            //loadExchangeRate
            $scope.loadExchangeRate = function () {
                $scope.selectedCurrency = $scope.currencyId;
                GetCurrencyRate();
            };

            //getSupplierName
            $scope.getSupplierName = function () {
                var q = $scope.supplierName === undefined ? '' : $scope.supplierName;
                if (q.length >= 3) {
                    supplierCommonService.FetchAllSupplier(
                        q
                    ).then(function (value) {
                        $scope.supplierNameJsonData = value.data;
                    });
                } else {
                    $scope.selectedSupplierId = 0;
                    $scope.supplierNameJsonData = null;
                }
            };

            $scope.fillSupplierTextbox = function (obj) {
                $scope.supplierName = '';
                $scope.selectedSupplierId = Number(obj.Value);
                $scope.supplierName = obj.Item;
                supplierCommonService.FetchSupplierShortInfo(
                    $scope.selectedSupplierId, $scope.baseCurrency
                ).then(function (value) {
                    $scope.supplierAddress = value.data.Address;
                }, function (reason) {
                    console.log(reason);
                });
                $scope.supplierNameJsonData = null;
            };

            //get Complain ReceiveNo
            $scope.getComplainReceiveNo = function () {
                var q = ($scope.complainReceiveNo === undefined || $scope.complainReceiveNo == "*") ? '' : $scope.complainReceiveNo;

                if (q.length >= 3 || $scope.complainReceiveNo == "*") {
                    replacementClaimService.FetchAllComplainReceiveNo(
                        q, (selectedCustomerId = $scope.selectedCustomerId === undefined ? 0 : $scope.selectedCustomerId), $scope.dateFrom, $scope.dateTo
                    ).then(function (value) {
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

                replacementClaimService.FetchAllComplainReceiveShortInfoById(
                    $scope.selectedComplainReceiveId
                ).then(function (value) {
                    $scope.selectedCustomerId = value.data.CustomerId;
                    $scope.customerName = value.data.CustomerName;
                    $scope.customerMobileNo = value.data.CustomerPhoneNo;
                    $scope.customerAddress = value.data.CustomerAddress;
                }, function (reason) {
                    console.log(reason);
                });
            };

            $scope.clickAdvanchSearch = function () {
                $scope.isAdvanchSearch = !$scope.isAdvanchSearch;
            };

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

            //get customer
            $scope.getCustomerName = function () {
                var q = $scope.customerName === undefined ? '' : $scope.customerName;
                getCustomer(q);
            };

            $scope.getCustomerNameByPhone = function () {
                var q = $scope.customerMobileNo === undefined ? '' : $scope.customerMobileNo;
                getCustomer(q);
            };

            $scope.fillCustomerTextbox = function (obj) {
                $scope.customerOrSupplierName = '';
                $scope.selectedCustomerId = Number(obj.Value);
                $scope.customerName = obj.Item;
                $scope.customerNameJsonData = null;
                $scope.customerNameByPhoneJsonData = null;

                customerCommonService.FetchCustomerShortInfo(
                    $scope.selectedCustomerId
                ).then(function (value) {
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
                    if ($scope.selectedComplainReceiveId) {
                        replacementClaimService.FetchRMAProductNameByComplainReceive(
                            q, ($scope.selectedComplainReceiveId === undefined || $scope.selectedComplainReceiveId == 0 ? '' : $scope.selectedComplainReceiveId)
                        ).then(function (value) {
                            $scope.productNameJsonData = value.data;
                        }, function (reason) {
                            console.log(reason);
                        });
                    }
                    else {
                        productCommonService.FetchProductNameFromRMA(
                            q
                        ).then(function (value) {
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
                GetProductWiseDimension();
                GetProductWiseUnitType();
                ProductSerialOrNot();
                $scope.unitCost = obj.Cost;
                $scope.productNameJsonData = null;
            };

            //product serial search
            $scope.getProductSerial = function () {
                var q = ($scope.searchProductSerial === undefined || $scope.searchProductSerial == "*") ? '' : $scope.searchProductSerial;

                if (q.length >= 2 || $scope.searchProductSerial == "*") {
                    if ($scope.selectedComplainReceiveId) {
                        replacementClaimService.FetchRMAProductBySerialFromComplainReceive(
                            q, $scope.selectedProductId, ($scope.selectedComplainReceiveId === undefined || $scope.selectedComplainReceiveId == 0 ? '' : $scope.selectedComplainReceiveId)
                        ).then(function (value) {
                            $scope.productSerialJsonData = value.data;
                        }, function (reason) {
                            console.log(reason);
                        });
                    }
                    else {
                        productCommonService.FetchProductByRMASerial(
                            q, $scope.selectedProductId
                        ).then(function (value) {
                            $scope.productSerialJsonData = value.data;
                        }, function (reason) {
                            console.log(reason);
                        });
                    }
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
                GetProductWiseDimension();
                GetProductWiseUnitType();
                ProductSerialOrNot();
                GetComplainReceiveInfoByProduct();
                ProductStockInReferenceInfo();
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

            //add parent product
            $scope.clickToAdd = function () {
                if (($scope.supplierName == "" || $scope.supplierName == undefined) || $scope.searchProductSerial == "" || $scope.searchProductSerial == undefined) {
                    // alert
                    $ngConfirm({
                        title: 'Required',
                        icon: 'glyphicon glyphicon-exclamation-sign',
                        theme: 'supervan',
                        content: 'Supplier Name or Product serial is empty!!!.',
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
                        "ComplainReceiveId": $scope.selectedComplainReceiveId,
                        'ComplainReceiveNo': $scope.complainReceiveNo,
                        'StockInRefNo': $scope.productStockInReferenceInfoData == null || $scope.productStockInReferenceInfoData.StockInRefNo == undefined ? '' : $scope.productStockInReferenceInfoData.StockInRefNo,
                        'StockInRefDate': $scope.productStockInReferenceInfoData == null || $scope.productStockInReferenceInfoData.StockInRefDate == undefined ? 'null' : $scope.productStockInReferenceInfoData.StockInRefDate,
                        'LCOrReferenceNo': $scope.productStockInReferenceInfoData == null || $scope.productStockInReferenceInfoData.ReferenceNo == undefined ? '' : $scope.productStockInReferenceInfoData.ReferenceNo,
                        'LCOrReferenceDate': $scope.productStockInReferenceInfoData == null || $scope.productStockInReferenceInfoData.ReferenceDate == undefined ? 'null' : $scope.productStockInReferenceInfoData.ReferenceDate,
                        "ProductId": $scope.selectedProductId.toString(),
                        "ProductDimensionId": $scope.productDimensionDropDownJsonData.length > 0 ? $scope.dimensionId.toString() : '0',
                        "Name": $scope.productName,
                        "Dimension": $scope.dimensionName,
                        "UnitTypeId": $scope.unitTypeId.toString(),
                        "UnitTypeName": $scope.unitTypeName,
                        "Serial": $scope.searchProductSerial,
                        "Cost": $scope.unitCost,
                        "Problem": getAllSelectedProblem(),
                        "replacementClaimDetail_Problem": $scope.allSelectedProblems
                    });
                    $scope.isDisableCustomer = true;
                }
                else {
                    $.each($scope.addedProductLists, function (index) {
                        if (this.identity === previousIentity) {
                            this.ComplainReceiveId = $scope.selectedComplainReceiveId;
                            this.ComplainReceiveNo = $scope.complainReceiveNo;
                            this.StockInRefNo = $scope.productStockInReferenceInfoData == null || $scope.productStockInReferenceInfoData.StockInRefNo == undefined ? '' : $scope.productStockInReferenceInfoData.StockInRefNo;
                            this.StockInRefDate = $scope.productStockInReferenceInfoData == null || $scope.productStockInReferenceInfoData.StockInRefDate == undefined ? 'null' : $scope.productStockInReferenceInfoData.StockInRefDate;
                            this.LCOrReferenceNo = $scope.productStockInReferenceInfoData == null || $scope.productStockInReferenceInfoData.ReferenceNo == undefined ? '' : $scope.productStockInReferenceInfoData.ReferenceNo;
                            this.LCOrReferenceDate = $scope.productStockInReferenceInfoData == null || $scope.productStockInReferenceInfoData.ReferenceDate == undefined ? 'null' : $scope.productStockInReferenceInfoData.ReferenceDate;
                            this.ProductId = $scope.selectedProductId.toString();
                            this.ProductDimensionId = $scope.productDimensionDropDownJsonData.length > 0 ? $scope.dimensionId.toString() : '0';
                            this.Name = $scope.productName;
                            this.Dimension = $scope.dimensionName;
                            this.UnitTypeId = $scope.unitTypeId.toString();
                            this.UnitTypeName = $scope.unitTypeName;
                            this.Serial = $scope.searchProductSerial;
                            this.Cost = $scope.unitCost;                           
                            this.Problem = getAllSelectedProblem();
                            this.replacementClaimDetail_Problem = $scope.allSelectedProblems;                            
                        }
                    });
                }

                $scope.clickToReset();
            };

            $scope.clickRemoveItem = function (identity) {
                $.each($scope.addedProductLists, function (index) {
                    if (this.identity === identity) {
                        $scope.addedProductLists.splice(index, 1);
                        return false;
                    }
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

                if ($scope.replacementClaimNo != undefined) {
                    // alert
                    $ngConfirm({
                        title: 'Warning',
                        icon: 'glyphicon glyphicon-exclamation-sign',
                        theme: 'supervan',
                        content: 'This Replacement Out already saved.',
                        animation: 'scale',
                        buttons: {
                            Ok: {
                                btnClass: "btn-blue"
                            }
                        },
                    });
                    return;
                }

                $scope.isSaved = true;
                replacementClaimService.SaveReplacementClaim(
                    $scope.selectedSupplierId, $scope.claimDate, $scope.selectedEmployeeId, $scope.remarks, $scope.baseCurrency, $scope.addedProductLists
                ).then(function (value) {
                    if (Number(value.status) === 200 && value.data.IsSuccess === true) {
                        $scope.replacementClaimNo = value.data.Message;
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

                        GetAllReplacementClaimGridListsData();
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
                var url = applicationBasePath + "/Inventory360Reports/PrintReplacementClaim?id=" + id
                    + "&no=" +""
                    + "&user=" + $scope.UserName
                    + "&token=" + userService.GetCurrentUser().access_token;
                var win = window.open(url, '_blank');
                win.focus();
            };

            $scope.previous = function (e) {
                e.preventDefault();
                if ($scope.pageIndex > 1) {
                    $scope.pageIndex--;
                    GetAllReplacementClaimGridListsData();
                }
            };

            $scope.next = function (e) {
                e.preventDefault();
                if ($scope.pageIndex < lastPageNo) {
                    $scope.pageIndex++;
                    GetAllReplacementClaimGridListsData();
                }
            };

            $scope.filterComplainReceive = function () {
                if ($scope.searchReplacementClaim !== undefined) {
                    $scope.pageIndex = 1;
                    GetAllReplacementClaimGridListsData();
                }
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
                });

                return allSelectedProblemName;
            }

            // load all ComplainReceive lists
            var GetAllReplacementClaimGridListsData = function () {
                var q = $scope.searchReplacementClaim === undefined ? '' : $scope.searchReplacementClaim;

                replacementClaimService.FetchReplacementClaimLists(
                    q, $scope.pageIndex, dataTableRows
                ).then(function (value) {
                    $scope.allReplacementClaimJsonLists = value.data;
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

            //get all problem
            var GetAllProblem = function () {
                replacementClaimService.FetchAllProblemWithComplainReceived(
                    $scope.ReceiveDetailId == undefined ? "" : $scope.ReceiveDetailId
                ).then(function (value) {
                    $scope.allProblemJsonLists = value.data;
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

            var ProductStockInReferenceInfo = function () {
                productCommonService.FetchProductStockInReferenceInfo(
                    $scope.selectedProductId, $scope.searchProductSerial, $scope.selectedSupplierId
                ).then(function (value) {
                    $scope.productStockInReferenceInfoData = value.data;
                    if (value.data != null) {
                        $scope.selectedSupplierId = $scope.productStockInReferenceInfoData.SupplierId;
                        $scope.supplierName = $scope.productStockInReferenceInfoData.SupplierName;
                        $scope.supplierAddress = $scope.productStockInReferenceInfoData.SupplierAddress;
                    }
                    else {
                        // alert
                        $ngConfirm({
                            title: 'Warning',
                            icon: 'glyphicon glyphicon-exclamation-sign',
                            theme: 'supervan',
                            content: 'No Supplier found for this serial.',
                            animation: 'scale',
                            buttons: {
                                Ok: {
                                    btnClass: "btn-blue"
                                }
                            },
                        });
                        return;
                    }

                }, function (reason) {
                    console.log(reason);
                });
            };

            //get invoice info
            var GetComplainReceiveInfoByProduct = function () {
                replacementClaimService.FetchComplainReceiveInfoByProduct(
                    $scope.selectedProductId, $scope.productSerial, $scope.selectedComplainReceiveId === undefined || $scope.selectedComplainReceiveId == 0 ? '' : $scope.selectedComplainReceiveId
                ).then(function (value) {
                    if (value.data != null) {
                        $scope.selectedCustomerId = value.data.CustomerId;
                        $scope.customerName = value.data.CustomerName;
                        $scope.customerMobileNo = value.data.CustomerPhoneNo;
                        $scope.customerAddress = value.data.CustomerAddress;
                        $scope.selectedComplainReceiveId = value.data.ReceiveId;
                        $scope.ReceiveDetailId = value.data.ReceiveDetailId;
                        $scope.complainReceiveNo = value.data.ReceiveNo;
                        GetAllProblem();
                    }
                }, function (reason) {
                    console.log(reason);
                });
            };

            // load currency
            var GetCurrencyForDropdown = function () {
                currencyCommonService.FetchCurrency().then(function (value) {
                    $scope.currencyDropDownJsonData = value.data;
                    $scope.currencyId = $scope.DefaultCurrency;
                    $scope.currencyIdForGrid = $scope.DefaultCurrency;
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
                    $scope.selectedCurrency = value.data.BaseCurrency;
                    $scope.isBaseCurrencySelected = ($scope.baseCurrency === $scope.currencyId);
                }, function (reason) {
                    console.log(reason);
                });
            }

            GetCurrencyForDropdown();
            GetAllProblem();
            GetAllReplacementClaimGridListsData();
        }
    ])
    .config(function ($locationProvider) {
        $locationProvider.html5Mode(true);
    });