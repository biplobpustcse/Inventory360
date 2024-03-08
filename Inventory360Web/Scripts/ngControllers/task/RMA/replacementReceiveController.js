myApp
    .controller('replacementReceiveController', ['$scope', '$ngConfirm', '$filter', 'dataTableRows', 'applicationBasePath', 'userService',
        'partyAdjustmentNatureCommonService', 'currencyCommonService', 'employeeCommonService', 'productCommonService',
        'supplierCommonService', 'chargeSetupService', 'replacementReceiveService',
        function (
            $scope, $ngConfirm, $filter, dataTableRows, applicationBasePath, userService,
            partyAdjustmentNatureCommonService, currencyCommonService, employeeCommonService, productCommonService, 
            supplierCommonService, chargeSetupService, replacementReceiveService
        ) {
            $scope.selectedCurrency = $scope.DefaultCurrency;
            $scope.selectedLocationId = 0;
            $scope.wareHouseId = 0;
            $scope.dimensionId = 0;
            $scope.newPoductDimensionId = 0;
            $scope.totalServiceChargeAmount = 0;
            $scope.totalChargeAmount = 0;
            $scope.pageIndex = 1;
            $scope.selectedEmployeeId = 0;
            $scope.selectedProductId = 0;
            $scope.productTotal = 0;
            $scope.grandTotal = 0;
            $scope.discount = 0;
            $scope.netTotal = 0;
            $scope.adjustedAmount = 0;
            $scope.isAdjustmentRequired = false;
            $scope.isAdvanchSearch = false;
            $scope.isRequiredCharges = true;
            $scope.isAdded = false;
            $scope.isSaved = false;
            $scope.isDisableCustomer = false;
            $scope.addedProductLists = [];
            $scope.receiveDate = $filter('date')(new Date(), "M/d/yyyy");

            //loadExchangeRate
            $scope.loadExchangeRate = function () {
                $scope.selectedCurrency = $scope.currencyId;
                GetCurrencyRate();
            };

            //change Delivery Product Option
            $scope.changeRepRecProductOption = function () {
                $scope.selectedNewProductId = 0;
                $scope.newProductName = "";
                $scope.newProductNameJsonData = null;

                $scope.productNewSerial = "";
                $scope.searchProductNewSerial = "";
                $scope.productNewSerialJsonData = null;
            };

            $scope.calculateProductTotal = function () {
                $scope.productTotal = 0;
                $.each($scope.addedProductLists, function () {
                    if (this.AdjustmentType == "A") {
                        $scope.productTotal += parseFloat(this.adjustedAmount);
                    }
                    else if (this.AdjustmentType == "D") {
                        $scope.productTotal -= parseFloat(this.adjustedAmount);
                    }
                });
                $scope.grandTotal = $scope.productTotal + $scope.totalServiceChargeAmount;;
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
                if ($scope.isAdjustmentRequired) {
                    $.each($scope.adjustmentTypeDropDownJsonData, function () {
                        if (this.IsSelected) {
                            $scope.adjustmentTypeId = this.Value;
                            $scope.adjustmentTypeName = this.Item;
                            return;
                        }
                    });
                }
                else {
                    $scope.adjustmentTypeId = null;
                    $scope.adjustmentTypeName = "";
                }
            };

            //is Required Charges
            $scope.clickForRequiredCharges = function () {
                if ($scope.isRequiredCharges)
                    $scope.isRequiredCharges = false;
                else $scope.isRequiredCharges = true;
            };

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
                $scope.calculateProductTotal();
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

            //get Complain ReceiveNo
            $scope.getReplacementClaimNo = function () {
                var q = ($scope.replacementClaimNo === undefined || $scope.replacementClaimNo == "*") ? '' : $scope.replacementClaimNo;

                if (q.length >= 3 || $scope.replacementClaimNo == "*") {
                    replacementReceiveService.FetchAllReplacementClaimNo(
                        q, (selectedSupplierId = $scope.selectedSupplierId === undefined ? 0 : $scope.selectedSupplierId), $scope.dateFrom, $scope.dateTo
                    ).then(function (value) {
                        $scope.replacementClaimNoJsonData = value.data;
                    }, function (reason) {
                        console.log(reason);
                    });
                } else {
                    $scope.selectedComplainReceiveId = undefined;
                    $scope.replacementClaimNoJsonData = null;
                }
            };

            $scope.fillReplacementClaimNoTextbox = function (obj) {
                $scope.selectedReplacementClaimId = obj.Value;
                $scope.replacementClaimNo = obj.Item;
                $scope.replacementClaimNoJsonData = null;
                GetAllCharge();
                replacementReceiveService.FetchAllReplacementClaimShortInfoById(
                    $scope.selectedReplacementClaimId
                ).then(function (value) {
                    $scope.selectedSupplierId = value.data.SupplierId;
                    $scope.supplierName = value.data.SupplierName;
                    $scope.supplierContactNo = value.data.SupplierPhoneNo;
                    $scope.supplierAddress = value.data.SupplierAddress;
                }, function (reason) {
                    console.log(reason);
                });
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
                    $scope.supplierAddress = "";
                    $scope.supplierContactNo = "";
                    $scope.supplierNameJsonData = null;
                }
            };

            $scope.fillSupplierTextbox = function (obj) {
                $scope.supplierName = '';
                $scope.selectedSupplierId = Number(obj.Value);
                $scope.supplierName = obj.Item;
                supplierCommonService.FetchSupplierShortInfo(
                    $scope.selectedSupplierId, $scope.selectedCurrency
                ).then(function (value) {
                    $scope.supplierAddress = value.data.Address;
                    $scope.supplierContactNo = value.data.ContactNo;
                }, function (reason) {
                    console.log(reason);
                });
                $scope.supplierNameJsonData = null;
            };

            //product search
            $scope.getProductName = function () {
                var q = ($scope.productName === undefined || $scope.productName == "*") ? '' : $scope.productName;

                if (q.length >= 2 || ($scope.productName == "*")) {
                    replacementReceiveService.FetchProductNameByReplacementClaim(
                        q, ($scope.selectedReplacementClaimId === undefined || $scope.selectedReplacementClaimId == 0 ? '' : $scope.selectedReplacementClaimId)
                    ).then(function (value) {
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
                GetProductWiseDimension();
                GetProductWiseUnitType();
                if ($scope.repRecProductOptionValue == '1' || $scope.repRecProductOptionValue == '2' || $scope.repRecProductOptionValue == '4') {
                    $scope.fillNewProductTextbox(obj);
                }
                $scope.productNameJsonData = null;
            };

            //product serial search
            $scope.getProductSerial = function () {
                var q = ($scope.searchProductSerial === undefined || $scope.searchProductSerial == "*") ? '' : $scope.searchProductSerial;

                if (q.length >= 2 || $scope.searchProductSerial == "*") {
                    replacementReceiveService.FetchProductBySerialFromReplacementClaim(
                        q, $scope.selectedProductId, ($scope.selectedReplacementClaimId === undefined || $scope.selectedReplacementClaimId == 0 ? '' : $scope.selectedReplacementClaimId)
                    ).then(function (value) {
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

            $scope.fillProductSerialTextbox = function (obj) {
                $scope.productSerial = obj.Item;
                $scope.searchProductSerial = obj.Item;
                $scope.selectedProductId = obj.ProductId;
                $scope.productName = obj.ProductName;
                $scope.unitCost = obj.Cost;
                GetProductWiseDimension();
                GetProductWiseUnitType();
                GetReplacementClaimInfoByProduct();
                //1 = Same Product Same Serial, 2 = Same Product Different Serial, 3 = Different Product Different Serial, 4 = Cash Back
                if ($scope.repRecProductOptionValue == '1' || $scope.repRecProductOptionValue == '4') {
                    $scope.fillProductNewSerialTextbox(obj);
                }
                else if ($scope.repRecProductOptionValue == '2') {
                    $scope.fillProductNewSerialTextbox(obj);
                    $scope.productNewSerial = "";
                    $scope.searchProductNewSerial = "";
                }
                else if ($scope.repRecProductOptionValue == '3') {
                    $scope.fillProductNewSerialTextbox(obj);
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

            $scope.fillNewProductTextbox = function (obj) {
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
                    productCommonService.FetchProductByRMASerial(
                        q, $scope.selectedNewProductId === undefined ? 0 : $scope.selectedNewProductId
                    ).then(function (value) {
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

            $scope.fillProductNewSerialTextbox = function (obj) {
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

            $scope.getNewUnitTypeName = function () {
                $.each($scope.newProductIdUnitTypeDropDownJsonData, function () {
                    if (this.Value === $scope.newProductIdUnitTypeId) {
                        $scope.newProductUnitTypeName = this.Item;
                    }
                });
            };

            //add parent product
            $scope.clickToAdd = function () {
                if ($scope.supplierName == ""
                    || $scope.supplierName == undefined
                    || $scope.searchProductSerial == ""
                    || $scope.searchProductSerial == undefined
                    || $scope.productNewSerial == ""
                    || $scope.productNewSerial == undefined
                ) {
                    // alert
                    $ngConfirm({
                        title: 'Required',
                        icon: 'glyphicon glyphicon-exclamation-sign',
                        theme: 'supervan',
                        content: 'Supplier Name or Product serial/new serial is empty!!!.',
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
                        "ReplacementClaimId": $scope.selectedReplacementClaimId,
                        "PreviousProductId": $scope.selectedProductId.toString(),
                        "PreviousProductDimensionId": $scope.productDimensionDropDownJsonData.length > 0 ? $scope.dimensionId.toString() : '0',
                        "Name": $scope.productName,
                        "Dimension": $scope.dimensionName,
                        "PreviousUnitTypeId": $scope.unitTypeId.toString(),
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
                        "AdjustmentTypeName": $scope.adjustmentTypeName,
                        "AdjustedAmount": $scope.adjustmentAmount,
                        "DeliveryType": $scope.repRecProductOptionValue,
                        "Problem": getAllSelectedProblem(),
                        "TotalAmount": 0
                    });

                    $scope.productIdForSparePart = $scope.selectedNewProductId.toString();
                    $scope.productNameForSparePart = $scope.newProductName;
                    $scope.productSerialForSparePart = $scope.productNewSerial;

                    $scope.isDisableCustomer = true;
                }
                else {
                    $.each($scope.addedProductLists, function (index) {
                        if (this.identity === previousIentity) {
                            this.ReplacementClaimId = $scope.selectedReplacementClaimId;
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
                            this.AdjustmentTypeName = $scope.adjustmentTypeName;
                            this.AdjustedAmount = $scope.adjustmentAmount;
                            this.DeliveryType = $scope.repRecProductOptionValue;
                            this.Problem = getAllSelectedProblem();
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
                $scope.allProblemJsonLists = "";
                $scope.calculateProductTotal();
            };

            $scope.clickRemoveItem = function (identity) {
                $.each($scope.addedProductLists, function (index) {
                    if (this.identity === identity) {
                        $scope.addedProductLists.splice(index, 1);
                        return false;
                    }
                });
                $scope.calculateProductTotal();
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

                if ($scope.receiveNo != undefined) {
                    // alert
                    $ngConfirm({
                        title: 'Warning',
                        icon: 'glyphicon glyphicon-exclamation-sign',
                        theme: 'supervan',
                        content: 'This replacement receive already saved.',
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
                $scope.replacementReceive_Charge = $scope.allSelectedCharges;

                $scope.isSaved = true;
                replacementReceiveService.SaveReplacementReceive(
                    $scope.receiveDate, $scope.selectedEmployeeId, $scope.remarks, $scope.selectedCurrency, $scope.addedProductLists, $scope.replacementReceive_Charge, $scope.totalChargeAmount, $scope.discount
                ).then(function (value) {
                    if (Number(value.status) === 200 && value.data.IsSuccess === true) {
                        $scope.receiveNo = value.data.Message;
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
                        GetAllReplacementReceiveGridListsData();
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
                var url = applicationBasePath + "/Inventory360Reports/ReplacementReceive?id=" + id
                    + "&user=" + $scope.UserName
                    + "&currency=" + $scope.currencyId
                    + "&token=" + userService.GetCurrentUser().access_token;
                var win = window.open(url, '_blank');
                win.focus();
            };

            $scope.previous = function (e) {
                e.preventDefault();
                if ($scope.pageIndex > 1) {
                    $scope.pageIndex--;
                    GetAllReplacementReceiveGridListsData();
                }
            };

            $scope.next = function (e) {
                e.preventDefault();
                if ($scope.pageIndex < lastPageNo) {
                    $scope.pageIndex++;
                    GetAllReplacementReceiveGridListsData();
                }
            };

            $scope.filterReplacementReceive = function () {
                if ($scope.searchReplacementReceive !== undefined) {
                    $scope.pageIndex = 1;
                    GetAllReplacementReceiveGridListsData();
                }
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

            $scope.getNewPoductDimensionName = function () {
                $.each($scope.newPoductDimensionDropDownJsonData, function () {
                    if (this.Value === $scope.newPoductDimensionId) {
                        $scope.newDimensionName = this.Item;
                    }
                });
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

            // load all ReplacementReceive lists
            var GetAllReplacementReceiveGridListsData = function () {
                var q = $scope.searchReplacementReceive === undefined ? '' : $scope.searchReplacementReceive;

                replacementReceiveService.FetchReplacementReceiveLists(
                    q, $scope.pageIndex, dataTableRows
                ).then(function (value) {
                    $scope.allReplacementReceiveJsonLists = value.data;
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

            //get invoice info
            var GetReplacementClaimInfoByProduct = function () {
                replacementReceiveService.FetchReplacementClaimInfoByProduct(
                    $scope.selectedProductId, $scope.productSerial, ($scope.selectedReplacementClaimId === undefined || $scope.selectedReplacementClaimId == 0 ? '' : $scope.selectedReplacementClaimId)
                ).then(function (value) {
                    $scope.selectedSupplierId = value.data.SupplierId;
                    $scope.supplierName = value.data.SupplierName;
                    $scope.supplierContactNo = value.data.SupplierPhoneNo;
                    $scope.supplierAddress = value.data.SupplierAddress;
                    $scope.selectedReplacementClaimId = value.data.ClaimId;
                    $scope.ClaimDetailId = value.data.ClaimDetailId;
                    $scope.replacementClaimNo = value.data.ClaimNo;
                    GetAllProblem();
                    if ($scope.selectedReplacementClaimId != undefined) {
                        GetAllCharge();
                    }
                }, function (reason) {
                    console.log(reason);
                });
            }

            //get all problem
            var GetAllProblem = function () {
                replacementReceiveService.FetchAllReplacementClaimDetail_Problem(
                    $scope.ClaimDetailId == undefined ? "" : $scope.ClaimDetailId
                ).then(function (value) {
                    $scope.allProblemJsonLists = value.data;
                }, function (reason) {
                    console.log(reason);
                });
            }

            //get all charge
            var GetAllCharge = function () {
                chargeSetupService.FetchRMAWiseAllCharge().then(function (value) {
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
                        $scope.getNewPoductDimensionName();
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
                            $scope.getNewUnitTypeName();
                        }
                    });
                }, function (reason) {
                    console.log(reason);
                });
            };

            //get New product cost
            var GetNewProductCost = function () {
                productCommonService.FetchProductCost(
                    $scope.selectedNewProductId, '0', $scope.newProductIdUnitTypeId, $scope.selectedLocationId, $scope.wareHouseId
                ).then(function (value) {
                    if (value.data)
                        $scope.newProductunitCost = value.data.Cost;
                }, function (reason) {
                    console.log(reason);
                });
            };

            var GetReplacementReceiveProductOption = function () {
                replacementReceiveService.GetReplacementReceiveProductOption().then(function (value) {
                    $scope.replacementReceiveProductOptionDropDownJsonData = value.data;
                    $.each($scope.replacementReceiveProductOptionDropDownJsonData, function () {
                        if (this.IsSelected) {
                            $scope.repRecProductOptionValue = this.Value;
                            return;
                        }
                    });
                }, function (reason) {
                    console.log(reason);
                });
            }

            // load Adjustment Nature
            var GetAdjustmentNature = function () {
                partyAdjustmentNatureCommonService.FetchPartyAdjustmentNature().then(function (value) {
                    $scope.adjustmentTypeDropDownJsonData = value.data;
                }, function (reason) {
                    console.log(reason);
                });
            }

            // load currency
            var GetCurrencyForDropdown = function () {
                currencyCommonService.FetchCurrency().then(function (value) {
                    $scope.currencyDropDownJsonData = value.data;
                    $scope.currencyId = $scope.DefaultCurrency;
                    $scope.listCurrencyId = $scope.DefaultCurrency;
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
            GetAllCharge();
            GetReplacementReceiveProductOption();
            GetAdjustmentNature();
            GetAllReplacementReceiveGridListsData();
        }
    ])
    .config(function ($locationProvider) {
        $locationProvider.html5Mode(true);
    });